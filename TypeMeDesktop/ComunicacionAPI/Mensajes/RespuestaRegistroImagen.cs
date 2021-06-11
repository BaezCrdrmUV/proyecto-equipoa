using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeMeDesktop.ComunicacionAPI.Mensajes
{
    public class RespuestaRegistroImagen
    {
        public bool status { get; set; }
        public string message { get; set; }
        public InfoImagen result { get; set; }
    }
}
