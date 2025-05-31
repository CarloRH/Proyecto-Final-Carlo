using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Web.WebView2.Core;

namespace Proyecto_Final_Carlo
{
    public partial class FormReporte : Form
    {
        private int usuarioId;
        private double latitud; // Almacena la latitud seleccionada en el mapa
        private double longitud;    // Almacena la longitud seleccionada en el mapa
        public FormReporte(int idUsuario)
        {
            InitializeComponent();
            usuarioId = idUsuario;
        }
    

        private async void FormReporte_Load(object sender, EventArgs e) // Evento que se ejecuta al cargar el formulario
        {
            await webViewReporte.EnsureCoreWebView2Async(null); // Asegura que el WebView2 esté listo antes de cargar el HTML
            // Crea el HTML para el mapa con Leaflet
            string html = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <link rel='stylesheet' href='https://unpkg.com/leaflet@1.7.1/dist/leaflet.css' />
    <script src='https://unpkg.com/leaflet@1.7.1/dist/leaflet.js'></script>
    <style>
        html, body, #mapid { height: 100%; margin: 0; padding: 0; }
    </style>
</head>
<body>
    <div id='mapid'></div>
    <script>
        var map = L.map('mapid').setView([14.28, -89.89], 13);
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap contributors'
        }).addTo(map);

        var marker;

        map.on('click', function(e) {
            var lat = e.latlng.lat;
            var lng = e.latlng.lng;
            if (marker) map.removeLayer(marker);
            marker = L.marker([lat, lng]).addTo(map);
            window.chrome.webview.postMessage({ lat: lat, lng: lng });
        });
    </script>
</body>
</html>";
            webViewReporte.NavigateToString(html);  // Carga el HTML en el WebView2

            webViewReporte.WebMessageReceived += WebViewReporte_WebMessageReceived; // Evento que se dispara cuando se recibe un mensaje del WebView2
        }
        private void WebViewReporte_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)    // Evento que maneja el mensaje recibido del WebView2
        {
            try    //  Deserializa el mensaje JSON recibido del WebView2
            {
                var json = e.WebMessageAsJson;
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                latitud = (double)obj.lat;
                longitud = (double)obj.lng;
            }
            catch { /* Silencio errores por seguridad */ }
        }
        /*private async Task<string> ClasificarConGemini(string descripcion)  // Método que clasifica el incidente usando Google Gemini (Infuncional)
        {
            string apiKey = ""; //Usa tu clave real de Gemini aquí
            string endpoint = $"https://generativelanguage.googleapis.com/v1/models/gemini-pro:generateContent?key={apiKey}";

            var prompt = $"Clasifica el siguiente incidente en una de estas categorías: Robo, Asalto, Accidente de tránsito, Acoso, Vandalismo, Otro.\n\nIncidente: \"{descripcion}\"\n\nCategoría:";

            var requestBody = new
            {
                contents = new[]
                {
            new {
                role = "user",
                parts = new[]
                {
                    new { text = prompt }
                }
            }
        }
            };
            var client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(endpoint, content);
            string resultText = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                dynamic result = JsonConvert.DeserializeObject(resultText);
                string output = result.candidates[0].content.parts[0].text;
                return output.Trim();
            }
            else
            {
                MessageBox.Show($"Error de Google Gemini ({response.StatusCode}):\n{resultText}");
                return "SinClasificar";
            }
        }
        */
        private Task<string> ClasificarIncidente(string descripcion)    // Método que clasifica el incidente basado en palabras clave en la descripción
        {
            descripcion = descripcion.ToLower();    // Convierte la descripción a minúsculas para facilitar la comparación

            if (descripcion.Contains("asalto") || descripcion.Contains("arma") || descripcion.Contains("agresion"))
                return Task.FromResult("Asalto");

            if (descripcion.Contains("robo") || descripcion.Contains("cartera") || descripcion.Contains("celular"))
                return Task.FromResult("Robo");

            if (descripcion.Contains("accidente") || descripcion.Contains("choque") || descripcion.Contains("atropello"))
                return Task.FromResult("Accidente");

            if (descripcion.Contains("acoso") || descripcion.Contains("seguimiento") || descripcion.Contains("mirada"))
                return Task.FromResult("Acoso");

            if (descripcion.Contains("vandalismo") || descripcion.Contains("graffiti") || descripcion.Contains("daño"))
                return Task.FromResult("Vandalismo");

            return Task.FromResult("Otro");
        }       // Se pueden agregar más palabras clave según sea necesario

        private async void btnGuardarReporte_Click(object sender, EventArgs e)  // Evento que se ejecuta al hacer clic en el botón "Guardar Reporte"
        {
            string descripcion = txtDescripcion.Text.Trim();    //  Elimina espacios en blanco al inicio y al final de la descripción del incidente
            if (string.IsNullOrEmpty(descripcion))  //  Verifica si la descripción está vacía
            {
                MessageBox.Show("Debe ingresar una descripción del incidente.");
                return;
            }

            if (latitud == 0 || longitud == 0)  // Verifica si la latitud y longitud son cero, lo que indica que no se ha seleccionado una ubicación en el mapa
            {
                MessageBox.Show("Debe seleccionar una ubicación en el mapa.");
                return;
            }
            string tipo = await ClasificarIncidente(descripcion);   // Clasifica el incidente basado en la descripción ingresada
            using (SqlConnection conn = new SqlConnection("Server=CARLO\\SQLEXPRESS01;Database=MAPIsecurity;Trusted_Connection=True;")) 
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Reportes (UsuarioId, Tipo, Descripcion, Latitud, Longitud, Fecha) VALUES (@uid, @tipo, @desc, @lat, @long, @fecha)", conn);
                cmd.Parameters.AddWithValue("@uid", usuarioId);
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@desc", descripcion);
                cmd.Parameters.AddWithValue("@lat", latitud);
                cmd.Parameters.AddWithValue("@long", longitud);
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                cmd.ExecuteNonQuery();  

                MessageBox.Show("Reporte enviado exitosamente.");
            }
        }
        private void webViewReporte_Click(object sender, EventArgs e)
        {

        }
        private void btnCerrar_Click_1(object sender, EventArgs e)
        {
            FormMenu menu = new FormMenu(usuarioId);
            menu.Show();
            this.Hide();
            this.Close();
        }
    }
}
