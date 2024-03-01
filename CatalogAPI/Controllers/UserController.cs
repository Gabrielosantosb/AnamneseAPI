using CatalogAPI.Models;
using CatalogAPI.Services.Token;
using CatalogAPI.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, ITokenService tokenService, IConfiguration configuration)
        {
            _userService = userService;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginModel)
        {
            if (loginModel == null)
            {
                return BadRequest("Credenciais inválidas");
            }

            bool isAuthenticated = await _userService.ValidateCredentials(loginModel.Email, loginModel.Password);

            if (isAuthenticated)
            {
                UserModel user = await _userService.GetUserByEmailAsync(loginModel.Email);

                var tokenString = _tokenService.GenerateToken(
                    _configuration["Jwt:Key"],
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    user
                );

                return Ok(new { token = tokenString });
            }
            else
            {
                return BadRequest("Credenciais inválidas");
            }
        }
        [HttpPost("create-user")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUser([FromBody] CreateUserModel userModel)
        {
            if (userModel == null)
            {
                return BadRequest("Dados do usuário inválidos");
            }

            if (_userService.IsEmailTaken(userModel.Email))
            {
                return BadRequest("E-mail já em uso. Escolha outro.");
            }

            UserModel createdUser = _userService.CreateUser(userModel);

            if (createdUser != null)
            {
                return Ok(new { message = "Usuário criado com sucesso", userId = createdUser.Id });
            }
            else
            {
                return BadRequest("Falha ao criar usuário");
            }
        }

        [HttpGet("doctors-with-patients")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetDoctorsWithPatients()
        {
            List<UserModel> doctorsWithPatients = _userService.GetDoctorsWithPatients();

            return Ok(doctorsWithPatients);
        }
    }
}
