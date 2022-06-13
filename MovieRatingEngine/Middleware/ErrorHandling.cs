using Microsoft.AspNetCore.Http;
using MovieRatingEngine.Dtos;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MovieRatingEngine.Middleware
{
    public class ErrorHandling
    {
        private readonly RequestDelegate _next;

        public ErrorHandling(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private static async Task HandleException(HttpContext context, Exception ex)
        {
            var exceptionType = ex.GetType();
            context.Response.ContentType = "application/json";

            var response = exceptionType switch
            {
                Type t when t == typeof(ArgumentNullException) => ServiceResponse<string>.BadRequest(),
                Type t when t == typeof(DllNotFoundException) => ServiceResponse<string>.NotFound(),
                _ => ServiceResponse<string>.InternalServerError()
            };

            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsync(SerializeResponse(response));
        }

        private static string SerializeResponse<T>(ServiceResponse<T> response) where T : class =>
            JsonConvert.SerializeObject(response);
    }
}
