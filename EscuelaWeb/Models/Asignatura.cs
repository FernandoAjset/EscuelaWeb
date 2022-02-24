using System;

namespace EscuelaWeb.Models
{

    public class Asignatura: ObjetoEscuelaBase
    {
        public string CarreraId { get; set; }
        public string CarreraNombre { get; set; }
        public Carrera Carrera { get; set; }
        public List<Evaluacion> Evaluaciones { get; set; }
    }
}