using System;
class Asignacion : Instruccion
{

    public int Linea {get; set;}
    public int Columna {get; set;}
    private string id;
    private Objeto objeto;
    private OpArray array;
    private Operacion expresion;


    //CONSTRUCTOR PARA LA ASGNACION TIPICA
    public Asignacion(string id, Operacion ex, int x, int y){
        this.id = id;
        this.expresion = ex;
        this.Linea = x;
        this.Columna = y;
    }

    //CONSTRUCTOR PARA LA ASIGNACION DE OBJETOS
    public Asignacion(Objeto objeto, Operacion ex, int x, int y){
        this.objeto = objeto;
        this.id = objeto.id;
        this.expresion = ex;
        this.Linea = x;
        this.Columna = y;
    }

    //CONSTRUCTOR PARA LA ASIGNACION DE ARRAYS
    public Asignacion(OpArray array, Operacion ex, int x, int y){
        this.array = array;
        this.id = array.Id;
        this.expresion = ex;
        this.Linea = x;
        this.Columna = y;
    }


    public object ejecutar(Entorno env){
        var valor = expresion.ejecutar(env);
        if (env.GetTipo(id) == Simbolo.Tipo.ERROR)
            throw new SemanticException($"La variable {this.id} no existe en el ambito", this.Linea, this.Columna);
        if (env.GetConst(id))
            throw new SemanticException($"La variable {this.id} es un valor de tipo constante", this.Linea, this.Columna); 
        switch (env.GetTipo(id))
        {
            //ASIGNACION RECURSIVA DE OBJETOS
            case Simbolo.Tipo.OBJECT:
                if (this.objeto != null)
                {
                    this.objeto.ejecutar_objeto(env, true, valor);
                }else {
                    env.SetValor(id, valor);
                }                
                break;
            case Simbolo.Tipo.ARRAY:
                this.array.ejecutar(env, valor);
                break;
            //ASGIGNACION DE OTROS TIPOS
            default:
                if ((valor is double) && (env.GetTipo(id) == Simbolo.Tipo.REAL || env.GetTipo(id) == Simbolo.Tipo.INTEGER))
                    env.SetValor(id, valor);
                else if (valor is string && (env.GetTipo(id) == Simbolo.Tipo.STRING))
                    env.SetValor(id, valor);
                else if (valor is bool && (env.GetTipo(id)) == Simbolo.Tipo.BOOLEAN)
                    env.SetValor(id, valor);
                else 
                    throw new SemanticException($"El valor tipo {valor.GetType().ToString()} no es asignable a la variable tipo {env.GetTipo(id).ToString()}", this.Linea, this.Columna);
                break;                
        } 
        return Control.ControlSet.NONE;
    }
}