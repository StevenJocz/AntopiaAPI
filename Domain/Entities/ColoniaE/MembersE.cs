using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.Entities.ColoniaE
{
    [Table("tbl_colonies_members")]
    public class MembersE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_colonies_members { get; set; }
        public int fk_tbl_colonies { get; set; }
        public int fk_tbl_user_members { get; set; }
    }
}
