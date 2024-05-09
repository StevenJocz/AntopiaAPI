using Antopia.Domain.Entities.PublicacionE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.PublicationDTOs
{
    public class LikeCommentsDTOs
    {
        public int id_publication_comments_like { get; set; }
        public int fk_tbl_user { get; set; }
        public int fk_tbl_publication_comments { get; set; }
        public int is_like { get; set; }

        public static LikeCommentsDTOs CreateDTO(LikeCommentsE likeCommentsE)
        {
            LikeCommentsDTOs likeCommentsDTOs = new()
            {
                id_publication_comments_like = likeCommentsE.id_publication_comments_like,
                fk_tbl_user = likeCommentsE.fk_tbl_user,
                fk_tbl_publication_comments = likeCommentsE.fk_tbl_publication_comments,
            };
            return likeCommentsDTOs;
        }

        public static LikeCommentsE CreateE(LikeCommentsDTOs likeCommentsDTOs)
        {
            LikeCommentsE likeCommentsE = new()
            {
                id_publication_comments_like = likeCommentsDTOs.id_publication_comments_like,
                fk_tbl_user = likeCommentsDTOs.fk_tbl_user,
                fk_tbl_publication_comments = likeCommentsDTOs.fk_tbl_publication_comments,
            };
            return likeCommentsE;
        }
    }
}
