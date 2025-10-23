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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGestionOrden));
            panel1 = new Panel();
            label12 = new Label();
            label11 = new Label();
            txtPrecioFinal = new TextBox();
            txtPrecioPresupuesto = new TextBox();
            label10 = new Label();
            cmbEstado = new ComboBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            groupBox3 = new GroupBox();
            txtFallaDeclarada = new TextBox();
            groupBox2 = new GroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            label4 = new Label();
            cmbTipoEquipo = new ComboBox();
            txtMarca = new TextBox();
            txtNumeroSerie = new TextBox();
            label6 = new Label();
            label5 = new Label();
            txtComponentes = new TextBox();
            txtModelo = new TextBox();
            label8 = new Label();
            label7 = new Label();
            groupBox1 = new GroupBox();
            txtDniCliente = new TextBox();
            label9 = new Label();
            btnBuscarCliente = new Button();
            txtEmailCliente = new TextBox();
            txtTelefonoCliente = new TextBox();
            txtNombreCliente = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            groupBox4 = new GroupBox();
            btnQuitarFoto = new Button();
            btnAdjuntarFoto = new Button();
            splitFotos = new SplitContainer();
            lstFotosAdjuntas = new ListBox();
            picFotoPreview = new PictureBox();
            panel1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitFotos).BeginInit();
            splitFotos.Panel1.SuspendLayout();
            splitFotos.Panel2.SuspendLayout();
            splitFotos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picFotoPreview).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label12);
            panel1.Controls.Add(label11);
            panel1.Controls.Add(txtPrecioFinal);
            panel1.Controls.Add(txtPrecioPresupuesto);
            panel1.Controls.Add(label10);
            panel1.Controls.Add(cmbEstado);
            panel1.Controls.Add(btnGuardar);
            panel1.Controls.Add(btnCancelar);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 727);
            panel1.Name = "panel1";
            panel1.Size = new Size(1199, 204);
            panel1.TabIndex = 1;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label12.Location = new Point(122, 130);
            label12.Name = "label12";
            label12.Size = new Size(128, 25);
            label12.TabIndex = 10;
            label12.Text = "Precio FINAL: ";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label11.Location = new Point(125, 80);
            label11.Name = "label11";
            label11.Size = new Size(125, 25);
            label11.TabIndex = 9;
            label11.Text = "Presupuesto :";
            // 
            // txtPrecioFinal
            // 
            txtPrecioFinal.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold);
            txtPrecioFinal.Location = new Point(262, 132);
            txtPrecioFinal.Name = "txtPrecioFinal";
            txtPrecioFinal.Size = new Size(216, 33);
            txtPrecioFinal.TabIndex = 8;
            // 
            // txtPrecioPresupuesto
            // 
            txtPrecioPresupuesto.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold);
            txtPrecioPresupuesto.Location = new Point(262, 80);
            txtPrecioPresupuesto.Name = "txtPrecioPresupuesto";
            txtPrecioPresupuesto.Size = new Size(216, 33);
            txtPrecioPresupuesto.TabIndex = 7;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label10.Location = new Point(74, 27);
            label10.Name = "label10";
            label10.Size = new Size(173, 25);
            label10.TabIndex = 6;
            label10.Text = "Estado del trabajo :";
            // 
            // cmbEstado
            // 
            cmbEstado.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cmbEstado.FormattingEnabled = true;
            cmbEstado.Location = new Point(260, 27);
            cmbEstado.Name = "cmbEstado";
            cmbEstado.Size = new Size(216, 33);
            cmbEstado.TabIndex = 5;
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnGuardar.Location = new Point(865, 110);
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
            btnCancelar.Location = new Point(716, 110);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(111, 55);
            btnCancelar.TabIndex = 4;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(txtFallaDeclarada);
            groupBox3.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox3.Location = new Point(3, 632);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(1178, 89);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Falla Declarada";
            // 
            // txtFallaDeclarada
            // 
            txtFallaDeclarada.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtFallaDeclarada.Location = new Point(13, 45);
            txtFallaDeclarada.Multiline = true;
            txtFallaDeclarada.Name = "txtFallaDeclarada";
            txtFallaDeclarada.Size = new Size(1156, 38);
            txtFallaDeclarada.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tableLayoutPanel1);
            groupBox2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(3, 222);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1184, 209);
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
            tableLayoutPanel1.Controls.Add(txtModelo, 1, 3);
            tableLayoutPanel1.Controls.Add(label8, 0, 3);
            tableLayoutPanel1.Controls.Add(label7, 2, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 29);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Size = new Size(1178, 177);
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
            cmbTipoEquipo.Location = new Point(238, 3);
            cmbTipoEquipo.Name = "cmbTipoEquipo";
            cmbTipoEquipo.Size = new Size(347, 33);
            cmbTipoEquipo.TabIndex = 1;
            // 
            // txtMarca
            // 
            txtMarca.Dock = DockStyle.Fill;
            txtMarca.Location = new Point(238, 47);
            txtMarca.Name = "txtMarca";
            txtMarca.Size = new Size(347, 33);
            txtMarca.TabIndex = 3;
            // 
            // txtNumeroSerie
            // 
            txtNumeroSerie.Dock = DockStyle.Fill;
            txtNumeroSerie.Location = new Point(238, 91);
            txtNumeroSerie.Name = "txtNumeroSerie";
            txtNumeroSerie.Size = new Size(347, 33);
            txtNumeroSerie.TabIndex = 4;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(3, 88);
            label6.Name = "label6";
            label6.Size = new Size(126, 25);
            label6.TabIndex = 5;
            label6.Text = "Nro de Serie :";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(3, 44);
            label5.Name = "label5";
            label5.Size = new Size(79, 25);
            label5.TabIndex = 2;
            label5.Text = "Marca : ";
            // 
            // txtComponentes
            // 
            txtComponentes.Dock = DockStyle.Fill;
            txtComponentes.Location = new Point(826, 3);
            txtComponentes.Multiline = true;
            txtComponentes.Name = "txtComponentes";
            tableLayoutPanel1.SetRowSpan(txtComponentes, 4);
            txtComponentes.Size = new Size(349, 171);
            txtComponentes.TabIndex = 7;
            // 
            // txtModelo
            // 
            txtModelo.Dock = DockStyle.Fill;
            txtModelo.Location = new Point(238, 135);
            txtModelo.Name = "txtModelo";
            txtModelo.Size = new Size(347, 33);
            txtModelo.TabIndex = 8;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(3, 132);
            label8.Name = "label8";
            label8.Size = new Size(86, 25);
            label8.TabIndex = 9;
            label8.Text = "Modelo :";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(591, 0);
            label7.Name = "label7";
            label7.Size = new Size(143, 25);
            label7.TabIndex = 6;
            label7.Text = "Componentes : ";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtDniCliente);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(btnBuscarCliente);
            groupBox1.Controls.Add(txtEmailCliente);
            groupBox1.Controls.Add(txtTelefonoCliente);
            groupBox1.Controls.Add(txtNombreCliente);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1184, 213);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Datos del Cliente";
            // 
            // txtDniCliente
            // 
            txtDniCliente.Location = new Point(193, 174);
            txtDniCliente.Name = "txtDniCliente";
            txtDniCliente.Size = new Size(384, 33);
            txtDniCliente.TabIndex = 8;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.Location = new Point(125, 174);
            label9.Name = "label9";
            label9.Size = new Size(53, 25);
            label9.TabIndex = 7;
            label9.Text = "DNI :";
            // 
            // btnBuscarCliente
            // 
            btnBuscarCliente.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnBuscarCliente.Location = new Point(769, 67);
            btnBuscarCliente.Name = "btnBuscarCliente";
            btnBuscarCliente.Size = new Size(320, 92);
            btnBuscarCliente.TabIndex = 6;
            btnBuscarCliente.Text = "Buscar Cliente";
            btnBuscarCliente.UseVisualStyleBackColor = true;
            btnBuscarCliente.Click += btnBuscarCliente_Click;
            // 
            // txtEmailCliente
            // 
            txtEmailCliente.Location = new Point(195, 126);
            txtEmailCliente.Name = "txtEmailCliente";
            txtEmailCliente.Size = new Size(384, 33);
            txtEmailCliente.TabIndex = 5;
            // 
            // txtTelefonoCliente
            // 
            txtTelefonoCliente.Location = new Point(193, 78);
            txtTelefonoCliente.Name = "txtTelefonoCliente";
            txtTelefonoCliente.Size = new Size(384, 33);
            txtTelefonoCliente.TabIndex = 4;
            // 
            // txtNombreCliente
            // 
            txtNombreCliente.Location = new Point(195, 25);
            txtNombreCliente.Name = "txtNombreCliente";
            txtNombreCliente.Size = new Size(384, 33);
            txtNombreCliente.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14.25F);
            label3.Location = new Point(108, 134);
            label3.Name = "label3";
            label3.Size = new Size(72, 25);
            label3.TabIndex = 2;
            label3.Text = "Email : ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F);
            label2.Location = new Point(89, 86);
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
            label1.Size = new Size(174, 25);
            label1.TabIndex = 0;
            label1.Text = "Nombre y Apellido:";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(groupBox1);
            flowLayoutPanel1.Controls.Add(groupBox2);
            flowLayoutPanel1.Controls.Add(groupBox4);
            flowLayoutPanel1.Controls.Add(groupBox3);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1199, 727);
            flowLayoutPanel1.TabIndex = 1;
            flowLayoutPanel1.WrapContents = false;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(btnQuitarFoto);
            groupBox4.Controls.Add(btnAdjuntarFoto);
            groupBox4.Controls.Add(splitFotos);
            groupBox4.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            groupBox4.Location = new Point(3, 437);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(1184, 189);
            groupBox4.TabIndex = 1;
            groupBox4.TabStop = false;
            groupBox4.Text = "Fotos Adjuntas";
            // 
            // btnQuitarFoto
            // 
            btnQuitarFoto.Location = new Point(272, 147);
            btnQuitarFoto.Name = "btnQuitarFoto";
            btnQuitarFoto.Size = new Size(253, 38);
            btnQuitarFoto.TabIndex = 2;
            btnQuitarFoto.Text = "Eliminar Foto";
            btnQuitarFoto.UseVisualStyleBackColor = true;
            btnQuitarFoto.Click += btnQuitarFoto_Click;
            // 
            // btnAdjuntarFoto
            // 
            btnAdjuntarFoto.Location = new Point(3, 147);
            btnAdjuntarFoto.Name = "btnAdjuntarFoto";
            btnAdjuntarFoto.Size = new Size(253, 38);
            btnAdjuntarFoto.TabIndex = 1;
            btnAdjuntarFoto.Text = "Adjuntar Foto";
            btnAdjuntarFoto.UseVisualStyleBackColor = true;
            btnAdjuntarFoto.Click += btnAdjuntarFoto_Click;
            // 
            // splitFotos
            // 
            splitFotos.Dock = DockStyle.Top;
            splitFotos.Location = new Point(3, 29);
            splitFotos.Name = "splitFotos";
            // 
            // splitFotos.Panel1
            // 
            splitFotos.Panel1.Controls.Add(lstFotosAdjuntas);
            // 
            // splitFotos.Panel2
            // 
            splitFotos.Panel2.Controls.Add(picFotoPreview);
            splitFotos.Size = new Size(1178, 112);
            splitFotos.SplitterDistance = 392;
            splitFotos.TabIndex = 0;
            // 
            // lstFotosAdjuntas
            // 
            lstFotosAdjuntas.Dock = DockStyle.Fill;
            lstFotosAdjuntas.FormattingEnabled = true;
            lstFotosAdjuntas.ItemHeight = 25;
            lstFotosAdjuntas.Location = new Point(0, 0);
            lstFotosAdjuntas.Name = "lstFotosAdjuntas";
            lstFotosAdjuntas.Size = new Size(392, 112);
            lstFotosAdjuntas.TabIndex = 0;
            lstFotosAdjuntas.SelectedIndexChanged += lstFotosAdjuntas_SelectedIndexChanged;
            // 
            // picFotoPreview
            // 
            picFotoPreview.BorderStyle = BorderStyle.FixedSingle;
            picFotoPreview.Dock = DockStyle.Fill;
            picFotoPreview.Location = new Point(0, 0);
            picFotoPreview.Name = "picFotoPreview";
            picFotoPreview.Size = new Size(782, 112);
            picFotoPreview.SizeMode = PictureBoxSizeMode.Zoom;
            picFotoPreview.TabIndex = 0;
            picFotoPreview.TabStop = false;
            picFotoPreview.Click += picFotoPreview_Click;
            // 
            // frmGestionOrden
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1199, 931);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmGestionOrden";
            Text = "frmGestionOrden";
            Load += frmGestionOrden_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            splitFotos.Panel1.ResumeLayout(false);
            splitFotos.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitFotos).EndInit();
            splitFotos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picFotoPreview).EndInit();
            ResumeLayout(false);
        }

        #endregion

        // --- ESTA SECCIÓN ES LA QUE FALTABA ---
        // Aquí se declaran las variables para que el archivo .cs las vea
        private Panel panel1;
        private Button btnGuardar;
        private Button btnCancelar;
        private GroupBox groupBox3;
        private TextBox txtFallaDeclarada;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label4;
        private ComboBox cmbTipoEquipo;
        private TextBox txtMarca;
        private TextBox txtNumeroSerie;
        private Label label6;
        private Label label5;
        private TextBox txtComponentes;
        private TextBox txtModelo;
        private Label label8;
        private Label label7;
        private GroupBox groupBox1;
        private TextBox txtDniCliente;
        private Label label9;
        private Button btnBuscarCliente;
        private TextBox txtEmailCliente;
        private TextBox txtTelefonoCliente;
        private TextBox txtNombreCliente;
        private Label label3;
        private Label label2;
        private Label label1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label10;
        private ComboBox cmbEstado;
        private Label label12;
        private Label label11;
        private TextBox txtPrecioFinal;
        private TextBox txtPrecioPresupuesto;
        private GroupBox groupBox4;
        private SplitContainer splitFotos;
        private Button btnAdjuntarFoto;
        private Button btnQuitarFoto;
        private ListBox lstFotosAdjuntas;
        private PictureBox picFotoPreview;
    }
}