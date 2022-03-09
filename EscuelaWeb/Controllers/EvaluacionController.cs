using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EscuelaWeb.Models;
using EscuelaWeb.Servicios;

namespace EscuelaWeb.Controllers
{
    public class EvaluacionController : Controller
    {
        private readonly EscuelaContext _context;

        public EvaluacionController(EscuelaContext context)
        {
            _context = context;
        }

        // GET: Evaluacion
        public async Task<IActionResult> Index()
        {
            var escuelaContext = _context.Evalucaiones.Include(e => e.Alumno).Include(e => e.Asignatura)
                                .OrderBy(e => e.Alumno.Nombre).ThenBy(e => e.Asignatura.Nombre).ThenBy(e => e.TipoEvaluacion);
            return View(await escuelaContext.ToListAsync());
        }
        // GET: Evaluacion/Create
        public async Task<IActionResult> Create()
        {
            EvaluacionCreacionViewModel evaluacion = new EvaluacionCreacionViewModel();
            var alumno = _context.Alumnos.OrderBy(a => a.Nombre).FirstOrDefault();
            evaluacion.Alumnos = _context.Alumnos.OrderBy(a => a.Nombre).Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
            evaluacion.Asignaturas = await Asignaturas(alumno.Id);
            evaluacion.AlumnoId = alumno.Id;

            return View(evaluacion);
        }

        // POST: Evaluacion/Create
        [HttpPost]
        public async Task<IActionResult> Create(EvaluacionCreacionViewModel evaluacion)
        {
            evaluacion.Alumnos = _context.Alumnos.OrderBy(a => a.Nombre).Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
            evaluacion.Asignaturas = await Asignaturas(evaluacion.AlumnoId);
            if (ModelState.IsValid)
            {
                var existe = await _context.Evalucaiones.AnyAsync(e => e.AlumnoId == evaluacion.AlumnoId
                                                  && e.AsignaturaId == evaluacion.AsignaturaId
                                                  && e.TipoEvaluacion == evaluacion.TipoEvaluacion
                                                  );
                if (existe)
                {
                    ModelState.AddModelError(nameof(evaluacion.AlumnoId),
                    $"Ya existe una evaluación para este alumno con estos datos");
                    return View(evaluacion);
                }
                if (!Evaluacion_DAO.NotaValida(evaluacion.TipoEvaluacion, evaluacion.Nota))
                {
                    ModelState.AddModelError(nameof(evaluacion.Nota),
                    $"La nota supera el máximo punteo para este tipo de evaluación.");
                    return View(evaluacion);
                }
                Evaluacion evaluacionNueva = new Evaluacion()
                {
                    Id = evaluacion.Id,
                    AlumnoId = evaluacion.AlumnoId,
                    AsignaturaId = evaluacion.AsignaturaId,
                    Nota = evaluacion.Nota,
                    TipoEvaluacion = evaluacion.TipoEvaluacion
                };

                _context.Add(evaluacionNueva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(evaluacion);
        }

        // GET: Evaluacion/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluacion = await _context.Evalucaiones.Include(e => e.Alumno).FirstOrDefaultAsync(e => e.Id == id);
            if (evaluacion == null)
            {
                return NotFound();
            }
            EvaluacionCreacionViewModel evaluacionEdit = new EvaluacionCreacionViewModel()
            {
                Id = evaluacion.Id,
                AlumnoId = evaluacion.AlumnoId,
                Alumno = evaluacion.Alumno,
                AsignaturaId = evaluacion.AsignaturaId,
                Nota = evaluacion.Nota,
                TipoEvaluacion = evaluacion.TipoEvaluacion,
                Asignaturas = await Asignaturas(evaluacion.AlumnoId)
            };
            return View(evaluacionEdit);
        }

        // POST: Evaluacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AlumnoId,AsignaturaId,Nota,TipoEvaluacion,Id")] EvaluacionCreacionViewModel evaluacion)
        {
            if (id != evaluacion.Id)
            {
                return NotFound();
            }
            evaluacion.Alumnos = _context.Alumnos.OrderBy(a => a.Nombre).Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
            evaluacion.Asignaturas = await Asignaturas(evaluacion.AlumnoId);
            evaluacion.Alumno = await _context.Alumnos.FirstOrDefaultAsync(a => a.Id == evaluacion.AlumnoId);
            evaluacion.Asignatura = _context.Asignaturas.FirstOrDefault(a => a.Id == evaluacion.AsignaturaId);
            if (ModelState.IsValid)
            {
                try
                {
                    var existe = await _context.Evalucaiones.AnyAsync(e => e.AlumnoId == evaluacion.AlumnoId
                                  && e.AsignaturaId == evaluacion.AsignaturaId
                                  && e.TipoEvaluacion == evaluacion.TipoEvaluacion
                                  && e.Id != evaluacion.Id
                                  );
                    if (existe)
                    {
                        ModelState.AddModelError(nameof(evaluacion.AlumnoId),
                        $"No puede tener dos evaluaciones para la asignatura {evaluacion.Asignatura.Nombre}" +
                        $" y tipo de evaluación {evaluacion.TipoEvaluacion}");
                        return View(evaluacion);
                    }

                    if (!Evaluacion_DAO.NotaValida(evaluacion.TipoEvaluacion, evaluacion.Nota))
                    {

                        ModelState.AddModelError(nameof(evaluacion.Nota),
                        $"La nota supera el máximo punteo para este tipo de evaluación.");
                        return View(evaluacion);
                    }
                    Evaluacion nuevaEvaluacion = new Evaluacion()
                    {
                        Id = evaluacion.Id,
                        AlumnoId = evaluacion.AlumnoId,
                        AsignaturaId = evaluacion.AsignaturaId,
                        Nota = evaluacion.Nota,
                        TipoEvaluacion = evaluacion.TipoEvaluacion,
                    };
                    _context.Update(nuevaEvaluacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvaluacionExists(evaluacion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(evaluacion);
        }

        // GET: Evaluacion/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluacion = await _context.Evalucaiones
                .Include(e => e.Alumno)
                .Include(e => e.Asignatura)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evaluacion == null)
            {
                return NotFound();
            }

            return View(evaluacion);
        }

        // POST: Evaluacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var evaluacion = await _context.Evalucaiones.FindAsync(id);
            _context.Evalucaiones.Remove(evaluacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EvaluacionExists(string id)
        {
            return _context.Evalucaiones.Any(e => e.Id == id);
        }

        private async Task<IEnumerable<SelectListItem>> Asignaturas(string alumnoId)
        {
            var alumno = _context.Alumnos.FirstOrDefault(a => a.Id == alumnoId);
            var asignaturas = _context.Asignaturas.Where(a => a.CarreraId == alumno.CarreraId);

            return asignaturas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerAsignaturas([FromBody] string AlumnoId)
        {
            var selec = await _context.Alumnos.FirstOrDefaultAsync(a => a.Id == AlumnoId);
            var asignaturas = _context.Asignaturas.OrderBy(a => a.Nombre).Where(a => a.CarreraId == selec.CarreraId);
            var listaAsignaturas = asignaturas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
            return Ok(listaAsignaturas);
        }
    }
}
