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

namespace Proyecto_Final_Carlo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtContrasena.UseSystemPasswordChar = true;     // Oculta la contraseña por defecto
        }

        private void btnRegistrar_Click(object sender, EventArgs e)     // Evento para registrar un nuevo usuario
        {
            string usuario = txtUsuario.Text.Trim();    // Elimina espacios en blanco al inicio y al final del nombre de usuario
            string contrasena = txtContrasena.Text.Trim();      // Elimina espacios en blanco al inicio y al final de la contraseña

            if (usuario == "" || contrasena == "")  // Verifica si los campos están vacíos
            {
                MessageBox.Show("Debe llenar ambos campos.");
                return;
            }   
            using (SqlConnection conn = new SqlConnection("Server=CARLO\\SQLEXPRESS01;Database=MAPIsecurity;Trusted_Connection=True;"))
            {
                conn.Open();    // Abre la conexión a la base de datos
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @usuario", conn); // Verifica si el nombre de usuario ya existe
                checkCmd.Parameters.AddWithValue("@usuario", usuario);  

                int count = (int)checkCmd.ExecuteScalar();  // Ejecuta la consulta y obtiene el número de registros que coinciden con el nombre de usuario
                if (count > 0)  // Si ya existe un usuario con ese nombre, muestra un mensaje de error
                {
                    MessageBox.Show("Ese nombre de usuario ya existe.");
                    return;
                }

                SqlCommand insertCmd = new SqlCommand("INSERT INTO Usuarios (NombreUsuario, Contrasena) VALUES (@usuario, @contrasena)", conn); // Inserta un nuevo usuario en la base de datos
                insertCmd.Parameters.AddWithValue("@usuario", usuario);
                insertCmd.Parameters.AddWithValue("@contrasena", contrasena);
                insertCmd.ExecuteNonQuery();

                MessageBox.Show("Registro exitoso.");
            }
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)     // Evento para iniciar sesión
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();

            using (SqlConnection conn = new SqlConnection("Server=CARLO\\SQLEXPRESS01;Database=MAPIsecurity;Trusted_Connection=True;"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Id FROM Usuarios WHERE NombreUsuario = @usuario AND Contrasena = @contrasena", conn);   // Consulta para verificar las credenciales del usuario
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@contrasena", contrasena);

                object result = cmd.ExecuteScalar();    // Ejecuta la consulta y obtiene el ID del usuario si las credenciales son correctas
                if (result != null) // Si las credenciales son correctas, se obtiene el ID del usuario
                {
                    int usuarioId = Convert.ToInt32(result);
                    FormMenu menu = new FormMenu(usuarioId);
                    menu.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Credenciales incorrectas.");
                }
            }
        }
        private void txtContrasena_TextChanged(object sender, EventArgs e)
        {

        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)   // Evento para mostrar u ocultar la contraseña
        {
            if (checkBoxContraseña.Checked)
            {
                txtContrasena.UseSystemPasswordChar = false;
            }
            else
            {
                txtContrasena.UseSystemPasswordChar = true;
            }
        }
    }
}
