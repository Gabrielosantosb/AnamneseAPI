using CatalogAPI.AppServicesExtensions;
using CatalogAPI.Models;
using CatalogAPI.Services.Pacient;
using CatalogAPI.Services.User;
using static CatalogAPI.Repository.Repository;

var builder = WebApplication.CreateBuilder(args);

// Adicione os serviços ao contêiner.
// Saiba mais sobre a configuração do Swagger/OpenAPI em https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAuthJWT();
builder.Services.AddCors();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPacientService, PacientService>();
builder.Services.AddScoped<BaseRepository<UserModel>>();
builder.Services.AddScoped<BaseRepository<PacientModel>>();

// Adicione o serviço de controladores
builder.Services.AddControllers();

var app = builder.Build();

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

app.MapControllers();
app.UseExceptionHandling(environment).UseSwaggerMiddleware().UseAppCors();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
