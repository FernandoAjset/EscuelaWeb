using EscuelaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Index(int pagina)
        {
            if (pagina == 0)
            {
                pagina = 1;
            }
            var cantidadRegistrosPorPagina = 10;
            var asignaturas = await _context.Asignaturas.Include(a => a.Carrera)
                .OrderBy(a => a.Carrera.Nombre).ThenBy(a => a.Nombre)
                //skip items before current page
                .Skip((pagina - 1) * cantidadRegistrosPorPagina)

                //take only 10 (page size) items
                .Take(cantidadRegistrosPorPagina)

                //call ToList() at the end to execute the query and return the result set
                .ToListAsync();
            var totalDeRegistros = _context.Asignaturas.Count();
            var modelo = new AsignaturasViewModel();
            modelo.listado = asignaturas;
            modelo.PaginaActual = pagina;
            modelo.TotalDeRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;

            return View(modelo);
        }

        // GET: Asignatura/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asignatura = await _context.Asignaturas
                .Include(a => a.Carrera)
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
            ViewData["CarreraId"] = new SelectList(_context.Carreras.OrderBy(c => c.Nombre), "Id", "Nombre");
            return View();
        }

        // POST: Asignatura/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarreraId,Id,Nombre")] Asignatura asignatura)
        {
            if (ModelState.IsValid)
            {
                asignatura.Id = Guid.NewGuid().ToString();
                var Carrera = from cur in _context.Carreras
                              where cur.Id == asignatura.CarreraId
                              select cur;
                asignatura.CarreraNombre = Carrera.FirstOrDefault().Nombre;
                var existe = from asig in _context.Asignaturas
                             where asig.Nombre == asignatura.Nombre
                             && asig.CarreraId == asignatura.CarreraId
                             select asig;
                if (existe.Any())
                {
                    ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre", asignatura.CarreraId);
                    ModelState.AddModelError(nameof(asignatura.Nombre),
                            $"Ya existe una asignatura con nombre {asignatura.Nombre} para la carrera {asignatura.CarreraNombre}");
                    return View(asignatura);
                }
                _context.Add(asignatura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre", asignatura.CarreraId);
            return View(asignatura);
        }

        // GET: Asignatura/Edit/5
        public async Task<IActionResult> Edit(string id, int PaginaActual)
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
            ViewData["CarreraId"] = new SelectList(_context.Carreras.OrderBy(c => c.Nombre), "Id", "Nombre", asignatura.CarreraId);
            ViewData["Pagina"] = PaginaActual;
            return View(asignatura);
        }

        // POST: Asignatura/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, int pagina, [Bind("CarreraId,CarreraNombre,Id,Nombre")] Asignatura asignatura)
        {
            if (id != asignatura.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var Carrera = from cur in _context.Carreras
                              where cur.Id == asignatura.CarreraId
                              select cur;
                asignatura.CarreraNombre = Carrera.FirstOrDefault().Nombre;
                var existe = from asig in _context.Asignaturas
                             where asig.Nombre == asignatura.Nombre
                             && asig.CarreraId == asignatura.CarreraId
                             select asig;
                if (existe.Any())
                {
                    ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre", asignatura.CarreraId);
                    ModelState.AddModelError(nameof(asignatura.Nombre),
                            $"Ya existe una asignatura con nombre {asignatura.Nombre} para la carrera {asignatura.CarreraNombre}");
                    return View(asignatura);
                }
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
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Id", asignatura.CarreraId);
            return View(asignatura);
        }

        // GET: Asignatura/Delete/5
        public async Task<IActionResult> Delete(string id, int PaginaActual)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asignatura = await _context.Asignaturas
                .Include(a => a.Carrera)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asignatura == null)
            {
                return NotFound();
            }
            ViewData["Pagina"] = PaginaActual;
            return View(asignatura);
        }

        // POST: Asignatura/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var asignaturaExiste = await _context.Asignaturas.FirstOrDefaultAsync(a => a.Id == id);
            if (asignaturaExiste is null)
                return View("NoEncontrado");

            var evaluaciones = await _context.Evalucaiones.Where(a => a.AsignaturaId == id).ToListAsync();
            _context.Evalucaiones.RemoveRange(evaluaciones);
            _context.Asignaturas.Remove(asignaturaExiste);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));




            //var asignatura = await _context.Asignaturas.FindAsync(id);
            //_context.Asignaturas.Remove(asignatura);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        private bool AsignaturaExists(string id)
        {
            return _context.Asignaturas.Any(e => e.Id == id);
        }
    }
}
