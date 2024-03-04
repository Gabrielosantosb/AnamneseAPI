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
                return Ok(new { message = "Paciente criado com sucesso", pacientId = createdPacient.Id });
            }
            else
            {
                return BadRequest("Falha ao criar paciente");
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
    }
}
