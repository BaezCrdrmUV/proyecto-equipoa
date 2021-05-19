using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeMeDesktop.ComunicacionAPI.Contactos
{
    class RespuestaAgregacionAPI
    {
        public string status { get; set; }
        public string message { get; set; }
        public InfoContacto result { get; set; }
    }
}
