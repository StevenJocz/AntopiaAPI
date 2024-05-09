using Antopia.Domain.Entities.NotificacionE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.NotificacionDTOs
{
    public class TypeNotificacionDTOs
    {
        public int id_notification_type { get; set; }
        public string s_name { get; set; }
        public string s_content { get; set; }

        public static TypeNotificacionDTOs CreateDTO(TypeNotificacionE typeNotificacionE)
        {
            TypeNotificacionDTOs typeNotificacionDTOs = new()
            {
                id_notification_type = typeNotificacionE.id_notification_type,
                s_name = typeNotificacionE.s_name,
                s_content = typeNotificacionE.s_content,
            };
            return typeNotificacionDTOs;
        }

        public static TypeNotificacionE CreateE(TypeNotificacionDTOs typeNotificacionDTOs)
        {
            TypeNotificacionE typeNotificacionE = new()
            {
                id_notification_type = typeNotificacionDTOs.id_notification_type,
                s_name = typeNotificacionDTOs.s_name,
                s_content = typeNotificacionDTOs.s_content,
            };
            return typeNotificacionE;
        }
    }
}
