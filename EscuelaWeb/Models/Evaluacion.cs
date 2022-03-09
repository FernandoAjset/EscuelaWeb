using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace EscuelaWeb.Models
{
    public class Evaluacion : ObjetoEscuelaBase
    {
        public override string? Nombre {get;set;}
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string AlumnoId { get; set; }
        public Alumno Alumno { get; set; }
        [Required(ErrorMessage = "El campo asignatura es requerido")]
        public string AsignaturaId { get; set; }
        public Asignatura Asignatura { get; set; }
        [Range(minimum: 0, maximum: 50,ErrorMessage ="El valor debe estar entre {1} y {2}")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public float Nota { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public TipoEvaluacion TipoEvaluacion { get; set; }
        public override string ToString()
        {
            return $"Nota: {Nota}, Nombre Alumno: {Alumno.Nombre}, Nombre Asignatura: {Asignatura.Nombre}";
        }

    }
}