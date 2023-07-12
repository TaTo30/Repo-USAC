# Manual Tecnico

## Cliente Golang
Para la realizacion del cliente de la aplicacion se utilizo golang con las siguientes librerias:

#### Go: 1.15.2
- bytes
- encoding/json
- fmt
- html/template
- io/ioutil
- log
- net/http
- os
- os/exec

## Servidor de JavaScript y Python
Para la realizacion del servidor de JavaScript y python se hizo de NodeJS con las siguientes dependencias:  

 #### NodeJS: 14.12.0
 - body-parser: 1.19.0
 - cors: 2.8.5
 - express: 4.17.1
 - jison: 0.4.18

**nota:** Ambos servidores funcionan bajo "localhost" y los siguientes puertos para cada uno:

- JavaScript: Puerto **3000**
- Python: Puerto **3030**
 
# Analisis Lexico
#### Expresiones Regulares
 - Comentario Unilinea: `//C*`
 - Comentario Multilinea: `/* C* */`
 - Cadenas: `"C*"`
 - Caracteres: `'C'`
 - Numeros: `D+(.D+)?`
#### Palabras Reservadas
- public
- class
- interface
- for
- while
- do
- if
- else
- break
- continue
- return
- int
- boolean
- true
- false
- double
- String
- char
- System
- out
- println
- print
- static

#### Signos 
|Logicos| Aritmeticos | Otros |
|--|--|--|
| \|\| | + | ;
| && | -| ,
| ! | *| =
| < | / | (
| > | ++ | )
| <= | -- | {
| >= | -(neg)| }
| == | | [
| != | | ]



# Analisis Sintáctico
## Gramatica Utilizada para el Analizador JavaScript (LR)
EXP
: EXP && EXP
| EXP || EXP
| EXP ^ EXP
| !EXP
| EXP > EXP
| EXP < EXP
| EXP >= EXP
| EXP <= EXP
| EXP == EXP
| EXP != EXP
| EXP + EXP
| EXP - EXP
| EXP * EXP
| EXP / EXP
| EXP++
| EXP
| -EXP
| (EXP)
| IDENTIFICADOR
| CADENA
| CARACTER
| RTRUE
| RFALSE
| ENTERO
| DECIMAL

TIPO
: RINT
| RSTRING
| RBOOLEAN
| RDOUBLE
| RCHAR
| RVOID

DECLARACION
: TIPO DECLARACION
|  IDENTIFICADOR DECLARACION
|  IDENTIFICADOR = EXP DECLARACION
| ,  IDENTIFICADOR DECLARACION
| , IDENTIFICADOR = EXP DECLARACION
| , IDENTIFICADOR
| , IDENTIFICADOR = EXP
| IDENTIFICADOR
| IDENTIFICADOR = EXP

SENTENCIAS
: RBREAK
| RCONTINUE
| RRETURN
| RRETURN EXP

ASIGNACION
: IDENTIFICADOR = EXP

PARAMETROS:
: EXP  PARAMETROS
| , EXP  PARAMETROS
| , EXP
| EXP
| TIPO IDENTIFICADOR  PARAMETROS
| , TIPO IDENTIFICADOR  PARAMETROS
| , TIPO IDENTIFICADOR
| TIPO IDENTIFICADOR

PRINT:
: RSYSTEM . ROUT . RPRINT ( EXP )
| RSYSTEM . ROUT . RPRINTLN ( EXP )

CALL
: IDENTIFICADOR ( PARAMETROS )

FUNCIONES
: RPUBLIC TIPO IDENTIFICADOR ( PARAMETROS )
METODOS
| RPUBLIC TIPO IDENTIFICADOR ( PARAMETROS ) { INSTRUCCIONES }

CLASES

: RPUBLIC RCLASS IDENTIFICADOR { INSTRUCCIONES }
: RPUBLIC RSTATIC TIPO RMAIN ( TIPO[] IDENTIFICADOR) { INSTRUC  CIONES }

INTERFACES
: RPUBLIC RINTERFACE IDENTIFICADOR { INSTRUCCIONES }

SENTENCIAIF
: RIF ( EXP ) { INSTRUCCIONES }
| RIF ( EXP ) { INSTRUCCIONES } RELSE { INSTRUCCIONES }
| RIF ( EXP ) { INSTRUCCIONES } RELSE SENTENCIAIF

SENTENCIAFOR
: RFOR  (  DECLARACION; EXP; EXP  ) {  INSTRUCCIONES }

SENTENCIAWHILE
: RWHILE ( EXP ) { INSTRUCCIONES }

SENTENCIADOWHILE
: RDO { INTRUCCIONES } RWHILE ( EXP )

INSTRUCCIONES
: INSTRUCCIONES INSTRUCCION
| INSTRUCCION

INSTRUCCION
: SENTENCIAFOR
| SENTENCIAWHILE
| SENTENCIADOWHILE PTCOMA
| SENTENCIAIF
| ASIGNACION PTCOMA
| DECLARACION PTCOMA
| PRINT PTCOMA
| CALL PTCOMA
| METODOS
| FUNCIONES PTCOMA
| CLASES
| INTERFACES
| SENTENCIAS PTCOMA

## Gramatica utilizada para el Analizador Python (LL)

