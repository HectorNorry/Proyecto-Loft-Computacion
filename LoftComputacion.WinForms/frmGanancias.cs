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
using System.IO;

namespace LoftComputacion.WinForms
{
    public partial class frmGanancias : Form
    {
        private readonly ApiClient _apiClient;
        public frmGanancias(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
        }

        private void frmGanancias_Load(object sender, EventArgs e)
        {

        }

        private async void btnFiltrarGanancias_Click(object sender, EventArgs e)
        {
            // 1. Obtenemos las fechas seleccionadas por el usuario
            DateTime fechaDesde = dtpFechaDesde.Value;
            DateTime fechaHasta = dtpFechaHasta.Value;

            try
            {
                // 2. Llamamos a la API para obtener los datos
                var resultadoGanancias = await _apiClient.GetGananciasAsync(fechaDesde, fechaHasta);

                // 3. Mostramos las órdenes en la grilla
                dgvGanancias.DataSource = null; // Limpiamos antes
                dgvGanancias.DataSource = resultadoGanancias.Ordenes;

                // 4. Configuramos las columnas de la grilla (puedes ajustar esto)
                if (dgvGanancias.Columns.Count > 0)
                {
                    dgvGanancias.AutoGenerateColumns = false; // Desactivar si defines columnas manualmente
                    dgvGanancias.Columns.Clear();

                    dgvGanancias.Columns.Add(new DataGridViewTextBoxColumn { Name = "colFechaPago", HeaderText = "Fecha Pago", DataPropertyName = nameof(OrdenDeServicio.FechaPago), DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yy HH:mm" } });
                    dgvGanancias.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCliente", HeaderText = "Cliente", DataPropertyName = "Cliente.NombreCompleto" });
                    dgvGanancias.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMonto", HeaderText = "Monto Cobrado", DataPropertyName = nameof(OrdenDeServicio.PrecioFinal), DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } });
                    dgvGanancias.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMetodoPago", HeaderText = "Método Pago", DataPropertyName = "MetodoDePago.Nombre" });

                    dgvGanancias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }


                // 5. Mostramos el total en el Label, formateado como moneda
                lblTotalGanancias.Text = $"Total: {resultadoGanancias.Total:C2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener las ganancias: {ex.Message}", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvGanancias.DataSource = null;
                lblTotalGanancias.Text = "Total: $0.00";
            }
        }

        private async void btnDescargarResumen_Click(object sender, EventArgs e)
        {
            // 1. Obtenemos las fechas seleccionadas (igual que en el filtro)
            DateTime fechaDesde = dtpFechaDesde.Value;
            DateTime fechaHasta = dtpFechaHasta.Value;

            // 2. Creamos un diálogo para "Guardar Como..."
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivo de Excel (*.xlsx)|*.xlsx", // Filtro para mostrar solo .xlsx
                Title = "Guardar Resumen de Ganancias",
                // Generamos un nombre de archivo sugerido
                FileName = $"Ganancias_Loft_{fechaDesde:yyyyMMdd}_a_{fechaHasta:yyyyMMdd}.xlsx"
            };

            // 3. Mostramos el diálogo y verificamos si el usuario hizo clic in "Guardar"
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 4. Llamamos a la API para obtener los bytes del archivo Excel
                    byte[] fileBytes = await _apiClient.DownloadGananciasExcelAsync(fechaDesde, fechaHasta);

                    // 5. Escribimos esos bytes en el archivo que el usuario seleccionó
                    File.WriteAllBytes(saveFileDialog.FileName, fileBytes);

                    MessageBox.Show($"Resumen guardado exitosamente en:\n{saveFileDialog.FileName}",
                                    "Descarga Completa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al descargar el resumen: {ex.Message}", "Error de Descarga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
