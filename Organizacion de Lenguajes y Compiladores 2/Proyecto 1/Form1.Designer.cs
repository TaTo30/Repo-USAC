
namespace Compi_2P1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeFileItem = new System.Windows.Forms.ToolStripMenuItem();
            this.programMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.traduProgramItem = new System.Windows.Forms.ToolStripMenuItem();
            this.interProgramItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reporteMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ASTReporteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ErroresProgramItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox1.Location = new System.Drawing.Point(12, 46);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(776, 250);
            this.textBox1.TabIndex = 0;
            this.textBox1.WordWrap = false;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox2.ForeColor = System.Drawing.Color.Orange;
            this.textBox2.Location = new System.Drawing.Point(12, 312);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox2.Size = new System.Drawing.Size(775, 161);
            this.textBox2.TabIndex = 1;
            this.textBox2.WordWrap = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.programMenu,
            this.reporteMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            this.fileMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileItem,
            this.saveFileItem,
            this.closeFileItem});
            this.fileMenu.ForeColor = System.Drawing.Color.White;
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(60, 20);
            this.fileMenu.Text = "Archivo";
            // 
            // openFileItem
            // 
            this.openFileItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.openFileItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.openFileItem.ForeColor = System.Drawing.Color.White;
            this.openFileItem.Name = "openFileItem";
            this.openFileItem.Size = new System.Drawing.Size(160, 22);
            this.openFileItem.Text = "Abrir Archivo";
            this.openFileItem.Click += new System.EventHandler(this.AbrirArchivoEvent);
            // 
            // saveFileItem
            // 
            this.saveFileItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.saveFileItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.saveFileItem.ForeColor = System.Drawing.Color.White;
            this.saveFileItem.Name = "saveFileItem";
            this.saveFileItem.Size = new System.Drawing.Size(160, 22);
            this.saveFileItem.Text = "Guardar Archivo";
            this.saveFileItem.Click += new System.EventHandler(this.GuardarArchivoEvent);
            // 
            // closeFileItem
            // 
            this.closeFileItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.closeFileItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.closeFileItem.ForeColor = System.Drawing.Color.White;
            this.closeFileItem.Name = "closeFileItem";
            this.closeFileItem.Size = new System.Drawing.Size(160, 22);
            this.closeFileItem.Text = "Cerrar";
            this.closeFileItem.Click += new System.EventHandler(this.CerrarAplicacionEvent);
            // 
            // programMenu
            // 
            this.programMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.programMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.traduProgramItem,
            this.interProgramItem});
            this.programMenu.ForeColor = System.Drawing.Color.White;
            this.programMenu.Name = "programMenu";
            this.programMenu.Size = new System.Drawing.Size(71, 20);
            this.programMenu.Text = "Programa";
            // 
            // traduProgramItem
            // 
            this.traduProgramItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.traduProgramItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.traduProgramItem.ForeColor = System.Drawing.Color.White;
            this.traduProgramItem.Name = "traduProgramItem";
            this.traduProgramItem.Size = new System.Drawing.Size(116, 22);
            this.traduProgramItem.Text = "Traducir";
            this.traduProgramItem.Click += new System.EventHandler(this.TraducirEntrada);
            // 
            // interProgramItem
            // 
            this.interProgramItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.interProgramItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.interProgramItem.ForeColor = System.Drawing.Color.White;
            this.interProgramItem.Name = "interProgramItem";
            this.interProgramItem.Size = new System.Drawing.Size(116, 22);
            this.interProgramItem.Text = "Ejecutar";
            this.interProgramItem.Click += new System.EventHandler(this.InterpretarEntrada);
            // 
            // reporteMenu
            // 
            this.reporteMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ASTReporteItem,
            this.ErroresProgramItem});
            this.reporteMenu.ForeColor = System.Drawing.Color.White;
            this.reporteMenu.Name = "reporteMenu";
            this.reporteMenu.Size = new System.Drawing.Size(65, 20);
            this.reporteMenu.Text = "Reportes";
            // 
            // ASTReporteItem
            // 
            this.ASTReporteItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.ASTReporteItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ASTReporteItem.ForeColor = System.Drawing.Color.White;
            this.ASTReporteItem.Name = "ASTReporteItem";
            this.ASTReporteItem.Size = new System.Drawing.Size(110, 22);
            this.ASTReporteItem.Text = "AST";
            this.ASTReporteItem.Click += new System.EventHandler(this.PrintAST);
            // 
            // ErroresProgramItem
            // 
            this.ErroresProgramItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.ErroresProgramItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ErroresProgramItem.ForeColor = System.Drawing.Color.White;
            this.ErroresProgramItem.Name = "ErroresProgramItem";
            this.ErroresProgramItem.Size = new System.Drawing.Size(110, 22);
            this.ErroresProgramItem.Text = "Errores";
            this.ErroresProgramItem.Click += new System.EventHandler(this.PrintErrores);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(800, 485);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Compi Pascal";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem openFileItem;
        private System.Windows.Forms.ToolStripMenuItem saveFileItem;
        private System.Windows.Forms.ToolStripMenuItem closeFileItem;
        private System.Windows.Forms.ToolStripMenuItem programMenu;
        private System.Windows.Forms.ToolStripMenuItem traduProgramItem;
        private System.Windows.Forms.ToolStripMenuItem interProgramItem;
        private System.Windows.Forms.ToolStripMenuItem reporteMenu;
        private System.Windows.Forms.ToolStripMenuItem ASTReporteItem;
        private System.Windows.Forms.ToolStripMenuItem ErroresProgramItem;
    }
}

