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
using System.Windows.Shapes;
using System.Net.Http;
using System.Text.Json;

namespace TypeMeDesktop.Ventanas
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void ClickLogin(object sender, RoutedEventArgs e)
        {
            VentanaPrincipal inicio = new VentanaPrincipal();
            inicio.Show();
            this.Close();
        }

        private void ClickCrearCuenta(object sender, RoutedEventArgs e)
        {
            RegistrarCuenta registro = new RegistrarCuenta();
            registro.Show();
            this.Close();

            //Solicitud();
        }

        //private async void Solicitud()
        //{
        //    string url = "https://jsonplaceholder.typicode.com/posts";
        //    var cliente = new HttpClient();

        //    DatosPrueba post = new DatosPrueba()
        //    {
        //        userID = 30,
        //        body = "Este es el cuerpo de la consulta",
        //        title = "Este es el titulo"
        //    };

        //    var data = JsonSerializer.Serialize<DatosPrueba>(post);
        //    HttpContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

        //    var httpResponse = await cliente.PostAsync(url, content);

        //    if (httpResponse.IsSuccessStatusCode)
        //    {
        //        var result = await httpResponse.Content.ReadAsStringAsync();
        //        var postResult = JsonSerializer.Deserialize<DatosPrueba>(result);
        //        MessageBox.Show("El resultado de la operacion es \n" + postResult.id + "\n" + postResult.userID + "\n" + postResult.title + "\n" + postResult.body);
        //    }
        //}
    }
}
