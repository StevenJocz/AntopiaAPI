﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antopia.Domain.DTOs.LoginDTOs
{
    public class AutorizacionResponse
    {
        public string Token { get; set; }
        public bool Resultado { get; set; }
        public string Msg { get; set; }
    }
}
