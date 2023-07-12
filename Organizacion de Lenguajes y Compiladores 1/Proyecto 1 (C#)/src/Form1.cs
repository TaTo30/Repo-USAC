using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Proyecto1
{
    public partial class Form1 : Form
    {
        private OpenFileDialog AbrirArchivo = new OpenFileDialog();
        private SaveFileDialog GuardarArchivo = new SaveFileDialog();
        private Analizador Scanner;
        public Form1()
        {
            InitializeComponent();
            ComponentesDialogo();
            Scanner = new Analizador(this);
        }

        private void AbrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            string tempo, texto = "";
            if (AbrirArchivo.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader lector = new System.IO.StreamReader(AbrirArchivo.FileName);
                tempo = lector.ReadLine();
                while (tempo != null)
                {
                    texto += tempo + "\r\n";
                    tempo = lector.ReadLine();
                }
                this.tabControl1.AddTab(System.IO.Path.GetFileNameWithoutExtension(AbrirArchivo.FileName),texto, AbrirArchivo.FileName);
                //richTextBox1.Text = texto
                lector.Close();

            }
        }

        private void ComponentesDialogo()
        {
            //CONFIGURACION PRIMARIA ABRIR ARCHIVO
            AbrirArchivo.AddExtension = true;
            AbrirArchivo.DefaultExt = ".er";
            AbrirArchivo.Title = "Abrir Archivo";
            AbrirArchivo.Filter = "Archivo de Expresiones|*.er|Todos los Archivos|*.*";
            //CONFIGURACION PRIMARIA GUARDAR ARCHIVO
            GuardarArchivo.AddExtension = true;
            GuardarArchivo.DefaultExt = ".er";
            GuardarArchivo.Title = "Guardar Archivo";
            GuardarArchivo.Filter = "Archivo de Expresiones|*.er|Todos los Archivos|*.*";
        }

        private void AnalizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            indexActual = 0;
            Scanner.Scan(TextoSeleccionado());
            if (Scanner.Expresiones.Count() != 0)
            {
                Scanner.GenerarDFA();
                if (MessageBox.Show("Analisis Completado", "Analisis",MessageBoxButtons.OK,MessageBoxIcon.Information) == DialogResult.OK)
                {
                    SetModoVista();
                }
            }
        }

        private void GuardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GuardarArchivo.ShowDialog() == DialogResult.OK)
            {
                if (GuardarArchivo.CheckPathExists)
                {
                    StreamWriter redactor = new StreamWriter(GuardarArchivo.FileName);
                    redactor.Write(TextoSeleccionado());
                    redactor.Close();
                }
            }         
        }

        private string TextoSeleccionado()
        {
            string texto = "";
            TabPage tabActual = tabControl1.SelectedTab;
            foreach (Control item in tabActual.Controls)
            {
                if (item is RichTextBox)
                {
                    RichTextBox temp = (RichTextBox)item;
                    texto = temp.Text;
                }
            }
            return texto;
        }

        public string ObtenerPath()
        {
            string path="";
            foreach (Control item in tabControl1.SelectedTab.Controls)
            {
                if (item is Label)
                {
                    Label temp = (Label)item;
                    path = temp.Text;
                }               
            }
            return path;
        }

        public void SetLog(string log)
        {
            textBox1.Text += log + "\r\n";
        }

        public void SetTable(DataGridView tabla)
        {
            //this.dataGridView1 = tabla;
            foreach (Control item in panelTabla.Controls)
            {
                panelTabla.Controls.Remove(item);
            }
            this.panelTabla.Controls.Add(tabla);           
            Console.WriteLine("Terminado");
        }

        public void SetDFA(string png)
        {
            pictureBox1.Image = System.Drawing.Image.FromFile(png);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;            
            
        }

        LinkedList<Expresion> listaExpresiones;
        int indexActual = 0;
        private void SetModoVista()
        {
            listaExpresiones = Scanner.Expresiones;
            SetTable(listaExpresiones.ElementAt(indexActual).GetTablaTransiciones());
            SetDFA(listaExpresiones.ElementAt(indexActual).GetImage());
            pathImage = listaExpresiones.ElementAt(indexActual).GetImage();
            label1.Text = listaExpresiones.ElementAt(indexActual).getNombre();
        }

        private void ButtonLeft_Click(object sender, EventArgs e)
        {
            if (indexActual > 0)
            {
                indexActual--;
                SetModoVista();
            }
        }

        private void ButtonRight_Click(object sender, EventArgs e)
        {
            if (indexActual < listaExpresiones.Count-1)
            {
                indexActual++;
                SetModoVista();
            }
        }

        string pathImage = "";
        private void PictureBox1_Click(object sender, EventArgs e)
        {
            
            System.Diagnostics.Process cms = new System.Diagnostics.Process();
            cms.StartInfo.FileName = pathImage;
            cms.Start();
            
        }

        private void XMLTokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string XMLguardar = "<ListaTokens>\r\n";
            foreach (Token item in Scanner.Tokens)
            {
                XMLguardar += "  <Token>\r\n";
                XMLguardar += "     <Nombre>"+item.getTokenTipo()+"</Nombre>\r\n";
                XMLguardar += "     <Valor>" + item.getContenido() + "</Valor>\r\n";
                XMLguardar += "  </Token>\r\n";
            }
            XMLguardar += "</ListaTokens>";
            StreamWriter streamWriter = new StreamWriter(Path.GetDirectoryName(ObtenerPath())+"\\XMLTokens.xml");
            streamWriter.Write(XMLguardar);
            streamWriter.Close();
            if (MessageBox.Show("¿Desea Abrir el Archivo?", "Tokens Generados", MessageBoxButtons.YesNo,MessageBoxIcon.Question)== DialogResult.Yes)
            {
                System.Diagnostics.Process cms = new System.Diagnostics.Process();
                cms.StartInfo.FileName = Path.GetDirectoryName(ObtenerPath()) + "\\XMLTokens.xml";
                cms.Start();
            }
        }

        private void XMLErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string XMLguardar = "<ListaErrores>\r\n";
            foreach (string item in Scanner.Errores)
            {
                XMLguardar += "  <Error>\r\n";
                XMLguardar += "     <Nombre>" + "DESCONOCIDO" + "</Nombre>\r\n";
                XMLguardar += "     <Valor>" + item + "</Valor>\r\n";
                XMLguardar += "  </Error>\r\n";
            }
            XMLguardar += "</ListaErrores>";
            StreamWriter streamWriter = new StreamWriter(Path.GetDirectoryName(ObtenerPath()) + "\\XMLErrores.xml");
            streamWriter.Write(XMLguardar);
            streamWriter.Close();
            if (MessageBox.Show("¿Desea Abrir el Archivo?", "Errores Generados", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                System.Diagnostics.Process cms = new System.Diagnostics.Process();
                cms.StartInfo.FileName = Path.GetDirectoryName(ObtenerPath()) + "\\XMLErrores.xml";
                cms.Start();
            }
        }
        private void ButtonAFN_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = System.Drawing.Image.FromFile(listaExpresiones.ElementAt(indexActual).GetAFNImage());
            pathImage = listaExpresiones.ElementAt(indexActual).GetAFNImage();
        }

        private void DFAButton_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = System.Drawing.Image.FromFile(listaExpresiones.ElementAt(indexActual).GetImage());
            pathImage = listaExpresiones.ElementAt(indexActual).GetImage();
        }

      
    }
}
