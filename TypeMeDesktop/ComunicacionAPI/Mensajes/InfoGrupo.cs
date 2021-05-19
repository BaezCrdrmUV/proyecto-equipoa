using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeMeDesktop.ComunicacionAPI.Login;

namespace TypeMeDesktop.ComunicacionAPI.Mensajes
{
    public class InfoGrupo
    {
        public string IdGrupo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string FechaCreacion { get; set; }
        public string MyProperty { get; set; }
        public List<Mensaje> Mensajes { get; set; }
        public List<InformacionTyper> Perteneces { get; set; }
    }
}
