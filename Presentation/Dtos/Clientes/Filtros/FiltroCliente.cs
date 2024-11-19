using ViteMontevideo_API.Presentation.Dtos.Cursor;

namespace ViteMontevideo_API.Presentation.Dtos.Clientes.Filtros
{
    public class FiltroCliente : CursorParams
    {
        public string NombreCompleto { get; set; } = string.Empty;
    }
}
