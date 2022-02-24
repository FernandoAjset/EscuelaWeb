using EscuelaWeb.Models;
using EscuelaWeb.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EscuelaWeb.Controllers
{
    public class CarreraController : Controller
    {
        private readonly EscuelaContext context;

        public CarreraController(EscuelaContext _context)
        {
            context = _context;
        }
        public IActionResult Index(string Id)
        {
            ViewBag.Fecha = DateTime.Now.ToString();
            if (!string.IsNullOrEmpty(Id))
            {
                var Carreras = from Carrera in context.Carreras
                             where Carrera.Id == Id
                             select Carrera;
                if (Carreras.SingleOrDefault() != null)
                    return View(Carreras.SingleOrDefault());
                else
                    return View("MultiCarrera", context.Carreras);
            }
            else
            {
                return View("MultiCarrera", context.Carreras);
            }
        }
        public IActionResult MultiCarrera(int pagina = 1)
        {
            var cantidadRegistrosPorPagina = 10;
            var listaCarreras = context.Carreras
                //make sure to order items before paging
                .OrderBy(x => x.Nombre)
                //skip items before current page
                .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                //take only 10 (page size) items
                .Take(cantidadRegistrosPorPagina)
                //call ToList() at the end to execute the query and return the result set
                .ToList();
            var totalDeRegistros = context.Carreras.Count();
            var modelo = new ListaViewModel();
            modelo.listado = listaCarreras;
            modelo.PaginaActual = pagina;
            modelo.TotalDeRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            ViewBag.Fecha = DateTime.Now.ToString();
            return View(modelo);
        }
        public IActionResult Create()
        {
            ViewBag.Fecha = DateTime.Now.ToString();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Carrera nuevoCarrera)
        {
            ViewBag.Fecha = DateTime.Now.ToString();
            if (!ModelState.IsValid)
            {
                return View(nuevoCarrera);
            }
            var existe = from Carrera in context.Carreras
                         where Carrera.Nombre == nuevoCarrera.Nombre + "/" + nuevoCarrera.Jornada.ToString()
                         && Carrera.Jornada == nuevoCarrera.Jornada
                         select Carrera;

            if (existe.Any())
            {
                ModelState.AddModelError(nameof(nuevoCarrera.Nombre),
                    $"Ya existe un Carrera con nombre {nuevoCarrera.Nombre}" +
                    $" y jornada {nuevoCarrera.Jornada}");
                return View(nuevoCarrera);
            }
            var escuela = context.Escuelas.FirstOrDefault();
            nuevoCarrera.EscuelaId = escuela.Id;
            nuevoCarrera.Nombre = nuevoCarrera.Nombre + "/" + nuevoCarrera.Jornada.ToString();
            context.Carreras.Add(nuevoCarrera);
            context.SaveChanges();
            ViewBag.MensajeExito = "Carrera creado";
            return View("Index", nuevoCarrera);
        }
        [HttpGet]
        public IActionResult Editar(string Id)
        {
            ViewBag.Fecha = DateTime.Now.ToString();
            var Carreras = from Carrera in context.Carreras
                         where Carrera.Id == Id
                         select Carrera;
            var CarreraSelec = Carreras.SingleOrDefault();
            if (CarreraSelec != null)
            {
                List<string> list = new List<string>();
                list = CarreraSelec.Nombre.Split('/').ToList();
                CarreraSelec.Nombre = list.FirstOrDefault();

                return View(CarreraSelec);
            }
            else
                return View("NoEncontrado");
        }
        [HttpPost]
        public IActionResult Editar(Carrera CarreraEdit)
        {
            if (!ModelState.IsValid)
            {
                return View(CarreraEdit);
            }
            var existe = from Carrera in context.Carreras
                         where Carrera.Nombre == CarreraEdit.Nombre + "/" + CarreraEdit.Jornada.ToString()
                         && Carrera.Jornada == CarreraEdit.Jornada
                         select Carrera;

            if (existe.Any())
            {
                ModelState.AddModelError(nameof(CarreraEdit.Nombre),
                    $"Ya existe un Carrera con nombre {CarreraEdit.Nombre}" +
                    $" y jornada {CarreraEdit.Jornada}");
                return View(CarreraEdit);
            }
            if (existe is null)
                return View("NoEncontrado");

            var escuela = context.Escuelas.FirstOrDefault();
            CarreraEdit.EscuelaId = escuela.Id;
            CarreraEdit.Nombre +="/" + CarreraEdit.Jornada.ToString();
            context.Carreras.Update(CarreraEdit);
            context.SaveChanges();
            ViewBag.MensajeExito = "Carrera actualizado";
            return View("Index", CarreraEdit);
        }
        [HttpGet]
        public IActionResult Borrar(string id)
        {
            var Carrera = from cur in context.Carreras
                        where id == cur.Id
                        select cur;
            if (!Carrera.Any())
            {
                return View("NoEncontrado");
            }
            var alumnos = from alum in context.Alumnos
                          where alum.CarreraId == Carrera.FirstOrDefault().Id
                          select alum;
            int numeroAlumnos = alumnos.Count();
            if (numeroAlumnos > 0)
            {
                ViewBag.MensajeBorradoPersonalizado = $"No se puede borrar el Carrera porque tiene {numeroAlumnos} alumnos asignados";
                return View("Borrar", Carrera.FirstOrDefault());
            }
            ViewBag.MensajeBorrado = "¿Está seguro que desea eliminar este Carrera?";
            return View("Borrar", Carrera.FirstOrDefault());
        }
        [HttpPost]
        public IActionResult Borrar(Carrera CarreraBorrar)
        {
            var Carrera = from cur in context.Carreras
                        where CarreraBorrar.Id == cur.Id
                        select cur;
            if (!Carrera.Any())
                return View("NoEncontrado");

            var CarreraEliminado = new Carrera();
            CarreraEliminado.Id = Carrera.FirstOrDefault().Id;
            CarreraEliminado.Nombre = Carrera.FirstOrDefault().Nombre;
            CarreraEliminado.Jornada = Carrera.FirstOrDefault().Jornada;
            try
            {
                ViewBag.MensajeExito = "Se eliminó el Carrera";
                context.Carreras.Remove(Carrera.FirstOrDefault());
                context.SaveChanges();
                return View("Index", CarreraEliminado);
            }
            catch (DbUpdateException)
            {
                ViewBag.MensajeErrorPersonalizado = "No se pudo actualizar la BD, revise la información del Carrera";
                return View("Index", CarreraEliminado);
            }
        }
    }
}