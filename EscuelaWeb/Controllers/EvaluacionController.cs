using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EscuelaWeb.Models;

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
            var escuelaContext = _context.Evalucaiones.Include(e => e.Alumno).Include(e => e.Asignatura);
            return View(await escuelaContext.ToListAsync());
        }

        // GET: Evaluacion/Details/5
        public async Task<IActionResult> Details(string id)
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

        // GET: Evaluacion/Create
        public IActionResult Create(string AlumnoId)
        {
            ViewData["AlumnoId"] = new SelectList(_context.Alumnos.OrderBy(a=>a.Nombre), "Id", "Nombre");
            var alumno=from al in _context.Alumnos
                       where al.Id == AlumnoId
                       select al;

            //var asignaturas=from asig in _context.Asignaturas
            //                where asig.CursoId == alumno.FirstOrDefault().CursoId
            //                select asig;
            //ViewData["AsignaturaId"] = new SelectList(asignaturas.ToList(), "Id", "Nombre");
            ViewData["AsignaturaId"] = new SelectList(_context.Asignaturas, "Id", "Nombre");

            return View();
        }

        // POST: Evaluacion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlumnoId,AsignaturaId,Nota,Id,Nombre")] Evaluacion evaluacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evaluacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlumnoId"] = new SelectList(_context.Alumnos, "Id", "Id", evaluacion.AlumnoId);
            ViewData["AsignaturaId"] = new SelectList(_context.Asignaturas, "Id", "Id", evaluacion.AsignaturaId);
            return View(evaluacion);
        }

        // GET: Evaluacion/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluacion = await _context.Evalucaiones.FindAsync(id);
            if (evaluacion == null)
            {
                return NotFound();
            }
            ViewData["AlumnoId"] = new SelectList(_context.Alumnos, "Id", "Id", evaluacion.AlumnoId);
            ViewData["AsignaturaId"] = new SelectList(_context.Asignaturas, "Id", "Id", evaluacion.AsignaturaId);
            return View(evaluacion);
        }

        // POST: Evaluacion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AlumnoId,AsignaturaId,Nota,Id,Nombre")] Evaluacion evaluacion)
        {
            if (id != evaluacion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evaluacion);
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
            ViewData["AlumnoId"] = new SelectList(_context.Alumnos, "Id", "Id", evaluacion.AlumnoId);
            ViewData["AsignaturaId"] = new SelectList(_context.Asignaturas, "Id", "Id", evaluacion.AsignaturaId);
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
    }
}
