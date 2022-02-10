using EscuelaWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EscuelaWeb.Controllers
{
    public class AsignaturaController : Controller
    {
        private readonly EscuelaContext context;

        public AsignaturaController(EscuelaContext _context)
        {
            context = _context;
        }

        //Especifíca como va la ruta en el navegador, "?" significa que puede o no llevar Id
        [Route("Asignatura/Index/{asignaturaId?}")] 
        public IActionResult Index(string asignaturaId)
        {
            ViewBag.Fecha = DateTime.Now.ToString();
            if (!string.IsNullOrEmpty(asignaturaId))
            {
                var asignatura = from asig in context.Asignaturas
                                 where asig.Id == asignaturaId
                                 select asig;
                if (asignatura.SingleOrDefault() != null)
                    return View(asignatura.SingleOrDefault());
                else
                    return View("Multiasignatura", context.Asignaturas);
            }
            else
            {
                return View("Multiasignatura", context.Asignaturas);
            }
        }
        public IActionResult MultiAsignatura()
        {
            var listaAsignaturas = context.Asignaturas;

            ViewBag.Fecha = DateTime.Now.ToString();
            return View(listaAsignaturas);
        }
    }
}
