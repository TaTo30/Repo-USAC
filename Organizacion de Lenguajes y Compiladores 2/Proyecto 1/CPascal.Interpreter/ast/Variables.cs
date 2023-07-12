using System.Collections.Generic;
class Variables : Instruccion
{
    public int Linea {get; set;}
    public int Columna {get; set;}
    LinkedList<Declaracion> declaraciones;
    public Variables(LinkedList<Declaracion> list){
        this.declaraciones = list;
        this.Linea = 0;
        this.Columna = 0;
    }
    public object ejecutar(Entorno env){
        foreach (var dec in this.declaraciones)
        {
            dec.ejecutar(env);
        }
        return Control.ControlSet.NONE;
    }
}