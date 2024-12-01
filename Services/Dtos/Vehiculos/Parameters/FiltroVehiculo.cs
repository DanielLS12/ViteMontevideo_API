using ViteMontevideo_API.Services.Dtos.Cursor;

namespace ViteMontevideo_API.Services.Dtos.Vehiculos.Parameters
{
    public class FiltroVehiculo : CursorParams
    {
        public string Placa { get; set; } = string.Empty;
    }
}
