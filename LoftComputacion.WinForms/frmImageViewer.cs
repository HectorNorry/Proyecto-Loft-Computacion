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
        public frmImageViewer(Image imageToShow) : this()
        {
            try
            {
                picGrande.Image = imageToShow;
                this.Text = "Visor de Imagen";

                // --- LÓGICA DE AJUSTE DE TAMAÑO ---
                int maxWidth = Screen.PrimaryScreen.WorkingArea.Width * 90 / 100;
                int maxHeight = Screen.PrimaryScreen.WorkingArea.Height * 90 / 100;

                // --- ESTAS LÍNEAS FALTABAN (Declaración de variables) ---
                // Calculamos el tamaño del PictureBox necesario para la imagen original
                int imageWidth = imageToShow.Width;
                int imageHeight = imageToShow.Height;

                // Ajustamos el tamaño del FORMULARIO (no solo del PictureBox)
                int formWidth = imageWidth + (this.Width - picGrande.Width); // Ancho de imagen + márgenes
                int formHeight = imageHeight + (this.Height - picGrande.Height); // Alto de imagen + márgenes
                                                                                 // --- FIN LÍNEAS FALTANTES ---

                // Aseguramos que el formulario no exceda el tamaño máximo
                if (formWidth > maxWidth)
                {
                    formHeight = (int)((double)formHeight * maxWidth / formWidth);
                    formWidth = maxWidth;
                }
                if (formHeight > maxHeight)
                {
                    formWidth = (int)((double)formWidth * maxHeight / formHeight);
                    formHeight = maxHeight;
                }

                // Aseguramos un tamaño mínimo
                int minFormWidth = 400;
                int minFormHeight = 300;
                formWidth = Math.Max(formWidth, minFormWidth);
                formHeight = Math.Max(formHeight, minFormHeight);

                this.Size = new Size(formWidth, formHeight); // Ahora 'formWidth' y 'formHeight' existen
                this.CenterToScreen();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la imagen: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}