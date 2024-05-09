using Antopia.Domain.DTOs.ColinaDTOs;
using Antopia.Domain.DTOs.PublicationDTOs;
using Antopia.Domain.DTOs.RespuestaDTOs;
using Antopia.Domain.Entities.ColoniaE;
using Antopia.Persistence.Commands.ColoniaCommands;
using Antopia.Persistence.Queries.ColoniaQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Antopia.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ColoniaController : Controller
    {

        private readonly ILogger<ColoniaController> _logger;
        private readonly IColoniaCommands _coloniaCommands;
        private readonly IColoniaQueries _coloniaQueries;

        public ColoniaController(ILogger<ColoniaController> logger, IColoniaCommands coloniaCommands, IColoniaQueries coloniaQueries)
        {
            _logger = logger;
            _coloniaCommands = coloniaCommands;
            _coloniaQueries = coloniaQueries;
        }

        [Authorize]
        [HttpPost("Create_Colonia")]
        public async Task<IActionResult> Create_Colonia([FromBody] ColoniaDTOs colinaDTOs)
        {
            _logger.LogInformation("Iniciando UserController.Create_Colonian...");

            var respuesta = await _coloniaCommands.CrearColonia(colinaDTOs);
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
        [HttpGet("User_Colonias")]
        public async Task<IActionResult> User_Colonia(int idUser)
        {
            var results = await _coloniaQueries.ColoniasUser(idUser);
            if (results == null || results.Count == 0)
            {
                return BadRequest("No se encontraron las colonias creadas por el usuario.");
            }
            else
            {
                return Ok(results);
            }
        }

        [Authorize]
        [HttpGet("Id_Colonias")]
        public async Task<IActionResult> ColoniasId (int idGrupo, int idUser)
        {
            _logger.LogInformation("Iniciando UserController.ColoniasId...");

            var respuesta = await _coloniaQueries.ColoniasId(idGrupo, idUser);
            if (respuesta == null || respuesta.Count == 0)
            {
                return BadRequest("No se encontraron la Colonia.");
            }
            else
            {
                return Ok(respuesta);
            }
        }

        [Authorize]
        [HttpPost("Unirme_Colonia")]
        public async Task<IActionResult> Unirme_Colonia([FromBody] MembersDTOs MembersDTOs)
        {
            _logger.LogInformation("Iniciando UserController.Unirme_Colonia...");

            var respuesta = await _coloniaCommands.UnirmeColonia(MembersDTOs);
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
        [HttpGet("Buscar_Colonias")]
        public async Task<IActionResult> Buscar_Colonias(string colonia)
        {
            _logger.LogInformation("Iniciando UserController.Buscar_Colonias...");

            var respuesta = await _coloniaQueries.BuscarColonias(colonia);
            if (respuesta == null || respuesta.Count == 0)
            {
                return BadRequest("No se encontraron la Colonia.");
            }
            else
            {
                return Ok(respuesta);
            }
        }

       
        [HttpGet("ImagenesColoniasId")]
        public async Task<IActionResult> ImagenesColoniasId(int idColonia)
        {
            _logger.LogInformation("Iniciando UserController.Buscar_Colonias...");

            var respuesta = await _coloniaQueries.ImagenesColoniasId(idColonia);
            if (respuesta == null || respuesta.Count == 0)
            {
                return BadRequest("No se encontraron imagenes de la colonia.");
            }
            else
            {
                return Ok(respuesta);
            }
        }

        [HttpGet("topColonias")]
        public async Task<IActionResult> ObtenerTresMejoresColoniasPorPuntos()
        {
            _logger.LogInformation("Iniciando UserController.ObtenerTresMejoresColoniasPorPuntos...");

            var respuesta = await _coloniaQueries.ObtenerTresMejoresColoniasPorPuntos();
            if (respuesta == null || respuesta.Count == 0)
            {
                return BadRequest("No se encontraron colonias.");
            }
            else
            {
                return Ok(respuesta);
            }
        }

    }
}
