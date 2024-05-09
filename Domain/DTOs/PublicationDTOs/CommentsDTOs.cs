using Antopia.Domain.Entities.PublicacionE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.PublicationDTOs
{
    public class CommentsDTOs
    {
        public int id_publication_comments { get; set; }
        public int fk_tbl_publication { get; set; }
        public int fk_tbl_user { get; set; }
        public string s_comments { get; set; }
        public DateTime dt_creation { get; set; }
        public string Imagen { get; set; }

        public static CommentsDTOs CreateDTO(CommentsE commentsE)
        {
            CommentsDTOs commentsDTOs = new()
            {
                id_publication_comments = commentsE.id_publication_comments,
                fk_tbl_publication = commentsE.fk_tbl_publication,
                fk_tbl_user = commentsE.fk_tbl_user,
                s_comments = commentsE.s_comments,
                dt_creation = commentsE.dt_creation,
            };
            return commentsDTOs;
        }

        public static CommentsE CreateE(CommentsDTOs commentsDTOs)
        {
            CommentsE commentsE = new()
            {
                id_publication_comments = commentsDTOs.id_publication_comments,
                fk_tbl_publication = commentsDTOs.fk_tbl_publication,
                fk_tbl_user = commentsDTOs.fk_tbl_user,
                s_comments = commentsDTOs.s_comments,
                dt_creation = commentsDTOs.dt_creation,
            };
            return commentsE;
        }
    }
}
