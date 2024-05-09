using Antopia.Domain.DTOs.PublicationDTOs;
using Antopia.Persistence.Commands.PublicationCommands;
using Antopia.Persistence.Queries.PublicationQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;

namespace Antopia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PubicationController : Controller
    {
        private readonly ILogger<PubicationController> _logger;
        private readonly IPublicationCommands _publicationCommands;
        private readonly IPublicationQueries _publicationQueries;

        public PubicationController(ILogger<PubicationController> logger, IPublicationCommands publicationCommands, IPublicationQueries publicationQueries)
        {
            _logger = logger;
            _publicationCommands = publicationCommands;
            _publicationQueries = publicationQueries;
        }

        [Authorize]
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

        [Authorize]
        [HttpDelete("Delete_Publication")]
        public async Task<IActionResult> Delete_Publication(int idPublication)
        {
            _logger.LogInformation("Iniciando UserController.Delete_Publication...");

            var respuesta = await _publicationCommands.DeletePublication(idPublication);
            if (respuesta.resultado == false)
            {
                return BadRequest(respuesta);
            }
            else
            {
                return Ok(respuesta);
            }
        }


       
        [HttpGet("TodoPublication")]
        public async Task<IActionResult> TodoPublication(int idUser, int tipo, int parametro, string hashtags)
        {
            _logger.LogInformation("Iniciando UserController.TodoPublication...");

            var respuesta = await _publicationQueries.TodoPublication(idUser, tipo, parametro, hashtags);
            if (respuesta == null || respuesta.Count == 0)
            {
                return BadRequest("No se encontraron publicaciones.");
            }
            else
            {
                return Ok(respuesta);
            }
        }

        [Authorize]
        [HttpPost("Add_Comentarios")]
        public async Task<IActionResult> Add_Comentarios([FromBody] CommentsDTOs commentsDTOs)
        {
            _logger.LogInformation("Iniciando UserController.Add_Comentarios...");

            var respuesta = await _publicationCommands.InsertarComentarios(commentsDTOs);
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
        [HttpPost("Add_Respuesta_Comentarios")]
        public async Task<IActionResult> InsertarRespuesta([FromBody] CommentsAnswercsDTOs commentsAnswercsDTOs)
        {
            _logger.LogInformation("Iniciando UserController.Add_Comentarios...");

            var respuesta = await _publicationCommands.InsertarRespuesta(commentsAnswercsDTOs);
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
        [HttpPost("Like_Comentarios")]
        public async Task<IActionResult> Like_Comentarios([FromBody] CommentsLikeDTOs commentsLikeDTOs)
        {
            _logger.LogInformation("Iniciando UserController.Like_Comentarios...");

            var respuesta = await _publicationCommands.like(commentsLikeDTOs);
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
        [HttpPost("likeComments")]
        public async Task<IActionResult> likeComments([FromBody] LikeCommentsDTOs likeCommentsDTOs)
        {
            _logger.LogInformation("Iniciando UserController.Like_Comentarios...");

            var respuesta = await _publicationCommands.likeComments(likeCommentsDTOs);
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
        [HttpPost("likeAnswers")]
        public async Task<IActionResult> likeAnswers([FromBody] AnswerLikeDTOs answerLikeDTOs)
        {
            _logger.LogInformation("Iniciando UserController.Like_Comentarios...");

            var respuesta = await _publicationCommands.likeAnswers(answerLikeDTOs);
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
        [HttpGet("SearchHashtags")]
        public IActionResult SearchHashtags(string searchTerm)
        {
            var results = _publicationQueries.SearchHashtags(searchTerm);
            return Ok(results);
        }

        [HttpGet("TopHashtags")]
        public IActionResult TopHashtags()
        {
            var results = _publicationQueries.TopHashtags();
            return Ok(results);
        }

        [Authorize]
        [HttpGet("buscarPublicaciones")]
        public async Task<IActionResult> buscarPublicaciones(string searchTerm)
        {
            var results = await _publicationQueries.BuscarPublications(searchTerm, 10);
            return Ok(results);
        }

        
        [HttpGet("BuscarPublicationSimilares")]
        public async Task<IActionResult> BuscarPublicationSimilares(string searchTerm)
        {
            var results = await _publicationQueries.BuscarPublicationSimilares(searchTerm, 3);
            return Ok(results);
        }

        [Authorize]
        [HttpPost("PublicationReportin")]
        public async Task<IActionResult> PublicationReportin([FromBody] PublicationReportingDTOs publicationReportingDTOs)
        {
            _logger.LogInformation("Iniciando UserController.PublicationReportin...");

            var respuesta = await _publicationCommands.PublicationReportin(publicationReportingDTOs);
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
        [HttpGet("Publication_reporting_reason")]
        public async Task<IActionResult> Publication_reporting_reason()
        {
            var results = await _publicationQueries.Publication_reporting_reason();
            return Ok(results);
        }

        [Authorize]
        [HttpGet("TopPublicaciones")]
        public async Task<IActionResult> TopPublicaciones(int tipo)
        {
            var results = await _publicationQueries.TopPublications(tipo);
            return Ok(results);
        }

        [Authorize]
        [HttpGet("ImagenesPublicaciones")]
        public async Task<IActionResult> ImagenesPublicaciones(int tipo)
        {
            var results = await _publicationQueries.ImagenesPublications(tipo);
            return Ok(results);
        }

    }
}
