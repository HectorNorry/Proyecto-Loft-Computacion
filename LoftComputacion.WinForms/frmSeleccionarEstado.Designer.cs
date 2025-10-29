namespace LoftComputacion.WinForms
{
    partial class frmSeleccionarEstado
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSeleccionarEstado));
            label1 = new Label();
            cmbNuevosEstados = new ComboBox();
            btnAceptarEstado = new Button();
            btnCancelarEstado = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(164, 23);
            label1.Name = "label1";
            label1.Size = new Size(223, 45);
            label1.TabIndex = 0;
            label1.Text = "Nuevo Estado";
            // 
            // cmbNuevosEstados
            // 
            cmbNuevosEstados.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbNuevosEstados.FormattingEnabled = true;
            cmbNuevosEstados.Location = new Point(151, 100);
            cmbNuevosEstados.Name = "cmbNuevosEstados";
            cmbNuevosEstados.Size = new Size(252, 33);
            cmbNuevosEstados.TabIndex = 1;
            // 
            // btnAceptarEstado
            // 
            btnAceptarEstado.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAceptarEstado.Location = new Point(151, 179);
            btnAceptarEstado.Name = "btnAceptarEstado";
            btnAceptarEstado.Size = new Size(104, 69);
            btnAceptarEstado.TabIndex = 2;
            btnAceptarEstado.Text = "Aceptar";
            btnAceptarEstado.UseVisualStyleBackColor = true;
            btnAceptarEstado.Click += btnAceptarEstado_Click;
            // 
            // btnCancelarEstado
            // 
            btnCancelarEstado.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancelarEstado.Location = new Point(301, 179);
            btnCancelarEstado.Name = "btnCancelarEstado";
            btnCancelarEstado.Size = new Size(102, 69);
            btnCancelarEstado.TabIndex = 3;
            btnCancelarEstado.Text = "Cancelar";
            btnCancelarEstado.UseVisualStyleBackColor = true;
            btnCancelarEstado.Click += btnCancelarEstado_Click;
            // 
            // frmSeleccionarEstado
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 328);
            Controls.Add(btnCancelarEstado);
            Controls.Add(btnAceptarEstado);
            Controls.Add(cmbNuevosEstados);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmSeleccionarEstado";
            Text = "Cambiar Estado";
            Load += frmSeleccionarEstado_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cmbNuevosEstados;
        private Button btnAceptarEstado;
        private Button btnCancelarEstado;
    }
}