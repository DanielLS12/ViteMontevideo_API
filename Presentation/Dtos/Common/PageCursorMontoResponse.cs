
namespace ViteMontevideo_API.Presentation.Dtos.Common
{
    public class PageCursorMontoResponse<T> : PageCursorResponse<T> where T : class
    {
        public decimal TotalMonto { get; set; }
        public decimal? TotalDescuento { get; set; }

        public PageCursorMontoResponse(int cantidad, int siguienteCursor, IEnumerable<T> data, decimal totalMonto) : base(cantidad, siguienteCursor, data)
        {
            TotalMonto = totalMonto;
        }

        public PageCursorMontoResponse(int cantidad, int siguienteCursor, IEnumerable<T> data, decimal totalMonto, decimal? totalDescuento) : base(cantidad, siguienteCursor, data)
        {
            TotalMonto = totalMonto;
            TotalDescuento = totalDescuento;
        }
    }
}
