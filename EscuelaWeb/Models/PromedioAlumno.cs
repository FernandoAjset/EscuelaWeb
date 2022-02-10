using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscuelaWeb.Models
{
    public class PromedioAlumno
    {
        public float Promedio { get; set; }
        public string AlumnoId { get; set; }
        public string AlumnoNombre { get; set; }

        public override string ToString()
        {
            return $"Nombre Alumno: {AlumnoNombre}, Promedio: {Promedio}";
        }
    }
}
