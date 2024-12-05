using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesBase.Respuestas
{
    public class Respuesta
    {
        public string? mensaje { get; set; } // ? puede ser nulo

        public bool success { get; set; }

        public object? obj { get; set; } // ? puede ser nulo
    }
}