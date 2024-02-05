namespace Talabt.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponse( int statusCode ,string? message = null) 
        {
            StatusCode = statusCode ;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "UnAuthorized",
                404 => "Resourse Not Found",
                500 => "Errors are based to the dark side",
                _ => null,
            };
        }
    }
}
