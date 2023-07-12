using Irony.Parsing;
using Irony.Ast;

class Gramatica : Grammar
{
    public Gramatica() : base(false){

                
        #region EXPRESIONES REGULARES
        var NUMERO = new NumberLiteral("NUMBER");
        var STRING = new StringLiteral("STRING","'");
        var IDENTIFICADOR = new IdentifierTerminal("IDENTIFICADOR");
        #endregion

        #region COMENTARIOS
        var UNILINE = new CommentTerminal("UNILINE","//","\n");
        var MULTILINE = new CommentTerminal("MULTILINE","(*","*)");
        var MULTILINEV2 = new CommentTerminal("MULTILINE","{","}");

        NonGrammarTerminals.Add(UNILINE);
        NonGrammarTerminals.Add(MULTILINE);
        NonGrammarTerminals.Add(MULTILINEV2);
        #endregion

        #region TERMINALES
        //Operadores
        var ADD = ToTerm("+");
        var SUB = ToTerm("-");
        var MUL = ToTerm("*");
        var DIV = ToTerm("/");
        var MOD = ToTerm("%");
        var G = ToTerm(">");
        var L = ToTerm("<");
        var GE = ToTerm(">=");
        var LE = ToTerm("<=");
        var NEQ = ToTerm("<>");
        var EQ = ToTerm("=");
        var AND = ToTerm("AND");
        var OR = ToTerm("OR");
        var NOT = ToTerm("NOT");
        
        var ASG = ToTerm(":=");
        //Simbolos
        var PARA = ToTerm("(");
        var PARC = ToTerm(")");
        var CORA = ToTerm("[");
        var CORC = ToTerm("]");
        var COMA = ToTerm(",");
        var PTCOMA = ToTerm(";");
        var DOSPUNTOS = ToTerm(":");
        var PUNTO = ToTerm(".");

        //Booleanos
        var TRUE = ToTerm("TRUE", "BOOLEAN");
        var FALSE = ToTerm("FALSE", "BOOLEAN");

        RegisterOperators(1, G, L, GE, LE, EQ, NEQ);
        RegisterOperators(2, ADD, SUB, OR);
        RegisterOperators(3, MUL, DIV, MOD, AND);
        RegisterOperators(4, NOT);
        
        RegisterOperators(-1, ASG);
        #endregion

        #region RESERVADAS
        var RWRITE = ToTerm("WRITE");
        var RWRITELN = ToTerm("WRITELN");
        var RVAR = ToTerm("VAR");
        var RPROGRAM = ToTerm("PROGRAM");
        var RBEGIN = ToTerm("BEGIN");
        var REND = ToTerm("END");
        var RCONST = ToTerm("CONST");
        var RARRAY = ToTerm("ARRAY");
        var ROF = ToTerm("OF");
        var RIF = ToTerm("IF");
        var RTHEN = ToTerm("THEN");
        var RELSE = ToTerm("ELSE");
        var RCASE = ToTerm("CASE");
        var RWHILE = ToTerm("WHILE");
        var RDO = ToTerm("DO");
        var RTO = ToTerm("TO");
        var RDOWN = ToTerm("DOWNTO");
        var RFOR = ToTerm("FOR");
        var RREPEAT = ToTerm("REPEAT");
        var RUNTIL = ToTerm("UNTIL");
        var RBREAK = ToTerm("BREAK");
        var RCONTINUE = ToTerm("CONTINUE");
        var RFUNCTION = ToTerm("FUNCTION");
        var RPROCEDURE = ToTerm("PROCEDURE");
        var REXIT = ToTerm("EXIT");
        var RGRAFICAR = ToTerm("GRAFICAR_TS");

        var ROBJECTO = ToTerm("OBJECT");
        var RBOOLEAN = ToTerm("BOOLEAN");
        var RSTRING = ToTerm("STRING");
        var RINT = ToTerm("INTEGER");
        var RREAL = ToTerm("REAL");
        var RTYPE = ToTerm("TYPE");
        #endregion

        #region NO TERMINALES
        NonTerminal inicio = new NonTerminal("INICIO");
        NonTerminal instrucciones = new NonTerminal("INSTRUCCIONES");
        NonTerminal instruccion = new NonTerminal("INSTRUCCION");
        NonTerminal ternaria = new NonTerminal("TERNARIA");
        NonTerminal expresion = new NonTerminal("EXPRESION");
        NonTerminal listadeclaracion = new NonTerminal("LISTADECLARACION");
        NonTerminal declaracion = new NonTerminal("DECLARACION");
        NonTerminal listaconstante = new NonTerminal("LISTACONSTANTE");
        NonTerminal tipo = new NonTerminal("TIPO");
        NonTerminal type = new NonTerminal("TYPE");
        NonTerminal writelist = new NonTerminal("WRITELIST");
        NonTerminal objectvarzone = new NonTerminal("OBJECTVARZONE");
        NonTerminal objeto = new NonTerminal("OBJETO");
        NonTerminal sentenciaIf = new NonTerminal("SENTENCIAIF");
        NonTerminal instruccionesBloque = new NonTerminal("INSTRUCCIONESIF");
        NonTerminal condicion = new NonTerminal("CONDICION");
        NonTerminal sentenciaCase = new NonTerminal("SENTENCIACASE");
        NonTerminal caseValue = new NonTerminal("CASEVALUE");
        NonTerminal caseList = new NonTerminal("CASELIST");
        NonTerminal sentenciaWhile = new NonTerminal("SENTENCIAWHILE");
        NonTerminal sentenciaFor = new NonTerminal("SENTENCIAFOR");
        NonTerminal sentenciaRepeat = new NonTerminal("SENTENCIAREPEAT");
        NonTerminal parametros = new NonTerminal("PARAMETROS");
        NonTerminal declaracionsufe = new NonTerminal("DECLARACIONSUFIJA");
        NonTerminal funcionesProcedimientos = new NonTerminal("FUNCPROC");
        NonTerminal call = new NonTerminal("CALL");
        NonTerminal array = new NonTerminal("ARRAY");
        


        inicio.Rule = 
            RPROGRAM + IDENTIFICADOR + PTCOMA + instrucciones + RBEGIN + instrucciones + REND + PUNTO;
        
        instrucciones.Rule 
            = instrucciones + instruccion
            | instruccion
            //| SyntaxError
            | Empty
            ;

        instruccion.Rule
            = RWRITE + PARA + writelist + PARC  + PTCOMA//5         //funcion Write
            | RWRITELN + PARA + writelist + PARC  + PTCOMA//5       //funcion WriteLine
            | REXIT + PARA + expresion + PARC + PTCOMA //5          //funcion Exit
            | RTYPE + IDENTIFICADOR + EQ + type + PTCOMA //5        //Declaracion de Tipo
            | objeto + ASG + expresion  + PTCOMA//4                 //Asignacion a Objeto
            | IDENTIFICADOR + ASG + expresion  + PTCOMA//4          //Asignacion a Identificador
            | array + ASG + expresion + PTCOMA //4                  //Asignacion a Array
            | RGRAFICAR + PARA + PARC + PTCOMA //4                  //funcion graficar
            | RVAR + listadeclaracion //2                           //Declaracion de Variables
            | RCONST + listaconstante //2                           //Declaracion de Constantes
            | RBREAK + PTCOMA //2                                   //Control BREAK
            | RCONTINUE + PTCOMA //2                                //Control Continue
            | sentenciaIf + PTCOMA //2
            | sentenciaCase + PTCOMA //2
            | sentenciaWhile + PTCOMA //2
            | sentenciaFor + PTCOMA //2
            | sentenciaRepeat + PTCOMA //2
            | call + PTCOMA //2
            | funcionesProcedimientos //1
            ;

        funcionesProcedimientos.Rule
            = RFUNCTION + IDENTIFICADOR + PARA + listadeclaracion + PARC + DOSPUNTOS + tipo + PTCOMA + instrucciones + RBEGIN + instrucciones + REND + PTCOMA //13
            | RFUNCTION + IDENTIFICADOR + PARA + PARC + DOSPUNTOS + tipo + PTCOMA + instrucciones + RBEGIN + instrucciones + REND + PTCOMA //12
            | RFUNCTION + IDENTIFICADOR + DOSPUNTOS + tipo + PTCOMA + instrucciones + RBEGIN + instrucciones + REND + PTCOMA //10
            | RPROCEDURE + IDENTIFICADOR + PARA + listadeclaracion + PARC + PTCOMA + instrucciones + RBEGIN + instrucciones + REND + PTCOMA //11
            | RPROCEDURE + IDENTIFICADOR + PARA + PARC + PTCOMA + instrucciones + RBEGIN + instrucciones + REND + PTCOMA //10
            | RPROCEDURE + IDENTIFICADOR + PTCOMA + instrucciones + RBEGIN + instrucciones + REND + PTCOMA //8
            ;

        array.Rule
            = IDENTIFICADOR + CORA + expresion + CORC
            ;

        call.Rule
            = IDENTIFICADOR + PARA + parametros + PARC
            | IDENTIFICADOR + PARA + PARC            
            | IDENTIFICADOR
            ;

        sentenciaRepeat.Rule
            = RREPEAT + instrucciones + RUNTIL + condicion;

        sentenciaFor.Rule
            = RFOR + IDENTIFICADOR + ASG + expresion + RTO + expresion + RDO + instruccionesBloque //8
            | RFOR + IDENTIFICADOR + ASG + expresion + RDOWN + expresion + RDO + instruccionesBloque; //8

        sentenciaWhile.Rule
            = RWHILE + condicion + RDO + instruccionesBloque;

        sentenciaCase.Rule
            = RCASE + condicion + ROF + caseValue + REND;

        caseValue.Rule
            = caseValue + caseList + DOSPUNTOS + instruccionesBloque + PTCOMA
            | caseValue + RELSE + instruccionesBloque + PTCOMA
            | caseList + DOSPUNTOS + instruccionesBloque + PTCOMA;

        caseList.Rule
            = caseList + COMA + expresion
            | expresion;

        sentenciaIf.Rule
            = RIF + condicion + RTHEN + instruccionesBloque //4
            | RIF + condicion + RTHEN + instruccionesBloque + RELSE + instruccionesBloque; //6
        //    | RIF + condicionIf + RTHEN + instruccionesIf + RELSE + sentenciaIf;

        condicion.Rule
            = PARA + expresion + PARC
            | expresion;

        instruccionesBloque.Rule
            = RBEGIN + instrucciones + REND
            | ternaria;

        ternaria.Rule
            = RWRITE + PARA + writelist + PARC  
            | RWRITELN + PARA + writelist + PARC 
            | REXIT + PARA + expresion + PARC
            | objeto + ASG + expresion 
            | array + ASG + expresion
            | IDENTIFICADOR + ASG + expresion 
            | RGRAFICAR + PARA + PARC
            | RBREAK
            | RCONTINUE
            | sentenciaIf
            | sentenciaCase
            | sentenciaWhile
            | sentenciaFor 
            | sentenciaRepeat
            | call
            ;

        objeto.Rule
            = objeto + PUNTO + IDENTIFICADOR
            | IDENTIFICADOR;

        writelist.Rule
            = writelist + COMA + expresion
            | expresion;

        type.Rule
            = RARRAY + CORA + expresion + PUNTO + PUNTO + expresion + CORC + ROF + tipo //9
            | ROBJECTO + objectvarzone + REND; //3

        objectvarzone.Rule
            = objectvarzone + RVAR + listadeclaracion//4
            | RVAR + listadeclaracion; //3
 
        listaconstante.Rule
            = listaconstante + IDENTIFICADOR + EQ + expresion + PTCOMA//5
            | listaconstante + IDENTIFICADOR + DOSPUNTOS + tipo + EQ + expresion + PTCOMA //7
            | IDENTIFICADOR + DOSPUNTOS + tipo + EQ + expresion + PTCOMA //6
            | IDENTIFICADOR + EQ + expresion + PTCOMA //4
            ; 
        
        listadeclaracion.Rule
            = listadeclaracion + declaracion + DOSPUNTOS + tipo + declaracionsufe//5
            | listadeclaracion + RVAR + declaracion + DOSPUNTOS + tipo + declaracionsufe //6
            | declaracion + DOSPUNTOS + tipo + declaracionsufe//4
            | RVAR + declaracion + DOSPUNTOS + tipo + declaracionsufe //5
            ;

        declaracionsufe.Rule
            = EQ + expresion + PTCOMA
            | PTCOMA
            | Empty
            ;

        declaracion.Rule
            = declaracion + COMA + IDENTIFICADOR
            | IDENTIFICADOR;

        tipo.Rule
            = RBOOLEAN
            | RREAL
            | RINT
            | RSTRING
            | IDENTIFICADOR;

        parametros.Rule
            = parametros + COMA + expresion
            | expresion;

        expresion.Rule
            = SUB + expresion
            | expresion + ADD + expresion
            | expresion + SUB + expresion
            | expresion + MUL + expresion
            | expresion + DIV + expresion
            | expresion + MOD + expresion
            | NOT + expresion
            | expresion + AND + expresion
            | expresion + OR + expresion
            | expresion + L + expresion
            | expresion + LE + expresion
            | expresion + GE + expresion
            | expresion + G + expresion
            | expresion + EQ + expresion
            | expresion + NEQ + expresion
            | TRUE
            | FALSE
            | NUMERO
            | STRING
            | objeto
            //| IDENTIFICADOR
            | array
            | call
            | PARA + expresion + PARC;
        #endregion

        #region PREFERENCIAS
        this.Root = inicio;

        #endregion 
    }
    
}