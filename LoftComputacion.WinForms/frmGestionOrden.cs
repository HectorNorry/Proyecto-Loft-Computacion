using LoftComputacion.Domain;
using System;
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
        public frmGestionOrden()
        {
            InitializeComponent();
            _apiClient = new ApiClient();
        }

        // Constructor para EDITAR una orden existente
        public frmGestionOrden(OrdenDeServicio orden) : this() // Llama al constructor de arriba
        {
            _ordenParaEditar = orden; // Guardamos la orden que recibimos
        }

        // --- EVENTOS ---

        private async void frmGestionOrden_Load(object sender, EventArgs e)
        {
            // Llenar ComboBox Tipo Equipo (siempre)
            cmbTipoEquipo.Items.Clear();
            cmbTipoEquipo.Items.Add("Notebook");
            cmbTipoEquipo.Items.Add("PC de Escritorio");
            cmbTipoEquipo.Items.Add("Impresora");

            // --- NUEVO: Cargar ComboBox de Estados ---
            try
            {
                var estados = await _apiClient.GetEstadosAsync();
                // Configuramos el ComboBox para que muestre el Nombre pero guarde el Id
                cmbEstado.DataSource = estados;
                cmbEstado.DisplayMember = "Nombre"; // Propiedad a mostrar
                cmbEstado.ValueMember = "Id";       // Propiedad a usar como valor interno
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los estados: {ex.Message}", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Podríamos deshabilitar el ComboBox o cerrar el form si falla
            }
            // --- FIN NUEVO ---


            if (_ordenParaEditar != null) // MODO EDICIÓN
            {
                this.Text = $"Editando Orden N° {_ordenParaEditar.Id}";

                // --- RESTAURAR ESTE BLOQUE: Cargar Cliente y Equipo ---
                if (_ordenParaEditar.Cliente != null)
                {
                    _clienteSeleccionado = _ordenParaEditar.Cliente;
                    txtNombreCliente.Text = _ordenParaEditar.Cliente.NombreCompleto;
                    txtTelefonoCliente.Text = _ordenParaEditar.Cliente.Telefono;
                    txtEmailCliente.Text = _ordenParaEditar.Cliente.Email;
                    txtDniCliente.Text = _ordenParaEditar.Cliente.DNI;

                    txtNombreCliente.ReadOnly = true;
                    txtTelefonoCliente.ReadOnly = true;
                    txtEmailCliente.ReadOnly = true;
                    txtDniCliente.ReadOnly = true;
                    btnBuscarCliente.Enabled = false;
                }

                if (_ordenParaEditar.Equipo != null)
                {
                    // Asegurarse que cmbTipoEquipo tenga items antes de seleccionar
                    if (cmbTipoEquipo.Items.Count > (int)_ordenParaEditar.Equipo.Tipo)
                    {
                        cmbTipoEquipo.SelectedIndex = (int)_ordenParaEditar.Equipo.Tipo;
                    }
                    txtMarca.Text = _ordenParaEditar.Equipo.Marca;
                    txtModelo.Text = _ordenParaEditar.Equipo.Modelo;
                    txtNumeroSerie.Text = _ordenParaEditar.Equipo.NumeroDeSerie;
                    txtComponentes.Text = _ordenParaEditar.Equipo.Componentes;

                    cmbTipoEquipo.Enabled = false;
                    txtMarca.ReadOnly = true;
                    txtModelo.ReadOnly = true;
                    txtNumeroSerie.ReadOnly = true;
                    txtComponentes.ReadOnly = true;
                }
                // --- FIN DEL BLOQUE RESTAURADO ---


                // --- Cargar Estado y Precios (Esto ya estaba bien) ---
                cmbEstado.SelectedValue = _ordenParaEditar.EstadoId;
                txtPrecioPresupuesto.Text = _ordenParaEditar.PrecioPresupuestado?.ToString("F2");
                txtPrecioFinal.Text = _ordenParaEditar.PrecioFinal?.ToString("F2");

                cmbEstado.Enabled = true;
                txtPrecioPresupuesto.ReadOnly = false;
                txtPrecioFinal.ReadOnly = false;
                // --- FIN CARGA ESTADO Y PRECIOS ---

                // Bloqueamos Falla inicial (Esto ya estaba bien)
                txtFallaDeclarada.Text = _ordenParaEditar.FallaDeclaradaPorCliente;
                txtFallaDeclarada.ReadOnly = true;

                btnGuardar.Text = "Actualizar";
            }
            else // MODO CREACIÓN (Esto ya estaba bien)
            {
                // ... (el código del else sigue igual) ...
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
    }
}