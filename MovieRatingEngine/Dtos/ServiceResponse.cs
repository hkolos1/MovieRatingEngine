using System.Net;

namespace MovieRatingEngine.Dtos
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = null;

        public HttpStatusCode StatusCode { get; set; }

        public static ServiceResponse<T> Ok(T result) => new ServiceResponse<T>
        {
            Data = result,
            StatusCode = HttpStatusCode.OK
        };

        public static ServiceResponse<T> Ok() => new ServiceResponse<T>
        {
            StatusCode = HttpStatusCode.OK
        };

        public static ServiceResponse<T> BadRequest() => new ServiceResponse<T>
        {
            StatusCode = HttpStatusCode.BadRequest
        };

        public static ServiceResponse<T> NotFound() => new ServiceResponse<T>
        {
            StatusCode = HttpStatusCode.NotFound
        };

        public static ServiceResponse<T> InternalServerError() => new ServiceResponse<T>
        {
            StatusCode = HttpStatusCode.InternalServerError
        };
    }
}
