using Antopia.Domain.Entities.LoginE;
using Antopia.Domain.Entities.UserE;
using Antopia.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Persistence.Queries.LoginQueries
{
    public interface ILoginQueries
    {
        Task<bool> ConsultarCodigo(CodigoRestablecimientoE nuevoCodigo);
        Task<UserE> ConsultarUsuarioXId(int idUsuario);
        Task<LoginE> ConsultarUsuarioXCorreo(string userEmail, string userPassword);
    }

    public class LoginQueries : ILoginQueries, IDisposable
    {
        private readonly AntopiaDbContext _context = null;
        private readonly ILogger<LoginQueries> _logger;
        private readonly IConfiguration _configuration;

        public LoginQueries(ILogger<LoginQueries> logger, IConfiguration configuration)
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

        public async Task<UserE> ConsultarUsuarioXId(int idUsuario)
        {
            _logger.LogTrace("Iniciando metodo LoginQueries.ConsultarUsuario...");
            try
            {
                var usuario = await _context.LoginEs.AsNoTracking().FirstOrDefaultAsync(x => x.IdLogin == idUsuario);
                var datosUsuario = await _context.UserEs.AsNoTracking().FirstOrDefaultAsync(x => x.id == usuario.fk_tblusers);

                return datosUsuario;
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar LoginQueries.ConsultarUsuario...");
                throw;
            }
        }

        public async Task<LoginE> ConsultarUsuarioXCorreo(string userEmail, string userPassword)
        {
            _logger.LogTrace("Iniciando metodo LoginQueries.ConsultarUsuarioAutorizado...");
            try
            {
                userEmail = userEmail.Trim();
                userPassword = userPassword.Trim();
                var usuario = await _context.LoginEs.AsNoTracking().FirstOrDefaultAsync(x =>
                    x.s_userEmail == userEmail &&
                    x.s_userPassword == userPassword
                );

                return usuario;
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar LoginQueries.ConsultarUsuarioAutorizado...");
                throw;
            }
        }


        public async Task<bool> ConsultarCodigo(CodigoRestablecimientoE nuevoCodigo)
        {
            _logger.LogTrace("Iniciando metodo LoginQueries.ConsultarCodigo...");
            try
            {
                var correocorrecto = await _context.CodigoRestablecimientoEs.AsNoTracking().FirstOrDefaultAsync(e => e.s_codigo == nuevoCodigo.s_codigo && e.s_correo == nuevoCodigo.s_correo);

                return (correocorrecto != null) ? true : false;

            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar LoginQueries.ConsultarCodigo...");
                throw;
            }
        }
    }
}
