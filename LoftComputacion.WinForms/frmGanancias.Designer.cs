namespace LoftComputacion.WinForms
{
    partial class frmGanancias
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
            pnlFiltros = new Panel();
            btnFiltrarGanancias = new Button();
            label3 = new Label();
            dtpFechaHasta = new DateTimePicker();
            dtpFechaDesde = new DateTimePicker();
            label2 = new Label();
            label1 = new Label();
            pnlTotales = new Panel();
            btnDescargarResumen = new Button();
            lblTotalGanancias = new Label();
            dgvGanancias = new DataGridView();
            pnlFiltros.SuspendLayout();
            pnlTotales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvGanancias).BeginInit();
            SuspendLayout();
            // 
            // pnlFiltros
            // 
            pnlFiltros.Controls.Add(btnFiltrarGanancias);
            pnlFiltros.Controls.Add(label3);
            pnlFiltros.Controls.Add(dtpFechaHasta);
            pnlFiltros.Controls.Add(dtpFechaDesde);
            pnlFiltros.Controls.Add(label2);
            pnlFiltros.Controls.Add(label1);
            pnlFiltros.Dock = DockStyle.Top;
            pnlFiltros.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            pnlFiltros.Location = new Point(0, 0);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Size = new Size(800, 149);
            pnlFiltros.TabIndex = 0;
            // 
            // btnFiltrarGanancias
            // 
            btnFiltrarGanancias.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFiltrarGanancias.Location = new Point(681, 95);
            btnFiltrarGanancias.Name = "btnFiltrarGanancias";
            btnFiltrarGanancias.Size = new Size(107, 48);
            btnFiltrarGanancias.TabIndex = 5;
            btnFiltrarGanancias.Text = "Filtrar";
            btnFiltrarGanancias.UseVisualStyleBackColor = true;
            btnFiltrarGanancias.Click += btnFiltrarGanancias_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(30, 9);
            label3.Name = "label3";
            label3.Size = new Size(138, 32);
            label3.TabIndex = 4;
            label3.Text = "Ganancias :";
            // 
            // dtpFechaHasta
            // 
            dtpFechaHasta.Font = new Font("Arial Narrow", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dtpFechaHasta.Location = new Point(367, 101);
            dtpFechaHasta.Name = "dtpFechaHasta";
            dtpFechaHasta.Size = new Size(307, 29);
            dtpFechaHasta.TabIndex = 3;
            // 
            // dtpFechaDesde
            // 
            dtpFechaDesde.Font = new Font("Arial Narrow", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dtpFechaDesde.Location = new Point(30, 99);
            dtpFechaDesde.Name = "dtpFechaDesde";
            dtpFechaDesde.Size = new Size(306, 29);
            dtpFechaDesde.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            label2.Location = new Point(367, 66);
            label2.Name = "label2";
            label2.Size = new Size(90, 32);
            label2.TabIndex = 1;
            label2.Text = "Hasta :";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            label1.Location = new Point(30, 64);
            label1.Name = "label1";
            label1.Size = new Size(94, 32);
            label1.TabIndex = 0;
            label1.Text = "Desde :";
            // 
            // pnlTotales
            // 
            pnlTotales.Controls.Add(btnDescargarResumen);
            pnlTotales.Controls.Add(lblTotalGanancias);
            pnlTotales.Dock = DockStyle.Bottom;
            pnlTotales.Location = new Point(0, 350);
            pnlTotales.Name = "pnlTotales";
            pnlTotales.Size = new Size(800, 100);
            pnlTotales.TabIndex = 1;
            // 
            // btnDescargarResumen
            // 
            btnDescargarResumen.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDescargarResumen.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDescargarResumen.Location = new Point(12, 33);
            btnDescargarResumen.Name = "btnDescargarResumen";
            btnDescargarResumen.Size = new Size(155, 52);
            btnDescargarResumen.TabIndex = 3;
            btnDescargarResumen.Text = "Descargar Resumen";
            btnDescargarResumen.UseVisualStyleBackColor = true;
            btnDescargarResumen.Click += btnDescargarResumen_Click;
            // 
            // lblTotalGanancias
            // 
            lblTotalGanancias.AutoSize = true;
            lblTotalGanancias.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTotalGanancias.Location = new Point(600, 53);
            lblTotalGanancias.Name = "lblTotalGanancias";
            lblTotalGanancias.Size = new Size(141, 32);
            lblTotalGanancias.TabIndex = 2;
            lblTotalGanancias.Text = "Total : $0.00";
            // 
            // dgvGanancias
            // 
            dgvGanancias.AllowUserToAddRows = false;
            dgvGanancias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvGanancias.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvGanancias.Dock = DockStyle.Fill;
            dgvGanancias.Location = new Point(0, 149);
            dgvGanancias.Name = "dgvGanancias";
            dgvGanancias.ReadOnly = true;
            dgvGanancias.Size = new Size(800, 201);
            dgvGanancias.TabIndex = 2;
            // 
            // frmGanancias
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dgvGanancias);
            Controls.Add(pnlTotales);
            Controls.Add(pnlFiltros);
            Name = "frmGanancias";
            Text = "frmGanancias";
            Load += frmGanancias_Load;
            pnlFiltros.ResumeLayout(false);
            pnlFiltros.PerformLayout();
            pnlTotales.ResumeLayout(false);
            pnlTotales.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvGanancias).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlFiltros;
        private Label label3;
        private DateTimePicker dtpFechaHasta;
        private DateTimePicker dtpFechaDesde;
        private Label label2;
        private Label label1;
        private Panel pnlTotales;
        private Button btnFiltrarGanancias;
        private Button btnDescargarResumen;
        private Label lblTotalGanancias;
        private DataGridView dgvGanancias;
    }
}