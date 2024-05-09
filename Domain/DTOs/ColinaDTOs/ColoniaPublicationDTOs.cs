using Antopia.Domain.Entities.ColoniaE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.ColinaDTOs
{
    public class ColoniaPublicationDTOs
    {
        public int id_colonies_publication { get; set; }
        public int fk_tbl_publication { get; set; }
        public int fk_tbl_colonies { get; set; }

        public static ColoniaPublicationDTOs CreateDTO(ColoniaPublicationE coloniaPublicationE)
        {
            ColoniaPublicationDTOs coloniaPublicationDTOs = new()
            {
                id_colonies_publication = coloniaPublicationE.id_colonies_publication,
                fk_tbl_publication = coloniaPublicationE.fk_tbl_publication,
                fk_tbl_colonies = coloniaPublicationE.fk_tbl_colonies,
            };
            return coloniaPublicationDTOs;
        }

        public static ColoniaPublicationE CreateE(ColoniaPublicationDTOs coloniaPublicationDTOs)
        {
            ColoniaPublicationE coloniaPublicationE = new()
            {
                id_colonies_publication = coloniaPublicationDTOs.id_colonies_publication,
                fk_tbl_publication = coloniaPublicationDTOs.fk_tbl_publication,
                fk_tbl_colonies = coloniaPublicationDTOs.fk_tbl_colonies,
            };
            return coloniaPublicationE;
        }
    }
}
