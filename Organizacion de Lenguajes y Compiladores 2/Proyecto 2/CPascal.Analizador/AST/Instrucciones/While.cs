using System.Collections.Generic;
using System.Linq;
public class While: Instruccion
{
    public Posicion posicion {get; set;}
    private List<Instruccion> bloqueWhile;
    private Expresion condicion;

    public While(Expresion condicion, List<Instruccion> bloqueWhile, Posicion posicion){
        this.posicion = posicion;
        this.condicion = condicion;
        this.bloqueWhile = bloqueWhile;
    }
    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        List<C3D> codigo = new List<C3D>();
        string inicioWhile = Saltos.Correlativo;
        string bloqueWhile = Saltos.Correlativo;
        string resto = Saltos.Correlativo;

        codigo.Add(new C3D(C3D.Unario.LABEL, inicioWhile));
        codigo = codigo.Concat(condicion.GenerarC3D(tabla, ambito, bloqueWhile, resto)).ToList();
        codigo.Add(new C3D(C3D.Unario.LABEL, bloqueWhile));
        TresDirecciones.Controles.AddFirst(new Aldo(inicioWhile, resto));
        foreach (var ins in this.bloqueWhile)
            codigo = codigo.Concat(ins.GenerarC3D(tabla, ambito)).ToList();
        TresDirecciones.Controles.RemoveFirst();
        codigo.Add(new C3D(C3D.Unario.GOTO, inicioWhile));
        codigo.Add(new C3D(C3D.Unario.LABEL, resto));
        return codigo;
    }
}