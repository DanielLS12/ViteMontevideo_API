namespace ViteMontevideo_API.Dtos.Common
{
    public class DataResponse<T>
    {
        public int Cantidad { get; set; }
        public List<T> Data { get; set; }

        public DataResponse(int cantidad, List<T> data)
        {
            Cantidad = cantidad;
            Data = data;
        }
    }
}
