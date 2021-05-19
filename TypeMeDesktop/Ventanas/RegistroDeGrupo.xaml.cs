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
    /// Lógica de interacción para RegistroDeGrupo.xaml
    /// </summary>
    public partial class RegistroDeGrupo : Window
    {
        public string NombreDelGrupo { get; set; }
        public string DescripcionDelGrupo { get; set; }

        public RegistroDeGrupo(List<string> listaDeParticipantes)
        {
            InitializeComponent();

            foreach (string participante in listaDeParticipantes)
            {
                TextBlock nombreParticipante = new TextBlock();
                nombreParticipante.Text = participante;
                nombreParticipante.FontSize = 15;
                nombreParticipante.TextAlignment = TextAlignment.Center;
                nombreParticipante.Width = 300;

                participantes.Children.Add(nombreParticipante);
            }
        }

        private void ClickRegistrar(object sender, RoutedEventArgs e)
        {
            if (CamposValidos())
            {
                NombreDelGrupo = campoNombreDeGrupo.Text.Trim();
                DescripcionDelGrupo = campoDescripcion.Text.Trim();
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Completa los campos solicitados", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClickCancelar(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private bool CamposValidos()
        {
            if (!campoNombreDeGrupo.Text.Trim().Equals("") &&
                !campoDescripcion.Text.Trim().Equals(""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
