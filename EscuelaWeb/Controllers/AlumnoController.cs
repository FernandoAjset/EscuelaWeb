﻿using EscuelaWeb.Models;
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

        public IActionResult Crear()
        {
            ViewBag.Fecha = DateTime.Now.ToString();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Alumno alumno)
        {
            ViewBag.Fecha = DateTime.Now.ToString();
            if (!ModelState.IsValid)
            {
                return View(alumno);
            }
            var existe = from al in context.Alumnos
                         where al.Nombre.ToLower() == alumno.Nombre.ToLower()
                         select al;
            if (existe.Any())
            {
                ModelState.AddModelError(nameof(alumno.Nombre), $"El alumno con nombre" +
                    $" {alumno.Nombre} ya existe");
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
            ViewBag.Fecha = DateTime.Now.ToString();
            var existe = from alumno in context.Alumnos
                         where alumno.Id == id
                         select alumno;
            if (existe.Any())
            {
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
                         select alumno;
            if (existe.Any())
            {
                ModelState.AddModelError(nameof(alumnoEdit.Nombre),
                    $"Ya existe un alumno con nombre {alumnoEdit.Nombre}");
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
            ViewBag.MensajeBorrado = "¿Está seguro que desea eliminar este alumno?";
            return View("Borrar", alumno.FirstOrDefault());

        }
        [HttpPost]
        public IActionResult Borrar(Curso cursoBorrar)
        {
            var alumno = from alum in context.Alumnos
                         where cursoBorrar.Id == alum.Id
                         select alum;
            if (!alumno.Any())
                return View("NoEncontrado");

            ViewBag.MensajeBorrado = "Se eliminó el alumno";
            var alumnoEliminado = new Alumno();
            alumnoEliminado.Id = alumno.FirstOrDefault().Id;
            alumnoEliminado.Nombre = alumno.FirstOrDefault().Nombre;
            context.Alumnos.Remove(alumno.FirstOrDefault());
            context.SaveChanges();
            return View("Index", alumnoEliminado);
        }
    }
}
