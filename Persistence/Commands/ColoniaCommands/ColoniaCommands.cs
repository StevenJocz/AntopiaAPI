using Antopia.Domain.DTOs.ColinaDTOs;
using Antopia.Domain.DTOs.NotificacionDTOs;
using Antopia.Domain.DTOs.PublicationDTOs;
using Antopia.Domain.DTOs.RespuestaDTOs;
using Antopia.Domain.DTOs.UserDTOs;
using Antopia.Domain.Entities.ColoniaE;
using Antopia.Domain.Entities.PublicacionE;
using Antopia.Domain.Entities.UserE;
using Antopia.Infrastructure;
using Antopia.Persistence.Commands.NotificacionCommands;
using Antopia.Persistence.ImageService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Antopia.Persistence.Commands.ColoniaCommands
{
    public interface IColoniaCommands
    {
        Task<Respuestas> CrearColonia(ColoniaDTOs colinaDTOs);
        Task<Respuestas> UnirmeColonia(MembersDTOs membersDTOs);
    }

    public class ColoniaCommands : IColoniaCommands, IDisposable
    {
        private readonly AntopiaDbContext _context = null;
        private readonly ILogger<ColoniaCommands> _logger;
        private readonly IConfiguration _configuration;
        private readonly IImageService _imageService;
        private readonly INotificacionCommands _notificacionCommands;

        public ColoniaCommands(ILogger<ColoniaCommands> logger, IConfiguration configuration, IImageService imageService, INotificacionCommands notificacionCommands)
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

        public async Task<Respuestas> CrearColonia(ColoniaDTOs colinaDTOs)
        {
            _logger.LogTrace("Iniciando metodo ColoniaCommands.CrearColonia...");
            try
            {
                string rutaImagen = "wwwroot/ImagesPhotoGrupo";
                var location = await _imageService.SaveImageAsync(colinaDTOs.s_photo, rutaImagen);

                var newColonia = new ColoniaDTOs 
                {
                    id_colonies = colinaDTOs.id_colonies,
                    s_name = colinaDTOs.s_name,
                    s_description = colinaDTOs.s_description,
                    fk_tbl_user_creator = colinaDTOs.fk_tbl_user_creator,
                    dt_creation = DateTime.UtcNow,
                    s_photo = location,
                    s_colors = colinaDTOs.s_colors,
                    points = 5,
                };

                var ColoniaE = ColoniaDTOs.CreateE(newColonia);
                await _context.ColoniaEs.AddAsync(ColoniaE);
                await _context.SaveChangesAsync();
                if (ColoniaE.id_colonies != 0)
                {
                    var newMembers = new MembersDTOs
                    {
                        id_colonies_members = 0,
                        fk_tbl_colonies = ColoniaE.id_colonies,
                        fk_tbl_user_members = ColoniaE.fk_tbl_user_creator,
                    };

                    var MembersE = MembersDTOs.CreateE(newMembers);
                    await _context.MembersEs.AddAsync(MembersE);
                    await _context.SaveChangesAsync();

                    return new Respuestas
                    {
                        resultado = true,
                        message = "¡Colonia creada!",
                    };
                }else
                {
                    return new Respuestas
                    {
                        resultado = false,
                        message = "¡Error al crear la colonia!",
                    };
                }
            }

            catch (Exception)
            {
                _logger.LogError("Error en el metodo ColoniaCommands.CrearColonia...");

                throw;
            }
        }


        public async Task<Respuestas> UnirmeColonia(MembersDTOs membersDTOs)
        {
            _logger.LogTrace("Iniciando metodo ColoniaCommands.UnirmeColonia...");
            try
            {
                if (membersDTOs.esMember == 0 )
                {

                    var MembersE = MembersDTOs.CreateE(membersDTOs);
                    await _context.MembersEs.AddAsync(MembersE);
                    await _context.SaveChangesAsync();


                    if (MembersE != null)
                    {
                        var points = _context.ColoniaEs.FirstOrDefault(x => x.id_colonies == membersDTOs.fk_tbl_colonies);
                        points.points = points.points + 5;
                        await _context.SaveChangesAsync();

                        var memberColonies = await _context.MembersEs.AsNoTracking().Where(x => x.fk_tbl_colonies == MembersE.fk_tbl_colonies).ToListAsync();

                        foreach (var memberList in memberColonies)
                        {
                            var notificacionDTOS = new NotificacionDTOs
                            {
                                id_notification = 0,
                                type_notification = 9,
                                for_user = memberList.fk_tbl_user_members,
                                of_user = membersDTOs.fk_tbl_user_members,
                                data_created = DateTime.UtcNow,
                                state = false,
                                fk_tbl_publication = 0,
                                fk_tbl_diary = 0,
                                fk_tbl_colonie = membersDTOs.fk_tbl_colonies,
                            };

                            bool notificacion = await _notificacionCommands.RegistrarNotificacion(notificacionDTOS);
                        }
                         

                        return new Respuestas
                        {
                            resultado = true,
                            message = "¡Se únio a la colonia exitosamente!",
                        };
                    }
                    else
                    {
                        return new Respuestas
                        {
                            resultado = false,
                            message = "¡No se únio exitosamente a la colonia!",
                        };
                    }
                }else
                {
                    var esMiembro = await _context.MembersEs.AsNoTracking().FirstOrDefaultAsync(x => x.fk_tbl_user_members == membersDTOs.fk_tbl_user_members && x.fk_tbl_colonies == membersDTOs.fk_tbl_colonies);
                    _context.MembersEs.Remove(esMiembro);
                    _context.SaveChangesAsync();

                    if (esMiembro != null)
                    {
                        var points = _context.ColoniaEs.FirstOrDefault(x => x.id_colonies == membersDTOs.fk_tbl_colonies);
                        points.points = points.points - 5;
                        await _context.SaveChangesAsync();

                        return new Respuestas
                        {
                            resultado = true,
                            message = "¡Se selio de la colonia exitosamente!",
                        };
                    }
                    else
                    {
                        return new Respuestas
                        {
                            resultado = false,
                            message = "¡No se puedo salir de la colonia  exitosamente!",
                        };
                    }
                }
            }

            catch (Exception)
            {
                _logger.LogError("Error en el metodo ColoniaCommands.UnirmeColonia...");

                throw;
            }
        }

       


    }
}
