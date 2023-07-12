using System.Collections.Generic;

class CaseValue
{
    LinkedList<Operacion> caselist;
    LinkedList<Instruccion> instrucciones;

    public CaseValue(LinkedList<Operacion> caselist, LinkedList<Instruccion> instrucciones){
        this.caselist = caselist;
        this.instrucciones = instrucciones;
    }

    public CaseValue(LinkedList<Instruccion> instrucciones){
        this.instrucciones = instrucciones;
        this.caselist = null;
    }

    public object ejecutar(Entorno env, object valor){
        if (this.caselist == null)
        {
            //En este caso si se llega a else SIEMPRE se ejecutara
            foreach (var ins in instrucciones)
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
            bool bandera = false;
            foreach (var op in this.caselist)
            {
                if (op.ejecutar(env).ToString().Equals(valor.ToString())) 
                {
                    bandera = true;
                    break;
                }
            }
            if (bandera) 
            {
                //cumple esta condicion, se ejecutan los bloques
                foreach (var ins in instrucciones)
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
                //no se cumple la condicion, retornamos un null
                return null;
            }
        }
    }
}