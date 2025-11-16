using LoftComputacion.Domain;
using System;
using System.ComponentModel;
using System.Data; // Agregado para DataGridViewCellFormattingEventArgs si es necesario
using System.Drawing; // Agregado para Font, etc. si es necesario
using System.Linq; // Agregado para OrderByDescending
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection; // Agregado para GetProperty
using MaterialSkin;
using MaterialSkin.Controls;

namespace LoftComputacion.WinForms
{
    /// <summary>
    /// Formulario principal de la aplicación. Muestra la lista de órdenes de servicio
    /// y permite acceder a otras funcionalidades.
    /// </summary>
    public partial class frmPrincipal : MaterialForm
    {
        #region Propiedades Privadas

        // Instancia única del cliente API, inyectada desde el login.
        private readonly ApiClient _apiClient;

        // Variables para gestionar el ordenamiento de la grilla principal.
        private string _columnaOrdenActual = string.Empty;
        private ListSortDirection _direccionOrdenActual = ListSortDirection.Ascending;
        private bool _estaCargandoGrilla = false; // Bandera para evitar eventos reentrantes

        #endregion

        #region Constructor y Carga

        /// <summary>
        /// Constructor del formulario principal. Recibe el ApiClient ya autenticado.
        /// </summary>
        /// <param name="apiClient">La instancia del ApiClient que ya contiene el Token JWT.</param>
        public frmPrincipal(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient; // Asigna el cliente que viene del login.

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue600,    // COLOR PRINCIPAL
                Primary.Blue700,    // COLOR OSCURO
                Primary.Blue200,    // COLOR CLARO
                Accent.LightBlue200,// COLOR DE ACENTO
                TextShade.WHITE     // COLOR DEL TEXTO
            );
        }

        /// <summary>
        /// Evento que se ejecuta cuando el formulario se carga por primera vez.
        /// Configura las grillas y carga los datos iniciales.
        /// </summary>
        private async void frmPrincipal_Load(object sender, EventArgs e)

