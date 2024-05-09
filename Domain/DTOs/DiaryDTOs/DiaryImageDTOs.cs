using Antopia.Domain.Entities.DiaryE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.DiaryDTOs
{
    public class DiaryImageDTOs
    {
        public int id_diary_image { get; set; }
        public int fk_tbl_diary_entries { get; set; }
        public string s_location { get; set; }

        public static DiaryImageDTOs CreateDTO(DiaryImageE diaryImageE)
        {
            DiaryImageDTOs diaryImageDTOs = new()
            {
                id_diary_image = diaryImageE.id_diary_image,
                fk_tbl_diary_entries = diaryImageE.fk_tbl_diary_entries,
                s_location = diaryImageE.s_location,
            };
            return diaryImageDTOs;
        }

        public static DiaryImageE CreateE(DiaryImageDTOs diaryImageDTOs)
        {
            DiaryImageE diaryImageE = new()
            {
                id_diary_image = diaryImageDTOs.id_diary_image,
                fk_tbl_diary_entries = diaryImageDTOs.fk_tbl_diary_entries,
                s_location = diaryImageDTOs.s_location
            };
            return diaryImageE;
        }
    }
}
