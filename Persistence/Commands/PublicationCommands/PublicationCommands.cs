
using Antopia.Domain.DTOs.ColinaDTOs;
using Antopia.Domain.DTOs.NotificacionDTOs;
using Antopia.Domain.DTOs.PublicationDTOs;
using Antopia.Domain.DTOs.RespuestaDTOs;
using Antopia.Domain.Entities.ColoniaE;
using Antopia.Domain.Entities.NotificacionE;
using Antopia.Domain.Entities.PublicacionE;
using Antopia.Infrastructure;
using Antopia.Persistence.Commands.NotificacionCommands;
using Antopia.Persistence.ImageService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Antopia.Persistence.Commands.PublicationCommands
{
    public interface IPublicationCommands
    {
        Task<Respuestas> InsertarPublication(ListPublicationDTOs listPublicationDTOs);
        Task<Respuestas> InsertarComentarios(CommentsDTOs commentsDTOs);
        Task<Respuestas> like(CommentsLikeDTOs CommentsLikeDTOs);
        Task<Respuestas> DeletePublication(int idPublication);
        Task<Respuestas> PublicationReportin(PublicationReportingDTOs publicationReportingDTOs);
        Task<Respuestas> likeComments(LikeCommentsDTOs likeCommentsDTOs);
        Task<Respuestas> likeAnswers(AnswerLikeDTOs answerLikeDTOs);
        Task<Respuestas> InsertarRespuesta(CommentsAnswercsDTOs commentsAnswercsDTOs);
    }

    public class PublicationCommands : IPublicationCommands, IDisposable
    {
        private readonly AntopiaDbContext _context = null;
        private readonly ILogger<PublicationCommands> _logger;
        private readonly IConfiguration _configuration;
        private readonly IImageService _imageService;
        private readonly INotificacionCommands _notificacionCommands;
        

        public PublicationCommands(ILogger<PublicationCommands> logger, IConfiguration configuration, IImageService imageService, INotificacionCommands notificacionCommands)
        {
            _logger = logger;
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection");
            _context = new AntopiaDbContext(connectionString);
            _imageService = imageService;
            _notificacionCommands = notificacionCommands;
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


        public async Task<Respuestas> InsertarPublication(ListPublicationDTOs listPublicationDTOs)
        {
            _logger.LogTrace("Iniciando metodo PublicationCommands.InsertarPublication...");
            try
            {
                var publication = new PublicationDTOs
                {
                    dt_creation = DateTime.UtcNow,
                    s_title = listPublicationDTOs.Publicaciones[0].s_title,
                    s_content = listPublicationDTOs.Publicaciones[0].s_content,
                    fk_tbl_user = listPublicationDTOs.Publicaciones[0].fk_tbl_user,
                    fk_tbl_type_publication = listPublicationDTOs.Publicaciones[0].fk_tbl_type_publication,
                    s_hashtags = listPublicationDTOs.Publicaciones[0].s_hashtags,
                };

                var publicacionE = PublicationDTOs.CreateE(publication);
                await _context.PublicationEs.AddAsync(publicacionE);
                await _context.SaveChangesAsync();

                if (publicacionE.fk_tbl_type_publication == 6)
                {
                    var publicationShareDTOs = new PublicationShareDTOs
                    {
                        fk_tbl_publication = publicacionE.id_publication,
                        fk_tbl_colonies = listPublicationDTOs.Publicaciones[0].fk_tbl_colinie,
                    };

                    var coloniaPublication = PublicationShareDTOs.CreateE(publicationShareDTOs);
                    await _context.PublicationShareEs.AddAsync(coloniaPublication);
                    await _context.SaveChangesAsync();
                }

                if (publicacionE.fk_tbl_type_publication == 5)
                {
                    var coloniaPublicationDTOs = new ColoniaPublicationDTOs
                    {
                        fk_tbl_publication = publicacionE.id_publication,
                        fk_tbl_colonies = listPublicationDTOs.Publicaciones[0].fk_tbl_colinie,
                    };

                    var coloniaPublication = ColoniaPublicationDTOs.CreateE(coloniaPublicationDTOs);
                    await _context.ColoniaPublicationEs.AddAsync(coloniaPublication);
                    await _context.SaveChangesAsync();

                    var memberColonies = await _context.MembersEs.AsNoTracking().Where(x => x.fk_tbl_colonies == listPublicationDTOs.Publicaciones[0].fk_tbl_colinie).ToListAsync();


                    var points = _context.ColoniaEs.FirstOrDefault(x => x.id_colonies == listPublicationDTOs.Publicaciones[0].fk_tbl_colinie);
                    points.points = points.points + 10;
                    await _context.SaveChangesAsync();

                    foreach (var memberList in memberColonies)
                    {
                        var notificacionDTOS = new NotificacionDTOs
                        {
                            id_notification = 0,
                            type_notification = 8,
                            for_user = memberList.fk_tbl_user_members,
                            of_user = listPublicationDTOs.Publicaciones[0].fk_tbl_user,
                            data_created = DateTime.UtcNow,
                            state = false,
                            fk_tbl_publication = publicacionE.id_publication,
                            fk_tbl_diary = 0,
                            fk_tbl_colonie = listPublicationDTOs.Publicaciones[0].fk_tbl_colinie,
                        };

                        bool notificacion = await _notificacionCommands.RegistrarNotificacion(notificacionDTOS);

                    }
                   
                }

                if (publicacionE.id_publication != 0)
                {
                    if (listPublicationDTOs.Imagenes != null && listPublicationDTOs.Imagenes.Count > 0)
                    {
                        foreach (var imagen in listPublicationDTOs.Imagenes)
                        {
                            string rutaImagen = "";
                          
                            if (publicacionE.fk_tbl_type_publication == 5)
                            {
                                rutaImagen = "wwwroot/ImagesColoniaPublication";
                            }
                            else
                            {
                                rutaImagen = "wwwroot/ImagesPublication";
                            }

                            var location = await _imageService.SaveImageAsync(imagen.s_location, rutaImagen);
                            var imagenes = new PublicationImageDTOs
                            {
                                date_creation = DateTime.UtcNow,
                                fk_tbl_publication = publicacionE.id_publication,
                                s_location = location,
                                fk_tbl_user = listPublicationDTOs.Publicaciones[0].fk_tbl_user,
                            };

                            var publicationImageE = PublicationImageDTOs.CreateE(imagenes);
                            await _context.PublicationImageEs.AddAsync(publicationImageE);
                            await _context.SaveChangesAsync();
                        }
                    }

                    if (listPublicationDTOs.Videos != null && listPublicationDTOs.Videos.Count > 0)
                    {
                        foreach (var video in listPublicationDTOs.Videos)
                        {
                            var videos = new PublicationVideoDTOs
                            {
                                date_creation = DateTime.UtcNow,
                                fk_tbl_publication = publicacionE.id_publication,
                                s_url = video.s_url
                            };

                            var publicationVideoE = PublicationVideoDTOs.CreateE(videos);
                            await _context.PublicationVideoEs.AddAsync(publicationVideoE);
                            await _context.SaveChangesAsync();
                        }
                    }

                    return new Respuestas
                    {
                        resultado = true,
                        message = "¡Publicación  satisfactoria!",
                    };
                }
                else
                {
                    return new Respuestas
                    {
                        resultado = false,
                        message = "¡Error al publicar!",
                    };
                }
              
            }
            catch (Exception)
            {
                _logger.LogError("Error en el metodo PublicationCommands.InsertarPublication...");
                throw;
            }
          
        }

        public async Task<Respuestas> InsertarComentarios(CommentsDTOs commentsDTOs)
        {
            _logger.LogTrace("Iniciando metodo PublicationCommands.InsertarComentarios...");
            try
            {
                var comentarios = new CommentsDTOs
                {
                    id_publication_comments = commentsDTOs.id_publication_comments,
                    fk_tbl_publication = commentsDTOs.fk_tbl_publication,
                    fk_tbl_user = commentsDTOs.fk_tbl_user,
                    s_comments = commentsDTOs.s_comments,
                    dt_creation = DateTime.UtcNow,
                };

                var comentariosE = CommentsDTOs.CreateE(comentarios);
                await _context.CommentsEs.AddAsync(comentariosE);
                await _context.SaveChangesAsync();

                if (comentariosE.id_publication_comments != 0 )
                {
                    if (commentsDTOs.Imagen != "")
                    {
                        string rutaImagen = "wwwroot/ImagesComments";
                        var location = await _imageService.SaveImageAsync(commentsDTOs.Imagen, rutaImagen);

                        var Imagencomentarios = new CommentsImagenDTOs
                        {
                            fk_tbl__publication_comments = comentariosE.id_publication_comments,
                            s_location = location,
                        };

                        var ImagencomentariosE = CommentsImagenDTOs.CreateE(Imagencomentarios);
                        await _context.CommentsImagenEs.AddAsync(ImagencomentariosE);
                        await _context.SaveChangesAsync();
                    }

                    var idUserpublications = _context.PublicationEs.Where(x => x.id_publication == commentsDTOs.fk_tbl_publication).FirstOrDefault().fk_tbl_user;

                    if (idUserpublications != commentsDTOs.fk_tbl_user)
                    {
                        var notificacionDTOS = new NotificacionDTOs
                        {
                            id_notification = 0,
                            type_notification = 2,
                            for_user = idUserpublications,
                            of_user = commentsDTOs.fk_tbl_user,
                            data_created = DateTime.UtcNow,
                            state = false,
                            fk_tbl_publication = commentsDTOs.fk_tbl_publication,
                            fk_tbl_diary = 0,
                        };

                        bool notificacion = await _notificacionCommands.RegistrarNotificacion(notificacionDTOS);
                    }

                    // Crear un HashSet para almacenar usuarios únicos a notificar
                    var usuariosNotificados = new HashSet<int>();

                    var listUserCometarios = await _context.CommentsEs
                        .AsNoTracking()
                        .Where(x => x.fk_tbl_publication == commentsDTOs.fk_tbl_publication)
                        .ToListAsync();

                    foreach (var memberList in listUserCometarios)
                    {

                        if (idUserpublications != memberList.fk_tbl_user && !usuariosNotificados.Contains(memberList.fk_tbl_user))
                        {
                            var notificacionDTOS = new NotificacionDTOs
                            {
                                id_notification = 0,
                                type_notification = 10,
                                for_user = memberList.fk_tbl_user,
                                of_user = commentsDTOs.fk_tbl_user,
                                data_created = DateTime.UtcNow,
                                state = false,
                                fk_tbl_publication = commentsDTOs.fk_tbl_publication,
                                fk_tbl_diary = 0,
                                fk_tbl_colonie = 0,
                            };

                            bool notificacion = await _notificacionCommands.RegistrarNotificacion(notificacionDTOS);

                            usuariosNotificados.Add(memberList.fk_tbl_user);
                        }
                    }

                   var resultado =  _context.ColoniaPublicationEs.Where(x => x.fk_tbl_publication == commentsDTOs.fk_tbl_publication).FirstOrDefault();


                    if (resultado != null)
                    {
                        var idcolonia = resultado.fk_tbl_colonies;
                        var points = _context.ColoniaEs.FirstOrDefault(x => x.id_colonies == idcolonia);
                        points.points = points.points + 3;
                        await _context.SaveChangesAsync();
                    }

                    return new Respuestas
                    {
                        resultado = true,
                        message = "¡Comentario agregado!",
                    };
                }else
                {
                    return new Respuestas
                    {
                        resultado = false,
                        message = "¡El comentario no agregado!",
                    };
                }

            }
            catch (Exception)
            {
                _logger.LogError("Error en el metodo PublicationCommands.InsertarComentarios...");
                throw;
            }
        }

        public async Task<Respuestas> InsertarRespuesta(CommentsAnswercsDTOs commentsAnswercsDTOs)
        {
            _logger.LogTrace("Iniciando metodo PublicationCommands.InsertarRespuesta...");
            try
            {
                var comentariosE = CommentsAnswercsDTOs.CreateE(commentsAnswercsDTOs);
                await _context.CommentsAnswercsEs.AddAsync(comentariosE);
                await _context.SaveChangesAsync();

                if (comentariosE.id_publication_comments_answer != 0) {

                    var idComentario = await _context.CommentsEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_publication_comments == commentsAnswercsDTOs.fk_tbl_publication_comments);

                    var idPublication = await _context.PublicationEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_publication == idComentario.fk_tbl_publication);

                    var notificacionDTOS = new NotificacionDTOs
                    {
                        id_notification = 0,
                        type_notification = 5,
                        for_user = idComentario.fk_tbl_user,
                        of_user = commentsAnswercsDTOs.fk_tbl_user,
                        data_created = DateTime.UtcNow,
                        state = false,
                        fk_tbl_publication = idPublication.id_publication,
                        fk_tbl_diary = 0,
                    };

                    bool notificacion = await _notificacionCommands.RegistrarNotificacion(notificacionDTOS);

                   
                    var resultado = _context.ColoniaPublicationEs.Where(x => x.fk_tbl_publication == idPublication.id_publication).FirstOrDefault();


                    if (resultado != null)
                    {
                        var idcolonia = resultado.fk_tbl_colonies;
                        var points = _context.ColoniaEs.FirstOrDefault(x => x.id_colonies == idcolonia);
                        points.points = points.points + 3;
                        await _context.SaveChangesAsync();
                    }

                    return new Respuestas
                    {
                        resultado = true,
                        message = "¡Respuesta agregado!",
                    };
                }
                else
                {
                    return new Respuestas
                    {
                        resultado = false,
                        message = "¡La respuesta no fue no agregada!",
                    };
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error en el metodo PublicationCommands.InsertarRespuesta...");
                throw;
            }
        }

        public async Task<Respuestas> like(CommentsLikeDTOs CommentsLikeDTOs)
        {

            _logger.LogTrace("Iniciando metodo PublicationCommands.like...");
            try
            {
                if (CommentsLikeDTOs.is_like == 0)
                {
                    var CommentsLikeE = CommentsLikeDTOs.CreateE(CommentsLikeDTOs);
                    await _context.CommentsLikeEs.AddAsync(CommentsLikeE);
                    await _context.SaveChangesAsync();
                    if (CommentsLikeE.id_publication_like != 0)
                    {
                        var idUserpublications = _context.PublicationEs.Where(x => x.id_publication == CommentsLikeDTOs.fk_tbl_publication).FirstOrDefault().fk_tbl_user;

                        if (idUserpublications != CommentsLikeDTOs.fk_tbl_user)
                        {
                            var notificacionDTOS = new NotificacionDTOs
                            {
                                id_notification = 0,
                                type_notification = 1,
                                for_user = idUserpublications,
                                of_user = CommentsLikeDTOs.fk_tbl_user,
                                data_created = DateTime.UtcNow,
                                state = false,
                                fk_tbl_publication = CommentsLikeDTOs.fk_tbl_publication,
                                fk_tbl_diary = 0,
                            };

                            bool notificacion = await _notificacionCommands.RegistrarNotificacion(notificacionDTOS);
                        }

                        var resultado = _context.ColoniaPublicationEs.Where(x => x.fk_tbl_publication == CommentsLikeDTOs.fk_tbl_publication).FirstOrDefault();


                        if (resultado != null)
                        {
                            var idcolonia = resultado.fk_tbl_colonies;
                            var points = _context.ColoniaEs.FirstOrDefault(x => x.id_colonies == idcolonia);
                            points.points = points.points + 2;
                            await _context.SaveChangesAsync();
                        }

                        return new Respuestas
                        {
                            resultado = true,
                            message = "¡like exitoso!",
                        };
                    }
                    else
                    {
                        return new Respuestas
                        {
                            resultado = false,
                            message = "¡like no exitoso!",
                        };
                    }
                }
                else {
                    var likesAEliminar = await _context.CommentsLikeEs.AsNoTracking().FirstOrDefaultAsync(x => x.fk_tbl_user == CommentsLikeDTOs.fk_tbl_user && x.fk_tbl_publication == CommentsLikeDTOs.fk_tbl_publication);
                    _context.CommentsLikeEs.Remove(likesAEliminar);
                    _context.SaveChanges();
                    if (likesAEliminar.id_publication_like != 0)
                    {

                        var resultado = _context.ColoniaPublicationEs.Where(x => x.fk_tbl_publication == CommentsLikeDTOs.fk_tbl_publication).FirstOrDefault();


                        if (resultado != null)
                        {
                            var idcolonia = resultado.fk_tbl_colonies;
                            var points = _context.ColoniaEs.FirstOrDefault(x => x.id_colonies == idcolonia);
                            points.points = points.points - 2;
                            await _context.SaveChangesAsync();
                        }

                        return new Respuestas
                        {
                            resultado = true,
                            message = "¡dislike exitoso!",
                        };
                    }
                    else
                    {
                        return new Respuestas
                        {
                            resultado = false,
                            message = "¡dislike no exitoso!",
                        };
                    }

                }
            }
            catch (Exception)
            {
                _logger.LogError("Error en el metodo PublicationCommands.like...");
                throw;
            }

        }

        public async Task<Respuestas> DeletePublication(int idPublication)
        {

            _logger.LogTrace("Iniciando metodo PublicationCommands.DeletePublication...");
            try
            {
                 var EliminarPublicacion = await _context.PublicationEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_publication == idPublication);


                if (EliminarPublicacion.id_publication != 0)
                {
                    var EliminarComentario = _context.CommentsEs.AsNoTracking().Where(x => x.fk_tbl_publication == idPublication).ToList();

                    var EliminarLike = _context.CommentsLikeEs.AsNoTracking().Where(x => x.fk_tbl_publication == idPublication).ToList();

                    var EliminarImagen = _context.PublicationImageEs.AsNoTracking().Where(x => x.fk_tbl_publication == idPublication).ToList();


                    if (EliminarComentario.Count > 0)
                    {
                        foreach (var comment in EliminarComentario)
                        {
                            _context.CommentsEs.Remove(comment);
                            _context.SaveChanges();
                        }
                    }

                    if (EliminarLike.Count > 0)
                    {
                        foreach (var like in EliminarLike)
                        {
                            _context.CommentsLikeEs.Remove(like);
                            _context.SaveChanges();
                        }
                    }

                    if (EliminarImagen.Count > 0)
                    {
                        foreach (var imagen in EliminarImagen)
                        {
                            var location = await _imageService.DeleteImage(imagen.s_location);
                            _context.PublicationImageEs.Remove(imagen);
                            _context.SaveChanges();
                        }
                    }

                    _context.PublicationEs.Remove(EliminarPublicacion);
                    _context.SaveChanges();

                    return new Respuestas
                    {
                        resultado = true,
                        message = "¡Publicación liminada exitosamente!",
                    };
                }
                else
                {
                    return new Respuestas
                    {
                        resultado = false,
                        message = "¡Problemas al eliminar la publicación!",
                    };
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error en el metodo PublicationCommands.DeletePublication...");
                throw;
            }
        }


        public async Task<Respuestas> PublicationReportin(PublicationReportingDTOs publicationReportingDTOs)
        {
            _logger.LogTrace("Iniciando metodo PublicationCommands.PublicationReportin...");
            try
            {
                var PublicationReportinE = PublicationReportingDTOs.CreateE(publicationReportingDTOs);
                await _context.PublicationReportingEs.AddAsync(PublicationReportinE);
                await _context.SaveChangesAsync();

                

                if (PublicationReportinE.id_publication_reporting > 0)
                {

                    var publication = _context.PublicationEs.Find(publicationReportingDTOs.fk_tbl_publication);

                    if (publication != null)
                    {
                        publication.byte_blocked = 1;
                        _context.SaveChanges(); // 
                    }

                    return new Respuestas
                    {
                        resultado = true,
                        message = "¡Publicación reportada exitosamente!",
                    };
                }
                else
                {
                    return new Respuestas
                    {
                        resultado = false,
                        message = "¡No se puedo reportar la publicación, intentalo mas tarde!",
                    };
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error en el metodo PublicationCommands.PublicationReportin...");
                throw;
            }
        
        }

        public async Task<Respuestas> likeComments(LikeCommentsDTOs likeCommentsDTOs)
        {

            _logger.LogTrace("Iniciando metodo PublicationCommands.like...");
            try
            {
                if (likeCommentsDTOs.is_like == 0)
                {
                    var CommentsLikeE = LikeCommentsDTOs.CreateE(likeCommentsDTOs);
                    await _context.LikeCommentsEs.AddAsync(CommentsLikeE);
                    await _context.SaveChangesAsync();
                    if (CommentsLikeE.id_publication_comments_like != 0)
                    {
                        
                        var idComentario =  await _context.CommentsEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_publication_comments == likeCommentsDTOs.fk_tbl_publication_comments);

                        var idPublication = await _context.PublicationEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_publication == idComentario.fk_tbl_publication);

                        if (idComentario.fk_tbl_user  != likeCommentsDTOs.fk_tbl_user)
                        {
                            var notificacionDTOS = new NotificacionDTOs
                            {
                                id_notification = 0,
                                type_notification = 3,
                                for_user = idComentario.fk_tbl_user,
                                of_user = likeCommentsDTOs.fk_tbl_user,
                                data_created = DateTime.UtcNow,
                                state = false,
                                fk_tbl_publication = idPublication.id_publication,
                                fk_tbl_diary = 0,
                            };

                            bool notificacion = await _notificacionCommands.RegistrarNotificacion(notificacionDTOS);

                        }

                        var resultado = _context.ColoniaPublicationEs.Where(x => x.fk_tbl_publication == idPublication.id_publication).FirstOrDefault();


                        if (resultado != null)
                        {
                            var idcolonia = resultado.fk_tbl_colonies;
                            var points = _context.ColoniaEs.FirstOrDefault(x => x.id_colonies == idcolonia);
                            points.points = points.points + 1;
                            await _context.SaveChangesAsync();
                        }

                        return new Respuestas
                        {
                            resultado = true,
                            message = "¡like exitoso!",
                        };
                    }
                    else
                    {
                        return new Respuestas
                        {
                            resultado = false,
                            message = "¡like no exitoso!",
                        };
                    }
                }
                else
                {
                    var likesAEliminar = await _context.LikeCommentsEs.AsNoTracking().FirstOrDefaultAsync(x => x.fk_tbl_user == likeCommentsDTOs.fk_tbl_user && x.fk_tbl_publication_comments == likeCommentsDTOs.fk_tbl_publication_comments);

                    _context.LikeCommentsEs.Remove(likesAEliminar);
                    _context.SaveChanges();
                    if (likesAEliminar.id_publication_comments_like != 0)
                    {

                        var idComentario = await _context.CommentsEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_publication_comments == likeCommentsDTOs.fk_tbl_publication_comments);

                        var idPublication = await _context.PublicationEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_publication == idComentario.fk_tbl_publication);


                        var resultado = _context.ColoniaPublicationEs.Where(x => x.fk_tbl_publication == idPublication.id_publication).FirstOrDefault();


                        if (resultado != null)
                        {
                            var idcolonia = resultado.fk_tbl_colonies;
                            var points = _context.ColoniaEs.FirstOrDefault(x => x.id_colonies == idcolonia);
                            points.points = points.points - 1;
                            await _context.SaveChangesAsync();
                        }

                        return new Respuestas
                        {
                            resultado = true,
                            message = "¡dislike exitoso!",
                        };
                    }
                    else
                    {
                        return new Respuestas
                        {
                            resultado = false,
                            message = "¡dislike no exitoso!",
                        };
                    }

                }
            }
            catch (Exception)
            {
                _logger.LogError("Error en el metodo PublicationCommands.like...");
                throw;
            }

        }

        public async Task<Respuestas> likeAnswers(AnswerLikeDTOs answerLikeDTOs)
        {

            _logger.LogTrace("Iniciando metodo PublicationCommands.like...");
            try
            {
                if (answerLikeDTOs.is_like == 0)
                {
                    var CommentsLikeE = AnswerLikeDTOs.CreateE(answerLikeDTOs);
                    await _context.AnswerLikeEs.AddAsync(CommentsLikeE);
                    await _context.SaveChangesAsync();
                    if (CommentsLikeE.id_publication_comments_answer_like != 0)
                    {

                        var idrespuesta = await _context.CommentsAnswercsEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_publication_comments_answer == answerLikeDTOs.fk_tbl_publication_comments_answer);

                        var idComentario = await _context.CommentsEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_publication_comments == idrespuesta.fk_tbl_publication_comments);

                        var idPublication = await _context.PublicationEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_publication == idComentario.fk_tbl_publication);

                        if (idrespuesta.fk_tbl_user  != answerLikeDTOs.fk_tbl_user)
                        {
                            var notificacionDTOS = new NotificacionDTOs
                            {
                                id_notification = 0,
                                type_notification = 4,
                                for_user = idrespuesta.fk_tbl_user,
                                of_user = answerLikeDTOs.fk_tbl_user,
                                data_created = DateTime.UtcNow,
                                state = false,
                                fk_tbl_publication = idPublication.id_publication,
                                fk_tbl_diary = 0,
                            };

                            bool notificacion = await _notificacionCommands.RegistrarNotificacion(notificacionDTOS);
                        }

                        var resultado = _context.ColoniaPublicationEs.Where(x => x.fk_tbl_publication == idPublication.id_publication).FirstOrDefault();


                        if (resultado != null)
                        {
                            var idcolonia = resultado.fk_tbl_colonies;
                            var points = _context.ColoniaEs.FirstOrDefault(x => x.id_colonies == idcolonia);
                            points.points = points.points + 1;
                            await _context.SaveChangesAsync();
                        }

                        return new Respuestas
                        {
                            resultado = true,
                            message = "¡like exitoso!",
                        };
                    }
                    else
                    {
                        return new Respuestas
                        {
                            resultado = false,
                            message = "¡like no exitoso!",
                        };
                    }
                }
                else
                {
                    var likesAEliminar = await _context.AnswerLikeEs.AsNoTracking().FirstOrDefaultAsync(x => x.fk_tbl_user == answerLikeDTOs.fk_tbl_user && x.fk_tbl_publication_comments_answer == answerLikeDTOs.fk_tbl_publication_comments_answer);

                    _context.AnswerLikeEs.Remove(likesAEliminar);
                    _context.SaveChanges();
                    if (likesAEliminar.id_publication_comments_answer_like != 0)
                    {
                        var idrespuesta = await _context.CommentsAnswercsEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_publication_comments_answer == answerLikeDTOs.fk_tbl_publication_comments_answer);

                        var idComentario = await _context.CommentsEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_publication_comments == idrespuesta.fk_tbl_publication_comments);

                        var idPublication = await _context.PublicationEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_publication == idComentario.fk_tbl_publication);

                        var resultado = _context.ColoniaPublicationEs.Where(x => x.fk_tbl_publication == idPublication.id_publication).FirstOrDefault();


                        if (resultado != null)
                        {
                            var idcolonia = resultado.fk_tbl_colonies;
                            var points = _context.ColoniaEs.FirstOrDefault(x => x.id_colonies == idcolonia);
                            points.points = points.points - 1;
                            await _context.SaveChangesAsync();
                        }

                        return new Respuestas
                        {
                            resultado = true,
                            message = "¡dislike exitoso!",
                        };
                    }
                    else
                    {
                        return new Respuestas
                        {
                            resultado = false,
                            message = "¡dislike no exitoso!",
                        };
                    }

                }
            }
            catch (Exception)
            {
                _logger.LogError("Error en el metodo PublicationCommands.like...");
                throw;
            }

        }
    }
}
