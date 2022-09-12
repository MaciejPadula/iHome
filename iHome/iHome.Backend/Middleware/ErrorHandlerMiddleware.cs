using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace iHome.Backend.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                await RespondWithError(context, e);
            }
        }

        private async Task RespondWithError(HttpContext context, Exception ex)
        {
            var responseObject = new
            {
                context.Request.Path,
                context.Response.StatusCode,
                ex.Message
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(responseObject));
        }
    }
}
