using System;
using System.Collections.Generic;
using System.Diagnostics;

class Write : Instruccion{

    public int Linea {get; set;}
    public int Columna {get; set;}
    private LinkedList<Operacion> Contenido;
    private bool NewLine = false;

    public Write(LinkedList<Operacion> contenido, int x, int y){
        this.Contenido = contenido;
        this.Linea = x;
        this.Columna = y;
    }

    public Write(LinkedList<Operacion> contenido, int x, int y, bool newline){
        this.NewLine = newline;
        this.Contenido = contenido;
        this.Linea = x;
        this.Columna = y;
    }

    public object ejecutar(Entorno env){
        foreach (var op in this.Contenido)
        {
            string result = op.ejecutar(env).ToString();
            Debug.Write(result);
            Log.AddLog(result);
        }
            
  
        if (NewLine)
        {
            Debug.WriteLine("");
            Log.AddLog("\r\n");
        }
            
        return Control.ControlSet.NONE;
    }
}