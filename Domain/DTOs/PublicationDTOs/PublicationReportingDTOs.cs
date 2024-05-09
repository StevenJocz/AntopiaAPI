using Antopia.Domain.Entities.PublicacionE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.PublicationDTOs
{
    public class PublicationReportingDTOs
    {
        public int id_publication_reporting { get; set; }
        public int fk_tbl_publication_reporting_reason { get; set; }
        public int fk_tbl_publication { get; set; }

        public static PublicationReportingDTOs CreateDTO(PublicationReportingE publicationReportingE)
        {
            PublicationReportingDTOs publicationReportingDTOs = new()
            {
                id_publication_reporting = publicationReportingE.id_publication_reporting,
                fk_tbl_publication_reporting_reason = publicationReportingE.fk_tbl_publication_reporting_reason,
                fk_tbl_publication = publicationReportingE.fk_tbl_publication,
            };
            return publicationReportingDTOs;
        }

        public static PublicationReportingE CreateE(PublicationReportingDTOs publicationReportingDTOs)
        {
            PublicationReportingE publicationReportingE = new()
            {
                id_publication_reporting = publicationReportingDTOs.id_publication_reporting,
                fk_tbl_publication_reporting_reason = publicationReportingDTOs.fk_tbl_publication_reporting_reason,
                fk_tbl_publication = publicationReportingDTOs.fk_tbl_publication,
            };
            return publicationReportingE;
        }
    }

    public class Publication_reporting_reasonDTOs
    {
        public int id_publication_reporting_reason { get; set; }
        public string s_title { get; set; }
        public string s_description { get; set; }
        public int byte_activo { get; set; }

        public static Publication_reporting_reasonDTOs CreateDTO(Publication_reporting_reasonE publication_reporting_reasonE)
        {
            Publication_reporting_reasonDTOs publication_reporting_reasonDTOs = new()
            {
                id_publication_reporting_reason = publication_reporting_reasonE.id_publication_reporting_reason,
                s_title = publication_reporting_reasonE.s_title,
                s_description = publication_reporting_reasonE.s_description,
                byte_activo = publication_reporting_reasonE.byte_activo,
            };
            return publication_reporting_reasonDTOs;
        }

        public static Publication_reporting_reasonE CreateE(Publication_reporting_reasonDTOs publication_reporting_reasonDTOs)
        {
            Publication_reporting_reasonE publication_reporting_reasonE = new()
            {
                id_publication_reporting_reason = publication_reporting_reasonDTOs.id_publication_reporting_reason,
                s_title = publication_reporting_reasonDTOs.s_title,
                s_description = publication_reporting_reasonDTOs.s_description,
                byte_activo = publication_reporting_reasonDTOs.byte_activo,
            };
            return publication_reporting_reasonE;
        }
    }
}
