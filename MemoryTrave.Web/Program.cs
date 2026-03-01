using MemoryTrave.Infrastructure;
using MemoryTrave.Web.Extensions;
using MemoryTrave.Web.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<MemoryTraveDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString(nameof(MemoryTraveDbContext)));
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddInfrastructureLayer();
builder.Services.AddApplicationLayer();

builder.Services.AddJwt(configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введите JWT токен"
    });
    
    o.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", doc)] = []
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();