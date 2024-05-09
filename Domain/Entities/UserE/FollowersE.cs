using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.Entities.UserE
{
    [Table("tbl_followers")]
    public class FollowersE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_followers { get; set; }
        public int id_user { get; set; }
        public int id_follower { get; set; }
    }
}
