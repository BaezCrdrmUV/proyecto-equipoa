using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TypeMeDesktop.ComunicacionAPI.Mensajes;

namespace TypeMeDesktop.Recursos
{
    /// <summary>
    /// Lógica de interacción para ControlMensaje.xaml
    /// </summary>
    public partial class ControlMensaje : UserControl
    {
        public ControlMensaje(Mensaje mensaje)
        {
            InitializeComponent();
            autorDeMensaje.Text = mensaje.Typer.Username;
            contenidoDeMensaje.Text = mensaje.Contenido;
        }

        public ControlMensaje(string mensaje, string usuario)
        {
            InitializeComponent();
            autorDeMensaje.Text = usuario;
            contenidoDeMensaje.Text = mensaje;
        }
    }
}
