using Antopia.Domain.Entities.UserE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.UserDTOs
{
    public class LevelDTOs
    {
        public int id_level { get; set; }
        public string s_level { get; set; }


        public static LevelDTOs CreateDTO(LevelE levelE)
        {
            LevelDTOs levelDTOs = new()
            {
                id_level = levelE.id_level,
                s_level = levelE.s_level,
                
            };
            return levelDTOs;
        }


        public static LevelE CreateE(LevelDTOs levelDTOs)
        {
            LevelE levelE = new()
            {
                id_level = levelDTOs.id_level,
                s_level = levelDTOs.s_level,

            };
            return levelE;
        }
    }
}
