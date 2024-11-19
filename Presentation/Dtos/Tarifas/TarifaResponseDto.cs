using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Presentation.Dtos.Tarifas
{
    public class TarifaResponseDto
    {
        public short IdTarifa { get; set; }
        public decimal PrecioDia { get; set; }
        public decimal PrecioNoche { get; set; }
        public TimeSpan? HoraDia { get; set; }
        public TimeSpan? HoraNoche { get; set; }
        public TimeSpan Tolerancia { get; set; }
        public string Modalidad
        {
            get
            {
                return HoraDia == null || HoraNoche == null ? "Hora" : "Turno";
            }
        }
        public virtual Actividad Actividad { get; set; } = null!;
        public virtual Categoria Categoria { get; set; } = null!;
    }
}
