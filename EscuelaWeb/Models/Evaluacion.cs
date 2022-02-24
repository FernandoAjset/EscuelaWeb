using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace EscuelaWeb.Models
{
    public class Evaluacion: ObjetoEscuelaBase
    {
        [Required]
        public override string Nombre { get; set; }
        public string AlumnoId { get; set; }
        public Alumno Alumno { get; set; }
        public string AsignaturaId  { get; set; }
        public Asignatura Asignatura { get; set; }
        [Required]
        public float Nota { get; set; }

        public override string ToString()
        {
            return $"Nota: {Nota}, Nombre Alumno: {Alumno.Nombre}, Nombre Asignatura: {Asignatura.Nombre}";
        }

    }
}