using Irony.Parsing;
using Irony.Ast;


namespace Optimizador
{
    class Gramatica : Grammar {
        public Gramatica() : base(false){                
            #region EXPRESIONES REGULARES
            var NUMERO = new NumberLiteral("NUMBER");
            var STRING = new StringLiteral("STRING","\"");
            var IDENTIFICADOR = new IdentifierTerminal("IDENTIFICADOR");
            #endregion

            #region COMENTARIOS
            var UNILINEA = new CommentTerminal("UNILINEA", "//");
            var MULTILINEA = new CommentTerminal("MULTILINE", "/*", "*/");
            NonGrammarTerminals.Add(UNILINEA);
            NonGrammarTerminals.Add(MULTILINEA);
            #endregion

            #region TERMINALES
            var ADD = ToTerm("+");
            var SUB = ToTerm("-");
            var MUL = ToTerm("*");
            var DIV = ToTerm("/");
            var MOD = ToTerm("%");

            var G = ToTerm(">");
            var L = ToTerm("<");
            var GE = ToTerm(">=");
            var LE = ToTerm("<=");
            var NEQ = ToTerm("!=");
            var EQ = ToTerm("==");

            var ASG = ToTerm("=");

            //Simbolos
            var PARA = ToTerm("(");
            var PARC = ToTerm(")");
            var LLA = ToTerm("{");
            var LLC = ToTerm("}");
            var CORA = ToTerm("[");
            var CORC = ToTerm("]");
            var COMA = ToTerm(",");
            var PTCOMA = ToTerm(";");
            var DOSPUNTOS = ToTerm(":");


            #endregion

            #region RESERVADAS
            var RIF = ToTerm("if", "IF");
            var RGOTO = ToTerm("goto", "GOTO");
            var RRETURN = ToTerm("return", "RETURN");
            var RPRINT = ToTerm("printf", "PRINTF");
            var RINT = ToTerm("int", "INT");
            var RFLOAT = ToTerm("float", "FLOAT");
            var RVOID = ToTerm("void", "RVOID");
            var RSTACK = ToTerm("stack", "STACK");
            var RHEAP = ToTerm("heap", "HEAP");
            

            #endregion

            #region NO TERMINALES
            
            NonTerminal inicio = new NonTerminal("inicio");

            NonTerminal funcion = new NonTerminal("funcion");
            NonTerminal operacion = new NonTerminal("operacion");
            NonTerminal condicion = new NonTerminal("condicion");
            NonTerminal expresion = new NonTerminal("expresion");
            NonTerminal acceso = new NonTerminal("acceso");
            NonTerminal sentencia_asignacion = new NonTerminal("sentencia_asignacion");
            NonTerminal sentencia_if = new NonTerminal("sentencia_if");
            NonTerminal sentencia_print = new NonTerminal("sentencia_print");
            NonTerminal sentencia_goto = new NonTerminal("sentencia_goto");
            NonTerminal sentencia_call = new NonTerminal("sentencia_call");
            NonTerminal instrucciones = new NonTerminal("instrucciones");
            NonTerminal instruccion = new NonTerminal("instruccion");
            NonTerminal funciones = new NonTerminal("funciones");
            NonTerminal encabezados = new NonTerminal("encabezados");
            NonTerminal encabezado = new NonTerminal("encabezado");
            NonTerminal declaraciones = new NonTerminal("declaraciones");


            inicio.Rule
                = encabezados + funciones;

            encabezados.Rule
                = MakePlusRule(encabezados, encabezado);

            encabezado.Rule
                = RINT + declaraciones + PTCOMA
                | RFLOAT + declaraciones + PTCOMA
                | ToTerm("#include") + ToTerm("<stdio.h>")
                ;

            declaraciones.Rule
                = IDENTIFICADOR
                | RHEAP + CORA + NUMERO + CORC
                | RSTACK + CORA + NUMERO + CORC
                | MakePlusRule(declaraciones, COMA, IDENTIFICADOR)
                ;

            funciones.Rule
                = MakePlusRule(funciones, funcion);

            funcion.Rule
                = RVOID + IDENTIFICADOR + PARA + PARC + LLA + instrucciones + LLC;

            instrucciones.Rule
                = MakePlusRule(instrucciones, instruccion);

            instruccion.Rule
                = sentencia_call + PTCOMA
                | sentencia_goto + PTCOMA
                | sentencia_print + PTCOMA
                | sentencia_if + PTCOMA
                | sentencia_asignacion + PTCOMA
                | IDENTIFICADOR + DOSPUNTOS
                | RRETURN + PTCOMA
                ;

            sentencia_call.Rule
                = IDENTIFICADOR + PARA + PARC
                ;

            sentencia_goto.Rule
                = RGOTO + IDENTIFICADOR
                ;

            sentencia_print.Rule
                = RPRINT + PARA + STRING + COMA + expresion + PARC
                ;

            sentencia_if.Rule
                = RIF + PARA + condicion + PARC + RGOTO + IDENTIFICADOR
                ;

            sentencia_asignacion.Rule
                = acceso + ASG + operacion // Heap[0] = E + E
                | acceso + ASG + expresion // HEAP[0] = 52/T1
                | IDENTIFICADOR + ASG + operacion   //T0 = 52 + T1
                | IDENTIFICADOR + ASG + expresion   //T0 = 52/T1
                | IDENTIFICADOR + ASG + acceso      //T0 = Heap[0]
                ;

            operacion.Rule 
                = expresion + ADD + expresion
                | expresion + SUB + expresion
                | expresion + MUL + expresion
                | expresion + DIV + expresion
                | expresion + MOD + expresion
                ;

            condicion.Rule
                = expresion + G + expresion
                | expresion + L + expresion
                | expresion + GE + expresion
                | expresion + LE + expresion
                | expresion + EQ + expresion
                | expresion + NEQ + expresion
                ;

            expresion.Rule
                = IDENTIFICADOR
                | NUMERO
                | SUB + NUMERO
                ;

            acceso.Rule
                = RHEAP + CORA + expresion + CORC
                | RSTACK + CORA + expresion + CORC
                ;
            #endregion

            this.Root = inicio;
        }
    }
}
