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
        public frmLogin(ApiClient apiClient) // <-- CAMBIO AQUÍ
        {
            InitializeComponent();
            _apiClient = apiClient; // <-- CAMBIO AQUÍ (Guardamos la instancia recibida)
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Ya no cierra la aplicación, solo el formulario con resultado "Cancelar"
            this.DialogResult = DialogResult.Cancel;
            this.Close();
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

            this.Cursor = Cursors.WaitCursor;
            btnIngresar.Enabled = false;
            btnIngresar.Text = "Ingresando...";

            try
            {
                // 1. Llamamos a LoginAsync
                var loginResponse = await _apiClient.LoginAsync(nombreUsuario, password);

                // --- CORRECCIÓN AQUÍ ---
                // Verificamos el objeto 'loginResponse', no la variable 'token'
                if (loginResponse != null)
                {
                    // ¡Login Exitoso!
                    // 2. Establecemos el token usando la propiedad del objeto
                    _apiClient.SetToken(loginResponse.Token);

                    // 3. Indicamos al Program.cs que el login fue exitoso
                    this.DialogResult = DialogResult.OK;
                    this.Close(); // Cerramos el login
                }
                // --- FIN CORRECCIÓN ---
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Login Fallido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo conectar con el servidor: {ex.Message}", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnIngresar.Enabled = true;
                btnIngresar.Text = "Ingresar";
            }
        }
    }
}