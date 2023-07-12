using System.Collections.Generic;
using System.Linq;
public class Repeat: Instruccion
{
    public Posicion posicion {get; set;}

    private List<Instruccion> bloqueRepeat;
    private Expresion condicion;

    public Repeat(Expresion condicion, List<Instruccion> bloqueRepeat, Posicion posicion){
        this.posicion = posicion;
        this.condicion = condicion;
        this.bloqueRepeat = bloqueRepeat;
    }
    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        List<C3D> codigo = new List<C3D>();
        string inicioRepeat = Saltos.Correlativo;
        string bloqueRepeat = Saltos.Correlativo;
        string resto = Saltos.Correlativo;
        codigo.Add(new C3D(C3D.Unario.LABEL, inicioRepeat));
        //ejecutados todo una vez
        TresDirecciones.Controles.AddFirst(new Aldo(inicioRepeat, resto));
        foreach (var ins in this.bloqueRepeat)
            codigo = codigo.Concat(ins.GenerarC3D(tabla, ambito)).ToList();
        TresDirecciones.Controles.RemoveFirst();
        codigo = codigo.Concat(condicion.GenerarC3D(tabla, ambito, resto, inicioRepeat)).ToList();
        codigo.Add(new C3D(C3D.Unario.LABEL, resto));
        return codigo;
    }
}