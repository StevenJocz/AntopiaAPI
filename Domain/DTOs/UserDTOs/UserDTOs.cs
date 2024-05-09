using Antopia.Domain.Entities.UserE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.UserDTOs
{
    public class UserDTOs
    {
        public int id { get; set; }
        public string s_user_name { get; set; }
        public int fk_user_address_city { get; set; }
        public string s_user_email { get; set; }
        public string Password { get; set; }
        public string s_userProfile { get; set; }
        public string s_userPhoto { get; set; }
        public string s_userFrontpage { get; set; }
        public string s_frase { get; set; }
        public int fk_tblRol { get; set; }
        public int fk_tbl_level { get; set; }

        public static UserDTOs CreateDTO(UserE UserE)
        {
            UserDTOs UserDTOs = new()
            {
                id = UserE.id,
                s_user_name = UserE.s_user_name,
                fk_user_address_city = UserE.fk_user_address_city,
                s_user_email = UserE.s_user_email,
                s_userProfile = UserE.s_userProfile,
                s_userPhoto = UserE.s_userPhoto,
                s_userFrontpage = UserE.s_userFrontpage,
                s_frase = UserE.s_frase,
                fk_tblRol = UserE.fk_tblRol,
                fk_tbl_level = UserE.fk_tbl_level,

            };
            return UserDTOs;
        }


        public static UserE CreateE(UserDTOs UserDTOs)
        {
            UserE UserE = new()
            {
                id = UserDTOs.id,
                s_user_name = UserDTOs.s_user_name,
                fk_user_address_city = UserDTOs.fk_user_address_city,
                s_user_email = UserDTOs.s_user_email,
                s_userProfile = UserDTOs.s_userProfile,
                s_userPhoto = UserDTOs.s_userPhoto,
                s_userFrontpage = UserDTOs.s_userFrontpage,
                s_frase = UserDTOs.s_frase,
                fk_tblRol = UserDTOs.fk_tblRol,
                fk_tbl_level = UserDTOs.fk_tbl_level,

            };
            return UserE;
        }
    }
}
