using System.Collections.Generic;
using System.Linq;
public class Case : Instruccion
{
    public Posicion posicion {get; set;}

    private Expresion condicion;
    private List<CaseValue> values;

    public Case(Expresion condicion, List<CaseValue> values, Posicion posicion){
        this.condicion = condicion;
        this.values = values;
        this.posicion = posicion;
    }

    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        List<C3D> codigo = new List<C3D>();
        codigo = codigo.Concat(condicion.GenerarC3D(tabla, ambito)).ToList();
        string resto = Saltos.Correlativo;
        foreach (var @case in values)
        {
            if (@case.expresions.Count == 0)
            {
                //DEFAULT
                foreach (var ins in @case.bloque)
                    codigo = codigo.Concat(ins.GenerarC3D(tabla, ambito)).ToList();
            } else {
                foreach (var value in @case.expresions) {
                    string verdadero = Saltos.Correlativo;
                    string falso = Saltos.Correlativo;
                    //traducimos la expresion a evaluar
                    codigo = codigo.Concat(value.GenerarC3D(tabla, ambito)).ToList();
                    //añadimos if (<CASE> == <VALUE>) goto <VERDADERO>
                    codigo.Add(new C3D(C3D.Operador.IGUAL, condicion.ultimoTemporal, value.ultimoTemporal, verdadero, falso));
                    //añadimos salto a falso
                    //<VERDADERO>:
                    codigo.Add(new C3D(C3D.Unario.LABEL, verdadero));
                    foreach (var ins in @case.bloque)                
                        codigo = codigo.Concat(ins.GenerarC3D(tabla, ambito)).ToList();
                    //goto <FINDECODIGO>
                    codigo.Add(new C3D(C3D.Unario.GOTO, resto)); 
                    //<FALSO>:
                    codigo.Add(new C3D(C3D.Unario.LABEL, falso));               
                }
            }            
        }
        codigo.Add(new C3D(C3D.Unario.LABEL,resto));
        return codigo;
    }
}