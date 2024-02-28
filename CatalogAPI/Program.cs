using CatalogAPI.ApiEndpoints;
using CatalogAPI.AppServicesExtensions;
using CatalogAPI.Services.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAuthJWT();
builder.Services.AddCors();
builder.Services.AddScoped<IUserService, UserService>();




var app = builder.Build();
app.MapAuthentificateEndpoints();
app.MapCategoryEndpoints();
app.MapProductEndpoints();
app.MapUserEndpoints();

app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AnamneseAPI");
        c.RoutePrefix = "swagger"; 
    });
}
var environment = app.Environment;
app.UseExceptionHandling(environment).UseSwaggerMiddleware().UseAppCors();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
