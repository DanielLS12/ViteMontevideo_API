namespace ViteMontevideo_API.Services.Parameters
{
    public class CobroParameter
    {
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public TimeSpan Estadia { get; set; }
        public TimeSpan HoraDia { get; set; }
        public TimeSpan HoraNoche { get; set; }
        public decimal PrecioDia { get; set; }
        public decimal PrecioNoche { get; set; }
        public TimeSpan Tolerancia { get; set; }

        public CobroParameter(DateTime fechaEntrada, DateTime fechaSalida, TimeSpan estadia, TimeSpan horaDia, TimeSpan horaNoche, decimal precioDia, decimal precioNoche, TimeSpan tolerancia)
        {
            FechaEntrada = fechaEntrada;
            FechaSalida = fechaSalida;
            Estadia = estadia;
            HoraDia = horaDia;
            HoraNoche = horaNoche;
            PrecioDia = precioDia;
            PrecioNoche = precioNoche;
            Tolerancia = tolerancia;
        }
    }
}
