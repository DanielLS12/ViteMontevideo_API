namespace ViteMontevideo_API.Presentation.Dtos.Common
{
    public class CursorResponse<T>
    {
        public int Cantidad { get; set; }
        public int SiguienteCursor { get; set; }
        public IEnumerable<T> Data { get; set; }

        public CursorResponse(int cantidad, int siguienteCursor, IEnumerable<T> data)
        {
            Cantidad = cantidad;
            SiguienteCursor = siguienteCursor;
            Data = data;
        }
    }
}
