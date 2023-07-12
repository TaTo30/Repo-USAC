using System.Collections.Generic;
using System.Linq;

public class Relacional : Expresion
{
    public Posicion posicion {get; set;}
    public string ultimoTemporal {get; set;}
    public C3D.Print tipoPrint {get; set;}
    public enum Tipo
    {
        MENOR,
        MENORIGUAL,
        MAYOR,
        MAYORIGUAL,
        IGUAL,
        DIFERENTE
    }
    private Expresion OperadorIzq;
    private Expresion OperadorDer;
    private Tipo tipo;
    public Relacional(Expresion izq, Expresion der, Tipo tipo, Posicion posicion){
        this.posicion = posicion;
        this.OperadorIzq = izq;
        this.OperadorDer = der;
        this.tipo = tipo;
    }

    public long ObtenerValorImplicito(){
        return 1;
    }
    
    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        //Generamos operador Izquierdo
        List<C3D> codigo = new List<C3D>();
        List<C3D> izq = OperadorIzq.GenerarC3D(tabla, ambito);
        List<C3D> der = OperadorDer.GenerarC3D(tabla, ambito);
        codigo = der.Concat(izq).ToList();
        this.ultimoTemporal = Temporales.Correlativo;
        this.tipoPrint = C3D.Print.DIGITO;
        codigo.Add(new C3D(GetEquivalente(), OperadorIzq.ultimoTemporal,  OperadorDer.ultimoTemporal, this.ultimoTemporal));
        return codigo;
    }

    public List<C3D> GenerarC3D(Tabla tabla, string ambito, string verdadero, string falso){
        List<C3D> codigo = new List<C3D>();
        List<C3D> izq = OperadorIzq.GenerarC3D(tabla, ambito);
        List<C3D> der = OperadorDer.GenerarC3D(tabla, ambito);
        codigo = der.Concat(izq).ToList();
        this.ultimoTemporal = Temporales.Correlativo;
        this.tipoPrint = C3D.Print.DIGITO;
        codigo.Add(new C3D(GetEquivalente(), OperadorIzq.ultimoTemporal, OperadorDer.ultimoTemporal, verdadero, falso));
        return codigo;
    }

    private C3D.Operador GetEquivalente(){
        switch (this.tipo)
        {
            case Tipo.IGUAL:
                return C3D.Operador.IGUAL;
            case Tipo.DIFERENTE:
                return C3D.Operador.DIFERENTE;
            case Tipo.MAYOR:
                return C3D.Operador.MAYOR;
            case Tipo.MENOR:
                return C3D.Operador.MENOR;
            case Tipo.MAYORIGUAL:
                return C3D.Operador.MAYORIGUAL;
            default:
                return C3D.Operador.MENORIGUAL;
        }
    }
}