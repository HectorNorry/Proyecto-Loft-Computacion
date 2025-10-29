using System;
using System.Windows.Forms;

namespace LoftComputacion.WinForms
{
    public partial class frmPasswordPrompt : Form
    {
        private readonly ApiClient _apiClient;

        public frmPasswordPrompt(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
        }

        private async void btnAceptarPass_Click(object sender, EventArgs e)
        {
            // 1. Obtenemos los datos de ESTE formulario
            string adminUsername = txtAdminUsuario.Text;
            string password = txtPasswordAdmin.Text;

            if (string.IsNullOrWhiteSpace(adminUsername) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, ingrese usuario y contraseña de administrador.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            btnAceptarPass.Enabled = false;

            try
            {
                // 2. Llamamos al Login con las credenciales ingresadas AHORA
                var loginResponse = await _apiClient.LoginAsync(adminUsername, password);

                // 3. Verificamos DOS cosas:
                //    a) Que el login sea exitoso (usuario y pass correctos)
                //    b) Que el usuario tenga el ROL de "Administrador"
                if (loginResponse != null && loginResponse.Rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
                {
                    // ¡Éxito! Es un admin validado
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    // Si loginResponse es null (falló login) O el rol no es "Administrador"
                    MessageBox.Show("Credenciales incorrectas o el usuario no tiene permisos de administrador.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al validar: {ex.Message}", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnAceptarPass.Enabled = true;
            }
        }
    }
}