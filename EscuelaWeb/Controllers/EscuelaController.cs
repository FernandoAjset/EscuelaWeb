using EscuelaWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EscuelaWeb.Controllers
{

    public class EscuelaController : Controller
    {
        private EscuelaContext _context;

        public EscuelaController(EscuelaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            var escuela = _context.Escuelas.FirstOrDefault();
            if (escuela != null)
            {
                return View(escuela);
            }
            return View();
        }
    }
}
