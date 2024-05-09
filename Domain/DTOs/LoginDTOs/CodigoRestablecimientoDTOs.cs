using Antopia.Domain.Entities.LoginE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.LoginDTOs
{
    public class CodigoRestablecimientoDTOs
    {
        public string s_codigo { get; set; }
        public string s_correo { get; set; }

        public static CodigoRestablecimientoDTOs CreateDTO(CodigoRestablecimientoE codigoRestablecimiento)
        {
            CodigoRestablecimientoDTOs CodigoRestablecimientoDTOs = new()
            {
                s_codigo = codigoRestablecimiento.s_codigo,
                s_correo = codigoRestablecimiento.s_correo,
            };
            return CodigoRestablecimientoDTOs;
        }
    }
}
