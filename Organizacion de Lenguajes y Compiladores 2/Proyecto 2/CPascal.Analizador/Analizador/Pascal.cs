using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;

public class Pascal
{
    public enum Status
    {
        PARSED,
        ERROR
    }
    public string Nombre {get; set;}
    public List<Instruccion> Declarativo {get; set;}
    public List<Instruccion> Ejecutivo {get; set;}
    public Tabla TablaSimbolos {get; set;}
    public Status LenguageStatus {get; set;}
    public List<PascalExcepcion> Errores {get; set;}
    public Pascal(){ 
        this.Errores = new List<PascalExcepcion>();
        
        //Analizar(cadena);
    }
    public void Analizar(string cadena){
        Parser parser = new Parser(new LanguageData(new Gramatica()));
        ParseTree arbol = parser.Parse(cadena);
        if (arbol.Status != ParseTreeStatus.Error)
        {
            //Graficador grafo = new Graficador(arbol.Root);
            //grafo.Print(Graficador.Graph.AST);
            Utilities.ClearInstruccion();
            Sintactico(arbol.Root);
            Mapear();
            //Console.WriteLine("Analisis Completado");
            LenguageStatus = Status.PARSED;
        } else {           
            //Console.WriteLine("Ha ocurrido un error analizando la entrada");
            LenguageStatus = Status.ERROR;
            foreach (var pm in arbol.ParserMessages)
            {
                this.Errores.Add(new PascalExcepcion(
                    pm.Message, 
                    pm.Message.ToString().Contains("Syntax")? PascalExcepcion.ParseError.SINTACTIO: PascalExcepcion.ParseError.LEXICO, 
                    pm.Location.Line, 
                    pm.Location.Column));
            }
        }  
    }
    private void Sintactico(ParseTreeNode node){
        if (node.ChildNodes.Count == 6)
        {
            this.Nombre = node.ChildNodes.ElementAt(1).Token.ValueString.ToLower();
            this.Declarativo = Utilities.GetSaveInstruccions().Concat(InstruccionesBloqueDeclaracion(node.ChildNodes.ElementAt(3))).ToList();
            this.Ejecutivo = InstruccionesBloqueEjecucion(node.ChildNodes.ElementAt(4));
        }
    }
    private void Mapear(){
        this.TablaSimbolos = new Tabla();
        Ambito global = new Ambito("Global");
        foreach (var item in this.Declarativo)
        {
            var def = (Definicion)item;
            def.MapearGlobales(TablaSimbolos, global);
        }
        //Graficador tabla = new Graficador(TablaSimbolos);
        //tabla.Print(Graficador.Graph.TS);
    }
    #region REPORTERIA
    public string TablaSimbolosDOT(){
        string dot = "";
        foreach (var item in this.TablaSimbolos)
        {
            string fields = "";
            fields += $"<td BORDER=\"1\">{item.Nombre}</td>\n";
            fields += $"<td BORDER=\"1\">{item.Ambito}</td>\n";
            fields += $"<td BORDER=\"1\">{item.Tipo}</td>\n";
            fields += $"<td BORDER=\"1\">{item.Rol}</td>\n";
            fields += $"<td BORDER=\"1\">{item.Apuntador}</td>\n";
            fields += $"<td BORDER=\"1\">{(item.Posicion == null? 0: item.Posicion.Linea)}</td>\n";
            fields += $"<td BORDER=\"1\">{(item.Posicion == null? 0: item.Posicion.Columna)}</td>\n";
            fields += $"<td BORDER=\"1\">{(item.Dimensiones.Count != 0? string.Join(", ", item.Dimensiones) : "")}</td>";
            fields += $"<td BORDER=\"1\">{item.Minimo}</td>";
            dot += $"<tr>\n{fields}</tr>\n";
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
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Minimo</td>" +
        "</tr>";

        dot = $"<table CELLPADDING=\"5\" CELLSPACING=\"0\" BORDER=\"0\"> {cabecera}{dot}</table>";
        dot = $"node [shape = none];\n a0[label = <{dot}>];\n";
        return "digraph g {\n" +dot + "}";
    }
    public string ErroresSimbolosDOT(){
        string dot = "";
        foreach (var err in this.Errores)
        {
            string fields = "";
            fields += String.Format("<td BORDER=\"1\">{0}</td>\n", err.Tipo == PascalExcepcion.ParseError.LEXICO? "Lexico": err.Tipo == PascalExcepcion.ParseError.SINTACTIO? "Sintactico": "Semantico");
            fields += String.Format("<td BORDER=\"1\">{0}</td>\n", err.Message);
            fields += String.Format("<td BORDER=\"1\">{0}</td>\n", err.Linea);
            fields += String.Format("<td BORDER=\"1\">{0}</td>\n", err.Columna);
            dot += String.Format("<tr>\n{0}</tr>\n", fields);
        }
        string cabecera =
        "<tr>" + 
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Tipo</td>" +
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Mensaje</td>" +
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\">Linea</td>" +
        "<td bgcolor=\"#e1e7c7\" BORDER=\"1\"> Columna</td>" +
        "</tr>";

        dot = String.Format("<table CELLPADDING=\"5\" CELLSPACING=\"0\" BORDER=\"0\"> {0}{1}</table>", cabecera, dot);
        dot = String.Format("node [shape = none];\n a0[label = <{0}>];\n", dot);
        return "digraph g {\n" +dot + "}";
    }
    
    
    #endregion

    #region INSTRUCCION
    private List<Instruccion> InstruccionesBloqueEjecucion(ParseTreeNode node){
        if (node.ChildNodes.Count == 3)
        {
            List<Instruccion> lista = new List<Instruccion>();
            foreach (ParseTreeNode cnode in node.ChildNodes.ElementAt(1).ChildNodes)            
                lista.Add(InstruccionesEjecucion(cnode.ChildNodes.ElementAt(0)));
            return lista;
        } else if (node.ChildNodes.Count == 1) {
            List<Instruccion> lista = new List<Instruccion>();
            lista.Add(InstruccionesEjecucion(node.ChildNodes.ElementAt(0)));
            return lista;
        } else {
            return new List<Instruccion>();
        }
    }
    private List<Instruccion> InstruccionesBloqueDeclaracion(ParseTreeNode node){
        if (node.ChildNodes.Count != 0)
        {
            List<Instruccion> lista = new List<Instruccion>();
            foreach (ParseTreeNode cnode in node.ChildNodes)            
                lista.Add(InstruccionesDeclaracion(cnode));
            return lista;
        } else {
            return new List<Instruccion>();
        } 
    }
    private Instruccion InstruccionesEjecucion(ParseTreeNode node){
        switch (node.ChildNodes.ElementAt(0).Term.ToString())
        {
            case "asignaciones":
                return Asignaciones(node.ChildNodes.ElementAt(0));
            case "sentencia_if":
                return SentenciaIf(node.ChildNodes.ElementAt(0));
            case "sentencia_case":
                return SentenciaCase(node.ChildNodes.ElementAt(0));
            case "sentencia_for":
                return SentenciaFor(node.ChildNodes.ElementAt(0));
            case "sentencia_while":
                return SentenciaWhile(node.ChildNodes.ElementAt(0));
            case "sentencia_repeat":
                return SentenciaRepeat(node.ChildNodes.ElementAt(0));
            case "sentencia_control":
                Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0));
                Control.Tipo tipo;
                if (node.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.ValueString.ToLower().Equals("break"))
                    tipo = Control.Tipo.BREAK;
                else
                    tipo = Control.Tipo.CONTINUE;
                return new Control(tipo, posicion);
            case "sentencia_write":
                return SentenciaWrite(node.ChildNodes.ElementAt(0));
            case "sentencia_exit":
                Posicion posicion1 = Utilities.GetPosicion(node.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0));
                if (node.ChildNodes.ElementAt(0).ChildNodes.Count == 4)
                    return new Exit(Expresion(node.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2)), posicion1);
                else
                    return new Exit(null, posicion1);
            case "sentencia_graficar":
                Posicion posicion2 = Utilities.GetPosicion(node.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0));
                return new Graficar(posicion2);
            default:
                return Llamadas(node.ChildNodes.ElementAt(0), Llamada.Tipo.PROCEDIMIENTO);
        }
    }
    private Instruccion InstruccionesDeclaracion(ParseTreeNode node){
        switch (node.ChildNodes.ElementAt(0).Term.ToString())
        {
            case "declaracion":
                return Declaracion(node.ChildNodes.ElementAt(0));
            case "metodo":
                return Metodos(node.ChildNodes.ElementAt(0));
            default:
                throw new Exception("Error: InstruccionesDeclaracion");
        }
    }
    #endregion 

    #region FUNCIONES Y PROCEDIMIENTOS
    private Metodo Metodos(ParseTreeNode node){
        if (node.ChildNodes.Count == 11)
        {
            Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(0));
            string identificador = node.ChildNodes.ElementAt(1).Token.ValueString;
            List<Variable> parametros = MetodoParametros(node.ChildNodes.ElementAt(3));
            string tipo = Tipo(node.ChildNodes.ElementAt(6));
            List<Instruccion> declaraciones = InstruccionesBloqueDeclaracion(node.ChildNodes.ElementAt(8));
            List<Instruccion> ejecuciones = InstruccionesBloqueEjecucion(node.ChildNodes.ElementAt(9));
            return new Metodo(identificador, Metodo.Tipo.FUNCION ,parametros, tipo, declaraciones, ejecuciones, posicion);
        } else {
            Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(0));
            string identificador = node.ChildNodes.ElementAt(1).Token.ValueString;
            List<Variable> parametros = MetodoParametros(node.ChildNodes.ElementAt(3));
            List<Instruccion> declaraciones = InstruccionesBloqueDeclaracion(node.ChildNodes.ElementAt(6));
            List<Instruccion> ejecuciones = InstruccionesBloqueEjecucion(node.ChildNodes.ElementAt(7));
            return new Metodo(identificador, Metodo.Tipo.PROCEDIMIENTO ,parametros, "VOID", declaraciones, ejecuciones, posicion);
        }
    }
    private List<Variable> MetodoParametros(ParseTreeNode node){
        if (node.ChildNodes.Count == 0)
            return new List<Variable>();
        List<Variable> lista = new List<Variable>();
        foreach (ParseTreeNode cnode in node.ChildNodes)
            lista = lista.Concat(MetodoParametrosUnidad(cnode)).ToList();
        return lista;
    }
    private List<Variable> MetodoParametrosUnidad(ParseTreeNode node){
        if (node.ChildNodes.Count == 3)
        {
            List<Variable> lista = new List<Variable>();
            List<string> variables = new List<string>();
            foreach (ParseTreeNode cnode in node.ChildNodes.ElementAt(0).ChildNodes)
                variables.Add(cnode.Token.ValueString.ToLower());
            string tipo = Tipo(node.ChildNodes.ElementAt(2));
            object value = DefaultValue(tipo);
            foreach (var id in variables)
                lista.Add(new Variable(id, value, tipo));
            return lista;
        }else{
            List<Variable> lista = new List<Variable>();
            List<string> variables = new List<string>();
            foreach (ParseTreeNode cnode in node.ChildNodes.ElementAt(1).ChildNodes)
                variables.Add(cnode.Token.ValueString.ToLower());
            string tipo = Tipo(node.ChildNodes.ElementAt(3));
            object value = DefaultValue(tipo);
            foreach (var id in variables)
                lista.Add(new Variable(id, value, tipo));
            return lista;
        }
    }
    private Llamada Llamadas(ParseTreeNode node, Llamada.Tipo mode){
        Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(0));
        string identificador = node.ChildNodes.ElementAt(0).Token.ValueString.ToLower();
        List<Expresion> expresiones = ExpresionList(node.ChildNodes.ElementAt(2));
        return new Llamada(identificador, expresiones, mode, posicion);
    }

    #endregion

    #region SENTENCIAS DE CONTROL
    private Write SentenciaWrite(ParseTreeNode node){
        Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(0));
        bool writeln = node.ChildNodes.ElementAt(0).Token.ValueString.ToLower().Equals("writeln")? true: false;
        List<Expresion> expresions = ExpresionList(node.ChildNodes.ElementAt(2));
        return new Write(expresions, writeln, posicion);
    }
    private Case SentenciaCase(ParseTreeNode node){
        Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(0));
        Expresion condicion = Expresion(node.ChildNodes.ElementAt(1));
        List<CaseValue> caseValues = CaseValues(node.ChildNodes.ElementAt(3));
        return new Case(condicion, caseValues, posicion);
    }
    private List<CaseValue> CaseValues(ParseTreeNode node){
        if (node.ChildNodes.Count == 4)
        {
            List<CaseValue> list = new List<CaseValue>();
            List<Expresion> expresions = ExpresionList(node.ChildNodes.ElementAt(0));
            List<Instruccion> instruccions = InstruccionesBloqueEjecucion(node.ChildNodes.ElementAt(2));
            list.Add(new CaseValue(expresions, instruccions));
            return list;
        } else if (node.ChildNodes.Count == 3) {
            List<CaseValue> list = new List<CaseValue>();
            List<Instruccion> instruccions = InstruccionesBloqueEjecucion(node.ChildNodes.ElementAt(1));
            list.Add(new CaseValue(new List<Expresion>(), instruccions));
            return list;
        } else {
            List<CaseValue> list1 = CaseValues(node.ChildNodes.ElementAt(0));
            List<CaseValue> list2 = CaseValues(node.ChildNodes.ElementAt(1));
            return list1.Concat(list2).ToList();            
        } 
    }
    private If SentenciaIf(ParseTreeNode node){
        if (node.ChildNodes.Count == 6)
        {
            Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(0));
            Expresion expresion = Expresion(node.ChildNodes.ElementAt(1));
            List<Instruccion> bloqueIf = InstruccionesBloqueEjecucion(node.ChildNodes.ElementAt(3));
            List<Instruccion> bloqueElse = InstruccionesBloqueEjecucion(node.ChildNodes.ElementAt(5));
            return new If(expresion, bloqueIf, bloqueElse, posicion);
        } else {
            Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(0));
            Expresion expresion = Expresion(node.ChildNodes.ElementAt(1));
            List<Instruccion> bloqueIf = InstruccionesBloqueEjecucion(node.ChildNodes.ElementAt(3));
            return new If(expresion, bloqueIf, posicion);
        }
    }
    private For SentenciaFor(ParseTreeNode node){
        Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(0));
        bool downto = node.ChildNodes.ElementAt(2).Token.ValueString.ToLower().Equals("to")? false : true;
        Asignacion asignacion = Asignaciones(node.ChildNodes.ElementAt(1));
        Expresion condicion = Expresion(node.ChildNodes.ElementAt(3));
        List<Instruccion> bloqueFor = InstruccionesBloqueEjecucion(node.ChildNodes.ElementAt(5));
        return new For(asignacion, condicion, bloqueFor, downto, posicion);
    }
    private While SentenciaWhile(ParseTreeNode node){
        Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(0));
        Expresion expresion = Expresion(node.ChildNodes.ElementAt(1));
        List<Instruccion> bloqueWhile = InstruccionesBloqueEjecucion(node.ChildNodes.ElementAt(3));
        return new While(expresion, bloqueWhile, posicion);                                                             
    }
    private Repeat SentenciaRepeat(ParseTreeNode node){
        Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(0));
        List<Instruccion> bloqueRepeat = new List<Instruccion>();
        foreach (ParseTreeNode cnode in node.ChildNodes.ElementAt(1).ChildNodes)            
            bloqueRepeat.Add(InstruccionesEjecucion(cnode.ChildNodes.ElementAt(0)));
        Expresion condicion = Expresion(node.ChildNodes.ElementAt(3));
        return new Repeat(condicion, bloqueRepeat, posicion);
    }
    #endregion
    
    #region ASIGNACIONES
    private Asignacion Asignaciones(ParseTreeNode node){
        Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(1));
        return new Asignacion(Accesos(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), posicion);
    }
    #endregion

    #region VANIDADES

    private object DefaultValue(string tipo){
        switch (tipo)
        {
            case "BOOLEAN":
                return true;
            case "INTEGER":
                return 0.0;
            case "REAL":
                return 0.0;
            case "STRING":
                return "";
            default:
                return null;
        }
    }
    private string Tipo(ParseTreeNode node){
        switch (node.ChildNodes.ElementAt(0).Token.ValueString.ToLower())
        {
            case "boolean":
                return "BOOLEAN";
            case "real":
                return "REAL";
            case "integer":
                return "INTEGER";
            case "string":
                return "STRING";
            default:
                return node.ChildNodes.ElementAt(0).Token.ValueString.ToUpper();
        }
    }
    private RangoArray RangoArrays(ParseTreeNode node){
        return new RangoArray(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(3)));
    }
    private List<Expresion> ExpresionList(ParseTreeNode node){
        if (node.ChildNodes.Count != 0)
        {
            List<Expresion> list = new List<Expresion>();
            foreach (ParseTreeNode cnode in node.ChildNodes)
                list.Add(Expresion(cnode));
            return list;
        }else {
            return new List<Expresion>();
        }
    }
    #endregion

    #region DECLARACIONES
    private Instruccion Declaracion(ParseTreeNode node){
        switch (node.ChildNodes.ElementAt(0).Term.ToString())
        {
            case "declaracion_definida":
                return DeclaracionDefinida(node.ChildNodes.ElementAt(0));
            default:
                return DeclaracionTipica(node.ChildNodes.ElementAt(0));         
        }
    }
    private Definidas DeclaracionDefinida(ParseTreeNode node){
        if (node.ChildNodes.Count == 7)
        {
            Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(1));
            List<Tipicas> lista = new List<Tipicas>();
            foreach (ParseTreeNode cnode in node.ChildNodes.ElementAt(4).ChildNodes)            
                lista.Add(DeclaracionTipica(cnode));
            return new Definidas(node.ChildNodes.ElementAt(1).Token.ValueString.ToLower(), lista, posicion);
        } else {
            Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(1));
            List<RangoArray> indices = new List<RangoArray>();
            string tipo = Tipo(node.ChildNodes.ElementAt(8));
            foreach (ParseTreeNode cnode in node.ChildNodes.ElementAt(5).ChildNodes)
                indices.Add(RangoArrays(cnode));
            return new Definidas(node.ChildNodes.ElementAt(1).Token.ValueString.ToLower(), indices, tipo, posicion);
        }
    }
    private Tipicas DeclaracionTipica(ParseTreeNode node){
        bool mutable = node.ChildNodes.ElementAt(0).Token.ValueString.ToLower().Equals("var") ? true : false;
        Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(0));
        return new Tipicas(DeclaracionVariables(node.ChildNodes.ElementAt(1)), mutable, posicion);
    }

    private List<Variable> DeclaracionVariables(ParseTreeNode node){
        List<Variable> lista = new List<Variable>();
        foreach (ParseTreeNode cnode in node.ChildNodes)        
            lista = lista.Concat(DeclaracionVariablesAsignacion(cnode)).ToList();
        return lista;   
    }
    private List<Variable> DeclaracionVariablesAsignacion(ParseTreeNode node){
        List<string> variables = new List<string>();
        foreach (ParseTreeNode cnode in node.ChildNodes.ElementAt(0).ChildNodes)    
            variables.Add(cnode.Token.ValueString.ToLower());
        
        if (node.ChildNodes.Count == 9)
        {
            Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(2));
            List<RangoArray> indices = new List<RangoArray>();
            String tipo = Tipo(node.ChildNodes.ElementAt(7));
            foreach (ParseTreeNode cnode in node.ChildNodes.ElementAt(4).ChildNodes)
                indices.Add(RangoArrays(cnode));
            int autoArrayDefinition = Utilities.GetArrayDefinicion();
            Utilities.SaveInstruccion(new Definidas($"AD{autoArrayDefinition}", indices, tipo, posicion));
            //this.Declarativo.Add(new Definidas($"AD{autoArrayDefinition}", indices, tipo, posicion));
            
            List<Variable> lista = new List<Variable>();
            string tipo1 = $"AD{autoArrayDefinition}";
            foreach (string var in variables)
                lista.Add(new Variable(var, null, tipo1));
            return lista;
            //return new Definidas(node.ChildNodes.ElementAt(1).Token.ValueString.ToLower(), indices, tipo, posicion);
        } else if (node.ChildNodes.Count == 6){
            List<Variable> lista = new List<Variable>();
            Expresion expresion = Expresion(node.ChildNodes.ElementAt(4));
            string tipo = Tipo(node.ChildNodes.ElementAt(2));
            foreach (string var in variables)
                lista.Add(new Variable(var, expresion, tipo));
            return lista;
        } else {
            if (node.ChildNodes.ElementAt(1).Token.ValueString.Equals(":"))
            {   
                List<Variable> lista = new List<Variable>();
                string tipo = Tipo(node.ChildNodes.ElementAt(2));
                object value = DefaultValue(tipo);            
                foreach (string var in variables)
                    lista.Add(new Variable(var, value, tipo));
                return lista;
            } else {
                List<Variable> lista = new List<Variable>();
                Expresion expresion = Expresion(node.ChildNodes.ElementAt(2));
                foreach (string var in variables)
                    lista.Add(new Variable(var, expresion));
                return lista;
            }
        }
    }
    #endregion
    
    #region EXPRESIONES
    private Expresion Expresion(ParseTreeNode node){
        if (node.ChildNodes.Count == 1)
        {
            switch (node.ChildNodes.ElementAt(0).Term.Name)
            {
                case "expresiones_aritmeticas":
                    return Aritmeticas(node.ChildNodes.ElementAt(0));
                case "expresiones_logicas":
                    return Logicas(node.ChildNodes.ElementAt(0));
                case "expresiones_relacionales":
                    return Relacionales(node.ChildNodes.ElementAt(0));
                case "expresiones_primitivas":
                    return Primitivas(node.ChildNodes.ElementAt(0));
                case "expresiones_accesibles":
                    return Accesos(node.ChildNodes.ElementAt(0));
                default:
                    return Llamadas(node.ChildNodes.ElementAt(0), Llamada.Tipo.FUNCION);
            }
        } else {
            return Expresion(node.ChildNodes.ElementAt(1));
        }
    }
    private Aritmetica Aritmeticas(ParseTreeNode node){
        if (node.ChildNodes.Count == 3)
        {
            Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(1));
            switch (node.ChildNodes.ElementAt(1).Token.ValueString)
            {
                case "+":
                    return new Aritmetica(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Aritmetica.Tipo.ADICION, posicion);
                case "-":
                    return new Aritmetica(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Aritmetica.Tipo.SUSTRACCION, posicion);
                case "*":
                    return new Aritmetica(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Aritmetica.Tipo.MULTIPLICACION, posicion);
                case "/":
                    return new Aritmetica(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Aritmetica.Tipo.DIVISION, posicion);
                default:
                    return new Aritmetica(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Aritmetica.Tipo.MODULO, posicion); 
            }
        } else {
            Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(0));
            return new Aritmetica(Expresion(node.ChildNodes.ElementAt(1)), Aritmetica.Tipo.NEGACION, posicion);
        }
    }
    private Logica Logicas(ParseTreeNode node){
        if (node.ChildNodes.Count == 3)
        {
            Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(1));
            switch (node.ChildNodes.ElementAt(1).Token.ValueString.ToLower())
            {
                case "and":
                    return new Logica(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Logica.Tipo.AND, posicion);
                default:
                    return new Logica(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Logica.Tipo.OR, posicion);

            }
        } else {
            Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(0));
            return new Logica(Expresion(node.ChildNodes.ElementAt(1)), Logica.Tipo.NOT, posicion);
        }
    }
    private Relacional Relacionales(ParseTreeNode node){
        Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(1));
        switch (node.ChildNodes.ElementAt(1).Token.ValueString)
        {
            case "<":
                return new Relacional(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Relacional.Tipo.MENOR, posicion);
            case ">":
                return new Relacional(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Relacional.Tipo.MAYOR, posicion);
            case "<=":
                return new Relacional(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Relacional.Tipo.MENORIGUAL, posicion);
            case ">=":
                return new Relacional(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Relacional.Tipo.MAYORIGUAL, posicion);
            case "=":
                return new Relacional(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Relacional.Tipo.IGUAL, posicion);
            default:
                return new Relacional(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Relacional.Tipo.DIFERENTE, posicion);
        }
    }
    private Primitiva Primitivas(ParseTreeNode node){
        Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(0));
        return new Primitiva(node.ChildNodes.ElementAt(0).Token.ValueString, posicion);
    }
    private Acceso Accesos(ParseTreeNode node){
        switch (node.ChildNodes.ElementAt(0).Term.ToString().ToLower())
        {
            case "acceso_struct":
                return AccesoStruct(node.ChildNodes.ElementAt(0));
            case "acceso_array":
                return AccesosArray(node.ChildNodes.ElementAt(0));
            default:
                Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(0));
                return new Acceso(node.ChildNodes.ElementAt(0).Token.ValueString.ToLower(), Acceso.Tipo.IDENTIFICADOR, posicion);
        }
    }
    private Acceso AccesoStruct(ParseTreeNode node){
        Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(2));
        return new Acceso(Accesos(node.ChildNodes.ElementAt(0)), node.ChildNodes.ElementAt(2).Token.ValueString.ToLower(), Acceso.Tipo.STRUCT, posicion);
    }
    private Acceso AccesosArray(ParseTreeNode node){
        Posicion posicion = Utilities.GetPosicion(node.ChildNodes.ElementAt(1));
        List<Expresion> indice = new List<Expresion>();
        foreach (ParseTreeNode cnode in node.ChildNodes.ElementAt(2).ChildNodes)   
            indice.Add(Expresion(cnode));
        return new Acceso(Accesos(node.ChildNodes.ElementAt(0)), indice, Acceso.Tipo.ARRAY, posicion);

    }
    #endregion
}
