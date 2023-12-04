using CatalogAPI.Integracao.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CatalogAPI.ApiEndpoints
{
    public static class ViaCepEndpoints
    {
        public static void MapViaCepEndpoints(this WebApplication app)
        {
            app.MapGet("/cep/{cep}", [AllowAnonymous] async (string cep, IViaCepIntegration viaCepIntegration) =>
            {
                var responseData = await viaCepIntegration.GetDataViaCep(cep);
                if (responseData == null)
                {
                    return Results.NotFound("CEP não encontrado");
                }
                return Results.Ok(responseData);
            })
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status200OK)
            .WithName("GetDataAddress")
            .WithTags("ConsultaCep");

        }
    }
}
