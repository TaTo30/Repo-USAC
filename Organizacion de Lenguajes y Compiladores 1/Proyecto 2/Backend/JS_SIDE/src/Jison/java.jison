%lex

%options case-insensitive

%%
\s+											// se ignoran espacios en blanco
"//".*										// comentario simple línea
[/][*][^*]*[*]+([^/*][^*]*[*]+)*[/]			// comentario multiple líneas


"public"            { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RPUBLIC';}
"class"             { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RCLASS';}
"interface"         { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RINTERFACE';}
"for"               { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RFOR';}
"while"             { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RWHILE';}
"do"                { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RDO';}
"if"                { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RIF';}
"else"              { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RELSE';}
"break"             { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RBREAK';}
"continue"          { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RCONTINUE';}
"return"            { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RRETURN';}
"int"               { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RINT';}
"void"              { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RVOID';}
"boolean"           { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RBOOLEAN';}
"true"              { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RTRUE';}
"false"             { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RFALSE';}
"double"            { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RDOUBLE';}
"string"            { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RSTRING';}
"char"              { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RCHAR';}
"system"            { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RSYSTEM';}
"out"               { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'ROUT';}
"println"           { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RPRINTLN';}
"print"             { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RPRINT';}
"static"            { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RSTATIC';}
"main"              { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'PALABRA RESERVADA', yylloc.first_line, yylloc.first_column);return 'RMAIN';}

">="                { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO LOGICO', yylloc.first_line, yylloc.first_column);return 'MAYORIGUALQUE';}
"<="                { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO LOGICO', yylloc.first_line, yylloc.first_column);return 'MENORIGUALQUE';}
"=="                { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO LOGICO', yylloc.first_line, yylloc.first_column);return 'IGUALIGUAL';}
"!="                { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO LOGICO', yylloc.first_line, yylloc.first_column);return 'DIFERENTE'}
"||"                { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO LOGICO', yylloc.first_line, yylloc.first_column);return 'OR';}
"&&"                { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO LOGICO', yylloc.first_line, yylloc.first_column);return 'AND';}
"!"                 { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO LOGICO', yylloc.first_line, yylloc.first_column);return 'NOT';}
"^"                 { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO LOGICO', yylloc.first_line, yylloc.first_column);return 'XOR';}
"<"                 { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO LOGICO', yylloc.first_line, yylloc.first_column);return 'MENORQUE';}
">"                 { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO LOGICO', yylloc.first_line, yylloc.first_column);return 'MAYORQUE';}

"++"                { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO ARITMETICO', yylloc.first_line, yylloc.first_column);return 'ADICION';}
"--"                { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO ARITMETICO', yylloc.first_line, yylloc.first_column);return 'SUBSTRACCION';}
"+"                 { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO ARITMETICO', yylloc.first_line, yylloc.first_column);return 'MAS';}
"-"                 { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO ARITMETICO', yylloc.first_line, yylloc.first_column);return 'MENOS';}
"*"                 { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO ARITMETICO', yylloc.first_line, yylloc.first_column);return 'MULTIPLICACION';}
"/"                 { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO ARITMETICO', yylloc.first_line, yylloc.first_column);return 'DIVISION';}


"."                 { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO', yylloc.first_line, yylloc.first_column);return 'PUNTO';}
";"                 { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO', yylloc.first_line, yylloc.first_column);return 'PUNTOCOMA';}
","                 { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO', yylloc.first_line, yylloc.first_column);return 'COMA';}
"="                 { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO', yylloc.first_line, yylloc.first_column);return 'IGUAL';}
"("                 { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO', yylloc.first_line, yylloc.first_column);return 'PARABRIR';}
")"                 { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO', yylloc.first_line, yylloc.first_column);return 'PARCERRAR';}
"{"                 { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO', yylloc.first_line, yylloc.first_column);return 'LLAVEABRIR';}
"}"                 { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO', yylloc.first_line, yylloc.first_column);return 'LLAVECERRAR';}
"["                 { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO', yylloc.first_line, yylloc.first_column);return 'CORABRIR';}
"]"                 { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'SIGNO', yylloc.first_line, yylloc.first_column);return 'CORCERRAR';}


\"[^\"]*\"	                                                    { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'CADENA', yylloc.first_line, yylloc.first_column);return 'CADENA';}
"'"[^']"'"				                                        { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'CARACTER', yylloc.first_line, yylloc.first_column);return 'CARACTER';}
[0-9]+("."[0-9]+)\b  											{ var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'DECIMAL', yylloc.first_line, yylloc.first_column);return 'DECIMAL';}
[0-9]+\b														{ var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'ENTERO', yylloc.first_line, yylloc.first_column);return 'ENTERO';}
([a-zA-Z])[a-zA-Z0-9_]*	                                        { var agregarToken = Tokens.AgregarToken;agregarToken(yytext, 'IDENTIFICADOR', yylloc.first_line, yylloc.first_column);return 'IDENTIFICADOR';}

<<EOF>>             return 'EOF';

.                   {
                        console.log('Este es un error léxico: ' + yytext + ', en la linea: ' + yylloc.first_line + ', en la columna: ' + yylloc.first_column);
                        var agregarError = ErroresINT.AgregarError;
                        agregarError(yytext, yylloc.first_line, yylloc.first_column, 1);
                    }
/lex

%{
    const ErroresINT = require('../Analizador/Errores.js');
    const Tokens = require('../Analizador/Tokens.js');
    

%}

/*PRECEDENCIA*/
%left 'OR'
%left 'AND'
%left 'XOR'
%left 'IGUALIGUAL' 'DIFERENTE'
%left 'MAYORQUE' 'MENORQUE' 'MENORIGUALQUE' 'MAYORIGUALQUE'
%left 'MAS' 'MENOS'
%left 'MULTIPLICACION' 'DIVISION'
%left 'UMENOS'
%right 'UNOT' 
%right 'ADICION' 'SUBSTRACCION'

%start INICIO

%%

INICIO
    : INSTRUCCIONES EOF { 
        return $1;
    }
;

INSTRUCCIONES
    : INSTRUCCIONES INSTRUCCION     { $$ = `${$1}${$2}`} //2
    | INSTRUCCION                   { $$ = `${$1}`}  //3     
;

INSTRUCCION
    : error PUNTOCOMA               
        {
            console.log('Error sintáctico:'+ $1 + 'en la linea: ' +this._$.first_line + ', en la columna: ' + this._$.first_column+ 'Se esperaba: '+ yy.parser.hash.expected.join(",")); 
            var agregarError = ErroresINT.AgregarError;
            agregarError(yy.parser.hash.expected.join(","), this._$.first_line, this._$.first_column, 2);
        }
    | error LLAVEABRIR
        {
            console.log('Error sintáctico:'+ $1 + 'en la linea: ' +this._$.first_line + ', en la columna: ' + this._$.first_column+ 'Se esperaba: '+ yy.parser.hash.expected.join(",")); 
            var agregarError = ErroresINT.AgregarError;
            agregarError(yy.parser.hash.expected.join(","), this._$.first_line, this._$.first_column, 2);
        }
    | error EOF
        {
            console.log('Error sintáctico:'+ $1 + 'en la linea: ' +this._$.first_line + ', en la columna: ' + this._$.first_column+ 'Se esperaba: '+ yy.parser.hash.expected.join(",")); 
            var agregarError = ErroresINT.AgregarError;
            agregarError(yy.parser.hash.expected.join(","), this._$.first_line, this._$.first_column, 2);
        }
    | SENTENCIAFOR                  { $$ = `${$1}`}//7
    | SENTENCIAWHILE                { $$ = `${$1}`}//8
    | SENTENCIADOWHILE PUNTOCOMA    { $$ = `${$1};\n`}//9
    | SENTENCIAIF                   { $$ = `${$1}`}//10
    | ASIGNACION PUNTOCOMA          { $$ = `${$1};\n`}//11
    | DECLARACION PUNTOCOMA         { $$ = `${$1};\n`}//12
    | PRINT PUNTOCOMA               { $$ = `${$1};\n`}//13
    | CALL PUNTOCOMA                { $$ = `${$1};\n`}//14
    | FUNCIONES                     { $$ = `${$1}`}//15
    | CLASES                        { $$ = `${$1}`}//16
    | INTERFACES                    { $$ = `${$1}`}//17
    | SENTENCIAS PUNTOCOMA          { $$ = `${$1};\n`}//18
    | EXP PUNTOCOMA                 { $$ = `${$1};\n`}//19
;

BLOQUESENTENCIAS
    : LLAVEABRIR INSTRUCCIONES LLAVECERRAR  { $$ = `{\n${$2}}\n`}
    | LLAVEABRIR LLAVECERRAR                { $$ = '{\n}\n'}
;


SENTENCIADOWHILE //22
    : RDO BLOQUESENTENCIAS RWHILE PARABRIR EXP PARCERRAR { $$ = `do ${$2} while (${$5})`;}
;

SENTENCIAWHILE//23
    : RWHILE PARABRIR EXP PARCERRAR BLOQUESENTENCIAS { $$ = `while (${$3}) ${$5}`;}
;

SENTENCIAFOR
    : RFOR PARABRIR DECLARACION PUNTOCOMA EXP PUNTOCOMA EXP PARCERRAR BLOQUESENTENCIAS  { $$ = `for (${$3}; ${$5}; ${$7}) ${$9}`;}
;

SENTENCIAIF//27-26-25
    : RIF PARABRIR EXP PARCERRAR BLOQUESENTENCIAS                           { $$ = `if(${$3}) ${$5}`;}
    | RIF PARABRIR EXP PARCERRAR BLOQUESENTENCIAS RELSE BLOQUESENTENCIAS    { $$ = `if(${$3}) ${$5} else ${$7}`;}
    | RIF PARABRIR EXP PARCERRAR BLOQUESENTENCIAS RELSE SENTENCIAIF         { $$ = `if(${$3}) ${$5} else ${$7}`;}
;

INTERFACES
    : RPUBLIC RINTERFACE IDENTIFICADOR BLOQUESENTENCIAS { $$ = '';}//28
;

CLASES
    : RPUBLIC RCLASS IDENTIFICADOR BLOQUESENTENCIAS                                                         { $$ = `class ${$3} ${$4}`;}
    | RPUBLIC RSTATIC RVOID RMAIN PARABRIR RSTRING CORABRIR CORCERRAR IDENTIFICADOR PARCERRAR BLOQUESENTENCIAS  { $$ = `class main ${$11}`;}
;

FUNCIONES
    : RPUBLIC TIPO IDENTIFICADOR PARABRIR LISTAPARAMETROSDECLARADOS PARCERRAR                   { $$ = `function ${$3}(${$5})`;}
    | RPUBLIC TIPO IDENTIFICADOR PARABRIR LISTAPARAMETROSDECLARADOS PARCERRAR BLOQUESENTENCIAS  { $$ = `function ${$3}(${$5}) ${$7}`;}
;

CALL
    : IDENTIFICADOR PARABRIR LISTAPARAMETROSENVIADOS PARCERRAR   { $$ = `${$1}(${$3})`;} //33
;

PRINT
    : RSYSTEM PUNTO ROUT PUNTO RPRINT PARABRIR EXP PARCERRAR      { $$ = `Console.log(${$7})`;}
    | RSYSTEM PUNTO ROUT PUNTO RPRINTLN PARABRIR EXP PARCERRAR    { $$ = `Console.log(${$7})`;}
;


LISTAPARAMETROSENVIADOS
    : LISTAPARAMETROSENVIADOS COMA EXP  { $$ = `${$1}, ${$3}`;}
    | EXP                               { $$ = `${$1}`;}//37
    |                                   { $$ = '';}
;

LISTAPARAMETROSDECLARADOS
    : LISTAPARAMETROSDECLARADOS COMA TIPO IDENTIFICADOR     { $$ = `${$1}, ${$4}`;}
    | TIPO IDENTIFICADOR                                    { $$ = `${$2}`;}
    |                                                       { $$ = '';}//41
;

ASIGNACION
    : IDENTIFICADOR IGUAL EXP   { $$ = `${$1} = ${$3}`;}//42
;

SENTENCIAS
    : RBREAK        { $$ = 'break';}//43
    | RCONTINUE     { $$ = 'continue';}
    | RRETURN       { $$ = 'return';}
    | RRETURN EXP   { $$ = `return ${$2}`;}
;

DECLARACION
    : TIPO LISTADECLARACION { $$ = `${$1}${$2}`;}//47
;


LISTADECLARACION
    : LISTADECLARACION COMA IDENTIFICADOR IGUAL EXP     { $$ = `${$1}, ${$3} = ${$5}`;}//48
    | LISTADECLARACION COMA IDENTIFICADOR               { $$ = `${$1}, ${$3}`;}
    | IDENTIFICADOR IGUAL EXP                           { $$ = `${$1} = ${$3}`;}
    | IDENTIFICADOR                                     { $$ = `${$1}`;}
;

TIPO
    : RINT      { $$ = 'var ';}//52
    | RSTRING   { $$ = 'var ';}
    | RBOOLEAN  { $$ = 'var ';}
    | RDOUBLE   { $$ = 'var ';}
    | RCHAR     { $$ = 'var ';}
    | RVOID     { $$ = 'var ';}
;


EXP   
    : EXP MAS EXP               { $$ = `${$1} + ${$3}`;}//58
    | EXP MENOS EXP             { $$ = `${$1} - ${$3}`;}
    | EXP MULTIPLICACION EXP    { $$ = `${$1} * ${$3}`;}
    | EXP DIVISION EXP          { $$ = `${$1} / ${$3}`;}
    | MENOS EXP %prec UMENOS    { $$ = `-${$2}`;}
    | IDENTIFICADOR             { $$ = $1;}
    | CADENA                    { $$ = `${$1}`;}//64
    | CARACTER                  { $$ = `${$1}`;}
    | RTRUE                     { $$ = `true`;}
    | RFALSE                    { $$ = `false`;}
    | ENTERO                    { $$ = `${$1}`;}//68
    | DECIMAL                   { $$ = `${$1}`;}    
    | EXP MAYORQUE EXP          { $$ = `${$1} > ${$3}`;}//70
    | EXP MENORQUE EXP          { $$ = `${$1} < ${$3}`;}
    | EXP MAYORIGUALQUE EXP     { $$ = `${$1} >= ${$3}`;}
    | EXP MENORIGUALQUE EXP     { $$ = `${$1} <= ${$3}`;}
    | EXP IGUALIGUAL EXP        { $$ = `${$1} == ${$3}`;}//74
    | EXP DIFERENTE EXP         { $$ = `${$1} != ${$3}`;}
    | EXP AND EXP               { $$ = `${$1} && ${$3}`;}
    | EXP OR EXP                { $$ = `${$1} || ${$3}`;}
    | EXP XOR EXP               { $$ = `${$1} ^ ${$3}`;}  //78
    | NOT EXP %prec UNOT        { $$ = `!${$2}`;}    
    | PARABRIR EXP PARCERRAR    { $$ = `(${$2})`;}
    | EXP ADICION               { $$ = `${$1}++`;}
    | EXP SUBSTRACCION          { $$ = `${$1}--`;}//82
;
