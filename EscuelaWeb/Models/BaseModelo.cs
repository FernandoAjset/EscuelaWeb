﻿namespace EscuelaWeb.Models
{
    public class BaseModelo
    {
        public int PaginaActual { get; set; }
        public int TotalDeRegistros { get; set; }
        public int RegistrosPorPagina{ get; set; }
        public int PrimerRegistroDePagina { get; set; }
    }
}
