using System.Collections.Generic;
public class Control : Instruccion
{
    public Posicion posicion {get; set;}
    public enum Tipo
    {
        BREAK,
        CONTINUE,
        NONE
    }

    private Tipo tipo;
    public Control(Tipo tipo, Posicion posicion){
        this.tipo = tipo;
        this.posicion = posicion;
    }
    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        List<C3D> codigo = new List<C3D>();
        switch (tipo)
        {
            case Tipo.BREAK:
                codigo.Add(new C3D(C3D.Unario.GOTO, TresDirecciones.Controles.First.Value.@break));
                return codigo;
            case Tipo.CONTINUE:
                codigo.Add(new C3D(C3D.Unario.GOTO, TresDirecciones.Controles.First.Value.@continue));
                return codigo;
            default:
                return codigo;
        }
    }
}