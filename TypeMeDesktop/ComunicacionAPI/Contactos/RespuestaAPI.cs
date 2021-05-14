using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeMeDesktop.ComunicacionAPI.Contactos
{
    public class RespuestaAPI
    {
        public string status { get; set; }
        public string message { get; set; }
        public InfoContacto result { get; set; }
    }
}
