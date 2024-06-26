﻿using Antopia.API.Application;
using Antopia.Domain.DTOs.EmailDTOs;
using Antopia.Domain.DTOs.LoginDTOs;
using Antopia.Domain.Entities.LoginE;
using Antopia.Infrastructure.EmailServices;
using Antopia.Persistence.Commands.LoginCommands;
using Antopia.Persistence.Queries.LoginQueries;
using Microsoft.AspNetCore.Mvc;

namespace Antopia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAutorizacionService _autorizacionService;
        private readonly ILoginCommands _loginCommands;
        private readonly ILoginQueries _loginQueries;
        private readonly IEmailServices _emailServices;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IAutorizacionService autorizacionService, ILoginCommands loginCommands, ILoginQueries loginQueries, IEmailServices emailServices, ILogger<LoginController> logger)
        {
            _autorizacionService = autorizacionService;
            _loginCommands = loginCommands;
            _loginQueries = loginQueries;
            _emailServices = emailServices;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Autenticar([FromBody] LoginDTOs autorizacion)
        {
            try
            {
                _logger.LogInformation("Iniciando Autenticar.Controller");
                var resultado_autorizacion = await _autorizacionService.DevolverToken(autorizacion);
                if (resultado_autorizacion == null)
                    return Unauthorized();
                return Ok(resultado_autorizacion);
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar Autenticar.Controller");
                throw;
            }
        }

        [HttpPost("EmailRestablecimientoPassword")]
        public async Task<IActionResult> EmailRestablecimientoPassword(EmailDTOs request)
        {
            try
            {
                var respuesta = await _emailServices.EmailRestablecimientoPassword(request);
                if (respuesta.codigo != null)
                {
                    var nuevoCodigo = new CodigoRestablecimientoE
                    {
                        s_correo = request.Para,
                        s_codigo = respuesta.codigo
                    };

                    bool registroRealizado = await _loginCommands.InsertarCodigo(nuevoCodigo);
                    if (registroRealizado)
                    {
                        return Ok(new { respuesta.message, respuesta.resultado });
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            resultado = false,
                            message = "Tenemos problemas al enviar el correo electrónico. Por favor intentalo más tarde.",
                        });
                    }
                }
                else
                {
                    return Ok(new
                    {
                        resultado = false,
                        message = respuesta.message,
                    });
                }


            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar EmailRestablecimientoPassword.Controller");
                throw;
            }
        }


        [HttpPost("VerificarCodigo")]
        public async Task<IActionResult> VerificarCodigo(CodigoRestablecimientoDTOs request)
        {
            try
            {
                var nuevoCodigo = new CodigoRestablecimientoE
                {
                    s_correo = request.s_correo,
                    s_codigo = request.s_codigo
                };

                bool codigoCorrecto = await _loginQueries.ConsultarCodigo(nuevoCodigo);
                if (codigoCorrecto)
                {
                    bool eliminarCodigo = await _loginCommands.EliminarCodigo(request.s_correo);
                    return Ok(new
                    {
                        resultado = true,
                        message = "El código ingresado es correcto. Puede continuar con el proceso.",
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        resultado = false,
                        message = "Código incorrecto. Por favor verifica el código y vuelve a intentarlo.",
                    });
                }

            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar EmailRestablecimientoPassword.Controller");
                throw;
            }
        }

        [HttpPut("ActualizarPassword")]
        public async Task<IActionResult> ActualizarPassword(string userEmail, string nuevoPassword)
        {
            try
            {
                bool Correcto = await _loginCommands.ActualizarPassword(userEmail, nuevoPassword);
                if (Correcto)
                {
                    return Ok(new
                    {
                        resultado = true,
                        message = "¡Cambio de contraseña exitoso! Por favor, inicia sesión nuevamente utilizando tu nueva contraseña.",
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        resultado = false,
                        message = "No se pudo cambiar la contraseña. Por favor, inténtalo nuevamente.",
                    });
                }

            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar EmailRestablecimientoPassword.Controller");
                throw;
            }
        }

        [HttpPut("ActualizarPasswordUser")]
        public async Task<IActionResult> ActualizarPasswordUser()
        {
            try
            {
                bool Correcto = await _loginCommands.ActualizarPasswordUser();
                if (Correcto)
                {
                    return Ok(new
                    {
                        resultado = true,
                        message = "¡Cambio de contraseñas exitoso!",
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        resultado = false,
                        message = "No se pudo cambiar las contraseñas. Por favor, inténtalo nuevamente.",
                    });
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
