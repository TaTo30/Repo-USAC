using System.Collections.Generic;
public interface Instruccion
{
    Posicion posicion {get; set;}    
    List<C3D> GenerarC3D(Tabla tabla, string ambito);
}