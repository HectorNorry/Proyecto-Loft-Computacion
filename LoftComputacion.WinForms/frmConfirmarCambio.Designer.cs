namespace LoftComputacion.WinForms
{
    partial class frmConfirmarCambio
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
            cmbUsuarios = new ComboBox();
            label2 = new Label();
            txtPasswordConfirmacion = new TextBox();
            btnConfirmar = new Button();
            btnCancelarConfirmacion = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold);
            label1.Location = new Point(50, 22);
            label1.Name = "label1";
            label1.Size = new Size(209, 25);
            label1.TabIndex = 0;
            label1.Text = "Seleccione su usuario : ";
            // 
            // cmbUsuarios
            // 
            cmbUsuarios.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUsuarios.FormattingEnabled = true;
            cmbUsuarios.Location = new Point(284, 27);
            cmbUsuarios.Name = "cmbUsuarios";
            cmbUsuarios.Size = new Size(178, 23);
            cmbUsuarios.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold);
            label2.Location = new Point(50, 74);
            label2.Name = "label2";
            label2.Size = new Size(208, 25);
            label2.TabIndex = 2;
            label2.Text = "Ingrese su contraseña :";
            // 
            // txtPasswordConfirmacion
            // 
            txtPasswordConfirmacion.Location = new Point(284, 79);
            txtPasswordConfirmacion.Name = "txtPasswordConfirmacion";
            txtPasswordConfirmacion.Size = new Size(178, 23);
            txtPasswordConfirmacion.TabIndex = 3;
            txtPasswordConfirmacion.UseSystemPasswordChar = true;
            // 
            // btnConfirmar
            // 
            btnConfirmar.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold);
            btnConfirmar.Location = new Point(236, 138);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(113, 37);
            btnConfirmar.TabIndex = 4;
            btnConfirmar.Text = "Confirmar";
            btnConfirmar.UseVisualStyleBackColor = true;
            btnConfirmar.Click += btnConfirmar_Click;
            // 
            // btnCancelarConfirmacion
            // 
            btnCancelarConfirmacion.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold);
            btnCancelarConfirmacion.Location = new Point(362, 138);
            btnCancelarConfirmacion.Name = "btnCancelarConfirmacion";
            btnCancelarConfirmacion.Size = new Size(100, 37);
            btnCancelarConfirmacion.TabIndex = 5;
            btnCancelarConfirmacion.Text = "Cancelar";
            btnCancelarConfirmacion.UseVisualStyleBackColor = true;
            btnCancelarConfirmacion.Click += btnCancelarConfirmacion_Click;
            // 
            // frmConfirmarCambio
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(529, 202);
            Controls.Add(btnCancelarConfirmacion);
            Controls.Add(btnConfirmar);
            Controls.Add(txtPasswordConfirmacion);
            Controls.Add(label2);
            Controls.Add(cmbUsuarios);
            Controls.Add(label1);
            Name = "frmConfirmarCambio";
            Text = "frmConfirmarCambio";
            Load += frmConfirmarCambio_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cmbUsuarios;
        private Label label2;
        private TextBox txtPasswordConfirmacion;
        private Button btnConfirmar;
        private Button btnCancelarConfirmacion;
    }
}