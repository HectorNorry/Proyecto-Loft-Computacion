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
                    //dgvOrdenes.Columns["Estado"].Visible = false;
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

                    // Le decimos a la columna "Estado" que muestre la propiedad "Nombre" del objeto Estado
                    dgvOrdenes.Columns["Estado"].DataPropertyName = "Nombre";
                    dgvOrdenes.Columns["Estado"].HeaderText = "Estado Actual"; // Renombramos la cabecera
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las órdenes de servicio: {ex.Message}", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void nuevaOrdenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 1. Creamos una nueva 'instancia' o 'copia' del formulario de gestión.
            frmGestionOrden formNuevaOrden = new frmGestionOrden();

            // 2. Lo mostramos como un diálogo. Esto significa que la ventana principal
            //    quedará bloqueada hasta que cerremos la ventana de "Nueva Orden".
            formNuevaOrden.ShowDialog();

            // 3. Cuando la ventana de nueva orden se cierre, este código se ejecutará.
            //    Recargamos la grilla para que, si creamos una nueva orden, aparezca al instante.
            await CargarOrdenesDeServicio();
        }

        private async void dgvOrdenes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 1. Nos aseguramos de que el doble clic fue sobre una fila válida (no la cabecera)
            if (e.RowIndex >= 0)
            {
                // 2. Obtenemos el objeto OrdenDeServicio completo de la fila seleccionada.
                //    DataGridView guarda el objeto original en DataBoundItem.
                if (dgvOrdenes.Rows[e.RowIndex].DataBoundItem is OrdenDeServicio ordenSeleccionada)
                {
                    // 3. Creamos el formulario de gestión, pasándole la orden seleccionada
                    //    al NUEVO constructor que creamos.
                    using (var formGestion = new frmGestionOrden(ordenSeleccionada))
                    {
                        // 4. Lo mostramos como diálogo
                        formGestion.ShowDialog();

                        // 5. Cuando se cierre el formulario de edición, recargamos la grilla
                        //    por si se hicieron cambios (ej: cambio de estado).
                        await CargarOrdenesDeServicio();
                    }
                }
            }
        }
    }
}