using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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
        private string urlEnvioDeMensaje = RecursosGlobales.RUTA_API + "/mensajes/enviarMensaje";
        private string urlObtenerMensajes = RecursosGlobales.RUTA_API + "/mensajes/obtenerMensajes/";
        private string urlAgregarIntegrantes = RecursosGlobales.RUTA_API + "/mensajes/agregarIntegrantes/";
        private string urlEnvioDeImagen = RecursosGlobales.RUTA_API + "/mensajes/registrarMultimedia";
        private InformacionTyper typer;
        private int idGrupo;
        private Ventanas.VentanaPrincipal _miVentana;
        private string fileName;

        public ChatGrupal(int idGrupo, InformacionTyper idTyper, Ventanas.VentanaPrincipal principal)
        {
            this.idGrupo = idGrupo;
            this.typer = idTyper;
            this._miVentana = principal;
            fileName = null;

            InitializeComponent();

            APIObtenerMensajesDeGrupo();
        }

        private void ClickEnviar(object sender, RoutedEventArgs e)
        {
            if (fileName == null)
            {
                APIEnviarMensaje(false);
            }
            else if (fileName != null)
            {
                APIEnviarMensaje(true);
            }
        }

        private void ClickMultimedia(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg)|*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
            }
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

        private async Task<string> APIEnviarImagen()
        {
            var cliente = new HttpClient();

            using var form = new MultipartFormDataContent();
            using var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(fileName));
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            form.Add(fileContent, "file", System.IO.Path.GetFileName(fileName));


            var response = await cliente.PostAsync(urlEnvioDeImagen + $"?idTyper={typer.IdTyper}", form);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                RespuestaRegistroImagen result = System.Text.Json.JsonSerializer.Deserialize<RespuestaRegistroImagen>(responseContent);

                if (result.status)
                {
                    return result.result.IdMultimedia;
                }
                else
                {
                    return "";
                }
            }

            return "";
        }

        private async void APIEnviarMensaje(bool tieneImagen)
        {
            string idImagen = "";
            if (tieneImagen)
            {
                idImagen = await APIEnviarImagen();
                if (string.IsNullOrEmpty(idImagen))
                {
                    MessageBox.Show("ocurrio un error en la conexion al enviar la imagen");
                    return;
                }
            }
            else if (tieneImagen == false && string.IsNullOrWhiteSpace(nuevoMensaje.Text))
            {
                return;
            }


            EnvioDeMensaje nuevoMsj = new EnvioDeMensaje()
            {
                contenido = nuevoMensaje.Text.Trim(),
                idGrupo = idGrupo.ToString(),
                idTyper = typer.IdTyper,
                idMultimedia = idImagen
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
                            Typer = infoRegistro.result.Typer,
                            IdMultimedia = infoRegistro.result.IdMultimedia
                        };

                        fileName = null;


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

        private void ClickAgregarMiembro(object sender, RoutedEventArgs e)
        {
            Ventanas.ListaDeContactos ventanaDeAgregacion = new Ventanas.ListaDeContactos(typer.IdTyper);
            
            if (ventanaDeAgregacion.ShowDialog() == true)
            {
                APIAgregarTypersAChat(ventanaDeAgregacion.ListaAgregados);
            }
        }

        private async void APIAgregarTypersAChat(List<InformacionTyper> listaDeNuevosTypers)
        {
            var cliente = new HttpClient();

            string json = JsonConvert.SerializeObject(listaDeNuevosTypers);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var httpresponse = await cliente.PostAsync(String.Concat(urlAgregarIntegrantes, idGrupo), content);

                if (httpresponse.IsSuccessStatusCode)
                {
                    var result = await httpresponse.Content.ReadAsStringAsync();
                    var infoRegistro = JsonConvert.DeserializeObject<RespuestaAgregacionAChat>(result);

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
                MessageBox.Show("ocurrio un error en la conexion al enviar el mensaje");
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("ocurrio un error en la conexion al enviar el mensaje");
            }
        }
    }
}
