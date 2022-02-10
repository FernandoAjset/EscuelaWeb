using EscuelaWeb.Models;
using EscuelaWeb.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EscuelaWeb.Controllers
{
    public class CursoController : Controller
    {
        private readonly EscuelaContext context;

        public CursoController(EscuelaContext _context)
        {
            context = _context;
        }

        //[Route("Curso/{Id?}")]
        //[Route("Curso/Index/{Id?}")]
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
        public IActionResult MultiCurso()
        {
            var listaCursos = context.Cursos;
            ViewBag.Fecha = DateTime.Now.ToString();
            return View(listaCursos);
        }
        public IActionResult Create()
        {
            ViewBag.Fecha = DateTime.Now.ToString();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Curso curso)
        {
            ViewBag.Fecha = DateTime.Now.ToString();
            if (ModelState.IsValid)
            {
                var escuela = context.Escuelas.FirstOrDefault();
                curso.EscuelaId = escuela.Id;
                context.Cursos.Add(curso);
                context.SaveChanges();
                ViewBag.MensajeExito = $"{curso.GetType().Name} creado exitosamente";
                //return View("Index",curso);
                return View();
            }
            else
            {
                return View(curso);
            }
        }
    }
}
