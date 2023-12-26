namespace Achiever.Client.Models
{
    public class ClientResult<T>
    {
        public T? Value { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
