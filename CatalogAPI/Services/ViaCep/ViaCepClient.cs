using CatalogAPI.Services.ViaCep.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Security.Claims;

namespace CatalogAPI.Services.ViaCep
{
    [AllowAnonymous]
    public class ViaCepClient
    {

        private RestClient _client;

        public ViaCepClient()
        {
            var uri = new Uri("https://viacep.com.br/");
            _client = new RestClient(uri);
        }

        public async Task<ViaCepResponse> GetCep(string cep)
        {
            var request = new RestSharp.RestRequest($"ws/{cep}/json");
            var response = await _client.ExecuteGetAsync(request);

            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<ViaCepResponse>(response.Content!);


            return  json;

        }
        public async Task<ViaCepResponse> PostCep(string cep)
        {

            //Aqui seria um post
            var request = new RestSharp.RestRequest($"ws/{cep}/json");
            var response = await _client.ExecuteGetAsync(request);

            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<ViaCepResponse>(response.Content);
            var data = System.Text.Json.JsonSerializer.Deserialize<ViaCepResponse>(response.Content);

            return data;

        }
    }
}
