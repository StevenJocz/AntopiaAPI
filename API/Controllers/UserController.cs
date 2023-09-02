using Antopia.Domain.DTOs.RespuestaDTOs;
using Antopia.Domain.DTOs.UserDTOs;
using Antopia.Domain.Entities.UserE;
using Antopia.Infrastructure.EmailServices;
using Antopia.Persistence.Commands.UserCommands;
using Microsoft.AspNetCore.Mvc;

namespace Antopia.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserCommands _userCommands;
        private readonly IEmailServices _emailServices;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserCommands userCommands, IEmailServices emailServices, ILogger<UserController> logger)
        {
            _userCommands = userCommands;
            _emailServices = emailServices;
            _logger = logger;
        }



        [HttpPost("Create_User")]
        public async Task<IActionResult> Create_User([FromBody] UserDTOs user)
        {
            try
            {
                _logger.LogInformation("Iniciando UserController.Create_User...");
                var respuesta = await _userCommands.InsertarUser(user);
                if (respuesta.resultado == false)
                {
                    return BadRequest(respuesta);
                }
                else 
                {
                    var CorreoEnviado = await _emailServices.EmailCreateUser(user.s_user_email);
                    return Ok(respuesta);
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar UserController.Create_User...");
                throw;
            }
        }

        [HttpPut("ActualizrDatos")]
        public async Task<IActionResult> ActualizrDatos(string idUser, string tipo, string dato)
        {
            try
            {
                var respuesta = await _userCommands.ActualizrDatos(int.Parse(idUser), int.Parse(tipo), dato);

                if (respuesta.resultado == false)
                {
                    return BadRequest(respuesta);
                }
                else
                {
                    return Ok(respuesta);
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar EmailRestablecimientoPassword.Controller");
                throw;
            }
        }
    }
}
