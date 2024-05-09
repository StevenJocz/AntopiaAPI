using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.Entities.PublicacionE
{
    [Table("tbl_publications")]
    public class PublicationE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_publication { get; set; }
        public DateTime dt_creation { get; set; }
        public string s_title { get; set; }
        public string s_content { get; set; }
        public int fk_tbl_user { get; set; }
        public int fk_tbl_type_publication { get; set; }
        public string? s_hashtags { get; set; }
        public int byte_blocked { get; set; }
    }
}
