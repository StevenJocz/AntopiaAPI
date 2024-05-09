using Antopia.Domain.Entities.DiaryE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.DiaryDTOs
{
    public class CommentDiaryDTOs
    {
        public int id_diary_comments { get; set; }
        public int fk_tbl_diary { get; set; }
        public int fk_tbl_user { get; set; }
        public string s_comments { get; set; }
        public DateTime dt_creation { get; set; }

        public static CommentDiaryDTOs CreateDTO(CommentDiaryE commentDiaryE)
        {
            CommentDiaryDTOs commentDiaryDTOs = new()
            {
                id_diary_comments = commentDiaryE.id_diary_comments,
                fk_tbl_diary = commentDiaryE.fk_tbl_diary,
                fk_tbl_user = commentDiaryE.fk_tbl_user,
                s_comments = commentDiaryE.s_comments,
                dt_creation = commentDiaryE.dt_creation
            };
            return commentDiaryDTOs;
        }

        public static CommentDiaryE CreateE(CommentDiaryDTOs commentDiaryDTOs)
        {
            CommentDiaryE commentDiaryE = new()
            {
                id_diary_comments = commentDiaryDTOs.id_diary_comments,
                fk_tbl_diary = commentDiaryDTOs.fk_tbl_diary,
                fk_tbl_user = commentDiaryDTOs.fk_tbl_user,
                s_comments = commentDiaryDTOs.s_comments,
                dt_creation = commentDiaryDTOs.dt_creation
            };
            return commentDiaryE;
        }
    }
}
