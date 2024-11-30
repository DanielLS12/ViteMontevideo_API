
namespace ViteMontevideo_API.Presentation.Dtos.Common
{
    public class DataMontoResponse<T> : DataResponse<T> where T : class
    {
        public decimal TotalMonto { get; set; }
        public decimal? TotalDescuento { get; set; }

        public DataMontoResponse(int cantidad, IEnumerable<T> data, decimal totalMonto) : base(cantidad, data)
        {
            TotalMonto = totalMonto;
        }

        public DataMontoResponse(int cantidad, IEnumerable<T> data, decimal totalMonto, decimal? totalDescuento) : base(cantidad, data)
        {
            TotalMonto = totalMonto;
            TotalDescuento = totalDescuento;
        }
    }
}
