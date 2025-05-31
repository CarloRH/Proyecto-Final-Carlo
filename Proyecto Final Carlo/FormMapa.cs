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
using System.Media;
using Microsoft.Web.WebView2.Core;

namespace Proyecto_Final_Carlo
{

    public partial class FormMapa : Form
    {
        private List<ReporteUbicacion> reportes = new List<ReporteUbicacion>(); // Lista para almacenar los reportes de ubicación
        private Timer alertTimer;   // Temporizador para verificar ubicaciones cercanas
        private bool markersAdded = false;  // Indica si los marcadores ya han sido agregados al mapa
        private int usuarioId;  // ID del usuario actual
        public FormMapa(int idUsuario)  // Constructor que recibe el ID del usuario actual
        {
            InitializeComponent();
            InicializarTimer();     // Inicializa el temporizador para verificar ubicaciones cercanas 
            this.usuarioId = idUsuario;     // Asigna el ID del usuario actual a la variable de instancia
        }
        private void InicializarTimer()   // Inicializa el temporizador para verificar ubicaciones cercanas cada 5 segundos
        {
            alertTimer = new Timer();
            alertTimer.Interval = 5000; // 5 segundos
            alertTimer.Tick += VerificarUbicacionCercana;
            alertTimer.Start();     
        }
        private async void InicializarMapa()    //se inicializa el mapa y se carga el HTML
        {
            await webViewMapa.EnsureCoreWebView2Async(null);    // Asegura que el WebView2 esté listo antes de cargar el HTML
            // Crea el HTML para el mapa con Leaflet
            string htmlMapa = @"    
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <link rel='stylesheet' href='https://unpkg.com/leaflet@1.7.1/dist/leaflet.css' />
    <script src='https://unpkg.com/leaflet@1.7.1/dist/leaflet.js'></script>
    <style>
        html, body { margin: 0; height: 100%; }
        #mapid { height: 100%; width: 100%; }
    </style>
</head>
<body>
    <div id='mapid'></div>
    <script>
        var mymap = L.map('mapid').setView([14.28, -89.89], 14);
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap contributors'
        }).addTo(mymap);

        window.agregarMarcador = function(lat, lng, tipo, descripcion) {
            var color = 'blue';
            switch(tipo.toLowerCase()) {
                case 'asalto': color = 'red'; break;
                case 'robo': color = 'orange'; break;
                case 'accidente': color = 'yellow'; break;
                case 'acoso': color = 'purple'; break;
                case 'vandalismo': color = 'green'; break;
            }
            var marker = L.marker([lat, lng]).addTo(mymap);
            marker.bindPopup('<b>' + tipo + '</b><br>' + descripcion);
        };
    </script>
</body>
</html>";

            webViewMapa.NavigateToString(htmlMapa); // Carga el HTML en el WebView2

            // Esperar a que cargue y luego agregar marcadores
            webViewMapa.CoreWebView2.NavigationCompleted += (s, e) => {
                if (!markersAdded)
                {
                    CargarReportesYAgregarMarcadores();     // Carga los reportes desde la base de datos y agrega los marcadores al mapa
                }
            };
        }
        private void VerificarUbicacionCercana(object sender, EventArgs e)
        {
            double usuarioLat = 14.28;
            double usuarioLng = -89.89;

            foreach (var r in reportes)
            {
                double distancia = CalcularDistancia(usuarioLat, usuarioLng, r.Latitud, r.Longitud);    // Calcula la distancia entre la ubicación del usuario y la ubicación del reporte
                if (distancia < 200)
                {
                    SystemSounds.Beep.Play();   // Reproduce un sonido de alerta si el usuario está cerca de un reporte
                    MessageBox.Show($"¡Estás cerca de un {r.Tipo}!\n{r.Descripcion}", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }
            }
        }

        private double CalcularDistancia(double lat1, double lng1, double lat2, double lng2)    // Calcula la distancia entre dos puntos geográficos utilizando la fórmula del haversine
        {
            const double R = 6371000;
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLng = (lng2 - lng1) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                       Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
        private void CargarReportesYAgregarMarcadores()
        {
            reportes.Clear();   // Limpia la lista de reportes antes de cargar nuevos datos
            try
            {
                using (SqlConnection conn = new SqlConnection("Server=CARLO\\SQLEXPRESS01;Database=MAPIsecurity;Trusted_Connection=True;"))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Tipo, Descripcion, Latitud, Longitud FROM Reportes", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var r = new ReporteUbicacion
                        {
                            Tipo = reader.GetString(0),
                            Descripcion = reader.GetString(1),
                            Latitud = reader.GetDouble(2),
                            Longitud = reader.GetDouble(3)
                        };
                        reportes.Add(r);

                        string script = $"agregarMarcador({r.Latitud.ToString().Replace(',', '.')}, {r.Longitud.ToString().Replace(',', '.')}, '{r.Tipo}', '{r.Descripcion.Replace("'", "\\'")}')";
                        webViewMapa.ExecuteScriptAsync(script);
                    }
                }
                markersAdded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los reportes: " + ex.Message); // Muestra un mensaje de error si ocurre un problema al cargar los reportes
            }
        }
        private void FormMapa_Load(object sender, EventArgs e)
        {
            InicializarMapa();
        }
        private void FormMapa_FormClosing(object sender, FormClosingEventArgs e)
        {
            alertTimer?.Stop();
            alertTimer?.Dispose();
        }

        private void btnActualizar_Click(object sender, EventArgs e)    // Botón para actualizar el mapa y recargar los reportes
        {
            markersAdded = false;   // Reinicia la variable para permitir que se agreguen nuevos marcadores
            CargarReportesYAgregarMarcadores();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            alertTimer?.Stop();
            FormMenu menu = new FormMenu(usuarioId);    // Crea una nueva instancia del formulario de menú
            menu.Show();
            this.Hide();
            this.Close();
        }

        private void webBrowserMapa_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
