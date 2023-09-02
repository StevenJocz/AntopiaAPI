using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.Entities.PublicacionE
{
    [Table("tbl_publication_video")]
    public class PublicationVideoE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_publication_video { get; set; }
        public DateTime date_creation { get; set; }
        public int fk_tbl_publication { get; set; }
        public string s_url { get; set; }
    }
}
