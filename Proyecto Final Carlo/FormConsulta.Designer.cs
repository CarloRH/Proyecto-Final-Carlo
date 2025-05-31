namespace Proyecto_Final_Carlo
{
    partial class FormConsulta
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConsulta));
            this.dataGridViewReportes = new System.Windows.Forms.DataGridView();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.webViewConsulta = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReportes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.webViewConsulta)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewReportes
            // 
            this.dataGridViewReportes.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dataGridViewReportes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewReportes.Location = new System.Drawing.Point(585, 12);
            this.dataGridViewReportes.Name = "dataGridViewReportes";
            this.dataGridViewReportes.RowHeadersWidth = 51;
            this.dataGridViewReportes.RowTemplate.Height = 24;
            this.dataGridViewReportes.Size = new System.Drawing.Size(577, 354);
            this.dataGridViewReportes.TabIndex = 0;
            this.dataGridViewReportes.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewReportes_CellContentClick);
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnCerrar.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.Location = new System.Drawing.Point(800, 439);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(151, 46);
            this.btnCerrar.TabIndex = 4;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // webViewConsulta
            // 
            this.webViewConsulta.AllowExternalDrop = true;
            this.webViewConsulta.CreationProperties = null;
            this.webViewConsulta.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webViewConsulta.Location = new System.Drawing.Point(12, 12);
            this.webViewConsulta.Name = "webViewConsulta";
            this.webViewConsulta.Size = new System.Drawing.Size(567, 568);
            this.webViewConsulta.TabIndex = 5;
            this.webViewConsulta.ZoomFactor = 1D;
            // 
            // FormConsulta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1174, 592);
            this.ControlBox = false;
            this.Controls.Add(this.webViewConsulta);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.dataGridViewReportes);
            this.Name = "FormConsulta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormConsulta";
            this.Load += new System.EventHandler(this.FormConsulta_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReportes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.webViewConsulta)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewReportes;
        private System.Windows.Forms.Button btnCerrar;
        private Microsoft.Web.WebView2.WinForms.WebView2 webViewConsulta;
    }
}