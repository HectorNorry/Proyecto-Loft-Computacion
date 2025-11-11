using System;
using System.Windows.Forms;

namespace LoftComputacion.WinForms
{
    public partial class frmAdminHub : Form
    {
        private readonly ApiClient _apiClient;

        // Recibimos el ApiClient para pasárselo a los otros formularios
        public frmAdminHub(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
        }

        private void btnGestionarUsuarios_Click(object sender, EventArgs e)
        {
            // Abrimos el formulario de usuarios
            using (var formUsuarios = new frmGestionUsuarios(_apiClient))
            {
                formUsuarios.ShowDialog();
            }
        }

        private void btnVerGanancias_Click(object sender, EventArgs e)
        {
            // Abrimos el formulario de ganancias
            using (var formGanancias = new frmGanancias(_apiClient))
            {
                formGanancias.ShowDialog();
            }
        }
    }
}