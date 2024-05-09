using Antopia.Domain.Entities.ColoniaE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.ColinaDTOs
{
    public class ColoniaPublicationImageDTOs
    {
        public int id_colonies_publications_image { get; set; }
        public int fk_tbl_colonies { get; set; }
        public DateTime data_creation { get; set; }
        public string s_location { get; set; }

        public static ColoniaPublicationImageDTOs CreateDTO(ColoniaPublicationImageE coloniaPublicationImageE)
        {
            ColoniaPublicationImageDTOs coloniaPublicationImageDTOs = new()
            {
                id_colonies_publications_image = coloniaPublicationImageE.id_colonies_publications_image,
                fk_tbl_colonies = coloniaPublicationImageE.fk_tbl_colonies,
                data_creation = coloniaPublicationImageE.data_creation,
                s_location = coloniaPublicationImageE.s_location,
            };
            return coloniaPublicationImageDTOs;
        }

        public static ColoniaPublicationImageE CreateE(ColoniaPublicationImageDTOs coloniaPublicationImageDTOs)
        {
            ColoniaPublicationImageE coloniaPublicationImageE = new()
            {
                id_colonies_publications_image = coloniaPublicationImageDTOs.id_colonies_publications_image,
                fk_tbl_colonies = coloniaPublicationImageDTOs.fk_tbl_colonies,
                data_creation = coloniaPublicationImageDTOs.data_creation,
                s_location = coloniaPublicationImageDTOs.s_location,
            };
            return coloniaPublicationImageE;
        }
    }
}
