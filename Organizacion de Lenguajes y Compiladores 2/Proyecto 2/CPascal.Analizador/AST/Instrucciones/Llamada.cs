using System.Collections.Generic;
using System.Linq;

public class Llamada: Instruccion, Expresion
{
    public Posicion posicion {get; set;}
    public string ultimoTemporal {get; set;}
    public C3D.Print tipoPrint {get; set;}
    public enum Tipo
    {
        FUNCION,
        PROCEDIMIENTO
    }
    private List<Expresion> expresiones;
    private string identificador;
    private Tipo mode;

    public Llamada(string identificador, List<Expresion> expresiones, Tipo mode, Posicion posicion){
        this.identificador = identificador;
        this.expresiones = expresiones;
        this.mode = mode;
        this.posicion = posicion;
    }

    /*

        Falta de Implementar Print
    
    */

    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        List<C3D> codigo = new List<C3D>();
        int currentAmbitoSize = tabla.GetAmbitoSize(ambito);
        string variableEmulada = Temporales.Correlativo;
        foreach (var exp in GetCalls())
        {
            //string temporalAsignacion = Temporales.Correlativo;
            codigo = codigo.Concat(exp.GenerarC3D(tabla, ambito)).ToList();
            //codigo.Add(new C3D(C3D.Operador.ADICION, variableEmulada, $"{this.expresiones.IndexOf(exp)+1}", temporalAsignacion));
            //codigo.Add(new C3D(C3D.Operador.NONE, "", exp.ultimoTemporal, $"Stack[{temporalAsignacion}]"));
        }
        codigo.Add(new C3D(C3D.Operador.ADICION, "SP", $"{currentAmbitoSize}", variableEmulada));
        foreach (var exp in GetCalls())
        {
            string temporalAsignacion = Temporales.Correlativo;
            codigo.Add(new C3D(C3D.Operador.ADICION, variableEmulada, $"{this.expresiones.IndexOf(exp)+1}", temporalAsignacion));
            codigo.Add(new C3D(C3D.Operador.NONE, "", exp.ultimoTemporal, $"Stack[{temporalAsignacion}]"));
        }
        foreach (var exp in GetOthers())
        {
            string temporalAsignacion = Temporales.Correlativo;
            codigo = codigo.Concat(exp.GenerarC3D(tabla, ambito)).ToList();
            codigo.Add(new C3D(C3D.Operador.ADICION, variableEmulada, $"{this.expresiones.IndexOf(exp)+1}", temporalAsignacion));
            codigo.Add(new C3D(C3D.Operador.NONE, "", exp.ultimoTemporal, $"Stack[{temporalAsignacion}]"));
        }
        codigo.Add(new C3D(C3D.Operador.ADICION, "SP",  $"{currentAmbitoSize}", "SP"));
        codigo.Add(new C3D(C3D.Unario.CALL, $"{this.identificador}()"));//llamada formal
        string temporalRetorno = Temporales.Correlativo;
        codigo.Add(new C3D(C3D.Operador.ADICION, "SP", $"{0}", temporalRetorno));
        this.ultimoTemporal = Temporales.Correlativo;
        codigo.Add(new C3D(C3D.Operador.NONE, "", $"Stack[{temporalRetorno}]", this.ultimoTemporal));
        codigo.Add(new C3D(C3D.Operador.SUSTRACCION, "SP", $"{currentAmbitoSize}", "SP"));
        return codigo;
    }

    public List<C3D> GenerarC3D(Tabla tabla, string ambito, string verdadero, string falso){
        List<C3D> codigo = new List<C3D>();
        int currentAmbitoSize = tabla.GetAmbitoSize(ambito);
        string variableEmulada = Temporales.Correlativo;
        foreach (var exp in GetCalls())
        {
            //string temporalAsignacion = Temporales.Correlativo;
            codigo = codigo.Concat(exp.GenerarC3D(tabla, ambito)).ToList();
            //codigo.Add(new C3D(C3D.Operador.ADICION, variableEmulada, $"{this.expresiones.IndexOf(exp)+1}", temporalAsignacion));
            //codigo.Add(new C3D(C3D.Operador.NONE, "", exp.ultimoTemporal, $"Stack[{temporalAsignacion}]"));
        }
        codigo.Add(new C3D(C3D.Operador.ADICION, "SP", $"{currentAmbitoSize}", variableEmulada));
        foreach (var exp in GetCalls())
        {
            string temporalAsignacion = Temporales.Correlativo;
            codigo.Add(new C3D(C3D.Operador.ADICION, variableEmulada, $"{this.expresiones.IndexOf(exp)+1}", temporalAsignacion));
            codigo.Add(new C3D(C3D.Operador.NONE, "", exp.ultimoTemporal, $"Stack[{temporalAsignacion}]"));
        }
        foreach (var exp in GetOthers())
        {
            string temporalAsignacion = Temporales.Correlativo;
            codigo = codigo.Concat(exp.GenerarC3D(tabla, ambito)).ToList();
            codigo.Add(new C3D(C3D.Operador.ADICION, variableEmulada, $"{this.expresiones.IndexOf(exp)+1}", temporalAsignacion));
            codigo.Add(new C3D(C3D.Operador.NONE, "", exp.ultimoTemporal, $"Stack[{temporalAsignacion}]"));
        }
        codigo.Add(new C3D(C3D.Operador.ADICION, "SP",  $"{currentAmbitoSize}", "SP"));
        codigo.Add(new C3D(C3D.Unario.CALL, $"{this.identificador}()"));//llamada formal
        string temporalRetorno = Temporales.Correlativo;
        codigo.Add(new C3D(C3D.Operador.ADICION, "SP", $"{0}", temporalRetorno));
        this.ultimoTemporal = Temporales.Correlativo;
        codigo.Add(new C3D(C3D.Operador.NONE, "", $"Stack[{temporalRetorno}]", this.ultimoTemporal));
        codigo.Add(new C3D(C3D.Operador.SUSTRACCION, "SP", $"{currentAmbitoSize}", "SP"));
        return codigo;
    }

    private List<Expresion> GetCalls(){
        List<Expresion> list = new List<Expresion>();
        foreach (var item in this.expresiones)
            if (item is Llamada)
                list.Add(item);
        return list;
    }
    private List<Expresion> GetOthers(){
        List<Expresion> list = new List<Expresion>();
        foreach (var item in this.expresiones)
            if (!(item is Llamada))
                list.Add(item);
        return list;
    }

    public long ObtenerValorImplicito(){
        return 0;
    }
}

