using System.Collections.Generic;
using System.Linq;
public class Aritmetica : Expresion
{
    public Posicion posicion {get; set;}
    public string ultimoTemporal {get; set;}
    public C3D.Print tipoPrint {get; set;}
    public enum Tipo
    {
        ADICION,
        SUSTRACCION,
        MULTIPLICACION,
        DIVISION,
        MODULO,
        NEGACION
    }
    private Expresion OperadorIzq;
    private Expresion OperadorDer;
    private Tipo tipo;
    public Aritmetica(Expresion izq, Expresion der, Tipo tipo, Posicion posicion){
        this.OperadorIzq = izq;
        this.OperadorDer = der;
        this.tipo = tipo;
        this.posicion = posicion;
    }
    public Aritmetica(Expresion der, Tipo tipo, Posicion posicion){
        this.OperadorDer = der;
        this.OperadorIzq = null;
        this.tipo = tipo;
        this.posicion = posicion;
    }
    public long ObtenerValorImplicito(){
        return 1;
    }

    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        //Generamos operador Izquierdo
        List<C3D> codigo = new List<C3D>();
        if (OperadorIzq != null)
        {
            List<C3D> izq = OperadorIzq.GenerarC3D(tabla, ambito);
            List<C3D> der = OperadorDer.GenerarC3D(tabla, ambito);
            codigo = der.Concat(izq).ToList();
            this.ultimoTemporal = Temporales.Correlativo;
            this.tipoPrint = C3D.Print.DIGITO;
            codigo.Add(new C3D(GetEquivalente(), OperadorIzq.ultimoTemporal,  OperadorDer.ultimoTemporal, this.ultimoTemporal));
            return codigo;
        } else {
            List<C3D> der = OperadorDer.GenerarC3D(tabla, ambito);
            codigo = der;
            this.ultimoTemporal = Temporales.Correlativo;
            this.tipoPrint = C3D.Print.DIGITO;
            codigo.Add(new C3D(GetEquivalente(), "",  OperadorDer.ultimoTemporal, this.ultimoTemporal));
            return codigo;
        }
    }

    public List<C3D> GenerarC3D(Tabla tabla, string ambito, string verdadero, string falso){
        //Generamos operador Izquierdo
        List<C3D> codigo = new List<C3D>();
        if (OperadorIzq != null)
        {
            List<C3D> izq = OperadorIzq.GenerarC3D(tabla, ambito);
            List<C3D> der = OperadorDer.GenerarC3D(tabla, ambito);
            codigo = der.Concat(izq).ToList();
            this.ultimoTemporal = Temporales.Correlativo;
            this.tipoPrint = C3D.Print.DIGITO;
            codigo.Add(new C3D(GetEquivalente(), OperadorIzq.ultimoTemporal,  OperadorDer.ultimoTemporal, this.ultimoTemporal));
            return codigo;
        } else {
            List<C3D> der = OperadorDer.GenerarC3D(tabla, ambito);
            codigo = der;
            this.ultimoTemporal = Temporales.Correlativo;
            this.tipoPrint = C3D.Print.DIGITO;
            codigo.Add(new C3D(GetEquivalente(), "",  OperadorDer.ultimoTemporal, this.ultimoTemporal));
            return codigo;
        }
    }

    private C3D.Operador GetEquivalente(){
        switch (this.tipo)
        {
            case Tipo.ADICION:
                return C3D.Operador.ADICION;
            case Tipo.SUSTRACCION:
                return C3D.Operador.SUSTRACCION;
            case Tipo.MULTIPLICACION:
                return C3D.Operador.MULTIPLICACION;
            case Tipo.DIVISION:
                return C3D.Operador.DIVISION;
            case Tipo.MODULO:
                return C3D.Operador.MODULO;
            default:
                return C3D.Operador.NEGACION;
        }
    }
}