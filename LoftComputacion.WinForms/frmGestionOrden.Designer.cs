namespace LoftComputacion.WinForms
{
    partial class frmGestionOrden
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
            groupBox1 = new GroupBox();
            button1 = new Button();
            txtEmailCliente = new TextBox();
            txtTelefonoCliente = new TextBox();
            txtNombreCliente = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            groupBox2 = new GroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            label4 = new Label();
            cmbTipoEquipo = new ComboBox();
            txtMarca = new TextBox();
            txtNumeroSerie = new TextBox();
            label6 = new Label();
            label5 = new Label();
            txtComponentes = new TextBox();
            textBox3 = new TextBox();
            label8 = new Label();
            label7 = new Label();
            groupBox3 = new GroupBox();
            txtFallaDeclarada = new TextBox();
            panel1 = new Panel();
            btnGuardar = new Button();
            btnCancelar = new Button();
            groupBox1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox3.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(txtEmailCliente);
            groupBox1.Controls.Add(txtTelefonoCliente);
            groupBox1.Controls.Add(txtNombreCliente);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(954, 181);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Datos del Cliente";
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.Location = new Point(657, 78);
            button1.Name = "button1";
            button1.Size = new Size(225, 44);
            button1.TabIndex = 6;
            button1.Text = "Buscar Cliente";
            button1.UseVisualStyleBackColor = true;
            // 
            // txtEmailCliente
            // 
            txtEmailCliente.Location = new Point(193, 134);
            txtEmailCliente.Name = "txtEmailCliente";
            txtEmailCliente.Size = new Size(384, 33);
            txtEmailCliente.TabIndex = 5;
            // 
            // txtTelefonoCliente
            // 
            txtTelefonoCliente.Location = new Point(193, 85);
            txtTelefonoCliente.Name = "txtTelefonoCliente";
            txtTelefonoCliente.Size = new Size(384, 33);
            txtTelefonoCliente.TabIndex = 4;
            // 
            // txtNombreCliente
            // 
            txtNombreCliente.Location = new Point(193, 35);
            txtNombreCliente.Name = "txtNombreCliente";
            txtNombreCliente.Size = new Size(384, 33);
            txtNombreCliente.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14.25F);
            label3.Location = new Point(6, 134);
            label3.Name = "label3";
            label3.Size = new Size(72, 25);
            label3.TabIndex = 2;
            label3.Text = "Email : ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F);
            label2.Location = new Point(6, 80);
            label2.Name = "label2";
            label2.Size = new Size(98, 25);
            label2.TabIndex = 1;
            label2.Text = "Telefono : ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F);
            label1.Location = new Point(6, 33);
            label1.Name = "label1";
            label1.Size = new Size(182, 25);
            label1.TabIndex = 0;
            label1.Text = "Nombre Completo : ";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(groupBox1);
            flowLayoutPanel1.Controls.Add(groupBox2);
            flowLayoutPanel1.Controls.Add(groupBox3);
            flowLayoutPanel1.Controls.Add(panel1);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1073, 657);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tableLayoutPanel1);
            groupBox2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(3, 190);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(954, 265);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Datos del Equipo";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.Controls.Add(label4, 0, 0);
            tableLayoutPanel1.Controls.Add(cmbTipoEquipo, 1, 0);
            tableLayoutPanel1.Controls.Add(txtMarca, 1, 1);
            tableLayoutPanel1.Controls.Add(txtNumeroSerie, 1, 2);
            tableLayoutPanel1.Controls.Add(label6, 0, 2);
            tableLayoutPanel1.Controls.Add(label5, 0, 1);
            tableLayoutPanel1.Controls.Add(txtComponentes, 3, 0);
            tableLayoutPanel1.Controls.Add(textBox3, 1, 3);
            tableLayoutPanel1.Controls.Add(label8, 0, 3);
            tableLayoutPanel1.Controls.Add(label7, 2, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 29);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(948, 233);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(3, 0);
            label4.Name = "label4";
            label4.Size = new Size(153, 25);
            label4.TabIndex = 0;
            label4.Text = "Tipo de Equipo : ";
            // 
            // cmbTipoEquipo
            // 
            cmbTipoEquipo.Dock = DockStyle.Fill;
            cmbTipoEquipo.FormattingEnabled = true;
            cmbTipoEquipo.Location = new Point(192, 3);
            cmbTipoEquipo.Name = "cmbTipoEquipo";
            cmbTipoEquipo.Size = new Size(278, 33);
            cmbTipoEquipo.TabIndex = 1;
            // 
            // txtMarca
            // 
            txtMarca.Dock = DockStyle.Fill;
            txtMarca.Location = new Point(192, 42);
            txtMarca.Name = "txtMarca";
            txtMarca.Size = new Size(278, 33);
            txtMarca.TabIndex = 3;
            // 
            // txtNumeroSerie
            // 
            txtNumeroSerie.Dock = DockStyle.Fill;
            txtNumeroSerie.Location = new Point(192, 81);
            txtNumeroSerie.Name = "txtNumeroSerie";
            txtNumeroSerie.Size = new Size(278, 33);
            txtNumeroSerie.TabIndex = 4;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(3, 78);
            label6.Name = "label6";
            label6.Size = new Size(126, 25);
            label6.TabIndex = 5;
            label6.Text = "Nro de Serie :";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(3, 39);
            label5.Name = "label5";
            label5.Size = new Size(79, 25);
            label5.TabIndex = 2;
            label5.Text = "Marca : ";
            // 
            // txtComponentes
            // 
            txtComponentes.Dock = DockStyle.Fill;
            txtComponentes.Location = new Point(665, 3);
            txtComponentes.Multiline = true;
            txtComponentes.Name = "txtComponentes";
            tableLayoutPanel1.SetRowSpan(txtComponentes, 4);
            txtComponentes.Size = new Size(280, 227);
            txtComponentes.TabIndex = 7;
            // 
            // textBox3
            // 
            textBox3.Dock = DockStyle.Fill;
            textBox3.Location = new Point(192, 120);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(278, 33);
            textBox3.TabIndex = 8;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(3, 117);
            label8.Name = "label8";
            label8.Size = new Size(86, 25);
            label8.TabIndex = 9;
            label8.Text = "Modelo :";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(476, 78);
            label7.Name = "label7";
            label7.Size = new Size(143, 25);
            label7.TabIndex = 6;
            label7.Text = "Componentes : ";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(txtFallaDeclarada);
            groupBox3.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox3.Location = new Point(3, 461);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(954, 124);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Falla Declarada";
            // 
            // txtFallaDeclarada
            // 
            txtFallaDeclarada.Location = new Point(13, 45);
            txtFallaDeclarada.Name = "txtFallaDeclarada";
            txtFallaDeclarada.Size = new Size(932, 33);
            txtFallaDeclarada.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnGuardar);
            panel1.Controls.Add(btnCancelar);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(3, 591);
            panel1.Name = "panel1";
            panel1.Size = new Size(954, 58);
            panel1.TabIndex = 5;
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnGuardar.Location = new Point(686, 3);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(112, 55);
            btnGuardar.TabIndex = 3;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancelar.Location = new Point(840, 3);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(111, 55);
            btnCancelar.TabIndex = 4;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // frmGestionOrden
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1073, 657);
            Controls.Add(flowLayoutPanel1);
            Name = "frmGestionOrden";
            Text = "frmGestionOrden";
            Load += frmGestionOrden_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label label3;
        private Label label2;
        private Label label1;
        private FlowLayoutPanel flowLayoutPanel1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private TextBox txtEmailCliente;
        private TextBox txtTelefonoCliente;
        private TextBox txtNombreCliente;
        private Button button1;
        private TextBox txtFallaDeclarada;
        private Button btnGuardar;
        private Button btnCancelar;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label4;
        private ComboBox cmbTipoEquipo;
        private Label label5;
        private TextBox txtMarca;
        private TextBox txtNumeroSerie;
        private Label label6;
        private Label label7;
        private TextBox txtComponentes;
        private TextBox textBox3;
        private Label label8;
    }
}