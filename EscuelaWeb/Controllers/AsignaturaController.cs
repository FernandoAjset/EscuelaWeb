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
    public class AsignaturaController : Controller
    {
        private readonly EscuelaContext _context;

        public AsignaturaController(EscuelaContext context)
        {
            _context = context;
        }

        // GET: Asignatura
        public async Task<IActionResult> Index()
        {
            //var escuelaContext = _context.Asignaturas.Include(a => a.Curso).OrderBy(a=>a.CursoNombre);
            var escuelaContext = from asig in _context.Asignaturas
                                 join cur in _context.Cursos
                                 on asig.CursoId equals cur.Id
                                 select new Asignatura
                                 {
                                     CursoNombre = cur.Nombre,
                                     CursoId = cur.Id,
                                     Nombre = asig.Nombre,
                                     Id=asig.Id
                                 };
            return View(await escuelaContext.OrderBy(a=>a.CursoNombre).ToListAsync());
        }

        // GET: Asignatura/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asignatura = await _context.Asignaturas
                .Include(a => a.Curso)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asignatura == null)
            {
                return NotFound();
            }

            return View(asignatura);
        }

        // GET: Asignatura/Create
        public IActionResult Create()
        {
            ViewData["CursoId"] = new SelectList(_context.Cursos.OrderBy(c => c.Nombre), "Id", "Nombre");
            return View();
        }

        // POST: Asignatura/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CursoId,Id,Nombre")] Asignatura asignatura)
        {
            if (ModelState.IsValid)
            {
                var curso = from cur in _context.Cursos
                            where cur.Id == asignatura.CursoId
                            select cur;
                asignatura.CursoNombre = curso.FirstOrDefault().Nombre;
                _context.Add(asignatura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Id", asignatura.CursoId);
            return View(asignatura);
        }

        // GET: Asignatura/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asignatura = await _context.Asignaturas.FindAsync(id);
            if (asignatura == null)
            {
                return NotFound();
            }
            ViewData["CursoId"] = new SelectList(_context.Cursos.OrderBy(c=>c.Nombre), "Id", "Nombre", asignatura.CursoId);
            return View(asignatura);
        }

        // POST: Asignatura/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CursoId,CursoNombre,Id,Nombre")] Asignatura asignatura)
        {
            if (id != asignatura.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asignatura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsignaturaExists(asignatura.Id))
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
            ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Id", asignatura.CursoId);
            return View(asignatura);
        }

        // GET: Asignatura/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asignatura = await _context.Asignaturas
                .Include(a => a.Curso)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asignatura == null)
            {
                return NotFound();
            }

            return View(asignatura);
        }

        // POST: Asignatura/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var asignatura = await _context.Asignaturas.FindAsync(id);
            _context.Asignaturas.Remove(asignatura);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AsignaturaExists(string id)
        {
            return _context.Asignaturas.Any(e => e.Id == id);
        }
    }
}
