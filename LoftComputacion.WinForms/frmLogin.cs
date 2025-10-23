using LoftComputacion.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoftComputacion.WinForms
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Cierra la aplicación por completo.
            Application.Exit();
        }

        private async void btnIngresar_Click(object sender, EventArgs e)
        {
            string nombreUsuario = txtUsuario.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, ingrese usuario y contraseña.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mostramos un cursor de espera
            this.Cursor = Cursors.WaitCursor;
            btnIngresar.Enabled = false; // Deshabilitamos el botón

            try
            {
                // 1. Llamamos al ApiClient que ya creamos
                var apiClient = new ApiClient();
                UsuarioAutenticado? usuario = await apiClient.LoginAsync(nombreUsuario, password);

                // 2. Verificamos la respuesta
                if (usuario != null)
                {
                    // ¡Login Exitoso!
                    // Ocultamos el formulario de login
                    this.Hide();

                    // Creamos y mostramos el formulario principal
                    frmPrincipal formularioPrincipal = new frmPrincipal();
                    formularioPrincipal.ShowDialog(); // ShowDialog() espera a que frmPrincipal se cierre

                    // Cuando frmPrincipal se cierre, cerramos la aplicación
                    Application.Exit();
                }
                else
                {
                    // Error de login (401 Unauthorized)
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Login Fallido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Error de conexión o cualquier otro problema
                MessageBox.Show($"No se pudo conectar con el servidor. Verifique su conexión.\n\nError: {ex.Message}", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Devolvemos el cursor y habilitamos el botón
                this.Cursor = Cursors.Default;
                btnIngresar.Enabled = true;
            }
        }
    }
}
