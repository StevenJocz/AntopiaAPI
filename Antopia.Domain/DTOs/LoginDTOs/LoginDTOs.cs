using Antopia.Domain.Entities.LoginE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.LoginDTOs
{
    public class LoginDTOs
    {
        public int IdLogin { get; set; }
        public string userEmail { get; set; }
        public string userPassword { get; set; }
        public int fk_tblusers { get; set; }

        public static LoginDTOs CreateDTO(LoginE LoginE)
        {
            LoginDTOs LoginDTOs = new()
            {
                IdLogin = LoginE.IdLogin,
                userEmail = LoginE.s_userEmail,
                userPassword = LoginE.s_userPassword,
                fk_tblusers = LoginE.fk_tblusers,
            };
            return LoginDTOs;
        }

        public static LoginE CreateE(LoginDTOs LoginDTOs)
        {
            LoginE LoginE = new()
            {
                IdLogin = LoginDTOs.IdLogin,
                s_userEmail = LoginDTOs.userEmail,
                s_userPassword = LoginDTOs.userPassword,
                fk_tblusers = LoginDTOs.fk_tblusers,
            };
            return LoginE;
        }
    }
}
