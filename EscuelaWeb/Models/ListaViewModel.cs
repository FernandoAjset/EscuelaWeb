using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EscuelaWeb.Models
{
    public class ListaViewModel : BaseModelo
    {
        public IEnumerable<ObjetoEscuelaBase> listado { get; set; }
    }
}
