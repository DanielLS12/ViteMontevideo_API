﻿using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Presentation.Dtos.ComerciosAdicionales.Filtros
{
    public class FiltroComercioAdicional
    {
        public string Cliente { get; set; } = string.Empty;
        public TipoComercioAdicional? Tipo { get; set; }
        public TipoPago? TipoPago { get; set; }
        public bool? EstaPagado { get; set; }
    }
}
