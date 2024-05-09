using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.Entities.UserE
{
    [Table("tbl_users")]
    public class UserE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string s_user_name { get; set; }
        public int fk_user_address_city { get; set; }
        public string s_user_email { get; set; }
        public string s_userProfile { get; set; }
        public string s_userPhoto { get; set; }
        public string s_userFrontpage { get; set; }
        public string s_frase { get; set; }
        public int fk_tbl_level { get; set; }
        public int fk_tblRol { get; set; }

    }
}
