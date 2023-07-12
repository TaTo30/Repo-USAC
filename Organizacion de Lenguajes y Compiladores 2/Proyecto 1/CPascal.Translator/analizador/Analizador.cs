using Irony.Parsing;
using Irony.Ast;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Diagnostics;
class CompiParser
{
    public string Analizar(string cadena){
        Traductor gramatica = new Traductor();
        LanguageData lenguaje = new LanguageData(gramatica);
        Parser parser = new Parser(lenguaje);
        ParseTree arbol = parser.Parse(cadena);
        ParseTreeNode raiz = arbol.Root;

        if (arbol.Status == ParseTreeStatus.Error)
        {
            Graficador graph = new Graficador();
            foreach (var item in arbol.ParserMessages)
            {
                graph.AgregarError(new Error(item.Location.Line, item.Location.Column, item.Message, item.Message.ToString().Contains("Syntax")? Error.Tipo.SINTACTICO: Error.Tipo.LEXICO));
            }
            Debug.WriteLine($"Se han encontrado errores lexico/sintacticos durante la construccion de AST por favor revisar el log de errores");
            Log.AddLog($"Se han encontrado errores lexico/sintacticos durante la construccion de AST por favor revisar el log de errores\r\n");
            graph.Print(Graficador.Graph.ERROR);
            return cadena;
        } else {
            //TraducirAST(raiz);
            string traduccion = String.Format("program {0};\r\n{1}\r\nbegin\r\n{2}\r\nend.",
                raiz.ChildNodes.ElementAt(1).Token.ValueString,
                Instrucciones(raiz.ChildNodes.ElementAt(3)),
                Instrucciones(raiz.ChildNodes.ElementAt(5))
            );
            Log.AddLog("Traduccion Terminada \r\n");
            Debug.WriteLine("Traduccion Terminada");
            return traduccion;
        }        
    }

    private void TraducirAST(ParseTreeNode raiz){
        using (System.IO.StreamWriter redactor = new System.IO.StreamWriter("./CPascal.Docs/traduccion.pas"))
        {
            redactor.Write(String.Format("program {0};\r\n{1}\r\nbegin\r\n{2}\r\nend.",
                raiz.ChildNodes.ElementAt(1).Token.ValueString,
                Instrucciones(raiz.ChildNodes.ElementAt(3)),
                Instrucciones(raiz.ChildNodes.ElementAt(5))
            ));
        }
        Console.Write("Traduccion Terminada");
    }

