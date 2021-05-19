using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeMeDesktop.ComunicacionAPI.Login;

namespace TypeMeDesktop.ComunicacionAPI.Mensajes
{
    public class RegistroNuevoGrupo
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public List<InformacionTyper> perteneces { get; set; }
    }
}
