using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.Entities.NotificacionE
{
    [Table("tbl_notification_type")]
    public class TypeNotificacionE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_notification_type { get; set; }
        public string s_name { get; set; }
        public string s_content { get; set; }
    }
}
