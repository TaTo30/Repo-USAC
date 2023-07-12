using System.Collections.Generic;

public class SemanticException : System.Exception
{
    public int Linea {get; set;}
    public int Columna {get; set;}
    public SemanticException(string message) : base(message) {
        this.Linea = 0;
        this.Columna = 0;
     }
    
    public SemanticException(string message, int line, int column) : base(message){
        this.Linea = line;
        this.Columna = column;
    }

}