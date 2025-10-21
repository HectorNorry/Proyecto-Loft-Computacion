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

        public frmConfirmarCambio()
        {
            InitializeComponent();
            _apiClient = new ApiClient();
            // Inicializamos UsuarioSeleccionado para evitar advertencias
            UsuarioSeleccionado = new Usuario();

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

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            // Validación simple de contraseña
            if (txtPasswordConfirmacion.Text == "1234") // ¡IMPORTANTE! Esto es una contraseña SIMULADA para desarrollo.
                                              // En un sistema real, NUNCA harías esto y verificarías contra un hash.
            {
                if (cmbUsuarios.SelectedItem is Usuario usuarioConfirmado)
                {
                    UsuarioSeleccionado = usuarioConfirmado; // Guardamos el usuario
                    this.DialogResult = DialogResult.OK; // Indicamos que la confirmación fue exitosa
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione un usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Contraseña incorrecta.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // --- MÉTODO TEMPORAL DE SIMULACIÓN ---
        // Este método DEBE reemplazarse por una llamada a la API
        private async Task<bool> ValidarPasswordSimuladoAsync(int usuarioId, string password)
        {
            // Simulación MUY BÁSICA: Suponemos que la contraseña correcta es "1234" para todos
            await Task.Delay(100); // Simula llamada a la red
            return password == "1234";
        }
        // --- FIN MÉTODO TEMPORAL ---


        private void btnCancelarConfirmacion_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; // Marcamos cancelación
            this.Close();
        }
    }
}