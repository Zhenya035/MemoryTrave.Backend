using MemoryTrave.Application.Interfaces;
using MemoryTrave.Infrastructure;
using MemoryTrave.Web;
using MemoryTrave.Web.Extensions;
using MemoryTrave.Web.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<MemoryTraveDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString(nameof(MemoryTraveDbContext)));
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddInfrastructureLayer();
builder.Services.AddApplicationLayer();
builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

builder.Services.AddJwt(configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();