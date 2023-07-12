using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
class Graficador
{
    private int node = 0;
    private string dot = "";
    public enum Graph {
        AST,
        TS,
        ERROR
    }
    public Graficador(ParseTreeNode raiz){
        GenerarAST(raiz, ref this.node);
    }
    public string StringDimensiones(List<int> D){
        string str = "";
        foreach (var item in D)
        {
            str += $"{item}, ";
        }
        return str.Substring(0, str.Length-2);
    }
    public Graficador(List<Simbolo> tabla){
        foreach (var item in tabla)
        {
            string fields = "";
            fields += $"<td BORDER=\"1\">{item.Nombre}</td>\n";
            fields += $"<td BORDER=\"1\">{item.Ambito}</td>\n";
            fields += $"<td BORDER=\"1\">{item.Tipo}</td>\n";
            fields += $"<td BORDER=\"1\">{item.Rol}</td>\n";
            fields += $"<td BORDER=\"1\">{item.Apuntador}</td>\n";
            fields += $"<td BORDER=\"1\">{(item.Posicion == null? 0: item.Posicion.Linea)}</td>\n";
            fields += $"<td BORDER=\"1\">{(item.Posicion == null? 0: item.Posicion.Columna)}</td>\n";
            fields += $"<td BORDER=\"1\">{(item.Dimensiones.Count != 0? StringDimensiones(item.Dimensiones) : "")}</td>";
            this.dot += $"<tr>\n{fields}</tr>\n";
        }
        string cabecera =
        "<tr>" + 
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Nombre</td>" +
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Ambito</td>" +
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Tipo</td>" +
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Rol</td>" +
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Apuntador</td>" +
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Linea</td>" +
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Columna</td>" +
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Dimensiones</td>" +
        "</tr>";

        this.dot = $"<table CELLPADDING=\"5\" CELLSPACING=\"0\" BORDER=\"0\"> {cabecera}{this.dot}</table>";
        this.dot = $"node [shape = none];\n a0[label = <{this.dot}>];\n";
    }
    private void GenerarAST(ParseTreeNode raiz, ref int node, int parent = -1){
        // Imprimimos el nodo
        this.dot += String.Format("node{0}[label=\"{1}\"];\n",node,raiz.ToString());
        // Si el valor padre es mayor que 0 entonces se asocia con su hijo
        if (parent >= 0)
        {
            this.dot += String.Format("node{0} -> node{1};\n",parent, node);
        }
        // Nuestro nodo tiene hijos, los vamos a graficar 
        if (raiz.ChildNodes.Count != 0)
        {
            var temporal = node;
            foreach (var parsenode in raiz.ChildNodes)
            {
                node++;
                GenerarAST(parsenode, ref node, temporal);
            }
        }
    }
    public Graficador(List<PascalExcepcion> errores) {
        foreach (var err in errores)
        {
            string fields = "";
            fields += String.Format("<td BORDER=\"1\">{0}</td>\n", err.Tipo == PascalExcepcion.ParseError.LEXICO? "Lexico": err.Tipo == PascalExcepcion.ParseError.SINTACTIO? "Sintactico": "Semantico");
            fields += String.Format("<td BORDER=\"1\">{0}</td>\n", err.Message);
            fields += String.Format("<td BORDER=\"1\">{0}</td>\n", err.Linea);
            fields += String.Format("<td BORDER=\"1\">{0}</td>\n", err.Columna);
            this.dot += String.Format("<tr>\n{0}</tr>\n", fields);
        }
        string cabecera =
        "<tr>" + 
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Tipo</td>" +
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Mensaje</td>" +
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Linea</td>" +
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\"> Columna</td>" +
        "</tr>";

        this.dot = String.Format("<table CELLPADDING=\"5\" CELLSPACING=\"0\" BORDER=\"0\"> {0}{1}</table>", cabecera, this.dot);
        this.dot = String.Format("node [shape = none];\n a0[label = <{0}>];\n", this.dot);

    }
    public void Print(Graph tipo){
        StreamWriter redactor;
        switch (tipo)
        {
            case Graph.AST:
                redactor = new StreamWriter("./AST.dot");
                break;
            case Graph.TS:
                redactor = new StreamWriter("./TS.dot");
                break;
            case Graph.ERROR:
                redactor = new StreamWriter("./Error.dot");
                break;
            default:
                return;       
        }        
        redactor.Write("digraph g {\n" +this.dot + "}");
        redactor.Close();
    }

}