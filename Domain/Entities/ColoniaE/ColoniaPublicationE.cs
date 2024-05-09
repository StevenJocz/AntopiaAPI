using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.Entities.ColoniaE
{
    [Table("trel_colonies_publication")]
    public class ColoniaPublicationE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_colonies_publication { get; set; }
        public int fk_tbl_publication { get; set; }
        public int fk_tbl_colonies { get; set; }
    }
}
