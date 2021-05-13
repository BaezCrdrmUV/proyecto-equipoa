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
using System.Net.Http;
using Newtonsoft.Json;
using TypeMeDesktop.ComunicacionAPI.Login;

namespace TypeMeDesktop.Paginas
{
    /// <summary>
    /// Lógica de interacción para MiPerfil.xaml
    /// </summary>
    public partial class MiPerfil : Page
    {
        private InformacionTyper perfilTyper;
        private string correoPrincipalOriginal;
        private string correoSecundarioOriginal;
        private string urlActualizar = "http://localhost:4000/typers/actualizarInfoTyper";
        private string urlActualizarCorreo = "http://localhost:4000/typers/actualizarCorreo";

        public MiPerfil(InformacionTyper infoMiPerfil)
        {
            InitializeComponent();
            campoUsuario.Text = infoMiPerfil.Username;
            CampoCorreoPrincipal.Text = infoMiPerfil.ObtenerCorreoPrincipal();
            correoPrincipalOriginal = infoMiPerfil.ObtenerCorreoPrincipal();
            campoCorreoSecundario.Text = infoMiPerfil.ObtenerCorreoSecundario();
            correoSecundarioOriginal = infoMiPerfil.ObtenerCorreoSecundario();
            campoEstado.Text = infoMiPerfil.Estado;
            perfilTyper = infoMiPerfil;
        }

        private void ActualizarFotoDePerfil(object sender, RoutedEventArgs e)
        {
            
        }

        private void ActualizarUsuario(object sender, RoutedEventArgs e)
        {
            if (!campoUsuario.Text.Trim().Equals(""))
            {
                DesactivarBotones();
                ConsultaJson consulta = new ConsultaJson()
                {
                    IdentificadorTyper = perfilTyper.IdTyper,
                    InformacionActualizada = campoUsuario.Text.Trim(),
                    ModificadorDeMetodo = "usuario"
                };

                ActualizarInfo(consulta, urlActualizar);
            }
            else
            {
                MessageBox.Show("Coloque informacion en el campo");
            }
        }

        private void ActualizarCorreoPrincipal(object sender, RoutedEventArgs e)
        {
            if (!CampoCorreoPrincipal.Text.Trim().Equals(""))
            {
                DesactivarBotones();
                ConsultaJson consulta = new ConsultaJson()
                {
                    IdentificadorTyper = perfilTyper.IdTyper,
                    InformacionComplementaria = correoPrincipalOriginal,
                    InformacionActualizada = CampoCorreoPrincipal.Text.Trim()
                };

                ActualizarInfo(consulta, urlActualizarCorreo);
            }
            else
            {
                MessageBox.Show("Coloque informacion en el campo");
            }
        }

        private void ActualizarCorreoSecundario(object sender, RoutedEventArgs e)
        {
            if (!campoCorreoSecundario.Text.Trim().Equals(""))
            {
                DesactivarBotones();
                ConsultaJson consulta = new ConsultaJson()
                {
                    IdentificadorTyper = perfilTyper.IdTyper,
                    InformacionComplementaria = correoSecundarioOriginal,
                    InformacionActualizada = campoCorreoSecundario.Text.Trim()
                };

                ActualizarInfo(consulta, urlActualizarCorreo);
            }
            else
            {
                MessageBox.Show("Coloque informacion en el campo");
            }
        }

        private void ActualizarEstado(object sender, RoutedEventArgs e)
        {
            if (!campoEstado.Text.Trim().Equals(""))
            {
                DesactivarBotones();
                ConsultaJson consulta = new ConsultaJson()
                {
                    IdentificadorTyper = perfilTyper.IdTyper,
                    InformacionActualizada = campoEstado.Text.Trim(),
                    ModificadorDeMetodo = "estado"
                };

                ActualizarInfo(consulta, urlActualizar);
            }
            else
            {
                MessageBox.Show("Coloque informacion en el campo");
            }
        }

        private async void ActualizarInfo(ConsultaJson consulta, string url)
        {
            var cliente = new HttpClient();

            string json = JsonConvert.SerializeObject(consulta);
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            try
            {
                var httpResponse = await cliente.PutAsync(url, content);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var result = await httpResponse.Content.ReadAsStringAsync();
                    var respuestaAPI = JsonConvert.DeserializeObject<RespuestaDeAPILogin>(result);

                    if (bool.Parse(respuestaAPI.status))
                    {
                        MessageBox.Show(respuestaAPI.message);
                    }
                    else
                    {
                        MessageBox.Show(respuestaAPI.message);
                    }
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Ocurrio un error en la conexion");
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Ocurrio un error en la conexion");
            }
            finally
            {
                ActivarBotones();
            }
        }

        public void DesactivarBotones()
        {
            botonImagen.IsEnabled = false;
            botonUsuario.IsEnabled = false;
            botonCorreoPrincipal.IsEnabled = false;
            botonCorreoSecundario.IsEnabled = false;
            botonEstado.IsEnabled = false;
        }
        public void ActivarBotones()
        {
            botonImagen.IsEnabled = true;
            botonUsuario.IsEnabled = true;
            botonCorreoPrincipal.IsEnabled = true;
            botonCorreoSecundario.IsEnabled = true;
            botonEstado.IsEnabled = true;
        }
    }
}
