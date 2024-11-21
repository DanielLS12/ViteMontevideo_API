
namespace ViteMontevideo_API.Presentation.Dtos.Common
{
    public class PageCursorMontoResponse<T> : PageCursorResponse<T> where T : class
    {
        public decimal TotalMonto { get; set; }
        public PageCursorMontoResponse(int cantidad, int siguienteCursor, IEnumerable<T> data, decimal totalMonto) : base(cantidad, siguienteCursor, data)
        {
            TotalMonto = totalMonto;
        }
    }
}
