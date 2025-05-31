using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace Proyecto_Final_Carlo
{
    public partial class FormConsulta : Form
    {
        private int usuarioId;
        public FormConsulta(int idUsuario)
        {
            InitializeComponent();
            this.usuarioId = idUsuario;
        }

        private void CargarReportes()   // Carga los reportes desde la base de datos y los muestra en el DataGridView
        {
            using (SqlConnection conn = new SqlConnection("Server=CARLO\\SQLEXPRESS01;Database=MAPIsecurity;Trusted_Connection=True;")) 
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT R.Id, U.NombreUsuario, R.Tipo, R.Descripcion, R.Fecha, R.Latitud, R.Longitud FROM Reportes R INNER JOIN Usuarios U ON R.UsuarioId = U.Id", conn);
                DataTable table = new DataTable();  // Crea una tabla para almacenar los datos de los reportes
                adapter.Fill(table);    // Llena la tabla con los datos de la consulta
                dataGridViewReportes.DataSource = table;    // Asigna la tabla de datos al DataGridView
            }
        }
        private string GenerarMapaBase()    // Genera el HTML base para el mapa utilizando Leaflet
        {   // Esta función devuelve un string con el HTML necesario para mostrar un mapa básico
            return @"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8' />
    <link rel='stylesheet' href='https://unpkg.com/leaflet@1.7.1/dist/leaflet.css' />
    <script src='https://unpkg.com/leaflet@1.7.1/dist/leaflet.js'></script>
    <style>
        html, body { height: 100%; margin: 0; }
        #mapid { height: 100%; width: 100%; }
    </style>
</head>
<body>
    <div id='mapid'></div>
    <script>
        var map = L.map('mapid').setView([14.28, -89.89], 13);
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap contributors'
        }).addTo(map);

        window.centrarMapa = function(lat, lng) {
            map.setView([lat, lng], 18);
            L.marker([lat, lng]).addTo(map);
        };
    </script>
</body>
</html>";
        }
        private async void FormConsulta_Load(object sender, EventArgs e)    // Evento que se ejecuta al cargar el formulario
        {
            await webViewConsulta.EnsureCoreWebView2Async(null);    // Asegura que el WebView2 esté listo antes de cargar el HTML
            webViewConsulta.NavigateToString(GenerarMapaBase());    // Carga el HTML del mapa en el WebView2
            CargarReportes(); 
        }

        private void dataGridViewReportes_CellContentClick(object sender, DataGridViewCellEventArgs e)  // Evento que se dispara al hacer clic en una celda del DataGridView
        {
            if (e.RowIndex >= 0)    // Verifica que la fila sea válida
            {   // Obtiene la latitud y longitud de la fila seleccionada
                double lat = Convert.ToDouble(dataGridViewReportes.Rows[e.RowIndex].Cells["Latitud"].Value);    // Obtiene la latitud de la celda seleccionada
                double lon = Convert.ToDouble(dataGridViewReportes.Rows[e.RowIndex].Cells["Longitud"].Value);   // Obtiene la latitud y longitud de la celda seleccionada
                string script = $"centrarMapa({lat.ToString().Replace(',', '.')}, {lon.ToString().Replace(',', '.')});";    // Ejecuta el script para centrar el mapa en la ubicación del reporte seleccionado
                webViewConsulta.ExecuteScriptAsync(script); // Ejecuta el script en el WebView2 para centrar el mapa en la ubicación del reporte seleccionado
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            FormMenu menu = new FormMenu(usuarioId);
            menu.Show();
            this.Hide();
            this.Close();
        }
    }
}
