using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Antopia.Domain.Entities.MensajeE
{
    [Table("tbl_mensajes")]
    public class MensajeE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UsuarioId { get; set; } // El ID del usuario que envió el mensaje

        [MaxLength(500)]
        public string s_contenido { get; set; }

        public DateTime dt_fechaEnvio { get; set; }

        [ForeignKey("UsuarioId")]
        public Antopia.Domain.Entities.UserE.UserE Usuario { get; set; }  // Relación con la entidad UsuarioE

    }
}
