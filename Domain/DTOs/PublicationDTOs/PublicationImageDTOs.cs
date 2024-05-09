
using Antopia.Domain.Entities.PublicacionE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.PublicationDTOs
{
    public class PublicationImageDTOs
    {
        public int id_publication_image { get; set; }
        public DateTime date_creation { get; set; }
        public int fk_tbl_publication { get; set; }
        public string s_location { get; set; }
        public int fk_tbl_user { get; set; }

        public static PublicationImageDTOs CreateDTO(PublicationImageE publicationImageE)
        {
            PublicationImageDTOs publicationImageDTOs = new()
            {
                id_publication_image = publicationImageE.id_publication_image,
                date_creation = publicationImageE.date_creation,
                fk_tbl_publication = publicationImageE.fk_tbl_publication,
                s_location = publicationImageE.s_location,
                fk_tbl_user = publicationImageE.fk_tbl_user,
            };
            return publicationImageDTOs;
        }

        public static PublicationImageE CreateE(PublicationImageDTOs publicationImageDTOs)
        {
            PublicationImageE publicationImageE = new()
            {
                id_publication_image = publicationImageDTOs.id_publication_image,
                date_creation = publicationImageDTOs.date_creation,
                fk_tbl_publication = publicationImageDTOs.fk_tbl_publication,
                s_location = publicationImageDTOs.s_location,
                fk_tbl_user = publicationImageDTOs.fk_tbl_user,
            };
            return publicationImageE;
        }
    }
}
