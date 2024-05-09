using Antopia.Domain.DTOs.ColinaDTOs;
using Antopia.Domain.DTOs.NotificacionDTOs;
using Antopia.Domain.DTOs.PublicationDTOs;
using Antopia.Domain.DTOs.UserDTOs;
using Antopia.Domain.Entities.ColoniaE;
using Antopia.Domain.Entities.PublicacionE;
using Antopia.Domain.Entities.UserE;
using Antopia.Infrastructure;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static Antopia.Persistence.Queries.PublicationQueries.PublicationQueries;

namespace Antopia.Persistence.Queries.PublicationQueries
{
    public interface IPublicationQueries
    {
        Task<List<TodoPublicationDTOs>> TodoPublication(int idUser, int tipo, int parametro, string hashtags);
        List<HashtagSearchResultDTOs> SearchHashtags(string searchTerm);
        List<HashtagSearchResultDTOs> TopHashtags();
        Task<List<FilterPublicationDTOs>> BuscarPublications(string searchTerm, int topCount);
        Task<List<Publication_reporting_reasonDTOs>> Publication_reporting_reason();
        Task<List<TopPublicationDTOs>> TopPublications(int tipo);
        Task<List<PerfilImagenesDTOs>> ImagenesPublications(int tipo);
        Task<List<FilterPublicationDTOs>> BuscarPublicationSimilares(string searchTerm, int topCount);

    }

    public class PublicationQueries : IPublicationQueries, IDisposable
    {
        private readonly AntopiaDbContext _context = null;
        private readonly ILogger<PublicationQueries> _logger;
        private readonly IConfiguration _configuration;

        public PublicationQueries( ILogger<PublicationQueries> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection");
            _context = new AntopiaDbContext(connectionString);
        }


