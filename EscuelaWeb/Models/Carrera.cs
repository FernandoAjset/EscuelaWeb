using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EscuelaWeb.Models
{
    public class Carrera : ObjetoEscuelaBase
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "La longitud del campo " +
    "{0} debe estar entre {2} y {1}")]
        public override string Nombre { get; set; }
        [Required(ErrorMessage ="Seleccione una jornada")]
        public TiposJornada Jornada { get; set; }
        public List<Asignatura> Asignaturas { get; set; }
        public List<Alumno> Alumnos { get; set; }
        public string EscuelaId { get; set; }
        public Escuela Escuela { get; set; }
    }
}