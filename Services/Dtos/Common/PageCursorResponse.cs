namespace ViteMontevideo_API.Services.Dtos.Common
{
    public class PageCursorResponse<T>
    {
        public int Cantidad { get; set; }
        public int SiguienteCursor { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PageCursorResponse(int cantidad, int siguienteCursor, IEnumerable<T> data)
        {
            Cantidad = cantidad;
            SiguienteCursor = siguienteCursor;
            Data = data;
        }
    }
}
