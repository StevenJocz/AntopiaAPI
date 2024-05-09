using Antopia.Domain.Entities.ColoniaE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.ColinaDTOs
{
    public class ColoniaDTOs
    {
        public int id_colonies { get; set; }
        public string s_name { get; set; }
        public string s_description { get; set; }
        public int fk_tbl_user_creator { get; set; }
        public DateTime dt_creation { get; set; }
        public string s_photo { get; set; }
        public string s_colors { get; set; }
        public int points { get; set; }

        public static ColoniaDTOs CreateDTO(ColoniaE coloniaE)
        {
            ColoniaDTOs colinaDTOs = new()
            {
                id_colonies = coloniaE.id_colonies,
                s_name = coloniaE.s_name,
                s_description = coloniaE.s_description,
                fk_tbl_user_creator = coloniaE.fk_tbl_user_creator,
                dt_creation = coloniaE.dt_creation,
                s_photo = coloniaE.s_photo,
                s_colors = coloniaE.s_colors,
                points = coloniaE.points,
            };
            return colinaDTOs;
        }

        public static ColoniaE CreateE(ColoniaDTOs colinaDTOs)
        {
            ColoniaE coloniaE = new()
            {
                id_colonies = colinaDTOs.id_colonies,
                s_name = colinaDTOs.s_name,
                s_description = colinaDTOs.s_description,
                fk_tbl_user_creator = colinaDTOs.fk_tbl_user_creator,
                dt_creation = colinaDTOs.dt_creation,
                s_photo = colinaDTOs.s_photo,
                s_colors = colinaDTOs.s_colors,
                points = colinaDTOs.points,
            };
            return coloniaE;
        }
    }
}
