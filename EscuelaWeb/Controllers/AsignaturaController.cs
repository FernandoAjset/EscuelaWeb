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
        public IActionResult MultiAsignatura(int pagina = 1)
        {
            var cantidadRegistrosPorPagina = 10;
            var listaAsignaturas = context.Asignaturas
                //make sure to order items before paging
                .OrderBy(x => x.Curso.Nombre)

                //skip items before current page
                .Skip((pagina - 1) * cantidadRegistrosPorPagina)

                //take only 10 (page size) items
                .Take(cantidadRegistrosPorPagina)

                //call ToList() at the end to execute the query and return the result set
                .ToList();
            var totalDeRegistros = context.Asignaturas.Count();
            var modelo = new ListaViewModel();
            modelo.listado = listaAsignaturas;
            modelo.PaginaActual = pagina;
            modelo.TotalDeRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            ViewBag.Fecha = DateTime.Now.ToString();
            return View(modelo);
        }
    }
}
