using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Diagnostics;

class Sintactico
{

    public Graficador Errores {get; set;}
    public void Analizar(string cadena){
        //Generamos la gramatica
        //Gramatica gramatica = ;
        //LanguageData lenguaje = ;
        //ParseTreeNode raiz = arbol.Root;
        Parser parser = new Parser(new LanguageData(new Gramatica()));
        ParseTree arbol = parser.Parse(cadena);
        Errores = new Graficador(); 

        if (arbol.Status == ParseTreeStatus.Error)
        {
            foreach (var item in arbol.ParserMessages)
            {
                Errores.AgregarError(new Error(item.Location.Line, item.Location.Column, item.Message, item.Message.ToString().Contains("Syntax")? Error.Tipo.SINTACTICO: Error.Tipo.LEXICO));
            }
            Debug.WriteLine($"Se han encontrado errores lexico/sintacticos durante la construccion de AST por favor revisar el log de errores");
            Log.AddLog($"Se han encontrado errores lexico/sintacticos durante la construccion de AST por favor revisar el log de errores\r\n");
            Errores.Print(Graficador.Graph.ERROR);
            return;
        } else {           
            GraficarAST(arbol.Root);
            EjectuarAST(arbol.Root);
        }      
    }    

    private void EjectuarAST(ParseTreeNode raiz){
        try
        {
            LinkedList<Instruccion> Declarativos = Instrucciones(raiz.ChildNodes.ElementAt(3));
            LinkedList<Instruccion> Ejecutivo = Instrucciones(raiz.ChildNodes.ElementAt(5));
            LinkedList<Instruccion> AST = new LinkedList<Instruccion>(Declarativos.Concat(Ejecutivo));

            Entorno global = new Entorno();
            //Declaracion
            foreach (var ins in AST)
            {
                try
                {
                    //ins.ejecutar(global);
                    var res = ins.ejecutar(global);
                    if (res is Control.ControlSet)
                    {
                        switch ((Control.ControlSet)res)
                        {
                            case Control.ControlSet.BREAK:
                            case Control.ControlSet.CONTINUE:
                            case Control.ControlSet.EXIT:
                                throw new SemanticException("Sentencia break o continue debe estar en una sentencia de repeticion");                             
                        }
                    }
                }
                catch (SemanticException ex)
                {
                    this.Errores.AgregarError(new Error(ex.Linea, ex.Columna, ex.Message, Error.Tipo.SEMANTICO));
                    Debug.WriteLine($"Ha ocurrido un error durante la ejecucion de una instruccion {ex.Linea}, {ex.Columna}, {ex.Message}");
                    Log.AddLog($"Ha ocurrido un error durante la ejecucion de una instruccion {ex.Linea}, {ex.Columna}, {ex.Message}\r\n");
                    continue;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Se han encontrado errores tecnicos durante la construccion de AST por favor revisar el codigo: {ex.Message} {ex.StackTrace}");
            Log.AddLog($"Se han encontrado errores tecnicos durante la construccion de AST por favor revisar el codigo: {ex.Message} {ex.StackTrace}\r\n");
            Errores.Print(Graficador.Graph.ERROR);
            return;
        }
        Errores.Print(Graficador.Graph.ERROR);
    }

    private void GraficarAST(ParseTreeNode raiz){
        Graficador GAST = new Graficador(raiz);
        GAST.Print(Graficador.Graph.AST);
    }

    private LinkedList<Instruccion> Instrucciones(ParseTreeNode node){
        if (node.ChildNodes.Count == 2)
        {
            LinkedList<Instruccion> lista = Instrucciones(node.ChildNodes.ElementAt(0));
            lista.AddLast(Instruccion(node.ChildNodes.ElementAt(1)));
            return lista;
        }else if (node.ChildNodes.Count == 1){
            LinkedList<Instruccion> lista = new LinkedList<Instruccion>();
            lista.AddLast(Instruccion(node.ChildNodes.ElementAt(0)));
            return lista;
        }else{
            return new LinkedList<Instruccion>();
        }
    }

    private Instruccion Instruccion(ParseTreeNode node){
        if (node.ChildNodes.Count == 5){
            if (node.ChildNodes.ElementAt(0).Term.Name.Equals("WRITE"))
            {
                //=============FUNCION WRITE================
                return new Write(WriteList(node.ChildNodes.ElementAt(2)), node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
            }else if (node.ChildNodes.ElementAt(0).Term.Name.Equals("WRITELN")){
                //=============FUNCION WRITELN==============
                return new Write(WriteList(node.ChildNodes.ElementAt(2)), node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column, true);
            }else if (node.ChildNodes.ElementAt(0).Term.Name.Equals("TYPE")){
                //=============FUNCION TYPE=================
                LinkedList<string> lista = new LinkedList<string>();
                lista.AddLast(node.ChildNodes.ElementAt(1).Token.ValueString);
                return new Declaracion(lista, Simbolo.Tipo.TYPE, Type(node.ChildNodes.ElementAt(3)), node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
            }else if (node.ChildNodes.ElementAt(0).Term.Name.Equals("EXIT")){
                //=============FUNCION EXIT================
                return new Exit(Expresion(node.ChildNodes.ElementAt(2)), node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
            }else{
                //==========LLAMADA A PROCEDIMIENTO=========
                return new Operacion("p"+node.ChildNodes.ElementAt(0).Token.ValueString, Parametros(node.ChildNodes.ElementAt(2)), node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
            }         
        } else if (node.ChildNodes.Count == 4){
            if (node.ChildNodes.ElementAt(0).Term.Name.Equals("OBJETO"))
            {
                //=============ASIGNACION DE OBJETO============
                return new Asignacion(Objeto(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
            } else if (node.ChildNodes.ElementAt(0).Term.Name.Equals("GRAFICAR_TS")) {
                return new Graficar();  
            } else if (node.ChildNodes.ElementAt(0).Term.Name.Equals("ARRAY")) {
                return new Asignacion(Array(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
            } else {
                //===============ASIGNACION TIPICA=============
                return new Asignacion(node.ChildNodes.ElementAt(0).Token.ValueString, Expresion(node.ChildNodes.ElementAt(2)), node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
            }
        } else if (node.ChildNodes.Count == 2){
            switch (node.ChildNodes.ElementAt(0).Term.Name)
            {
                case "CONST":
                    //=============CONST========================
                    return new Variables(ListaConstantes(node.ChildNodes.ElementAt(1)));
                case "VAR":
                    //=============VAR==========================
                    return new Variables(ListaDeclaracion(node.ChildNodes.ElementAt(1)));
                case "SENTENCIACASE":
                    //=============CASE=========================
                    return CaseSentencia(node.ChildNodes.ElementAt(0));
                case "SENTENCIAWHILE":
                    //=============WHILE========================
                    return WhileSentencia(node.ChildNodes.ElementAt(0));
                case "SENTENCIAFOR":
                    //==============FOR=========================
                    return ForSentencia(node.ChildNodes.ElementAt(0));
                case "SENTENCIAREPEAT":
                    //=============REPEAT=======================
                    return RepeatSentencia(node.ChildNodes.ElementAt(0));
                case "SENTENCIAIF":
                    //==============IF==========================
                    return IfSentencia(node.ChildNodes.ElementAt(0));
                case "CALL":
                    //==============CALL========================
                    return Call(node.ChildNodes.ElementAt(0));
                case "BREAK":
                    //=============BREAK========================
                    return new Control(Control.ControlSet.BREAK, node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
                case "CONTINUE":
                    //=============CONTINUE=====================
                    return new Control(Control.ControlSet.CONTINUE, node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
                default:
                    return null;
            }  
        } else {
            return FuncProc(node.ChildNodes.ElementAt(0));
        }
    }

    private OpArray Array(ParseTreeNode node){
        return new OpArray(node.ChildNodes.ElementAt(0).Token.ValueString, Expresion(node.ChildNodes.ElementAt(2)), node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
    }
    private Operacion Call(ParseTreeNode node){
        //==========LLAMADA A PROCEDIMIENTO=========
        if (node.ChildNodes.Count == 4)
        {
            return new Operacion("f"+node.ChildNodes.ElementAt(0).Token.ValueString, Parametros(node.ChildNodes.ElementAt(2)), node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
        }else if (node.ChildNodes.Count == 3){
            return new Operacion("f"+node.ChildNodes.ElementAt(0).Token.ValueString, new LinkedList<Operacion>(), node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
        }else{
            return new Operacion("f"+node.ChildNodes.ElementAt(0).Token.ValueString, new LinkedList<Operacion>(), node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
        }
        
    }

    private Declaracion FuncProc(ParseTreeNode node){
        if (node.ChildNodes.Count == 13)
        {
            var funcion = new Funcion(node.ChildNodes.ElementAt(1).Token.ValueString, TipoSimbolo(node.ChildNodes.ElementAt(6)), ListaDeclaracion(node.ChildNodes.ElementAt(3)), Instrucciones(node.ChildNodes.ElementAt(8)), Instrucciones(node.ChildNodes.ElementAt(10)));
            LinkedList<string> lista = new LinkedList<string>();
            lista.AddLast("f"+node.ChildNodes.ElementAt(1).Token.ValueString);
            return new Declaracion(lista, TipoSimbolo(node.ChildNodes.ElementAt(6)), funcion, node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
        }else if (node.ChildNodes.Count == 12){
            var funcion = new Funcion(node.ChildNodes.ElementAt(1).Token.ValueString, TipoSimbolo(node.ChildNodes.ElementAt(5)), new LinkedList<Declaracion>(), Instrucciones(node.ChildNodes.ElementAt(7)), Instrucciones(node.ChildNodes.ElementAt(9)));
            LinkedList<string> lista = new LinkedList<string>();
            lista.AddLast("f"+node.ChildNodes.ElementAt(1).Token.ValueString);
            return new Declaracion(lista, TipoSimbolo(node.ChildNodes.ElementAt(5)), funcion, node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
        }else if (node.ChildNodes.Count == 11){
            var proc = new Funcion(node.ChildNodes.ElementAt(1).Token.ValueString, Simbolo.Tipo.VOID, ListaDeclaracion(node.ChildNodes.ElementAt(3)), Instrucciones(node.ChildNodes.ElementAt(6)), Instrucciones(node.ChildNodes.ElementAt(8)));
            LinkedList<string> lista = new LinkedList<string>();
            lista.AddLast("f"+node.ChildNodes.ElementAt(1).Token.ValueString);
            return new Declaracion(lista, Simbolo.Tipo.VOID, proc, node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
        }else if (node.ChildNodes.Count == 10){
            if (node.ChildNodes.ElementAt(0).Term.Name.Equals("FUNCTION"))
            {
                var funcion = new Funcion(node.ChildNodes.ElementAt(1).Token.ValueString, TipoSimbolo(node.ChildNodes.ElementAt(3)), new LinkedList<Declaracion>(), Instrucciones(node.ChildNodes.ElementAt(5)), Instrucciones(node.ChildNodes.ElementAt(7)));
                LinkedList<string> lista = new LinkedList<string>();
                lista.AddLast("f"+node.ChildNodes.ElementAt(1).Token.ValueString);
                return new Declaracion(lista, TipoSimbolo(node.ChildNodes.ElementAt(3)), funcion, node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
            }else{
                var proc = new Funcion(node.ChildNodes.ElementAt(1).Token.ValueString, Simbolo.Tipo.VOID, new LinkedList<Declaracion>(), Instrucciones(node.ChildNodes.ElementAt(5)), Instrucciones(node.ChildNodes.ElementAt(7)));
                LinkedList<string> lista = new LinkedList<string>();
                lista.AddLast("f"+node.ChildNodes.ElementAt(1).Token.ValueString);
                return new Declaracion(lista, Simbolo.Tipo.VOID, proc, node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
            }
        }else{
            var proc = new Funcion(node.ChildNodes.ElementAt(1).Token.ValueString, Simbolo.Tipo.VOID, new LinkedList<Declaracion>(), Instrucciones(node.ChildNodes.ElementAt(3)), Instrucciones(node.ChildNodes.ElementAt(5)));
            LinkedList<string> lista = new LinkedList<string>();
            lista.AddLast("f"+node.ChildNodes.ElementAt(1).Token.ValueString);
            return new Declaracion(lista, Simbolo.Tipo.VOID, proc, node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
        }
    }

    private Instruccion Ternaria(ParseTreeNode node){
        if (node.ChildNodes.Count == 4){
            if (node.ChildNodes.ElementAt(0).Term.Name.Equals("WRITE"))
            {
                //=============FUNCION WRITE================
                return new Write(WriteList(node.ChildNodes.ElementAt(2)), node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
            }else if (node.ChildNodes.ElementAt(0).Term.Name.Equals("WRITELN")){
                //=============FUNCION WRITELN==============
                return new Write(WriteList(node.ChildNodes.ElementAt(2)), node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column, true);
            }else{
                return new Exit(Expresion(node.ChildNodes.ElementAt(2)), node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
            }          
        } else if (node.ChildNodes.Count == 3){
            if (node.ChildNodes.ElementAt(0).Term.Name.Equals("OBJETO"))
            {
                //=============ASIGNACION DE OBJETO============
                return new Asignacion(Objeto(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
            } else if (node.ChildNodes.ElementAt(0).Term.Name.Equals("GRAFICAR_TS")) {
                return new Graficar();
            } else if (node.ChildNodes.ElementAt(0).Term.Name.Equals("ARRAY")) {
                return new Asignacion(Array(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
            } else {    
                //===============ASIGNACION TIPICA=============
                return new Asignacion(node.ChildNodes.ElementAt(0).Token.ValueString, Expresion(node.ChildNodes.ElementAt(2)), node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
            }
        } else if (node.ChildNodes.Count == 1){
            switch (node.ChildNodes.ElementAt(0).Term.Name)
            {
                case "SENTENCIACASE":
                    //=============CASE=========================
                    return CaseSentencia(node.ChildNodes.ElementAt(0));
                case "SENTENCIAWHILE":
                    //=============WHILE========================
                    return WhileSentencia(node.ChildNodes.ElementAt(0));
                case "SENTENCIAFOR":
                    //==============FOR=========================
                    return ForSentencia(node.ChildNodes.ElementAt(0));
                case "SENTENCIAREPEAT":
                    //=============REPEAT=======================
                    return RepeatSentencia(node.ChildNodes.ElementAt(0));
                case "SENTENCIAIF":
                    //==============IF==========================
                    return IfSentencia(node.ChildNodes.ElementAt(0));
                case "CALL":
                    return Call(node.ChildNodes.ElementAt(0));
                case "BREAK":
                    //=============BREAK========================
                    return new Control(Control.ControlSet.BREAK, node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
                case "CONTINUE":
                    //=============CONTINUE=====================
                    return new Control(Control.ControlSet.CONTINUE, node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
                default:
                    return null;
            }    
        } else {
            return null;
        }
    }
    private Repeat RepeatSentencia(ParseTreeNode node){
        return new Repeat(Condicion(node.ChildNodes.ElementAt(3)), Instrucciones(node.ChildNodes.ElementAt(1)), node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
    }
    private For ForSentencia(ParseTreeNode node){
        return new For(node.ChildNodes.ElementAt(1).Token.ValueString, Expresion(node.ChildNodes.ElementAt(3)), Expresion(node.ChildNodes.ElementAt(5)), InstruccionesBloque(node.ChildNodes.ElementAt(7)), node.ChildNodes.ElementAt(4).Token.ValueString.Equals("to")? false : true, node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
    }
    private While WhileSentencia(ParseTreeNode node){
        return new While(Condicion(node.ChildNodes.ElementAt(1)), InstruccionesBloque(node.ChildNodes.ElementAt(3)), node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
    }
    private Case CaseSentencia(ParseTreeNode node){
        return new Case(Condicion(node.ChildNodes.ElementAt(1)), caseValue(node.ChildNodes.ElementAt(3)));
        
    }
    private LinkedList<CaseValue> caseValue(ParseTreeNode node){
        if (node.ChildNodes.Count == 5)
        {
            LinkedList<CaseValue> lista = caseValue(node.ChildNodes.ElementAt(0));
            lista.AddLast(new CaseValue(caseList(node.ChildNodes.ElementAt(1)), InstruccionesBloque(node.ChildNodes.ElementAt(3))));
            return lista;
        }else{
            if (node.ChildNodes.ElementAt(0).Term.Name.Equals("CASEVALUE"))
            {
                LinkedList<CaseValue> lista = caseValue(node.ChildNodes.ElementAt(0));
                lista.AddLast(new CaseValue(InstruccionesBloque(node.ChildNodes.ElementAt(2))));
                return lista;
            }else{
                LinkedList<CaseValue> lista = new LinkedList<CaseValue>();
                lista.AddLast(new CaseValue(caseList(node.ChildNodes.ElementAt(0)), InstruccionesBloque(node.ChildNodes.ElementAt(2))));
                return lista;
            }
        }
    }
    private LinkedList<Operacion> caseList(ParseTreeNode node){
        if (node.ChildNodes.Count == 3)
        {
            LinkedList<Operacion> lista = caseList(node.ChildNodes.ElementAt(0));
            lista.AddLast(Expresion(node.ChildNodes.ElementAt(2)));
            return lista;
        }else{
            LinkedList<Operacion> lista = new LinkedList<Operacion>();
            lista.AddLast(Expresion(node.ChildNodes.ElementAt(0)));
            return lista;
        }
    }
    private If IfSentencia(ParseTreeNode node){
        if (node.ChildNodes.Count == 4)
        {
            return new If(Condicion(node.ChildNodes.ElementAt(1)), InstruccionesBloque(node.ChildNodes.ElementAt(3)), node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
        }else{
            return new If(Condicion(node.ChildNodes.ElementAt(1)), InstruccionesBloque(node.ChildNodes.ElementAt(3)), InstruccionesBloque(node.ChildNodes.ElementAt(5)), node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
        }
    }
    private Operacion Condicion(ParseTreeNode node){
        if (node.ChildNodes.Count == 3)
        {
            return Expresion(node.ChildNodes.ElementAt(1));
        }else{
            return Expresion(node.ChildNodes.ElementAt(0));
        }
    }
    private LinkedList<Instruccion> InstruccionesBloque(ParseTreeNode node){
        if (node.ChildNodes.Count == 3)
        {
            return Instrucciones(node.ChildNodes.ElementAt(1));
        }else{
            LinkedList<Instruccion> lista = new LinkedList<Instruccion>();
            lista.AddLast(Ternaria(node.ChildNodes.ElementAt(0)));
            return lista;
        }
    }
    private Objeto Objeto(ParseTreeNode node, bool child = false, Objeto propiedad = null){
        if (node.ChildNodes.Count == 3)
        {
            if (child)
            {
                return Objeto(node.ChildNodes.ElementAt(0), true, new Objeto(node.ChildNodes.ElementAt(2).Token.ValueString, propiedad, node.ChildNodes.ElementAt(2).Token.Location.Line, node.ChildNodes.ElementAt(2).Token.Location.Column));
            }else{
                return Objeto(node.ChildNodes.ElementAt(0), true, new Objeto(node.ChildNodes.ElementAt(2).Token.ValueString, node.ChildNodes.ElementAt(2).Token.Location.Line, node.ChildNodes.ElementAt(2).Token.Location.Column));
            }
        } else {
            return new Objeto(node.ChildNodes.ElementAt(0).Token.ValueString, propiedad, node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
        }
    }
    private LinkedList<Operacion> WriteList(ParseTreeNode node){
        if (node.ChildNodes.Count == 3){
            LinkedList<Operacion> lista = WriteList(node.ChildNodes.ElementAt(0));
            lista.AddLast(Expresion(node.ChildNodes.ElementAt(2)));
            return lista;
        } else {
            LinkedList<Operacion> lista = new LinkedList<Operacion>();
            lista.AddLast(Expresion(node.ChildNodes.ElementAt(0)));
            return lista;
        }
    }

    private LinkedList<Declaracion> ListaConstantes(ParseTreeNode node){
        if (node.ChildNodes.Count == 5) {
            LinkedList<Declaracion> lista = ListaConstantes(node.ChildNodes.ElementAt(0));
            LinkedList<string> constante = new LinkedList<string>();
            constante.AddLast(node.ChildNodes.ElementAt(1).Token.ValueString);
            lista.AddLast(new Declaracion(constante, Expresion(node.ChildNodes.ElementAt(3)), node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column));
            return lista;
        } else if (node.ChildNodes.Count == 7) {
            LinkedList<Declaracion> lista = ListaConstantes(node.ChildNodes.ElementAt(0));
            LinkedList<string> constante = new LinkedList<string>();
            constante.AddLast(node.ChildNodes.ElementAt(1).Token.ValueString);
            lista.AddLast(new Declaracion(constante, Expresion(node.ChildNodes.ElementAt(5)), node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column));
            return lista;
        } else if (node.ChildNodes.Count == 6) {
            LinkedList<Declaracion> lista = new LinkedList<Declaracion>();
            LinkedList<string> constante = new LinkedList<string>();
            constante.AddLast(node.ChildNodes.ElementAt(0).Token.ValueString);
            lista.AddLast(new Declaracion(constante, Expresion(node.ChildNodes.ElementAt(4)), node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column));
            return lista;
        } else {
            LinkedList<Declaracion> lista = new LinkedList<Declaracion>();
            LinkedList<string> constante = new LinkedList<string>();
            constante.AddLast(node.ChildNodes.ElementAt(0).Token.ValueString);
            lista.AddLast(new Declaracion(constante, Expresion(node.ChildNodes.ElementAt(2)), node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column));
            return lista;
        }
    }

    private object Type(ParseTreeNode node){
        if (node.ChildNodes.Count == 3)
        {
            return new Variables(ListaObject(node.ChildNodes.ElementAt(1)));
        } else {
            return new Array(Expresion(node.ChildNodes.ElementAt(2)), Expresion(node.ChildNodes.ElementAt(5)), TipoSimbolo(node.ChildNodes.ElementAt(8)));
        }
    }
    
    private LinkedList<Declaracion> ListaObject(ParseTreeNode node){
        if (node.ChildNodes.Count == 3){
            LinkedList<Declaracion> lista = ListaObject(node.ChildNodes.ElementAt(0));
            return new LinkedList<Declaracion>(lista.Concat(ListaDeclaracion(node.ChildNodes.ElementAt(2))));
        }else{
            return ListaDeclaracion(node.ChildNodes.ElementAt(1));
        }
    }

    private LinkedList<Declaracion> ListaDeclaracion(ParseTreeNode node){
        if (node.ChildNodes.Count == 6)
        {
            LinkedList<Declaracion> lista = ListaDeclaracion(node.ChildNodes.ElementAt(0));
            Declaracion vartemp = ListaDeclaracionValue(node.ChildNodes.ElementAt(5), TipoSimbolo(node.ChildNodes.ElementAt(4)), Declaracion(node.ChildNodes.ElementAt(2)), node.ChildNodes.ElementAt(3).Token.Location.Line, node.ChildNodes.ElementAt(3).Token.Location.Column, Tipo(node.ChildNodes.ElementAt(4)));
            vartemp.referencia = true;
            lista.AddLast(vartemp);
            return lista;
        } else if (node.ChildNodes.Count == 5){
            if (node.ChildNodes.ElementAt(0).Token != null)
            {
                LinkedList<Declaracion> lista = new LinkedList<Declaracion>();
                Declaracion vartemp = ListaDeclaracionValue(node.ChildNodes.ElementAt(4), TipoSimbolo(node.ChildNodes.ElementAt(3)), Declaracion(node.ChildNodes.ElementAt(1)), node.ChildNodes.ElementAt(2).Token.Location.Line, node.ChildNodes.ElementAt(2).Token.Location.Column, Tipo(node.ChildNodes.ElementAt(3)));
                vartemp.referencia = true;
                lista.AddLast(vartemp);
                return lista;
            }else{
                LinkedList<Declaracion> lista = ListaDeclaracion(node.ChildNodes.ElementAt(0));
                lista.AddLast(ListaDeclaracionValue(node.ChildNodes.ElementAt(4), TipoSimbolo(node.ChildNodes.ElementAt(3)), Declaracion(node.ChildNodes.ElementAt(1)), node.ChildNodes.ElementAt(2).Token.Location.Line, node.ChildNodes.ElementAt(2).Token.Location.Column, Tipo(node.ChildNodes.ElementAt(3))));
                return lista;
            }                    
        } else {
            LinkedList<Declaracion> lista = new LinkedList<Declaracion>();
            lista.AddLast(ListaDeclaracionValue(node.ChildNodes.ElementAt(3), TipoSimbolo(node.ChildNodes.ElementAt(2)), Declaracion(node.ChildNodes.ElementAt(0)), node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column, Tipo(node.ChildNodes.ElementAt(2))));
            return lista;
        }
    }

    private Declaracion ListaDeclaracionValue(ParseTreeNode node, Simbolo.Tipo tipo, LinkedList<string> list, int x, int y, string s_tipo = ""){
        if (node.ChildNodes.Count == 3)
        {
            return new Declaracion(list, tipo, Expresion(node.ChildNodes.ElementAt(1)), x, y);
            //return Expresion(node.ChildNodes.ElementAt(1));
        }else{
            switch (tipo)
            {
                case Simbolo.Tipo.INTEGER:
                    return new Declaracion(list, tipo, 0, x, y);
                case Simbolo.Tipo.STRING:
                    return new Declaracion(list, tipo, "", x, y);
                case Simbolo.Tipo.BOOLEAN:
                    return new Declaracion(list, tipo, true, x, y);
                case Simbolo.Tipo.REAL:
                    return new Declaracion(list, tipo, 0.0, x, y);
                default:
                    return new Declaracion(list, tipo, s_tipo, x, y);
            }
        }
    }

    private LinkedList<string> Declaracion(ParseTreeNode node){
        if (node.ChildNodes.Count == 3){
            LinkedList<string> lista = Declaracion(node.ChildNodes.ElementAt(0));
            lista.AddLast(node.ChildNodes.ElementAt(2).Token.ValueString);
            return lista;
        } else {
            LinkedList<string> lista = new LinkedList<string>();
            lista.AddLast(node.ChildNodes.ElementAt(0).Token.ValueString);
            return lista;
        }
    }

    private string Tipo(ParseTreeNode node){
        return node.ChildNodes.ElementAt(0).Token.ValueString;
    }

    public Simbolo.Tipo TipoSimbolo(ParseTreeNode node){
        switch (node.ChildNodes.ElementAt(0).Term.Name)
        {
            case "BOOLEAN":
                return Simbolo.Tipo.BOOLEAN;
            case "INTEGER":
                return Simbolo.Tipo.INTEGER;
            case "STRING":
                return Simbolo.Tipo.STRING;
            case "REAL":
                return Simbolo.Tipo.REAL;
            default:
                return Simbolo.Tipo.TYPE;            
        }
    }

    private LinkedList<Operacion> Parametros(ParseTreeNode node){
        if (node.ChildNodes.Count == 3)
        {
            LinkedList<Operacion> lista = Parametros(node.ChildNodes.ElementAt(0));
            lista.AddLast(Expresion(node.ChildNodes.ElementAt(2)));
            return lista;
        }else{
            LinkedList<Operacion> lista = new LinkedList<Operacion>();
            lista.AddLast(Expresion(node.ChildNodes.ElementAt(0)));
            return lista;
        }
    }

    private Operacion Expresion(ParseTreeNode node){
        if (node.ChildNodes.Count == 3){
            //node.ChildNodes.ElementAt(1).Token != null? node.ChildNodes.ElementAt(1).Token.ValueString : node.ChildNodes.ElementAt(1).Term.Name
            //node.ChildNodes.ElementAt(1).Token.ValueString
            switch (node.ChildNodes.ElementAt(1).Term.Name)
            {
                case "+":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Operacion.TipoOperacion.SUMA, node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
                case "-":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Operacion.TipoOperacion.RESTA, node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
                case "*":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Operacion.TipoOperacion.MULTIPLICACION, node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
                case "/":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Operacion.TipoOperacion.DIVISION, node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
                case "%":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Operacion.TipoOperacion.MODULO, node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
                case ">":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Operacion.TipoOperacion.MAYOR, node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
                case "<":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Operacion.TipoOperacion.MENOR, node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
                case ">=":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Operacion.TipoOperacion.MAYORIGUAL, node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
                case "<=":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Operacion.TipoOperacion.MENORIGUAL, node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
                case "<>":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Operacion.TipoOperacion.DIFERENTE, node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
                case "=":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Operacion.TipoOperacion.IGUAL, node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
                case "AND":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Operacion.TipoOperacion.AND, node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
                case "OR":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(0)), Expresion(node.ChildNodes.ElementAt(2)), Operacion.TipoOperacion.OR, node.ChildNodes.ElementAt(1).Token.Location.Line, node.ChildNodes.ElementAt(1).Token.Location.Column);
                default:
                    return Expresion(node.ChildNodes.ElementAt(1));
            }            
        } else if (node.ChildNodes.Count == 2){
            switch (node.ChildNodes.ElementAt(0).Term.Name){
                case "NOT":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(1)), Operacion.TipoOperacion.NOT, node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
                case "-":
                    return new Operacion(Expresion(node.ChildNodes.ElementAt(1)), Operacion.TipoOperacion.NEGACION, node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
                default:
                    return null;
            }
        } else {
            switch (node.ChildNodes.ElementAt(0).Term.Name)
            {
                case "NUMBER":
                    return new Operacion(node.ChildNodes.ElementAt(0).Token.Value, Operacion.TipoOperacion.NUMERO, node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
                case "IDENTIFICADOR":
                    return new Operacion(node.ChildNodes.ElementAt(0).Token.Value, Operacion.TipoOperacion.IDENTIFICADOR, node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
                case "STRING":
                    return new Operacion(node.ChildNodes.ElementAt(0).Token.Value, Operacion.TipoOperacion.CADENA, node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
                case "BOOLEAN":
                    return new Operacion(node.ChildNodes.ElementAt(0).Token.Value, Operacion.TipoOperacion.BOOLEANO, node.ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).Token.Location.Column);
                case "CALL":
                    return Call(node.ChildNodes.ElementAt(0));
                case "ARRAY":
                    return new Operacion(Array(node.ChildNodes.ElementAt(0)), Operacion.TipoOperacion.ARRAY, 0, 0);
                default:
                    if (node.ChildNodes.ElementAt(0).ChildNodes.Count == 1)
                        return new Operacion(node.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Value, Operacion.TipoOperacion.IDENTIFICADOR, node.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Line, node.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Column);
                    else
                        return new Operacion(Objeto(node.ChildNodes.ElementAt(0)), Operacion.TipoOperacion.OBJETO, node.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).Token.Location.Line, node.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).Token.Location.Column);
            }
        }
    }
}