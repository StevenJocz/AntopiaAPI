using Antopia.Domain.Entities.DiaryE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.DiaryDTOs
{
    public class DiaryLikeDTOs
    {
        public int id_diary_like { get; set; }
        public int fk_tbl_diary { get; set; }
        public int fk_tbl_user { get; set; }
        public int is_like { get; set; }

        public static DiaryLikeDTOs CreateDTO(DiaryLikeE diaryLikeE)
        {
            DiaryLikeDTOs diaryLikeDTOs = new()
            {
                id_diary_like = diaryLikeE.id_diary_like,
                fk_tbl_diary = diaryLikeE.fk_tbl_diary,
                fk_tbl_user = diaryLikeE.fk_tbl_user
            };
            return diaryLikeDTOs;
        }

        public static DiaryLikeE CreateE(DiaryLikeDTOs diaryLikeDTOs)
        {
            DiaryLikeE diaryLikeE = new()
            {
                id_diary_like = diaryLikeDTOs.id_diary_like,
                fk_tbl_diary = diaryLikeDTOs.fk_tbl_diary,
                fk_tbl_user = diaryLikeDTOs.fk_tbl_user
            };
            return diaryLikeE;
        }
    }
}
