using LoftComputacion.Domain; // Importa las clases de nuestro modelo (Cliente, OrdenDeServicio, etc.)
using System;
using System.ComponentModel; // Necesario para BindingList
using System.Threading.Tasks; // Necesario para operaciones asíncronas (async/await)
using System.Windows.Forms;

namespace LoftComputacion.WinForms
{
    /// <summary>
    /// Formulario principal de la aplicación. Muestra la lista de órdenes de servicio
    /// y permite acceder a otras funcionalidades.
    /// </summary>
    public partial class frmPrincipal : Form
    {
        // Instancia del cliente API para comunicarse con el backend.
        private readonly ApiClient _apiClient;

        private string _columnaOrdenActual = string.Empty;
        private ListSortDirection _direccionOrdenActual = ListSortDirection.Ascending;

        /// <summary>
        /// Constructor del formulario principal.
        /// </summary>
        public frmPrincipal()
        {
            InitializeComponent(); // Método autogenerado que crea y configura los controles visuales.
            _apiClient = new ApiClient(); // Crea una nueva instancia del cliente API.
        }

        /// <summary>
        /// Evento que se ejecuta cuando el formulario se carga por primera vez.
        /// Llama al método para cargar los datos iniciales en la grilla.
        /// </summary>
        private async void frmPrincipal_Load(object sender, EventArgs e)
        {
            await CargarOrdenesDeServicio(); // Carga inicial sin filtro.

            dgvHistorial.AutoGenerateColumns = false; // MUY IMPORTANTE: Desactivar autogeneración
            dgvHistorial.Columns.Clear();

            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colHistFecha",
                HeaderText = "Fecha y Hora",
                DataPropertyName = nameof(HistorialOrden.FechaHora), // Enlace directo
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yy HH:mm" },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells // Ajustar ancho al contenido
            });

