using Newtonsoft.Json;
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
using System.Windows.Shapes;
using TypeMeDesktop.ComunicacionAPI.Login;

namespace TypeMeDesktop.Ventanas
{
    /// <summary>
    /// Lógica de interacción para RegistrarCuenta.xaml
    /// </summary>
    public partial class RegistrarCuenta : Window
    {
        private string urlregistro = "http://localhost:4000/typers/registrarTyper";
        public RegistrarCuenta()
        {
            InitializeComponent();
        }

        private void ClickCrearCuenta(object sender, RoutedEventArgs e)
        {
            if (CamposCompletos() && ContraseniasCorrectas())
            {
                Desactivarbotones();
                InformacionTyper nuevoTyper = CrearUsuario();
                APIRegistro(nuevoTyper);
            }
        }

        private void ClickCancelar(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        public bool CamposCompletos()
        {
            bool validos = false;

            if (!campoUsuario.Text.Trim().Equals("") && 
                !campoContrasenia.Password.Trim().Equals("") &&
                !campoContraseni2.Password.Trim().Equals("") && 
                !campoCorreoPrincipal.Text.Trim().Equals("") && 
                !campoCorreoSecundario.Text.Trim().Equals(""))
            {
                validos = true;
            }
            else
            {
                MessageBox.Show("Los campos no estan completos");
            }

            return validos;
        }

        public bool ContraseniasCorrectas()
        {
            bool sonIguales = false;

            if (campoContrasenia.Password.Trim().Equals(campoContraseni2.Password.Trim()))
            {
                sonIguales = true;
            }
            else
            {
                MessageBox.Show("Las contrasenias no son iguales");
            }

            return sonIguales;
        }

        public InformacionTyper CrearUsuario()
        {
            List<CorreoTyper> correosTyper = new List<CorreoTyper>();
            List<ContraseniaTyper> contraseniaTyper = new List<ContraseniaTyper>();
            correosTyper.Add(new CorreoTyper()
            {
                Direccion = campoCorreoPrincipal.Text,
                EsPrincipal = 1
            });

            correosTyper.Add(new CorreoTyper()
            {
                Direccion = campoCorreoSecundario.Text,
                EsPrincipal = 0
            });

            contraseniaTyper.Add(new ContraseniaTyper()
            {
                contrasenia1 = campoContrasenia.Password
            });

            InformacionTyper nuevoTyper = new InformacionTyper()
            {
                Username = campoUsuario.Text,
                Estatus = 1,
                Contrasenia = contraseniaTyper,
                Correos = correosTyper
            };

            return nuevoTyper;
        }

        private async void APIRegistro(InformacionTyper nuevoTyper)
        {
            var cliente = new HttpClient();

            string json = JsonConvert.SerializeObject(nuevoTyper);
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            try
            {
                var httpResponse = await cliente.PostAsync(urlregistro, content);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var result = await httpResponse.Content.ReadAsStringAsync();
                    var infoRegistro = JsonConvert.DeserializeObject<RespuestaDeAPILogin>(result);

                    if (bool.Parse(infoRegistro.status))
                    {
                        MessageBox.Show(infoRegistro.message);
                        Login login = new Login();
                        login.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show(infoRegistro.message);
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
                Activarbotones();
            }
        }

        private void Desactivarbotones()
        {
            botonCrearCuenta.IsEnabled = false;
            botonCancelar.IsEnabled = false;
        }

        private void Activarbotones()
        {
            botonCrearCuenta.IsEnabled = true;
            botonCancelar.IsEnabled = true;
        }
    }
}
