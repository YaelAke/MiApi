namespace MiApi.Models
{
    public class ResponseBase<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
    }

    public class ResponseBase
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
