using System.Collections.Generic;
using System.Linq;
public class Exit: Instruccion
{
    public Posicion posicion {get; set;}
    private Expresion expresion;

    public Exit(Expresion expresion, Posicion posicion){
        this.expresion = expresion;
        this.posicion = posicion;
    }
    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        List<C3D> codigo = new List<C3D>();
        if (this.expresion != null)
            codigo = codigo.Concat(this.expresion.GenerarC3D(tabla, ambito)).ToList();
        string temporal = Temporales.Correlativo;
        int pointer = (int)tabla.GetPointer(ambito.ToLower(), ambito);
        // T0 = SP + <POINTER>
        // Stack[T0] = T<RESULT>
        codigo.Add(new C3D(C3D.Operador.ADICION, "SP", $"{pointer}", temporal));
        if (this.expresion != null)
            codigo.Add(new C3D(C3D.Operador.NONE, "", this.expresion.ultimoTemporal, $"Stack[{temporal}]"));
        else 
            codigo.Add(new C3D(C3D.Operador.NONE, "", "0", $"Stack[{temporal}]"));
        foreach (var func in TresDirecciones.Funciones)
            if (func.Nombre == ambito.ToLower())
                codigo.Add(new C3D(C3D.Unario.GOTO, func.SaltoReturn));
        return codigo;
    }
}