            // Columna Descripción
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colHistDesc",
                HeaderText = "Descripción",
                DataPropertyName = nameof(HistorialOrden.DescripcionDelCambio), // Enlace directo
                DefaultCellStyle = new DataGridViewCellStyle { WrapMode = DataGridViewTriState.True }, // Ajuste de línea
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill // Que ocupe el espacio restante
            });

            // Columna Usuario (La dejamos comentada hasta que la API incluya el nombre)
            /* dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn() 
            { 
                Name = "colHistUsuario", 
                HeaderText = "Usuario", 
                DataPropertyName = "Usuario.NombreCompleto", // Propiedad anidada
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells 
            });
            */

            // Ajustamos la altura de las filas para el texto con ajuste de línea
            dgvHistorial.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            // --- FIN BLOQUE dgvHistorial ---
        }

        /// <summary>
        /// Carga (o recarga) la lista de órdenes de servicio desde la API
        /// y las muestra en el DataGridView principal (dgvOrdenes).
        /// </summary>
        /// <param name="filtro">Texto opcional para filtrar la búsqueda en la API.</param>
        private async Task CargarOrdenesDeServicio(string? filtro = null)
        {
            try
            {
                // 1. Obtiene los datos desde la API usando el filtro (si existe).
                var ordenes = await _apiClient.GetOrdenesDeServicioAsync(filtro);
                // 2. Convierte la lista a un BindingList para mejor enlace con el DataGridView.
                var bindingList = new BindingList<OrdenDeServicio>(ordenes);

                // --- Configuración del DataGridView ---
                dgvOrdenes.AutoGenerateColumns = false; // Desactivamos la creación automática de columnas.
                dgvOrdenes.Columns.Clear();           // Limpiamos columnas previas para evitar duplicados al recargar.

                // 3. Definimos manualmente CADA columna que queremos mostrar.
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colId", HeaderText = "N° Orden", DataPropertyName = nameof(OrdenDeServicio.Id) });
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colFechaIngreso", HeaderText = "Fecha Ingreso", DataPropertyName = nameof(OrdenDeServicio.FechaIngreso) });
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colClienteNombre", HeaderText = "Cliente" }); // Sin DataPropertyName, se llenará con CellFormatting
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colEquipoDesc", HeaderText = "Equipo" });     // Sin DataPropertyName, se llenará con CellFormatting
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colFalla", HeaderText = "Falla Declarada", DataPropertyName = nameof(OrdenDeServicio.FallaDeclaradaPorCliente) });
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colEstado", HeaderText = "Estado Actual" }); // Sin DataPropertyName, se llenará con CellFormatting
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colPresupuesto", HeaderText = "Presupuesto", DataPropertyName = nameof(OrdenDeServicio.PrecioPresupuestado), DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } });
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colPrecioFinal", HeaderText = "Precio Final", DataPropertyName = nameof(OrdenDeServicio.PrecioFinal), DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } });

                // 4. Hacemos que las columnas se ajusten al ancho de la grilla.
                dgvOrdenes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // 5. Asignamos la lista de datos a la grilla.
                dgvOrdenes.DataSource = bindingList;
            }
            catch (Exception ex)
            {
                // Si ocurre un error al cargar, mostramos un mensaje.
                MessageBox.Show($"Error al cargar las órdenes de servicio: {ex.Message}", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvOrdenes.DataSource = null; // Dejamos la grilla vacía en caso de error.
            }
        }

        /// <summary>
        /// Evento que se ejecuta al hacer clic en el ítem de menú "Nueva Orden".
        /// Abre el formulario frmGestionOrden en modo creación.
        /// </summary>
        private async void nuevaOrdenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmGestionOrden formNuevaOrden = new frmGestionOrden())
            {
                formNuevaOrden.ShowDialog(); // Muestra el formulario y espera a que se cierre.
            }
            // Después de cerrar, recarga la grilla por si se creó una nueva orden.
            await CargarOrdenesDeServicio();
        }

        /// <summary>
        /// Evento que se ejecuta al hacer doble clic en una celda de la grilla de órdenes.
        /// Abre el formulario frmGestionOrden en modo edición con los datos de la orden seleccionada.
        /// </summary>
        private async void dgvOrdenes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ignora clics en la cabecera.
            {
                // Obtiene el objeto OrdenDeServicio asociado a la fila clickeada.
                if (dgvOrdenes.Rows[e.RowIndex].DataBoundItem is OrdenDeServicio ordenSeleccionada)
                {
                    // Crea y muestra el formulario de gestión pasándole la orden.
                    using (var formGestion = new frmGestionOrden(ordenSeleccionada))
                    {
                        formGestion.ShowDialog();
                    }
                    // Después de cerrar, recarga la grilla por si hubo cambios.
                    await CargarOrdenesDeServicio();
                }
            }
        }

        /// <summary>
        /// Evento que se ejecuta al hacer clic en el botón "Buscar".
        /// Llama a CargarOrdenesDeServicio pasando el texto del cuadro de búsqueda.
        /// </summary>
        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            await CargarOrdenesDeServicio(txtBuscar.Text);
        }

        /// <summary>
        /// Evento que se ejecuta al presionar una tecla en el cuadro de búsqueda.
        /// Si la tecla es Enter, ejecuta la búsqueda.
        /// </summary>
        private async void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await CargarOrdenesDeServicio(txtBuscar.Text);
                e.SuppressKeyPress = true; // Evita el sonido "beep" de Windows.
            }
        }

        /// <summary>
        /// Evento que se dispara para cada celda ANTES de ser mostrada.
        /// Lo usamos para poner manualmente el valor en las columnas que muestran datos anidados.
        /// </summary>
        private void dgvOrdenes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Verificamos que sea una fila de datos válida.
            if (e.RowIndex >= 0 && e.RowIndex < dgvOrdenes.Rows.Count)
            {
                // Obtenemos el objeto OrdenDeServicio de la fila.
                if (dgvOrdenes.Rows[e.RowIndex].DataBoundItem is OrdenDeServicio orden)
                {
                    // Obtenemos el nombre interno de la columna actual.
                    string colName = dgvOrdenes.Columns[e.ColumnIndex].Name;

                    if (colName == "colFechaIngreso" && e.Value is DateTime fechaUtc)
                    {
                        // Convertimos la fecha UTC a la hora local de la PC
                        e.Value = fechaUtc.ToLocalTime().ToString("dd/MM/yyyy HH:mm"); // Formato deseado
                        e.FormattingApplied = true;
                    }
                    // Llenamos el valor según la columna.
                    if (colName == "colClienteNombre" && orden.Cliente != null)
                    {
                        e.Value = orden.Cliente.NombreCompleto;
                        e.FormattingApplied = true; // Avisamos que ya pusimos el valor.
                    }
                    else if (colName == "colEquipoDesc" && orden.Equipo != null)
                    {
                        // Mostramos solo el tipo, reemplazando el guion bajo
                        e.Value = orden.Equipo.Tipo.ToString().Replace("_", " ");
                        e.FormattingApplied = true;
                    }
                    else if (colName == "colEstado" && orden.Estado != null)
                    {
                        e.Value = orden.Estado.Nombre;
                        e.FormattingApplied = true;
                    }
                }
            }
        }

        /// <summary>
        /// Evento que se dispara cuando cambia la fila seleccionada en la grilla principal.
        /// Actualiza el panel de detalles con la información de la orden seleccionada.
        /// </summary>
        private void dgvOrdenes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvOrdenes.CurrentRow != null && dgvOrdenes.CurrentRow.DataBoundItem is OrdenDeServicio ordenSeleccionada)
            {
                // Llena los TextBoxes del panel de detalles.
                txtDetalleClienteNombre.Text = ordenSeleccionada.Cliente?.NombreCompleto ?? "N/A";
                txtDetalleEquipoDesc.Text = ordenSeleccionada.Equipo?.DescripcionCompleta ?? "N/A";
                txtDetalleEstadoActual.Text = ordenSeleccionada.Estado?.Nombre ?? "N/A";
                txtDetalleFalla.Text = ordenSeleccionada.FallaDeclaradaPorCliente;

                // Llena la grilla del historial.
                dgvHistorial.DataSource = null;
                if (ordenSeleccionada.Historial != null)
                {
                    var historialOrdenado = ordenSeleccionada.Historial.OrderByDescending(h => h.FechaHora).ToList();
                    dgvHistorial.DataSource = historialOrdenado;
                }
            }
            else
            {
                // Si no hay selección, limpia el panel.
                txtDetalleClienteNombre.Text = string.Empty;
                txtDetalleEquipoDesc.Text = string.Empty;
                txtDetalleEstadoActual.Text = string.Empty;
                txtDetalleFalla.Text = string.Empty;
                dgvHistorial.DataSource = null;
            }
        }
        /// <summary>
        /// Evento que se dispara para cada celda del DataGridView de Historial ANTES de ser mostrada.
        /// Lo usamos para formatear la fecha y asegurarnos de que el texto largo se ajuste.
        /// </summary>
        private void dgvHistorial_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Verificación robusta para evitar errores durante la carga/recarga
            if (e.RowIndex < 0 || e.RowIndex >= dgvHistorial.RowCount ||
                e.ColumnIndex < 0 || e.ColumnIndex >= dgvHistorial.ColumnCount)
            {
                return; // Salir si el índice está fuera de rango
            }

            if (dgvHistorial.Columns[e.ColumnIndex].DataPropertyName == nameof(HistorialOrden.FechaHora))
            {
                if (e.Value is DateTime fechaUtc) // El valor original es UTC
                {
                    // Convertimos a local y aplicamos formato
                    e.Value = fechaUtc.ToLocalTime().ToString("dd/MM/yy HH:mm");
                    e.FormattingApplied = true;
                }
            }

            // Solo formateamos si es una fila de datos válida y tiene un objeto asociado
            if (dgvHistorial.Rows[e.RowIndex].DataBoundItem is HistorialOrden historial)
            {
                // Formatear la columna de Fecha
                if (dgvHistorial.Columns[e.ColumnIndex].DataPropertyName == nameof(HistorialOrden.FechaHora))
                {
                    if (e.Value is DateTime fecha)
                    {
                        e.Value = fecha.ToString("dd/MM/yy HH:mm"); // Formato corto
                        e.FormattingApplied = true;
                    }
                }
                // Asegurar ajuste de línea en la columna de Descripción
                else if (dgvHistorial.Columns[e.ColumnIndex].DataPropertyName == nameof(HistorialOrden.DescripcionDelCambio))
                {
                    // Aplicamos el estilo de ajuste de línea directamente a la celda
                    e.CellStyle.WrapMode = DataGridViewTriState.True;
                    // Aseguramos que las filas ajusten su altura (redundante si está en Load, pero no daña)
                    dgvHistorial.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                }
                // (Aquí iría la lógica futura para mostrar el nombre del Usuario)
            }
        }

        private async void tsmiCambiarEstado_Click(object sender, EventArgs e)
        {
            // 1. Verificar selección en la grilla principal
            if (dgvOrdenes.CurrentRow != null && dgvOrdenes.CurrentRow.DataBoundItem is OrdenDeServicio ordenParaCambiar)
            {
                // 2. Abrir form para seleccionar nuevo estado
                Estado? nuevoEstadoSeleccionado = null;
                using (var formSeleccionar = new frmSeleccionarEstado())
                {
                    if (formSeleccionar.ShowDialog() == DialogResult.OK)
                    {
                        nuevoEstadoSeleccionado = formSeleccionar.EstadoSeleccionado;
                    }
                    else
                    {
                        return; // Canceló selección de estado
                    }
                }

                // 3. Abrir form para confirmar usuario
                Usuario? usuarioConfirmado = null;
                using (var formConfirmacion = new frmConfirmarCambio())
                {
                    if (formConfirmacion.ShowDialog() == DialogResult.OK)
                    {
                        usuarioConfirmado = formConfirmacion.UsuarioSeleccionado;
                    }
                    else
                    {
                        MessageBox.Show("Cambio de estado cancelado.", "Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return; // Canceló confirmación
                    }
                }

                // 4. Actualizar la orden vía API
                try
                {
                    // Creamos un objeto temporal solo con los cambios
                    var ordenActualizada = new OrdenDeServicio
                    {
                        Id = ordenParaCambiar.Id,
                        EstadoId = nuevoEstadoSeleccionado.Id,
                        // Mantenemos precios existentes (importante!)
                        PrecioPresupuestado = ordenParaCambiar.PrecioPresupuestado,
                        PrecioFinal = ordenParaCambiar.PrecioFinal
                    };

                    await _apiClient.UpdateOrdenDeServicioAsync(ordenParaCambiar.Id, ordenActualizada, usuarioConfirmado.Id);
                    MessageBox.Show("¡Estado actualizado con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 5. Recargar la grilla
                    await CargarOrdenesDeServicio();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al actualizar el estado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una orden de la lista.", "Selección Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tsmiAdministracion_Click(object sender, EventArgs e)
        {
            // 1. Pedimos la contraseña
            using (var formPassword = new frmPasswordPrompt())
            {
                // Si la contraseña es correcta (DialogResult.OK)...
                if (formPassword.ShowDialog() == DialogResult.OK)
                {
                    // ...abrimos el formulario de ganancias
                    using (var formGanancias = new frmGanancias())
                    {
                        formGanancias.ShowDialog();
                    }
                }
                // Si cancela o la contraseña es incorrecta, no hacemos nada.
            }
        }

        private void dgvOrdenes_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Obtenemos la columna en la que se hizo clic
            var columnaClickeada = dgvOrdenes.Columns[e.ColumnIndex];
            // Obtenemos el nombre de la propiedad a la que está enlazada (o el Name si es manual)
            string nombrePropiedad = columnaClickeada.DataPropertyName ?? columnaClickeada.Name;

            // Si no tiene nombre de propiedad (raro) o es una columna que no queremos ordenar, salimos
            if (string.IsNullOrEmpty(nombrePropiedad)) return;

            // Obtenemos la lista actual de órdenes desde el DataSource
            if (dgvOrdenes.DataSource is BindingList<OrdenDeServicio> listaOrdenes)
            {
                // Determinamos la nueva dirección de ordenamiento
                ListSortDirection nuevaDireccion;
                if (_columnaOrdenActual == nombrePropiedad)
                {
                    // Si se hace clic en la misma columna, invertimos la dirección
                    nuevaDireccion = (_direccionOrdenActual == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
                }
                else
                {
                    // Si es una columna nueva, empezamos ascendente
                    nuevaDireccion = ListSortDirection.Ascending;
                }

                // --- LÓGICA DE ORDENAMIENTO CORREGIDA ---
                IEnumerable<OrdenDeServicio> ordenesOrdenadas;

                // Determinamos cómo ordenar según el NOMBRE de la columna clickeada
                switch (columnaClickeada.Name) // <-- CAMBIO AQUÍ: Usamos .Name
                {
                    case "colClienteNombre": // <-- Usamos el Name que definimos
                        ordenesOrdenadas = (nuevaDireccion == ListSortDirection.Ascending)
                            ? listaOrdenes.OrderBy(o => o.Cliente?.NombreCompleto)
                            : listaOrdenes.OrderByDescending(o => o.Cliente?.NombreCompleto);
                        break;
                    case "colEquipoDesc": // <-- Usamos el Name que definimos
                                          // Ordenamos por Tipo, como configuramos en CellFormatting
                        ordenesOrdenadas = (nuevaDireccion == ListSortDirection.Ascending)
                           ? listaOrdenes.OrderBy(o => o.Equipo?.Tipo)
                           : listaOrdenes.OrderByDescending(o => o.Equipo?.Tipo);
                        break;
                    case "colEstado": // <-- Usamos el Name que definimos
                        ordenesOrdenadas = (nuevaDireccion == ListSortDirection.Ascending)
                            ? listaOrdenes.OrderBy(o => o.Estado?.Nombre)
                            : listaOrdenes.OrderByDescending(o => o.Estado?.Nombre);
                        break;
                    default: // Ordenar por propiedades directas (usando DataPropertyName)
                        if (!string.IsNullOrEmpty(nombrePropiedad)) // Asegurarnos de que SÍ tenga DataPropertyName
                        {
                            var propInfo = typeof(OrdenDeServicio).GetProperty(nombrePropiedad);
                            if (propInfo != null)
                            {
                                ordenesOrdenadas = (nuevaDireccion == ListSortDirection.Ascending)
                                  ? listaOrdenes.OrderBy(o => propInfo.GetValue(o))
                                  : listaOrdenes.OrderByDescending(o => propInfo.GetValue(o));
                            }
                            else { ordenesOrdenadas = listaOrdenes; }
                        }
                        else { ordenesOrdenadas = listaOrdenes; }
                        break;
                }
                // --- FIN LÓGICA CORREGIDA ---


                // Actualizamos el DataSource con la lista ordenada
                // Convertimos de nuevo a BindingList para mantener el enlace
                dgvOrdenes.DataSource = new BindingList<OrdenDeServicio>(ordenesOrdenadas.ToList());

                // Guardamos la columna y dirección actual para la próxima vez
                _columnaOrdenActual = nombrePropiedad;
                _direccionOrdenActual = nuevaDireccion;

                // Opcional: Podríamos añadir un glifo (flechita ??) a la cabecera
                // dgvOrdenes.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = ...
            }
        }
    }
}