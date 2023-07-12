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
        RegisterOperators(4, Associativity.Right, NOT);
     

        
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
        // raiz
        NonTerminal inicio = new NonTerminal("inicio");

        // para expresiones
        NonTerminal expresion = new NonTerminal("expresion");
        NonTerminal expresiones_logicas = new NonTerminal("expresiones_logicas");
        NonTerminal expresiones_aritmeticas = new NonTerminal("expresiones_aritmeticas");
        NonTerminal expresiones_primitivas = new NonTerminal("expresiones_primitivas");
        NonTerminal expresiones_accesibles = new NonTerminal("expresiones_accesibles");
        NonTerminal expresiones_relacionales = new NonTerminal("expresiones_relacionales");
        NonTerminal expresion_list = new NonTerminal("expresion_list");
        
        // para objetos
        NonTerminal acceso_struct = new NonTerminal("acceso_struct");
        NonTerminal acceso_struct_property = new NonTerminal("acceso_struct");

        // para arrays
        NonTerminal acceso_array = new NonTerminal("acceso_array");
        NonTerminal acceso_array_dimensiones = new NonTerminal("acceso_array_dimensiones");
        NonTerminal array_dimensiones = new NonTerminal("array_dimensiones");

        // para llamadas a funciones
        NonTerminal call_funcion = new NonTerminal("call_funcion");
        NonTerminal call_funcion_parametros = new NonTerminal("call_funcion_parametros");

        // Para asignaciones
        NonTerminal asignaciones = new NonTerminal("asignaciones");

        // Para declarar variables
        NonTerminal declaracion = new NonTerminal("declaracion");
        NonTerminal declaracion_tipica = new NonTerminal("declaracion_tipica");
        NonTerminal declaracion_definida = new NonTerminal("declaracion_definida");
        NonTerminal declaracion_definida_objeto = new NonTerminal("declaracion_definida_objeto");
        NonTerminal declaracion_definida_range = new NonTerminal("declaracion_definida_range");
        NonTerminal declaracion_variables = new NonTerminal("declaracion_variables");
        NonTerminal declaracion_variables_identificadores = new NonTerminal("declaracion_variables_identificadores");
        NonTerminal declaracion_variables_asignacion = new NonTerminal("declaracion_variables_asignacion");

        // Para establece tipos
        NonTerminal tipos = new NonTerminal("tipos");

        // Para instrucciones
        NonTerminal instrucciones_ejecucion = new NonTerminal("instrucciones_ejecucion");
        NonTerminal instrucciones_declaracion = new NonTerminal("instrucciones_declaracion");

        NonTerminal instrucciones_bloque_ejecucion = new NonTerminal("instrucciones_bloque_ejecutivo");
        NonTerminal instrucciones_bloque_declaracion = new NonTerminal("instrucciones_bloque_declaracion");
        NonTerminal instrucciones_grupo = new NonTerminal("instrucciones_grupo");
        NonTerminal instrucciones_grupo_semi = new NonTerminal("instrucciones_grupo_semi");

        // Para sentencia if
        NonTerminal sentencia_if = new NonTerminal("sentencia_if");

        // Para sentencia case
        NonTerminal sentencia_case = new NonTerminal("sentencia_case");
        NonTerminal sentencia_case_value = new NonTerminal("sentencia_case_value");

        // Para sentencias de repeticion
        NonTerminal sentencia_while = new NonTerminal("sentencia_while");
        NonTerminal sentencia_for = new NonTerminal("sentencia_for");
        NonTerminal sentencia_repeat = new NonTerminal("sentencia_repeat");

        // Para sentencias de control
        NonTerminal sentencia_control = new NonTerminal("sentencia_control");

        // Para sentencias definidas
        NonTerminal sentencia_write = new NonTerminal("sentencia_write");
        NonTerminal sentencia_graficar = new NonTerminal("sentencia_graficar");
        NonTerminal sentencia_exit = new NonTerminal("sentencia_exit");

        // Para funciones y procedimientos
        NonTerminal metodo = new NonTerminal("metodo");
        NonTerminal metodo_parametros = new NonTerminal("metodo_parametros");
        NonTerminal metodo_parametros_unidad = new NonTerminal("metodo_parametros_unidad");

        inicio.Rule 
            = RPROGRAM + IDENTIFICADOR + PTCOMA + instrucciones_bloque_declaracion + instrucciones_bloque_ejecucion + PUNTO
            ;
        
        metodo.Rule
            = RFUNCTION + IDENTIFICADOR + PARA + metodo_parametros + PARC + DOSPUNTOS + tipos + PTCOMA + instrucciones_bloque_declaracion + instrucciones_bloque_ejecucion + PTCOMA
            | RPROCEDURE + IDENTIFICADOR + PARA + metodo_parametros + PARC + PTCOMA + instrucciones_bloque_declaracion + instrucciones_bloque_ejecucion + PTCOMA
            ;

        metodo_parametros.Rule
            = MakeStarRule(metodo_parametros, PTCOMA, metodo_parametros_unidad)
            | Empty
            ;

        metodo_parametros_unidad.Rule
            = declaracion_variables_identificadores + DOSPUNTOS + tipos
            | RVAR + declaracion_variables_identificadores + DOSPUNTOS + tipos
            ;

        sentencia_exit.Rule
            = REXIT + PARA + expresion + PARC
            | REXIT + PARA + PARC
            ;

        sentencia_graficar.Rule
            = RGRAFICAR + PARA + PARA
            ;

        sentencia_write.Rule
            = RWRITE + PARA + expresion_list + PARC 
            | RWRITELN + PARA + expresion_list + PARC
            ;

        sentencia_control.Rule
            = RBREAK
            | RCONTINUE
            ;

        sentencia_case.Rule
            = RCASE + expresion + ROF + sentencia_case_value + REND
            ;

        sentencia_case_value.Rule
            = MakePlusRule(sentencia_case_value, sentencia_case_value)
            | expresion_list + DOSPUNTOS + instrucciones_bloque_ejecucion + PTCOMA
            | RELSE + instrucciones_bloque_ejecucion + PTCOMA
            ;

        sentencia_for.Rule
            = RFOR + asignaciones + RTO + expresion + RDO + instrucciones_bloque_ejecucion
            | RFOR + asignaciones + RDOWN + expresion + RDO + instrucciones_bloque_ejecucion
            ; 

        sentencia_repeat.Rule
            = RREPEAT + instrucciones_grupo + RUNTIL + expresion;

        sentencia_while.Rule
            = RWHILE + expresion + RDO + instrucciones_bloque_ejecucion
            ;

        sentencia_if.Rule
            = RIF + expresion + RTHEN + instrucciones_bloque_ejecucion 
            | RIF + expresion + RTHEN + instrucciones_bloque_ejecucion + RELSE + instrucciones_bloque_ejecucion
            ;

        expresion_list.Rule
            = MakeStarRule(expresion_list, COMA, expresion)
            | Empty
            ;

        instrucciones_bloque_ejecucion.Rule
            = RBEGIN + instrucciones_grupo + REND
            | instrucciones_ejecucion
            | Empty
            ;

        instrucciones_bloque_declaracion.Rule
            = MakePlusRule(instrucciones_bloque_declaracion, instrucciones_declaracion)
            | Empty
            ;

        instrucciones_grupo.Rule
            = MakePlusRule(instrucciones_grupo, instrucciones_grupo_semi);
            ;
        
        instrucciones_grupo_semi.Rule
            = instrucciones_ejecucion + PTCOMA
            ;

        instrucciones_declaracion.Rule
            = declaracion
            | metodo
            ;
        
        instrucciones_ejecucion.Rule
            = asignaciones
            | sentencia_if
            | sentencia_for
            | sentencia_repeat
            | sentencia_while
            | sentencia_case
            | sentencia_control
            | sentencia_write   
            | sentencia_exit
            | sentencia_graficar
            | call_funcion
            ;

        declaracion.Rule
            = declaracion_definida //PUNTO COMA
            | declaracion_tipica   //SIN PUNTO COMA
            ;

        declaracion_definida.Rule
            = RTYPE + IDENTIFICADOR + EQ + ROBJECTO + declaracion_definida_objeto + REND + PTCOMA
            | RTYPE + IDENTIFICADOR + EQ + RARRAY + CORA + declaracion_definida_range + CORC + ROF + tipos + PTCOMA
            ;

        declaracion_definida_objeto.Rule
            = MakePlusRule(declaracion_definida_objeto, declaracion_tipica)
            ;

        declaracion_definida_range.Rule
            = MakeStarRule(declaracion_definida_range, COMA, array_dimensiones)
            ;

        declaracion_tipica.Rule
            = RVAR + declaracion_variables
            | RCONST + declaracion_variables
            ;

        declaracion_variables.Rule
            = MakePlusRule(declaracion_variables, declaracion_variables_asignacion)
            ;
        
        declaracion_variables_asignacion.Rule
            = declaracion_variables_identificadores + DOSPUNTOS + RARRAY + CORA + declaracion_definida_range + CORC + ROF + tipos + PTCOMA
            | declaracion_variables_identificadores + DOSPUNTOS + tipos + EQ + expresion + PTCOMA
            | declaracion_variables_identificadores + DOSPUNTOS + tipos + PTCOMA
            | declaracion_variables_identificadores + EQ + expresion + PTCOMA
            ;
        
        declaracion_variables_identificadores.Rule
            = MakeStarRule(declaracion_variables_identificadores, COMA, IDENTIFICADOR)
            ;

        array_dimensiones.Rule
            = expresion + PUNTO + PUNTO + expresion
            ;

        tipos.Rule
            = RBOOLEAN
            | RREAL
            | RINT
            | RSTRING
            | IDENTIFICADOR;

        asignaciones.Rule
            = expresiones_accesibles + ASG + expresion // PUNTO COMA
            ;

        call_funcion.Rule
            = IDENTIFICADOR + PARA + expresion_list + PARC
            ;

        acceso_struct.Rule
            = expresiones_accesibles + PUNTO + IDENTIFICADOR
            ;

        acceso_array.Rule
            = expresiones_accesibles + CORA +  acceso_array_dimensiones + CORC
            ;
        acceso_array_dimensiones.Rule 
            = MakeStarRule(acceso_array_dimensiones, COMA, expresion)
            ;        
        expresion.Rule
            = expresiones_aritmeticas
            | expresiones_logicas
            | expresiones_relacionales
            | expresiones_primitivas
            | expresiones_accesibles
            | PARA + expresion + PARC
            | call_funcion
            ;
        expresiones_aritmeticas.Rule
            = SUB + expresion
            | expresion + ADD + expresion
            | expresion + SUB + expresion
            | expresion + MUL + expresion
            | expresion + DIV + expresion
            | expresion + MOD + expresion
            ;
        expresiones_logicas.Rule
            = NOT + expresion
            | expresion + AND + expresion
            | expresion + OR + expresion
            ;
        expresiones_relacionales.Rule
            = expresion + L + expresion
            | expresion + LE + expresion
            | expresion + GE + expresion
            | expresion + G + expresion
            | expresion + EQ + expresion
            | expresion + NEQ + expresion
            ;
        expresiones_primitivas.Rule
            = TRUE
            | FALSE
            | NUMERO
            | STRING
            ;
        expresiones_accesibles.Rule
            = IDENTIFICADOR
            | acceso_struct
            | acceso_array
            ;

        #endregion

        #region PREFERENCIAS
        this.Root = inicio;

        #endregion 
    }
    
}