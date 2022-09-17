using iHome.Core.Exceptions;
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
            catch (DatabaseException e)
            {
                await RespondWithError(context, context.Response.StatusCode, e);
            }
            catch (Exception e)
            {
                await RespondWithError(context, 500, e);
            }
        }

        private async Task RespondWithError(HttpContext context, int statusCode, Exception ex)
        {
            var responseObject = new
            {
                context.Request.Path,
                statusCode,
                ex.Message,
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(responseObject));
        }
    }
}
