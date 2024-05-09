using Antopia.Domain.DTOs.EmailDTOs;
using Antopia.Domain.DTOs.NotificacionDTOs;
using Antopia.Domain.DTOs.RespuestaDTOs;
using Antopia.Domain.DTOs.UserDTOs;
using Antopia.Domain.Entities.UserE;
using Antopia.Infrastructure.EmailServices;
using Antopia.Persistence.Commands.UserCommands;
using Antopia.Persistence.Queries.UserQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System;

namespace Antopia.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserCommands _userCommands;
        private readonly IUserQueries _userQueries;
        private readonly IEmailServices _emailServices;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserQueries userQueries,IUserCommands userCommands, IEmailServices emailServices, ILogger<UserController> logger)
        {
            _userQueries = userQueries;
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

        [Authorize]
        [HttpPut("ActualizrDatos")]
        public async Task<IActionResult> ActualizrDatos([FromBody] UpdateUserDTOs updateUserDTOs)
        {
            _logger.LogInformation("Iniciando UserController.ActualizrDatos...");
            try
            {
                var respuesta = await _userCommands.ActualizrDatos(updateUserDTOs);

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
                _logger.LogError("Error al iniciar ActualizrDatos.Controller");
                throw;
            }
        }

        [Authorize]
        [HttpGet("Datos_User")]
        public async Task<IActionResult> Datos_User(int idUser, int idUserConsulta)
        {
            _logger.LogInformation("Iniciando UserController.Datos_User...");
            try
            {
                var respuesta = await _userQueries.ConsultarUsuario(idUser, idUserConsulta);

                if (respuesta == null || !respuesta.Any())
                {
                    return BadRequest("No se encontraron resultados para el usuario con el ID proporcionado.");
                }
                else
                {
                    return Ok(respuesta);
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar DatorUser.Controller");
                throw;
            }
        }

        [Authorize]
        [HttpPost("Followers_User")]
        public async Task<IActionResult> Followers_User([FromBody] FollowersDTOs followersDTOs)
        {
            try
            {
                _logger.LogInformation("Iniciando UserController.Followers_User...");
                var respuesta = await _userCommands.FollowersDatos(followersDTOs);
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
                _logger.LogError("Error al iniciar UserController.Followers_User...");
                throw;
            }
        }

        [Authorize]
        [HttpGet("ConsultarUsuarioPorProfile")]
        public IActionResult ConsultarUsuarioPorProfile(string keyword)
        {
            _logger.LogInformation("Iniciando UserController.ConsultarUsuarioPorProfile...");
            try
            {
                var respuesta =  _userQueries.ConsultarUsuarioPorProfile(keyword);
                return Ok(respuesta);
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar ConsultarUsuarioPorProfile.Controller");
                throw;
            }
        }

        [Authorize]
        [HttpGet("ConsultarFollowers")]
        public async Task<IActionResult> ConsultarFollowers(int accion, int user)
        {
            _logger.LogInformation("Iniciando UserController.ConsultarFollowers...");
            try
            {
                var respuesta = await _userQueries.ConsultarFollowers(accion, user);
                return Ok(respuesta);
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar ConsultarFollowers.Controller");
                throw;
            }
        }

        [Authorize]
        [HttpGet("ConsultarNotFollowers")]
        public async Task<IActionResult> ConsultarNotFollowers(int user)
        {
            _logger.LogInformation("Iniciando UserController.ConsultarNotFollowers...");
            try
            {
                var respuesta = await _userQueries.ConsultarNotFollowers(user);
                return Ok(respuesta);
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar ConsultarNotFollowers.Controller");
                throw;
            }
        }

        [Authorize]
        [HttpGet("level")]
        public async Task<IActionResult> level(int idUser)
        {
            _logger.LogInformation("Iniciando UserController.level...");
            try
            {
                var respuesta = await _userCommands.level(idUser);
                return Ok(respuesta);
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar level.Controller");
                throw;
            }
        }

        [Authorize]
        [HttpGet("InvitarDiario")]
        public async Task<IActionResult> InvitarDiario(int idPerfil, string NombreUser)
        {
            _logger.LogInformation("Iniciando UserController.InvitarDiario...");
            try
            {
                var respuesta = await _emailServices.InvitarDiario(idPerfil, NombreUser);
                return Ok(respuesta);
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar InvitarDiario.Controller");
                throw;
            }
        }
    }
}
