using LoftComputacion.Domain; // Asegúrate de que este using esté
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            await CargarOrdenesDeServicio(); // Sin filtro al inicio
        }

        // 5. Este es nuestro método para ir a buscar los datos a la API.
        private async Task CargarOrdenesDeServicio(string? filtro = null)
        {
            try
            {
                var ordenes = await _apiClient.GetOrdenesDeServicioAsync(filtro);
                var bindingList = new System.ComponentModel.BindingList<OrdenDeServicio>(ordenes);

                dgvOrdenes.AutoGenerateColumns = false;
                dgvOrdenes.Columns.Clear();

                // Definimos las columnas manualmente OTRA VEZ, pero SIN DataPropertyName para las anidadas
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colId", HeaderText = "N° Orden", DataPropertyName = nameof(OrdenDeServicio.Id) });
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colFechaIngreso", HeaderText = "Fecha Ingreso", DataPropertyName = nameof(OrdenDeServicio.FechaIngreso) });

                // Columnas anidadas: SIN DataPropertyName por ahora
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colClienteNombre", HeaderText = "Cliente" });
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colEquipoDesc", HeaderText = "Equipo" });

                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colFalla", HeaderText = "Falla Declarada", DataPropertyName = nameof(OrdenDeServicio.FallaDeclaradaPorCliente) });

                // Columna Estado: SIN DataPropertyName por ahora
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colEstado", HeaderText = "Estado Actual" });

                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colPresupuesto", HeaderText = "Presupuesto", DataPropertyName = nameof(OrdenDeServicio.PrecioPresupuestado), DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } });
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colPrecioFinal", HeaderText = "Precio Final", DataPropertyName = nameof(OrdenDeServicio.PrecioFinal), DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } });

                dgvOrdenes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvOrdenes.DataSource = bindingList; // Asignamos datos al final
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las órdenes de servicio: {ex.Message}", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvOrdenes.DataSource = null;
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

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            // Tomamos el texto del TextBox y llamamos a CargarOrdenes con ese filtro
            await CargarOrdenesDeServicio(txtBuscar.Text);
        }

        private async void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            // Si el usuario presiona la tecla Enter...
            if (e.KeyCode == Keys.Enter)
            {
                // ...ejecutamos la búsqueda.
                await CargarOrdenesDeServicio(txtBuscar.Text);
                e.SuppressKeyPress = true; // Evita el "ding" de Windows al presionar Enter
            }
        }

        private void dgvOrdenes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Verificamos que estemos en una fila de datos válida
            if (e.RowIndex >= 0 && e.RowIndex < dgvOrdenes.Rows.Count)
            {
                // Obtenemos el objeto OrdenDeServicio completo de la fila actual
                if (dgvOrdenes.Rows[e.RowIndex].DataBoundItem is OrdenDeServicio orden)
                {
                    // Verificamos el nombre de la columna que se está formateando
                    string colName = dgvOrdenes.Columns[e.ColumnIndex].Name;

                    // Si es la columna del Cliente...
                    if (colName == "colClienteNombre" && orden.Cliente != null)
                    {
                        e.Value = orden.Cliente.NombreCompleto; // Ponemos el nombre del cliente
                        e.FormattingApplied = true; // Indicamos que ya formateamos esta celda
                    }
                    // Si es la columna del Equipo...
                    else if (colName == "colEquipoDesc" && orden.Equipo != null)
                    {
                        e.Value = orden.Equipo.DescripcionCompleta; // Ponemos la descripción
                        e.FormattingApplied = true;
                    }
                    // Si es la columna del Estado...
                    else if (colName == "colEstado" && orden.Estado != null)
                    {
                        e.Value = orden.Estado.Nombre; // Ponemos el nombre del estado
                        e.FormattingApplied = true;
                    }
                }
            }
        }
    }
}