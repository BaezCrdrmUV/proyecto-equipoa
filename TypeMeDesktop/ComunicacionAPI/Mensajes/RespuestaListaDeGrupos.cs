﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeMeDesktop.ComunicacionAPI.Mensajes
{
    public class RespuestaListaDeGrupos
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<InfoGrupo> result { get; set; }
    }
}