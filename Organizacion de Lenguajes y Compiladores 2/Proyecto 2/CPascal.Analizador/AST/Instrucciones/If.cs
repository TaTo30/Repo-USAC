using System.Collections.Generic;
using System.Linq;
public class If: Instruccion
{
    public Posicion posicion {get; set;}

    private Expresion condicion;
    private List<Instruccion> bloqueIf;
    private List<Instruccion> bloqueElse;

    public If(Expresion condicion, List<Instruccion> bloqueIf, List<Instruccion> bloqueElse, Posicion posicion){
        this.condicion = condicion;
        this.bloqueElse = bloqueElse;
        this.bloqueIf = bloqueIf;
        this.posicion = posicion;
    }
    public If(Expresion condicion, List<Instruccion> bloqueIf, Posicion posicion){
        this.condicion = condicion;
        this.bloqueIf = bloqueIf;
        this.posicion = posicion;
    }
    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        List<C3D> codigo = new List<C3D>();
        if (this.bloqueElse != null)
        {
            string saltoVerdadero = Saltos.Correlativo;
            string saltoElse = Saltos.Correlativo;
            string saltoResto = Saltos.Correlativo;
            codigo = codigo.Concat(condicion.GenerarC3D(tabla, ambito, saltoVerdadero, saltoElse)).ToList();
            codigo.Add(new C3D(C3D.Unario.LABEL, saltoVerdadero));
            foreach (var item in this.bloqueIf)            
                codigo = codigo.Concat(item.GenerarC3D(tabla, ambito)).ToList();
            codigo.Add(new C3D(C3D.Unario.GOTO,saltoResto));
            codigo.Add(new C3D(C3D.Unario.LABEL, saltoElse));
            foreach (var item in this.bloqueElse)
                codigo = codigo.Concat(item.GenerarC3D(tabla, ambito)).ToList();
            codigo.Add(new C3D(C3D.Unario.GOTO,saltoResto));
            codigo.Add(new C3D(C3D.Unario.LABEL, saltoResto));
            return codigo;
        } else {
            string saltoVerdadero = Saltos.Correlativo;
            string saltoResto = Saltos.Correlativo;
            codigo = codigo.Concat(condicion.GenerarC3D(tabla, ambito, saltoVerdadero, saltoResto)).ToList();
            codigo.Add(new C3D(C3D.Unario.LABEL, saltoVerdadero));
            foreach (var item in this.bloqueIf)            
                codigo = codigo.Concat(item.GenerarC3D(tabla, ambito)).ToList();
            codigo.Add(new C3D(C3D.Unario.GOTO,saltoResto));
            codigo.Add(new C3D(C3D.Unario.LABEL, saltoResto));
            return codigo;
        }
    }
}