namespace ViteMontevideo_API.Services.Dtos.Common
{
    public class DataResponse<T>
    {
        public int Cantidad { get; set; }
        public IEnumerable<T> Data { get; set; }

        public DataResponse(int cantidad, IEnumerable<T> data)
        {
            Cantidad = cantidad;
            Data = data;
        }
    }
}
