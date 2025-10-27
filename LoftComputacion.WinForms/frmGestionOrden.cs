using LoftComputacion.Domain;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks; // Asegúrate de tener este
using System.Windows.Forms;

namespace LoftComputacion.WinForms
{
    // Solo UNA definición de la clase aquí
    public partial class frmGestionOrden : Form
    {
        private readonly ApiClient _apiClient;
        private Cliente? _clienteSeleccionado = null;
        private OrdenDeServicio? _ordenParaEditar = null; // Variable para guardar la orden en modo edición

        // Constructor para CREAR una nueva orden
        public frmGestionOrden(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
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

                // --- Habilitar/Deshabilitar Paneles ---
                groupBox4.Enabled = true; // HABILITAMOS el panel de fotos
                btnBuscarCliente.Enabled = false;
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
                cmbEstado.Enabled = true;
                txtPrecioPresupuesto.ReadOnly = false;
                txtPrecioFinal.ReadOnly = false;

                // --- Cargar Datos ---
                // Cliente
                if (_ordenParaEditar.Cliente != null)
                {
                    _clienteSeleccionado = _ordenParaEditar.Cliente;
                    txtNombreCliente.Text = _ordenParaEditar.Cliente.NombreCompleto;
                    txtTelefonoCliente.Text = _ordenParaEditar.Cliente.Telefono;
                    txtEmailCliente.Text = _ordenParaEditar.Cliente.Email;
                    txtDniCliente.Text = _ordenParaEditar.Cliente.DNI;
                }

                // Equipo
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

                // Falla
                txtFallaDeclarada.Text = _ordenParaEditar.FallaDeclaradaPorCliente;

                // Estado y Precios
                cmbEstado.SelectedValue = _ordenParaEditar.EstadoId;
                txtPrecioPresupuesto.Text = _ordenParaEditar.PrecioPresupuestado?.ToString("F2");
                txtPrecioFinal.Text = _ordenParaEditar.PrecioFinal?.ToString("F2");

                // Cargar Fotos (al final)
                await CargarFotosDeLaOrden();
            }
            else // MODO CREACIÓN
            {
                this.Text = "Crear Nueva Orden de Servicio";
                btnGuardar.Text = "Guardar";

                // --- Habilitar/Deshabilitar Paneles ---
                groupBox4.Enabled = false; // DESHABILITAMOS el panel de fotos
                btnBuscarCliente.Enabled = true;
                cmbTipoEquipo.Enabled = true; // Habilitado para crear
                txtNombreCliente.ReadOnly = false;
                txtTelefonoCliente.ReadOnly = false;
                txtEmailCliente.ReadOnly = false;
                txtDniCliente.ReadOnly = false;
                txtMarca.ReadOnly = false;
                txtModelo.ReadOnly = false;
                txtNumeroSerie.ReadOnly = false;
                txtComponentes.ReadOnly = false;
                txtFallaDeclarada.ReadOnly = false;
                cmbEstado.Enabled = false; // El estado inicial (Recibido) no se elige
                txtPrecioPresupuesto.ReadOnly = true;
                txtPrecioFinal.ReadOnly = true;

                // --- Estado Inicial ---
                cmbTipoEquipo.SelectedIndex = 0; // Notebook por defecto
                if (cmbEstado.Items.Count > 0)
                {
                    cmbEstado.SelectedIndex = 0; // "Recibido" por defecto
                }
            }
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
                using (var formConfirmacion = new frmConfirmarCambio())
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

                    // --- AQUÍ SE DECLARA nuevaOrden ---
                    var nuevaOrden = new OrdenDeServicio
                    {
                        ClienteId = clienteParaGuardar.Id,
                        EquipoId = equipoCreado.Id,
                        FallaDeclaradaPorCliente = txtFallaDeclarada.Text
                    };
                    // --- Y AQUÍ SE USA ---
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
            using (var formBusqueda = new frmBuscarCliente())
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
    }

}