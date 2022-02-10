using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscuelaWeb.Models
{
    public abstract class ObjetoEscuelaBase
    {
        public string Id { get; set; }
        public virtual string Nombre { get; set; }

        public ObjetoEscuelaBase()
        {
            Id=Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return $"Nombre: {Nombre}, ID: {Id}";
        }

    }
}
