using ViteMontevideo_API.Services.Dtos.Cursor;

namespace ViteMontevideo_API.Services.Dtos.Clientes.Parameters
{
    public class FiltroCliente : CursorParams
    {
        public string NombreCompleto { get; set; } = string.Empty;
    }
}
