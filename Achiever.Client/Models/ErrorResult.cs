namespace Achiever.Client.Models
{
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
