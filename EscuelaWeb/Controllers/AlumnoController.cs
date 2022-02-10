using EscuelaWeb.Models;
using EscuelaWeb.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            ViewBag.Fecha = DateTime.Now.ToString();
            if (!string.IsNullOrEmpty(Id))
            {
                var alumnos = from alum in context.Alumnos
                                 where alum.Id == Id
                                 select alum;
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

        public IActionResult MultiAlumno()
        {
            var listaAlumnos = context.Alumnos;
            ViewBag.Fecha = DateTime.Now.ToString();
            return View(listaAlumnos);
        }
    }
}
