using CatalogAPI.Context;
using CatalogAPI.Integracao.Interfaces;
using CatalogAPI.Integracao.Response;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.ApiEndpoints
{

    [Route("cep")]
    public class ViaCepEndoints :ControllerBase
    {
        public readonly IViaCepIntegration _viaCepIntegration;

        public ViaCepEndoints(IViaCepIntegration viaCepIntegration)
        {
            _viaCepIntegration = viaCepIntegration;
        }

        [HttpGet("{cep}")]
        public async Task<ActionResult<ViaCepResponse>> GetDataAddress(string cep)
        {
            var responseData = await _viaCepIntegration.GetDataViaCep(cep);
            if (responseData == null)
            {
                return NotFound(); 
            }
            return Ok(responseData);
        }
    }
    
}
