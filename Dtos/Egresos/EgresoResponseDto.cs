namespace ViteMontevideo_API.Dtos.Egresos
{
    public class EgresoResponseDto
    {
        public int IdEgreso { get; set; }
        public int IdCaja { get; set; }
        public string Motivo { get; set; } = null!;
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
    }
}
