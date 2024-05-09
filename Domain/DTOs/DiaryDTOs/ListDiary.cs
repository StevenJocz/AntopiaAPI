using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.DiaryDTOs
{
    public class ListDiary
    {
        public List<DiaryEntriesDTOs> DiaryEntries { get; set; }
        public List<DiaryImageDTOs> Imagenes { get; set; }
    }

    public class DiaryUser
    {
        public int idPerfil { get; set; }
        public int id { get; set; }
        public string diario { get; set; }
        public int megustas { get; set; }
        public int userLikes { get; set; }
        public int comentarios { get; set; }
        public List<DiaryRegistro> registros { get; set; }
        public List<DiaryComentarios> comentariosDiary { get; set; }
    }


    public class DiaryRegistro
    {
        public int idRegistro { get; set; }
        public int idDiary { get; set; }
        public string fecha { get; set; }
        public string contenido { get; set; }
        public List<string> imagen { get; set; }
    }


    public class DiaryComentarios
    {
        public int IdPerfilComentarios { get; set; }
        public string FechaComentario { get; set; }
        public string NombrePerfilComentarios { get; set; }
        public string ImagenPerfilComentarios { get; set; }
        public string Comentario { get; set; }
        public string urlPerfil { get; set; }
    }




}
