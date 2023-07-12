using System.Collections.Generic;
using System.Linq;
public class Asignacion : Instruccion
{
    public Posicion posicion {get; set;}

    public Acceso acceso {get; set;}
    public Expresion expresion {get; set;}
    public Asignacion(Acceso acceso, Expresion expresion, Posicion posicion){
        this.acceso = acceso;
        this.expresion = expresion;
        this.posicion = posicion;
    }
   
    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        List<C3D> codigo = new List<C3D>();
        codigo = codigo.Concat(expresion.GenerarC3D(tabla, ambito)).ToList();
        codigo = codigo.Concat(acceso.GenerarC3D(tabla, ambito, expresion.ultimoTemporal)).ToList();
        return codigo;
    }
}