using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace Compi_2P1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void AbrirArchivoEvent(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Archivo Pascal (*.pas) | *.pas";
            fileDialog.InitialDirectory = "C://";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                using (System.IO.StreamReader lector = new System.IO.StreamReader(fileDialog.FileName))
                { 
                    this.textBox1.Text = lector.ReadToEnd();
                }
            }
        }
        private void GuardarArchivoEvent(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Archivo Pascal (*.pas) | *.pas";
            saveDialog.InitialDirectory = "C://";
            if(saveDialog.ShowDialog() == DialogResult.OK)
            {
                using(System.IO.StreamWriter redactor = new System.IO.StreamWriter(saveDialog.FileName))
                {
                    redactor.Write(this.textBox1.Text);
                }
            }
        }
        private void CerrarAplicacionEvent(object sender, EventArgs e)
        {
            this.Close();
        }
        private void TraducirEntrada(object sender, EventArgs e)
        {
            Log.AddLog("Traduciendo Entrada...\r\n");
            Log.AddLog("\r\n");
            if (!this.textBox1.Equals(""))
            {
                CompiParser traductor = new CompiParser();
                this.textBox1.Text = traductor.Analizar(this.textBox1.Text);
            }
            List<String> log = Log.GetLogs();
            string output = "";
            foreach (string l in log)
            {
                output += l;
            }
            this.textBox2.Text = output;
        }
        private void InterpretarEntrada(object sender, EventArgs e)
        {
            Log.AddLog("Interpretando Entrada...\r\n");
            Log.AddLog("\r\n");
            if (!this.textBox1.Equals(""))
            {
                Sintactico interprete = new Sintactico();
                interprete.Analizar(this.textBox1.Text);
            }
            List<String> log = Log.GetLogs();
            string output = "";
            foreach (string l in log)
            {
               output += l;
            }
            this.textBox2.Text = output;
        }
        private void PrintAST(object sender, EventArgs e)
        {
            System.Diagnostics.Process cmd = new System.Diagnostics.Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            try
            {
                string path = System.IO.Directory.GetCurrentDirectory();
                cmd.Start();
                cmd.StandardInput.WriteLine($"dot -Tpng {path}\\AST.dot -o {path}\\AST.png");
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.Close();
                Log.AddLog($"Arbol AST Creado en {path}\\AST.png\r\n");
                Log.AddLog("\r\n");
            } catch(Exception ex) {
                Log.AddLog($"Ha ocurrido un error durante la generacion del arbol AST {ex.Message} {ex.Data} \r\n");
                Log.AddLog("\r\n");
            }
            List<String> log = Log.GetLogs();
            string output = "";
            foreach (string l in log)
            {
                output += l;
            }
            this.textBox2.Text = output;
        }
        private void PrintErrores(object sender, EventArgs e)
        {
            System.Diagnostics.Process cmd = new System.Diagnostics.Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            try
            {
                string path = System.IO.Directory.GetCurrentDirectory();
                cmd.Start();
                cmd.StandardInput.WriteLine($"dot -Tpng {path}\\Error.dot -o {path}\\Error.png");
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.Close();
                Log.AddLog($"Tabla de Errores Creado en {path}\\Error.png\r\n");
                Log.AddLog("\r\n");
            }
            catch (Exception ex)
            {
                Log.AddLog($"Ha ocurrido un error durante la generacion de errores {ex.Message} {ex.Data} \r\n");
                Log.AddLog("\r\n");
            }
            List<String> log = Log.GetLogs();
            string output = "";
            foreach (string l in log)
            {
                output += l;
            }
            this.textBox2.Text = output;
        }
    }
}
