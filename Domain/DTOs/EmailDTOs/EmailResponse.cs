using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.EmailDTOs
{
    public class EmailResponse
    {
        public bool resultado { get; set; }
        public string message { get; set; }
        public string? codigo { get; set; }
    }
}
