using LoftComputacion.Domain;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks; // Asegúrate de tener este
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace LoftComputacion.WinForms
{
    // Solo UNA definición de la clase aquí
    public partial class frmGestionOrden : MaterialForm
    {
        private readonly ApiClient _apiClient;
        private Cliente? _clienteSeleccionado = null;
        private OrdenDeServicio? _ordenParaEditar = null; // Variable para guardar la orden en modo edición

        // Constructor para CREAR una nueva orden
        public frmGestionOrden(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;

            var skin = MaterialSkinManager.Instance;
            skin.AddFormToManage(this);
            skin.Theme = MaterialSkinManager.Themes.LIGHT;

            skin.ColorScheme = new ColorScheme(
                Primary.Blue600,
                Primary.Blue700,
                Primary.Blue200,
                Accent.LightBlue200,
                TextShade.WHITE
            );
        }

        // Constructor para EDITAR una orden existente
        public frmGestionOrden(OrdenDeServicio orden, ApiClient apiClient) : this(apiClient) // Llama al constructor de arriba
        {
            _ordenParaEditar = orden;
        }

        // --- EVENTOS ---

        private async void frmGestionOrden_Load(object sender, EventArgs e)
        {
            // Llenar ComboBox Tipo Equipo (siempre)
            cmbTipoEquipo.Items.Clear();
            cmbTipoEquipo.Items.Add("Notebook");
            cmbTipoEquipo.Items.Add("PC de Escritorio");
            cmbTipoEquipo.Items.Add("Impresora");

            // Cargar ComboBox de Estados (siempre)
            try
            {
                var estados = await _apiClient.GetEstadosAsync();
                cmbEstado.DataSource = estados;
                cmbEstado.DisplayMember = "Nombre";
                cmbEstado.ValueMember = "Id";

                // Lógica de selección del ComboBox DESPUÉS de cargar el DataSource
                if (_ordenParaEditar != null)
                {
                    // Estamos en MODO EDICIÓN, seleccionamos el valor guardado
                    cmbEstado.SelectedValue = _ordenParaEditar.EstadoId;
                }
                else
                {
                    // Estamos en MODO CREACIÓN, seleccionamos el primero
                    if (cmbEstado.Items.Count > 0)
                    {
                        cmbEstado.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los estados: {ex.Message}", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // --- LÓGICA DE MODO EDICIÓN VS. CREACIÓN ---

            if (_ordenParaEditar != null) // MODO EDICIÓN
            {
                this.Text = $"Editando Orden N° {_ordenParaEditar.Id}";
                btnGuardar.Text = "Actualizar";

                // --- 1. Lógica de Visibilidad (La que hicimos) ---
                pnlGestionTecnica.Visible = true;
                pnlBotones.Visible = true;
                label10.Visible = true; // "Estado del trabajo"
                cmbEstado.Visible = true;
                label11.Visible = true; // "Presupuesto"
                txtPrecioPresupuesto.Visible = true;
                label12.Visible = true; // "Precio FINAL"
                txtPrecioFinal.Visible = true;
                txtResumenTecnico.Visible = true;

                btnPagarMP.Visible = (_ordenParaEditar.PrecioFinal ?? 0) > 0;

                // ---

                // --- 2. Lógica de Habilitación (La que hicimos) ---
                groupBox4.Enabled = true; // Panel de fotos
                btnBuscarCliente.Enabled = false;

                // --- ¡ESTO TAMBIÉN FALTABA! (Ponerlos ReadOnly) ---
                cmbTipoEquipo.Enabled = false;
                txtNombreCliente.ReadOnly = true;
                txtTelefonoCliente.ReadOnly = true;
                txtEmailCliente.ReadOnly = true;
                txtDniCliente.ReadOnly = true;
                txtMarca.ReadOnly = true;
                txtModelo.ReadOnly = true;
                txtNumeroSerie.ReadOnly = true;
                txtComponentes.ReadOnly = true;
                txtFallaDeclarada.ReadOnly = true;
                // Habilitar controles de gestión
                cmbEstado.Enabled = true;
                txtPrecioPresupuesto.ReadOnly = false;
                txtPrecioFinal.ReadOnly = false;
                txtResumenTecnico.ReadOnly = false;
                // ---

                // --- 3. ¡EL CÓDIGO QUE SE BORRÓ! (Cargar Datos) ---
                if (_ordenParaEditar.Cliente != null)
                {
                    _clienteSeleccionado = _ordenParaEditar.Cliente;
                    txtNombreCliente.Text = _ordenParaEditar.Cliente.NombreCompleto;
                    txtTelefonoCliente.Text = _ordenParaEditar.Cliente.Telefono;
                    txtEmailCliente.Text = _ordenParaEditar.Cliente.Email;
                    txtDniCliente.Text = _ordenParaEditar.Cliente.DNI;
                }

                if (_ordenParaEditar.Equipo != null)
                {
                    if (cmbTipoEquipo.Items.Count > (int)_ordenParaEditar.Equipo.Tipo)
                    {
                        cmbTipoEquipo.SelectedIndex = (int)_ordenParaEditar.Equipo.Tipo;
                    }
                    txtMarca.Text = _ordenParaEditar.Equipo.Marca;
                    txtModelo.Text = _ordenParaEditar.Equipo.Modelo;
                    txtNumeroSerie.Text = _ordenParaEditar.Equipo.NumeroDeSerie;
                    txtComponentes.Text = _ordenParaEditar.Equipo.Componentes;
                }

                txtFallaDeclarada.Text = _ordenParaEditar.FallaDeclaradaPorCliente;
                txtPrecioPresupuesto.Text = _ordenParaEditar.PrecioPresupuestado?.ToString("F2");
                txtPrecioFinal.Text = _ordenParaEditar.PrecioFinal?.ToString("F2");
                txtResumenTecnico.Text = _ordenParaEditar.ResumenTecnico;
                // --- FIN DEL BLOQUE QUE FALTABA ---

                await CargarFotosDeLaOrden();

                // (Si el dgvHistorial está en este form, aquí iría la llamada a CargarHistorial())
            }
            else // MODO CREACIÓN
            {
                this.Text = "Crear Nueva Orden de Servicio";
                btnGuardar.Text = "Guardar";

                // --- Lógica de Visibilidad (Modo Creación) ---
                pnlGestionTecnica.Visible = true;
                pnlBotones.Visible = true;
                label10.Visible = false; // "Estado del trabajo"
                cmbEstado.Visible = false;
                label11.Visible = false; // "Presupuesto"
                txtPrecioPresupuesto.Visible = false;
                label12.Visible = false; // "Precio FINAL"
                txtPrecioFinal.Visible = false;
                // lblResumenTecnico.Visible = false;
                txtResumenTecnico.Visible = false;
                // ---

                // --- Lógica de Habilitación (Modo Creación) ---
                groupBox4.Enabled = false; // Panel de fotos
                btnBuscarCliente.Enabled = true;

                // ¡Habilitar los campos para la creación!
                cmbTipoEquipo.Enabled = true;
                txtNombreCliente.ReadOnly = false;
                txtTelefonoCliente.ReadOnly = false;
                txtEmailCliente.ReadOnly = false;
                txtDniCliente.ReadOnly = false;
                txtMarca.ReadOnly = false;
                txtModelo.ReadOnly = false;
                txtNumeroSerie.ReadOnly = false;
                txtComponentes.ReadOnly = false;
                txtFallaDeclarada.ReadOnly = false;

                btnPagarMP.Visible = false;
                // ---

                // Estado Inicial
                cmbTipoEquipo.SelectedIndex = 0;
            }
            this.BackColor = Color.FromArgb(245, 245, 245);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (_ordenParaEditar != null) // Estamos en modo EDICIÓN
            {
                // --- MOSTRAR VENTANA DE CONFIRMACIÓN ---
                int usuarioIdConfirmado = 0; // Variable para guardar el ID del usuario validado
                using (var formConfirmacion = new frmConfirmarCambio(_apiClient))
                {
                    // Mostramos el formulario de confirmación
                    if (formConfirmacion.ShowDialog() == DialogResult.OK)
                    {
                        // Si el usuario confirmó correctamente, guardamos su ID
                        usuarioIdConfirmado = formConfirmacion.UsuarioSeleccionado.Id;
                    }
                    else
                    {
                        // Si el usuario canceló, no hacemos nada más.
                        MessageBox.Show("Actualización cancelada.", "Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return; // Salimos del método
                    }
                }
                // --- FIN VENTANA DE CONFIRMACIÓN ---

                // Si llegamos aquí, la confirmación fue exitosa. Procedemos a actualizar.
                try
                {
                    // Leemos el nuevo estado seleccionado
                    if (cmbEstado.SelectedValue != null)
                    {
                        _ordenParaEditar.EstadoId = (int)cmbEstado.SelectedValue;
                    }

                    // Leemos los precios
                    if (decimal.TryParse(txtPrecioPresupuesto.Text, out decimal presupuesto))
                    {
                        _ordenParaEditar.PrecioPresupuestado = presupuesto;
                    }
                    else
                    {
                        _ordenParaEditar.PrecioPresupuestado = null;
                    }

                    if (decimal.TryParse(txtPrecioFinal.Text, out decimal precioFinal))
                    {
                        _ordenParaEditar.PrecioFinal = precioFinal;
                    }
                    else
                    {
                        _ordenParaEditar.PrecioFinal = null;
                    }

                    // --- ¡NUEVA LÍNEA! ---
                    // Leemos el resumen del técnico desde el nuevo TextBox
                    _ordenParaEditar.ResumenTecnico = txtResumenTecnico.Text;

                    // Llamamos al ApiClient PASANDO EL ID DEL USUARIO CONFIRMADO
                    await _apiClient.UpdateOrdenDeServicioAsync(_ordenParaEditar.Id, _ordenParaEditar, usuarioIdConfirmado);

                    MessageBox.Show("¡Orden de servicio actualizada con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (FormatException)
                {
                    MessageBox.Show("Por favor, ingrese un valor numérico válido para los precios.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al actualizar la orden: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // Estamos en modo CREACIÓN
            {
                // (El modo CREACIÓN no se toca, ya que el ResumenTecnico
                //  se genera después, durante la edición/reparación)
                try
                {
                    Cliente clienteParaGuardar;
                    if (_clienteSeleccionado != null)
                    {
                        clienteParaGuardar = _clienteSeleccionado;
                    }
                    else
                    {
                        var nuevoCliente = new Cliente
                        {
                            NombreCompleto = txtNombreCliente.Text,
                            Telefono = txtTelefonoCliente.Text,
                            Email = txtEmailCliente.Text,
                            DNI = txtDniCliente.Text
                        };
                        clienteParaGuardar = await _apiClient.CreateClienteAsync(nuevoCliente);
                    }

                    var nuevoEquipo = new Equipo
                    {
                        Tipo = ObtenerTipoDeEquipoSeleccionado(),
                        Marca = txtMarca.Text,
                        Modelo = txtModelo.Text,
                        NumeroDeSerie = txtNumeroSerie.Text,
                        Componentes = txtComponentes.Text
                    };
                    var equipoCreado = await _apiClient.CreateEquipoAsync(nuevoEquipo);

                    var nuevaOrden = new OrdenDeServicio
                    {
                        ClienteId = clienteParaGuardar.Id,
                        EquipoId = equipoCreado.Id,
                        FallaDeclaradaPorCliente = txtFallaDeclarada.Text
                        // ResumenTecnico se deja en null, está correcto.
                    };

                    await _apiClient.CreateOrdenDeServicioAsync(nuevaOrden);

                    MessageBox.Show("¡Orden de servicio creada con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al guardar la orden: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            using (var formBusqueda = new frmBuscarCliente(_apiClient))
            {
                if (formBusqueda.ShowDialog() == DialogResult.OK)
                {
                    _clienteSeleccionado = formBusqueda.ClienteSeleccionado;
                    txtNombreCliente.Text = _clienteSeleccionado.NombreCompleto;
                    txtTelefonoCliente.Text = _clienteSeleccionado.Telefono;
                    txtEmailCliente.Text = _clienteSeleccionado.Email;
                    txtDniCliente.Text = _clienteSeleccionado.DNI;

                    txtNombreCliente.ReadOnly = true;
                    txtTelefonoCliente.ReadOnly = true;
                    txtEmailCliente.ReadOnly = true;
                    txtDniCliente.ReadOnly = true;
                    btnBuscarCliente.Enabled = false; // Deshabilitamos buscar una vez seleccionado
                }
            }
        }

        // --- MÉTODOS AYUDANTE ---
        private TipoDeEquipo ObtenerTipoDeEquipoSeleccionado()
        {
            string seleccion = cmbTipoEquipo.SelectedItem?.ToString() ?? string.Empty; // Más seguro
            switch (seleccion)
            {
                case "Notebook": return TipoDeEquipo.Notebook;
                case "PC de Escritorio": return TipoDeEquipo.PC_Escritorio;
                case "Impresora": return TipoDeEquipo.Impresora;
                default: throw new InvalidOperationException("Tipo de equipo no válido.");
            }
        }

        private async void btnAdjuntarFoto_Click(object sender, EventArgs e)
        {
            // 1. Verificar que estemos en modo edición
            if (_ordenParaEditar == null)
            {
                MessageBox.Show("Debe guardar la orden antes de poder adjuntar fotos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2. Abrir el diálogo para seleccionar archivos
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Archivos de Imagen|*.jpg;*.jpeg;*.png";
                openFileDialog.Title = "Seleccionar una o más fotos";
                openFileDialog.Multiselect = true; // Permitir seleccionar varias

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        try
                        {
                            // 3. Comprimir la imagen antes de subirla
                            using (var imageStream = new MemoryStream())
                            {
                                using (var img = Image.FromFile(filePath))
                                {
                                    // (Opcional: Reescalar si es muy grande)
                                    // var resizedImg = ReescalarImagen(img, 1024); 

                                    // Guardamos la imagen en el stream en formato Jpeg con calidad 85%
                                    img.Save(imageStream, ImageFormat.Jpeg);
                                }

                                imageStream.Position = 0; // Rebobinamos el stream al inicio

                                // 4. Subir la imagen comprimida usando el ApiClient
                                var fileName = Path.GetFileName(filePath);
                                var fotoSubida = await _apiClient.UploadFotoAsync(_ordenParaEditar.Id, imageStream, fileName);

                                // 5. Agregar la foto recién subida a la lista visual
                                lstFotosAdjuntas.Items.Add(fotoSubida);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error al subir la foto '{Path.GetFileName(filePath)}': {ex.Message}", "Error de Subida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    MessageBox.Show("¡Fotos adjuntadas con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void lstFotosAdjuntas_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verificamos que el ítem sea un objeto Foto
            if (lstFotosAdjuntas.SelectedItem is Foto fotoSeleccionada)
            {
                try
                {
                    string urlImagen = fotoSeleccionada.RutaArchivo;

                    if (!string.IsNullOrEmpty(urlImagen))
                    {
                        // ¡LA SOLUCIÓN!
                        // Usamos el método nativo de PictureBox para cargar una imagen desde una URL.
                        // Esto maneja la descarga en segundo plano (asíncrona) automáticamente.
                        picFotoPreview.LoadAsync(urlImagen);
                    }
                    else
                    {
                        picFotoPreview.Image = null; // Limpiamos si no hay URL
                    }
                }
                catch (Exception ex)
                {
                    picFotoPreview.Image = null;
                    MessageBox.Show($"Error al cargar la vista previa: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnQuitarFoto_Click(object sender, EventArgs e)
        {
            if (lstFotosAdjuntas.SelectedItem is Foto fotoSeleccionada)
            {
                // Confirmación
                var confirmResult = MessageBox.Show(
                    "¿Está seguro de que desea eliminar esta foto permanentemente?",
                    "Confirmar Eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        // Llamamos a la API para borrarla
                        await _apiClient.DeleteFotoAsync(fotoSeleccionada.Id);

                        // Si la API no dio error, la quitamos de la lista
                        lstFotosAdjuntas.Items.Remove(fotoSeleccionada);
                        picFotoPreview.Image = null; // Limpiamos vista previa
                        MessageBox.Show("Foto eliminada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al eliminar la foto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una foto de la lista para eliminar.", "No hay foto seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void picFotoPreview_Click(object sender, EventArgs e)
        {
            // Verificamos si el PictureBox tiene una URL de imagen asignada
            if (!string.IsNullOrEmpty(picFotoPreview.ImageLocation))
            {
                // Creamos el visor pasándole la URL, no la imagen.
                // Es más simple y el visor descargará su propia copia.
                using (frmImageViewer visor = new frmImageViewer(picFotoPreview.ImageLocation))
                {
                    visor.ShowDialog();
                }
            }
        }

        private async Task CargarFotosDeLaOrden()
        {
            // 1. Asegurarnos de que estamos en modo edición
            if (_ordenParaEditar == null) return;

            try
            {
                // 2. Limpiamos los controles
                lstFotosAdjuntas.Items.Clear();
                picFotoPreview.Image = null;

                // 3. Llamamos a la API para obtener la lista de fotos de esta orden
                // (¡Asegurate de que el método GetFotosAsync exista en tu ApiClient!)
                var fotos = await _apiClient.GetFotosAsync(_ordenParaEditar.Id);

                // 4. Llenamos el ListBox con los objetos 'Foto'
                foreach (var foto in fotos)
                {
                    lstFotosAdjuntas.Items.Add(foto);
                }

                // 5. Le decimos al ListBox qué propiedad del objeto 'Foto' debe mostrar
                lstFotosAdjuntas.DisplayMember = "RutaArchivo";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las fotos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnPagarMP_Click(object sender, EventArgs e)
        {
            // 1. Verificamos que estemos en Modo Edición
            if (_ordenParaEditar == null)
            {
                MessageBox.Show("Debe guardar la orden antes de poder generar un pago.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Verificamos que haya un precio final para cobrar
            if ((_ordenParaEditar.PrecioFinal ?? 0) <= 0)
            {
                MessageBox.Show("La orden no tiene un precio final válido para cobrar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            btnPagarMP.Enabled = false;
            btnPagarMP.Text = "Generando link...";

            try
            {
                // 3. Llamamos al ApiClient (el método que creamos en el paso 5A)
                string urlDePago = await _apiClient.CrearLinkDePagoAsync(_ordenParaEditar.Id);

                // 4. ¡LA MAGIA! Abrimos el link de pago en el navegador
                //    predeterminado del usuario (Chrome, Edge, etc.)

                // --- Para que esto funcione, añade la siguiente línea ---
                // --- al INICIO de tu archivo (arriba de todo):     ---
                // using System.Diagnostics;
                // --------------------------------------------------------
                Process.Start(new ProcessStartInfo(urlDePago) { UseShellExecute = true });

                // (Opcional) Preguntamos si el pago se completó
                var result = MessageBox.Show(
                    "Se ha abierto el link de pago en su navegador.\n\n¿El pago se completó exitosamente?",
                    "Pago Enviado",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // (Futuro) Aquí podríamos cambiar el estado a "Pagado" y cerrar la orden.
                    // Por ahora, solo cerramos.
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                // 5. Manejamos cualquier error de la API
                MessageBox.Show($"Error al generar el link de pago: {ex.Message}", "Error de API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnPagarMP.Enabled = true;
                btnPagarMP.Text = "Pagar con Mercado Pago";
            }
        }
    }

}