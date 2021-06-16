using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
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
using TypeMeDesktop.ComunicacionAPI.Contactos;
using TypeMeDesktop.ComunicacionAPI.Login;

namespace TypeMeDesktop.Ventanas
{
    /// <summary>
    /// Lógica de interacción para ListaDeContactos.xaml
    /// </summary>
    public partial class ListaDeContactos : Window
    {
        private string urlListaDeContactos = Recursos.RecursosGlobales.RUTA_API + "/typers/obtenerContactos/";
        public ObservableCollection<InformacionTyper> listaDeContacto;
        private string idTyper;
        public List<InformacionTyper> ListaAgregados { get; set; }

        public ListaDeContactos(string idTyper)
        {
            InitializeComponent();
            this.idTyper = idTyper;
            ListaAgregados = new List<InformacionTyper>();
            APIObtenerListaDeContactos();
        }
        public ObservableCollection<InformacionTyper> ObtenerListaDeContactos(List<InfoContacto> listaDeContactos)
        {
            ObservableCollection<InformacionTyper> listaDeInformacion = new ObservableCollection<InformacionTyper>();

            foreach (InfoContacto contactoCompleto in listaDeContactos)
            {
                listaDeInformacion.Add(contactoCompleto.contacto);
            }

            return listaDeInformacion;
        }

        private async void APIObtenerListaDeContactos()
        {
            var cliente = new HttpClient();

            try
            {
                var httpresponse = await cliente.GetAsync(String.Concat(urlListaDeContactos, idTyper));

                if (httpresponse.IsSuccessStatusCode)
                {
                    var result = await httpresponse.Content.ReadAsStringAsync();
                    var infoContactos = JsonConvert.DeserializeObject<RespuestaAPI>(result);

                    if (bool.Parse(infoContactos.status))
                    { 
                        listaDeContacto = ObtenerListaDeContactos(infoContactos.result);
                        tablaDeContactos.ItemsSource = listaDeContacto;
                    }
                    else
                    {
                        MessageBox.Show("No cuentas con contactos en este momento");
                    }
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("ocurrio un error en la conexion al obtener los contactos");
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("ocurrio un error en la conexion al obtener los contactos");
            }
        }

        private void ClickAgregarContacto(object sender, RoutedEventArgs e)
        {
            InformacionTyper typerSeleccionado = (InformacionTyper)tablaDeContactos.SelectedItem;
            ListaAgregados.Add(typerSeleccionado);
            listaDeContacto.Remove(typerSeleccionado);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
