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

namespace TypeMeDesktop.Paginas
{
    /// <summary>
    /// Lógica de interacción para AgregarContacto.xaml
    /// </summary>
    public partial class AgregarContacto : Page
    {
        public AgregarContacto()
        {
            InitializeComponent();
        }

        private void ClickAgregarContacto(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Nuevo contacto agregado");
        }

        private void ClickCancelar(object sender, RoutedEventArgs e)
        {

        }
    }
}
