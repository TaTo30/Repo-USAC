using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimizador
{
   public class Analizador
    {
        public enum Status
        {
            ERROR,
            OPTIMIZADO,
            PARCIAL
        }
        
        public Dictionary<string, List<IC3D>> ListaFunciones {get; set;}
        public List<Bitacora> Reporte {get; set;}
        public List<string> Errores {get; set;}
        public Status Success {get; set;}
        public string Cabecera {get; set;}
        public string Cuerpo {
            get{
                string Traduccion = "";
                foreach (var item in this.ListaFunciones)
                {
                    string instrucciones = "";
                    foreach (var ins in item.Value)
                        instrucciones += $"{ins.ParseString()}\n";
                    Traduccion += $"\nvoid {item.Key}() {{\n{instrucciones}}}\n";
                }
                return Traduccion;
            }
        }
        public Analizador(){
            this.Errores = new List<string>();
            this.Reporte = new List<Bitacora>();
            this.ListaFunciones = new Dictionary<string, List<IC3D>>();
            //Analizar(cadena);
        }
        public void Analizar(string cadena){
            Parser parser = new Parser(new LanguageData(new Gramatica()));
            ParseTree arbol = parser.Parse(cadena);
            if (arbol.Status != ParseTreeStatus.Error)
            {
                this.ListaFunciones = Funciones(arbol.Root.ChildNodes.ElementAt(1));
                this.Cabecera = Encabezados(arbol.Root.ChildNodes.ElementAt(0));
                foreach (var item in this.ListaFunciones)
                {
                    try
                    {
                        var Optimizador = new Optimizar(item.Value);
                        this.Reporte = this.Reporte.Concat(Optimizador.Reporte).ToList();
                        Console.WriteLine($"Se han optimizado {Optimizador.Reporte.Count} lineas de codigo");
                    }
                    catch (Exception ex)
                    {
                        this.Errores.Add($"{ex.Message}, {ex.StackTrace}");
                        continue;
                    }
                }
                this.Success = this.Errores.Count == 0? Status.OPTIMIZADO : Status.PARCIAL;
            } else {
                //HUBO UN ERROR RECONOCIENDO LA CADENA
                this.Success = Status.ERROR;
                foreach (var msg in arbol.ParserMessages)
                    Errores.Add(msg.Message);                
            }
        }
        public string GetErroresDOT(){
            string dot = "";
            int contador = 0;
            foreach (var item in this.Errores)
            {
                string fields = "";
                fields += $"<td BORDER=\"1\">{++contador}</td>\n";
                fields += $"<td BORDER=\"1\">{item}</td>\n";
                dot += $"<tr>\n{fields}</tr>\n";
            }
            
            string cabecera =
            "<tr>" + 
            "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">NO.</td>" +
            "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Mensajes</td>" +
            "</tr>";

            dot = $"<table CELLPADDING=\"5\" CELLSPACING=\"0\" BORDER=\"0\"> {cabecera}{dot}</table>";
            dot = $"node [shape = none];\n a0[label = <{dot}>];\n";
            return "digraph g {\n" +dot + "}";
        }
        public string GetReporteDOT(){
            string dot = "";
            foreach (var item in this.Reporte)
            {
                string fields = "";
                fields += $"<td BORDER=\"1\">{item.Linea}</td>\n";
                fields += $"<td BORDER=\"1\">{item.Regla}</td>\n";
                fields += $"<td BORDER=\"1\">{InlineInstruccion(item.Eliminados)}</td>\n";
                fields += $"<td BORDER=\"1\">{InlineInstruccion(item.Agregados)}</td>\n";
                dot += $"<tr>\n{fields}</tr>\n";
            }
            
            string cabecera =
            "<tr>" + 
            "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Linea</td>" +
            "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Regla</td>" +
            "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Eliminados</td>" +
            "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Agregados</td>" +
            "</tr>";

            dot = $"<table CELLPADDING=\"5\" CELLSPACING=\"0\" BORDER=\"0\"> {cabecera}{dot}</table>";
            dot = $"node [shape = none];\n a0[label = <{dot}>];\n";
            return "digraph g {\n" +dot + "}";
        }

        private string InlineInstruccion(List<IC3D> lista){
            var instrucciones = lista.Select(ins => ins.ParseString().TrimEnd(';', ':')).ToArray();
            return String.Join(", ", instrucciones);
        }

        #region ARBOL ENCABEZADO
        private string Encabezados(ParseTreeNode node){
            string encabezado = "";
            foreach (var child in node.ChildNodes)
                encabezado += $"{Encabezado(child)}\n";
            return encabezado;
        }
        private string Encabezado(ParseTreeNode node){
            if (node.ChildNodes.Count == 3)
                return $"{node.ChildNodes.ElementAt(0).Token.ValueString} {Declaraciones(node.ChildNodes.ElementAt(1))};";
            else
                return $"{node.ChildNodes.ElementAt(0).Token.ValueString} {node.ChildNodes.ElementAt(1).Token.ValueString}";
        }

        private string Declaraciones(ParseTreeNode node){
            if (node.ChildNodes.Count == 1)
            {
                return node.ChildNodes.ElementAt(0).Token.ValueString;
            }else if (node.ChildNodes.Count == 4){
                return $"{node.ChildNodes.ElementAt(0).Token.ValueString}[{node.ChildNodes.ElementAt(2).Token.ValueString}]";
            }else{
                string[] ids = node.ChildNodes.Select(node => node.Token.ValueString).ToArray();
                return string.Join(", ", ids);
            }
        }
        #endregion

        #region ARBOL FUNCIONES
        private Dictionary<string, List<IC3D>> Funciones(ParseTreeNode node){
            Dictionary<string, List<IC3D>> lista = new Dictionary<string, List<IC3D>>();
            foreach (var item in node.ChildNodes)
                lista.Add(item.ChildNodes.ElementAt(1).Token.ValueString, Instrucciones(item.ChildNodes.ElementAt(5)));
            return lista;
        }
        private List<IC3D> Instrucciones(ParseTreeNode node){
            List<IC3D> lista = new List<IC3D>();
            foreach (var item in node.ChildNodes)        
                lista.Add(Intruccion(item));
            return lista;
        }
        private IC3D Intruccion(ParseTreeNode node){
            var value = node.ChildNodes.ElementAt(0).Term.Name.ToLower();
            switch (value)
            {
                case "sentencia_call":
                    return Call(node.ChildNodes.ElementAt(0));
                case "sentencia_goto":
                    return Goto(node.ChildNodes.ElementAt(0));
                case "sentencia_print":
                    return Print(node.ChildNodes.ElementAt(0));
                case "sentencia_if":
                    return If(node.ChildNodes.ElementAt(0));
                case "sentencia_asignacion":
                    return Asignacion(node.ChildNodes.ElementAt(0));
                default:
                    if (node.ChildNodes.ElementAt(1).Token.ValueString == ":")
                        return new Etiqueta(node.ChildNodes.ElementAt(0).Token.ValueString, GetLinea(node.ChildNodes.ElementAt(0)));
                    else
                        return new Return(GetLinea(node.ChildNodes.ElementAt(0))); 
            }
        }
        private Call Call(ParseTreeNode node){
            return new Call(node.ChildNodes.ElementAt(0).Token.ValueString, GetLinea(node.ChildNodes.ElementAt(0)));
        }
        private Goto Goto(ParseTreeNode node){
            return new Goto(node.ChildNodes.ElementAt(1).Token.ValueString, GetLinea(node.ChildNodes.ElementAt(1)));
        }
        private Print Print(ParseTreeNode node){
            return new Print(Expresion(node.ChildNodes.ElementAt(4)), node.ChildNodes.ElementAt(2).Token.ValueString, GetLinea(node.ChildNodes.ElementAt(2)));
        }
        private If If(ParseTreeNode node){
            return new If(Condicion(node.ChildNodes.ElementAt(2)), node.ChildNodes.ElementAt(5).Token.ValueString, GetLinea(node.ChildNodes.ElementAt(5)));
        }
        private Asignacion Asignacion(ParseTreeNode node){
            if (node.ChildNodes.ElementAt(0).Term.Name.ToLower() == "acceso")
            {
                if (node.ChildNodes.ElementAt(2).Term.Name.ToLower() == "operacion")
                    return new Asignacion(Acceso(node.ChildNodes.ElementAt(0)), Operacion(node.ChildNodes.ElementAt(2)), Optimizador.Asignacion.Tipo.ACOP, GetLinea(node.ChildNodes.ElementAt(1)));
                else
                    return new Asignacion(Acceso(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Optimizador.Asignacion.Tipo.ACEX,GetLinea(node.ChildNodes.ElementAt(1)));
            }else{
                if (node.ChildNodes.ElementAt(2).Term.Name.ToLower() == "operacion")
                    return new Asignacion(node.ChildNodes.ElementAt(0).Token.ValueString, Operacion(node.ChildNodes.ElementAt(2)), Optimizador.Asignacion.Tipo.EXOP,GetLinea(node.ChildNodes.ElementAt(1)));
                else if (node.ChildNodes.ElementAt(2).Term.Name.ToLower() == "expresion")
                    return new Asignacion(node.ChildNodes.ElementAt(0).Token.ValueString, Expresion(node.ChildNodes.ElementAt(2)), Optimizador.Asignacion.Tipo.EXEX,GetLinea(node.ChildNodes.ElementAt(1)));
                else
                    return new Asignacion(node.ChildNodes.ElementAt(0).Token.ValueString, Acceso(node.ChildNodes.ElementAt(2)), Optimizador.Asignacion.Tipo.EXAC, GetLinea(node.ChildNodes.ElementAt(1)));
            }
        }
        private Operacion Operacion(ParseTreeNode node){
            var value = node.ChildNodes.ElementAt(1).Token.ValueString;
            switch (value)
            {
                case "+":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Optimizador.Operacion.Tipo.ADICION);
                case "-":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Optimizador.Operacion.Tipo.SUSTRACCION);
                case "*":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Optimizador.Operacion.Tipo.MULTIPLICACION);
                case "/":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Optimizador.Operacion.Tipo.DIVISION);
                default:
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Optimizador.Operacion.Tipo.RESTO);   
            }
        }
        private Condicion Condicion(ParseTreeNode node){
            var value = node.ChildNodes.ElementAt(1).Token.ValueString;
            switch (value)
            {
                case ">":
                    return new Condicion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Optimizador.Condicion.Tipo.MAYOR);
                case "<":
                    return new Condicion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Optimizador.Condicion.Tipo.MENOR);
                case ">=":
                    return new Condicion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Optimizador.Condicion.Tipo.MAYORQUE);
                case "<=":
                    return new Condicion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Optimizador.Condicion.Tipo.MENORQUE);
                case "==":
                    return new Condicion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Optimizador.Condicion.Tipo.IGUAL);
                default:
                    return new Condicion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Optimizador.Condicion.Tipo.DIFERENTE);
            }
        }
        private Acceso Acceso(ParseTreeNode node){
            var value = node.ChildNodes.ElementAt(0).Token.ValueString;
            if (value.ToLower() == "heap")        
                return new Acceso(Expresion(node.ChildNodes.ElementAt(2)), Optimizador.Acceso.Tipo.HEAP);
            else
                return new Acceso(Expresion(node.ChildNodes.ElementAt(2)), Optimizador.Acceso.Tipo.STACK);
            
        }
        private Expresion Expresion(ParseTreeNode node){
            if (node.ChildNodes.Count == 1)
            {
                var value = node.ChildNodes.ElementAt(0).Token.ValueString;
                double numerico = 0;
                if (Double.TryParse(value, out numerico))        
                    return new Expresion(numerico, Optimizador.Expresion.Tipo.NUMERO);
                else    
                    return new Expresion(value, Optimizador.Expresion.Tipo.IDENTIFICADOR);
            } else {
                double numero = Convert.ToDouble(node.ChildNodes.ElementAt(1).Token.ValueString);
                return new Expresion(-1*numero, Optimizador.Expresion.Tipo.NUMERO);
            }
        }
        #endregion

        #region UTILIDADES
        private int GetLinea(ParseTreeNode node){
            return node.Token.Location.Line;
        }
        #endregion
    } 
}
