using CatalogAPI.ApiEndpoints;
using CatalogAPI.AppServicesExtensions;
using CatalogAPI.Context;
using CatalogAPI.Integracao;
using CatalogAPI.Integracao.Interfaces;
using CatalogAPI.Integracao.Refit;
using CatalogAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Refit;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAuthJWT();
builder.Services.AddCors();



builder.Services.AddScoped<IViaCepIntegration, ViaCepIntegration>();

builder.Services.AddRefitClient<IViaCepIntegracaoRefit>().ConfigureHttpClient(httpClient =>
    httpClient.BaseAddress = new Uri("https://viacep.com.br"));



var app = builder.Build();
app.MapAuthentificateEndpoints();
app.MapCategoryEndpoints();
app.MapProductEndpoints();
app.MapViaCepEndpoints();
app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { });
}
var environment = app.Environment;
app.UseExceptionHandling(environment).UseSwaggerMiddleware().UseAppCors();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
