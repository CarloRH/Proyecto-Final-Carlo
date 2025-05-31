using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Final_Carlo
{
    public partial class FormMenu : Form
    {
        private int usuarioId;  // Almacena el ID del usuario actual
        public FormMenu(int idUsuario)  //Constructor que recibe el ID del usuario actual
        {
            InitializeComponent();
            this.usuarioId = idUsuario;
        }

        private void FormMenu_Load(object sender, EventArgs e)
        {

        }

        private void btnReporte_Click(object sender, EventArgs e)   // Evento para abrir el formulario de reporte
        {
            FormReporte reporte = new FormReporte(usuarioId);
            reporte.Show();
            this.Hide();
        }

        private void btnConsultar_Click(object sender, EventArgs e) // Evento para abrir el formulario de consulta
        {
            FormConsulta consulta = new FormConsulta(usuarioId);
            consulta.Show();
            this.Hide(); 
        }

        private void btnMapa_Click(object sender, EventArgs e)  // Evento para abrir el formulario de mapa
        {
            FormMapa mapa = new FormMapa(usuarioId);
            mapa.Show();
            this.Hide();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)  // Evento para cerrar sesión y salir de la aplicación
        {
            this.Close();
            Application.Exit();
        }

        private void lblUsuario_Click(object sender, EventArgs e)
        {

        }

        private void lblUsuario_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
