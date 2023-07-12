
public class Variable
{
    public string identificador {get; set;}
    public object valor {get; set;}
    public Expresion expresion {get; set;}
    public string tipo {get; set;}

    public Variable(string id, object valor, string tipo = "NONE"){
        this.identificador = id;
        this.valor = valor;
        this.tipo = tipo;
    }
    public Variable(string id, Expresion expresion, string tipo = "NONE"){
        this.identificador = id;
        this.expresion = expresion;
        this.tipo = tipo;
    }
}