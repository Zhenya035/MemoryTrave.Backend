namespace MemoryTrave.Web.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, IWebHostEnvironment env)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (UnauthorizedAccessException e)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            var problemDetail = new 
            {
                Title = "Unauthorized",
                Detail = env.IsDevelopment() ? e.Message : "An unexpected error occurred.",
                Instance = context.Request.Path
            };
            
            await context.Response.WriteAsJsonAsync(problemDetail);
        }
        catch (Exception e)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var problemDetail = new 
            {
                Title = "Internal Server Error",
                Detail = env.IsDevelopment() ? e.Message : "An unexpected error occurred.",
                Instance = context.Request.Path
            };
            
            await context.Response.WriteAsJsonAsync(problemDetail);
        }
    }
}