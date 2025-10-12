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
        public frmGestionOrden()
        {
            InitializeComponent();
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

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Por ahora, solo mostramos un mensaje para confirmar que funciona
            MessageBox.Show("¡Lógica para guardar la orden irá aquí!");
        }
    }
}