        #region implementacion Disponse
        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposed = true;
            }
        }

        #endregion


        public async Task<List<TodoPublicationDTOs>> TodoPublication(int idUser, int tipo, int parametro, string hashtags)
        {
            _logger.LogTrace("Iniciando metodo PublicationQueries.TodoPublication...");
            try
            {
                var expresion = (Expression<Func<PublicationE, bool>>)null;
                if (tipo == 1)
                {
                    expresion = expresion = x => x.fk_tbl_type_publication != 5 && x.byte_blocked == 0;
                }
                else if (tipo == 2)
                {
                    expresion = expresion = x => x.fk_tbl_type_publication == parametro && x.byte_blocked == 0;
                }
                else if (tipo == 3)
                {
                    expresion = expresion = x => x.fk_tbl_user == parametro && x.fk_tbl_type_publication != 5 && x.byte_blocked == 0;
                }
                else if (tipo == 4)
                {
                    expresion = expresion = x => x.fk_tbl_type_publication == parametro && x.byte_blocked == 0;
                }
                else if (tipo == 5)
                {
                    hashtags = hashtags.ToLower();
                    expresion = expresion =  p => p.s_hashtags != null && p.byte_blocked == 0 && p.s_hashtags.ToLower().Contains(hashtags);
                }
                else if (tipo == 6)
                {
                    expresion = expresion = x => x.id_publication == parametro && x.byte_blocked == 0;
                }
                else if (tipo == 7)
                {
                    var SiguiEN = await _context.FollowersEs.AsNoTracking().Where(x => x.id_follower == idUser).Select(x => x.id_user).ToListAsync();
                    expresion =  expresion = x => SiguiEN.Contains(x.fk_tbl_user) && x.fk_tbl_type_publication != 5 && x.byte_blocked == 0;
                }

                var Todopublications = await _context.PublicationEs.AsNoTracking().Where(expresion).OrderByDescending(x => x.dt_creation).Take(150).ToListAsync();

                var todoPublicationsList = new List<TodoPublicationDTOs>(); 

                foreach (var publication in Todopublications)
                {
                    var datosUsuario = await _context.UserEs.AsNoTracking().FirstOrDefaultAsync(x => x.id == publication.fk_tbl_user);

                    var imagenesPublications = await _context.PublicationImageEs.AsNoTracking().Where(x => x.fk_tbl_publication == publication.id_publication).ToListAsync();

                    var UrlYoutube = await _context.PublicationVideoEs.AsNoTracking().FirstOrDefaultAsync(x => x.fk_tbl_publication == publication.id_publication);

                    var Comentarios = await _context.CommentsEs.AsNoTracking().Where(x => x.fk_tbl_publication == publication.id_publication).ToListAsync();

                    var like = await _context.CommentsLikeEs.AsNoTracking().Where(x => x.fk_tbl_publication == publication.id_publication).ToListAsync();

                    var userlike = await _context.CommentsLikeEs.AsNoTracking().Where(x => x.fk_tbl_publication == publication.id_publication && x.fk_tbl_user == idUser).ToListAsync();

                    var Siguiendo = await _context.FollowersEs.AsNoTracking().Where(x => x.id_user == datosUsuario.id && x.id_follower == idUser).ToListAsync();

                    var coloniaPublication = await _context.ColoniaPublicationEs.AsNoTracking().FirstOrDefaultAsync(x => x.fk_tbl_publication == publication.id_publication);

                    List<MembersE> esMiembro = null;

                    if (coloniaPublication != null)
                    {
                         esMiembro = await _context.MembersEs.AsNoTracking().Where(x => x.fk_tbl_user_members == idUser && x.fk_tbl_colonies == coloniaPublication.fk_tbl_colonies).ToListAsync();
                    }

                    var todoCommentsList = new List<ComentarioPublication>();
                    foreach (var comments in Comentarios)
                    {
                        var ComentariosResonse = await _context.CommentsAnswercsEs.AsNoTracking().Where(x => x.fk_tbl_publication_comments == comments.id_publication_comments).ToListAsync();

                        var todoResponseList = new List<ComentarioRespuesta>();
                        foreach (var responseR in ComentariosResonse)
                        {
                            var datosUserCommentsR = await _context.UserEs.AsNoTracking().FirstOrDefaultAsync(x => x.id == responseR.fk_tbl_user);

                            var likeComentarioR = await _context.AnswerLikeEs.AsNoTracking().Where(x => x.fk_tbl_publication_comments_answer == responseR.id_publication_comments_answer).ToListAsync();

                            var userlikeComentarioR = await _context.AnswerLikeEs.AsNoTracking().Where(x => x.fk_tbl_publication_comments_answer == responseR.id_publication_comments_answer && x.fk_tbl_user == idUser).ToListAsync();

                            var response = new ComentarioRespuesta
                            {
                                IdComentarios = comments.id_publication_comments,
                                IdResponse = responseR.id_publication_comments_answer,
                                IdPerfilComentarios = datosUserCommentsR.id,
                                FechaComentario = responseR.dt_creation.ToString("yyyy-MM-dd HH:mm:ss"),
                                NombrePerfilComentarios = datosUserCommentsR.s_user_name,
                                urlPerfil = datosUserCommentsR.s_userProfile,
                                ImagenPerfilComentarios = datosUserCommentsR.s_userPhoto,
                                Comentario = responseR.s_answer,
                                megustaComentarios = likeComentarioR.Count,
                                userLike = userlikeComentarioR.Count,
                            };

                            todoResponseList.Add(response);
                        }

                        var datosUserComments = await _context.UserEs.AsNoTracking().FirstOrDefaultAsync(x => x.id == comments.fk_tbl_user);

                        var imagenComment = await _context.CommentsImagenEs.AsNoTracking().FirstOrDefaultAsync(x => x.fk_tbl__publication_comments == comments.id_publication_comments);

                        var likeComentario = await _context.LikeCommentsEs.AsNoTracking().Where(x => x.fk_tbl_publication_comments == comments.id_publication_comments).ToListAsync();

                        var userlikeComentario = await _context.LikeCommentsEs.AsNoTracking().Where(x => x.fk_tbl_publication_comments == comments.id_publication_comments && x.fk_tbl_user == idUser).ToListAsync();

                        var comentarios = new ComentarioPublication
                        {
                            IdComentarios = comments.id_publication_comments,
                            IdPerfilComentarios = datosUserComments.id,
                            FechaComentario = comments.dt_creation.ToString("yyyy-MM-dd HH:mm:ss"),
                            NombrePerfilComentarios = datosUserComments.s_user_name,
                            urlPerfil = datosUserComments.s_userProfile,
                            ImagenPerfilComentarios = datosUserComments.s_userPhoto,
                            Comentario = comments.s_comments,
                            imagenComentario = (imagenComment != null && !string.IsNullOrEmpty(imagenComment.s_location)) ? imagenComment.s_location : "" ,
                            megustaComentarios = likeComentario.Count,
                            userLike = userlikeComentario.Count,
                            ComentariosRespuesta = todoResponseList
                        };

                        todoCommentsList.Add(comentarios);
                    }

                    var colonia = new List<ColoniasUserDTOs>();
                    if (publication.fk_tbl_type_publication == 6)
                    {
                        var IdColoniaShare = await _context.PublicationShareEs.AsNoTracking().FirstOrDefaultAsync(x => x.fk_tbl_publication == publication.id_publication);

                        var InfoColonia = await _context.ColoniaEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_colonies == IdColoniaShare.fk_tbl_colonies);

                        var list = new ColoniasUserDTOs
                        {
                            id_colonies = InfoColonia.id_colonies,
                            s_name = InfoColonia.s_name,
                            s_photo = InfoColonia.s_photo,
                            colors = InfoColonia.s_colors
                        };
                        colonia.Add(list);
                    }


                    var publicacion = new TodoPublicationDTOs
                    {
                        IdPerfil = datosUsuario.id,
                        NombrePerfil = datosUsuario.s_user_name,
                        urlPerfil = datosUsuario.s_userProfile,
                        ImagenPerfil = datosUsuario.s_userPhoto,
                        userLike = userlike.Count,
                        level = datosUsuario.fk_tbl_level,
                        IdPublicacion = publication.id_publication,
                        IdTipo = publication.fk_tbl_type_publication,
                        IdColonia = (coloniaPublication != null) ? coloniaPublication.fk_tbl_colonies : 0,
                        esMiembroColonia = (esMiembro != null) ? esMiembro.Count : 0,
                        Megustas = like.Count,
                        CantidadComentarios = Comentarios.Count,
                        Siguiendo =  Siguiendo.Count,
                        FechaPublicacion = publication.dt_creation.ToString("yyyy-MM-dd HH:mm:ss"),
                        Titulo = publication.s_title,
                        Contenido = publication.s_content,
                        UrlYoutube = (UrlYoutube != null && !string.IsNullOrEmpty(UrlYoutube.s_url)) ? UrlYoutube.s_url : "",
                        ImagenesPublicacion = imagenesPublications.Select(ip => ip.s_location).ToList(),
                        Comentarios = todoCommentsList,
                        InfoColonia = colonia
                    };
                    
                    todoPublicationsList.Add(publicacion);
                }

                return todoPublicationsList;
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar PublicationQueries.TodoPublication");
                throw;
            }
        }

        public List<HashtagSearchResultDTOs> SearchHashtags(string searchTerm)
        {
            _logger.LogTrace("Iniciando metodo PublicationQueries.SearchHashtags...");
            try
            {

                // Convertir la cadena de búsqueda a minúsculas
                searchTerm = searchTerm.ToLower();

                // Obtener todas las publicaciones que contienen la cadena de búsqueda en sus hashtags
                var matchingPublications = _context.PublicationEs
                    .Where(p => p.s_hashtags != null && p.s_hashtags.ToLower().Contains(searchTerm))
                    .ToList();

                // Crear un diccionario para almacenar los hashtags y sus recuentos
                var hashtagCounts = new Dictionary<string, int>();

                // Iterar a través de las publicaciones coincidentes y contar los hashtags
                foreach (var publication in matchingPublications)
                {
                    if (!string.IsNullOrEmpty(publication.s_hashtags))
                    {
                        // Dividir los hashtags de la publicación
                        var hashtags = publication.s_hashtags.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                        // Iterar a través de los hashtags y contarlos
                        foreach (var hashtag in hashtags)
                        {
                            var lowercaseHashtag = hashtag.ToLower();
                            if (lowercaseHashtag.Contains(searchTerm))
                            {
                                if (hashtagCounts.ContainsKey(lowercaseHashtag))
                                {
                                    // Incrementar el recuento del hashtag si ya está en el diccionario
                                    hashtagCounts[lowercaseHashtag]++;
                                }
                                else
                                {
                                    // Agregar el hashtag al diccionario si no está presente
                                    hashtagCounts.Add(lowercaseHashtag, 1);
                                }
                            }
                        }
                    }
                }

                // Convertir el diccionario en una lista de resultados
                var results = hashtagCounts.Select(kv => new HashtagSearchResultDTOs
                {
                    Hashtag = kv.Key,
                    Count = kv.Value
                }).ToList();

                return results;

            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar PublicationQueries.SearchHashtags");
                throw;
            }
           
        }

        public List<HashtagSearchResultDTOs> TopHashtags()
        {
            _logger.LogTrace("Iniciando metodo PublicationQueries.GetTopHashtags...");
            try
            {
                // Obtener todas las publicaciones con hashtags
                var allPublications = _context.PublicationEs
                    .Where(p => p.s_hashtags != null)
                    .ToList();

                // Crear un diccionario para almacenar los hashtags y sus recuentos
                var hashtagCounts = new Dictionary<string, int>();

                // Iterar a través de todas las publicaciones y contar los hashtags
                foreach (var publication in allPublications)
                {
                    if (!string.IsNullOrEmpty(publication.s_hashtags))
                    {
                        // Dividir los hashtags de la publicación
                        var hashtags = publication.s_hashtags.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                        // Iterar a través de los hashtags y contarlos
                        foreach (var hashtag in hashtags)
                        {
                            var lowercaseHashtag = hashtag.ToLower();
                            if (hashtagCounts.ContainsKey(lowercaseHashtag))
                            {
                                // Incrementar el recuento del hashtag si ya está en el diccionario
                                hashtagCounts[lowercaseHashtag]++;
                            }
                            else
                            {
                                // Agregar el hashtag al diccionario si no está presente
                                hashtagCounts.Add(lowercaseHashtag, 1);
                            }
                        }
                    }
                }

                // Ordenar los hashtags por recuento en orden descendente y tomar los 10 primeros
                var topHashtags = hashtagCounts
                    .OrderByDescending(kv => kv.Value)
                    .Take(6)
                    .Select(kv => new HashtagSearchResultDTOs
                    {
                        Hashtag = kv.Key,
                        Count = kv.Value
                    })
                    .ToList();

                return topHashtags;

            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar PublicationQueries.GetTopHashtags");
                throw;
            }
        }


        public async Task<List<FilterPublicationDTOs>> BuscarPublications(string searchTerm, int topCount)
        {
            _logger.LogTrace("Iniciando metodo PublicationQueries.BuscarPublications...");
            try
            {
                // Convertir la cadena de búsqueda a minúsculas
                searchTerm = searchTerm.ToLower();

                // Buscar las publicaciones que coincidan con s_title o s_content y contar la cantidad de coincidencias
                var matchingPublications = _context.PublicationEs
                    .Where(p =>
                        p.s_title != null && p.s_title.ToLower().Contains(searchTerm) ||
                        p.s_content != null && p.s_content.ToLower().Contains(searchTerm))
                    .ToList();

                // Ordenar las publicaciones por la cantidad de coincidencias y tomar las 10 principales
                var topPublications = matchingPublications
                    .OrderByDescending(p =>
                        CountOccurrences(p.s_title.ToLower(), searchTerm) +
                        CountOccurrences(p.s_content.ToLower(), searchTerm))
                    .Take(topCount)
                    .ToList();


                var publicationList = new List<FilterPublicationDTOs>();


                foreach (var publication in topPublications)
                {

                    var datosUsuario = await _context.UserEs.AsNoTracking().FirstOrDefaultAsync(x => x.id == publication.fk_tbl_user);
                    var list = new FilterPublicationDTOs
                    {

                        NombrePerfil = datosUsuario.s_user_name,
                        ImagenPerfil = datosUsuario.s_userPhoto,
                        IdPublicacion = publication.id_publication,
                        IdTipo = publication.fk_tbl_type_publication,
                        FechaPublicacion = publication.dt_creation,
                        Titulo = publication.s_title,
                        Contenido = publication.s_content != null ?
                                    (publication.s_content.Length > 100 ? publication.s_content.Substring(0, 200) : publication.s_content) :
                                    null

                    };

                    publicationList.Add(list);

                }

                return publicationList;
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar PublicationQueries.BuscarPublications");
                throw;
            }
            
        }

        public async Task<List<FilterPublicationDTOs>> BuscarPublicationSimilares(string searchTerm, int topCount)
        {
            _logger.LogTrace("Iniciando metodo PublicationQueries.BuscarPublications...");
            try
            {
                // Convertir la cadena de búsqueda a minúsculas
                searchTerm = searchTerm.ToLower();

                // Obtener todas las publicaciones
                var allPublications = await _context.PublicationEs.ToListAsync();

                // Calcular la similitud de coseno entre la cadena de búsqueda y el título o contenido de cada publicación
                var similarPublications = allPublications
                    .Select(p => new
                    {
                        Publication = p,
                        Similarity = CalculateCosineSimilarity(searchTerm, $"{p.s_title} ")
                    })
                    .OrderByDescending(x => x.Similarity)
                    .Take(topCount)
                    .ToList();

                var publicationList = new List<FilterPublicationDTOs>();

                foreach (var item in similarPublications)
                {
                    var publication = item.Publication;

                    var datosUsuario = await _context.UserEs.AsNoTracking().FirstOrDefaultAsync(x => x.id == publication.fk_tbl_user);
                    var list = new FilterPublicationDTOs
                    {
                        NombrePerfil = datosUsuario.s_user_name,
                        ImagenPerfil = datosUsuario.s_userPhoto,
                        IdPublicacion = publication.id_publication,
                        IdTipo = publication.fk_tbl_type_publication,
                        FechaPublicacion = publication.dt_creation,
                        Titulo = publication.s_title,
                        Contenido = publication.s_content != null ?
                                        (publication.s_content.Length > 100 ? publication.s_content.Substring(0, 200) : publication.s_content) :
                                        null
                    };

                    publicationList.Add(list);
                }

                return publicationList;
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar PublicationQueries.BuscarPublications");
                throw;
            }
        }

        private double CalculateCosineSimilarity(string searchTerm, string text)
        {
            // Implementa la lógica para calcular la similitud de coseno entre searchTerm y text
            // Puedes usar bibliotecas existentes o implementar tu propio cálculo
            // Aquí hay un ejemplo simple de implementación:
            var searchTermVector = searchTerm.Split(' ').ToHashSet();
            var textVector = text.Split(' ').ToHashSet();

            var intersection = searchTermVector.Intersect(textVector).Count();
            var cosineSimilarity = intersection / (Math.Sqrt(searchTermVector.Count) * Math.Sqrt(textVector.Count));

            return cosineSimilarity;
        }


        public async Task<List<TopPublicationDTOs>> TopPublications(int tipo)
        {
            _logger.LogTrace("Iniciando metodo PublicationQueries.TopPublications...");
            try
            {
                var result = await (
                                     from publication in _context.PublicationEs
                                     join like in _context.CommentsLikeEs
                                         on publication.id_publication equals like.fk_tbl_publication
                                     where (tipo == 0 || publication.fk_tbl_type_publication == tipo) && publication.byte_blocked == 0
                                     group like by publication into groupedLikes
                                     orderby groupedLikes.Count() descending
                                     select new
                                     {
                                         Publication = groupedLikes.Key,
                                         Likes = groupedLikes.Count()
                                     }
                                 ).Take(5).ToListAsync();


                var publicationList = new List<TopPublicationDTOs>();

                foreach (var publication in result)
                {
                    var datosUsuario = await _context.UserEs.AsNoTracking().FirstOrDefaultAsync(x => x.id == publication.Publication.fk_tbl_user);
                    var list = new TopPublicationDTOs
                    {

                        IdPublicacion = publication.Publication.id_publication,
                        FechaPublicacion = publication.Publication.dt_creation,
                        Titulo = publication.Publication.s_title,
                        Likes = publication.Likes,
                        IdUser = datosUsuario.id,
                        NombrePerfil = datosUsuario.s_user_name,
                        Foto = datosUsuario.s_userPhoto,
                        Url = datosUsuario.s_userProfile

                    };

                    publicationList.Add(list);

                }

                return publicationList;
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar PublicationQueries.TopPublications");
                throw;
            }

        }


        public async Task<List<PerfilImagenesDTOs>> ImagenesPublications(int tipo)
        {
            _logger.LogTrace("Iniciando metodo PublicationQueries.ImagenesPublications...");
            try
            {
                var Todopublications = await _context.PublicationEs
                .AsNoTracking()
                .Where(x => x.fk_tbl_type_publication == tipo && x.byte_blocked == 0)
                .OrderByDescending(x => x.dt_creation)
                .Take(100)
                .ToListAsync();

                            var imagenesPublications = await _context.PublicationImageEs
                                .AsNoTracking()
                                .Where(x => Todopublications.Select(p => p.id_publication).Contains(x.fk_tbl_publication))
                                .OrderByDescending(x => x.date_creation)
                                .ToListAsync();

                var imagenesList = new List<PerfilImagenesDTOs>();

                Random random = new Random();
                imagenesPublications = imagenesPublications.OrderBy(x => random.Next()).ToList();

                foreach (var imagenes in imagenesPublications)
                    {
                        var perfilImagenesDTOs = new PerfilImagenesDTOs
                        {
                            idPublicacion = imagenes.fk_tbl_publication,
                            url = imagenes.s_location,
                        };

                        imagenesList.Add(perfilImagenesDTOs);
                    }

                return imagenesList;
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar PublicationQueries.ImagenesPublications");
                throw;
            }

        }

        private int CountOccurrences(string text, string keyword)
        {
            int count = 0;
            int index = text.IndexOf(keyword);

            while (index != -1)
            {
                count++;
                index = text.IndexOf(keyword, index + 1);
            }

            return count;
        }

        public async Task<List<Publication_reporting_reasonDTOs>> Publication_reporting_reason()
        {
            _logger.LogTrace("Iniciando metodo LoginQueries.ConsultarUsuarioAutorizado...");
            try
            {
                var Publication_reporting_reason = _context.Publication_reporting_reasonEs.AsNoTracking().Where(x => x.byte_activo == 1).ToList();

                var rasonList = new List<Publication_reporting_reasonDTOs>();

                foreach (var rason in Publication_reporting_reason)
                {
                    var list = new Publication_reporting_reasonDTOs
                    {
                        id_publication_reporting_reason = rason.id_publication_reporting_reason,
                        s_title = rason.s_title,
                        s_description = rason.s_description,
                        byte_activo = rason.byte_activo
                    };
                    rasonList.Add(list);
                }

                return rasonList;
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar PublicationQueries.BuscarPublications");
                throw;
            }
        }
    }


}

