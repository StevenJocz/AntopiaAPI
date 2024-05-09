using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.Entities.DiaryE
{
    [Table("tbl_diary_like")]
    public class DiaryLikeE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_diary_like { get; set; }
        public int fk_tbl_diary { get; set; }
        public int fk_tbl_user { get; set; }
    }
}
