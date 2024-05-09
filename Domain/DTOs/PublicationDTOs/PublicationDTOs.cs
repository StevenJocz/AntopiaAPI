
using Antopia.Domain.Entities.PublicacionE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.PublicationDTOs
{
    public class PublicationDTOs
    {
        public int id_publication { get; set; }
        public DateTime dt_creation { get; set; }
        public string s_title { get; set; }
        public string s_content { get; set; }
        public int fk_tbl_user { get; set; }
        public int fk_tbl_type_publication { get; set; }
        public string s_hashtags { get; set; }
        public int fk_tbl_colinie { get; set; }
        public int byte_blocked { get; set; }
        

        public static PublicationDTOs CreateDTO(PublicationE publicationE)
        {
            PublicationDTOs publicationDTOs = new()
            {
                id_publication = publicationE.id_publication,
                dt_creation = publicationE.dt_creation,
                s_title = publicationE.s_title,
                s_content   = publicationE.s_content,
                fk_tbl_user = publicationE.fk_tbl_user,
                fk_tbl_type_publication = publicationE.fk_tbl_type_publication,
                s_hashtags = publicationE.s_hashtags,
                byte_blocked = publicationE.byte_blocked,
            };
            return publicationDTOs;
        }

        public static PublicationE CreateE(PublicationDTOs publicationDTOs)
        {
            PublicationE publicationE = new()
            {
                id_publication = publicationDTOs.id_publication,
                dt_creation = publicationDTOs.dt_creation,
                s_title = publicationDTOs.s_title,
                s_content = publicationDTOs.s_content,
                fk_tbl_user = publicationDTOs.fk_tbl_user,
                fk_tbl_type_publication = publicationDTOs.fk_tbl_type_publication,
                s_hashtags = publicationDTOs.s_hashtags,
                byte_blocked = publicationDTOs.byte_blocked,
            };
            return publicationE;
        }

    }
}
