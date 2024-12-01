namespace ViteMontevideo_API.Services.Dtos.CajasChicas.Parameters
{
    public class FiltroCajaChica
    {
        public DateTime FechaInicio { get; set; } = DateTime.UtcNow.AddDays(-1);
        public DateTime FechaFinal { get; set; } = DateTime.UtcNow;
        public string Turno { get; set; } = string.Empty;
    }
}
