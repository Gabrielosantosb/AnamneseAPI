using CatalogAPI.Integracao.Response;

namespace CatalogAPI.Integracao.Interfaces
{
    public interface IViaCepIntegration
    {
        Task<ViaCepResponse> GetDataViaCep(string cep);
    }
}
