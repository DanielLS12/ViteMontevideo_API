using ViteMontevideo_API.Helpers.Enums;
using ViteMontevideo_API.Presentation.Dtos.Cursor;

namespace ViteMontevideo_API.Presentation.Dtos.ComerciosAdicionales.Filtros
{
    public class FiltroComercioAdicional : CursorParams
    {
        public string Cliente { get; set; } = string.Empty;
        public TipoComercioAdicional? Tipo { get; set; }
        public TipoPago? TipoPago { get; set; }
        public bool? EstadoPago { get; set; }
    }
}
