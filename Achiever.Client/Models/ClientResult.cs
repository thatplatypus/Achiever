namespace Achiever.Client.Models
{
    public class ClientResult<T>
    {
        public T? Value { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class SuccessResult<T> : ClientResult<T>
    {
        public SuccessResult(T value, string message = "")
        {
            Value = value;
            IsSuccess = true;
            Message = message;
        }
    }

    public class ErrorResult<T> : ClientResult<T>
    {
        public ErrorResult(string message)
        {
            Message = message;
            IsSuccess = false;
            Value = default;
        }
    }   
}
