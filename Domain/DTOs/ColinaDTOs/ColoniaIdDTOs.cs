using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.ColinaDTOs
{
    public class ColoniaIdDTOs
    {
        public int id_colonies { get; set; }
        public string s_name { get; set; }
        public string s_description { get; set; }
        public int fk_tbl_user_creator { get; set; }
        public string name_creator { get; set; }
        public string photo_creator { get; set; }
        public string url_creator { get; set; }
        public int level_creator { get; set; }
        public DateTime dt_creation { get; set; }
        public string s_photo { get; set; }
        public string s_colors { get; set; }
        public int esmember { get; set; }
        public int cantidadMembers { get; set; }
        public int points { get; set; }
        public List<userMembers> userMembers { get; set; }
    }

    public class userMembers
    {
        public int id_user { get; set; }
        public string foto { get; set; }
        public string nombre { get; set; }
        public string urluser { get; set; }
        public int level { get; set; }
        public int siguiendo { get; set; }

    }

    public class ColoniaTop
    {
        public int id_colonies { get; set; }
        public string s_name { get; set; }
        public string s_photo { get; set; }
        public int  points { get; set; }
    }
}
