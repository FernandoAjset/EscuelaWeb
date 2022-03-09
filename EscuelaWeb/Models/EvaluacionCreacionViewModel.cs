using Microsoft.AspNetCore.Mvc.Rendering;

namespace EscuelaWeb.Models
{
    public class EvaluacionCreacionViewModel: Evaluacion
    {
        public IEnumerable<SelectListItem> Alumnos { get; set; }
        public IEnumerable<SelectListItem> Asignaturas { get; set; }
    }
}
