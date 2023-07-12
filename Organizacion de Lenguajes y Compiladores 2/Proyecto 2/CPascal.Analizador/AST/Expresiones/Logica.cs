using System.Collections.Generic;
using System.Linq;
public class Logica : Expresion
{
    public Posicion posicion {get; set;}
    public string ultimoTemporal {get; set;}
    public C3D.Print tipoPrint {get; set;}
    public enum Tipo
    {
        AND,
        OR,
        NOT
    }
    private Expresion operadorIzq;
    private Expresion operadorDer;
    private Tipo tipo;

    public Logica(Expresion izq, Expresion der, Tipo tipo, Posicion posicion){
        this.operadorIzq = izq;
        this.operadorDer = der;
        this.tipo = tipo;
        this.posicion = posicion;
    }
    public Logica(Expresion der, Tipo tipo, Posicion posicion){
        this.operadorIzq = null;
        this.operadorDer = der;
        this.tipo = tipo;
        this.posicion = posicion;
    }
    public long ObtenerValorImplicito(){
        return 1;
    }
    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        //Generamos operador Izquierdo
        List<C3D> codigo = new List<C3D>();
        if (operadorIzq != null)
        {
            List<C3D> izq = operadorIzq.GenerarC3D(tabla, ambito);
            List<C3D> der = operadorDer.GenerarC3D(tabla, ambito);
            codigo = der.Concat(izq).ToList();
            this.ultimoTemporal = Temporales.Correlativo;
            this.tipoPrint = C3D.Print.DIGITO;
            codigo.Add(new C3D(GetEquivalente(), operadorIzq.ultimoTemporal,  operadorDer.ultimoTemporal, this.ultimoTemporal));
            return codigo;
        } else {
            List<C3D> der = operadorDer.GenerarC3D(tabla, ambito);
            codigo = der;
            this.ultimoTemporal = Temporales.Correlativo;
            this.tipoPrint = C3D.Print.DIGITO;
            codigo.Add(new C3D(GetEquivalente(), "",  operadorDer.ultimoTemporal, this.ultimoTemporal));
            return codigo;
        }
    }

    public List<C3D> GenerarC3D(Tabla tabla, string ambito, string verdadero, string falso){
        List<C3D> codigo = new List<C3D>();
        if (tipo == Tipo.AND)
        {
            //PRIMERO GENERAMOS UNA PARTE DE AND IZQUIERDA
            // generamos una nueva etiqueta para condicion verdadera
            string nuevaEtiqueta = Saltos.Correlativo;
            codigo = codigo.Concat(operadorIzq.GenerarC3D(tabla, ambito, nuevaEtiqueta, falso)).ToList();
            //imprimimos la etiqueta
            codigo.Add(new C3D(C3D.Unario.LABEL, nuevaEtiqueta));
            // ahora se evalua la segunda condicion con las verdadero y falso anteriores
            codigo = codigo.Concat(operadorDer.GenerarC3D(tabla, ambito, verdadero, falso)).ToList();
            return codigo;
        } else if (tipo == Tipo.OR) {
            string nuevaEtiqueta = Saltos.Correlativo;
            codigo = codigo.Concat(operadorDer.GenerarC3D(tabla, ambito, verdadero, nuevaEtiqueta)).ToList();
            //imprimos la etiqueta
            codigo.Add(new C3D(C3D.Unario.LABEL, nuevaEtiqueta));
            //ahora la segunda condicion con los verdadero y falso normales
            codigo = codigo.Concat(operadorDer.GenerarC3D(tabla, ambito, verdadero, falso)).ToList();
            return codigo;
        }else{
            codigo = codigo.Concat(operadorDer.GenerarC3D(tabla, ambito, falso, verdadero)).ToList();
            return codigo;
        }
    }

    private C3D.Operador GetEquivalente(){
        switch (this.tipo)
        {
            case Tipo.AND:
                return C3D.Operador.AND;
            case Tipo.OR:
                return C3D.Operador.OR;
            default:
                return C3D.Operador.NOT;
        }
    }
}