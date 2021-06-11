using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeMeDesktop.ComunicacionAPI.Login
{
    public class RespuestaDeActualizacion
    {
        public bool status { get; set; }
        public string message { get; set; }
        public InformacionTyper result { get; set; }
    }
}
