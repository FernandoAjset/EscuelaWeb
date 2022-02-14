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
            var listaAlumnos = context.Alumnos.ToList();

            var pagina = 1;
            var cantidadRegistrosPorPagina = 10;
            var lista = context.Alumnos
                //make sure to order items before paging
                .OrderBy(x => x.Nombre)
                //skip items before current pagek
                //skip items before current pagek
                .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                //take only 10 (page size) items
                .Take(cantidadRegistrosPorPagina)
                //call ToList() at the end to execute the query and return the result set
                .ToList();
            var totalDeRegistros = context.Alumnos.Count();
            var modelo = new ListaViewModel();
            modelo.listado = lista;
            modelo.PaginaActual = pagina;
            modelo.TotalDeRegistros = totalDeRegistros;

            if (!string.IsNullOrEmpty(Id))
            {
                var alumnos = from alum in context.Alumnos
                              where alum.Id == Id
                              select alum;
                if (alumnos.SingleOrDefault() != null)
                    return View(alumnos.SingleOrDefault());
                else
                {
                    return View("MultiAlumno",modelo);
                }
            }
            else
            {
                return View("MultiAlumno", modelo);
            }
        }
        public IActionResult MultiAlumno(int pagina = 1)
        {
            var cantidadRegistrosPorPagina = 10;
            var listaAlumnos = context.Alumnos
                //make sure to order items before paging
                .OrderBy(x => x.Nombre)

                //skip items before current page
                .Skip((pagina - 1) * cantidadRegistrosPorPagina)

                //take only 10 (page size) items
                .Take(cantidadRegistrosPorPagina)

                //call ToList() at the end to execute the query and return the result set
                .ToList();
            var totalDeRegistros = context.Alumnos.Count();
            var modelo = new ListaViewModel();
            modelo.listado = listaAlumnos;
            modelo.PaginaActual = pagina;
            modelo.TotalDeRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            ViewBag.Fecha = DateTime.Now.ToString();
            return View(modelo);
        }
    }
}
