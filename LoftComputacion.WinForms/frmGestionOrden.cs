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
    public partial class frmGestionOrden : Form
    {
        private readonly ApiClient _apiClient;

        public frmGestionOrden()
        {
            InitializeComponent();
            _apiClient = new ApiClient();
        }

        private void frmGestionOrden_Load(object sender, EventArgs e)
        {
            // Limpiamos el ComboBox por si acaso
            cmbTipoEquipo.Items.Clear();

            // Agregamos los tipos de equipo que definimos en nuestra lógica
            cmbTipoEquipo.Items.Add("Notebook");
            cmbTipoEquipo.Items.Add("PC de Escritorio");
            cmbTipoEquipo.Items.Add("Impresora");

            // Opcional: Hacemos que la primera opción aparezca seleccionada por defecto
            cmbTipoEquipo.SelectedIndex = 0;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close(); // Cierra el formulario actual
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Crear el nuevo Cliente
                var nuevoCliente = new Cliente
                {
                    NombreCompleto = txtNombreCliente.Text,
                    Telefono = txtTelefonoCliente.Text,
                    Email = txtEmailCliente.Text
                };
                var clienteCreado = await _apiClient.CreateClienteAsync(nuevoCliente);

                // 2. Crear el nuevo Equipo
                var nuevoEquipo = new Equipo
                {
                    // Convertimos el texto del ComboBox al tipo de dato correcto (enum)
                    Tipo = (TipoDeEquipo)cmbTipoEquipo.SelectedIndex,
                    Marca = txtMarca.Text,
                    Modelo = txtModelo.Text,
                    NumeroDeSerie = txtNumeroSerie.Text,
                    Componentes = txtComponentes.Text
                };
                var equipoCreado = await _apiClient.CreateEquipoAsync(nuevoEquipo);

                // 3. Crear la nueva Orden de Servicio usando los IDs de los objetos recién creados
                var nuevaOrden = new OrdenDeServicio
                {
                    ClienteId = clienteCreado.Id,
                    EquipoId = equipoCreado.Id,
                    FallaDeclaradaPorCliente = txtFallaDeclarada.Text
                };
                await _apiClient.CreateOrdenDeServicioAsync(nuevaOrden);

                // 4. Si todo salió bien, mostramos un mensaje de éxito y cerramos el formulario
                MessageBox.Show("¡Orden de servicio creada con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                // Si algo falla, mostramos el error
                MessageBox.Show($"Ocurrió un error al guardar la orden: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
