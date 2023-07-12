using System.Collections.Generic;
using System.Linq;
public class Write : Instruccion
{
    public Posicion posicion {get; set;}

    private List<Expresion> expresions;
    private bool writeln;

    public Write(List<Expresion> expresions, bool writeln, Posicion posicion){
        this.expresions = expresions;
        this.writeln = writeln;
        this.posicion = posicion;
    }
    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        List<C3D> codigo = new List<C3D>();
        foreach (var exp in expresions)
        {
            codigo = codigo.Concat(exp.GenerarC3D(tabla, ambito)).ToList();

            if (exp.tipoPrint == C3D.Print.CARACTER)
            {
                string heapTemporal = Temporales.Correlativo;
                string valueTemporal = Temporales.Correlativo;
                string inicio = Saltos.Correlativo;
                string print = Saltos.Correlativo;
                string noPrint = Saltos.Correlativo;
                codigo.Add(new C3D(C3D.Operador.NONE, "", exp.ultimoTemporal, heapTemporal));
                codigo.Add(new C3D(C3D.Unario.LABEL, inicio));
                codigo.Add(new C3D(C3D.Operador.NONE, "", $"Heap[{heapTemporal}]", valueTemporal));
                codigo.Add(new C3D(C3D.Operador.IGUAL, valueTemporal, "36", noPrint, print));
                codigo.Add(new C3D(C3D.Unario.LABEL, print));
                codigo.Add(new C3D(C3D.Print.CARACTER, valueTemporal));
                codigo.Add(new C3D(C3D.Operador.ADICION, heapTemporal, "1", heapTemporal));
                codigo.Add(new C3D(C3D.Unario.GOTO, inicio));
                codigo.Add(new C3D(C3D.Unario.LABEL, noPrint));
            } else 
                codigo.Add(new C3D(C3D.Print.DIGITO, exp.ultimoTemporal));            
        }
        if (writeln)
                codigo.Add(new C3D(C3D.Print.CARACTER, $"{10}"));
        return codigo;
    }
}

/*
T0 = T<exp> //posicion inicial del string en heap
L0:
T1 = Heap[T0]       //obtenemos el valor
if (T1 == 36) goto L2;
goto L1;
L1:
print("%c", T1);
T0 = T0 + 1
goto L0;
L2:

*/
