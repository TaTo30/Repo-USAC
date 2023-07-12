using System.Collections.Generic;
public class Graficar : Instruccion
{
    public Posicion posicion {get; set;}

    public Graficar(Posicion posicion){
        this.posicion = posicion;
    }
    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        return new List<C3D>();
    }
}