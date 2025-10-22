using LoftComputacion.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks; // Necesario
using System.Windows.Forms;

namespace LoftComputacion.WinForms
{
    public partial class frmSeleccionarEstado : Form
    {
        private readonly ApiClient _apiClient;
        // Propiedad pública para devolver el estado seleccionado
        public Estado EstadoSeleccionado { get; private set; } = null!;

        public frmSeleccionarEstado()
        {
            InitializeComponent();
            _apiClient = new ApiClient();
        }

        private async void frmSeleccionarEstado_Load(object sender, EventArgs e)
        {
            try
            {
                // Cargamos todos los estados posibles desde la API
                var estados = await _apiClient.GetEstadosAsync();
                cmbNuevosEstados.DataSource = estados;
                cmbNuevosEstados.DisplayMember = "Nombre";
                cmbNuevosEstados.ValueMember = "Id";
                if (estados.Count > 0) cmbNuevosEstados.SelectedIndex = 0; // Seleccionar el primero
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar estados: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void btnAceptarEstado_Click(object sender, EventArgs e)
        {
            if (cmbNuevosEstados.SelectedItem is Estado estado)
            {
                EstadoSeleccionado = estado; // Guardamos el estado elegido
                this.DialogResult = DialogResult.OK; // Marcamos éxito
                this.Close();
            }
        }

        private void btnCancelarEstado_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; // Marcamos cancelación
            this.Close();
        }
    }
}