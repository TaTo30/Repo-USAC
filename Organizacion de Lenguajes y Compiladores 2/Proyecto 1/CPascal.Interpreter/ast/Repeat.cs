using System.Collections.Generic;

class Repeat : Instruccion
{
    public int Linea {get; set;}
    public int Columna {get; set;}
    private LinkedList<Instruccion> instrucciones;
    private Operacion condicion;

    public Repeat(Operacion condicion, LinkedList<Instruccion> instrucciones, int x, int y){
        this.condicion = condicion;
        this.instrucciones = instrucciones;
        this.Linea = x;
        this.Columna = y;
    }
    public object ejecutar(Entorno env){
        var condicional = condicion.ejecutar(env);
        if (!(condicional is bool))
            throw new SemanticException("Valor condicional no retorna un booleano", this.Linea, this.Columna);
        do
        {
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
            condicional = condicion.ejecutar(env);
        } while (!(bool)condicional);
        return Control.ControlSet.NONE;
    }
}