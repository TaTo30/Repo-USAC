using System.Collections.Generic;
using System.Linq;
public class For : Instruccion
{
    public Posicion posicion {get; set;}

    private Asignacion asignacion;
    private Expresion condicion;
    private List<Instruccion> bloqueFor;
    private bool DownTo;

    public For(Asignacion asignacion, Expresion condicion, List<Instruccion> bloqueFor, bool DownTo, Posicion posicion){
        this.posicion = posicion;
        this.asignacion = asignacion;
        this.condicion = condicion;
        this.bloqueFor = bloqueFor;
        this.DownTo = DownTo;
    }
    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        List<C3D> codigo = new List<C3D>();
        string saltoInicio = Saltos.Correlativo;
        string saltoFor = Saltos.Correlativo;
        string saltoResto = Saltos.Correlativo;
        //CREAMOS LA ASIGNACION INICIAL
        // a := 0;
        codigo = codigo.Concat(asignacion.GenerarC3D(tabla, ambito)).ToList();
        //  L1:
        codigo.Add(new C3D(C3D.Unario.LABEL, saltoInicio));
        //  T0 = a
        codigo = codigo.Concat(asignacion.acceso.GenerarC3D(tabla, ambito)).ToList(); //AQUI OBTENGO UN TEMPORAL
        string temporalAsignacion = asignacion.acceso.ultimoTemporal;

        //EVALUAMOS LA CONDICION
        codigo = codigo.Concat(condicion.GenerarC3D(tabla, ambito)).ToList();
        string temporalCondicion = condicion.ultimoTemporal;

        //hacemos el if
        codigo.Add(new C3D(DownTo? C3D.Operador.MAYORIGUAL : C3D.Operador.MENORIGUAL, temporalAsignacion, temporalCondicion, saltoFor, saltoResto));
        //  L2:
        codigo.Add(new C3D(C3D.Unario.LABEL, saltoFor));
        TresDirecciones.Controles.AddFirst(new Aldo(saltoInicio, saltoResto));
        foreach (var ins in bloqueFor)
            codigo = codigo.Concat(ins.GenerarC3D(tabla, ambito)).ToList();   
        TresDirecciones.Controles.RemoveFirst();      
        Posicion posicion = new Posicion(0,0,0);
        var newAsignacion = new Asignacion(asignacion.acceso, new Aritmetica(asignacion.acceso, new Primitiva(1, posicion), DownTo? Aritmetica.Tipo.SUSTRACCION: Aritmetica.Tipo.ADICION, posicion), posicion);
        codigo = codigo.Concat(newAsignacion.GenerarC3D(tabla, ambito)).ToList();
        //  goto L1
        codigo.Add(new C3D(C3D.Unario.GOTO, saltoInicio));
        //  L3:
        codigo.Add(new C3D(C3D.Unario.LABEL, saltoResto));
        return codigo;
    }
}