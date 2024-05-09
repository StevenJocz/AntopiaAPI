using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.ColinaDTOs
{
    public class ColoniasUserDTOs
    {
        public int id_colonies { get; set; }
        public string s_name { get; set; }
        public string s_photo { get; set; }
        public string colors { get; set; }
        public int cantidadMembers { get; set; }
        public int points { get; set; }
    }
}
