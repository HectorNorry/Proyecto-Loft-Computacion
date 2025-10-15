namespace LoftComputacion.WinForms
{
    partial class frmBuscarCliente
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txtBusquedaCliente = new TextBox();
            dgvClientes = new DataGridView();
            pnlBusqueda = new Panel();
            pnlAcciones = new Panel();
            btnSeleccionar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvClientes).BeginInit();
            pnlBusqueda.SuspendLayout();
            pnlAcciones.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Bahnschrift SemiBold", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(19, 24);
            label1.Name = "label1";
            label1.Size = new Size(376, 29);
            label1.TabIndex = 0;
            label1.Text = "Buscar Cliente por nombre o DNI :";
            // 
            // txtBusquedaCliente
            // 
            txtBusquedaCliente.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBusquedaCliente.Location = new Point(401, 33);
            txtBusquedaCliente.Name = "txtBusquedaCliente";
            txtBusquedaCliente.Size = new Size(305, 23);
            txtBusquedaCliente.TabIndex = 1;
            // 
            // dgvClientes
            // 
            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvClientes.Dock = DockStyle.Fill;
            dgvClientes.Location = new Point(0, 86);
            dgvClientes.Name = "dgvClientes";
            dgvClientes.ReadOnly = true;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.Size = new Size(800, 264);
            dgvClientes.TabIndex = 2;
            // 
            // pnlBusqueda
            // 
            pnlBusqueda.Controls.Add(label1);
            pnlBusqueda.Controls.Add(txtBusquedaCliente);
            pnlBusqueda.Dock = DockStyle.Top;
            pnlBusqueda.Location = new Point(0, 0);
            pnlBusqueda.Name = "pnlBusqueda";
            pnlBusqueda.Size = new Size(800, 86);
            pnlBusqueda.TabIndex = 3;
            // 
            // pnlAcciones
            // 
            pnlAcciones.Controls.Add(btnSeleccionar);
            pnlAcciones.Dock = DockStyle.Bottom;
            pnlAcciones.Location = new Point(0, 350);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Size = new Size(800, 100);
            pnlAcciones.TabIndex = 4;
            // 
            // btnSeleccionar
            // 
            btnSeleccionar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSeleccionar.Location = new Point(637, 26);
            btnSeleccionar.Name = "btnSeleccionar";
            btnSeleccionar.Size = new Size(128, 53);
            btnSeleccionar.TabIndex = 0;
            btnSeleccionar.Text = "Seleccionar";
            btnSeleccionar.UseVisualStyleBackColor = true;
            // 
            // frmBuscarCliente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvClientes);
            this.Controls.Add(this.pnlAcciones);
            this.Controls.Add(this.pnlBusqueda);
            this.Name = "frmBuscarCliente";
            this.Text = "Buscar Cliente";

            // --- ¡AGREGÁ ESTA LÍNEA AQUÍ! ---
            this.Load += new System.EventHandler(this.frmBuscarCliente_Load);
            // --- FIN DE LA LÍNEA A AGREGAR ---

            this.pnlBusqueda.ResumeLayout(false);
            this.pnlBusqueda.PerformLayout();
            this.pnlAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private TextBox txtBusquedaCliente;
        private DataGridView dgvClientes;
        private Panel pnlBusqueda;
        private Panel pnlAcciones;
        private Button btnSeleccionar;
    }
}