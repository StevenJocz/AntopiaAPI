using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.UserDTOs
{
    public class PerfilUserDTOs
    {
        public int IdPerfil { get; set; }
        public string NombrePerfil { get; set; }
        public string urlPerfil { get; set; }
        public string ImagenPerfil { get; set; }
        public string ImagenPortada { get; set; }
        public string Frase { get; set; }
        public string Correo { get; set; }
        public int CantidadPublicaciones { get; set; }
        public int Seguidores { get; set; }
        public int TotalSeguiendo { get; set; }
        public int Seguiendo { get; set; }
        public int Level { get; set; }
        public List<PerfilImagenesDTOs> PerfilImagenes { get; set; }
    }


    public class PerfilImagenesDTOs
    {
        public int idPublicacion { get; set; }
        public string url { get; set; }
    }
}
