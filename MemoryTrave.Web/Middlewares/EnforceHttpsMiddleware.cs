namespace MemoryTrave.Web.Middlewares;

public class EnforceHttpsMiddleware(RequestDelegate next, IWebHostEnvironment env)
{
    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.IsHttps && !env.IsDevelopment())
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "text/plain; charset=utf-8";
            await context.Response.WriteAsync("HTTPS is required. HTTP connections are not accepted.");
            return;
        }

        await next(context);
    }
}