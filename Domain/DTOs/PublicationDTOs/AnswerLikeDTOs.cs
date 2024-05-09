using Antopia.Domain.Entities.PublicacionE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.PublicationDTOs
{
    public class AnswerLikeDTOs
    {
        public int id_publication_comments_answer_like { get; set; }
        public int fk_tbl_user { get; set; }
        public int fk_tbl_publication_comments_answer { get; set; }
        public int is_like { get; set; }

        public static AnswerLikeDTOs CreateDTO(AnswerLikeE answerLikeE)
        {
            AnswerLikeDTOs answerLikeDTOs = new()
            {
                id_publication_comments_answer_like = answerLikeE.id_publication_comments_answer_like,
                fk_tbl_user = answerLikeE.fk_tbl_user,
                fk_tbl_publication_comments_answer = answerLikeE.fk_tbl_publication_comments_answer,
            };
            return answerLikeDTOs;
        }

        public static AnswerLikeE CreateE(AnswerLikeDTOs answerLikeDTOs)
        {
            AnswerLikeE answerLikeE = new()
            {
                id_publication_comments_answer_like = answerLikeDTOs.id_publication_comments_answer_like,
                fk_tbl_user = answerLikeDTOs.fk_tbl_user,
                fk_tbl_publication_comments_answer = answerLikeDTOs.fk_tbl_publication_comments_answer,
            };
            return answerLikeE;
        }
    }
}
