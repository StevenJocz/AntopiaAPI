using Antopia.Domain.Entities.PublicacionE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.PublicationDTOs
{
    public class PublicationVideoDTOs
    {
        public int id_publication_video { get; set; }
        public DateTime date_creation { get; set; }
        public int fk_tbl_publication { get; set; }
        public string s_url { get; set; }

        public static PublicationVideoDTOs CreateDTO(PublicationVideoE publicationVideoE)
        {
            PublicationVideoDTOs publicationVideoDTOs = new()
            {
                id_publication_video = publicationVideoE.id_publication_video,
                date_creation = publicationVideoE.date_creation,
                fk_tbl_publication = publicationVideoE.fk_tbl_publication,
                s_url = publicationVideoE.s_url
            };
            return publicationVideoDTOs;
        }

        public static PublicationVideoE CreateE(PublicationVideoDTOs publicationVideoDTOs)
        {
            PublicationVideoE publicationVideoE = new()
            {
                id_publication_video = publicationVideoDTOs.id_publication_video,
                date_creation = publicationVideoDTOs.date_creation,
                fk_tbl_publication = publicationVideoDTOs.fk_tbl_publication,
                s_url = publicationVideoDTOs.s_url
            };
            return publicationVideoE;
        }
    }
}
