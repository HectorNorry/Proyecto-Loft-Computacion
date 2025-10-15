using LoftComputacion.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LoftComputacion.WinForms
{
    public partial class frmBuscarCliente : Form
    {
        private readonly ApiClient _apiClient;
        private List<Cliente> _listaCompletaClientes;

        public Cliente ClienteSeleccionado { get; private set; }

        public frmBuscarCliente()
        {
            InitializeComponent();
            _apiClient = new ApiClient();
            _listaCompletaClientes = new List<Cliente>();
        }


        private async void frmBuscarCliente_Load(object sender, EventArgs e)
        {
            try
            {
                _listaCompletaClientes = await _apiClient.GetClientesAsync();
                dgvClientes.DataSource = _listaCompletaClientes;

                // --- AÑADÍ ESTE BLOQUE DE CÓDIGO ---

                // 1. Hacemos que las columnas se ajusten para rellenar el espacio.
                dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // 2. Renombramos las cabeceras para que sean más claras.
                dgvClientes.Columns["Id"].HeaderText = "ID";
                dgvClientes.Columns["NombreCompleto"].HeaderText = "Nombre Completo";
                dgvClientes.Columns["Telefono"].HeaderText = "Teléfono";
                dgvClientes.Columns["Email"].HeaderText = "Email";
                dgvClientes.Columns["DNI"].HeaderText = "DNI";

                // --- FIN DEL BLOQUE A AÑADIR ---
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtBusquedaCliente_TextChanged(object sender, EventArgs e)
        {
            var filtro = txtBusquedaCliente.Text.ToLower();
            var clientesFiltrados = _listaCompletaClientes.Where(c =>
                c.NombreCompleto.ToLower().Contains(filtro) ||
                (c.DNI != null && c.DNI.Contains(filtro))
            ).ToList();
            dgvClientes.DataSource = clientesFiltrados;
        }

        private void SeleccionarClienteYSalir()
        {
            if (dgvClientes.CurrentRow != null)
            {
                ClienteSeleccionado = dgvClientes.CurrentRow.DataBoundItem as Cliente;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            SeleccionarClienteYSalir();
        }

        private void dgvClientes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SeleccionarClienteYSalir();
            }
        }
    }
}