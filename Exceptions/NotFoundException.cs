namespace ViteMontevideo_API.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("No se logro encontrar el recurso.") { }
    }
}
