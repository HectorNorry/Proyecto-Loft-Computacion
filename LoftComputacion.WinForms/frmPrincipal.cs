using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using LoftComputacion.Domain; // Asegúrate de que este using esté

namespace LoftComputacion.WinForms
{
    // 1. Esta línea es CRUCIAL. Le dice a C# que esto es un formulario.
    public partial class frmPrincipal : Form
    {
        // 2. Campo privado para guardar nuestro ApiClient
        private readonly ApiClient _apiClient;

        // 3. Este es el constructor. Se ejecuta cuando el formulario se crea.
        public frmPrincipal()
        {
            InitializeComponent(); // Esta es la "llave de encendido" que arma el diseño.
            _apiClient = new ApiClient(); // Creamos nuestra instancia del ApiClient.
        }

        // 4. Este es el evento que se dispara cuando el formulario termina de cargar.
        private async void frmPrincipal_Load(object sender, EventArgs e)
        {
            await CargarOrdenesDeServicio();
        }

        // 5. Este es nuestro método para ir a buscar los datos a la API.
        private async Task CargarOrdenesDeServicio()
        {
            try
            {
                var ordenes = await _apiClient.GetOrdenesDeServicioAsync();
                dgvOrdenes.DataSource = ordenes;

                // --- AÑADE ESTA SECCIÓN PARA CONFIGURAR LAS COLUMNAS ---
                if (dgvOrdenes.Columns.Count > 0)
                {
                    // Ocultamos las columnas que no nos interesan
                    dgvOrdenes.Columns["ClienteId"].Visible = false;
                    dgvOrdenes.Columns["EstadoId"].Visible = false;
                    dgvOrdenes.Columns["EquipoId"].Visible = false;
                    dgvOrdenes.Columns["MetodoDePagoId"].Visible = false;

                    // Ocultamos los objetos complejos
                    dgvOrdenes.Columns["Cliente"].Visible = false;
                    dgvOrdenes.Columns["Estado"].Visible = false;
                    dgvOrdenes.Columns["Equipo"].Visible = false;
                    dgvOrdenes.Columns["MetodoDePago"].Visible = false;
                    dgvOrdenes.Columns["Fotos"].Visible = false;
                    dgvOrdenes.Columns["Historial"].Visible = false;

                    // Renombramos las cabeceras para que sean más legibles
                    dgvOrdenes.Columns["Id"].HeaderText = "N° Orden";
                    dgvOrdenes.Columns["FechaIngreso"].HeaderText = "Fecha de Ingreso";
                    dgvOrdenes.Columns["FallaDeclaradaPorCliente"].HeaderText = "Falla Declarada";
                    dgvOrdenes.Columns["PrecioPresupuestado"].HeaderText = "Presupuesto";
                    dgvOrdenes.Columns["PrecioFinal"].HeaderText = "Precio Final";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las órdenes de servicio: {ex.Message}", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}