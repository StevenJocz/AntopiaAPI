using Antopia.Domain.DTOs.LoginDTOs;
using Antopia.Domain.DTOs.RespuestaDTOs;
using Antopia.Domain.DTOs.UserDTOs;
using Antopia.Domain.Entities.UserE;
using Antopia.Infrastructure;
using Antopia.Persistence.Commands.LoginCommands;
using Antopia.Persistence.ImageService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Persistence.Commands.UserCommands
{
    public interface IUserCommands
    {
        Task<Respuestas> InsertarUser(UserDTOs userDTOs);
        Task<Respuestas> ActualizrDatos(int idUser, int tipo, string dato);
    }

    public class UserCommands : IUserCommands, IDisposable
    {

        private readonly AntopiaDbContext _context = null;
        private readonly ILogger<UserCommands> _logger;
        private readonly IConfiguration _configuration;
        private readonly ILoginCommands _loginCommands;
        private readonly IImageService _imageService;

        public UserCommands(ILogger<UserCommands> logger, IConfiguration configuration, ILoginCommands loginCommands, IImageService imageService)
        {
            _logger = logger;
            _configuration = configuration;
            _loginCommands = loginCommands;
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

        public async Task<Respuestas> InsertarUser(UserDTOs userDTOs)
        {
            _logger.LogTrace("Iniciando metodo UserCommands.InsertarUser...");

            try
            {
                var Email = _context.UserEs.FirstOrDefault(x => x.s_user_email == userDTOs.s_user_email);

                if (Email == null)
                {
                    string rutaPerfil = "wwwroot/ImagesPerfil";
                    string rutaFondo = "wwwroot/ImageFondo";
                    var rutaPhoto = await _imageService.SaveImageAsync(userDTOs.s_userPhoto, rutaPerfil);
                    var rutaImagenFondo =  await _imageService.SaveImageAsync(userDTOs.s_userFrontpage, rutaFondo);

                    var nuevoUser = new UserDTOs
                    {
                        s_user_name = userDTOs.s_user_name,
                        dt_user_birthdate = userDTOs.dt_user_birthdate,
                        s_user_gender = userDTOs.s_user_gender,
                        fk_user_address_city = userDTOs.fk_user_address_city,
                        s_user_cellphone = userDTOs.s_user_cellphone,
                        s_user_email = userDTOs.s_user_email,
                        s_userProfile = userDTOs.s_userProfile,
                        s_userPhoto = rutaPhoto,
                        s_userFrontpage = rutaImagenFondo,
                        s_frase = userDTOs.s_frase,
                        fk_tblRol = userDTOs.fk_tblRol,
                    };

                    var userE = UserDTOs.CreateE(nuevoUser);

                    await _context.UserEs.AddAsync(userE);
                    await _context.SaveChangesAsync();

                    if (userE.id != 0)
                    {
                        var nuevoLogin = new LoginDTOs
                        {
                            userEmail = userDTOs.s_user_email,
                            userPassword = userDTOs.Password,
                            fk_tblusers = userE.id,
                        };

                        await _loginCommands.InsertarLogin(nuevoLogin);


                        return new Respuestas
                        {
                            resultado = true,
                            message = "¡Has completado el registro satisfactoriamente! Para continuar, inicia sesión.",
                        };
                    }
                    else
                    {
                        return new Respuestas
                        {
                            resultado = false,
                            message = "No se logro crear el usuario correctamente.Por favor, inténtalo nuevamente.",
                        };
                    }
                }
                else
                {
                    return new Respuestas
                    {
                        resultado = false,
                        message = "Ya existe un usuario registrado con el correo " + userDTOs.s_user_email + ". Por favor, intenta registrarte con otra dirección de correo electrónico.",
                    };
                }

            }
            catch (Exception)
            {
                _logger.LogError("Error en el metodo UserCommands.InsertarUser...");
                throw;
            }
        }




        public async Task<Respuestas> ActualizrDatos(int idUser, int tipo, string dato)
        {
            _logger.LogTrace("Iniciando metodo UserCommands.ActualizrDatos...");
            try
            {
                var user = _context.UserEs.FirstOrDefault(x => x.id == idUser);

                if (user != null)
                {
                    if (tipo == 1) // Frase
                    {
                        user.s_frase = dato;
                    }
                    else if (tipo == 2) // Foto Perfil
                    {
                        user.s_userPhoto = dato;
                    }
                    else if (tipo == 3) // Foto Fondo
                    {
                        user.s_userFrontpage = dato;
                    }

                    await _context.SaveChangesAsync();
                    return new Respuestas
                    {
                        resultado = true,
                        message = "Actualizado correctamente",
                    };
                }
                else
                {
                    return new Respuestas
                    {
                        resultado = false,
                        message = "No se actualizado correctamente",
                    };
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
