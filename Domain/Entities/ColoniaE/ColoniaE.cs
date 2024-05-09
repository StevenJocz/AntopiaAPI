using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.Entities.ColoniaE
{
    [Table("tbl_colonies")]
    public class ColoniaE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_colonies { get; set; }
        public string s_name { get; set; }
        public string s_description { get; set; }
        public int fk_tbl_user_creator { get; set; }
        public DateTime dt_creation { get; set; }
        public string s_photo { get; set; }
        public string s_colors { get; set; }
        public int points { get; set; }

    }
}
