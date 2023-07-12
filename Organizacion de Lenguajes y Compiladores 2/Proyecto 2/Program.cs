using System.Collections.Generic;
using System.Linq;
using System;
namespace OLC2_P2
{
    class Program
    {
        static void Main(string[] args)
        {   
            System.IO.StreamReader lector = new System.IO.StreamReader("./Docs/Basico.pas");
            Pascal sintactico = new Pascal();
            sintactico.Analizar(lector.ReadToEnd()); 
            
            if (sintactico.LenguageStatus == Pascal.Status.PARSED)
            {
                Console.WriteLine("Entrada Analizada con exito");
                Console.WriteLine("Generando Tabla de simbolos...");
                using (System.IO.StreamWriter w = new System.IO.StreamWriter("./TS.dot"))
                    w.Write(sintactico.TablaSimbolosDOT());
                Console.WriteLine("Tabla de simbolos generada");
                try
                {
                    Console.WriteLine("Traduciendo...");
                    TresDirecciones.Traducir(sintactico);
                    System.IO.StreamWriter redactor = new System.IO.StreamWriter("./C3D.c");
                    redactor.Write(TresDirecciones.Parse());
                    redactor.Close();
                    lector.Close();                    
                    Console.WriteLine("Traduccion Completada");
                }
                catch (PascalExcepcion ex)
                {
                    Console.WriteLine("Error Traduciendo entrada");
                    Console.WriteLine($"Error: {ex.Message} {ex.StackTrace}");
                }
                
                Console.WriteLine("Optimizando...");
                Optimizador.Analizador optimizador = new Optimizador.Analizador();
                optimizador.Analizar(TresDirecciones.Parse());   
                if (optimizador.Success == Optimizador.Analizador.Status.ERROR)
                {
                    Console.WriteLine("Ocurrio un error optimizando la entrada");
                    Console.WriteLine("Generando reporte de errores...");
                    using (System.IO.StreamWriter w = new System.IO.StreamWriter("./ERR1.dot")){
                        w.Write(optimizador.GetErroresDOT());
                        w.Close();
                    }
                    Console.WriteLine("Reporte Generado");
                }else if (optimizador.Success == Optimizador.Analizador.Status.PARCIAL){
                    Console.WriteLine("Han ocurrido errores optimizando la entrada, podria no funcionar correctamente");
                    Console.WriteLine("Generando reporte de errores...");
                    using (System.IO.StreamWriter w = new System.IO.StreamWriter("./ERR1.dot")){
                        w.Write(optimizador.GetErroresDOT());
                        w.Close();
                    }
                    Console.WriteLine("Reporte Generado");
                    Console.WriteLine("Generando salida optimizada");
                    using (System.IO.StreamWriter w = new System.IO.StreamWriter("./OC3D.c")){
                        w.Write($"{optimizador.Cabecera}{optimizador.Cuerpo}");
                        w.Close();
                    }
                    Console.WriteLine("Generando reporte de optimizacion");
                    using (System.IO.StreamWriter w = new System.IO.StreamWriter("./OPT.dot"))
                    {
                        w.Write(optimizador.GetReporteDOT());
                        w.Close();
                    }
                    Console.WriteLine("Reporte Generado");
                }else{
                    Console.WriteLine("Generando salida optimizada");
                    using (System.IO.StreamWriter w = new System.IO.StreamWriter("./OC3D.c")){
                        w.Write($"{optimizador.Cabecera}{optimizador.Cuerpo}");
                        w.Close();
                    }
                    Console.WriteLine("Generando reporte de optimizacion");
                    using (System.IO.StreamWriter w = new System.IO.StreamWriter("./OPT.dot"))
                    {
                        w.Write(optimizador.GetReporteDOT());
                        w.Close();
                    }
                    Console.WriteLine("Reporte Generado");
                }
                
                         
            } else {
                Console.WriteLine("Ha ocurrido un error analizando la entrada");
                Console.WriteLine("Generando Reporte de errores");
                using (System.IO.StreamWriter w = new System.IO.StreamWriter("./ERR0.dot"))
                    w.Write(sintactico.ErroresSimbolosDOT());
                Console.WriteLine("Reporte generado");   
                Console.WriteLine("Operacion Abortada");
            }
        }

        public void Start(){
            
        }
    }
}
