using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.Entities.PublicacionE
{
    [Table("tbl_publication_like")]
    public class CommentsLikeE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_publication_like { get; set; }
        public int fk_tbl_user { get; set; }
        public int fk_tbl_publication { get; set; }
    }
}
