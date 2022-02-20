using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EscuelaWeb.Models
{
    public class Alumno: ObjetoEscuelaBase
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Remote(action: "NombreValidoOExiste", controller:"Alumno")]
        public override string Nombre { get; set; }
        public string CursoId { get; set; }
        public Curso Curso { get; set; }
        public List<Evaluacion> Evaluaciones { get; set; }
    }
}