namespace ViteMontevideo_API.Services.Dtos.Usuarios
{
    public class UsuarioResponseDto
    {
        public short IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}
