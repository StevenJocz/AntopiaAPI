using Antopia.Domain.DTOs.DiaryDTOs;
using Antopia.Domain.DTOs.NotificacionDTOs;
using Antopia.Domain.DTOs.PublicationDTOs;
using Antopia.Domain.DTOs.RespuestaDTOs;
using Antopia.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Persistence.Commands.NotificacionCommands
{
    public interface INotificacionCommands
    {
        Task<bool> RegistrarNotificacion(NotificacionDTOs notificacionDTOs);
        Task<Respuestas> CambiarEstadoNotificacion(idNotification idNotification);
        Task<Respuestas> MarcaNotificacionesNoLeidas(IdUser idUser);
    }
    public class NotificacionCommands : INotificacionCommands, IDisposable
    {
        private readonly AntopiaDbContext _context = null;
        private readonly ILogger<NotificacionCommands> _logger;
        private readonly IConfiguration _configuration;

        public NotificacionCommands(ILogger<NotificacionCommands> logger, IConfiguration configuration)
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


        public async Task<bool> RegistrarNotificacion(NotificacionDTOs notificacionDTOs)
        {
            _logger.LogTrace("Iniciando metodo NotificacionCommands.RegistrarNotificacion...");
            try
            {
                var notificacionE = NotificacionDTOs.CreateE(notificacionDTOs);
                await _context.NotificacionEs.AddAsync(notificacionE);
                await _context.SaveChangesAsync();

                if (notificacionE.id_notification != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar NotificacionCommands.RegistrarNotificacion");
                throw;
            }
        }

        public async Task<Respuestas> CambiarEstadoNotificacion(idNotification idNotification)
        {
            _logger.LogTrace("Iniciando metodo NotificacionCommands.CambiarEstadoNotificacion...");
            try
            {
                var notificacion =  _context.NotificacionEs.Find(idNotification.id_notification);

                if (notificacion != null)
                {
                    notificacion.state = true;
                    _context.SaveChanges();  
                }

                return new Respuestas
                {
                    resultado = true,
                    message = "¡cambio de estado exitosamente!",
                };
            }
            catch (Exception)
            {
                _logger.LogError("Error en el metodo NotificacionCommands.CambiarEstadoNotificacion...");
                throw;
            }
        }

        public async Task<Respuestas> MarcaNotificacionesNoLeidas(IdUser idUser)
        {
            _logger.LogTrace("Iniciando metodo NotificacionQueries.MarcaNotificacionesNoLeidas...");
            try
            {
                var notificaciones = _context.NotificacionEs
                                    .Where(x => x.for_user == idUser.id_user && x.of_user != idUser.id_user && x.state == false)
                                    .ToList();

                if (notificaciones.Any())
                {
                    foreach (var notificacion in notificaciones)
                    {
                        notificacion.state = true; 
                    }

                    await _context.SaveChangesAsync(); 
                }

                return new Respuestas
                {
                    resultado = true,
                    message = "¡Cambio de estado exitosamente!",
                };
            }
            catch (Exception)
            {
                _logger.LogError("Error en el metodo NotificacionCommands.MarcaNotificacionesNoLeidas...");
                throw;
            }
        }
    }
}
