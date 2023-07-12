using System.Collections.Generic;
using System.Collections;
class If : Instruccion
{
    public int Linea {get; set;}
    public int Columna {get; set;}
    private Operacion condicion;
    private LinkedList<Instruccion> trueIf;
    private LinkedList<Instruccion> falseIf;

    public If(Operacion condicion, LinkedList<Instruccion> trueIf, LinkedList<Instruccion> falseIf, int x, int y){
        this.condicion = condicion;
        this.trueIf = trueIf;
        this.falseIf = falseIf;
        this.Linea = x;
        this.Columna = y;
    }

    public If(Operacion condicion, LinkedList<Instruccion> trueIf, int x, int y){
        this.condicion = condicion;
        this.trueIf = trueIf;
        this.falseIf = null;
        this.Linea = x;
        this.Columna = y;
    }
    public object ejecutar(Entorno env){
        var condicion = this.condicion.ejecutar(env);
        if ((bool)condicion)
        {
            //ejecucion verdadera
            //Entorno Local = new Entorno(env);
            foreach (var ins in this.trueIf)
            {
                //ins.ejecutar(env);
                var res = ins.ejecutar(env);
                if (res is Control.ControlSet)
                {
                    switch ((Control.ControlSet)res)
                    {
                        case Control.ControlSet.BREAK:
                            return Control.ControlSet.BREAK;
                        case Control.ControlSet.CONTINUE:
                            return Control.ControlSet.CONTINUE;
                        case Control.ControlSet.EXIT:
                            return Control.ControlSet.EXIT;
                    }
                }
            }
            return Control.ControlSet.NONE;
        } else {
            //ejecucion falsa
            if (this.falseIf != null)
            {
                //bloque else
                //Entorno Local = new Entorno(env);
                foreach (var ins in this.falseIf)
                {
                    //ins.ejecutar(env);
                    var res = ins.ejecutar(env);
                    if (res is Control.ControlSet)
                    {
                        switch ((Control.ControlSet)res)
                        {
                            case Control.ControlSet.BREAK:
                                return Control.ControlSet.BREAK;
                            case Control.ControlSet.CONTINUE:
                                return Control.ControlSet.CONTINUE;
                            case Control.ControlSet.EXIT:
                                return Control.ControlSet.EXIT;
                        }
                    }
                }
                return Control.ControlSet.NONE;
            }else{
                return Control.ControlSet.NONE;
            }            
        }
    }
}