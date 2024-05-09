using Antopia.Domain.DTOs.ColinaDTOs;
using Antopia.Domain.DTOs.NotificacionDTOs;
using Antopia.Domain.DTOs.PublicationDTOs;
using Antopia.Domain.DTOs.RespuestaDTOs;
using Antopia.Domain.Entities.NotificacionE;
using Antopia.Domain.Entities.PublicacionE;
using Antopia.Infrastructure;
using Antopia.Persistence.Commands.PublicationCommands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Persistence.Queries.NotificacionQueries
{
    public interface INotificacionQueries
    {
        Task<List<NotificacionesUser>> ListarNotificacionesUser(int idUser);
        Task<Respuestas> Notificaciones(int idUser);
    }
    public class NotificacionQueries : INotificacionQueries, IDisposable
    {
        private readonly AntopiaDbContext _context = null;
        private readonly ILogger<PublicationCommands> _logger;
        private readonly IConfiguration _configuration;

        public NotificacionQueries(ILogger<PublicationCommands> logger, IConfiguration configuration)
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

        public async Task<List<NotificacionesUser>> ListarNotificacionesUser(int idUser)
        {
            _logger.LogTrace("Iniciando metodo NotificacionQueries.ListarNotificacionesUser...");
            try
            {
                var notificacionesE = await _context.NotificacionEs.AsNoTracking().Where(x => x.for_user == idUser && x.of_user != idUser).OrderByDescending(x => x.data_created).Take(30).ToListAsync();

                var notificacionesList = new List<NotificacionesUser>();
                foreach (var noti in notificacionesE)
                {
                    var datosUsuario = await _context.UserEs.AsNoTracking().FirstOrDefaultAsync(x => x.id == noti.of_user);

                    var publications = await _context.PublicationEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_publication == noti.fk_tbl_publication);

                    var nombreColonia = await _context.ColoniaEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_colonies == noti.fk_tbl_colonie);

                    var typeNotificacion = await _context.TypeNotificacionEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_notification_type == noti.type_notification);

                    var list = new NotificacionesUser
                    {
                        idNotification = noti.id_notification,
                        typeNotification = noti.type_notification,
                        idPublicacion = publications?.id_publication ?? 0,
                        tituloPublicacion = publications?.s_title ?? "Título predeterminado",
                        idUser = datosUsuario.id,
                        urlPerfil = datosUsuario.s_userProfile,
                        fotoUser = datosUsuario.s_userPhoto,
                        NombreUser = datosUsuario.s_user_name,
                        contenido = typeNotificacion.s_content,
                        idColonia = nombreColonia?.id_colonies ?? 0,
                        nombreColonia = nombreColonia?.s_name ?? "Nombre predeterminado",
                        state = noti.state,
                        fechaCreacion = noti.data_created
                    };

                    notificacionesList.Add(list);
                }

                return notificacionesList;

            }
            catch (Exception)
            {
                _logger.LogError("Error en el metodo NotificacionQueries.ListarNotificacionesUser...");
                throw;
            }
        }

        public async Task<Respuestas> Notificaciones(int idUser)
        {
            _logger.LogTrace("Iniciando metodo NotificacionQueries.ListarNotificacionesUser...");
            try
            {
                var notificacionesE = await _context.NotificacionEs.AsNoTracking().Where(x => x.for_user == idUser && x.of_user != idUser && x.state == false).ToListAsync();

                if (notificacionesE.Count > 0)
                {
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
                _logger.LogError("Error en el metodo NotificacionQueries.ListarNotificacionesUser...");
                throw;
            }
        }

    }
}
