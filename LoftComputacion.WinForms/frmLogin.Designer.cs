namespace LoftComputacion.WinForms
{
    partial class frmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtUsuario = new TextBox();
            txtPassword = new TextBox();
            btnIngresar = new Button();
            btnCancelar = new Button();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(315, 208);
            label1.Name = "label1";
            label1.Size = new Size(173, 40);
            label1.TabIndex = 0;
            label1.Text = "Inicie Sesion";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(189, 277);
            label2.Name = "label2";
            label2.Size = new Size(168, 30);
            label2.TabIndex = 1;
            label2.Text = "Usuario o Email :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(228, 327);
            label3.Name = "label3";
            label3.Size = new Size(129, 30);
            label3.TabIndex = 2;
            label3.Text = "Contraseña :";
            // 
            // txtUsuario
            // 
            txtUsuario.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUsuario.Location = new Point(363, 274);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(229, 33);
            txtUsuario.TabIndex = 3;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPassword.Location = new Point(363, 324);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(229, 33);
            txtPassword.TabIndex = 4;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // btnIngresar
            // 
            btnIngresar.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnIngresar.Location = new Point(250, 386);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(147, 50);
            btnIngresar.TabIndex = 5;
            btnIngresar.Text = "Ingresar";
            btnIngresar.UseVisualStyleBackColor = true;
            btnIngresar.Click += btnIngresar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancelar.Location = new Point(456, 388);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(136, 50);
            btnCancelar.TabIndex = 6;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(221, 1);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(371, 267);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            // 
            // frmLogin
            // 
            AcceptButton = btnIngresar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(800, 471);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(btnCancelar);
            Controls.Add(btnIngresar);
            Controls.Add(txtPassword);
            Controls.Add(txtUsuario);
            Controls.Add(label3);
            Controls.Add(label2);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtUsuario;
        private TextBox txtPassword;
        private Button btnIngresar;
        private Button btnCancelar;
        private PictureBox pictureBox1;
    }
}