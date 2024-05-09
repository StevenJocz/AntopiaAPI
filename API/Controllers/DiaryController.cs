using Antopia.Domain.DTOs.DiaryDTOs;
using Antopia.Domain.DTOs.PublicationDTOs;
using Antopia.Persistence.Commands.DiaryCommands;
using Antopia.Persistence.Commands.PublicationCommands;
using Antopia.Persistence.Queries.DiaryQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Antopia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiaryController : Controller
    {
        private readonly ILogger<DiaryController> _logger;
        private readonly IDiaryCommands _diaryCommand;
        private readonly IDiaryQueries _diaryQueries;

        public DiaryController(ILogger<DiaryController> logger, IDiaryCommands diaryCommand, IDiaryQueries diaryQueries)
        {
            _logger = logger;
            _diaryCommand = diaryCommand;
            _diaryQueries = diaryQueries;
        }

        [Authorize]
        [HttpPost("RegistrarDiario")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarDiario([FromBody] DiaryDTOs diaryDTOs)
        {
            _logger.LogInformation("Iniciando DiaryController.RegistrarDiario...");

            var respuesta = await _diaryCommand.RegistrarDiario(diaryDTOs);

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
        [HttpPost("AddRegistroDiario")]
        public async Task<IActionResult> AddRegistroDiario([FromBody] ListDiary listDiary)
        {
            _logger.LogInformation("Iniciando DiaryController.AddRegistroDiario...");

            var respuesta = await _diaryCommand.AddRegistroDiario(listDiary);

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
        [HttpGet("ListarDiarios")]
        public async Task<IActionResult> ListarDiarios(int idUser, int userConsulta)
        {
            _logger.LogInformation("Iniciando DiaryController.ListarDiarios...");

            var respuesta = await _diaryQueries.ListarDiarios(idUser, userConsulta);

            if (respuesta == null || respuesta.Count == 0)
            {
                return BadRequest("No se encontraron diarios.");
            }
            else
            {
                return Ok(respuesta);
            }
        }

        [Authorize]
        [HttpPost("likeDiary")]
        public async Task<IActionResult> likeDiary([FromBody] DiaryLikeDTOs diaryLikeDTOs)
        {
            _logger.LogInformation("Iniciando DiaryController.likeDiary...");

            var respuesta = await _diaryCommand.likeDiary(diaryLikeDTOs);
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
        [HttpPost("RegistrarComentarios")]
        public async Task<IActionResult> RegistrarComentarios([FromBody] CommentDiaryDTOs commentDiaryDTOs)
        {
            _logger.LogInformation("Iniciando DiaryController.RegistrarComentarios...");

            var respuesta = await _diaryCommand.RegistrarComentarios(commentDiaryDTOs);
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
