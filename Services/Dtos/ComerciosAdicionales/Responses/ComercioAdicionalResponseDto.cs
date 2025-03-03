﻿using ViteMontevideo_API.Services.Dtos.Clientes.Responses;

namespace ViteMontevideo_API.Services.Dtos.ComerciosAdicionales.Responses
{
    public class ComercioAdicionalResponseDto
    {
        public int IdComercioAdicional { get; set; }
        public int? IdCaja { get; set; }
        public string TipoComercioAdicional { get; set; } = null!;
        public decimal Monto { get; set; }
        public DateTime? FechaPago { get; set; }
        public TimeSpan? HoraPago { get; set; }
        public string? TipoPago { get; set; }
        public bool EstadoPago
        {
            get
            {
                return IdCaja != null;
            }
        }
        public string? Observacion { get; set; }
        public virtual ClienteDto Cliente { get; set; } = null!;
    }
}
