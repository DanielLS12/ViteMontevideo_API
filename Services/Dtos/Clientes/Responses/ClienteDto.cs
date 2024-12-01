namespace ViteMontevideo_API.Services.Dtos.Clientes.Responses
{
    public class ClienteDto
    {
        public int IdCliente { get; set; }
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
    }
}
