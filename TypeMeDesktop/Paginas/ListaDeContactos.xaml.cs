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
using TypeMeDesktop.ComunicacionAPI.Contactos;

namespace TypeMeDesktop.Paginas
{
    /// <summary>
    /// Lógica de interacción para ListaDeContactos.xaml
    /// </summary>
    public partial class ListaDeContactos : Page
    {
        private string idTyper;
        private List<InfoContacto> listaDeContactosParaGrupo;

        public ListaDeContactos(string idTyper)
        {
            InitializeComponent();
            this.idTyper = idTyper;
        }

        private void ClickAgregarContacto(object sender, RoutedEventArgs e)
        {

        }

        private void ClickCrearGrupo(object sender, RoutedEventArgs e)
        {

        }
    }
}
