using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeMeDesktop.ComunicacionAPI.Login;

namespace TypeMeDesktop.ComunicacionAPI.Mensajes
{
    public class Mensaje
    {
        public string IdMensaje { get; set; }
        public string Contenido { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string IdGrupo { get; set; }
        public InformacionTyper Typer { get; set; }
        public string IdMultimedia { get; set; }
    }
}
