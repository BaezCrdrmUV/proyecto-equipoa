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
using System.Windows.Shapes;

namespace TypeMeDesktop.Ventanas
{
    /// <summary>
    /// Lógica de interacción para RegistrarCuenta.xaml
    /// </summary>
    public partial class RegistrarCuenta : Window
    {
        public RegistrarCuenta()
        {
            InitializeComponent();
        }

        private void ClickCrearCuenta(object sender, RoutedEventArgs e)
        {

        }

        private void ClickCancelar(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}
