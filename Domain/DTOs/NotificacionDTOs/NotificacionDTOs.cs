using Antopia.Domain.Entities.NotificacionE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.NotificacionDTOs
{
    public class NotificacionDTOs
    {
        public int id_notification { get; set; }
        public int type_notification { get; set; }
        public int for_user { get; set; }
        public int of_user { get; set; }
        public DateTime data_created { get; set; }
        public bool state { get; set; }
        public int fk_tbl_publication { get; set; }
        public int fk_tbl_diary { get; set; }
        public int fk_tbl_colonie { get; set; }

        public static NotificacionDTOs CreateDTO(NotificacionE notificacionE)
        {
            NotificacionDTOs notificacionDTOs = new()
            {
                id_notification = notificacionE.id_notification,
                type_notification = notificacionE.type_notification,
                for_user = notificacionE.for_user,
                of_user = notificacionE.of_user,
                data_created = notificacionE.data_created,
                state = notificacionE.state,
                fk_tbl_publication = notificacionE.fk_tbl_publication,
                fk_tbl_diary = notificacionE.fk_tbl_diary,
                fk_tbl_colonie = notificacionE.fk_tbl_colonie,
            };
            return notificacionDTOs;
        }

        public static NotificacionE CreateE(NotificacionDTOs notificacionDTOs)
        {
            NotificacionE notificacionE = new()
            {
                id_notification = notificacionDTOs.id_notification,
                type_notification = notificacionDTOs.type_notification,
                for_user = notificacionDTOs.for_user,
                of_user = notificacionDTOs.of_user,
                data_created = notificacionDTOs.data_created,
                state = notificacionDTOs.state,
                fk_tbl_publication = notificacionDTOs.fk_tbl_publication,
                fk_tbl_diary = notificacionDTOs.fk_tbl_diary,
                fk_tbl_colonie = notificacionDTOs.fk_tbl_colonie,
            };
            return notificacionE;
        }

    }

    public class NotificacionesUser
    {
        public int idNotification { get; set; }
        public int typeNotification { get; set; }
        public int idPublicacion { get; set; }
        public string tituloPublicacion { get; set; }
        public int idUser { get; set; }
        public string fotoUser { get; set; }
        public string NombreUser { get; set; }
        public string urlPerfil { get; set; }
        public string contenido { get; set; }
        public int idColonia { get; set; }
        public string nombreColonia { get; set; }
        public bool state { get; set; }
        public DateTime fechaCreacion { get; set; }
    }

    public class idNotification
    {
        public int id_notification { get; set; }
    }

    public class IdUser
    {
        public int id_user { get; set; }
    }


}
