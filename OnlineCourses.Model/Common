namespace OnlineCourses.Model.Common
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ServiceResult()
        {
        }

        public ServiceResult(bool success, string message, T data = default(T))
        {
            Success = success;
            Message = message;
            Data = data;
        }

        // Helper methods for common scenarios
        public static ServiceResult<T> SuccessResult(T data, string message = "Operation successful")
        {
            return new ServiceResult<T>(true, message, data);
        }

        public static ServiceResult<T> FailureResult(string message)
        {
            return new ServiceResult<T>(false, message, default(T));
        }
    }
}
