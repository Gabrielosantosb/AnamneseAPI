using CatalogAPI.Models;
using CatalogAPI.Services.Pacient;
using CatalogAPI.Services.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientController : ControllerBase
    {
        private readonly IPacientService _pacientService;
        private readonly ITokenService _tokenService;

        public PacientController(IPacientService pacientService, ITokenService tokenService)
        {
            _pacientService = pacientService;
            _tokenService = tokenService;
        }

        [HttpPost("create-pacient")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreatePacient([FromBody] PacientModel pacientModel)
        {
            //pacientModel.DoctorId = _tokenService.GetUserId();

            if (pacientModel == null)
            {
                return BadRequest("Dados do paciente inválidos");
            }                        
            var createdPacient = _pacientService.CreatePacient(pacientModel);            
            if (createdPacient != null)
            {
                return Ok(pacientModel);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("get-pacients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetPacients()
        {
            var pacients = _pacientService.GetAllPacients();

            return Ok(pacients);
        }

        [HttpGet("get-pacient/{pacientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetPacientsById(int pacientId)
        {
            var pacient = _pacientService.GetPacientById(pacientId);

            if (pacient != null)
            {
                return Ok(pacient);
            }
            else
            {
                return BadRequest("Paciente não encontrado");
            }
        }


        [HttpDelete("remove-pacient/{pacientId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult RemovePacient(int pacientId)
        {
            var removedPacient = _pacientService.DeletePacient(pacientId);

            return Ok(removedPacient);

        }

        [HttpPut("update-pacient/{pacientId}")]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePacient(int pacientId, [FromBody] PacientModel updatedPacientModel)
        {
            if (updatedPacientModel == null)
            {
                return BadRequest("Dados do paciente inválidos");
            }

            var existingPacient = _pacientService.GetPacientById(pacientId);

            if (existingPacient == null)
            {
                return BadRequest("Paciente não encontrado");
            }

            // Atualize as propriedades do paciente existente com os dados do modelo atualizado
            existingPacient.UserName = updatedPacientModel.UserName;
            existingPacient.Email = updatedPacientModel.Email;
            existingPacient.Adress = updatedPacientModel.Adress;
            existingPacient.Uf = updatedPacientModel.Uf;
            existingPacient.Phone = updatedPacientModel.Phone;
            existingPacient.Birth = updatedPacientModel.Birth;
            existingPacient.Gender = updatedPacientModel.Gender;

            // Chame o serviço para atualizar o paciente
            var updatedPacient = _pacientService.UpdatePacient(pacientId, updatedPacientModel);

            if (updatedPacient != null)
            {
                return Ok(updatedPacient);
            }
            else
            {
                return BadRequest("Falha ao atualizar paciente");
            }
        }
    }
}
