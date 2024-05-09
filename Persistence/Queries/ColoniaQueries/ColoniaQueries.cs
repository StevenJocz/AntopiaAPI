using Antopia.Domain.DTOs.ColinaDTOs;
using Antopia.Domain.DTOs.PublicationDTOs;
using Antopia.Domain.DTOs.UserDTOs;
using Antopia.Domain.Entities.PublicacionE;
using Antopia.Infrastructure;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Antopia.Persistence.Queries.ColoniaQueries
{
    public interface IColoniaQueries
    {
        Task<List<ColoniasUserDTOs>> ColoniasUser(int idUser);
        Task<List<ColoniaIdDTOs>> ColoniasId(int idColonia, int idUser);
        Task<List<ColoniaIdDTOs>> BuscarColonias(string colonia);
        Task<List<PerfilImagenesDTOs>> ImagenesColoniasId(int idColonia);
        Task<List<ColoniaTop>> ObtenerTresMejoresColoniasPorPuntos();
    }

    public class ColoniaQueries : IColoniaQueries, IDisposable
    {
        private readonly AntopiaDbContext _context = null;
        private readonly ILogger<ColoniaQueries> _logger;
        private readonly IConfiguration _configuration;

        public ColoniaQueries(ILogger<ColoniaQueries> logger, IConfiguration configuration)
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

        public async Task<List<ColoniasUserDTOs>> ColoniasUser(int idUser)
        {
            _logger.LogTrace("Iniciando metodo ColoniaQueries.ColoniasUser...");

            try
            {
                var esMiembro = await _context.MembersEs.AsNoTracking().Where(x => x.fk_tbl_user_members == idUser).ToListAsync();
                var gruposList = new List<ColoniasUserDTOs>();
                foreach (var miembro in esMiembro)
                {
                    var GruposUser = await _context.ColoniaEs.AsNoTracking().Where(x => x.id_colonies == miembro.fk_tbl_colonies).ToListAsync();
                    
                    if (GruposUser != null)
                    {
                        
                        foreach (var grupos in GruposUser)
                        {
                            var members = await _context.MembersEs.AsNoTracking().Where(x => x.fk_tbl_colonies == grupos.id_colonies).ToListAsync();
                            var list = new ColoniasUserDTOs
                            {
                                id_colonies = grupos.id_colonies,
                                s_name = grupos.s_name,
                                s_photo = grupos.s_photo,
                                colors = grupos.s_colors,
                                cantidadMembers = members.Count,
                                points = grupos.points

                            };
                            gruposList.Add(list);
                        }
                    }
                }
                
                return gruposList;

            }
            catch (Exception)
            {
                _logger.LogError("Error en el metodo ColoniaQueries.ColoniasUser...");
                throw;
            }
        }

        public async Task<List<ColoniaIdDTOs>> ColoniasId(int idColonia, int idUser)
        {
            _logger.LogTrace("Iniciando metodo ColoniaQueries.ColoniasId...");
            try
            {
                var coloniasList = new List<ColoniaIdDTOs>();
                var colonia = await _context.ColoniaEs.AsNoTracking().FirstOrDefaultAsync(x => x.id_colonies == idColonia);
                var datosCreador = await _context.UserEs.AsNoTracking().FirstOrDefaultAsync(x => x.id == colonia.fk_tbl_user_creator);
                var esMiembro = await _context.MembersEs.AsNoTracking().FirstOrDefaultAsync(x => x.fk_tbl_user_members == idUser && x.fk_tbl_colonies == idColonia);

                var membersList = new List<userMembers>();

                var members = await _context.MembersEs.AsNoTracking().Where(x => x.fk_tbl_colonies == idColonia).ToListAsync();

                foreach (var membersDatos in members)
                {
                    var datosmembers = await _context.UserEs.AsNoTracking().FirstOrDefaultAsync(x => x.id == membersDatos.fk_tbl_user_members);
                    var Siguiendo = await _context.FollowersEs.AsNoTracking().Where(x => x.id_user == datosmembers.id && x.id_follower == idUser).ToListAsync();

                    var listMembers = new userMembers
                    {
                        id_user = datosmembers.id,
                        foto = datosmembers.s_userPhoto,
                        nombre = datosmembers.s_user_name,
                        urluser = datosmembers.s_userProfile,
                        level = datosmembers.fk_tbl_level,
                        siguiendo = Siguiendo.Count,
                    };

                    membersList.Add(listMembers);
                }

                var list = new ColoniaIdDTOs
                {
                    id_colonies = colonia.id_colonies,
                    s_name = colonia.s_name,
                    s_description = colonia.s_description,
                    fk_tbl_user_creator = colonia.fk_tbl_user_creator,
                    name_creator = datosCreador.s_user_name,
                    photo_creator = datosCreador.s_userPhoto,
                    url_creator = datosCreador.s_userProfile,
                    level_creator = datosCreador.fk_tbl_level,
                    dt_creation = colonia.dt_creation,
                    s_photo = colonia.s_photo,
                    s_colors = colonia.s_colors,
                    cantidadMembers = members.Count,
                    esmember = (esMiembro != null) ? 1 : 0,
                    points = colonia.points,
                    userMembers = membersList
                };

                coloniasList.Add(list);

                return coloniasList;

            }

            catch (Exception)
            {
                _logger.LogError("Error en el metodo ColoniaQueries.ColoniasId...");
                throw;
            }
        
        }


        public async Task<List<ColoniaIdDTOs>> BuscarColonias(string colonia)
        {
            _logger.LogTrace("Iniciando metodo ColoniaQueries.BuscarColonias...");
            try
            {
                var coloniasList = new List<ColoniaIdDTOs>();
                var TodoColonia = colonia == "1"
                 ? await _context.ColoniaEs
                     .OrderBy(x => EF.Functions.Random())
                    .Take(15)
                    .AsNoTracking()
                    .ToListAsync()
                 : await _context.ColoniaEs
                     .Where(x => x.s_name != null && x.s_name.ToLower().Contains(colonia))
                     .AsNoTracking()
                     .ToListAsync();

                foreach (var item in TodoColonia)
                  {
                      var membersList = new List<userMembers>();

                      var members = await _context.MembersEs.AsNoTracking().Where(x => x.fk_tbl_colonies == item.id_colonies).ToListAsync();

                      foreach (var membersDatos in members)
                      {
                          var datosmembers = await _context.UserEs.AsNoTracking().FirstOrDefaultAsync(x => x.id == membersDatos.fk_tbl_user_members);

                          var listMembers = new userMembers
                          {
                              foto = datosmembers.s_userPhoto,
                          };

                          membersList.Add(listMembers);
                      }

                      var list = new ColoniaIdDTOs
                      {
                          id_colonies = item.id_colonies,
                          s_name = item.s_name,
                          s_photo = item.s_photo,
                          cantidadMembers = members.Count,
                          userMembers = membersList,
                          points = item.points,
                      };

                      coloniasList.Add(list);

                  }
                return coloniasList;
            }

            catch (Exception)
            {
                _logger.LogError("Error en el metodo ColoniaQueries.BuscarColonias...");
                throw;
            }

        }

        public async Task<List<PerfilImagenesDTOs>> ImagenesColoniasId(int idColonia)
        {
            _logger.LogTrace("Iniciando metodo ColoniaQueries.ImagenesColoniasId...");
            try
            {
                
                var IdPublication = await _context.ColoniaPublicationEs.AsNoTracking().Where(x => x.fk_tbl_colonies == idColonia).ToListAsync();

                var imagenesList = new List<PerfilImagenesDTOs>();
                if (IdPublication != null)
                {
                    foreach (var item in IdPublication)
                    {
                        var imagenesPublications = await _context.PublicationImageEs
                        .AsNoTracking()
                        .Where(x => x.fk_tbl_publication == item.fk_tbl_publication)
                        .OrderByDescending(x => x.id_publication_image) 
                        .ToListAsync();

                        if (imagenesPublications.Count > 0)
                        {
                            foreach (var imagenes in imagenesPublications)
                            {
                                var ImagenesDTOs = new PerfilImagenesDTOs
                                {
                                    idPublicacion = imagenes.fk_tbl_publication,
                                    url = imagenes.s_location,
                                };

                                imagenesList.Add(ImagenesDTOs);
                            }
                        }
                    }

                    return imagenesList;
                }
                return imagenesList;

            }

            catch (Exception)
            {
                _logger.LogError("Error en el metodo ColoniaQueries.ImagenesColoniasId...");
                throw;
            }

        }

        public async Task<List<ColoniaTop>> ObtenerTresMejoresColoniasPorPuntos()
        {
            _logger.LogTrace("Iniciando método ObtenerTresMejoresColoniasPorPuntos...");
            try
            {
                var coloniasList = new List<ColoniaTop>();

                var colonias = await _context.ColoniaEs
                    .OrderByDescending(c => c.points) 
                    .Take(4) 
                    .AsNoTracking()
                    .ToListAsync();

                foreach (var item in colonias)
                {
                    var list = new ColoniaTop
                    {
                        id_colonies = item.id_colonies,
                        s_name = item.s_name,
                        s_photo = item.s_photo,
                        points = item. points
                    };

                    coloniasList.Add(list);
                }

                return coloniasList;
            }
            catch (Exception)
            {
                _logger.LogError("Error en el método ObtenerTresMejoresColoniasPorPuntos...");
                throw;
            }
        }

    }
}
