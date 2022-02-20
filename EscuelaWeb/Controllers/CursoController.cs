using EscuelaWeb.Models;
using EscuelaWeb.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EscuelaWeb.Controllers
{
    public class CursoController : Controller
    {
        private readonly EscuelaContext context;

        public CursoController(EscuelaContext _context)
        {
            context = _context;
        }
        public IActionResult Index(string Id)
        {
            ViewBag.Fecha = DateTime.Now.ToString();
            if (!string.IsNullOrEmpty(Id))
            {
                var cursos = from curso in context.Cursos
                             where curso.Id == Id
                             select curso;
                if (cursos.SingleOrDefault() != null)
                    return View(cursos.SingleOrDefault());
                else
                    return View("MultiCurso", context.Cursos);
            }
            else
            {
                return View("MultiCurso", context.Cursos);
            }
        }
        public IActionResult MultiCurso(int pagina = 1)
        {
            var cantidadRegistrosPorPagina = 10;
            var listaCursos = context.Cursos
                //make sure to order items before paging
                .OrderBy(x => x.Nombre)
                //skip items before current page
                .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                //take only 10 (page size) items
                .Take(cantidadRegistrosPorPagina)
                //call ToList() at the end to execute the query and return the result set
                .ToList();
            var totalDeRegistros = context.Cursos.Count();
            var modelo = new ListaViewModel();
            modelo.listado = listaCursos;
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
        public IActionResult Create(Curso nuevoCurso)
        {
            ViewBag.Fecha = DateTime.Now.ToString();
            if (!ModelState.IsValid)
            {
                return View(nuevoCurso);
            }
            var existe = from curso in context.Cursos
                         where curso.Nombre == nuevoCurso.Nombre
                         && curso.Jornada == nuevoCurso.Jornada
                         select curso;

            if (existe.Any())
            {
                ModelState.AddModelError(nameof(nuevoCurso.Nombre),
                    $"Ya existe un curso con nombre {nuevoCurso.Nombre}" +
                    $" y jornada {nuevoCurso.Jornada}");
                return View(nuevoCurso);
            }
            var escuela = context.Escuelas.FirstOrDefault();
            nuevoCurso.EscuelaId = escuela.Id;
            context.Cursos.Add(nuevoCurso);
            context.SaveChanges();
            ViewBag.MensajeExito = "Curso creado";
            return View("Index", nuevoCurso);
        }
        [HttpGet]
        public IActionResult Editar(string Id)
        {
            ViewBag.Fecha = DateTime.Now.ToString();
            var cursos = from curso in context.Cursos
                         where curso.Id == Id
                         select curso;
            var cursoSelec = cursos.SingleOrDefault();
            if (cursoSelec != null)
                return View(cursos.SingleOrDefault());
            else
                return View("NoEncontrado");
        }
        [HttpPost]
        public IActionResult Editar(Curso cursoEdit)
        {
            if (!ModelState.IsValid)
            {
                return View(cursoEdit);
            }
            var existe = from curso in context.Cursos
                         where curso.Nombre == cursoEdit.Nombre
                         && curso.Jornada == cursoEdit.Jornada
                         select curso;

            if (existe.Any())
            {
                ModelState.AddModelError(nameof(cursoEdit.Nombre),
                    $"Ya existe un curso con nombre {cursoEdit.Nombre}" +
                    $" y jornada {cursoEdit.Jornada}");
                return View(cursoEdit);
            }
            if (existe is null)
                return View("NoEncontrado");

            var escuela = context.Escuelas.FirstOrDefault();
            cursoEdit.EscuelaId = escuela.Id;
            context.Cursos.Update(cursoEdit);
            context.SaveChanges();
            ViewBag.MensajeExito = "Curso actualizado";
            return View("Index", cursoEdit);
        }
        [HttpGet]
        public IActionResult Borrar(string id)
        {
            var curso = from cur in context.Cursos
                        where id == cur.Id
                        select cur;
            if (!curso.Any())
            {
                return View("NoEncontrado");
            }
            var alumnos = from alum in context.Alumnos
                          where alum.CursoId == curso.FirstOrDefault().Id
                          select alum;
            int numeroAlumnos = alumnos.Count();
            if (numeroAlumnos > 0)
            {
                ViewBag.MensajeBorradoPersonalizado = $"No se puede borrar el curso porque tiene {numeroAlumnos} alumnos asignados";
                return View("Borrar", curso.FirstOrDefault());
            }
            ViewBag.MensajeBorrado = "¿Está seguro que desea eliminar este curso?";
            return View("Borrar", curso.FirstOrDefault());
        }
        [HttpPost]
        public IActionResult Borrar(Curso cursoBorrar)
        {
            var curso = from cur in context.Cursos
                        where cursoBorrar.Id == cur.Id
                        select cur;
            if (!curso.Any())
                return View("NoEncontrado");

            var cursoEliminado = new Curso();
            cursoEliminado.Id = curso.FirstOrDefault().Id;
            cursoEliminado.Nombre = curso.FirstOrDefault().Nombre;
            cursoEliminado.Jornada = curso.FirstOrDefault().Jornada;
            try
            {
                ViewBag.MensajeExito = "Se eliminó el curso";
                context.Cursos.Remove(curso.FirstOrDefault());
                context.SaveChanges();
                return View("Index", cursoEliminado);
            }
            catch (DbUpdateException)
            {
                ViewBag.MensajeErrorPersonalizado = "No se pudo actualizar la BD, revise la información del curso";
                return View("Index", cursoEliminado);
            }
        }
    }
}