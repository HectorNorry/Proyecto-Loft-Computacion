using LoftComputacion.Domain;
using System;
using System.Windows.Forms;
// (No necesitás todos los otros usings si no los usás directamente aquí)

namespace LoftComputacion.WinForms
{
    public partial class frmLogin : Form
    {
        // Guardamos la instancia ÚNICA del ApiClient que nos pasa el Program.cs
        private readonly ApiClient _apiClient;

        /// <summary>
        /// Constructor modificado. Ahora RECIBE la instancia del ApiClient.
        /// </summary>
        /// <param name="apiClient">La instancia única del cliente API para toda la aplicación.</param>
        public frmLogin(ApiClient apiClient) // <-- CAMBIO AQUÍ
        {
            InitializeComponent();
            _apiClient = apiClient; // <-- CAMBIO AQUÍ (Guardamos la instancia recibida)
        }

        /// <summary>
        /// Cierra la aplicación por completo si el usuario cancela el login.
        /// </summary>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Maneja el intento de inicio de sesión.
        /// </summary>
        private async void btnIngresar_Click(object sender, EventArgs e)
        {
            string nombreUsuario = txtUsuario.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, ingrese usuario y contraseña.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mostramos feedback visual al usuario
            this.Cursor = Cursors.WaitCursor;
            btnIngresar.Enabled = false;
            btnIngresar.Text = "Ingresando...";

            try
            {
                // 1. Llamamos a LoginAsync usando la instancia _apiClient del formulario
                string? token = await _apiClient.LoginAsync(nombreUsuario, password);

                if (!string.IsNullOrEmpty(token))
                {
                    // ¡Login Exitoso! 

                    // 2. Establecemos el token en la instancia compartida del ApiClient
                    _apiClient.SetToken(token);

                    // 3. Indicamos al Program.cs que el login fue exitoso
                    this.DialogResult = DialogResult.OK;

                    // 4. Cerramos este formulario para que Program.cs pueda abrir el frmPrincipal
                    this.Close();
                }
                else
                {
                    // Error de login (401 Unauthorized), la API devolvió null
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
                // Restauramos la interfaz sin importar el resultado
                this.Cursor = Cursors.Default;
                btnIngresar.Enabled = true;
                btnIngresar.Text = "Ingresar";
            }
        }
    }
}