using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeMeDesktop.ComunicacionAPI.Login
{
    public class InformacionTyper : RespuestaDeAPILogin
    {
        public string IdTyper { get; set; }
        public string Username { get; set; }
        public string Estado { get; set; }
        public string FotoDePerfil { get; set; }
        public int Estatus { get; set; }
        public List<CorreoTyper> Correos { get; set; }
    }
}
