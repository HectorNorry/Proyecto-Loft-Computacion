using LoftComputacion.Domain;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LoftComputacion.WinForms
{
    public partial class frmConfirmarCambio : Form
    {
        private readonly ApiClient _apiClient;
        public Usuario UsuarioSeleccionado { get; private set; } // Para devolver el usuario validado

        public frmConfirmarCambio(ApiClient apiClient) // Recibe el ApiClient
        {
            InitializeComponent();
            _apiClient = apiClient; // Asigna la instancia recibida (que ya tiene el token)
        }

        private async void frmConfirmarCambio_Load(object sender, EventArgs e)
        {
            try
            {
                var usuarios = await _apiClient.GetUsuariosAsync(); // Llamamos al API para obtener usuarios
                cmbUsuarios.DisplayMember = "NombreCompleto"; // Propiedad a mostrar en el ComboBox
                cmbUsuarios.ValueMember = "Id";               // Propiedad que será el valor real
                cmbUsuarios.DataSource = usuarios;            // Asignamos la lista de usuarios
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel; // Cerramos el formulario si falla
                this.Close();
            }
        }

        private async void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (cmbUsuarios.SelectedItem == null || string.IsNullOrWhiteSpace(txtPasswordConfirmacion.Text))
            {
                MessageBox.Show("Por favor, seleccione un usuario e ingrese la contraseña.", "Datos Requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var usuario = cmbUsuarios.SelectedItem as Usuario;
            var passwordIngresada = txtPasswordConfirmacion.Text;

            this.Cursor = Cursors.WaitCursor;
            btnConfirmar.Enabled = false;

            try
            {
                // --- CORRECCIÓN AQUÍ ---
                // 1. Cambiamos 'string? tokenRespuesta' por 'var loginResponse'
                var loginResponse = await _apiClient.LoginAsync(usuario.NombreCompleto, passwordIngresada);

                // 2. Comprobamos 'loginResponse', no 'tokenRespuesta'
                if (loginResponse != null)
                // --- FIN CORRECCIÓN ---
                {
                    // ¡Éxito! La contraseña es correcta.
                    UsuarioSeleccionado = usuario;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Contraseña incorrecta.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al validar: {ex.Message}", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnConfirmar.Enabled = true;
            }
        }




        private void btnCancelarConfirmacion_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; // Marcamos cancelación
            this.Close();
        }
    }
}