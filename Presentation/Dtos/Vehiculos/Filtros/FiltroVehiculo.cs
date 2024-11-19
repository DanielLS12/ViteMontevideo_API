using ViteMontevideo_API.Presentation.Dtos.Cursor;

namespace ViteMontevideo_API.Presentation.Dtos.Vehiculos.Filtros
{
    public class FiltroVehiculo : CursorParams
    {
        public string Placa { get; set; } = string.Empty;
    }
}
