namespace LoftComputacion.WinForms
{
    partial class frmUsuarioDetalle
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
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            txtNombreUsuario = new TextBox();
            txtEmailUsuario = new TextBox();
            txtPasswordUsuario = new TextBox();
            cmbRolUsuario = new ComboBox();
            label5 = new Label();
            btnGuardarUsuario = new Button();
            btnCancelarUsuario = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F);
            label1.Location = new Point(197, 231);
            label1.Name = "label1";
            label1.Size = new Size(146, 32);
            label1.TabIndex = 0;
            label1.Text = "Contraseña: ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 18F);
            label2.Location = new Point(260, 164);
            label2.Name = "label2";
            label2.Size = new Size(83, 32);
            label2.TabIndex = 1;
            label2.Text = "Email: ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 18F);
            label3.Location = new Point(124, 95);
            label3.Name = "label3";
            label3.Size = new Size(219, 32);
            label3.TabIndex = 2;
            label3.Text = "Nombre Completo:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 18F);
            label4.Location = new Point(284, 290);
            label4.Name = "label4";
            label4.Size = new Size(59, 32);
            label4.TabIndex = 3;
            label4.Text = "Rol: ";
            // 
            // txtNombreUsuario
            // 
            txtNombreUsuario.Font = new Font("Segoe UI", 15.75F);
            txtNombreUsuario.Location = new Point(349, 92);
            txtNombreUsuario.Name = "txtNombreUsuario";
            txtNombreUsuario.Size = new Size(259, 35);
            txtNombreUsuario.TabIndex = 4;
            // 
            // txtEmailUsuario
            // 
            txtEmailUsuario.Font = new Font("Segoe UI", 15.75F);
            txtEmailUsuario.Location = new Point(349, 161);
            txtEmailUsuario.Name = "txtEmailUsuario";
            txtEmailUsuario.Size = new Size(259, 35);
            txtEmailUsuario.TabIndex = 5;
            // 
            // txtPasswordUsuario
            // 
            txtPasswordUsuario.Font = new Font("Segoe UI", 15.75F);
            txtPasswordUsuario.Location = new Point(349, 228);
            txtPasswordUsuario.Name = "txtPasswordUsuario";
            txtPasswordUsuario.Size = new Size(259, 35);
            txtPasswordUsuario.TabIndex = 6;
            txtPasswordUsuario.UseSystemPasswordChar = true;
            // 
            // cmbRolUsuario
            // 
            cmbRolUsuario.Font = new Font("Segoe UI", 15.75F);
            cmbRolUsuario.FormattingEnabled = true;
            cmbRolUsuario.Location = new Point(349, 284);
            cmbRolUsuario.Name = "cmbRolUsuario";
            cmbRolUsuario.Size = new Size(259, 38);
            cmbRolUsuario.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(332, 21);
            label5.Name = "label5";
            label5.Size = new Size(190, 40);
            label5.TabIndex = 8;
            label5.Text = "Crear Usuario";
            // 
            // btnGuardarUsuario
            // 
            btnGuardarUsuario.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnGuardarUsuario.Location = new Point(249, 373);
            btnGuardarUsuario.Name = "btnGuardarUsuario";
            btnGuardarUsuario.Size = new Size(144, 44);
            btnGuardarUsuario.TabIndex = 9;
            btnGuardarUsuario.Text = "Guardar";
            btnGuardarUsuario.UseVisualStyleBackColor = true;
            btnGuardarUsuario.Click += btnGuardarUsuario_Click;
            // 
            // btnCancelarUsuario
            // 
            btnCancelarUsuario.Font = new Font("Segoe UI", 15.75F);
            btnCancelarUsuario.Location = new Point(460, 373);
            btnCancelarUsuario.Name = "btnCancelarUsuario";
            btnCancelarUsuario.Size = new Size(148, 44);
            btnCancelarUsuario.TabIndex = 10;
            btnCancelarUsuario.Text = "Cancelar";
            btnCancelarUsuario.UseVisualStyleBackColor = true;
            btnCancelarUsuario.Click += btnCancelarUsuario_Click;
            // 
            // frmUsuarioDetalle
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCancelarUsuario);
            Controls.Add(btnGuardarUsuario);
            Controls.Add(label5);
            Controls.Add(cmbRolUsuario);
            Controls.Add(txtPasswordUsuario);
            Controls.Add(txtEmailUsuario);
            Controls.Add(txtNombreUsuario);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "frmUsuarioDetalle";
            Text = "Crear Usuario";
            Load += frmUsuarioDetalle_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox txtNombreUsuario;
        private TextBox txtEmailUsuario;
        private TextBox txtPasswordUsuario;
        private ComboBox cmbRolUsuario;
        private Label label5;
        private Button btnGuardarUsuario;
        private Button btnCancelarUsuario;
    }
}