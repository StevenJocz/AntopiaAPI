using Antopia.Domain.DTOs.ColinaDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.PublicationDTOs
{
    public class TodoPublicationDTOs
    {
        public int IdPerfil { get; set; }
        public string NombrePerfil { get; set; }
        public string urlPerfil { get; set; }
        public string ImagenPerfil { get; set; }
        public int IdPublicacion { get; set; }
        public int IdTipo { get; set; }
        public int IdColonia { get; set; }
        public int esMiembroColonia { get; set; }
        public int Megustas { get; set; }
        public int CantidadComentarios { get; set; }
        public int Siguiendo { get; set; }
        public string FechaPublicacion { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public string UrlYoutube { get; set; }
        public int userLike { get; set; }
        public int level { get; set; }
        public List<string> ImagenesPublicacion { get; set; }
        public List<ComentarioPublication> Comentarios { get; set; }
        public List<ColoniasUserDTOs> InfoColonia { get; set; }

    }

    public class ComentarioPublication
    {
        public int IdComentarios { get; set; }
        public int IdPerfilComentarios { get; set; }
        public string FechaComentario { get; set; }
        public string NombrePerfilComentarios { get; set; }
        public string urlPerfil { get; set; }
        public string ImagenPerfilComentarios { get; set; }
        public string Comentario { get; set; }
        public string imagenComentario { get; set; }
        public int megustaComentarios { get; set; }
        public int userLike { get; set; }
        public List<ComentarioRespuesta> ComentariosRespuesta { get; set; }

    }

    public class ComentarioRespuesta
    {
        public int IdComentarios { get; set; }
        public int IdResponse { get; set; }
        public int IdPerfilComentarios { get; set; }
        public string FechaComentario { get; set; }
        public string NombrePerfilComentarios { get; set; }
        public string urlPerfil { get; set; }
        public string ImagenPerfilComentarios { get; set; }
        public string Comentario { get; set; }
        public int megustaComentarios { get; set; }
        public int userLike { get; set; }
    }
}
