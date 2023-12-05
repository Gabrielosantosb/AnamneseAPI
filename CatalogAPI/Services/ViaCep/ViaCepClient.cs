using CatalogAPI.Services.ViaCep.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace CatalogAPI.Services.ViaCep
{
    [AllowAnonymous]
    public class ViaCepClient
    {
        private readonly RestClient _restClient;

        public ViaCepClient(RestClient restClient)
        {
            _restClient = restClient;
        }

        public ViaCepResponse GetCep(string cep)
        {
            var resource = $"ws/{cep}/json";
            var request = new RestSharp.RestRequest(resource, Method.Get);

            try
            {
                var response = _restClient.Execute<ViaCepResponse>(request);
                if (response.IsSuccessful) return response.Data!;
                throw new Exception("Erro na requisição à API Via CEP");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro na requisição à API Via CEP");
            }
        }
    }
}
