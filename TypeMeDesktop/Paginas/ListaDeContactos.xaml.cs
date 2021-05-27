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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TypeMeDesktop.ComunicacionAPI.Contactos;
using TypeMeDesktop.ComunicacionAPI.Login;
using TypeMeDesktop.Ventanas;
using TypeMeDesktop.ComunicacionAPI.Mensajes;

namespace TypeMeDesktop.Paginas
{
    /// <summary>
    /// Lógica de interacción para ListaDeContactos.xaml
    /// </summary>
    public partial class ListaDeContactos : Page
    {
        private string urlRegistroGrupo = "http://localhost:4000/mensajes/crearGrupo";
        private string urlListaDeContactos = "http://localhost:4000/typers/obtenerContactos/";
        private string idTyper;
        public List<InformacionTyper> listaDeNuevoGrupo;
        public List<string> listaDeNombresGrupo;
        public List<InformacionTyper> listaDeContactosOriginal;
        public ObservableCollection<InformacionTyper> listaDeContactosObservable;
        private List<InfoContacto> listaDeContactos;

        private VentanaPrincipal miVentana;

        public ListaDeContactos(string idTyper, VentanaPrincipal miVentana)
        {
            InitializeComponent();
            this.idTyper = idTyper;
            this.miVentana = miVentana;
            APIObtenerListaDeContactos();
            
        }

        public void ResetVentana()
        {
            listaDeNuevoGrupo = new List<InformacionTyper>();
            listaDeNuevoGrupo.Add(new InformacionTyper() { IdTyper = idTyper });
            listaDeNombresGrupo = new List<string>();
            listaDeContactosObservable = new ObservableCollection<InformacionTyper>(listaDeContactosOriginal);
            tablaDeContactos.ItemsSource = listaDeContactosObservable;
        }

        public List<InformacionTyper> ObtenerListaDeContactos(List<InfoContacto> listaDeContactos)
        {
            List<InformacionTyper> listaDeInformacion = new List<InformacionTyper>();

            foreach (InfoContacto contactoCompleto in listaDeContactos)
            {
                listaDeInformacion.Add(contactoCompleto.contacto);
            }

            return listaDeInformacion;
        }

        private void ClickAgregarContacto(object sender, RoutedEventArgs e)
        {
            InformacionTyper typerSeleccionado = (InformacionTyper)tablaDeContactos.SelectedItem;
            listaDeNuevoGrupo.Add(typerSeleccionado);
            listaDeNombresGrupo.Add(typerSeleccionado.Username);
            listaDeContactosObservable.Remove(typerSeleccionado);
        }

        private void ClickCrearGrupo(object sender, RoutedEventArgs e)
        {
            if (listaDeNuevoGrupo.Count > 1)
            {
                RegistroDeGrupo ventanaRegistro = new RegistroDeGrupo(listaDeNombresGrupo);
                ventanaRegistro.ShowDialog();
                if (ventanaRegistro.DialogResult == true)
                {
                    RegistroNuevoGrupo nuevoGrupo = new RegistroNuevoGrupo()
                    {
                        nombre = ventanaRegistro.NombreDelGrupo,
                        descripcion = ventanaRegistro.DescripcionDelGrupo,
                        perteneces = listaDeNuevoGrupo
                    };

                    APICrearGrupo(nuevoGrupo);
                }

                ResetVentana();
            }
            else
            {
                MessageBox.Show("Debes seleccionar minimo un integrante para crear un grupo");
            }
        }

        private async void APICrearGrupo(RegistroNuevoGrupo nuevoGrupo)
        {
            var cliente = new HttpClient();

            string json = JsonConvert.SerializeObject(nuevoGrupo);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var httpresponse = await cliente.PostAsync(urlRegistroGrupo, content);

                if (httpresponse.IsSuccessStatusCode)
                {
                    var result = await httpresponse.Content.ReadAsStringAsync();
                    var infoRegistro = JsonConvert.DeserializeObject<RespuestaCreacionGrupo>(result);

                    if (bool.Parse(infoRegistro.status))
                    {
                        miVentana.APIObtenerListaDeGrupos();
                    }
                    else
                    {
                        MessageBox.Show(infoRegistro.message);
                    }
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("ocurrio un error en la conexion");
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("ocurrio un error en la conexion");
            }
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
                            listaDeContactos = infoContactos.result;
                            listaDeContactosOriginal = ObtenerListaDeContactos(listaDeContactos);
                            ResetVentana();   
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
    }
}
