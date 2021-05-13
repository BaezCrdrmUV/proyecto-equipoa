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
        public List<ContraseniaTyper> Contrasenia { get; set; }

        public string ObtenerCorreoPrincipal()
        {
            CorreoTyper correoPrincipal = this.Correos.ElementAt(0);
            return correoPrincipal.Direccion;
        }

        public string ObtenerCorreoSecundario()
        {
            CorreoTyper correoPrincipal = this.Correos.ElementAt(1);
            return correoPrincipal.Direccion;
        }
    }
}
