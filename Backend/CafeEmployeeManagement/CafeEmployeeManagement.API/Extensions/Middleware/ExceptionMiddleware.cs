using CafeEmployeeManagement.Application.Common.Models;
using System.Text.Json;

namespace CafeEmployeeManagement.API.Extensions.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = "An error occurred.",
                    Errors = [ex.Message],
                };

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
