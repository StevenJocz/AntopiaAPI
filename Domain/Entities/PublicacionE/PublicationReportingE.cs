using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.Entities.PublicacionE
{
    [Table("tbl_publication_reporting")]
    public class PublicationReportingE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_publication_reporting { get; set; }
        public int fk_tbl_publication_reporting_reason { get; set; }
        public int fk_tbl_publication { get; set; }
    }

    [Table("tbl_publication_reporting_reason")]
    public class Publication_reporting_reasonE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_publication_reporting_reason { get; set; }
        public string s_title { get; set; }
        public string s_description { get; set; }
        public int byte_activo { get; set; }
    }
}
