using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TypeMeDesktop.ComunicacionAPI.Login;
using TypeMeDesktop.ComunicacionAPI.Mensajes;
using TypeMeDesktop.Paginas;

namespace TypeMeDesktop.Ventanas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class VentanaPrincipal : Window
    {
        private InformacionTyper perfilTyper;
        private string urlListaDeGrupos = Recursos.RecursosGlobales.RUTA_API + "/mensajes/misGrupos/";
        private string urlSignalR = Recursos.RecursosGlobales.RUTA_SIGNALR_SERVER + "/chatHub";
        private HubConnection _conexion;
        private Dictionary<int, Ellipse> notificaciones;
        private int idGrupoAbiertoActual = 0;

        public VentanaPrincipal(InformacionTyper typer)
        {
            InitializeComponent();

            notificaciones = new Dictionary<int, Ellipse>();
            this.perfilTyper = typer;
            this.infoHeader.Text = perfilTyper.Username;
            PaginaFrame.Navigate(new Bienvenida());

            APIObtenerListaDeGrupos();
            ConectarAHub();

        }

        public async void ConectarAHub()
        {
            _conexion = new HubConnectionBuilder().WithUrl(urlSignalR).Build();
            _conexion.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _conexion.StartAsync();
            };

            _conexion.On<Mensaje>("RecibirMensaje", (mensajeRecibido) =>
            {
                this.Dispatcher.Invoke(() => {
                    if (mensajeRecibido.IdGrupo == idGrupoAbiertoActual)
                    {
                        ChatGrupal chatActual = (ChatGrupal)PaginaFrame.Content;
                        chatActual.InsertarMensaje(mensajeRecibido);
                    }
                    else
                    {
                        if (notificaciones.ContainsKey(mensajeRecibido.IdGrupo))
                        {
                            Ellipse notificacionActiva;
                            notificaciones.TryGetValue(mensajeRecibido.IdGrupo, out notificacionActiva);
                            notificacionActiva.Visibility = Visibility.Visible;
                        }
                    }
                });
            });

            try
            {
                await _conexion.StartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task EnviarMensaje(Mensaje nuevoMensaje)
        {
            try
            {
                await _conexion.InvokeAsync("EnviarMensaje", nuevoMensaje);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClickNuevoChat(object sender, RoutedEventArgs e)
        {
            this.infoHeader.Text = "Crear nuevo chat";
            this.desciptionHeader.Text = "";
            idGrupoAbiertoActual = 0;
            PaginaFrame.Navigate(new TypeMeDesktop.Paginas.ListaDeContactos(perfilTyper.IdTyper, this));
        }

        private void ClickNuevoContacto(object sender, RoutedEventArgs e)
        {
            this.infoHeader.Text = "Nuevo contacto";
            this.desciptionHeader.Text = "";
            idGrupoAbiertoActual = 0;
            PaginaFrame.Navigate(new AgregarContacto(perfilTyper.IdTyper));
        }

        private void ClickMiCuenta(object sender, RoutedEventArgs e)
        {
            this.infoHeader.Text = perfilTyper.Username;
            this.desciptionHeader.Text = "";
            idGrupoAbiertoActual = 0;
            PaginaFrame.Navigate(new MiPerfil(perfilTyper));
        }

        private void CrearPreviewDeChat(InfoGrupo grupo)
        {

            Style style = this.FindResource("botonChat") as Style;
            Button nuevaPreviewChat = new Button();
            nuevaPreviewChat.Style = style;

            StackPanel preview = new StackPanel();
            preview.Orientation = Orientation.Horizontal;
            preview.Width = 230;
            preview.Margin = new Thickness(0, 20, 0, 0);

            Ellipse imagenPerfil = new Ellipse();
            imagenPerfil.Height = 60;
            imagenPerfil.Width = 60;
            ImageBrush imagen = new ImageBrush();


            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(@"..\..\..\Recursos\chat.png", UriKind.Relative);
            bitmap.EndInit();

            imagen.ImageSource = bitmap;
            imagenPerfil.Fill = imagen;

            TextBlock nombreGrupo = new TextBlock();
            nombreGrupo.Text = grupo.Nombre; 
            nombreGrupo.VerticalAlignment = VerticalAlignment.Center;
            nombreGrupo.Margin = new Thickness(15, 0, 0, 0);
            nombreGrupo.TextWrapping = TextWrapping.Wrap;
            nombreGrupo.Width = 130;

            Ellipse notificacion = new Ellipse();
            notificacion.Height = 10;
            notificacion.Width = 10;
            notificacion.Margin = new Thickness(10, 0, 0, 0);
            notificacion.Fill = new SolidColorBrush(Colors.Red);
            notificacion.Visibility = Visibility.Hidden;
            notificaciones.Add(grupo.IdGrupo, notificacion);


            preview.Children.Add(imagenPerfil);
            preview.Children.Add(nombreGrupo);
            preview.Children.Add(notificacion);
            nuevaPreviewChat.Content = preview;

            nuevaPreviewChat.Click += (s, ev) =>
            {
                this.infoHeader.Text = grupo.Nombre;
                this.desciptionHeader.Text = grupo.Descripcion;
                Ellipse notificacionActiva;
                notificaciones.TryGetValue(grupo.IdGrupo, out notificacionActiva);
                notificacionActiva.Visibility = Visibility.Hidden;
                idGrupoAbiertoActual = grupo.IdGrupo;

                PaginaFrame.Navigate(new ChatGrupal(grupo.IdGrupo, perfilTyper, this)); 
            };

            listaDeChats.Children.Add(nuevaPreviewChat);
        }

        public async void APIObtenerListaDeGrupos()
        {
            var cliente = new HttpClient();

            try
            {
                var httpresponse = await cliente.GetAsync(String.Concat(urlListaDeGrupos, perfilTyper.IdTyper));

                if (httpresponse.IsSuccessStatusCode)
                {
                    var result = await httpresponse.Content.ReadAsStringAsync();
                    var infoGrupos = JsonConvert.DeserializeObject<RespuestaListaDeGrupos>(result);

                    if (bool.Parse(infoGrupos.status))
                    {
                        if (infoGrupos.result.Count != 0)
                        {
                            notificaciones.Clear();
                            listaDeChats.Children.Clear();
                            foreach (InfoGrupo grupo in infoGrupos.result)
                            {
                                CrearPreviewDeChat(grupo);
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("ocurrio un error en la conexion al obtener los grupos del typer");
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("ocurrio un error en la conexion al obtener los grupos del typer");
            }
        }

        private void ClickCerrarSEsion(object sender, RoutedEventArgs e)
        {
            Login ventanaLogin = new Login();
            ventanaLogin.Show();
            this.Close();
        }
    }
}
