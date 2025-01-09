using ViteMontevideo_API.Services.Dtos.Cursor;
using ViteMontevideo_API.Services.Enums;

namespace ViteMontevideo_API.Services.Dtos.CajasChicas.Parameters
{
    public class FiltroCajaChica : CursorParams
    {
        public DateTime FechaInicio { get; set; } = DateTime.UtcNow.AddDays(-1);
        public DateTime FechaFinal { get; set; } = DateTime.UtcNow;
        public Turno? Turno { get; set; }
    }
}