#### EXPRESIONES:
\<EXPA>: EXPB <EXPAP>
\<EXPAP>: or EXPB <EXPAP>
\<EXPAP>: ε
\<EXPB>: EXPC <EXPBP>
\<EXPBP>: and EXPC <EXPBP>
\<EXPBP>: ε
\<EXPC>: EXPD <EXPCP>
\<EXPCP>: xor EXPD <EXPCP>
\<EXPCP>: ε
\<EXPD>: EXPE <EXPDP>
\<EXPDP>: == EXPE <EXPDP>
\<EXPDP>: != EXPE <EXPDP>
\<EXPDP>: ε
\<EXPE>: EXPF <EXPEP>
\<EXPEP>: > EXPF <EXPEP>
\<EXPEP>: < EXPF <EXPEP>
\<EXPEP>: <= EXPF <EXPEP>
\<EXPEP>: >= EXPF <EXPEP>
\<EXPEP>: 
\<EXPF>: EXPG <EXPFP>
\<EXPFP>: + EXPG <EXPFP>
\<EXPFP>: - EXPG <EXPFP>
\<EXPFP>: ε
\<EXPG>: EXPH <EXPGP>
\<EXPGP>: * EXPH <EXPGP>
\<EXPGP>: / EXPH <EXPGP>
\<EXPGP>: ε
\<EXPH>: -EXPA
\<EXPH>: EXPI
\<EXPI>: !EXPA
\<EXPI>: EXPJ
\<EXPJ>: EXPK <EXPJP>
\<EXPJP>: ++ <EXPJP>
\<EXPJP>: -- <EXPJP>
\<EXPJP>: ε
\<EXPK>: ( EXPA )
\<EXPK>: IDENTIFICADOR
\<EXPK>: CADENA
\<EXPK>: CARACTER
\<EXPK>: ENTERO
\<EXPK>: DECIMAL
\<EXPK>: TRUE
\<EXPK>: FALSE

#### TIPO:
\<TIPO>: int
\<TIPO>: string
\<TIPO>: boolean
\<TIPO>: double
\<TIPO>: char
\<TIPO>: void

#### DECLARACION:
\<DEC>: TIPO LISTDEC

#### LISTA DECLARCION:
\<LISTDEC>: identificador = EXPA LISTDECP
\<LISTDEC>: identfificador LISTDECP
\<LISTDECP>: , identificador = EXPA LISTDECP
\<LISTDECP>: , identificadro LISTDECP
\<LISTDECP>: ε

#### SENTENCIAS:
\<SENTENCIAS>: break
\<SENTENCIAS>: continue
\<SENTENCIAS>: return
\<SENTENCIAS>: return EXPA

#### ASIGNACION:
\<ASG>: identificador = EXPA

#### PARAMETROS DECLARADOS:
\<LPD>: TIPO identificador <LPDP>
\<LPD>: <LPDP>
\<LPDP>: , TIPO Identificador <LPDP>
\<LPDP>: ε

#### PARAMETROS ENVIADOS:
\<LPE>: EXPA <LPEP>
\<LPE>: <LPEP>
\<LPEP>: , EXPA <LPEP>
\<LPEP>: ε

#### PRINTS:
\<PRINT>: system.out.print(EXPA)
\<PRINT>: system.out.println(EXPA)

#### CALL:
\<CALL>: identificador (LPE)

#### FUNCIONES:
\<FUNC>: public TIPO identificador(LPD);
\<FUNC>: public TIPO identificador(LPD) BLOQUE

#### CLASES:
\<CLASE>: public class identificador BLOQUE
\<CLASE>: public static void main (string[] identificador) BLOQUE

#### INTERFACES:
\<INTERFAZ>: public interface identificador BLOQUE

#### IF:
\<IF>: if(EXPA) BLOQUE
\<IF>: if(EXPA) BLOQUE else BLOQUE
\<IF>: if(EXPA) BLOQUE else IF

#### FOR:
\<FOR>: for(DEC; EXPA; EXPA) BLOQUE

#### WHILE:
\<WHILE>: while(EXPA) BLOQUE

#### DOWHILE:
\<DO>: do BLOQUE while(EXPA)

#### BLOQUE:
\<BLOQUE> { INSTRUCCIONES }
\<BLOQUE> {}

#### INSTRUCCIONES:
\<INSTRUCCIONES>: <INSTRUCCION> <INSTRUCCIONESP>
\<INSTRUCCIONESP>: <INSTRUCCION> <INSTRUCCIONESP>
\<INSTRUCCIONESP>: ε

#### INSTRUCCION:
\<INSTRUCCION>: FOR
\<INSTRUCCION>: WHILE
\<INSTRUCCION>: DO;
\<INSTRUCCION>: IF
\<INSTRUCCION>: ASG;
\<INSTRUCCION>: DEC;
\<INSTRUCCION>: PRINT;
\<INSTRUCCION>: CALL;
\<INSTRUCCION>: FUNC
\<INSTRUCCION>: CLASE
\<INSTRUCCION>: INTERFAZ
\<INSTRUCCION>: SENTENCIAS;
\<INSTRUCCION>: EXPA;

#### INICIO:
\<INICIO>: INSTRUCCIONES fin
