using Antopia.Domain.DTOs.ColinaDTOs;
using Antopia.Domain.DTOs.NotificacionDTOs;
using Antopia.Domain.DTOs.PublicationDTOs;
using Antopia.Persistence.Commands.NotificacionCommands;
using Antopia.Persistence.Queries.NotificacionQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Antopia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionesController : Controller
    {
        private readonly ILogger<ColoniaController> _logger;
        private readonly INotificacionQueries _notificacionQueries;
        private readonly INotificacionCommands _notificacionCommands;

        public NotificacionesController(ILogger<ColoniaController> logger, INotificacionQueries notificacionQueries, INotificacionCommands notificacionCommands)
        {
            _logger = logger;
            _notificacionQueries = notificacionQueries;
            _notificacionCommands = notificacionCommands;
        }

        [Authorize]
        [HttpGet("ListarNotificacionesUser")]
        public async Task<IActionResult> ListarNotificacionesUser(int idUser)
        {
            _logger.LogInformation("Iniciando NotificacionesController.ListarNotificacionesUser...");

            var respuesta = await _notificacionQueries.ListarNotificacionesUser(idUser);
            if (respuesta == null || respuesta.Count == 0)
            {
                return Ok("No se encontraron notificaciones");
            }
            else
            {
                return Ok(respuesta);
            }
        }

        [Authorize]
        [HttpPost("CambiarEstadoNotificacion")]
        public async Task<IActionResult> CambiarEstadoNotificacion([FromBody] idNotification idNotificacion)
        {
            _logger.LogInformation("Iniciando NotificacionesController.CambiarEstadoNotificacion...");

            var respuesta = await _notificacionCommands.CambiarEstadoNotificacion(idNotificacion);
            if (respuesta.resultado == false)
            {
                return BadRequest(respuesta);
            }
            else
            {
                return Ok(respuesta);
            }
        }

        [Authorize]
        [HttpGet("Notificaciones")]
        public async Task<IActionResult> Notificaciones(int idUser)
        {
            _logger.LogInformation("Iniciando NotificacionesController.Notificaciones...");

            var respuesta = await _notificacionQueries.Notificaciones(idUser);
            if (respuesta.resultado == false)
            {
                return BadRequest(respuesta);
            }
            else
            {
                return Ok(respuesta);
            }
        }

        [Authorize]
        [HttpPost("MarcaNotificacionesNoLeidas")]
        public async Task<IActionResult> MarcaNotificacionesNoLeidas([FromBody] IdUser idUser)
        {
            _logger.LogInformation("Iniciando NotificacionesController.MarcaNotificacionesNoLeidas...");

            var respuesta = await _notificacionCommands.MarcaNotificacionesNoLeidas(idUser);
            if (respuesta.resultado == false)
            {
                return BadRequest(respuesta);
            }
            else
            {
                return Ok(respuesta);
            }
        }
    }
}
