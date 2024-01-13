namespace Achiever.Client.Models
{
    public class SuccessResult<T> : ClientResult<T>
    {
        public SuccessResult(T value, string message = "")
        {
            Value = value;
            IsSuccess = true;
            Message = message;
        }
    }
}
