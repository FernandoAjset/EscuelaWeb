using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EscuelaWeb.Controllers
{
    public class EscuelaController : Controller
    {
        // GET: EscuelaController
        public IActionResult Index()
        {
            return View();
        }
    }
}
