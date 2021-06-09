using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
        private int idGrupo;
        private Ventanas.VentanaPrincipal _miVentana;

        public ChatGrupal(int idGrupo, InformacionTyper idTyper, Ventanas.VentanaPrincipal principal)
        {
            this.idGrupo = idGrupo;
            this.typer = idTyper;
            this._miVentana = principal;

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
            //Aqui se obtiene el stream
            //se bloquea el cuadro de texto
            //Se llama a la api de imagenes se le puede pasar el stream de bytes
            //Se muestra una preview
        }

        public void InsertarMensaje(Mensaje nuevo)
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
                idGrupo = idGrupo.ToString(),
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
                        nuevoMensaje.Text = "";

                        Mensaje nuevo = new Mensaje()
                        {
                            IdMensaje = infoRegistro.result.IdMensaje,
                            IdGrupo = infoRegistro.result.IdGrupo,
                            Contenido = infoRegistro.result.Contenido,
                            Typer = infoRegistro.result.Typer
                        };


                        await _miVentana.EnviarMensaje(nuevo);
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
