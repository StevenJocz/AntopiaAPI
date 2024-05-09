using Antopia.Domain.Entities.PublicacionE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.PublicationDTOs
{
    public class CommentsLikeDTOs
    {
        public int id_publication_like { get; set; }
        public int fk_tbl_user { get; set; }
        public int fk_tbl_publication { get; set; }
        public int is_like { get; set; }

        public static CommentsLikeDTOs CreateDTO(CommentsLikeE commentsLikeE)
        {
            CommentsLikeDTOs commentsLikeDTOs = new()
            {
                id_publication_like = commentsLikeE.id_publication_like,
                fk_tbl_user = commentsLikeE.fk_tbl_user,
                fk_tbl_publication = commentsLikeE.fk_tbl_publication,
            };
            return commentsLikeDTOs;
        }

        public static CommentsLikeE CreateE(CommentsLikeDTOs commentsLikeDTOs)
        {
            CommentsLikeE commentsLikeE = new()
            {
                id_publication_like = commentsLikeDTOs.id_publication_like,
                fk_tbl_user = commentsLikeDTOs.fk_tbl_user,
                fk_tbl_publication = commentsLikeDTOs.fk_tbl_publication,
            };
            return commentsLikeE;
        }
    }
}
