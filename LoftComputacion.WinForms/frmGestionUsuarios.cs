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
    public partial class frmGestionUsuarios : Form
    {
        private readonly ApiClient _apiClient;

        public frmGestionUsuarios(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
        }

        private async void frmGestionUsuarios_Load(object sender, EventArgs e)
        {
            await CargarUsuarios();
        }

        private async Task CargarUsuarios()
        {
            try
            {
                // Llamamos a la API para obtener la lista
                var usuarios = await _apiClient.GetUsuariosAsync();

                dgvUsuarios.DataSource = null; // Limpiamos
                dgvUsuarios.DataSource = usuarios; // Asignamos los datos

                // Configuramos las columnas
                if (dgvUsuarios.Columns.Count > 0)
                {
                    // Ocultamos el Hash (¡MUY IMPORTANTE!)
                    dgvUsuarios.Columns[nameof(Usuario.PasswordHash)].Visible = false;

                    // Renombramos las cabeceras
                    dgvUsuarios.Columns[nameof(Usuario.Id)].HeaderText = "ID";
                    dgvUsuarios.Columns[nameof(Usuario.NombreCompleto)].HeaderText = "Nombre Completo";
                    dgvUsuarios.Columns[nameof(Usuario.Email)].HeaderText = "Email";
                    dgvUsuarios.Columns[nameof(Usuario.Rol)].HeaderText = "Rol";

                    // Ajustamos el tamaño
                    dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnEliminarUsuario_Click(object sender, EventArgs e)
        {
            // 1. Verificar que haya un usuario seleccionado
            if (dgvUsuarios.CurrentRow == null || dgvUsuarios.CurrentRow.DataBoundItem is not Usuario usuarioSeleccionado)
            {
                MessageBox.Show("Por favor, seleccione un usuario de la lista para eliminar.", "Ningún usuario seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Pedir confirmación
            var confirmResult = MessageBox.Show(
                $"¿Está seguro de que desea eliminar al usuario '{usuarioSeleccionado.NombreCompleto}'?\nEsta acción es permanente.",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // 3. Llamar a la API para eliminar
                    await _apiClient.DeleteUsuarioAsync(usuarioSeleccionado.Id); // <-- Necesitamos crear este método

                    MessageBox.Show("Usuario eliminado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 4. Recargar la grilla para que desaparezca el usuario
                    await CargarUsuarios();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnNuevoUsuario_Click(object sender, EventArgs e)
        {
            // Abrimos el nuevo formulario de detalle en modo "Creación"
            using (var formDetalle = new frmUsuarioDetalle(_apiClient))
            {
                // Mostramos el formulario de detalle
                if (formDetalle.ShowDialog() == DialogResult.OK)
                {
                    // Si el usuario guardó (DialogResult.OK),
                    // recargamos la grilla para ver el nuevo usuario.
                    await CargarUsuarios();
                }
            }
        }

        private async void btnEditarUsuario_Click(object sender, EventArgs e)
        {
            // 1. Verificar que haya un usuario seleccionado
            if (dgvUsuarios.CurrentRow == null || dgvUsuarios.CurrentRow.DataBoundItem is not Usuario usuarioSeleccionado)
            {
                MessageBox.Show("Por favor, seleccione un usuario de la lista para editar.", "Ningún usuario seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Abrimos el formulario de detalle en modo "Edición"
            using (var formDetalle = new frmUsuarioDetalle(usuarioSeleccionado, _apiClient))
            {
                if (formDetalle.ShowDialog() == DialogResult.OK)
                {
                    // Si el usuario actualizó, recargamos la grilla
                    await CargarUsuarios();
                }
            }
        }
    }
}
