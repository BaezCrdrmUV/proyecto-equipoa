using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

namespace TypeMeDesktop.Paginas
{
    /// <summary>
    /// Lógica de interacción para AgregarContacto.xaml
    /// </summary>
    public partial class AgregarContacto : Page
    {
        private string idTyper;
        private string urlRegistroContacto = "http://localhost:4000/typers/agregarContacto";
        public AgregarContacto(string idTyper)
        {
            InitializeComponent();
            this.idTyper = idTyper;
        }

        private void ClickAgregarContacto(object sender, RoutedEventArgs e)
        {
            if (CamposComppletos())
            {
                DesactivarBoton();
                APILlamadaAgregar consulta = CrearConsulta();
                APIAgregarContacto(consulta);
            }
            else
            {
                MessageBox.Show("El campo esta vacio");
            }
        }

        public bool CamposComppletos()
        {
            return !campoUsuario.Text.Trim().Equals("");
        }

        public APILlamadaAgregar CrearConsulta()
        {
            APILlamadaAgregar llamadaAPI = new APILlamadaAgregar()
            {
                idTyper = idTyper,
                contacto = campoUsuario.Text.Trim()
            };
            return llamadaAPI;
        }

        public void DesactivarBoton()
        {
            botonAgregar.IsEnabled = false;
        }

        public void ActivarBoton()
        {
            botonAgregar.IsEnabled = true;
        }

        private async void APIAgregarContacto(APILlamadaAgregar consulta)
        {
            var cliente = new HttpClient();

            string json = JsonConvert.SerializeObject(consulta);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var httpresponse = await cliente.PostAsync(urlRegistroContacto, content);

                if (httpresponse.IsSuccessStatusCode)
                {
                    var result = await httpresponse.Content.ReadAsStringAsync();
                    var infoRegistro = JsonConvert.DeserializeObject<RespuestaAgregacionAPI>(result);

                    if (bool.Parse(infoRegistro.status))
                    {
                        MessageBox.Show(infoRegistro.message);
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
            finally
            {
                ActivarBoton();
            }
        }
    }
}
