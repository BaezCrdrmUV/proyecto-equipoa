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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TypeMeDesktop.ComunicacionAPI.Login;
using TypeMeDesktop.ComunicacionAPI.Mensajes;
using TypeMeDesktop.Recursos;

namespace TypeMeDesktop.Paginas
{
    /// <summary>
    /// Lógica de interacción para ChatGrupal.xaml
    /// </summary>
    public partial class ChatGrupal : Page
    {
        private string urlEnvioDeMensaje = "http://localhost:4000/mensajes/enviarMensaje";
        private string urlObtenerMensajes = "http://localhost:4000/mensajes/obtenerMensajes/";
        private InformacionTyper typer;
        private string idGrupo;

        public ChatGrupal(string idGrupo, InformacionTyper idTyper)
        {
            this.idGrupo = idGrupo;
            this.typer = idTyper;

            InitializeComponent();

            APIObtenerMensajesDeGrupo();
        }

        private void ClickEnviar(object sender, RoutedEventArgs e)
        {
            if (!nuevoMensaje.Text.Trim().Equals(""))
            {
                APIEnviarMensaje();
            }
        }

        private void ClickMultimedia(object sender, RoutedEventArgs e)
        {

        }

        private void InsertarMensaje(Mensaje nuevo)
        {
            ControlMensaje nuevoControl = new ControlMensaje(nuevo);
            nuevoControl.Margin = new Thickness(15);

            if (nuevo.Typer.IdTyper.Equals(typer.IdTyper))
            {
                nuevoControl.HorizontalAlignment = HorizontalAlignment.Right;
            }
            else
            {
                nuevoControl.HorizontalAlignment = HorizontalAlignment.Left;
            }

            listaDeMensajes.Children.Add(nuevoControl);
        }

        private async void APIEnviarMensaje()
        {
            EnvioDeMensaje nuevoMsj = new EnvioDeMensaje()
            {
                contenido = nuevoMensaje.Text.Trim(),
                idGrupo = idGrupo,
                idTyper = typer.IdTyper
            };

            var cliente = new HttpClient();

            string json = JsonConvert.SerializeObject(nuevoMsj);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var httpresponse = await cliente.PostAsync(urlEnvioDeMensaje, content);

                if (httpresponse.IsSuccessStatusCode)
                {
                    var result = await httpresponse.Content.ReadAsStringAsync();
                    var infoRegistro = JsonConvert.DeserializeObject<RespuestaMensajeEnviado>(result);

                    if (bool.Parse(infoRegistro.status))
                    {
                        ControlMensaje nuevoControl = new ControlMensaje(nuevoMensaje.Text.Trim(), typer.Username);
                        nuevoControl.HorizontalAlignment = HorizontalAlignment.Right;
                        nuevoControl.Margin = new Thickness(15);

                        listaDeMensajes.Children.Add(nuevoControl);
                        nuevoMensaje.Text = "";
                    }
                    else
                    {
                        MessageBox.Show(infoRegistro.message);
                    }
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("ocurrio un error en la conexion al enviar el mensaje");
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("ocurrio un error en la conexion al enviar el mensaje");
            }
        }

        private async void APIObtenerMensajesDeGrupo()
        {
            var cliente = new HttpClient();

            try
            {
                var httpresponse = await cliente.GetAsync(urlObtenerMensajes + idGrupo);

                if (httpresponse.IsSuccessStatusCode)
                {
                    var result = await httpresponse.Content.ReadAsStringAsync();
                    var resultadoMensajes = JsonConvert.DeserializeObject<RespuestaListaDeMensajes>(result);

                    if (bool.Parse(resultadoMensajes.status))
                    {
                        foreach (Mensaje mensaje in resultadoMensajes.result)
                        {
                            InsertarMensaje(mensaje);
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("ocurrio un error en la conexion al enviar el mensaje");
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("ocurrio un error en la conexion al enviar el mensaje");
            }
        }
    }
}
