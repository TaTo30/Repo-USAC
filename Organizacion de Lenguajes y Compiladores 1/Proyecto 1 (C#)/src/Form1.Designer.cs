namespace Proyecto1
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analizarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.añadirTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xMLTokenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xMLErrorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelDFA = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelTabla = new System.Windows.Forms.Panel();
            this.buttonLeft = new System.Windows.Forms.Button();
            this.buttonRight = new System.Windows.Forms.Button();
            this.tabControl1 = new Proyecto1.TabAdvance();
            this.ButtonAFN = new System.Windows.Forms.Button();
            this.DFAButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.panelDFA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 342);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(432, 115);
            this.textBox1.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.reportesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(890, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.abrirToolStripMenuItem,
            this.guardarToolStripMenuItem,
            this.analizarToolStripMenuItem,
            this.añadirTabToolStripMenuItem});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // abrirToolStripMenuItem
            // 
            this.abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            this.abrirToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.abrirToolStripMenuItem.Text = "Abrir";
            this.abrirToolStripMenuItem.Click += new System.EventHandler(this.AbrirToolStripMenuItem_Click);
            // 
            // guardarToolStripMenuItem
            // 
            this.guardarToolStripMenuItem.Name = "guardarToolStripMenuItem";
            this.guardarToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.guardarToolStripMenuItem.Text = "Guardar";
            this.guardarToolStripMenuItem.Click += new System.EventHandler(this.GuardarToolStripMenuItem_Click);
            // 
            // analizarToolStripMenuItem
            // 
            this.analizarToolStripMenuItem.Name = "analizarToolStripMenuItem";
            this.analizarToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.analizarToolStripMenuItem.Text = "Analizar";
            this.analizarToolStripMenuItem.Click += new System.EventHandler(this.AnalizarToolStripMenuItem_Click);
            // 
            // añadirTabToolStripMenuItem
            // 
            this.añadirTabToolStripMenuItem.Name = "añadirTabToolStripMenuItem";
            this.añadirTabToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.añadirTabToolStripMenuItem.Text = "Añadir Tab";
            // 
            // reportesToolStripMenuItem
            // 
            this.reportesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xMLTokenToolStripMenuItem,
            this.xMLErrorToolStripMenuItem});
            this.reportesToolStripMenuItem.Name = "reportesToolStripMenuItem";
            this.reportesToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.reportesToolStripMenuItem.Text = "Reportes";
            // 
            // xMLTokenToolStripMenuItem
            // 
            this.xMLTokenToolStripMenuItem.Name = "xMLTokenToolStripMenuItem";
            this.xMLTokenToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.xMLTokenToolStripMenuItem.Text = "XML Token";
            this.xMLTokenToolStripMenuItem.Click += new System.EventHandler(this.XMLTokenToolStripMenuItem_Click);
            // 
            // xMLErrorToolStripMenuItem
            // 
            this.xMLErrorToolStripMenuItem.Name = "xMLErrorToolStripMenuItem";
            this.xMLErrorToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.xMLErrorToolStripMenuItem.Text = "XML Error";
            this.xMLErrorToolStripMenuItem.Click += new System.EventHandler(this.XMLErrorToolStripMenuItem_Click);
            // 
            // panelDFA
            // 
            this.panelDFA.AutoScroll = true;
            this.panelDFA.Controls.Add(this.pictureBox1);
            this.panelDFA.Location = new System.Drawing.Point(484, 56);
            this.panelDFA.Name = "panelDFA";
            this.panelDFA.Size = new System.Drawing.Size(365, 222);
            this.panelDFA.TabIndex = 5;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(359, 229);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.PictureBox1_Click);
            // 
            // panelTabla
            // 
            this.panelTabla.Location = new System.Drawing.Point(484, 292);
            this.panelTabla.Name = "panelTabla";
            this.panelTabla.Size = new System.Drawing.Size(365, 165);
            this.panelTabla.TabIndex = 6;
            // 
            // buttonLeft
            // 
            this.buttonLeft.Location = new System.Drawing.Point(454, 250);
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(24, 65);
            this.buttonLeft.TabIndex = 7;
            this.buttonLeft.Text = "<";
            this.buttonLeft.UseVisualStyleBackColor = true;
            this.buttonLeft.Click += new System.EventHandler(this.ButtonLeft_Click);
            // 
            // buttonRight
            // 
            this.buttonRight.Location = new System.Drawing.Point(855, 250);
            this.buttonRight.Name = "buttonRight";
            this.buttonRight.Size = new System.Drawing.Size(24, 65);
            this.buttonRight.TabIndex = 8;
            this.buttonRight.Text = ">";
            this.buttonRight.UseVisualStyleBackColor = true;
            this.buttonRight.Click += new System.EventHandler(this.ButtonRight_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Location = new System.Drawing.Point(12, 42);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(432, 294);
            this.tabControl1.TabIndex = 4;
            // 
            // ButtonAFN
            // 
            this.ButtonAFN.Location = new System.Drawing.Point(484, 31);
            this.ButtonAFN.Name = "ButtonAFN";
            this.ButtonAFN.Size = new System.Drawing.Size(75, 23);
            this.ButtonAFN.TabIndex = 9;
            this.ButtonAFN.Text = "AFN";
            this.ButtonAFN.UseVisualStyleBackColor = true;
            this.ButtonAFN.Click += new System.EventHandler(this.ButtonAFN_Click);
            // 
            // DFAButton
            // 
            this.DFAButton.Location = new System.Drawing.Point(771, 31);
            this.DFAButton.Name = "DFAButton";
            this.DFAButton.Size = new System.Drawing.Size(75, 23);
            this.DFAButton.TabIndex = 10;
            this.DFAButton.Text = "DFA";
            this.DFAButton.UseVisualStyleBackColor = true;
            this.DFAButton.Click += new System.EventHandler(this.DFAButton_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(565, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 22);
            this.label1.TabIndex = 11;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 469);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DFAButton);
            this.Controls.Add(this.ButtonAFN);
            this.Controls.Add(this.buttonRight);
            this.Controls.Add(this.buttonLeft);
            this.Controls.Add(this.panelTabla);
            this.Controls.Add(this.panelDFA);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelDFA.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guardarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem analizarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem añadirTabToolStripMenuItem;
        private TabAdvance tabControl1;
        private System.Windows.Forms.Panel panelDFA;
        private System.Windows.Forms.Panel panelTabla;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonLeft;
        private System.Windows.Forms.Button buttonRight;
        private System.Windows.Forms.ToolStripMenuItem reportesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xMLTokenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xMLErrorToolStripMenuItem;
        private System.Windows.Forms.Button ButtonAFN;
        private System.Windows.Forms.Button DFAButton;
        private System.Windows.Forms.Label label1;
    }
}

