using System.Collections.Generic;

class Case : Instruccion
{
    public int Linea {get; set;}
    public int Columna {get; set;}
    private Operacion expresion;
    private LinkedList<CaseValue> valores;

    public Case(Operacion expresion, LinkedList<CaseValue> valores){
        this.expresion = expresion;
        this.valores = valores;
        this.Linea = 0;
        this.Columna = 0;
    }
    public object ejecutar(Entorno env){
        object valor = expresion.ejecutar(env);
        foreach (var item in valores)
        {
            var res = item.ejecutar(env, valor);
            //Si no se valida el case, se salta a la siguiente iteracion
            if (res == null)
                continue;
            //Si se valida el case, se retorna el valor
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
            return Control.ControlSet.NONE; 
        }
        return Control.ControlSet.NONE;
    }
}