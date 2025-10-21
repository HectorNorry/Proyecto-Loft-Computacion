namespace LoftComputacion.WinForms
{
    partial class frmPrincipal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            toolStripMenuItem1 = new ToolStripMenuItem();
            salirToolStripMenuItem = new ToolStripMenuItem();
            órdenesToolStripMenuItem = new ToolStripMenuItem();
            nuevaOrdenToolStripMenuItem = new ToolStripMenuItem();
            dgvOrdenes = new DataGridView();
            lblbuscar = new Label();
            txtBuscar = new TextBox();
            btnBuscar = new Button();
            panel1 = new Panel();
            pnlDetalles = new Panel();
            dgvHistorial = new DataGridView();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblDetalleClienteNombre = new Label();
            lblDetalleEquipoDesc = new Label();
            lblDetalleEstadoActual = new Label();
            lblDetalleFalla = new Label();
            lblDetalleHistorial = new Label();
            txtDetalleClienteNombre = new TextBox();
            txtDetalleEquipoDesc = new TextBox();
            txtDetalleEstadoActual = new TextBox();
            txtDetalleFalla = new TextBox();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrdenes).BeginInit();
            panel1.SuspendLayout();
            pnlDetalles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistorial).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Font = new Font("Verdana", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, órdenesToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(884, 33);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { salirToolStripMenuItem });
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(100, 29);
            toolStripMenuItem1.Text = "Archivo";
            // 
            // salirToolStripMenuItem
            // 
            salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            salirToolStripMenuItem.Size = new Size(131, 30);
            salirToolStripMenuItem.Text = "Salir";
            // 
            // órdenesToolStripMenuItem
            // 
            órdenesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nuevaOrdenToolStripMenuItem });
            órdenesToolStripMenuItem.Name = "órdenesToolStripMenuItem";
            órdenesToolStripMenuItem.Size = new Size(108, 29);
            órdenesToolStripMenuItem.Text = "Órdenes";
            // 
            // nuevaOrdenToolStripMenuItem
            // 
            nuevaOrdenToolStripMenuItem.Name = "nuevaOrdenToolStripMenuItem";
            nuevaOrdenToolStripMenuItem.Size = new Size(217, 30);
            nuevaOrdenToolStripMenuItem.Text = "Nueva Orden";
            nuevaOrdenToolStripMenuItem.Click += nuevaOrdenToolStripMenuItem_Click;
            // 
            // dgvOrdenes
            // 
            dgvOrdenes.AllowUserToAddRows = false;
            dgvOrdenes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrdenes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrdenes.Dock = DockStyle.Fill;
            dgvOrdenes.Location = new Point(0, 85);
            dgvOrdenes.Name = "dgvOrdenes";
            dgvOrdenes.Size = new Size(435, 524);
            dgvOrdenes.TabIndex = 1;
            dgvOrdenes.CellDoubleClick += dgvOrdenes_CellDoubleClick;
            dgvOrdenes.CellFormatting += dgvOrdenes_CellFormatting;
            dgvOrdenes.SelectionChanged += dgvOrdenes_SelectionChanged;
            // 
            // lblbuscar
            // 
            lblbuscar.AutoSize = true;
            lblbuscar.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblbuscar.Location = new Point(12, 15);
            lblbuscar.Name = "lblbuscar";
            lblbuscar.Size = new Size(52, 20);
            lblbuscar.TabIndex = 2;
            lblbuscar.Text = "Buscar";
            // 
            // txtBuscar
            // 
            txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBuscar.Location = new Point(70, 15);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(721, 23);
            txtBuscar.TabIndex = 3;
            txtBuscar.KeyDown += txtBuscar_KeyDown;
            // 
            // btnBuscar
            // 
            btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBuscar.Location = new Point(797, 8);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(75, 35);
            btnBuscar.TabIndex = 4;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(lblbuscar);
            panel1.Controls.Add(btnBuscar);
            panel1.Controls.Add(txtBuscar);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 33);
            panel1.Name = "panel1";
            panel1.Size = new Size(884, 52);
            panel1.TabIndex = 5;
            // 
            // pnlDetalles
            // 
            pnlDetalles.Controls.Add(dgvHistorial);
            pnlDetalles.Controls.Add(tableLayoutPanel1);
            pnlDetalles.Dock = DockStyle.Right;
            pnlDetalles.Location = new Point(435, 85);
            pnlDetalles.Name = "pnlDetalles";
            pnlDetalles.Size = new Size(449, 524);
            pnlDetalles.TabIndex = 6;
            // 
            // dgvHistorial
            // 
            dgvHistorial.AllowUserToAddRows = false;
            dgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistorial.BorderStyle = BorderStyle.Fixed3D;
            dgvHistorial.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistorial.Dock = DockStyle.Fill;
            dgvHistorial.Location = new Point(0, 238);
            dgvHistorial.Name = "dgvHistorial";
            dgvHistorial.ReadOnly = true;
            dgvHistorial.RowHeadersVisible = false;
            dgvHistorial.Size = new Size(449, 286);
            dgvHistorial.TabIndex = 9;
            dgvHistorial.CellFormatting += dgvHistorial_CellFormatting;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(lblDetalleClienteNombre, 0, 0);
            tableLayoutPanel1.Controls.Add(lblDetalleEquipoDesc, 0, 1);
            tableLayoutPanel1.Controls.Add(lblDetalleEstadoActual, 0, 2);
            tableLayoutPanel1.Controls.Add(lblDetalleFalla, 0, 3);
            tableLayoutPanel1.Controls.Add(lblDetalleHistorial, 0, 4);
            tableLayoutPanel1.Controls.Add(txtDetalleClienteNombre, 1, 0);
            tableLayoutPanel1.Controls.Add(txtDetalleEquipoDesc, 1, 1);
            tableLayoutPanel1.Controls.Add(txtDetalleEstadoActual, 1, 2);
            tableLayoutPanel1.Controls.Add(txtDetalleFalla, 1, 3);
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(449, 238);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // lblDetalleClienteNombre
            // 
            lblDetalleClienteNombre.AutoSize = true;
            lblDetalleClienteNombre.Font = new Font("Arial", 14.25F);
            lblDetalleClienteNombre.Location = new Point(3, 9);
            lblDetalleClienteNombre.Margin = new Padding(3, 9, 3, 9);
            lblDetalleClienteNombre.Name = "lblDetalleClienteNombre";
            lblDetalleClienteNombre.Size = new Size(152, 22);
            lblDetalleClienteNombre.TabIndex = 0;
            lblDetalleClienteNombre.Text = "Nombre Cliente :";
            // 
            // lblDetalleEquipoDesc
            // 
            lblDetalleEquipoDesc.AutoSize = true;
            lblDetalleEquipoDesc.Font = new Font("Arial", 14.25F);
            lblDetalleEquipoDesc.Location = new Point(3, 49);
            lblDetalleEquipoDesc.Margin = new Padding(3, 9, 3, 9);
            lblDetalleEquipoDesc.Name = "lblDetalleEquipoDesc";
            lblDetalleEquipoDesc.Size = new Size(80, 22);
            lblDetalleEquipoDesc.TabIndex = 1;
            lblDetalleEquipoDesc.Text = "Equipo :";
            // 
            // lblDetalleEstadoActual
            // 
            lblDetalleEstadoActual.AutoSize = true;
            lblDetalleEstadoActual.Font = new Font("Arial", 14.25F);
            lblDetalleEstadoActual.Location = new Point(3, 89);
            lblDetalleEstadoActual.Margin = new Padding(3, 9, 3, 9);
            lblDetalleEstadoActual.Name = "lblDetalleEstadoActual";
            lblDetalleEstadoActual.Size = new Size(80, 22);
            lblDetalleEstadoActual.TabIndex = 3;
            lblDetalleEstadoActual.Text = "Estado :";
            // 
            // lblDetalleFalla
            // 
            lblDetalleFalla.AutoSize = true;
            lblDetalleFalla.Font = new Font("Arial", 14.25F);
            lblDetalleFalla.Location = new Point(3, 129);
            lblDetalleFalla.Margin = new Padding(3, 9, 3, 9);
            lblDetalleFalla.Name = "lblDetalleFalla";
            lblDetalleFalla.Size = new Size(60, 22);
            lblDetalleFalla.TabIndex = 2;
            lblDetalleFalla.Text = "Falla :";
            // 
            // lblDetalleHistorial
            // 
            lblDetalleHistorial.AutoSize = true;
            lblDetalleHistorial.Font = new Font("Arial", 14.25F);
            lblDetalleHistorial.Location = new Point(3, 210);
            lblDetalleHistorial.Margin = new Padding(3, 9, 3, 9);
            lblDetalleHistorial.Name = "lblDetalleHistorial";
            lblDetalleHistorial.Size = new Size(87, 22);
            lblDetalleHistorial.TabIndex = 4;
            lblDetalleHistorial.Text = "Historial :";
            // 
            // txtDetalleClienteNombre
            // 
            txtDetalleClienteNombre.BorderStyle = BorderStyle.None;
            txtDetalleClienteNombre.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold);
            txtDetalleClienteNombre.Location = new Point(161, 9);
            txtDetalleClienteNombre.Margin = new Padding(3, 9, 3, 9);
            txtDetalleClienteNombre.Name = "txtDetalleClienteNombre";
            txtDetalleClienteNombre.ReadOnly = true;
            txtDetalleClienteNombre.Size = new Size(206, 20);
            txtDetalleClienteNombre.TabIndex = 5;
            // 
            // txtDetalleEquipoDesc
            // 
            txtDetalleEquipoDesc.BorderStyle = BorderStyle.None;
            txtDetalleEquipoDesc.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold);
            txtDetalleEquipoDesc.Location = new Point(161, 49);
            txtDetalleEquipoDesc.Margin = new Padding(3, 9, 3, 9);
            txtDetalleEquipoDesc.Name = "txtDetalleEquipoDesc";
            txtDetalleEquipoDesc.ReadOnly = true;
            txtDetalleEquipoDesc.Size = new Size(206, 20);
            txtDetalleEquipoDesc.TabIndex = 6;
            // 
            // txtDetalleEstadoActual
            // 
            txtDetalleEstadoActual.BorderStyle = BorderStyle.None;
            txtDetalleEstadoActual.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold);
            txtDetalleEstadoActual.Location = new Point(161, 89);
            txtDetalleEstadoActual.Margin = new Padding(3, 9, 3, 9);
            txtDetalleEstadoActual.Name = "txtDetalleEstadoActual";
            txtDetalleEstadoActual.ReadOnly = true;
            txtDetalleEstadoActual.Size = new Size(206, 20);
            txtDetalleEstadoActual.TabIndex = 7;
            // 
            // txtDetalleFalla
            // 
            txtDetalleFalla.BorderStyle = BorderStyle.None;
            txtDetalleFalla.Dock = DockStyle.Fill;
            txtDetalleFalla.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold);
            txtDetalleFalla.Location = new Point(161, 129);
            txtDetalleFalla.Margin = new Padding(3, 9, 3, 9);
            txtDetalleFalla.Multiline = true;
            txtDetalleFalla.Name = "txtDetalleFalla";
            txtDetalleFalla.ReadOnly = true;
            txtDetalleFalla.ScrollBars = ScrollBars.Vertical;
            txtDetalleFalla.Size = new Size(285, 63);
            txtDetalleFalla.TabIndex = 8;
            // 
            // frmPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 609);
            Controls.Add(dgvOrdenes);
            Controls.Add(pnlDetalles);
            Controls.Add(panel1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "frmPrincipal";
            Text = "Form1";
            Load += frmPrincipal_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrdenes).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            pnlDetalles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHistorial).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem salirToolStripMenuItem;
        private ToolStripMenuItem órdenesToolStripMenuItem;
        private ToolStripMenuItem nuevaOrdenToolStripMenuItem;
        private DataGridView dgvOrdenes;
        private Label lblbuscar;
        private TextBox txtBuscar;
        private Button btnBuscar;
        private Panel panel1;
        private Panel pnlDetalles;
        private Label lblDetalleFalla;
        private Label lblDetalleEquipoDesc;
        private Label lblDetalleClienteNombre;
        private Label lblDetalleHistorial;
        private Label lblDetalleEstadoActual;
        private TableLayoutPanel tableLayoutPanel1;
        private TextBox txtDetalleClienteNombre;
        private TextBox txtDetalleEquipoDesc;
        private TextBox txtDetalleEstadoActual;
        private TextBox txtDetalleFalla;
        private DataGridView dgvHistorial;
    }
}
