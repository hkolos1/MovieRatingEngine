using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using MovieRatingEngine.Dtos;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MovieRatingEngine.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly IHostEnvironment _env;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(IHostEnvironment env, RequestDelegate next)
        {
            _env = env;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                
                if (_env.IsDevelopment())
                    await HandleExceptionForDevelopment(context, ex);
                else
                    await HandleExceptionForProduction(context, ex);
            }
        }

        private static async Task HandleExceptionForProduction(HttpContext context, Exception ex)
        {
            var exceptionType = ex.GetType();
            context.Response.ContentType = "application/json";

            var response = exceptionType switch
            {
                Type t when t == typeof(ArgumentNullException) => ExceptionResponse.CreateError("", "", HttpStatusCode.BadRequest),
                Type t when t == typeof(DllNotFoundException) => ExceptionResponse.CreateError("","", HttpStatusCode.NotFound),
                _ => ExceptionResponse.CreateError("", "", HttpStatusCode.InternalServerError)
            };

            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsync(SerializeResponse(response));
        }

        private static async Task HandleExceptionForDevelopment(HttpContext context, Exception ex)
        {
            var exceptionType = ex.GetType();
            context.Response.ContentType = "application/json";

            var response = exceptionType switch
            {
                Type t when t == typeof(ArgumentNullException) => ExceptionResponse.CreateError(ex.Message,ex.StackTrace?.ToString(),HttpStatusCode.BadRequest),
                Type t when t == typeof(DllNotFoundException) => ExceptionResponse.CreateError(ex.Message,ex.StackTrace?.ToString(),HttpStatusCode.NotFound),
                _ => ExceptionResponse.CreateError(ex.Message, ex.StackTrace?.ToString(), HttpStatusCode.InternalServerError)
            };

            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsync(SerializeResponse(response));
        }

        private static string SerializeResponse(ExceptionResponse response)  =>
            JsonConvert.SerializeObject(response);
    }
}
