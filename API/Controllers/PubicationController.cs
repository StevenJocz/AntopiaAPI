using Antopia.Domain.DTOs.PublicationDTOs;
using Antopia.Persistence.Commands.PublicationCommands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Antopia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PubicationController : Controller
    {
        private readonly ILogger<PubicationController> _logger;
        private readonly IPublicationCommands _publicationCommands;

        public PubicationController(ILogger<PubicationController> logger, IPublicationCommands publicationCommands)
        {
            _logger = logger;
            _publicationCommands = publicationCommands;
        }


        [HttpPost("Create_Publication")]
        public async Task<IActionResult> Create_Publication([FromBody] ListPublicationDTOs listPublicationDTOs)
        {
            _logger.LogInformation("Iniciando UserController.Create_User...");

            var respuesta = await _publicationCommands.InsertarPublication(listPublicationDTOs);
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
