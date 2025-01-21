namespace TaskManagement
{
    public class ServiceResult
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        public object? Data { get; }

        public ServiceResult(bool isSuccess, string errorMessage = "", object? data = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Data = data;
        }
    }
}