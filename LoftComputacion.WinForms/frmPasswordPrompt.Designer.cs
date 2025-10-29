namespace LoftComputacion.WinForms
{
    partial class frmPasswordPrompt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPasswordPrompt));
            label1 = new Label();
            txtPasswordAdmin = new TextBox();
            btnAceptarPass = new Button();
            btnCancelarPass = new Button();
            txtAdminUsuario = new TextBox();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(44, 31);
            label1.Name = "label1";
            label1.Size = new Size(463, 32);
            label1.TabIndex = 0;
            label1.Text = "Ingrese las credenciales de administrador";
            // 
            // txtPasswordAdmin
            // 
            txtPasswordAdmin.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPasswordAdmin.Location = new Point(192, 144);
            txtPasswordAdmin.Name = "txtPasswordAdmin";
            txtPasswordAdmin.Size = new Size(208, 33);
            txtPasswordAdmin.TabIndex = 1;
            txtPasswordAdmin.UseSystemPasswordChar = true;
            // 
            // btnAceptarPass
            // 
            btnAceptarPass.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAceptarPass.Location = new Point(160, 220);
            btnAceptarPass.Name = "btnAceptarPass";
            btnAceptarPass.Size = new Size(89, 47);
            btnAceptarPass.TabIndex = 2;
            btnAceptarPass.Text = "Aceptar";
            btnAceptarPass.UseVisualStyleBackColor = true;
            btnAceptarPass.Click += btnAceptarPass_Click;
            // 
            // btnCancelarPass
            // 
            btnCancelarPass.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancelarPass.Location = new Point(272, 220);
            btnCancelarPass.Name = "btnCancelarPass";
            btnCancelarPass.Size = new Size(96, 47);
            btnCancelarPass.TabIndex = 3;
            btnCancelarPass.Text = "Cancelar";
            btnCancelarPass.UseVisualStyleBackColor = true;
            // 
            // txtAdminUsuario
            // 
            txtAdminUsuario.Font = new Font("Segoe UI", 14.25F);
            txtAdminUsuario.Location = new Point(192, 94);
            txtAdminUsuario.Name = "txtAdminUsuario";
            txtAdminUsuario.Size = new Size(208, 33);
            txtAdminUsuario.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(100, 102);
            label2.Name = "label2";
            label2.Size = new Size(86, 25);
            label2.TabIndex = 5;
            label2.Text = "Usuario: ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(69, 152);
            label3.Name = "label3";
            label3.Size = new Size(117, 25);
            label3.TabIndex = 6;
            label3.Text = "Contraseña: ";
            // 
            // frmPasswordPrompt
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(536, 301);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txtAdminUsuario);
            Controls.Add(btnCancelarPass);
            Controls.Add(btnAceptarPass);
            Controls.Add(txtPasswordAdmin);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmPasswordPrompt";
            Text = "Ganancias";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtPasswordAdmin;
        private Button btnAceptarPass;
        private Button btnCancelarPass;
        private TextBox txtAdminUsuario;
        private Label label2;
        private Label label3;
    }
}