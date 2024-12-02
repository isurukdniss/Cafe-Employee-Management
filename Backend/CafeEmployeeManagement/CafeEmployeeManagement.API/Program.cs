using Autofac;
using Autofac.Extensions.DependencyInjection;
using CafeEmployeeManagement.API.Extensions.Middleware;
using CafeEmployeeManagement.Application;
using CafeEmployeeManagement.Infrastructure;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
         builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
});

var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Logos");
if (!Directory.Exists(uploadPath))
{
    Directory.CreateDirectory(uploadPath);
}

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterInfrastructureServices(builder.Configuration);
    containerBuilder.RegisterApplicationServices();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/Uploads"
});

app.UseCors("AllowAll");


app.MapControllers();

app.Run();
