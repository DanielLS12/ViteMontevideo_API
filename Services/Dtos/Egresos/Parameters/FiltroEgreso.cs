using ViteMontevideo_API.Services.Dtos.Cursor;

namespace ViteMontevideo_API.Services.Dtos.Egresos.Parameters
{
    public class FiltroEgreso : CursorParams
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
    }
}
