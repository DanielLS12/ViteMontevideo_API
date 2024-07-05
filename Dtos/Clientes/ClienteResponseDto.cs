namespace ViteMontevideo_API.Dtos.Clientes
{
    public class ClienteResponseDto
    {
        public int IdCliente { get; set; }
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public int NumeroVehiculos { get; set; }
        public int NumeroComerciosAdicionales { get; set; }
    }
}
