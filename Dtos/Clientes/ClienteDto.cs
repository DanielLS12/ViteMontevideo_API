namespace ViteMontevideo_API.Dtos.Clientes
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
