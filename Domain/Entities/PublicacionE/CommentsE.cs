using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.Entities.PublicacionE
{
    [Table("tbl_publication_comments")]
    public class CommentsE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_publication_comments { get; set; }
        public int fk_tbl_publication { get; set; }
        public int fk_tbl_user { get; set; }
        public string s_comments { get; set; }
        public DateTime dt_creation { get; set; }
    }
}
