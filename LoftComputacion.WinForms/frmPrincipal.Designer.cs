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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrincipal));
            menuStrip1 = new MenuStrip();
            toolStripMenuItem1 = new ToolStripMenuItem();
            salirToolStripMenuItem = new ToolStripMenuItem();
            órdenesToolStripMenuItem = new ToolStripMenuItem();
            nuevaOrdenToolStripMenuItem = new ToolStripMenuItem();
            administraciónToolStripMenuItem = new ToolStripMenuItem();
            dgvOrdenes = new DataGridView();
            cmsOrdenes = new ContextMenuStrip(components);
            tsmiCambiarEstado = new ToolStripMenuItem();
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
            lblDetalleHistorial = new Label();
            txtDetalleClienteNombre = new TextBox();
            txtDetalleEquipoDesc = new TextBox();
            txtDetalleEstadoActual = new TextBox();
            txtDetalleFalla = new TextBox();
            lblDetalleFalla = new Label();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrdenes).BeginInit();
            cmsOrdenes.SuspendLayout();
            panel1.SuspendLayout();
            pnlDetalles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistorial).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Font = new Font("Verdana", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, órdenesToolStripMenuItem, administraciónToolStripMenuItem });
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
            // administraciónToolStripMenuItem
            // 
            administraciónToolStripMenuItem.Name = "administraciónToolStripMenuItem";
            administraciónToolStripMenuItem.Size = new Size(174, 29);
            administraciónToolStripMenuItem.Text = "Administración";
            administraciónToolStripMenuItem.Click += administraciónToolStripMenuItem_Click;
            // 
            // dgvOrdenes
            // 
            dgvOrdenes.AllowUserToAddRows = false;
            dgvOrdenes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrdenes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrdenes.ContextMenuStrip = cmsOrdenes;
            dgvOrdenes.Dock = DockStyle.Fill;
            dgvOrdenes.Location = new Point(0, 92);
            dgvOrdenes.Name = "dgvOrdenes";
            dgvOrdenes.Size = new Size(435, 599);
            dgvOrdenes.TabIndex = 1;
            dgvOrdenes.CellDoubleClick += dgvOrdenes_CellDoubleClick;
            dgvOrdenes.CellFormatting += dgvOrdenes_CellFormatting;
            dgvOrdenes.ColumnHeaderMouseClick += dgvOrdenes_ColumnHeaderMouseClick;
            dgvOrdenes.SelectionChanged += dgvOrdenes_SelectionChanged;
            // 
            // cmsOrdenes
            // 
            cmsOrdenes.Items.AddRange(new ToolStripItem[] { tsmiCambiarEstado });
            cmsOrdenes.Name = "cmsOrdenes";
            cmsOrdenes.Size = new Size(158, 26);
            // 
            // tsmiCambiarEstado
            // 
            tsmiCambiarEstado.Name = "tsmiCambiarEstado";
            tsmiCambiarEstado.Size = new Size(157, 22);
            tsmiCambiarEstado.Text = "Cambiar Estado";
            tsmiCambiarEstado.Click += tsmiCambiarEstado_Click;
            // 
            // lblbuscar
            // 
            lblbuscar.AutoSize = true;
            lblbuscar.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblbuscar.Location = new Point(12, 17);
            lblbuscar.Name = "lblbuscar";
            lblbuscar.Size = new Size(52, 20);
            lblbuscar.TabIndex = 2;
            lblbuscar.Text = "Buscar";
            // 
            // txtBuscar
            // 
            txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBuscar.Location = new Point(70, 17);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(721, 25);
            txtBuscar.TabIndex = 3;
            txtBuscar.KeyDown += txtBuscar_KeyDown;
            // 
            // btnBuscar
            // 
            btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBuscar.Location = new Point(797, 9);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(75, 40);
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
            panel1.Size = new Size(884, 59);
            panel1.TabIndex = 5;
            // 
            // pnlDetalles
            // 
            pnlDetalles.Controls.Add(dgvHistorial);
            pnlDetalles.Controls.Add(tableLayoutPanel1);
            pnlDetalles.Dock = DockStyle.Right;
            pnlDetalles.Location = new Point(435, 92);
            pnlDetalles.Name = "pnlDetalles";
            pnlDetalles.Size = new Size(449, 599);
            pnlDetalles.TabIndex = 6;
            // 
            // dgvHistorial
            // 
            dgvHistorial.AllowUserToAddRows = false;
            dgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistorial.BorderStyle = BorderStyle.Fixed3D;
            dgvHistorial.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistorial.Dock = DockStyle.Fill;
            dgvHistorial.Location = new Point(0, 270);
            dgvHistorial.Name = "dgvHistorial";
            dgvHistorial.ReadOnly = true;
            dgvHistorial.RowHeadersVisible = false;
            dgvHistorial.Size = new Size(449, 329);
            dgvHistorial.TabIndex = 9;
            dgvHistorial.CellFormatting += dgvHistorial_CellFormatting;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(lblDetalleClienteNombre, 0, 0);
            tableLayoutPanel1.Controls.Add(lblDetalleEquipoDesc, 0, 1);
            tableLayoutPanel1.Controls.Add(lblDetalleEstadoActual, 0, 2);
            tableLayoutPanel1.Controls.Add(lblDetalleHistorial, 0, 4);
            tableLayoutPanel1.Controls.Add(txtDetalleClienteNombre, 1, 0);
            tableLayoutPanel1.Controls.Add(txtDetalleEquipoDesc, 1, 1);
            tableLayoutPanel1.Controls.Add(txtDetalleEstadoActual, 1, 2);
            tableLayoutPanel1.Controls.Add(txtDetalleFalla, 1, 3);
            tableLayoutPanel1.Controls.Add(lblDetalleFalla, 0, 3);
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
            tableLayoutPanel1.Size = new Size(449, 270);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // lblDetalleClienteNombre
            // 
            lblDetalleClienteNombre.AutoSize = true;
            lblDetalleClienteNombre.Font = new Font("Arial", 14.25F);
            lblDetalleClienteNombre.Location = new Point(4, 12);
            lblDetalleClienteNombre.Margin = new Padding(3, 11, 3, 11);
            lblDetalleClienteNombre.Name = "lblDetalleClienteNombre";
            lblDetalleClienteNombre.Size = new Size(79, 22);
            lblDetalleClienteNombre.TabIndex = 0;
            lblDetalleClienteNombre.Text = "Cliente :";
            // 
            // lblDetalleEquipoDesc
            // 
            lblDetalleEquipoDesc.AutoSize = true;
            lblDetalleEquipoDesc.Font = new Font("Arial", 14.25F);
            lblDetalleEquipoDesc.Location = new Point(4, 57);
            lblDetalleEquipoDesc.Margin = new Padding(3, 11, 3, 11);
            lblDetalleEquipoDesc.Name = "lblDetalleEquipoDesc";
            lblDetalleEquipoDesc.Size = new Size(80, 22);
            lblDetalleEquipoDesc.TabIndex = 1;
            lblDetalleEquipoDesc.Text = "Equipo :";
            // 
            // lblDetalleEstadoActual
            // 
            lblDetalleEstadoActual.AutoSize = true;
            lblDetalleEstadoActual.Font = new Font("Arial", 14.25F);
            lblDetalleEstadoActual.Location = new Point(4, 102);
            lblDetalleEstadoActual.Margin = new Padding(3, 11, 3, 11);
            lblDetalleEstadoActual.Name = "lblDetalleEstadoActual";
            lblDetalleEstadoActual.Size = new Size(80, 22);
            lblDetalleEstadoActual.TabIndex = 3;
            lblDetalleEstadoActual.Text = "Estado :";
            // 
            // lblDetalleHistorial
            // 
            lblDetalleHistorial.AutoSize = true;
            lblDetalleHistorial.Font = new Font("Arial", 14.25F);
            lblDetalleHistorial.Location = new Point(4, 241);
            lblDetalleHistorial.Margin = new Padding(3, 11, 3, 11);
            lblDetalleHistorial.Name = "lblDetalleHistorial";
            lblDetalleHistorial.Size = new Size(87, 22);
            lblDetalleHistorial.TabIndex = 4;
            lblDetalleHistorial.Text = "Historial :";
            // 
            // txtDetalleClienteNombre
            // 
            txtDetalleClienteNombre.BorderStyle = BorderStyle.None;
            txtDetalleClienteNombre.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold);
            txtDetalleClienteNombre.Location = new Point(98, 12);
            txtDetalleClienteNombre.Margin = new Padding(3, 11, 3, 11);
            txtDetalleClienteNombre.Name = "txtDetalleClienteNombre";
            txtDetalleClienteNombre.ReadOnly = true;
            txtDetalleClienteNombre.Size = new Size(206, 20);
            txtDetalleClienteNombre.TabIndex = 5;
            // 
            // txtDetalleEquipoDesc
            // 
            txtDetalleEquipoDesc.BorderStyle = BorderStyle.None;
            txtDetalleEquipoDesc.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold);
            txtDetalleEquipoDesc.Location = new Point(98, 57);
            txtDetalleEquipoDesc.Margin = new Padding(3, 11, 3, 11);
            txtDetalleEquipoDesc.Name = "txtDetalleEquipoDesc";
            txtDetalleEquipoDesc.ReadOnly = true;
            txtDetalleEquipoDesc.Size = new Size(206, 20);
            txtDetalleEquipoDesc.TabIndex = 6;
            // 
            // txtDetalleEstadoActual
            // 
            txtDetalleEstadoActual.BorderStyle = BorderStyle.None;
            txtDetalleEstadoActual.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold);
            txtDetalleEstadoActual.Location = new Point(98, 102);
            txtDetalleEstadoActual.Margin = new Padding(3, 11, 3, 11);
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
            txtDetalleFalla.Location = new Point(98, 147);
            txtDetalleFalla.Margin = new Padding(3, 11, 3, 11);
            txtDetalleFalla.Multiline = true;
            txtDetalleFalla.Name = "txtDetalleFalla";
            txtDetalleFalla.ReadOnly = true;
            txtDetalleFalla.ScrollBars = ScrollBars.Vertical;
            txtDetalleFalla.Size = new Size(347, 71);
            txtDetalleFalla.TabIndex = 8;
            // 
            // lblDetalleFalla
            // 
            lblDetalleFalla.AutoSize = true;
            lblDetalleFalla.Font = new Font("Arial", 14.25F);
            lblDetalleFalla.Location = new Point(7, 147);
            lblDetalleFalla.Margin = new Padding(6, 11, 3, 11);
            lblDetalleFalla.Name = "lblDetalleFalla";
            lblDetalleFalla.Size = new Size(60, 22);
            lblDetalleFalla.TabIndex = 2;
            lblDetalleFalla.Text = "Falla :";
            // 
            // frmPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 691);
            Controls.Add(dgvOrdenes);
            Controls.Add(pnlDetalles);
            Controls.Add(panel1);
            Controls.Add(menuStrip1);
            Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "frmPrincipal";
            Text = "Menu Principal";
            WindowState = FormWindowState.Maximized;
            Load += frmPrincipal_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrdenes).EndInit();
            cmsOrdenes.ResumeLayout(false);
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
        private ContextMenuStrip cmsOrdenes;
        private ToolStripMenuItem tsmiCambiarEstado;
        private ToolStripMenuItem administraciónToolStripMenuItem;
    }
}