    private string Instrucciones(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 2)
        {
            return String.Format("{0}\r\n{1}", Instrucciones(node.ChildNodes.ElementAt(0), func), Instruccion(node.ChildNodes.ElementAt(1), func));
        }
        else if (node.ChildNodes.Count == 1) {
            return String.Format("{0}\r\n", Instruccion(node.ChildNodes.ElementAt(0), func));
        }else{
            return "";
        }

    }

    private string Instruccion(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 1)
        {
            return Funciones(node.ChildNodes.ElementAt(0), func);
        } else if (node.ChildNodes.Count == 2){
            switch (node.ChildNodes.ElementAt(0).Term.Name)
            {
                case "VAR":
                    return String.Format("var {0}", 
                        ListaDeclaracion(node.ChildNodes.ElementAt(1), func));
                case "CONST":
                    return String.Format("const {0}",
                        ListaConstante(node.ChildNodes.ElementAt(1), func));
                case "BREAK":
                    return "break;\r\n";
                case "CONTINUE":
                    return "continue;\r\n";
                case "SENTENCIACASE":
                    return String.Format("{0};\r\n", 
                        CaseSentencia(node.ChildNodes.ElementAt(0), func));
                case "SENTENCIAWHILE":
                    return String.Format("{0};\r\n", 
                        WhileSentencia(node.ChildNodes.ElementAt(0), func));
                case "SENTENCIAFOR":
                    return String.Format("{0};\r\n", 
                        ForSentencia(node.ChildNodes.ElementAt(0), func));
                case "SENTENCIAREPEAT":
                    return String.Format("{0};\r\n", 
                        RepeatSentencia(node.ChildNodes.ElementAt(0), func));
                case "SENTENCIAIF":
                    return String.Format("{0};\r\n", 
                        IfSentencia(node.ChildNodes.ElementAt(0), func));
                case "CALL":
                    return String.Format("{0};\r\n", 
                        Call(node.ChildNodes.ElementAt(0), func));
                default:
                    return "";               
            }
        } else if (node.ChildNodes.Count == 4){
            if (node.ChildNodes.ElementAt(0).Term.Name.Equals("OBJETO"))
            {
                return String.Format("{0} := {1};\r\n",
                    Objeto(node.ChildNodes.ElementAt(0), func),
                    Expresion(node.ChildNodes.ElementAt(2), func));
            } else if (node.ChildNodes.ElementAt(0).Term.Name.Equals("GRAFICAR_TS")) {
                return "graficar_ts();\r\n";  
            } else if (node.ChildNodes.ElementAt(0).Term.Name.Equals("ARRAY")){
                return String.Format("{0} := {1};\r\n",
                    Array(node.ChildNodes.ElementAt(0), func),
                    Expresion(node.ChildNodes.ElementAt(2), func));
            } else {
                return String.Format("{0} := {1};\r\n",
                    node.ChildNodes.ElementAt(0).Token.ValueString,
                    Expresion(node.ChildNodes.ElementAt(2), func));
            }
        } else if (node.ChildNodes.Count == 5){
            if (node.ChildNodes.ElementAt(0).Term.Name.Equals("WRITE"))
            {
                return String.Format("write({0});\r\n",
                    WriteList(node.ChildNodes.ElementAt(2), func));
            }else if (node.ChildNodes.ElementAt(0).Term.Name.Equals("WRITELN")){
                return String.Format("writeln({0});\r\n",
                    WriteList(node.ChildNodes.ElementAt(2), func));
            }else if (node.ChildNodes.ElementAt(0).Term.Name.Equals("TYPE")){
                return String.Format("type {0} = {1};\r\n",
                    node.ChildNodes.ElementAt(1).Token.ValueString,
                    Type(node.ChildNodes.ElementAt(3), func));
            }else if (node.ChildNodes.ElementAt(0).Term.Name.Equals("EXIT")){
                return String.Format("exit({0});\r\n",
                    Expresion(node.ChildNodes.ElementAt(2), func));
            }else{
                return "";
            }         
        }
        return "";
    }

    private string Funciones(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 13)
        {
            if (func == null)
            {
                //funcion padre
                Funciones padre = new Funciones(node.ChildNodes.ElementAt(1).Token.ValueString);
                padre.AgregarTraduccion(String.Format("function {0}({1}):{2};\r\n{3}\r\nbegin\r\n{4}end;\r\n", 
                    node.ChildNodes.ElementAt(1).Token.ValueString, 
                    ListaDeclaracion(node.ChildNodes.ElementAt(3), padre),
                    Tipo(node.ChildNodes.ElementAt(6), padre), 
                    Instrucciones(node.ChildNodes.ElementAt(8), padre), 
                    Instrucciones(node.ChildNodes.ElementAt(10), padre)));
                return padre.ObtenerTraduccion();
            }else{
                //funcion hija
                Funciones hija = new Funciones(node.ChildNodes.ElementAt(1).Token.ValueString);
                string identificador = node.ChildNodes.ElementAt(1).Token.ValueString;
                string listadeclaracion = ListaDeclaracion(node.ChildNodes.ElementAt(3), hija);
                string tipo = Tipo(node.ChildNodes.ElementAt(6), hija);
                string declarativos = Instrucciones(node.ChildNodes.ElementAt(8), hija);
                string instructivos = Instrucciones(node.ChildNodes.ElementAt(10), hija);              
                func.AgregarHija(hija);  
                func.ConectarPadreHija();
                hija.AgregarTraduccion(String.Format("function {0}({1}{2}):{3};\r\n{4}\r\nbegin\r\n{5}end;\r\n", 
                    identificador,
                    listadeclaracion,
                    hija.GetNecesitados(),
                    tipo,
                    declarativos,
                    instructivos));
                return "";
            }
        } else if (node.ChildNodes.Count == 12) {
            if (func == null)
            {
                //funcion padre
                Funciones padre = new Funciones(node.ChildNodes.ElementAt(1).Token.ValueString);
                padre.AgregarTraduccion(String.Format("function {0}():{1};\r\n{2}\r\nbegin\r\n{3}end;\r\n", 
                    node.ChildNodes.ElementAt(1).Token.ValueString, 
                    Tipo(node.ChildNodes.ElementAt(5), padre), 
                    Instrucciones(node.ChildNodes.ElementAt(7), padre), 
                    Instrucciones(node.ChildNodes.ElementAt(9), padre)));
                return padre.ObtenerTraduccion();
            }else{
                //funcion hija
                Funciones hija = new Funciones(node.ChildNodes.ElementAt(1).Token.ValueString);
                string identificador = node.ChildNodes.ElementAt(1).Token.ValueString;
                string tipo = Tipo(node.ChildNodes.ElementAt(5), hija);
                string declarativos = Instrucciones(node.ChildNodes.ElementAt(7), hija);
                string instructivos = Instrucciones(node.ChildNodes.ElementAt(9), hija);
                func.AgregarHija(hija);
                func.ConectarPadreHija();
                hija.AgregarTraduccion(String.Format("function {0}({1}):{2};\r\n{3}\r\nbegin\r\n{4}end;\r\n",
                    identificador,
                    hija.GetNecesitados(),
                    tipo,
                    declarativos,
                    instructivos));
                return "";
            }
        } else if (node.ChildNodes.Count == 11) {
            if (func == null)
            {
                //funcion padre
                Funciones padre = new Funciones(node.ChildNodes.ElementAt(1).Token.ValueString);
                padre.AgregarTraduccion(String.Format("procedure {0}({1});\r\n{2}\r\nbegin\r\n{3}end;\r\n", 
                    node.ChildNodes.ElementAt(1).Token.ValueString, 
                    ListaDeclaracion(node.ChildNodes.ElementAt(3), padre), 
                    Instrucciones(node.ChildNodes.ElementAt(6), padre), 
                    Instrucciones(node.ChildNodes.ElementAt(8), padre)));
                return padre.ObtenerTraduccion();
            }else{
                //funcion hija
                Funciones hija = new Funciones(node.ChildNodes.ElementAt(1).Token.ValueString);
                string identificador = node.ChildNodes.ElementAt(1).Token.ValueString;
                string listadeclaracion = ListaDeclaracion(node.ChildNodes.ElementAt(3), hija);
                string declarativos = Instrucciones(node.ChildNodes.ElementAt(6), hija);
                string instructivos = Instrucciones(node.ChildNodes.ElementAt(8), hija);
                func.AgregarHija(hija);
                func.ConectarPadreHija();
                hija.AgregarTraduccion(String.Format("procedure {0}({1}{2});\r\n{3}\r\nbegin\r\n{4}end;\r\n", 
                    identificador,
                    listadeclaracion,
                    hija.GetNecesitados(),
                    declarativos,
                    instructivos));                  
                return "";
            }
        } else if (node.ChildNodes.Count == 10) {
            if (node.ChildNodes.ElementAt(0).Term.Name.Equals("FUNCTION"))
            {
                if (func == null)
                {
                    //funcion padre
                    Funciones padre = new Funciones(node.ChildNodes.ElementAt(1).Token.ValueString);
                    padre.AgregarTraduccion(String.Format("function {0}:{1};\r\n{2}\r\nbegin\r\n{3}end;\r\n", 
                        node.ChildNodes.ElementAt(1).Token.ValueString, 
                        Tipo(node.ChildNodes.ElementAt(3), padre), 
                        Instrucciones(node.ChildNodes.ElementAt(5), padre), 
                        Instrucciones(node.ChildNodes.ElementAt(7), padre)));
                    return padre.ObtenerTraduccion();
                }else{
                    //funcion hija
                    Funciones hija = new Funciones(node.ChildNodes.ElementAt(1).Token.ValueString);
                    string identificador = node.ChildNodes.ElementAt(1).Token.ValueString;
                    string tipo = Tipo(node.ChildNodes.ElementAt(3), hija);
                    string declarativos = Instrucciones(node.ChildNodes.ElementAt(5), hija);
                    string instructivos = Instrucciones(node.ChildNodes.ElementAt(7), hija);
                    func.AgregarHija(hija);  
                    func.ConectarPadreHija();
                    hija.AgregarTraduccion(String.Format("function {0}({1}):{2};\r\n{3}\r\nbegin\r\n{4}end;\r\n",
                        identificador,
                        hija.GetNecesitados(),
                        tipo,
                        declarativos,
                        instructivos));
                    return "";
                }
            } else {
                if (func == null)
                {
                    //procedimiento padre
                    Funciones padre = new Funciones(node.ChildNodes.ElementAt(1).Token.ValueString);
                    padre.AgregarTraduccion(String.Format("procedure {0}();\r\n{1}\r\nbegin\r\n{2}end;\r\n", 
                        node.ChildNodes.ElementAt(1).Token.ValueString, 
                        Instrucciones(node.ChildNodes.ElementAt(5), padre), 
                        Instrucciones(node.ChildNodes.ElementAt(7), padre)));
                    return padre.ObtenerTraduccion();
                }else{
                    //procedimiento hija
                    Funciones hija = new Funciones(node.ChildNodes.ElementAt(1).Token.ValueString);
                    string identificador = node.ChildNodes.ElementAt(1).Token.ValueString;
                    string declarativos = Instrucciones(node.ChildNodes.ElementAt(5), hija);
                    string instructivos = Instrucciones(node.ChildNodes.ElementAt(7), hija);
                    func.AgregarHija(hija);  
                    func.ConectarPadreHija();
                    hija.AgregarTraduccion(String.Format("procedure {0}({1});\r\n{2}\r\nbegin\r\n{3}end;\r\n", 
                        node.ChildNodes.ElementAt(1).Token.ValueString, 
                        hija.GetNecesitados(),
                        Instrucciones(node.ChildNodes.ElementAt(5), hija), 
                        Instrucciones(node.ChildNodes.ElementAt(7), hija)));
                    return "";
                }
            }            
        } else {
            if (func == null)
            {
                //funcion padre
                Funciones padre = new Funciones(node.ChildNodes.ElementAt(1).Token.ValueString);
                padre.AgregarTraduccion(String.Format("procedure {0};\r\n{1}\r\nbegin\r\n{2}end;\r\n", 
                    node.ChildNodes.ElementAt(1).Token.ValueString, 
                    Instrucciones(node.ChildNodes.ElementAt(3), padre), 
                    Instrucciones(node.ChildNodes.ElementAt(5), padre)));
                padre.ConectarPadreHija();
                return padre.ObtenerTraduccion();
            }else{
                //funcion hija
                Funciones hija = new Funciones(node.ChildNodes.ElementAt(1).Token.ValueString);
                string identificador = node.ChildNodes.ElementAt(1).Token.ValueString;
                string declarativos = Instrucciones(node.ChildNodes.ElementAt(3), hija);
                string instructivos = Instrucciones(node.ChildNodes.ElementAt(5), hija);
                func.AgregarHija(hija);  
                func.ConectarPadreHija();
                hija.AgregarTraduccion(String.Format("procedure {0}({1});\r\n{2}\r\nbegin\r\n{3}end;\r\n", 
                    identificador,
                    hija.GetNecesitados(),
                    declarativos,
                    instructivos));
                return "";
            }
        }
    }

    private string Array(ParseTreeNode node, Funciones func = null){
        return String.Format("{0}[{1}]",
        node.ChildNodes.ElementAt(0).Token.ValueString,
        Expresion(node.ChildNodes.ElementAt(2), func));
    }
    private string Call(ParseTreeNode node, Funciones func = null){
        //==========LLAMADA A PROCEDIMIENTO=========
        if (node.ChildNodes.Count == 4)
        {
            string identificador = node.ChildNodes.ElementAt(0).Token.ValueString;
            string parametros = Parametros(node.ChildNodes.ElementAt(2), func);
            string masparametros = "";
            if (func != null)
                masparametros = func.SetNecesitados(identificador);
            return String.Format("{0}({1}{2})", identificador, parametros, masparametros);
        }else if (node.ChildNodes.Count == 3){
            string identificador = node.ChildNodes.ElementAt(0).Token.ValueString;
            string masparametros = "";
            if (func != null)
                masparametros = func.SetNecesitados(identificador, true);
            return String.Format("{0}({1})", identificador, masparametros);
        }else{
            return node.ChildNodes.ElementAt(0).Token.ValueString;
        }
        
    }

    private string RepeatSentencia(ParseTreeNode node, Funciones func = null){
        return String.Format("repeat\r\n {0} until {1}",
            Instrucciones(node.ChildNodes.ElementAt(1), func),
            Condicion(node.ChildNodes.ElementAt(3), func));
    }

    private string ForSentencia(ParseTreeNode node, Funciones func = null){
        return String.Format("for {0} := {1} {2} {3} do \r\n {4}",
            node.ChildNodes.ElementAt(1).Token.ValueString,
            Expresion(node.ChildNodes.ElementAt(3), func),
            node.ChildNodes.ElementAt(4).Token.ValueString,
            Expresion(node.ChildNodes.ElementAt(5), func),
            InstruccionesBloque(node.ChildNodes.ElementAt(7), func));
    }

    private string WhileSentencia(ParseTreeNode node, Funciones func = null){
        return String.Format("while {0} do \r\n {1}",
            Condicion(node.ChildNodes.ElementAt(1), func),
            InstruccionesBloque(node.ChildNodes.ElementAt(3), func));
    }

    private string CaseSentencia(ParseTreeNode node, Funciones func = null){
        return String.Format("case({0}) of \r\n {1} end",
            Expresion(node.ChildNodes.ElementAt(2), func),
            CaseValue(node.ChildNodes.ElementAt(5), func));
    }

    private string CaseValue(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 5)
        {
            return String.Format("{0}{1}: {2};\r\n",
                CaseValue(node.ChildNodes.ElementAt(0), func),
                CaseList(node.ChildNodes.ElementAt(1), func),
                InstruccionesBloque(node.ChildNodes.ElementAt(3), func));
        }else{
            if (node.ChildNodes.ElementAt(0).Term.Name.Equals("CASEVALUE"))
            {
                return String.Format("{0}else {1};\r\n",
                    CaseValue(node.ChildNodes.ElementAt(0), func),
                    InstruccionesBloque(node.ChildNodes.ElementAt(2), func));
            }else{
                return String.Format("{0}: {1};\r\n",
                    CaseList(node.ChildNodes.ElementAt(0), func),
                    InstruccionesBloque(node.ChildNodes.ElementAt(2), func));
            }
        }
    }

    private string CaseList(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 3)
        {
            return String.Format("{0}, {1}",
                CaseList(node.ChildNodes.ElementAt(0), func),
                Expresion(node.ChildNodes.ElementAt(2), func));
        }else{
            return Expresion(node.ChildNodes.ElementAt(0), func);
        }
    }

    private string IfSentencia(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 4)
        {
            return String.Format("if {0} then\r\n {1}",
                Condicion(node.ChildNodes.ElementAt(1), func),
                InstruccionesBloque(node.ChildNodes.ElementAt(3), func));
        }else{
            return String.Format("if {0} then\r\n {1} else\r\n {2}",
                Condicion(node.ChildNodes.ElementAt(1), func),
                InstruccionesBloque(node.ChildNodes.ElementAt(3), func),
                InstruccionesBloque(node.ChildNodes.ElementAt(5), func));
        }
    }


    private string Condicion(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 3)
        {
            return String.Format("({0})",
                Expresion(node.ChildNodes.ElementAt(1), func));
        }else{
            return Expresion(node.ChildNodes.ElementAt(0), func);
        }
    }

    private string InstruccionesBloque(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 3)
        {
            return String.Format("begin\r\n{0}end\r\n",
                Instrucciones(node.ChildNodes.ElementAt(1), func));
        }else{
            return Ternaria(node.ChildNodes.ElementAt(0));
        }
    }

    private string Ternaria(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 4){
            if (node.ChildNodes.ElementAt(0).Term.Name.Equals("WRITE"))
            {
                //=============FUNCION WRITE================
                return String.Format("write({0})",
                    WriteList(node.ChildNodes.ElementAt(2), func));
            }else if (node.ChildNodes.ElementAt(0).Term.Name.Equals("WRITELN")){
                //=============FUNCION WRITELN==============
                return String.Format("writeln({0})",
                    WriteList(node.ChildNodes.ElementAt(2), func));
            }else{
                return String.Format("exit({0})",
                    Expresion(node.ChildNodes.ElementAt(2), func));
            }          
        } else if (node.ChildNodes.Count == 3){
            if (node.ChildNodes.ElementAt(0).Term.Name.Equals("OBJETO"))
            {
                return String.Format("{0} := {1}",
                    Objeto(node.ChildNodes.ElementAt(0), func),
                    Expresion(node.ChildNodes.ElementAt(2), func));   
            } else if (node.ChildNodes.ElementAt(0).Term.Name.Equals("GRAFICAR_TS")) {
                return "graficar_ts()";
            } else if (node.ChildNodes.ElementAt(0).Term.Name.Equals("ARRAY")) {
                return String.Format("{0} := {1}",
                    Array(node.ChildNodes.ElementAt(0), func),
                    Expresion(node.ChildNodes.ElementAt(2), func));
            } else {
                //===============ASIGNACION TIPICA=============
                return String.Format("{0} := {1}",
                    node.ChildNodes.ElementAt(0).Token.ValueString,
                    Expresion(node.ChildNodes.ElementAt(2), func));
            }
        } else if (node.ChildNodes.Count == 1){
            switch (node.ChildNodes.ElementAt(0).Term.Name)
            {
                case "SENTENCIACASE":
                    //=============CASE=========================
                    return CaseSentencia(node.ChildNodes.ElementAt(0), func);
                case "SENTENCIAWHILE":
                    //=============WHILE========================
                    return WhileSentencia(node.ChildNodes.ElementAt(0), func);
                case "SENTENCIAFOR":
                    //==============FOR=========================
                    return ForSentencia(node.ChildNodes.ElementAt(0), func);
                case "SENTENCIAREPEAT":
                    //=============REPEAT=======================
                    return RepeatSentencia(node.ChildNodes.ElementAt(0), func);
                case "SENTENCIAIF":
                    //==============IF==========================
                    return IfSentencia(node.ChildNodes.ElementAt(0), func);
                case "CALL":
                    return Call(node.ChildNodes.ElementAt(0), func);
                case "BREAK":
                    //=============BREAK========================
                    return "break";
                case "CONTINUE":
                    //=============CONTINUE=====================
                    return "continue";
                default:
                    return "";
            }    
        } else {
            return "";
        }
    }

    private string Objeto(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 3)
        {
            return String.Format("{0}.{1}",
                Objeto(node.ChildNodes.ElementAt(0), func),
                node.ChildNodes.ElementAt(2).Token.ValueString);
        } else {
            return node.ChildNodes.ElementAt(0).Token.ValueString;
        }
    }
    private string WriteList(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 3){
            return String.Format("{0}, {1}",
                WriteList(node.ChildNodes.ElementAt(0), func),
                Expresion(node.ChildNodes.ElementAt(2), func));
        } else {
            return Expresion(node.ChildNodes.ElementAt(0), func);
        }
    }

    private string Type(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 3)
        {
            return String.Format("object {0} end",
                ListaObject(node.ChildNodes.ElementAt(1), func));
        } else {
            return String.Format("array[{0}..{1}] of {2}",
                Expresion(node.ChildNodes.ElementAt(2), func),
                Expresion(node.ChildNodes.ElementAt(5), func),
                Tipo(node.ChildNodes.ElementAt(8), func));
        }
    }
    
    private string ListaObject(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 3){
            return String.Format("{0}var {1}\r\n", 
                ListaObject(node.ChildNodes.ElementAt(0), func),
                ListaDeclaracion(node.ChildNodes.ElementAt(2), func));    
        }else{
            return String.Format("var {0}\r\n",
                ListaDeclaracion(node.ChildNodes.ElementAt(1), func));
        }
    }

    private string ListaConstante(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 5)
        {
            return String.Format("{0}{1} = {2};\r\n",
                ListaConstante(node.ChildNodes.ElementAt(0), func),
                node.ChildNodes.ElementAt(1).Token.ValueString,
                Expresion(node.ChildNodes.ElementAt(3), func));
        }else { 
            return String.Format("{0} = {1};\r\n",
                node.ChildNodes.ElementAt(0).Token.ValueString,
                Expresion(node.ChildNodes.ElementAt(2), func));
        }
    }

    private string ListaDeclaracion(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 6)
        {
            string value = String.Format("{0} var {1} : {2}{3}",
                ListaDeclaracion(node.ChildNodes.ElementAt(0), func),
                Declaracion(node.ChildNodes.ElementAt(2), func),
                Tipo(node.ChildNodes.ElementAt(4), func),
                DeclaracionSufija(node.ChildNodes.ElementAt(5), func));
            if (func != null)
                func.AgregarVariableTipo(Tipo(node.ChildNodes.ElementAt(4), func));
            return value;
        } else if (node.ChildNodes.Count == 5){
            if (node.ChildNodes.ElementAt(0).Term.Name.Equals("VAR"))
            {
                string value = String.Format("var {0} : {1}{2}",
                    Declaracion(node.ChildNodes.ElementAt(1), func),
                    Tipo(node.ChildNodes.ElementAt(3), func),
                    DeclaracionSufija(node.ChildNodes.ElementAt(4), func));
                if (func != null)
                    func.AgregarVariableTipo(Tipo(node.ChildNodes.ElementAt(3), func));
                return value;
            } else {
                string value = String.Format("{0}{1} : {2}{3}",
                    ListaDeclaracion(node.ChildNodes.ElementAt(0), func),
                    Declaracion(node.ChildNodes.ElementAt(1), func),
                    Tipo(node.ChildNodes.ElementAt(3), func),
                    DeclaracionSufija(node.ChildNodes.ElementAt(4), func));
                if (func != null)
                    func.AgregarVariableTipo(Tipo(node.ChildNodes.ElementAt(3), func));
                return value;
            }
        } else {
            string value = String.Format("{0} : {1}{2}",
                Declaracion(node.ChildNodes.ElementAt(0), func),
                Tipo(node.ChildNodes.ElementAt(2), func),
                DeclaracionSufija(node.ChildNodes.ElementAt(3), func));
            if (func != null)
                func.AgregarVariableTipo(Tipo(node.ChildNodes.ElementAt(2), func));
            return value;
        }
    }

    private string DeclaracionSufija(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 3)
        {
            return String.Format(" = {0};\r\n",
                Expresion(node.ChildNodes.ElementAt(1), func));
        } else if (node.ChildNodes.Count == 1) {
            return ";\r\n";
        } else {
            return "";
        }
    }
    private string Declaracion(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 3)
        {
            if (func != null)
                func.AgregarVariable(node.ChildNodes.ElementAt(2).Token.ValueString);
            return String.Format("{0}, {1}",
                Declaracion(node.ChildNodes.ElementAt(0), func),
                node.ChildNodes.ElementAt(2).Token.ValueString);
        } else {
            if (func != null)
                func.AgregarVariable(node.ChildNodes.ElementAt(0).Token.ValueString);
            return node.ChildNodes.ElementAt(0).Token.ValueString;
        }
    }

    private string Tipo(ParseTreeNode node, Funciones func = null){
        return node.ChildNodes.ElementAt(0).Token.ValueString;
    }

    private string Parametros(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 3)
        {
            return String.Format("{0}, {1}",
                Parametros(node.ChildNodes.ElementAt(0), func),
                Expresion(node.ChildNodes.ElementAt(2), func));
        } else {
            return Expresion(node.ChildNodes.ElementAt(0), func);
        }
    }

    private string Expresion(ParseTreeNode node, Funciones func = null){
        if (node.ChildNodes.Count == 3){
            switch (node.ChildNodes.ElementAt(1).Term.Name)
            {
                case "+":
                    return String.Format("{0} + {1}", 
                        Expresion(node.ChildNodes.ElementAt(0), func),
                        Expresion(node.ChildNodes.ElementAt(2), func));
                case "-":
                    return String.Format("{0} - {1}", 
                        Expresion(node.ChildNodes.ElementAt(0), func),
                        Expresion(node.ChildNodes.ElementAt(2), func));
                case "*":
                    return String.Format("{0} * {1}", 
                        Expresion(node.ChildNodes.ElementAt(0), func),
                        Expresion(node.ChildNodes.ElementAt(2), func));
                case "/":
                    return String.Format("{0} / {1}", 
                        Expresion(node.ChildNodes.ElementAt(0), func),
                        Expresion(node.ChildNodes.ElementAt(2), func));
                case "%":
                    return String.Format("{0} % {1}", 
                        Expresion(node.ChildNodes.ElementAt(0), func),
                        Expresion(node.ChildNodes.ElementAt(2), func));
                case ">":
                    return String.Format("{0} > {1}", 
                        Expresion(node.ChildNodes.ElementAt(0), func),
                        Expresion(node.ChildNodes.ElementAt(2), func));
                case "<":
                    return String.Format("{0} < {1}", 
                        Expresion(node.ChildNodes.ElementAt(0), func),
                        Expresion(node.ChildNodes.ElementAt(2), func));
                case ">=":
                    return String.Format("{0} >= {1}", 
                        Expresion(node.ChildNodes.ElementAt(0), func),
                        Expresion(node.ChildNodes.ElementAt(2), func));
                case "<=":
                    return String.Format("{0} <= {1}", 
                        Expresion(node.ChildNodes.ElementAt(0), func),
                        Expresion(node.ChildNodes.ElementAt(2), func));
                case "<>":
                    return String.Format("{0} <> {1}", 
                        Expresion(node.ChildNodes.ElementAt(0), func),
                        Expresion(node.ChildNodes.ElementAt(2), func));
                case "=":
                    return String.Format("{0} = {1}", 
                        Expresion(node.ChildNodes.ElementAt(0), func),
                        Expresion(node.ChildNodes.ElementAt(2), func));
                case "AND":
                    return String.Format("{0} and {1}", 
                        Expresion(node.ChildNodes.ElementAt(0), func),
                        Expresion(node.ChildNodes.ElementAt(2), func));
                case "OR":
                    return String.Format("{0} or {1}", 
                        Expresion(node.ChildNodes.ElementAt(0), func),
                        Expresion(node.ChildNodes.ElementAt(2), func));
                default:
                    return String.Format("({0})", 
                        Expresion(node.ChildNodes.ElementAt(1), func));
            }            
        } else if (node.ChildNodes.Count == 2){
            switch (node.ChildNodes.ElementAt(0).Term.Name){
                case "NOT":
                    return String.Format("not {0}", 
                        Expresion(node.ChildNodes.ElementAt(1), func));
                case "-":
                    return String.Format("-{0}", 
                        Expresion(node.ChildNodes.ElementAt(1), func));
                default:
                    return null;
            }
        } else {
            switch (node.ChildNodes.ElementAt(0).Term.Name)
            {
                case "NUMBER":
                    return node.ChildNodes.ElementAt(0).Token.ValueString;
                case "IDENTIFICADOR":
                    if (func != null)
                        func.AgregarNecesidad(node.ChildNodes.ElementAt(0).Token.ValueString);
                    return node.ChildNodes.ElementAt(0).Token.ValueString;
                case "STRING":
                    return String.Format("'{0}'", node.ChildNodes.ElementAt(0).Token.ValueString);
                case "BOOLEAN":
                    return node.ChildNodes.ElementAt(0).Token.ValueString;
                case "CALL":
                    return Call(node.ChildNodes.ElementAt(0), func);
                case "ARRAY":
                    return Array(node.ChildNodes.ElementAt(0), func);
                default:
                    return Objeto(node.ChildNodes.ElementAt(0), func);
                    //return new Operacion(Objeto(node.ChildNodes.ElementAt(0)), Operacion.TipoOperacion.OBJETO);
            }
        }
    }
}
