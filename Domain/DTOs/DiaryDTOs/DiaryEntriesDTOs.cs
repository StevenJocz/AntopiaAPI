using Antopia.Domain.Entities.DiaryE;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.DiaryDTOs
{
    public class DiaryEntriesDTOs
    {
        public int id_diary_entries { get; set; }
        public int fk_tbl_diary { get; set; }
        public string s_content { get; set; }
        public DateTime dt_creation { get; set; }

        public static DiaryEntriesDTOs CreateDTO(DiaryEntriesE diaryEntriesE)
        {
            DiaryEntriesDTOs diaryEntriesDTOs = new()
            {
                id_diary_entries = diaryEntriesE.id_diary_entries,
                fk_tbl_diary = diaryEntriesE.fk_tbl_diary,
                s_content = diaryEntriesE.s_content,
                dt_creation = diaryEntriesE.dt_creation
            };
            return diaryEntriesDTOs;
        }

        public static DiaryEntriesE CreateE(DiaryEntriesDTOs diaryEntriesDTOs)
        {
            DiaryEntriesE diaryEntriesE = new()
            {
                id_diary_entries = diaryEntriesDTOs.id_diary_entries,
                fk_tbl_diary = diaryEntriesDTOs.fk_tbl_diary,
                s_content = diaryEntriesDTOs.s_content,
                dt_creation = diaryEntriesDTOs.dt_creation
            };
            return diaryEntriesE;
        }
    }
}
