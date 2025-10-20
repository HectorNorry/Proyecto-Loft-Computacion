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

        private Cliente? _clienteSeleccionado = null;

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

        private TipoDeEquipo ObtenerTipoDeEquipoSeleccionado()
        {
            // Tomamos el texto seleccionado en el ComboBox (ej: "PC de Escritorio")
            string seleccion = cmbTipoEquipo.SelectedItem.ToString();

            // Usamos un switch para devolver el valor enum correcto
            switch (seleccion)
            {
                case "Notebook":
                    return TipoDeEquipo.Notebook;
                case "PC de Escritorio":
                    return TipoDeEquipo.PC_Escritorio;
                case "Impresora":
                    return TipoDeEquipo.Impresora;
                default:
                    // Si por alguna razón hay un valor inesperado, lanzamos un error
                    throw new InvalidOperationException("Tipo de equipo no válido seleccionado.");
            }
        }
        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente clienteParaGuardar; // Variable para guardar el cliente (nuevo o existente)

                // --- LÓGICA Cliente Nuevo vs. Existente (CORREGIDA) ---
                if (_clienteSeleccionado != null)
                {
                    // Si _clienteSeleccionado tiene valor, usamos ese cliente directamente.
                    clienteParaGuardar = _clienteSeleccionado;
                }
                else
                {
                    // Si _clienteSeleccionado es null, AHÍ SÍ creamos uno nuevo.
                    var nuevoCliente = new Cliente
                    {
                        NombreCompleto = txtNombreCliente.Text,
                        Telefono = txtTelefonoCliente.Text,
                        Email = txtEmailCliente.Text,
                        DNI = txtDniCliente.Text
                    };
                    // Llamamos a la API SOLO si es un cliente nuevo.
                    clienteParaGuardar = await _apiClient.CreateClienteAsync(nuevoCliente);
                }
                // --- FIN LÓGICA Cliente ---


                // Crear el nuevo Equipo (esta parte sigue igual)
                var nuevoEquipo = new Equipo
                {
                    Tipo = ObtenerTipoDeEquipoSeleccionado(),
                    Marca = txtMarca.Text,
                    Modelo = txtModelo.Text,
                    NumeroDeSerie = txtNumeroSerie.Text,
                    Componentes = txtComponentes.Text
                };
                var equipoCreado = await _apiClient.CreateEquipoAsync(nuevoEquipo);

                // Crear la nueva Orden de Servicio (esta parte sigue igual)
                var nuevaOrden = new OrdenDeServicio
                {
                    ClienteId = clienteParaGuardar.Id,
                    EquipoId = equipoCreado.Id,
                    FallaDeclaradaPorCliente = txtFallaDeclarada.Text
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

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            using (var formBusqueda = new frmBuscarCliente())
            {
                if (formBusqueda.ShowDialog() == DialogResult.OK)
                {
                    // --- ¡ESTA ES LA LÍNEA QUE FALTABA! ---
                    _clienteSeleccionado = formBusqueda.ClienteSeleccionado; // Guardamos el cliente seleccionado
                                                                             // --- FIN DE LA LÍNEA QUE FALTABA ---

                    // Rellenamos los TextBox con sus datos
                    txtNombreCliente.Text = _clienteSeleccionado.NombreCompleto;
                    txtTelefonoCliente.Text = _clienteSeleccionado.Telefono;
                    txtEmailCliente.Text = _clienteSeleccionado.Email;
                    txtDniCliente.Text = _clienteSeleccionado.DNI;

                    // Bloqueamos los campos para evitar edición
                    txtNombreCliente.ReadOnly = true;
                    txtTelefonoCliente.ReadOnly = true;
                    txtEmailCliente.ReadOnly = true;
                    txtDniCliente.ReadOnly = true;
                }
            }
        }
    }
}
