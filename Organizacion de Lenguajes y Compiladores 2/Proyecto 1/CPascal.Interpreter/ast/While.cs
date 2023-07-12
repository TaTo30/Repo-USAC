using System.Collections.Generic;

class While : Instruccion
{
    public int Linea {get; set;}
    public int Columna {get; set;}
    private Operacion condicional;
    private LinkedList<Instruccion> instrucciones;

    public While(Operacion condicional, LinkedList<Instruccion> instrucciones, int x, int y){
        this.condicional = condicional;
        this.instrucciones = instrucciones;
        this.Linea = x;
        this.Columna = y;
    }
    public object ejecutar(Entorno env){
        var condicion = condicional.ejecutar(env);
        if (!(condicion is bool))
            throw new SemanticException("Valor condicional no retorna un booleano", this.Linea, this.Columna);
            
        while((bool)condicion){
            foreach (var ins in this.instrucciones)
            {
                //ins.ejecutar(env);
                var res = ins.ejecutar(env);
                if (res is Control.ControlSet)
                {
                    switch ((Control.ControlSet)res)
                    {
                        case Control.ControlSet.BREAK:
                            return Control.ControlSet.NONE;
                        case Control.ControlSet.EXIT:
                            return Control.ControlSet.EXIT;
                        case Control.ControlSet.CONTINUE:
                            goto Reasignacion;
                    }
                }
            }
            Reasignacion:
            condicion = condicional.ejecutar(env);
        }
        return Control.ControlSet.NONE;
    }
}