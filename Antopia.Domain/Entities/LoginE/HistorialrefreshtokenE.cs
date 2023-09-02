using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.Entities.LoginE
{
    [Table("tbl_login_historial_token")]
    public class HistorialrefreshtokenE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idhistorialtoken { get; set; }
        public int idusuario { get; set; }
        public string s_token { get; set; }
        public DateTime ts_fechacreacion { get; set; }
        public DateTime ts_fechaexpiracion { get; set; }
    }
}
