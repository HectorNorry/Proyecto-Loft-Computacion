using LoftComputacion.Domain;
using System;
using System.Windows.Forms;

namespace LoftComputacion.WinForms
{
    public partial class frmUsuarioDetalle : Form
    {
        private readonly ApiClient _apiClient;
        private Usuario? _usuarioParaEditar = null; // Para guardar el usuario en modo edición

        // Constructor único que recibe el ApiClient
        public frmUsuarioDetalle(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
        }

        // Constructor para MODO EDICIÓN
        public frmUsuarioDetalle(Usuario usuario, ApiClient apiClient) : this(apiClient)
        {
            _usuarioParaEditar = usuario; // Guardamos el usuario a editar
        }

        private void frmUsuarioDetalle_Load(object sender, EventArgs e)
        {
            // Llenamos el ComboBox de Roles
            cmbRolUsuario.Items.Add("Administrador");
            cmbRolUsuario.Items.Add("Empleado");

            if (_usuarioParaEditar != null)
            {
                // --- MODO EDICIÓN ---
                this.Text = "Editar Usuario";
                btnGuardarUsuario.Text = "Actualizar";

                txtNombreUsuario.Text = _usuarioParaEditar.NombreCompleto;
                txtEmailUsuario.Text = _usuarioParaEditar.Email;
                cmbRolUsuario.SelectedItem = _usuarioParaEditar.Rol;

                txtPasswordUsuario.PlaceholderText = "Dejar en blanco para no cambiar";
            }
            else
            {
                // --- MODO CREACIÓN ---
                this.Text = "Nuevo Usuario";
                btnGuardarUsuario.Text = "Guardar";
                cmbRolUsuario.SelectedIndex = 1; // "Empleado" por defecto
            }
        }

        private async void btnGuardarUsuario_Click(object sender, EventArgs e)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(txtNombreUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtEmailUsuario.Text) ||
                cmbRolUsuario.SelectedItem == null)
            {
                MessageBox.Show("Los campos Nombre, Email y Rol son obligatorios.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Si estamos en modo Creación, la contraseña es obligatoria
            if (_usuarioParaEditar == null && string.IsNullOrWhiteSpace(txtPasswordUsuario.Text))
            {
                MessageBox.Show("La contraseña es obligatoria al crear un nuevo usuario.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            try
            {
                if (_usuarioParaEditar == null)
                {
                    // --- LÓGICA DE CREAR (la que ya tenías) ---
                    await _apiClient.CreateUsuarioAsync(
                        txtNombreUsuario.Text,
                        txtEmailUsuario.Text,
                        txtPasswordUsuario.Text,
                        cmbRolUsuario.SelectedItem.ToString()
                    );
                    MessageBox.Show("Usuario creado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // --- LÓGICA DE ACTUALIZAR ---

                    // Verificamos si el usuario escribió una nueva contraseña
                    string? nuevaPassword = null;
                    if (!string.IsNullOrWhiteSpace(txtPasswordUsuario.Text))
                    {
                        nuevaPassword = txtPasswordUsuario.Text;
                    }

                    // Llamamos al ApiClient con los datos actualizados
                    await _apiClient.UpdateUsuarioAsync(
                        _usuarioParaEditar.Id,
                        txtNombreUsuario.Text,
                        txtEmailUsuario.Text,
                        cmbRolUsuario.SelectedItem.ToString(),
                        nuevaPassword // Pasamos la nueva contraseña o null
                    );

                    MessageBox.Show("Usuario actualizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelarUsuario_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}