        {
            
            await CargarOrdenesDeServicio(); // Carga la grilla principal

            // --- Configuración Inicial del dgvHistorial (se hace una sola vez) ---
            dgvHistorial.AutoGenerateColumns = false;
            dgvHistorial.Columns.Clear();

            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colHistFecha",
                HeaderText = "Fecha y Hora",
                DataPropertyName = nameof(HistorialOrden.FechaHora),
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yy HH:mm" },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            });

            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colHistDesc",
                HeaderText = "Descripción",
                DataPropertyName = nameof(HistorialOrden.DescripcionDelCambio),
                DefaultCellStyle = new DataGridViewCellStyle { WrapMode = DataGridViewTriState.True },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // --- ¡AQUÍ AGREGAMOS LA COLUMNA PARA EL USUARIO! ---
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colHistUsuario",
                HeaderText = "Realizado Por",
                // Usamos la propiedad anidada. La API debe incluir el Usuario en el Historial.
                DataPropertyName = "Usuario.NombreCompleto",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            });
            // --- FIN COLUMNA USUARIO ---

            dgvHistorial.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        #endregion



        #region Carga y Refresco de Datos

        /// <summary>
        /// Carga (o recarga) la lista de órdenes de servicio desde la API
        /// y las muestra en el DataGridView principal (dgvOrdenes).
        /// </summary>
        /// <param name="filtro">Texto opcional para filtrar la búsqueda en la API.</param>
        private async Task CargarOrdenesDeServicio(string? filtro = null)
        {
            _estaCargandoGrilla = true; // Levantamos la bandera para evitar errores
            try
            {
                var ordenes = await _apiClient.GetOrdenesDeServicioAsync(filtro);
                var bindingList = new BindingList<OrdenDeServicio>(ordenes);

                dgvOrdenes.AutoGenerateColumns = false;
                dgvOrdenes.Columns.Clear();

                // Definimos manualmente las columnas
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colId", HeaderText = "N° Orden", DataPropertyName = nameof(OrdenDeServicio.Id) });
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colFechaIngreso", HeaderText = "Fecha Ingreso", DataPropertyName = nameof(OrdenDeServicio.FechaIngreso) });
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colClienteNombre", HeaderText = "Cliente" });
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colEquipoDesc", HeaderText = "Equipo" });
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colFalla", HeaderText = "Falla Declarada", DataPropertyName = nameof(OrdenDeServicio.FallaDeclaradaPorCliente) });
                dgvOrdenes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colEstado", HeaderText = "Estado Actual" });

                // Ocultamos las columnas de precio en la grilla principal
                var colPresupuesto = new DataGridViewTextBoxColumn() { Name = "colPresupuesto", HeaderText = "Presupuesto", DataPropertyName = nameof(OrdenDeServicio.PrecioPresupuestado), DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }, Visible = false };
                var colPrecioFinal = new DataGridViewTextBoxColumn() { Name = "colPrecioFinal", HeaderText = "Precio Final", DataPropertyName = nameof(OrdenDeServicio.PrecioFinal), DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }, Visible = false };
                dgvOrdenes.Columns.Add(colPresupuesto);
                dgvOrdenes.Columns.Add(colPrecioFinal);

                dgvOrdenes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvOrdenes.DataSource = bindingList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las órdenes de servicio: {ex.Message}", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvOrdenes.DataSource = null;
            }
            finally
            {
                _estaCargandoGrilla = false; // Bajamos la bandera
            }
        }

        #endregion

        #region Eventos de Interfaz (Clicks, Selección, etc.)

        /// <summary>
        /// Abre el formulario de Nueva Orden.
        /// </summary>
        private async void nuevaOrdenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmGestionOrden formNuevaOrden = new frmGestionOrden(_apiClient))
            {
                formNuevaOrden.ShowDialog();
            }
            await CargarOrdenesDeServicio(); // Recarga la grilla al cerrar
        }

        /// <summary>
        /// Abre el formulario de Edición de Orden.
        /// </summary>
        private async void dgvOrdenes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dgvOrdenes.Rows[e.RowIndex].DataBoundItem is OrdenDeServicio ordenSeleccionada)
                {
                    using (var formGestion = new frmGestionOrden(ordenSeleccionada, _apiClient))
                    {
                        formGestion.ShowDialog();
                    }
                    await CargarOrdenesDeServicio(); // Recarga la grilla al cerrar
                }
            }
        }

        /// <summary>
        /// Ejecuta la búsqueda al hacer clic en el botón "Buscar".
        /// </summary>
        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            await CargarOrdenesDeServicio(txtBuscar.Text);
        }

        /// <summary>
        /// Ejecuta la búsqueda al presionar "Enter" en el TextBox de búsqueda.
        /// </summary>
        private async void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await CargarOrdenesDeServicio(txtBuscar.Text);
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Abre el formulario de Ganancias (previa validación de admin).
        /// </summary>
        private void tsmiAdministracion_Click(object sender, EventArgs e)
        {
            using (var formPassword = new frmPasswordPrompt(_apiClient))
            {
                if (formPassword.ShowDialog() == DialogResult.OK)
                {
                    using (var formGanancias = new frmGanancias(_apiClient))
                    {
                        formGanancias.ShowDialog();
                    }
                }
            }
        }

        /// <summary>
        /// Actualiza el panel de detalles cuando el usuario selecciona una fila en la grilla principal.
        /// </summary>
        // --- Reemplaza tu método existente por este ---

        /// <summary>
        /// Actualiza el panel de detalles CUANDO EL USUARIO SELECCIONA UNA FILA en la grilla principal.
        /// AHORA TAMBIÉN CARGA EL HISTORIAL ASÍNCRONAMENTE.
        /// </summary>
        private async void dgvOrdenes_SelectionChanged(object sender, EventArgs e)
        {
            if (_estaCargandoGrilla) return; // Evita error reentrante

            if (dgvOrdenes.CurrentRow != null && dgvOrdenes.CurrentRow.DataBoundItem is OrdenDeServicio ordenSeleccionada)
            {
                // 1. Llena los TextBoxes del panel de detalles (esto estaba bien)
                txtDetalleClienteNombre.Text = ordenSeleccionada.Cliente?.NombreCompleto ?? "N/A";
                txtDetalleEquipoDesc.Text = ordenSeleccionada.Equipo?.DescripcionCompleta ?? "N/A";
                txtDetalleEstadoActual.Text = ordenSeleccionada.Estado?.Nombre ?? "N/A";
                txtDetalleFalla.Text = ordenSeleccionada.FallaDeclaradaPorCliente;

                // --- 2. ¡LÓGICA CORREGIDA PARA EL HISTORIAL! ---
                dgvHistorial.DataSource = null; // Limpiamos la grilla

                try
                {
                    // Hacemos una nueva llamada a la API para traer el historial
                    // (Asegúrate de que tu ApiClient tenga este método)
                    var historial = await _apiClient.GetHistorialDeOrdenAsync(ordenSeleccionada.Id);

                    if (historial != null)
                    {
                        var historialOrdenado = historial.OrderByDescending(h => h.FechaHora).ToList();
                        dgvHistorial.DataSource = historialOrdenado;
                    }
                }
                catch (Exception ex)
                {
                    // Manejamos el error si la API falla
                    MessageBox.Show($"No se pudo cargar el historial: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Limpia el panel si no hay selección (esto estaba bien)
                txtDetalleClienteNombre.Text = string.Empty;
                txtDetalleEquipoDesc.Text = string.Empty;
                txtDetalleEstadoActual.Text = string.Empty;
                txtDetalleFalla.Text = string.Empty;
                dgvHistorial.DataSource = null;
            }
        }

        #endregion

        #region Eventos de Formateo y Ordenamiento de Grillas

        /// <summary>
        /// Formatea manualmente las celdas de la grilla principal (dgvOrdenes)
        /// para mostrar datos anidados (Cliente, Equipo, Estado) y fechas.
        /// </summary>
        private void dgvOrdenes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvOrdenes.Rows.Count)
            {
                if (dgvOrdenes.Rows[e.RowIndex].DataBoundItem is OrdenDeServicio orden)
                {
                    string colName = dgvOrdenes.Columns[e.ColumnIndex].Name;

                    if (colName == "colFechaIngreso" && e.Value is DateTime fechaUtc)
                    {
                        e.Value = fechaUtc.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
                        e.FormattingApplied = true;
                    }
                    else if (colName == "colClienteNombre" && orden.Cliente != null)
                    {
                        e.Value = orden.Cliente.NombreCompleto;
                        e.FormattingApplied = true;
                    }
                    else if (colName == "colEquipoDesc" && orden.Equipo != null)
                    {
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
        /// Formatea manualmente las celdas de la grilla de historial (dgvHistorial).
        /// (Este código está corregido y simplificado).
        /// </summary>
        /// <summary>
        /// Evento que se dispara para cada celda del DataGridView de Historial ANTES de ser mostrada.
        /// Lo usamos para formatear la fecha (a hora local) y mostrar el nombre del usuario.
        /// </summary>
        private void dgvHistorial_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Verificación robusta para evitar errores durante la carga/recarga
            if (e.RowIndex < 0 || e.RowIndex >= dgvHistorial.RowCount ||
                e.ColumnIndex < 0 || e.ColumnIndex >= dgvHistorial.ColumnCount)
            {
                return; // Salir si el índice está fuera de rango
            }

            // Solo formateamos si es una fila de datos válida y tiene un objeto asociado
            if (dgvHistorial.Rows[e.RowIndex].DataBoundItem is HistorialOrden historial)
            {
                // Obtenemos el NOMBRE de la columna (el que pusimos en el Load, ej: "colHistFecha")
                string colName = dgvHistorial.Columns[e.ColumnIndex].Name;

                switch (colName)
                {
                    // CASO 1: Formatear la columna de Fecha y Hora
                    case "colHistFecha":
                        if (e.Value is DateTime fechaUtc) // El valor original es UTC
                        {
                            // Convertimos a local y aplicamos formato
                            e.Value = fechaUtc.ToLocalTime().ToString("dd/MM/yy HH:mm");
                            e.FormattingApplied = true;
                        }
                        break;

                    // CASO 2: Formatear la columna de Descripción (asegurar ajuste de línea)
                    case "colHistDesc":
                        e.CellStyle.WrapMode = DataGridViewTriState.True;
                        dgvHistorial.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                        break;

                    // CASO 3: Poner el nombre del Usuario (la lógica que faltaba)
                    case "colHistUsuario":
                        if (historial.Usuario != null)
                        {
                            e.Value = historial.Usuario.NombreCompleto;
                            e.FormattingApplied = true;
                        }
                        else
                        {
                            // Esto pasa si la API no incluyó el .ThenInclude(h => h.Usuario)
                            e.Value = $"(ID: {historial.UsuarioId})";
                            e.FormattingApplied = true;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Maneja el clic en la cabecera de la grilla principal para ordenar las columnas.
        /// </summary>
        private void dgvOrdenes_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var columnaClickeada = dgvOrdenes.Columns[e.ColumnIndex];
            string nombreColumnaActual = columnaClickeada.Name; // Usamos Name

            if (dgvOrdenes.DataSource is BindingList<OrdenDeServicio> listaOrdenes)
            {
                ListSortDirection nuevaDireccion;
                if (_columnaOrdenActual == nombreColumnaActual)
                {
                    nuevaDireccion = (_direccionOrdenActual == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
                }
                else
                {
                    nuevaDireccion = ListSortDirection.Ascending;
                }

                IEnumerable<OrdenDeServicio> ordenesOrdenadas;

                switch (nombreColumnaActual)
                {
                    case "colClienteNombre":
                        ordenesOrdenadas = (nuevaDireccion == ListSortDirection.Ascending)
                            ? listaOrdenes.OrderBy(o => o.Cliente?.NombreCompleto)
                            : listaOrdenes.OrderByDescending(o => o.Cliente?.NombreCompleto);
                        break;
                    case "colEquipoDesc":
                        ordenesOrdenadas = (nuevaDireccion == ListSortDirection.Ascending)
                           ? listaOrdenes.OrderBy(o => o.Equipo?.Tipo)
                           : listaOrdenes.OrderByDescending(o => o.Equipo?.Tipo);
                        break;
                    case "colEstado":
                        ordenesOrdenadas = (nuevaDireccion == ListSortDirection.Ascending)
                            ? listaOrdenes.OrderBy(o => o.Estado?.Nombre)
                            : listaOrdenes.OrderByDescending(o => o.Estado?.Nombre);
                        break;
                    default:
                        string nombrePropiedad = columnaClickeada.DataPropertyName;
                        if (!string.IsNullOrEmpty(nombrePropiedad))
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

                dgvOrdenes.DataSource = new BindingList<OrdenDeServicio>(ordenesOrdenadas.ToList());

                _columnaOrdenActual = nombreColumnaActual;
                _direccionOrdenActual = nuevaDireccion;

                // Limpiamos glifos (flechas) anteriores y ponemos el nuevo
                foreach (DataGridViewColumn col in dgvOrdenes.Columns) { col.HeaderCell.SortGlyphDirection = SortOrder.None; }
                dgvOrdenes.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = (nuevaDireccion == ListSortDirection.Ascending) ? SortOrder.Ascending : SortOrder.Descending;
            }
        }

        #endregion

        #region Menú Contextual (Clic Derecho)

        /// <summary>
        /// Maneja el clic en la opción "Cambiar Estado" del menú contextual.
        /// Inicia el flujo de selección de estado y confirmación de usuario.
        /// </summary>
        private async void tsmiCambiarEstado_Click(object sender, EventArgs e)
        {
            if (dgvOrdenes.CurrentRow != null && dgvOrdenes.CurrentRow.DataBoundItem is OrdenDeServicio ordenParaCambiar)
            {
                // 1. Abrir form para seleccionar nuevo estado
                Estado? nuevoEstadoSeleccionado = null;
                using (var formSeleccionar = new frmSeleccionarEstado())
                {
                    if (formSeleccionar.ShowDialog() == DialogResult.OK)
                    {
                        nuevoEstadoSeleccionado = formSeleccionar.EstadoSeleccionado;
                    }
                    else { return; } // Canceló
                }

                // 2. Abrir form para confirmar usuario
                Usuario? usuarioConfirmado = null;
                using (var formConfirmacion = new frmConfirmarCambio(_apiClient))
                {
                    if (formConfirmacion.ShowDialog() == DialogResult.OK)
                    {
                        usuarioConfirmado = formConfirmacion.UsuarioSeleccionado;
                    }
                    else { return; } // Canceló
                }

                // 3. Actualizar la orden vía API
                try
                {
                    var ordenActualizada = new OrdenDeServicio
                    {
                        Id = ordenParaCambiar.Id,
                        EstadoId = nuevoEstadoSeleccionado.Id,
                        PrecioPresupuestado = ordenParaCambiar.PrecioPresupuestado, // Mantenemos precios
                        PrecioFinal = ordenParaCambiar.PrecioFinal
                    };

                    await _apiClient.UpdateOrdenDeServicioAsync(ordenParaCambiar.Id, ordenActualizada, usuarioConfirmado.Id);
                    MessageBox.Show("¡Estado actualizado con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await CargarOrdenesDeServicio(); // Recargar la grilla
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"No se pudo cargar el historial: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una orden de la lista.", "Selección Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        

        #endregion

        private void administraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 1. Validamos al administrador (reutilizando el método que ya definimos)
            //    (Asumo que tu form de contraseña se llama frmPasswordPrompt)
            if (ValidarAdministrador())
            {
                // 2. Si es OK, abrimos el NUEVO formulario "Hub"
                using (var formHub = new frmAdminHub(_apiClient))
                {
                    formHub.ShowDialog();
                }
            }
            // else: No hacemos nada, el validador (frmPasswordPrompt) ya se encargó.
        }

        /// <summary>
        /// (Este es el método que te pasé antes, asegúrate de tenerlo)
        /// Abre el formulario de contraseña y valida las credenciales.
        /// </summary>
        private bool ValidarAdministrador()
        {
            // ¡REVISA QUE EL NOMBRE 'frmPasswordPrompt' SEA CORRECTO!
            using (var formPassword = new frmPasswordPrompt(_apiClient))
            {
                if (formPassword.ShowDialog() == DialogResult.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}