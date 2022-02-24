using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace EscuelaWeb.Models
{

    public class Asignatura: ObjetoEscuelaBase
    {
        [Required(ErrorMessage ="Nombre es requerido")]
        public override string Nombre { get; set; }
        public string CarreraId { get; set; }
        public string CarreraNombre { get; set; }
        public Carrera Carrera { get; set; }
        public List<Evaluacion> Evaluaciones { get; set; }
    }
}