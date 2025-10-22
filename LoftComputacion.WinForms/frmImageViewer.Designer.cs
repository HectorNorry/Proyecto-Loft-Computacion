namespace LoftComputacion.WinForms
{
    partial class frmImageViewer
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
            picGrande = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picGrande).BeginInit();
            SuspendLayout();
            // 
            // picGrande
            // 
            picGrande.Dock = DockStyle.Fill;
            picGrande.Location = new Point(0, 0);
            picGrande.Name = "picGrande";
            picGrande.Size = new Size(800, 450);
            picGrande.SizeMode = PictureBoxSizeMode.Zoom;
            picGrande.TabIndex = 0;
            picGrande.TabStop = false;
            // 
            // frmImageViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(picGrande);
            Name = "frmImageViewer";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Visor de Imagen";
            ((System.ComponentModel.ISupportInitialize)picGrande).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox picGrande;
    }
}