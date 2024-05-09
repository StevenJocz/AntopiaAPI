using Antopia.Domain.Entities.DiaryE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.DiaryDTOs
{
    public class DiaryDTOs
    {
        public int id_diary { get; set; }
        public int fk_tbl_user { get; set; }
        public string s_name { get; set; }

        public static DiaryDTOs CreateDTO(DiaryE diaryE)
        {
            DiaryDTOs diaryDTOs = new()
            {
                id_diary = diaryE.id_diary,
                fk_tbl_user = diaryE.fk_tbl_user,
                s_name = diaryE.s_name,
            };
            return diaryDTOs;
        }

        public static DiaryE CreateE(DiaryDTOs diaryDTOs)
        {
            DiaryE diaryE = new()
            {
                id_diary = diaryDTOs.id_diary,
                fk_tbl_user = diaryDTOs.fk_tbl_user,
                s_name = diaryDTOs.s_name,
            };
            return diaryE;
        }
    }
}
