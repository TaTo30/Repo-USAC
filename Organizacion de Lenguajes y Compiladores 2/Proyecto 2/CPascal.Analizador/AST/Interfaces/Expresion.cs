using System.Collections.Generic;
public interface Expresion : Instruccion
{
    string ultimoTemporal {get; set;}
    C3D.Print tipoPrint {get; set;}
    //public Simbolo.Tipo tipo {get; set;} 
    List<C3D> GenerarC3D(Tabla tabla, string ambito, string verdadero, string falso);
    long ObtenerValorImplicito();
    
}