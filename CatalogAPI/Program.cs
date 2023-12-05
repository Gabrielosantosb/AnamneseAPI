using CatalogAPI.ApiEndpoints;
using CatalogAPI.AppServicesExtensions;
using CatalogAPI.Context;

using CatalogAPI.Services;
using CatalogAPI.Services.ViaCep;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Refit;
using RestSharp;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "apiagenda", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() 
                { 
                    Name = "Authorization", 
                    Type = SecuritySchemeType.ApiKey, 
                    Scheme = "Bearer", 
                    BearerFormat = "JWT", 
                    In = ParameterLocation.Header,
                   // Description = "JWT Authorization header using the Bearer scheme.
                   //\r\n\r\n Enter 'Bearer'[space] and then your token in the text input below.
                   // \r\n\r\nExample: \"Bearer 12345abcdef\"",
                }); 
                c.AddSecurityRequirement(new OpenApiSecurityRequirement 
                { 
                    { 
                          new OpenApiSecurityScheme 
                          { 
                              Reference = new OpenApiReference 
                              { 
                                  Type = ReferenceType.SecurityScheme, 
                                  Id = "Bearer" 
                              } 
                          }, 
                         new string[] {} 
                    } 
                }); 
   });
builder.Services.AddAuthorization();
builder.Services.AddCors();



builder.Services.AddSingleton<RestClient>(_ => new RestClient("https://viacep.com.br/"));
builder.Services.AddSingleton<ViaCepClient>();

builder.Services.AddSingleton<ITokenService>(new TokenService());



//--------------------------ValidarToken-----------------------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});




var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);

builder.Services.AddDbContext<MySQLContext>(options =>
    options.UseMySql(connectionString, serverVersion));

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
