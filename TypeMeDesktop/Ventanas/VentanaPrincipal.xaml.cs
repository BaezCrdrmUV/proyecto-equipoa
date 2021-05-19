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
using TypeMeDesktop.Paginas;

namespace TypeMeDesktop.Ventanas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class VentanaPrincipal : Window
    {
        private InformacionTyper perfilTyper;
        private string urlListaDeContactos = "http://localhost:4000/typers/obtenerContactos/";
        private List<InfoContacto> listaDeContactos;
        private bool consultaContactosTerminada = false;

        public VentanaPrincipal(InformacionTyper typer)
        {
            InitializeComponent();

            this.perfilTyper = typer;
            this.infoHeader.Text = perfilTyper.Username;
            listaDeContactos = new List<InfoContacto>();

            APIObtenerListaDeContactos();


            Random numeros = new Random();
            for (int i = 0; i < 10; i++)
            {
                CrearPreviewDeChat(numeros.Next().ToString());
            }
        }

        private void ClickNuevoChat(object sender, RoutedEventArgs e)
        {
            if (TerminoConsulta() && HayContactos())
            {
                PaginaFrame.Navigate(new ListaDeContactos(listaDeContactos, perfilTyper.IdTyper));
            }
        }

        private void ClickNuevoContacto(object sender, RoutedEventArgs e)
        {
            PaginaFrame.Navigate(new AgregarContacto(perfilTyper.IdTyper));
        }

        private void ClickMiCuenta(object sender, RoutedEventArgs e)
        {
            PaginaFrame.Navigate(new MiPerfil(perfilTyper));
        }

        private void CrearPreviewDeChat(string identificador)
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
            imagen.ImageSource = new BitmapImage(new Uri("C:\\Users\\Angel\\Desktop\\Exportacion\\3.jpg"));
            imagenPerfil.Fill = imagen;

            TextBlock nombreContacto = new TextBlock();
            nombreContacto.Text = "Angel de jesus juarez Garcia - " + identificador; //Aqui se toma del json
            nombreContacto.VerticalAlignment = VerticalAlignment.Center;
            nombreContacto.Margin = new Thickness(15, 0, 0, 0);
            nombreContacto.TextWrapping = TextWrapping.Wrap;
            nombreContacto.Width = 150;


            preview.Children.Add(imagenPerfil);
            preview.Children.Add(nombreContacto);
            nuevaPreviewChat.Content = preview;

            nuevaPreviewChat.Click += (s, ev) =>
            {
                MessageBox.Show(nombreContacto.Text);
            };

            listaDeChats.Children.Add(nuevaPreviewChat);
        }

        private bool HayContactos()
        {
            if (listaDeContactos.Count <= 0)
            {
                MessageBox.Show("No cuentas con contactos para iniciar un chat");
                return false;
            }

            return true;
        }

        private bool TerminoConsulta()
        {
            bool termino = consultaContactosTerminada;
            if (!termino)
            {
                MessageBox.Show("Espera a que se carguen tus contactos");
                return termino;
            }
            return termino;
        }

        private async void APIObtenerListaDeContactos()
        {
            var cliente = new HttpClient();

            try
            {
                var httpresponse = await cliente.GetAsync(String.Concat(urlListaDeContactos, perfilTyper.IdTyper));

                if (httpresponse.IsSuccessStatusCode)
                {
                    var result = await httpresponse.Content.ReadAsStringAsync();
                    var infoContactos = JsonConvert.DeserializeObject<RespuestaAPI>(result);

                    if (bool.Parse(infoContactos.status))
                    {
                        listaDeContactos = infoContactos.result;
                    }
                }
                consultaContactosTerminada = true;
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

    }
}
