using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.Entities.PublicacionE
{
    [Table("tbl_publication_image")]
    public class PublicationImageE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_publication_image { get; set; }
        public DateTime date_creation { get; set; }
        public int fk_tbl_publication { get; set; }
        public string s_location { get; set; }
        public int fk_tbl_user { get; set; }
    }
}
