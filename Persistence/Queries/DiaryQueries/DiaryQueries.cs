using Antopia.Domain.DTOs.DiaryDTOs;
using Antopia.Domain.DTOs.PublicationDTOs;
using Antopia.Domain.Entities.ColoniaE;
using Antopia.Domain.Entities.DiaryE;
using Antopia.Domain.Entities.PublicacionE;
using Antopia.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Antopia.Persistence.Queries.DiaryQueries
{
    public interface IDiaryQueries
    {
        Task<List<DiaryUser>> ListarDiarios(int idUser, int userConsulta);
    }

    public class DiaryQueries : IDiaryQueries, IDisposable
    {
        private readonly AntopiaDbContext _context = null;
        private readonly ILogger<DiaryQueries> _logger;
        private readonly IConfiguration _configuration;

        public DiaryQueries(ILogger<DiaryQueries> logger, IConfiguration configuration)
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

        public async Task<List<DiaryUser>> ListarDiarios(int idUser, int userConsulta)
        {
            _logger.LogTrace("Iniciando metodo DiaryQueries.ListarDiarios...");
            try
            {
                var Diarios = await _context.DiaryEs.AsNoTracking().Where(x => x.fk_tbl_user == idUser).ToListAsync();

                var todoList = new List<DiaryUser>();

                foreach (var diary in Diarios)
                {
                    var RegistroDiarios = await _context.DiaryEntriesEs.AsNoTracking().Where(x => x.fk_tbl_diary == diary.id_diary).ToListAsync();

                    var like = await _context.DiaryLikeEs.AsNoTracking().Where(x => x.fk_tbl_diary == diary.id_diary).ToListAsync();

                    var userlike = await _context.DiaryLikeEs.AsNoTracking().Where(x => x.fk_tbl_diary == diary.id_diary && x.fk_tbl_user == userConsulta).ToListAsync();

                    var cantidadComentarios = await _context.CommentDiaryEs.AsNoTracking().Where(x => x.fk_tbl_diary == diary.id_diary).ToListAsync();

                    var todoListRegistros = new List<DiaryRegistro>();

                    foreach (var diaryRegistros in RegistroDiarios)
                    {
                        var imagenes = await _context.DiaryImageEs.AsNoTracking().Where(x => x.fk_tbl_diary_entries == diaryRegistros.id_diary_entries).ToListAsync();

                        var listregistros = new DiaryRegistro
                        {
                            idRegistro = diaryRegistros.id_diary_entries,
                            idDiary = diaryRegistros.fk_tbl_diary,
                            fecha = diaryRegistros.dt_creation.ToString("yyyy-MM-dd HH:mm:ss"),
                            contenido = diaryRegistros.s_content,
                            imagen = imagenes.Select(ip => ip.s_location).ToList()
                        };


                        todoListRegistros.Add(listregistros);
                    }

                    var Comentarios = await _context.CommentDiaryEs.AsNoTracking().Where(x => x.fk_tbl_diary == diary.id_diary).ToListAsync();

                    var todoListComentarios = new List<DiaryComentarios>();

                    foreach (var diaryComentarios in Comentarios)
                    {
                        var datosUserComments = await _context.UserEs.AsNoTracking().FirstOrDefaultAsync(x => x.id == diaryComentarios.fk_tbl_user);

                        var listComentarios = new DiaryComentarios
                        {
                            IdPerfilComentarios = datosUserComments.id,
                            FechaComentario = diaryComentarios.dt_creation.ToString("yyyy-MM-dd HH:mm:ss"),
                            NombrePerfilComentarios = datosUserComments.s_user_name,
                            urlPerfil = datosUserComments.s_userProfile,
                            ImagenPerfilComentarios = datosUserComments.s_userPhoto,
                            Comentario = diaryComentarios.s_comments,
                        };


                        todoListComentarios.Add(listComentarios);
                    }

                    var list = new DiaryUser
                    {
                        idPerfil = diary.fk_tbl_user,
                        id = diary.id_diary,
                        diario = diary.s_name,
                        megustas = like.Count,
                        userLikes = userlike.Count,
                        comentarios = cantidadComentarios.Count,
                        registros = todoListRegistros,
                        comentariosDiary = todoListComentarios
                    };

                    todoList.Add(list);

                }
                
                return todoList;
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar DiaryQueries.ListarDiarios...");
                throw;
            }
        }


    }
}
