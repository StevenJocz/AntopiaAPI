using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.Entities.PublicacionE
{
    [Table("tbl_publication_comments_imagen")]
    public class CommentsImagenE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_publication_comments_imagen { get; set; }
        public int fk_tbl__publication_comments { get; set; }
        public string s_location { get; set; }
    }
}
