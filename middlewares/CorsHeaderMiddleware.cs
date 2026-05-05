using Microsoft.AspNetCore.Http;

namespace WebApplication1.Middlewares
{
    public class CorsHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public CorsHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
            await _next(context);
        }
    }
}