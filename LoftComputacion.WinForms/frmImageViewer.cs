using System;
using System.Drawing;
using System.Windows.Forms;


namespace LoftComputacion.WinForms
{
    public partial class frmImageViewer : Form
    {
        public frmImageViewer()
        {
            InitializeComponent();
        }

        // Constructor modificado que recibe un objeto Image
        public frmImageViewer(string imageUrl) : this()
        {
            try
            {
                // Le decimos al PictureBox grande que cargue la imagen desde la URL
                picGrande.LoadAsync(imageUrl);
                this.Text = "Visor de Imagen";

                // --- (Aquí iría la lógica de ajuste de tamaño, si querés mantenerla) ---
                // Por ahora, lo dejamos simple. El PictureBox con Dock=Fill y SizeMode=Zoom
                // se ajustará al tamaño del formulario.
                // Podés re-agregar la lógica de ajuste de tamaño si lo ves necesario.
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la imagen: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}