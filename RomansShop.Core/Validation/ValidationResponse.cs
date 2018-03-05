namespace RomansShop.Core.Validation
{
    public class ValidationResponse<TData> where TData : class
    {
        public TData ResponseData { get; }

        public string Message { get; }

        public ValidationStatus Status { get; }

        public ValidationResponse(TData responseData,  ValidationStatus status, string message = "")
        {
            ResponseData = responseData;
            Message = message;
            Status = status;
        }

        public ValidationResponse(ValidationStatus status, string message) : this(null, status, message)
        {
        }
    }
}