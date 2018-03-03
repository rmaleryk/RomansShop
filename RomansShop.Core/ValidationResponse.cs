namespace RomansShop.Core
{
    public enum ValidationStatus
    {
        Ok,
        NotFound,
        Failed
    }

    public class ValidationResponse<TData>
    {
        public TData ResponseData { get; set; }

        public string Message { get; set; }

        public ValidationStatus Status { get; set; } = ValidationStatus.Ok;
    }
}