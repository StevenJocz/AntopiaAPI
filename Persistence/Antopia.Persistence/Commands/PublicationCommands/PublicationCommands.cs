
using Antopia.Domain.DTOs.PublicationDTOs;
using Antopia.Domain.DTOs.RespuestaDTOs;
using Antopia.Domain.DTOs.UserDTOs;
using Antopia.Domain.Entities.PublicacionE;
using Antopia.Infrastructure;
using Antopia.Persistence.ImageService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Persistence.Commands.PublicationCommands
{
    public interface IPublicationCommands
    {
        Task<Respuestas> InsertarPublication(ListPublicationDTOs listPublicationDTOs);
    }

    public class PublicationCommands : IPublicationCommands, IDisposable
    {
        private readonly AntopiaDbContext _context = null;
        private readonly ILogger<PublicationCommands> _logger;
        private readonly IConfiguration _configuration;
        private readonly IImageService _imageService;

        public PublicationCommands(ILogger<PublicationCommands> logger, IConfiguration configuration, IImageService imageService)
        {
            _logger = logger;
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection_Salud");
            _context = new AntopiaDbContext(connectionString);
            _imageService = imageService;
        }

        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposed = true;
            }
        }


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
                    fk_tbl_type_publication = listPublicationDTOs.Publicaciones[0].fk_tbl_type_publication
                };

                var publicacionE = PublicationDTOs.CreateE(publication);
                await _context.PublicationEs.AddAsync(publicacionE);
                await _context.SaveChangesAsync();

                if (publicacionE.id_publication != 0)
                {
                    if (listPublicationDTOs.Imagenes != null && listPublicationDTOs.Imagenes.Count > 0)
                    {
                        foreach (var imagen in listPublicationDTOs.Imagenes)
                        {
                            string rutaImagen = "wwwroot/ImagesPublication";
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
    }
}
