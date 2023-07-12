using System.Collections.Generic;
using System.Linq;
public class Metodo: Instruccion, Definicion
{
    public Posicion posicion {get; set;}
    public enum Tipo
    {
        FUNCION,
        PROCEDIMIENTO
    }

    private string identificador;
    private List<Variable> parametros;
    private Tipo mode;
    private List<Instruccion> bloqueDeclaracion;
    private List<Instruccion> bloqueEjecucion;
    private string tipo;

    public Metodo(string identificador, Tipo mode,List<Variable> parametros, string tipo, List<Instruccion> bloqueDeclaracion, List<Instruccion> bloqueEjecucion, Posicion posicion){
        this.identificador = identificador;
        this.parametros = parametros;
        this.tipo = tipo;
        this.mode = mode;
        this.bloqueDeclaracion = bloqueDeclaracion;
        this.bloqueEjecucion = bloqueEjecucion;
        this.posicion = posicion;
    }

    public void MapearGlobales(Tabla tabla, Ambito ambito){
        tabla.Add(new Simbolo(
            this.identificador,
            this.tipo,
            ambito.Nombre,
            "Funcion",
            null,
            posicion
        ));
        Ambito funcion = new Ambito(this.identificador);
        //Return
        tabla.Add(new Simbolo(this.identificador.ToLower(), this.tipo, funcion.Nombre, "Variable", funcion.Correlativo, posicion));
        
        //Parametros        
        foreach (var item in this.parametros)
            tabla.Add(new Simbolo(
                item.identificador,
                item.tipo,
                funcion.Nombre,
                "Variable",
                funcion.Correlativo,
                posicion
            ));
        //Declaraciones
        foreach (var item in this.bloqueDeclaracion)
        {
            var def = (Definicion)item;
            def.MapearGlobales(tabla, funcion);
        }
        
    }
    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        List<C3D> Codigo = new List<C3D>();
        Funcion func = new Funcion(this.identificador.ToLower(), Saltos.Correlativo);
        TresDirecciones.Funciones.Add(func);
        foreach (var item in this.bloqueDeclaracion)
            Codigo = Codigo.Concat(item.GenerarC3D(tabla, this.identificador)).ToList();
        foreach (var item in this.bloqueEjecucion)
            Codigo = Codigo.Concat(item.GenerarC3D(tabla, this.identificador)).ToList();
        Codigo.Add(new C3D(C3D.Unario.LABEL, func.SaltoReturn));
        Codigo.Add(new C3D(C3D.Unario.CALL, "return"));
        func.Codigo = Codigo;
        //TresDirecciones.Funciones.Add(new Funcion(this.identificador.ToLower(), Codigo));
        return new List<C3D>();
    }

}