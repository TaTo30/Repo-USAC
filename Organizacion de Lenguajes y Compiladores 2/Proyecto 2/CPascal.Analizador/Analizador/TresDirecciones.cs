using System.Collections.Generic;
using System.Linq;
public static class TresDirecciones
{
    public static List<Funcion> Funciones = new List<Funcion>();
    public static LinkedList<Aldo> Controles = new LinkedList<Aldo>();
    

    public static List<Funcion> Traducir(Pascal pascal){
        List<C3D> Codigo = new List<C3D>();
        Funcion func = new Funcion("main", Saltos.Correlativo);
        Funciones.Add(func);
        Codigo.Add(new C3D(C3D.Operador.ADICION, "HP", $"{pascal.TablaSimbolos.GetAmbitoSize("Global")}", "HP"));
        foreach (var item in pascal.Declarativo)
            Codigo = Codigo.Concat(item.GenerarC3D(pascal.TablaSimbolos, "Global")).ToList();
        foreach (var item in pascal.Ejecutivo)
            Codigo = Codigo.Concat(item.GenerarC3D(pascal.TablaSimbolos, "Global")).ToList();
        Codigo.Add(new C3D(C3D.Unario.LABEL, func.SaltoReturn));
        Codigo.Add(new C3D(C3D.Unario.CALL, "return"));
        func.Codigo = Codigo;
        return Funciones;
    }

    public static string Parse(){
        string traduccion = "";
        foreach (var func in Funciones)
        {
            string codeblock = "";
            foreach (var cod in func.Codigo)            
                codeblock += $"{cod.ToString()}\n";
            traduccion += $"void {func.Nombre}() {{\n{codeblock}}}\n\n";
        }
        string encabezado = "#include <stdio.h>\n";
        encabezado += "float Heap[100000];\n";
        encabezado += "float Stack[100000];\n";
        encabezado += "int SP;\n";
        encabezado += "int HP;\n";
        encabezado += "int ";
        for (int i = 0; i < Temporales.Count; i++)        
            encabezado += i != Temporales.Count - 1? $"T{i}, " : $"T{i};\n";
        return $"{encabezado}\n{traduccion}";
    }
}