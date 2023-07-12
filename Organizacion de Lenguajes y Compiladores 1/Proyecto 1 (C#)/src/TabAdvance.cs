using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto1
{
    class TabAdvance : TabControl
    {
        private int contadorTabs = 0;
        public TabAdvance()
        {
            contadorTabs = 0;
        }

        public void AddTab()
        {
            TabPage tab = new TabPage();
            RichTextBox textBox = new RichTextBox();
            Label path = new Label();
            path.Text = "";
            path.Visible = false;
            textBox.Multiline = true;
            textBox.Size = new System.Drawing.Size(this.Width-12, this.Height-27);
            textBox.Location = new System.Drawing.Point(2, 2);
            textBox.ScrollBars = RichTextBoxScrollBars.Both;
            textBox.Text = "";
            textBox.WordWrap = false;
            tab.Text = "Pestaña " + contadorTabs;            
            tab.Controls.Add(path);
            tab.Controls.Add(textBox);
            tab.UseVisualStyleBackColor = true;
            tab.Size = new System.Drawing.Size(this.Width-2, this.Height-2);
            this.TabPages.Add(tab);
            contadorTabs++;
        }

        public void AddTab(string titulo, string texto, string directory)
        {
            TabPage tab = new TabPage();
            RichTextBox textBox = new RichTextBox();
            Label path = new Label();
            path.Text = directory;
            path.Visible = false;
            textBox.Multiline = true;
            textBox.Size = new System.Drawing.Size(this.Width - 12, this.Height - 27);
            textBox.Location = new System.Drawing.Point(2, 2);
            textBox.ScrollBars = RichTextBoxScrollBars.Both;
            textBox.Text = texto;
            textBox.WordWrap = false;
            tab.Text = titulo;
            tab.Controls.Add(path);
            tab.Controls.Add(textBox);
            tab.UseVisualStyleBackColor = true;
            tab.Size = new System.Drawing.Size(this.Width - 2, this.Height - 2);
            this.TabPages.Add(tab);
            contadorTabs++;
        }
    }
}
