using Antopia.Domain.Entities.PublicacionE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.PublicationDTOs
{
    public class CommentsAnswercsDTOs
    {
        public int id_publication_comments_answer { get; set; }
        public int fk_tbl_publication_comments { get; set; }
        public int fk_tbl_user { get; set; }
        public string s_answer { get; set; }
        public DateTime dt_creation { get; set; }

        public static CommentsAnswercsDTOs CreateDTO(CommentsAnswercsE commentsAnswercsE)
        {
            CommentsAnswercsDTOs commentsAnswercsDTOs = new()
            {
                id_publication_comments_answer = commentsAnswercsE.id_publication_comments_answer,
                fk_tbl_publication_comments = commentsAnswercsE.fk_tbl_publication_comments,
                fk_tbl_user = commentsAnswercsE.fk_tbl_user,
                s_answer = commentsAnswercsE.s_answer,
                dt_creation = commentsAnswercsE.dt_creation,
            };
            return commentsAnswercsDTOs;
        }

        public static CommentsAnswercsE CreateE(CommentsAnswercsDTOs commentsAnswercsDTOs)
        {
            CommentsAnswercsE commentsAnswercsE = new()
            {
                id_publication_comments_answer = commentsAnswercsDTOs.id_publication_comments_answer,
                fk_tbl_publication_comments = commentsAnswercsDTOs.fk_tbl_publication_comments,
                fk_tbl_user = commentsAnswercsDTOs.fk_tbl_user,
                s_answer = commentsAnswercsDTOs.s_answer,
                dt_creation = commentsAnswercsDTOs.dt_creation,
            };
            return commentsAnswercsE;
        }
    }
}
