using Antopia.Domain.DTOs.LoginDTOs;
using Antopia.Domain.DTOs.NotificacionDTOs;
using Antopia.Domain.DTOs.PublicationDTOs;
using Antopia.Domain.DTOs.RespuestaDTOs;
using Antopia.Domain.DTOs.UserDTOs;
using Antopia.Infrastructure;
using Antopia.Persistence.Commands.LoginCommands;
using Antopia.Persistence.Commands.NotificacionCommands;
using Antopia.Persistence.ImageService;
using Antopia.Persistence.Utilidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Persistence.Commands.UserCommands
{
    public interface IUserCommands
    {
        Task<Respuestas> InsertarUser(UserDTOs userDTOs);
        Task<Respuestas> ActualizrDatos(UpdateUserDTOs updateUserDTOs);
        Task<Respuestas> FollowersDatos(FollowersDTOs followersDTOs);
        Task<LevelDTOs> level(int idUser);
    }

    public class UserCommands : IUserCommands, IDisposable
    {

        private readonly AntopiaDbContext _context = null;
        private readonly ILogger<UserCommands> _logger;
        private readonly IConfiguration _configuration;
        private readonly ILoginCommands _loginCommands;
        private readonly IImageService _imageService;
        private readonly INotificacionCommands _notificacionCommands;
        private readonly IUtilidades _utilidades;

        public UserCommands(ILogger<UserCommands> logger, IConfiguration configuration, ILoginCommands loginCommands, IImageService imageService, INotificacionCommands notificacionCommands, IUtilidades utilidades)
        {
            _logger = logger;
            _configuration = configuration;
            _loginCommands = loginCommands;
            string? connectionString = _configuration.GetConnectionString("Connection");
            _context = new AntopiaDbContext(connectionString);
            _imageService = imageService;
            _notificacionCommands = notificacionCommands;
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
                        fk_user_address_city = userDTOs.fk_user_address_city,
                        s_user_email = userDTOs.s_user_email,
                        s_userProfile = userDTOs.s_userProfile,
                        s_userPhoto = rutaPhoto,
                        s_userFrontpage = rutaImagenFondo,
                        s_frase = userDTOs.s_frase,
                        fk_tblRol = userDTOs.fk_tblRol,
                        fk_tbl_level = userDTOs.fk_tbl_level,

                    };

                    var userE = UserDTOs.CreateE(nuevoUser);

                    await _context.UserEs.AddAsync(userE);
                    await _context.SaveChangesAsync();

                    if (userE.id != 0)
                    {
                        var hashedPassword = await _utilidades.HashPassword(userDTOs.Password.Trim());
                        var nuevoLogin = new LoginDTOs
                        {
                            userEmail = userDTOs.s_user_email.Trim(),
                            userPassword = hashedPassword,
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


        public async Task<Respuestas> ActualizrDatos(UpdateUserDTOs updateUserDTOs)
        {
            _logger.LogTrace("Iniciando metodo UserCommands.ActualizrDatos...");
            try
            {
                var user = _context.UserEs.FirstOrDefault(x => x.id == updateUserDTOs.idUser);

                if (user != null)
                {
                    if (updateUserDTOs.tipo == 1) // Frase
                    {
                        user.s_frase = updateUserDTOs.dato;
                    }

                    else if (updateUserDTOs.tipo == 2) // Foto Perfil
                    {
                        string rutaPerfil = "wwwroot/ImagesPerfil";
                        bool eliminar = await _imageService.DeleteImageAsync(user.s_userPhoto, updateUserDTOs.tipo);
                        var rutaPhoto = await _imageService.SaveImageAsync(updateUserDTOs.dato, rutaPerfil);
                        user.s_userPhoto = rutaPhoto;
                    }

                    else if (updateUserDTOs.tipo == 3) // Foto Fondo
                    {
                        string rutaFondo = "wwwroot/ImageFondo";
                        bool eliminar = await _imageService.DeleteImageAsync(user.s_userFrontpage, updateUserDTOs.tipo);
                        var rutaImagenFondo = await _imageService.SaveImageAsync(updateUserDTOs.dato, rutaFondo);
                        user.s_userFrontpage = rutaImagenFondo;
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

        public async Task<Respuestas> FollowersDatos(FollowersDTOs followersDTOs)
        {

            if (followersDTOs.isfollower == 1)
            {
                var seguir = FollowersDTOs.CreateE(followersDTOs);
                await _context.FollowersEs.AddAsync(seguir);
                await _context.SaveChangesAsync();

                var notificacionDTOS = new NotificacionDTOs
                {
                    id_notification = 0,
                    type_notification = 6,
                    for_user = followersDTOs.id_user,
                    of_user = followersDTOs.id_follower,
                    data_created = DateTime.UtcNow,
                    state = false,
                    fk_tbl_publication = 0,
                    fk_tbl_diary = 0,
                };

                bool notificacion = await _notificacionCommands.RegistrarNotificacion(notificacionDTOS);

                return new Respuestas
                {
                    resultado = true,
                    message = "siguiendo",
                };
            }
            else
            {
                var dejarSeguir = await _context.FollowersEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_user == followersDTOs.id_user && x.id_follower == followersDTOs.id_follower);
                _context.FollowersEs.Remove(dejarSeguir);
                _context.SaveChanges();

                return new Respuestas
                {
                    resultado = true,
                    message = "nosiguiendo",
                };
            }
        }


        public async Task<LevelDTOs> level(int idUser)
        {
            _logger.LogTrace("Iniciando metodo UserQueries.level...");
            try
            {
                var datosUsuario = await _context.UserEs.AsNoTracking().FirstOrDefaultAsync(x => x.id == idUser);

                var TotalPublications = await _context.PublicationEs.AsNoTracking().Where(x => x.fk_tbl_user == idUser && x.fk_tbl_type_publication != 5 && x.byte_blocked == 0).ToListAsync();

                var TotalSeguidores = await _context.FollowersEs.AsNoTracking().Where(x => x.id_user == idUser).ToListAsync();

                int Level = datosUsuario.fk_tbl_level;

                if (TotalPublications.Count > 25 && TotalSeguidores.Count > 20)
                {
                    Level = 2;
                }
                else if (TotalPublications.Count > 100 && TotalSeguidores.Count > 50)
                {
                    Level = 3;
                }
                else if (TotalPublications.Count > 200 && TotalSeguidores.Count > 100)
                {
                    Level = 4;
                }
                else if (TotalPublications.Count > 300 && TotalSeguidores.Count > 150)
                {
                    Level = 5;
                }
                else if (TotalPublications.Count > 500 && TotalSeguidores.Count > 250)
                {
                    Level = 6;
                }

                if (Level != datosUsuario.fk_tbl_level)
                {
                    datosUsuario.fk_tbl_level = Level;
                    await _context.SaveChangesAsync();
                }

               string nombre =  Level == 6 ? "Reina" : Level == 5 ? "Majors" : Level == 4 ? "Obrera" : Level == 3 ? "Pupa" : Level == 2 ? "Larva" : "Huevo";

                return new LevelDTOs
                {
                    id_level = Level,
                    s_level = nombre
                };
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar UserQueries.level");
                throw;
            }
        }

       
    }
}
