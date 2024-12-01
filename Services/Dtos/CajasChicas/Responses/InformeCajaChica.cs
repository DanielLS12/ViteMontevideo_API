using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Services.Dtos.CajasChicas.Responses
{
    public class InformeCajaChica
    {
        public string Cajero { get; set; } = null!;
        public decimal MParticulares { get; set; }
        public decimal MTurnos { get; set; }
        public decimal MEsSalud { get; set; }
        public decimal MEfectivo { get; set; }
        public decimal CantidadSobranteFaltante { get; set; }
        public decimal MYape { get; set; }
        public decimal MOtros { get; set; }
        public decimal MServicios { get; set; }
        public decimal MContratosAbonados { get; set; }
        public decimal MEgresos { get; set; }
        public decimal MComerciosAdicionales { get; set; }
        public ICollection<ComercioAdicional> ComerciosAdicionales { get; set; } = new List<ComercioAdicional>();
        public ICollection<Egreso> Egresos { get; set; } = new List<Egreso>();
        public ICollection<ContratoAbonado> Abonados { get; set; } = new List<ContratoAbonado>();
    }
}
