using Antopia.Domain.DTOs.ColinaDTOs;
using Antopia.Domain.Entities.ColoniaE;
using Antopia.Domain.Entities.PublicacionE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.PublicationDTOs
{
    public class PublicationShareDTOs
    {
        public int id_publication_share_colonia { get; set; }
        public int fk_tbl_publication { get; set; }
        public int fk_tbl_colonies { get; set; }

        public static PublicationShareDTOs CreateDTO(PublicationShareE publicationShareE)
        {
            PublicationShareDTOs publicationShareDTOs = new()
            {
                id_publication_share_colonia = publicationShareE.id_publication_share_colonia,
                fk_tbl_publication = publicationShareE.fk_tbl_publication,
                fk_tbl_colonies = publicationShareE.fk_tbl_colonies,
            };
            return publicationShareDTOs;
        }

        public static PublicationShareE CreateE(PublicationShareDTOs publicationShareDTOs)
        {
            PublicationShareE publicationShareE = new()
            {
                id_publication_share_colonia = publicationShareDTOs.id_publication_share_colonia,
                fk_tbl_publication = publicationShareDTOs.fk_tbl_publication,
                fk_tbl_colonies = publicationShareDTOs.fk_tbl_colonies,
            };
            return publicationShareE;
        }
    }
}
