using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.Entities.ColoniaE
{
    [Table("tbl_colonies_publications_imagen")]
    public class ColoniaPublicationImageE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_colonies_publications_image { get; set; }
        public int fk_tbl_colonies { get; set; }
        public DateTime data_creation { get; set; }
        public string s_location { get; set; }
    }
}
