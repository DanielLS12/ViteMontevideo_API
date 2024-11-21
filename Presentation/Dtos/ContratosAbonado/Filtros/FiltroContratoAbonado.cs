using ViteMontevideo_API.Presentation.Dtos.Cursor;

namespace ViteMontevideo_API.Presentation.Dtos.ContratosAbonado.Filtros
{
    public class FiltroContratoAbonado : CursorParams
    {
        public string Placa { get; set; } = string.Empty;
        public bool? EstaPagado { get; set; }
    }
}
