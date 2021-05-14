using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeMeDesktop.ComunicacionAPI.Login;

namespace TypeMeDesktop.ComunicacionAPI.Contactos
{
    public class InfoContacto
    {
        public string bloqueado { get; set; }
        public string esFavorito { get; set; }
        public InformacionTyper contacto { get; set; }
    }
}