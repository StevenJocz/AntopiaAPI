using Antopia.Domain.Entities.PublicacionE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Antopia.Domain.DTOs.PublicationDTOs
{
    public class ListPublicationDTOs
    {
        public List<PublicationDTOs> Publicaciones { get; set; }
        public List<PublicationImageDTOs> Imagenes { get; set; }
        public List<PublicationVideoDTOs> Videos { get; set; }
    }
}
