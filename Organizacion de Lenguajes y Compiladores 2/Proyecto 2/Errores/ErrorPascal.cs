using System;
public class PascalExcepcion : System.Exception
{
    public enum ParseError
    {
        SINTACTIO,
        LEXICO,
        SEMANTICO
    }
    public ParseError Tipo {get; set;}
    public int Columna {get; set;}
    public int Linea {get; set;}
    public DateTime Fecha {get; set;}
    public PascalExcepcion() { }
    public PascalExcepcion(string message) : base(message) { }
    public PascalExcepcion(string message, ParseError error, int x, int y) : base(message){
        this.Tipo = error;
        this.Columna = y;
        this.Linea = x;
        this.Fecha = DateTime.Now;
    }
}