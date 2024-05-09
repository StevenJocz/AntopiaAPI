using Antopia.Domain.Entities.PublicacionE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.PublicationDTOs
{
    public class CommentsImagenDTOs
    {
        public int id_publication_comments_imagen { get; set; }
        public int fk_tbl__publication_comments { get; set; }
        public string s_location { get; set; }

        public static CommentsImagenDTOs CreateDTO(CommentsImagenE commentsImagenE)
        {
            CommentsImagenDTOs commentsImagenDTOs = new()
            {
                id_publication_comments_imagen = commentsImagenE.id_publication_comments_imagen,
                fk_tbl__publication_comments = commentsImagenE.fk_tbl__publication_comments,
                s_location = commentsImagenE.s_location,
            };
            return commentsImagenDTOs;
        }

        public static CommentsImagenE CreateE(CommentsImagenDTOs commentsImagenDTOs)
        {
            CommentsImagenE commentsImagenE = new()
            {
                id_publication_comments_imagen = commentsImagenDTOs.id_publication_comments_imagen,
                fk_tbl__publication_comments = commentsImagenDTOs.fk_tbl__publication_comments,
                s_location = commentsImagenDTOs.s_location,
            };
            return commentsImagenE;
        }
    }
}
