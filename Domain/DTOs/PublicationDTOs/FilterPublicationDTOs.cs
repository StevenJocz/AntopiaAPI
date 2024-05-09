using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.PublicationDTOs
{
    public class FilterPublicationDTOs
    {
        public string NombrePerfil { get; set; }
        public string ImagenPerfil { get; set; }
        public int IdPublicacion { get; set; }
        public int IdTipo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
       
    }

    public class TopPublicationDTOs
    {
        public int IdPublicacion { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public string Titulo { get; set; }
        public int Likes { get; set; }
        public int IdUser { get; set; }
        public string NombrePerfil { get; set; }
        public string Foto { get; set; }
        public string Url { get; set; }
    }


}
