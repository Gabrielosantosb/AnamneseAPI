using CatalogAPI.Models;
using CatalogAPI.Services.Token;
using CatalogAPI.Services.User;
using Microsoft.AspNetCore.Authorization;

namespace CatalogAPI.ApiEndpoints
{
    public static class AuthentificateEndpoints
    {
        public static void MapAuthentificateEndpoints(this WebApplication app)
        {
            app.MapPost("/login", [AllowAnonymous] (LoginRequestModel loginModel, IUserService userService, ITokenService tokenService) =>
            {
                if (loginModel == null)
                {
                    return Results.BadRequest("Credenciais inválidas");
                }

                
                UserModel user = userService.ValidateCredentials(loginModel.Email, loginModel.Password);

                if (user != null)
                {
                    var tokenString = tokenService.GenerateToken(
                        app.Configuration["Jwt:Key"],
                        app.Configuration["Jwt:Issuer"],
                        app.Configuration["Jwt:Audience"],
                        user
                    );

                    return Results.Ok(new { token = tokenString });
                }
                else
                {
                    return Results.BadRequest("Credenciais inválidas");
                }
            })
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK)
            .WithName("Login")
            .WithTags("Autentificacao");
        }
    }
}
