using CatalogAPI.Models;
using CatalogAPI.Services;
using CatalogAPI.Services.User;
using Microsoft.AspNetCore.Authorization;

namespace CatalogAPI.ApiEndpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            app.MapPost("/create-user", [AllowAnonymous] (CreateUserModel newUser, IUserService userService) =>
            {
                if (newUser == null) return Results.BadRequest("Dados do usuário inválidos");

                
                if (userService.IsEmailTaken(newUser.Email))
                {
                    return Results.BadRequest("E-mail já em uso. Escolha outro.");
                }

                
                UserModel createdUser = userService.CreateUser(newUser);

                if (createdUser != null)
                {
                    return Results.Ok(new { message = "Usuário criado com sucesso", userId = createdUser.Id });
                }
                else
                {
                    return Results.BadRequest("Falha ao criar usuário");
                }
            })
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK)
            .WithName("CreateUser")
            .WithTags("Usuário");
        }
    }
}
