using CatalogAPI.Integracao.Response;
using Refit;

namespace CatalogAPI.Integracao.Refit
{
    public interface IViaCepIntegracaoRefit
    {
        //Assinatura Refit
        [Get("/ws/{cep}/json")]
        Task<ApiResponse<ViaCepResponse>> GetDataViaCep(string cep);

    };
}
