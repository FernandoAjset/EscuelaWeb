using EscuelaWeb.Models;
using EscuelaWeb.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace EscuelaWeb.Controllers
{
    public class AlumnoController : Controller
    {
        private readonly EscuelaContext context;
        public AlumnoController(EscuelaContext _context)
        {
            context = _context;
        }
        public IActionResult Index(string Id)
        {

            if (!string.IsNullOrEmpty(Id))
            {
                var alumnos = from alumno in context.Alumnos
                              where alumno.Id == Id
                              select alumno;
                if (alumnos.SingleOrDefault() != null)
                    return View(alumnos.SingleOrDefault());
                else
                    return View("MultiAlumno", context.Alumnos);
            }
            else
            {
                return View("MultiAlumno", context.Alumnos);
            }
        }

        public IActionResult MultiAlumno(int pagina = 1)
        {
            var cantidadRegistrosPorPagina = 10;
            var listaAlumnos = context.Alumnos
                .Include(a => a.Carrera)
                //make sure to order items before paging
                .OrderBy(x => x.Nombre)

                //skip items before current page
                .Skip((pagina - 1) * cantidadRegistrosPorPagina)

                //take only 10 (page size) items
                .Take(cantidadRegistrosPorPagina)

                //call ToList() at the end to execute the query and return the result set
                .ToList();

            //var escuelaContext = context.Asignaturas.Include(a => a.Carrera).OrderBy(a=>a.CarreraNombre);
            var totalDeRegistros = context.Alumnos.Count();
            var modelo = new ListaViewModel();
            modelo.listado = listaAlumnos;
            modelo.PaginaActual = pagina;
            modelo.TotalDeRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;

            return View(modelo);
        }

        public IActionResult Crear()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(Alumno alumno)
        {

            if (!ModelState.IsValid)
            {
                return View(alumno);
            }
            context.Alumnos.Add(alumno);
            context.SaveChanges();
            ViewBag.MensajeExito = "Alumno creado";
            return View("Index", alumno);
        }
        [HttpGet]
        public IActionResult Editar(string id)
        {

            var existe = from alumno in context.Alumnos
                         where alumno.Id == id
                         select alumno;

            if (existe.Any())
            {
                ViewData["CarreraId"] = new SelectList(context.Carreras.OrderBy(c => c.Nombre), "Id", "Nombre", existe.FirstOrDefault().CarreraId);
                return View("Editar", existe.FirstOrDefault());
            }
            return View("NoEncontrado");
        }
        [HttpPost]
        public IActionResult Editar(Alumno alumnoEdit)
        {
            if (!ModelState.IsValid)
                return View(alumnoEdit);
            var existe = from alumno in context.Alumnos
                         where alumno.Nombre.ToLower() == alumnoEdit.Nombre.ToLower()
                         && alumno.CarreraId == alumnoEdit.CarreraId
                         select alumno;

            var carrera = context.Carreras.Where(c => c.Id == alumnoEdit.CarreraId).FirstOrDefault();
            if (existe.Any())
            {
                ViewData["CarreraId"] = new SelectList(context.Carreras.OrderBy(c => c.Nombre), "Id", "Nombre", existe.FirstOrDefault().CarreraId);
                ModelState.AddModelError(nameof(alumnoEdit.Nombre),
                    $"Ya existe un alumno con nombre {alumnoEdit.Nombre} y carrera {carrera.Nombre}");
                return View(alumnoEdit);
            }
            if (existe is null)
                return View("NoEncontrado");
            context.Alumnos.Update(alumnoEdit);
            context.SaveChanges();
            ViewBag.MensajeExito = "Alumno actualizado";
            return View("Index", alumnoEdit);
        }
        [HttpGet]
        public IActionResult Borrar(string id)
        {
            var alumno = from alum in context.Alumnos
                         where id == alum.Id
                         select alum;
            if (!alumno.Any())
            {
                return View("NoEncontrado");
            }
            ViewBag.MensajeBorrado = "¿Está seguro que desea eliminar este alumno? NOTA: Al eliminar el alumno también se" +
                " eliminarán sus evaluaciones";
            return View("Borrar", alumno.FirstOrDefault());

        }
        [HttpPost]
        public async Task<IActionResult> BorrarAlumno(string id)
        {
            var alumnoExiste = await context.Alumnos.FirstOrDefaultAsync(a => a.Id == id);
            if (alumnoExiste is null)
                return View("NoEncontrado");

            var evaluaciones = await context.Evalucaiones.Where(a => a.AlumnoId == id).ToListAsync();
            context.Evalucaiones.RemoveRange(evaluaciones);
            context.Alumnos.Remove(alumnoExiste);
            context.SaveChanges();
            ViewBag.MensajeExito = "Se eliminó el alumno";
            return View("Index", alumnoExiste);
        }
        [HttpGet]
        public IActionResult NombreValidoOExiste(string nombre, string CarreraId)
        {
            if (string.IsNullOrEmpty(nombre))
                return View();
            string pattern = @"^([A-Z]{1}[a-z]+[\s]?){2,}$";
            Regex rg = new Regex(pattern);
            bool valido = Regex.IsMatch(nombre, pattern);
            if (!valido)
                return Json($"El nombre no es valido, un nombre valido debe cumplir: \n" +
                    $"1.Tener al menos un nombre y un apellido, \n" +
                    $"2.El Nombre y apellido deben empezar con Mayuscula, \n" +
                    $"3.No tener espacios despues del apellido, \n" +
                    $"4.No puede incluir números ni simbolos");

            var existe = from al in context.Alumnos
                         where al.Nombre.ToLower() == nombre.ToLower()
                         && al.CarreraId == CarreraId
                         || al.Nombre.ToLower() == nombre.ToLower()
                         select al;
            if (existe.Any())
            {
                return Json($"Ya existe el alumno con nombre {nombre}");
            }
            return Json("true");
        }
    }
}
