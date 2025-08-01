namespace FIAP.PLAY.Application.Shared.Resource
{
    public class Resultado<T> where T : class
    {
        public Resultado(T data)
        {
            Data = data;
            Success = true;
        }
        public Resultado()
        {
            Success = true;
        }
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string MessageDetail { get; set; }
    }
}
