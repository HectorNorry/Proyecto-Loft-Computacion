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
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrdenes).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Font = new Font("Verdana", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, órdenesToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 33);
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
            dgvOrdenes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrdenes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrdenes.Dock = DockStyle.Fill;
            dgvOrdenes.Location = new Point(0, 85);
            dgvOrdenes.Name = "dgvOrdenes";
            dgvOrdenes.Size = new Size(800, 365);
            dgvOrdenes.TabIndex = 1;
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
            txtBuscar.Size = new Size(637, 23);
            txtBuscar.TabIndex = 3;
            // 
            // btnBuscar
            // 
            btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBuscar.Location = new Point(713, 8);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(75, 35);
            btnBuscar.TabIndex = 4;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(lblbuscar);
            panel1.Controls.Add(btnBuscar);
            panel1.Controls.Add(txtBuscar);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 33);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 52);
            panel1.TabIndex = 5;
            // 
            // frmPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dgvOrdenes);
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
    }
}
