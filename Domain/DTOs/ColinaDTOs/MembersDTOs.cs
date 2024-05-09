using Antopia.Domain.Entities.ColoniaE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.ColinaDTOs
{
    public class MembersDTOs
    {
        public int id_colonies_members { get; set; }
        public int fk_tbl_colonies { get; set; }
        public int fk_tbl_user_members { get; set; }
        public int esMember { get; set; }

        public static MembersDTOs CreateDTO(MembersE membersE)
        {
            MembersDTOs membersDTOs = new()
            {
                id_colonies_members = membersE.id_colonies_members,
                fk_tbl_colonies = membersE.fk_tbl_colonies,
                fk_tbl_user_members = membersE.fk_tbl_user_members,
            };
            return membersDTOs;
        }

        public static MembersE CreateE(MembersDTOs membersDTOs)
        {
            MembersE membersE = new()
            {
                id_colonies_members = membersDTOs.id_colonies_members,
                fk_tbl_colonies = membersDTOs.fk_tbl_colonies,
                fk_tbl_user_members = membersDTOs.fk_tbl_user_members,
            };
            return membersE;
        }
    }
}
