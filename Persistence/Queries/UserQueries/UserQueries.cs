
using Antopia.Domain.DTOs.PublicationDTOs;
using Antopia.Domain.DTOs.UserDTOs;
using Antopia.Domain.Entities.PublicacionE;
using Antopia.Domain.Entities.UserE;
using Antopia.Infrastructure;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Persistence.Queries.UserQueries
{

    public interface IUserQueries
    {
        Task<List<PerfilUserDTOs>> ConsultarUsuario(int idUser, int idUserConsulta);
        List<UserDTOs> ConsultarUsuarioPorProfile(string keyword);
        Task<List<UserDTOs>> ConsultarFollowers(int accion, int user);
        Task<List<UserDTOs>> ConsultarNotFollowers(int user);
    }

    public class UserQueries : IUserQueries, IDisposable
    {
        private readonly AntopiaDbContext _context = null;
        private readonly ILogger<UserQueries> _logger;
        private readonly IConfiguration _configuration;

        public UserQueries(ILogger<UserQueries> logger, IConfiguration configuration)
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

        public async Task<List<PerfilUserDTOs>> ConsultarUsuario(int idUser, int idUserConsulta)
        {
            _logger.LogTrace("Iniciando metodo UserQueries.PerfilUserDTOs...");
            try
            {
                var datosUsuario = await _context.UserEs.AsNoTracking().FirstOrDefaultAsync(x => x.id == idUser);
                var perflList = new List<PerfilUserDTOs>();
                if (datosUsuario != null)
                {

                    
                    var TotalPublications = await _context.PublicationEs.AsNoTracking().Where(x => x.fk_tbl_user == idUser && x.fk_tbl_type_publication != 5 && x.byte_blocked == 0).ToListAsync();

                    var TotalSeguidores = await _context.FollowersEs.AsNoTracking().Where(x => x.id_user == idUser).ToListAsync();
                    var TotalSeguiendo = await _context.FollowersEs.AsNoTracking().Where(x => x.id_follower == idUser).ToListAsync();
                    var Siguiendo = await _context.FollowersEs.AsNoTracking().Where(x => x.id_user == idUser && x.id_follower == idUserConsulta).ToListAsync();

                    

                    var imagenesList = new List<PerfilImagenesDTOs>();
                    if (TotalPublications.Count > 0)
                    {
                        foreach (var idPublicaciones in TotalPublications)
                        {
                            var imagenesPublications = await _context.PublicationImageEs.AsNoTracking().Where(x => x.fk_tbl_publication == idPublicaciones.id_publication).ToListAsync();

                            if (imagenesPublications.Count > 0)
                            { 
                                foreach (var imagenes in imagenesPublications)
                                {
                                    var perfilImagenesDTOs = new PerfilImagenesDTOs
                                    {
                                        idPublicacion = imagenes.fk_tbl_publication,
                                        url = imagenes.s_location,
                                    };

                                    imagenesList.Add(perfilImagenesDTOs);
                                }
                            }
                        }

                    }
                    
                    var perfil = new PerfilUserDTOs
                    {
                        IdPerfil = datosUsuario.id,
                        NombrePerfil = datosUsuario.s_user_name,
                        urlPerfil = datosUsuario.s_userProfile,
                        ImagenPerfil = datosUsuario.s_userPhoto,
                        ImagenPortada = datosUsuario.s_userFrontpage,
                        Frase = datosUsuario.s_frase,
                        Correo = datosUsuario.s_user_email,
                        CantidadPublicaciones = TotalPublications.Count,
                        Seguidores = TotalSeguidores.Count,
                        TotalSeguiendo = TotalSeguiendo.Count,
                        Seguiendo = Siguiendo.Count,
                        Level = datosUsuario.fk_tbl_level,
                        PerfilImagenes = imagenesList,
                    };

                    perflList.Add(perfil);
                }

                return perflList;
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar UserQueries.PerfilUserDTOs");
                throw;
            }
            
        }

        public List<UserDTOs> ConsultarUsuarioPorProfile(string keyword)
        {
            _logger.LogTrace("Iniciando metodo UserQueries.ConsultarUsuarioPorProfile...");
            try
            {
                keyword = keyword.ToLower();

                var infoUser = _context.UserEs
                   .Where(p => p.s_userProfile != null && p.s_userProfile.ToLower().Contains(keyword))
                   .ToList();

                var perflList = new List<UserDTOs>();

                foreach (var users in infoUser)
                {
                    var perfil = new UserDTOs
                    {
                        id = users.id,
                        s_user_name = users.s_user_name,
                        s_userProfile = users.s_userProfile,
                        s_userPhoto = users.s_userPhoto,
                        fk_tbl_level = users.fk_tbl_level,

                    };

                    perflList.Add(perfil);

                }

                return perflList;
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar UserQueries.ConsultarUsuarioPorProfile");
                throw;
            }

           
        }

        public async Task<List<UserDTOs>> ConsultarFollowers(int accion, int user)
        {
            _logger.LogTrace("Iniciando metodo UserQueries.ConsultarFollowers...");
            try
            {

                var expresion = (Expression<Func<FollowersE, bool>>)null;
                
                if (accion == 1)
                {
                    expresion = expresion = x => x.id_user == user;
                } 
                else
                {
                    expresion = expresion = x => x.id_follower == user;
                }

                var seguidores = await _context.FollowersEs.AsNoTracking().Where(expresion).ToListAsync();

                var perflList = new List<UserDTOs>();

                foreach (var users in seguidores)
                {
                    var expresionDos = (Expression<Func<UserE, bool>>)null;

                    if (accion == 1)
                    {
                        expresionDos = expresionDos = x => x.id == users.id_follower;
                    }
                    else
                    {
                        expresionDos = expresionDos = x => x.id == users.id_user;
                    }

                    var datosUsuario = await _context.UserEs.AsNoTracking().FirstOrDefaultAsync(expresionDos);

                    var perfil = new UserDTOs
                    {
                        id = datosUsuario.id,
                        s_user_name = datosUsuario.s_user_name,
                        s_userProfile = datosUsuario.s_userProfile,
                        s_userPhoto = datosUsuario.s_userPhoto,
                        fk_tbl_level = datosUsuario.fk_tbl_level,

                    };
                    perflList.Add(perfil);
                }
                return perflList;
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar UserQueries.ConsultarFollowers");
                throw;
            }
           
        }

        public async Task<List<UserDTOs>> ConsultarNotFollowers(int user)
        {
            _logger.LogTrace("Iniciando metodo UserQueries.ConsultarNotFollowers...");
            try
            {

                var recommendedUsers = _context.recomendarFoFollowersEs
                    .FromSqlRaw($"SELECT * FROM get_random_users({user});")
                    .ToList();

                var perflList = new List<UserDTOs>();

                foreach (var users in recommendedUsers)
                {
                    var datosUsuario = await _context.UserEs.AsNoTracking().FirstOrDefaultAsync(x => x.id == users.id);

                    var perfil = new UserDTOs
                    {
                        id = datosUsuario.id,
                        s_user_name = datosUsuario.s_user_name,
                        s_userProfile = datosUsuario.s_userProfile,
                        s_userPhoto = datosUsuario.s_userPhoto,
                        fk_tbl_level = datosUsuario.fk_tbl_level,

                    };
                    perflList.Add(perfil);
                }
                return perflList;
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar UserQueries.ConsultarNotFollowers");
                throw;
            }

        }
    }
}
