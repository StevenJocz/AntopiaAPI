using Antopia.Domain.DTOs.LoginDTOs;
using Antopia.Domain.DTOs.RespuestaDTOs;
using Antopia.Domain.Entities.LoginE;
using Antopia.Infrastructure;
using Antopia.Persistence.Utilidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Persistence.Commands.LoginCommands
{
    public interface ILoginCommands
    {
        Task<bool> InsertarCodigo(CodigoRestablecimientoE nuevoCodigo);
        Task<bool> EliminarCodigo(string correo);
        Task<bool> ActualizarPassword(string userEmail, string nuevoPassword);
        Task<bool> InsertarLogin(LoginDTOs LoginDTOs);
        Task<bool> ActualizarPasswordUser();
    }

    public class LoginCommands : ILoginCommands, IDisposable
    {
        private readonly AntopiaDbContext _context = null;
        private readonly ILogger<LoginCommands> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUtilidades _utilidades;

        public LoginCommands(ILogger<LoginCommands> logger, IConfiguration configuration, IUtilidades utilidades)
        {
            _logger = logger;
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection");
            _context = new AntopiaDbContext(connectionString);
            _utilidades = utilidades;
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

        public async Task<bool> InsertarCodigo(CodigoRestablecimientoE nuevoCodigo)
        {
            _logger.LogTrace("Iniciando metodo LoginCommands.InsertarCodigo...");
            try
            {
                await _context.CodigoRestablecimientoEs.AddAsync(nuevoCodigo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Error en el metodo LoginCommands.InsertarCodigo...");
                throw;
            }
        }

        public async Task<bool> EliminarCodigo(string correo)
        {
            _logger.LogTrace("Iniciando metodo LoginCommands.EliminarCodigo...");
            try
            {
                var codigo = await _context.CodigoRestablecimientoEs.FirstOrDefaultAsync(c => c.s_correo == correo);
                if (codigo != null)
                {
                    _context.CodigoRestablecimientoEs.Remove(codigo);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error en el LoginCommands.EliminarCodigo...");
                throw;
            }
        }

        public async Task<bool> ActualizarPassword(string userEmail, string nuevoPassword)
        {
            _logger.LogTrace("Iniciando metodo LoginCommands.ActualizarCodigo...");
            try
            {
                var codigo = await _context.LoginEs.FirstOrDefaultAsync(c => c.s_userEmail == userEmail);
                if (codigo != null)
                {
                    var hashedPassword = await _utilidades.HashPassword(nuevoPassword.Trim());
                    codigo.s_userPassword = hashedPassword;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error en el LoginCommands.ActualizarCodigo...");
                throw;
            }
        }


        public async Task<bool> InsertarLogin(LoginDTOs LoginDTOs)
        {
            _logger.LogTrace("Iniciando metodo LoginCommands.InsertarLogin...");
            try
            {
                var LoginE = LoginDTOs.CreateE(LoginDTOs);
                await _context.LoginEs.AddAsync(LoginE);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Error en el metodo LoginCommands.InsertarLogin...");
                throw;
            }
        }

        public async Task<bool> ActualizarPasswordUser()
        {
            var usuarios = await _context.LoginEs.ToListAsync();

            if (usuarios != null)
            {
                foreach (var usuario in usuarios)
                {
                    var hashedPassword = await _utilidades.HashPassword(usuario.s_userPassword);
                    usuario.s_userPassword = hashedPassword;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
