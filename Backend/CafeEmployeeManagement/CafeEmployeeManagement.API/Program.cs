using CafeEmployeeManagement.API.Extensions;
using CafeEmployeeManagement.API.Extensions.Middleware;
using CafeEmployeeManagement.API.Extensions.OpenApi;
using CafeEmployeeManagement.Application;
using CafeEmployeeManagement.Infrastructure;
using CafeEmployeeManagement.Infrastructure.Identity;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

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

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Cafe Employee Management API"));
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/Uploads"
});

// CORS must run before authentication so 401/403 responses still carry CORS headers.
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await IdentitySeeder.SeedAdminUserAsync(app.Services);

app.Run();
