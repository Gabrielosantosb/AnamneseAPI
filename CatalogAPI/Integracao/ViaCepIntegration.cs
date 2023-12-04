using CatalogAPI.Integracao.Interfaces;
using CatalogAPI.Integracao.Refit;
using CatalogAPI.Integracao.Response;

namespace CatalogAPI.Integracao
{
    public class ViaCepIntegration : IViaCepIntegration
    {
        private readonly IViaCepIntegracaoRefit _viaCepIntegracaoRefit;


        public ViaCepIntegration(IViaCepIntegracaoRefit viaCepIntegracaoRefit)
        {
            _viaCepIntegracaoRefit = viaCepIntegracaoRefit;
        }

        public async Task<ViaCepResponse> GetDataViaCep(string cep)
        {
            var responseData = await _viaCepIntegracaoRefit.GetDataViaCep(cep);
            if (responseData != null && responseData.IsSuccessStatusCode) return responseData.Content;
            return null;
        }
    }
}
