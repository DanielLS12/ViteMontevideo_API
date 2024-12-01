using ViteMontevideo_API.Services.Dtos.Cursor;
using ViteMontevideo_API.Services.Enums;

namespace ViteMontevideo_API.Services.Dtos.ComerciosAdicionales.Parameters
{
    public class FiltroComercioAdicional : CursorParams
    {
        public string Cliente { get; set; } = string.Empty;
        public TipoComercioAdicional? Tipo { get; set; }
        public TipoPago? TipoPago { get; set; }
        public bool? EstadoPago { get; set; }
    }
}
