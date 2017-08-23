namespace WindowsFormsApplication2
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.imagen = new System.Windows.Forms.PictureBox();
            this.btnstream = new System.Windows.Forms.Button();
            this.btnParar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imagen)).BeginInit();
            this.SuspendLayout();
            // 
            // imagen
            // 
            this.imagen.Location = new System.Drawing.Point(59, 24);
            this.imagen.Name = "imagen";
            this.imagen.Size = new System.Drawing.Size(640, 480);
            this.imagen.TabIndex = 0;
            this.imagen.TabStop = false;
            // 
            // btnstream
            // 
            this.btnstream.Location = new System.Drawing.Point(719, 505);
            this.btnstream.Name = "btnstream";
            this.btnstream.Size = new System.Drawing.Size(75, 23);
            this.btnstream.TabIndex = 1;
            this.btnstream.Text = "Empezar";
            this.btnstream.UseVisualStyleBackColor = true;
            this.btnstream.Click += new System.EventHandler(this.btnstream_Click);
            // 
            // btnParar
            // 
            this.btnParar.Location = new System.Drawing.Point(733, 303);
            this.btnParar.Name = "btnParar";
            this.btnParar.Size = new System.Drawing.Size(75, 23);
            this.btnParar.TabIndex = 2;
            this.btnParar.Text = "Parar";
            this.btnParar.UseVisualStyleBackColor = true;
            this.btnParar.Click += new System.EventHandler(this.btnParar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 540);
            this.Controls.Add(this.btnParar);
            this.Controls.Add(this.btnstream);
            this.Controls.Add(this.imagen);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imagen)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox imagen;
        private System.Windows.Forms.Button btnstream;
        private System.Windows.Forms.Button btnParar;
    }
}

