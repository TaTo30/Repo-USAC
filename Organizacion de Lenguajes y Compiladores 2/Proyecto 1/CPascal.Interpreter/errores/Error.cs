using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
public class Error
{
    public int Linea {get; set;}
    public int Columna {get; set;}
    public string Mensaje {get; set;}
    public Tipo tipo {get; set;}

    public enum Tipo{
        LEXICO,
        SINTACTICO,
        SEMANTICO
    }

    public Error(int linea, int columna, string mensaje, Tipo tipo){
        this.Linea = linea;
        this.Columna = columna;
        this.Mensaje = mensaje;
        this.tipo = tipo;
    }

    public Error(string mensaje){
        this.Mensaje = mensaje;
        this.tipo = Tipo.SEMANTICO;
    }
}