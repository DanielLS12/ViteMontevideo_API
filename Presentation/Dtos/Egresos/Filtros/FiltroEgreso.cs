using ViteMontevideo_API.Presentation.Dtos.Cursor;

namespace ViteMontevideo_API.Presentation.Dtos.Egresos.Filtros
{
    public class FiltroEgreso : CursorParams
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
    }
}
