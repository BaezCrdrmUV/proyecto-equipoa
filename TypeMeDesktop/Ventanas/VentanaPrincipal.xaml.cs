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
using TypeMeDesktop.ComunicacionAPI.Contactos;
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
        private string urlListaDeGrupos = "http://localhost:4000/mensajes/misGrupos/";

        public VentanaPrincipal(InformacionTyper typer)
        {
            InitializeComponent();

            this.perfilTyper = typer;
            this.infoHeader.Text = perfilTyper.Username;
            PaginaFrame.Navigate(new Bienvenida());

            APIObtenerListaDeGrupos();
        }

        private void ClickNuevoChat(object sender, RoutedEventArgs e)
        {
            PaginaFrame.Navigate(new ListaDeContactos(perfilTyper.IdTyper, this));
        }

        private void ClickNuevoContacto(object sender, RoutedEventArgs e)
        {
            PaginaFrame.Navigate(new AgregarContacto(perfilTyper.IdTyper));
        }

        private void ClickMiCuenta(object sender, RoutedEventArgs e)
        {
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
            imagen.ImageSource = new BitmapImage(new Uri("C:\\Users\\Angel\\Desktop\\Exportacion\\4.png"));
            imagenPerfil.Fill = imagen;

            TextBlock nombreGrupo = new TextBlock();
            nombreGrupo.Text = grupo.Nombre; 
            nombreGrupo.VerticalAlignment = VerticalAlignment.Center;
            nombreGrupo.Margin = new Thickness(15, 0, 0, 0);
            nombreGrupo.TextWrapping = TextWrapping.Wrap;
            nombreGrupo.Width = 150;


            preview.Children.Add(imagenPerfil);
            preview.Children.Add(nombreGrupo);
            nuevaPreviewChat.Content = preview;

            nuevaPreviewChat.Click += (s, ev) =>
            {
                PaginaFrame.Navigate(new ChatGrupal(grupo.IdGrupo, perfilTyper)); 
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
    }
}
