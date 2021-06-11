using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeMeDesktop.ComunicacionAPI.Mensajes
{
    public class RespuestaAgregacionAChat
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<Pertenece> result { get; set; }
    }
}
