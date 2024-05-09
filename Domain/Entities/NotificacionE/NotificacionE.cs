using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.Entities.NotificacionE
{
    [Table("tbl_notification")]
    public class NotificacionE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_notification { get; set; }
        public int type_notification { get; set; }
        public int for_user { get; set; }
        public int of_user { get; set; }
        public DateTime data_created { get; set; }
        public bool state { get; set; }
        public int fk_tbl_publication { get; set; }
        public int fk_tbl_diary { get; set; }
        public int fk_tbl_colonie { get; set; }

    }
}
