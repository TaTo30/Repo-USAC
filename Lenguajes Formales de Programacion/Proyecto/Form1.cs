using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _LFP_Proyecto
{
    public partial class Form1 : Form
    {
        private string path = "";
        private string directory = "C:\\";
        private string filename = "";
        private LinkedList<Token> listaTokenReport;
        private LinkedList<Simbolo> listaSimbolosReport;
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "Consola:";
            
        }

    

      

        private void GenerarTraduccionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ANALIZADOR LEXICO
            String entrada = richTextBox1.Text;
            AnalizadorLexico scanner = new AnalizadorLexico();
            LinkedList<Token> tokens = scanner.Analizar(entrada);
            listaTokenReport = tokens;
            //scanner.imprimirLista(tokens);
            //ANALIZADOR SINTACTICO
            tokens.AddLast(new Token(Token.Tipo.ULTIMO, "ultimo"));
            AnalizadorSintatico parser = new AnalizadorSintatico();
            parser.parsear(tokens);
            Console.WriteLine("FIN!!!");
            textBox1.Text = parser.printConsole();
            Console.WriteLine(parser.solocitaTrad());
            richTextBox2.Text = parser.solocitaTrad();
            System.IO.StreamWriter redactor = new System.IO.StreamWriter(directory+".py");
            Console.WriteLine(directory + ".py");
            redactor.Write(richTextBox2.Text);
            redactor.Close();
            listaSimbolosReport = parser.ListaSimbolos();

            foreach (Simbolo item in listaSimbolosReport)
            {
                Console.WriteLine(item.getNombreVar() + " - " + item.getValorVar() + " - " + item.getTipo());
            }





        }

        private void GuardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog abrirArchivo = new OpenFileDialog();
            abrirArchivo.Filter = "Archivos C# (*.cs) |*.cs";
            abrirArchivo.InitialDirectory = "C://";
            abrirArchivo.ShowDialog();
            if (abrirArchivo.CheckFileExists)
            {


                
                path = abrirArchivo.FileName;
                directory = System.IO.Path.GetDirectoryName(abrirArchivo.FileName)+"\\" +System.IO.Path.GetFileNameWithoutExtension(abrirArchivo.FileName);
                

            }
            System.IO.StreamReader lector = new System.IO.StreamReader(path);
            string textoTemp = "";
            string detNullText = lector.ReadLine();
            while (detNullText != null)
            {
                textoTemp += detNullText + "\r\n";
                detNullText = lector.ReadLine();
            }
            richTextBox1.Text = textoTemp;
            lector.Close();
        }

        private void GuardarComoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog guardarArchivo = new SaveFileDialog();
            guardarArchivo.Filter = "Archivo C# *.cs | *.cs";
            guardarArchivo.InitialDirectory = "C://";
            guardarArchivo.ShowDialog();
            if (guardarArchivo.CheckPathExists)
            {
                path = guardarArchivo.FileName;
                Console.WriteLine(path);
            }
            System.IO.StreamWriter redactor = new System.IO.StreamWriter(path);
            redactor.Write(richTextBox1.Text);
            redactor.Close();
        }

        private void SalirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBox1_KeyEvent(object sender, EventArgs e)
        {
            Console.WriteLine(e.ToString());
        }

        private void ListaDeTokensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string htmlListTokeN = "<div align=\"center\" >" +
                "<table> <tr><td style='text-align: center;' width='500' bgcolor='#FF2D2D'><strong><font color='white'>Valor del Token</font></strong></td><td style='text-align: center;' width='500' bgcolor='#FF2D2D'><strong><font color='white'>Tipo del Token</font></strong></td></tr>";
            foreach (Token item in listaTokenReport)
            {
                htmlListTokeN += "<tr>" +
                    "<td style='text-align: center;'>" +
                    item.GetValor() +
                    "</td>" +
                    "<td style='text-align: center;'>" +
                    item.GetTipoString() +
                    "</td>"+
                    "</tr>";
            }
            htmlListTokeN += "</table></div>";
            System.IO.StreamWriter redactor = new System.IO.StreamWriter("ListaToken.html");
            redactor.Write(htmlListTokeN);
            redactor.Close();
            //System.IO.FileStream archivo;
            System.Diagnostics.Process.Start("ListaToken.html");

        }

        private void ListaDeSimbolosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string htmlListSimbolos = "<div align=\"center\" >" +
                "<table> <tr><td style='text-align: center;' width='500' bgcolor='#FF2D2D'><strong><font color='white'>Tipo de Variable</font></strong></td><td style='text-align: center;' width='500' bgcolor='#FF2D2D'><strong><font color='white'>Nombre Variable</font></strong></td><td style='text-align: center;' width='500' bgcolor='#FF2D2D'><strong><font color='white'>Valor de la variable</font></strong></td></tr>";
            foreach (Simbolo item in listaSimbolosReport)
            {
                htmlListSimbolos += "<tr>" +
                    "<td style='text-align: center;'>" +
                    item.getTipo() +
                    "</td>" +
                    "<td style='text-align: center;'>" +
                    item.getNombreVar() +
                    "</td>" +
                    "<td style='text-align: center;'>" +
                    item.getValorVar() +
                    "</td>" +
                    "</tr>";
            }
            htmlListSimbolos += "</table></div>";
            System.IO.StreamWriter redactor = new System.IO.StreamWriter("ListaSimbolo.html");
            redactor.Write(htmlListSimbolos);
            redactor.Close();
            //System.IO.FileStream archivo;
            System.Diagnostics.Process.Start("ListaSimbolo.html");
        }
    }
}
