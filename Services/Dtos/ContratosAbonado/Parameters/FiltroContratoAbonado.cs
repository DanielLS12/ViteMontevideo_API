using ViteMontevideo_API.Services.Dtos.Cursor;

namespace ViteMontevideo_API.Services.Dtos.ContratosAbonado.Parameters
{
    public class FiltroContratoAbonado : CursorParams
    {
        public string Placa { get; set; } = string.Empty;
        public bool? EstadoPago { get; set; }
    }
}
