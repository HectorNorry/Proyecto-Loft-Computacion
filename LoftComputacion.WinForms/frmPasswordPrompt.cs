using System;
using System.Windows.Forms;

namespace LoftComputacion.WinForms
{
    public partial class frmPasswordPrompt : Form
    {
        // Contraseña hardcodeada TEMPORALMENTE. ¡Esto debe cambiarse!
        private const string AdminPassword = "admin";

        public frmPasswordPrompt()
        {
            InitializeComponent();
        }

        private void btnAceptarPass_Click(object sender, EventArgs e)
        {
            if (txtPasswordAdmin.Text == AdminPassword)
            {
                this.DialogResult = DialogResult.OK; // Marcamos éxito
                this.Close();
            }
            else
            {
                MessageBox.Show("Contraseña incorrecta.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // No cerramos, permite reintentar
            }
        }

        private void btnCancelarPass_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; // Marcamos cancelación
            this.Close();
        }
    }
}