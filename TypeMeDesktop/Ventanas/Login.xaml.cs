using System.Windows;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TypeMeDesktop.ComunicacionAPI.Login;
using System;

namespace TypeMeDesktop.Ventanas
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private string urlLogin = "http://localhost:4000/typers/loginTyper";
        
        public Login()
        {
            InitializeComponent();
        }

        private void ClickLogin(object sender, RoutedEventArgs e)
        {
            if (CamposCorrectos())
            {
                DesactivarBotones();
                ConsultaJson infoLoginJson = CrearConsulta();
                APILogin(infoLoginJson);
            }
            else
            {
                MessageBox.Show("Los campos estan incompletos");
            }
        }

        private void DesactivarBotones()
        {
            botonLogin.IsEnabled = false;
            botonRegistro.IsEnabled = false;
        }

        private void ClickCrearCuenta(object sender, RoutedEventArgs e)
        {
            RegistrarCuenta registro = new RegistrarCuenta();
            registro.Show();
            this.Close();
        }

        private bool CamposCorrectos()
        {
            if (!usuario.Text.Trim().Equals("") &&
                !contrasenia.Password.Trim().Equals(""))
            {
                return true;
            }

            return false;
        }

        private ConsultaJson CrearConsulta()
        {
            ConsultaJson consulta = new ConsultaJson()
            {
                IdentificadorTyper = usuario.Text.Trim(),
                InformacionComplementaria = contrasenia.Password.Trim()
            };

            return consulta;
        }

        private async void APILogin(ConsultaJson consulta)
        {
            var cliente = new HttpClient();

            string json = JsonConvert.SerializeObject(consulta);
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            try
            {
                var httpResponse = await cliente.PostAsync(urlLogin, content);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var result = await httpResponse.Content.ReadAsStringAsync();
                    var infoLogin = JsonConvert.DeserializeObject<RespuestaDeAPILogin>(result);

                    if (bool.Parse(infoLogin.status))
                    {
                        VentanaPrincipal inicio = new VentanaPrincipal(infoLogin.result);
                        inicio.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show(infoLogin.message);
                    }
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Ocurrio un error en la conexion");
            }catch (HttpRequestException)
            {
                MessageBox.Show("Ocurrio un error en la conexion");
            }
            finally
            {
                botonLogin.IsEnabled = true;
                botonRegistro.IsEnabled = true;
            }
        }
    }
}
