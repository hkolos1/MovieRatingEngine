using System.Net;

namespace MovieRatingEngine.Middleware
{
    public class ExceptionResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null;

        public HttpStatusCode StatusCode { get; set; }
        public string Details { get; set; }

        public static ExceptionResponse CreateError(string exMessage, string exStackTrace, HttpStatusCode statusCode) => new ExceptionResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Message = exMessage,
            Details = exStackTrace,
            Success = false

        };
    }
}
