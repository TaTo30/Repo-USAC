using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
class Graficador
{
    private int node = 0;
    private string dot = "";
    private string tsname = "";
    private List<Error> errores;
    public enum Graph {
        AST,
        TS,
        ERROR
    }    
    
    //CONSTRUCTOR DE AST
    public Graficador(ParseTreeNode raiz){
        GenerarAST(raiz, ref this.node);
    }

    //CONSTRUCTOR DE AMBITOS
    public Graficador(Entorno env){
        GenerarTS(env);
    }

    //CONSTRUCTOR DE ERRORES
    public Graficador(){
        this.errores = new List<Error>();
    }

    public void AgregarError(Error err){
        this.errores.Add(err);
    }

    private void GenerarReporte() {
        foreach (var err in this.errores)
        {
            string fields = "";
            fields += String.Format("<td BORDER=\"1\">{0}</td>\n", err.tipo == Error.Tipo.LEXICO? "Lexico": err.tipo == Error.Tipo.SINTACTICO? "Sintactico": "Semantico");
            fields += String.Format("<td BORDER=\"1\">{0}</td>\n", err.Mensaje.Replace("<", "MENOR").Replace(">", "MAYOR"));
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
    private void GenerarTS(Entorno env){
        foreach (var item in env)
        {
            string fields = "";
            fields += String.Format("<td BORDER=\"1\">{0}</td>\n", item.GetId());
            fields += String.Format("<td BORDER=\"1\">{0}</td>\n", item.GetTipo().ToString());
            fields += String.Format("<td BORDER=\"1\">{0}</td>\n", item.GetEnv());
            fields += String.Format("<td BORDER=\"1\">{0}</td>\n", 0);
            fields += String.Format("<td BORDER=\"1\">{0}</td>\n", 0);
            this.dot += String.Format("<tr>\n{0}</tr>\n", fields);
        }
        string cabecera =
        "<tr>" + 
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Nombre</td>" +
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Tipo</td>" +
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Ambito</td>" +
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Linea</td>" +
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\"> Columna</td>" +
        "</tr>";

        this.dot = String.Format("<table CELLPADDING=\"5\" CELLSPACING=\"0\" BORDER=\"0\"> {0}{1}</table>", cabecera, this.dot);
        this.dot = String.Format("node [shape = none];\n a0[label = <{0}>];\n", this.dot);
        this.tsname = env.nombre;
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
    public void Print(Graph tipo){
        StreamWriter redactor;
        switch (tipo)
        {
            case Graph.AST:
                redactor = new StreamWriter("AST.dot");
                break;
            case Graph.TS:
                redactor = new StreamWriter(this.tsname+".dot");
                break;
            default:
                GenerarReporte();
                redactor = new StreamWriter("Error.dot");
                break;          
        }        
        redactor.Write("digraph g {\n" +this.dot + "}");
        redactor.Close();
    }

}