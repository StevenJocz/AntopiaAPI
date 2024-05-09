using Antopia.Domain.DTOs.DiaryDTOs;
using Antopia.Domain.DTOs.PublicationDTOs;
using Antopia.Domain.DTOs.RespuestaDTOs;
using Antopia.Domain.Entities.DiaryE;
using Antopia.Domain.Entities.PublicacionE;
using Antopia.Infrastructure;
using Antopia.Persistence.ImageService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Persistence.Commands.DiaryCommands
{
    public interface IDiaryCommands
    {
        Task<Respuestas> RegistrarDiario(DiaryDTOs diaryDTOs);
        Task<Respuestas> AddRegistroDiario(ListDiary listDiary);
        Task<Respuestas> likeDiary(DiaryLikeDTOs DiaryLikeDTOs);
        Task<Respuestas> RegistrarComentarios(CommentDiaryDTOs commentDiaryDTOs);
    }

    public class DiaryCommands : IDiaryCommands, IDisposable
    {
        private readonly AntopiaDbContext _context = null;
        private readonly ILogger<DiaryCommands> _logger;
        private readonly IConfiguration _configuration;
        private readonly IImageService _imageService;

        public DiaryCommands(ILogger<DiaryCommands> logger, IConfiguration configuration, IImageService imageService)
        {
            _logger = logger;
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection");
            _context = new AntopiaDbContext(connectionString);
            _imageService = imageService;
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

        public async Task<Respuestas> RegistrarDiario(DiaryDTOs diaryDTOs)
        {
            _logger.LogTrace("Iniciando metodo DiaryCommands.AgregarDiario...");
            try
            {
                var DiaryE = DiaryDTOs.CreateE(diaryDTOs);
                await _context.DiaryEs.AddAsync(DiaryE);
                await _context.SaveChangesAsync();

                if (DiaryE.id_diary != 0)
                {
                    return new Respuestas
                    {
                        resultado = true,
                        message = "¡Diario registrado exitosamente!",
                    };
                }
                else
                {
                    return new Respuestas
                    {
                        resultado = false,
                        message = "¡No se pudo registrar el diario con exitoso!",
                    };
                }

            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar DiaryCommands.AgregarDiario");
                throw;
            }
        }


        public async Task<Respuestas> AddRegistroDiario(ListDiary listDiary)
        {
            _logger.LogTrace("Iniciando metodo DiaryCommands.AddRegistroDiario...");
            try
            {
                var EntriesDiaryE = DiaryEntriesDTOs.CreateE(listDiary.DiaryEntries[0]);
                await _context.DiaryEntriesEs.AddAsync(EntriesDiaryE);
                await _context.SaveChangesAsync();

                if (EntriesDiaryE.id_diary_entries != 0)
                {
                    if (listDiary.Imagenes != null && listDiary.Imagenes.Count > 0)
                    {
                        foreach (var imagen in listDiary.Imagenes)
                        {
                            string rutaImagen = "wwwroot/ImagesDiary";
                            var location = await _imageService.SaveImageAsync(imagen.s_location, rutaImagen);

                            var imagenes = new DiaryImageDTOs
                            {
                                fk_tbl_diary_entries = EntriesDiaryE.id_diary_entries,
                                s_location = location,
                            };

                            var publicationImageE = DiaryImageDTOs.CreateE(imagenes);
                            await _context.DiaryImageEs.AddAsync(publicationImageE);
                            await _context.SaveChangesAsync();
                        }
                    }
                    
                    return new Respuestas
                    {
                        resultado = true,
                        message = "¡Registro agregado al diario exitosamente!",
                    };
                }
                else
                {
                    return new Respuestas
                    {
                        resultado = false,
                        message = "¡No se pudo agregar el registro al diario con exitoso!",
                    };
                }

            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar DiaryCommands.AddRegistroDiario");
                throw;
            }
        }


        public async Task<Respuestas> likeDiary(DiaryLikeDTOs DiaryLikeDTOs)
        {

            _logger.LogTrace("Iniciando metodo PublicationCommands.like...");
            try
            {
                if (DiaryLikeDTOs.is_like == 0)
                {
                    var DiaryLikeE = DiaryLikeDTOs.CreateE(DiaryLikeDTOs);
                    await _context.DiaryLikeEs.AddAsync(DiaryLikeE);
                    await _context.SaveChangesAsync();
                    if (DiaryLikeE.id_diary_like != 0)
                    {
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
                    var likesAEliminar = await _context.DiaryLikeEs.AsNoTracking().FirstOrDefaultAsync(x => x.fk_tbl_user == DiaryLikeDTOs.fk_tbl_user && x.fk_tbl_diary == DiaryLikeDTOs.fk_tbl_diary);

                    _context.DiaryLikeEs.Remove(likesAEliminar);
                    _context.SaveChanges();
                    if (likesAEliminar.id_diary_like != 0)
                    {
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


        public async Task<Respuestas> RegistrarComentarios(CommentDiaryDTOs commentDiaryDTOs)
        {
            _logger.LogTrace("Iniciando metodo DiaryCommands.AgregarDiario...");
            try
            {
                var CommentDiaryE = CommentDiaryDTOs.CreateE(commentDiaryDTOs);
                await _context.CommentDiaryEs.AddAsync(CommentDiaryE);
                await _context.SaveChangesAsync();

                if (CommentDiaryE.id_diary_comments != 0)
                {
                    return new Respuestas
                    {
                        resultado = true,
                        message = "¡Comentario registrado exitosamente!",
                    };
                }
                else
                {
                    return new Respuestas
                    {
                        resultado = false,
                        message = "¡No se pudo registrar el comentarios con exitoso!",
                    };
                }

            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar DiaryCommands.AgregarDiario");
                throw;
            }
        }
    }
}
