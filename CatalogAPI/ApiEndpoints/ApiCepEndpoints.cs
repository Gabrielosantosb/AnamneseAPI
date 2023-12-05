using CatalogAPI.Services.ViaCep.Model;
using CatalogAPI.Services.ViaCep;

namespace CatalogAPI.ApiEndpoints
{
    public static class ViaCepEndpoints
    {
        public static void MapViaCepEndpoints(this WebApplication app)
        {
            app.MapGet("/cep/{cep}", (string cep, ViaCepClient viaCepClient)
                => Results.Ok(viaCepClient.GetCep(cep)))
                .Produces<ViaCepResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("GetDataAddress")
                .WithTags("ConsultaCep");
        }
    }
}
