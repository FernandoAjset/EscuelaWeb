using System;

namespace EscuelaWeb.Models
{
    public class Evaluacion: ObjetoEscuelaBase
    {
        public string AlumnoId { get; set; }
        public Alumno Alumno { get; set; }
        public string AsignaturaId  { get; set; }
        public Asignatura Asignatura { get; set; }
        public float Nota { get; set; }

        public override string ToString()
        {
            return $"Nota: {Nota}, Nombre Alumno: {Alumno.Nombre}, Nombre Asignatura: {Asignatura.Nombre}";
        }

    }
}