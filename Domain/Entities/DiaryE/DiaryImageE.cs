using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.Entities.DiaryE
{
    [Table("tbl_diary_image")]
    public class DiaryImageE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_diary_image { get; set; }
        public int fk_tbl_diary_entries { get; set; }
        public string s_location { get; set; }
    }
}
