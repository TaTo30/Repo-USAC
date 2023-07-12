const DOT = require('./dot');
const Errores = require('./Errores');
const Tokens = require('./Tokens');

var forPrint = 0;
var ifAnidados = false;

var traduccion = "";
var contadorBloques = 0;

var lineas = 0;
var columnas = 0;
var contador = 0;
let reservadas = [["RPUBLIC", "public"], ["RCLASS", "class"], ["RINTERFACE", "interface"], ["RFOR", "for"], ["RWHILE", "while"], ["RDO", "do"], ["RIF", "if"], ["RELSE", "else"], ["RBREAK", "break"], ["RCONTINUE", "continue"], ["RRETURN", "return"], ["RINT", "int"], ["RVOID", "void"], ["RBOOLEAN", "boolean"], ["RTRUE", "true"], ["RFALSE", "false"], ["RDOUBLE", "double"], ["RSTRING", "string"], ["RCHAR", "char"], ["RSYSTEM", "system"], ["ROUT", "out"], ["RPRINTLN", "println"], ["RPRINT", "print"], ["RSTATIC", "static"], ["RMAIN", "main"]];
let signos = [[">=", "MAYORIGUAL"], ["<=", "MENORIGUAL"], ["==", "IGUALIGUAL"], ["!=", "DIFERENTE"], ["||", "OR"], ["&&", "AND"], ["!", "NOT"], ["^", "XOR"], ["<", "MENOR"], [">", "MAYOR"], ["++", "ADICION"], ["--", "SUBSTRACCION"], ["+", "MAS"], ["-", "MENOS"], ["*", "MULTIPLICACION"], ["/", "DIVISION"], [".", "PUNTO"], [";", "PUNTOCOMA"], [",", "COMA"], ["=", "IGUAL"], ["{", "LLAVEABRIR"], ["}", "LLAVECERRAR"], ["(", "PARABRIR"], [")", "PARACERRAR"], ["[", "CORABRIR"], ["]", "CORCERRAR"]];

let tokens = [];
let tokens_parse = [];

var TokenActual;

function Clear() {
    forPrint = 0;
    ifAnidados = false;

    traduccion = "";
    contadorBloques = 0;
    
    lineas = 0;
    columnas = 0;
    contador = 0;

    tokens.splice(0,tokens.length);
    tokens_parse.splice(0,tokens_parse.length);
}

function parser(texto) {
    lineas++;
    columnas++;
    
    while (contador < texto.length) {
        if (texto[contador].charCodeAt() >= 48 && texto[contador].charCodeAt() <= 57) { //NUMEROS: ENTEROS O DECIMALES
            tokens.push(NUMERO(texto, texto[contador], lineas, columnas));
        }else if (texto[contador].charCodeAt() == 34) { //CADENAS
            tokens.push(CADENA(texto, texto[contador], lineas, columnas));
        }else if (texto[contador].charCodeAt() == 39){ //CARACTERES
            tokens.push(CARACTER(texto, texto[contador], lineas, columnas));
        }else if ((texto[contador].charCodeAt() >= 65 && texto[contador].charCodeAt() <= 90) || (texto[contador].charCodeAt() >= 97 && texto[contador].charCodeAt() <= 122)){
            tokens.push(IDENTIFICADOR(texto, texto[contador], lineas, columnas));
        }else if (texto[contador].charCodeAt() == 47){//COMENTARIOS: UNALINEA O MULTILINEA
            tokens.push(COMENTARIO(texto, texto[contador], lineas, columnas));
        }else if (texto[contador].charCodeAt() == 32 || texto[contador].charCodeAt() == 9 || texto[contador].charCodeAt() == 13){ //espacios y tabulaciones
            columnas++;
            contador++;
        }else if (texto[contador].charCodeAt() == 10) { //saltos de linea
            lineas++;
            contador++;
            columnas = 0;
        }else{
            tokens.push(SIGNO(texto, texto[contador], lineas, columnas));
        }
    }
    
    for (let index = 0; index < tokens.length; index++) {
        const element = tokens[index];
        if (element[1] == 'ERROR' || element[1] == 'MULTILINEA' || element[1] == 'CUNILINEA') {
            tokens.splice(index,1);
        }      
    }
    for (let index = 0; index < tokens.length; index++) {
        Tokens.AgregarToken(tokens[index][0], tokens[index][1],tokens[index][2],tokens[index][3]);        
    }
    Parse();
    return traduccion;
}

function GetTokens() {
    return tokens;
}


function NUMERO(texto, lexema, line, column) {
    contador++;
    columnas++;
    if (contador < texto.length) {
        if (texto[contador].charCodeAt() >= 48 && texto[contador].charCodeAt() <= 57) { //0-9
            return NUMERO(texto, lexema+texto[contador], line, column);
        }else if (texto[contador].charCodeAt() == 46){ // punto decimal
            return DECIMAL(texto, lexema+texto[contador], line, column);
        }else{
            return [lexema, "ENTERO", line, column];
        }
    } else {
        return [lexema, "ENTERO", line, column];
    }    
}

function DECIMAL(texto, lexema, line, column) {
    contador++;
    columnas++;
    if (contador < texto.length) {
        if (texto[contador].charCodeAt() >= 48 && texto[contador].charCodeAt() <= 57) { //0-9
            return DECIMAL(texto, lexema+texto[contador], line, column);
        } else {
            return [lexema, "DECIMAL", line, column];
        }
    } else {
        return [lexema, "DECIMAL", line, column];
    }
}

function CADENA(texto, lexema, line, column) {
    contador++;
    columnas++;
    if (contador < texto.length) {
        if (texto[contador].charCodeAt() == 34) { //la " indica el fin de cadena
            contador++;
            return [lexema+texto[contador-1], "CADENA", line, column];
        } else { // cualquier cosa puede venir dentro de una cadena
            return CADENA(texto, lexema+texto[contador], line, column);
        }
    } else {
        return [lexema, "CADENA", line, column];
    }
}

function CARACTER(texto, lexema, line, column) {
    contador++;
    columnas++;
    if (contador < texto.length) {
        if (texto[contador].charCodeAt() == 39) { //la ' indica el fin de cadena
            contador++;
            return [lexema+texto[contador-1], "CARACTER", line, column];
        } else { // cualquier cosa puede venir dentro de una cadena
            return CARACTER(texto, lexema+texto[contador], line, column);
        }
    } else {
        return [lexema, "CARACTER", line, column]
    }
}

function IDENTIFICADOR(texto, lexema, line, column) {
    contador++;
    columnas++;
    if (contador < texto.length) {
        if ((texto[contador].charCodeAt() >= 65 && texto[contador].charCodeAt() <= 90) || (texto[contador].charCodeAt() >= 97 && texto[contador].charCodeAt() <= 122) || (texto[contador].charCodeAt() >= 48 && texto[contador].charCodeAt() <= 57) || (texto[contador].charCodeAt() == 95)) {
            return IDENTIFICADOR(texto, lexema+texto[contador], line, column);
        } else { // DE LO CONTRARIO YA NO CORRESPONDE A UN IDENTIFICADOR
            return PARSERESERVADA(lexema, line, column);
        }
    } else {
        return PARSERESERVADA(lexema, line, column);
    }
}

function COMENTARIO(texto, lexema, line, column){
    contador++;
    columnas++;
    if (contador < texto.length) {
        if (texto[contador].charCodeAt() == 42) { // multilinea
            return CMULTILINEA(texto, lexema+texto[contador], line, column, false);
        }else if (texto[contador].charCodeAt() == 47){ // unilinea
            return CUNILINEA(texto, lexema+texto[contador], line, column);
        }else{
            return [lexema, "DIVISION", line, column];
        }
    } else {
        return [lexema, "DIVISION", line, column];;
    }
}

function CMULTILINEA(texto, lexema, line, column, bandera) {
    contador++;
    columnas++;
    if (contador < texto.length) {
        if (texto[contador].charCodeAt() == 42) { //bandera para termina el comentario multilinea
            return CMULTILINEA(texto, lexema+texto[contador], line, column, true);
        }else if (texto[contador].charCodeAt() == 47) { // simbolo /
            if (bandera) { // hubo * antes
                contador++;
                return [lexema+texto[contador-1], "MULTILINEA", line, column];
            }else{ //no hubo * antes seguimos concatenando
                return CMULTILINEA(texto, lexema+texto[contador], line, column, false);
            }
        }else if (texto[contador].charCodeAt() == 10){ // Saltos de linea los seguimos tomando en cuenta
            lineas++;
            columnas = 1;
            return CMULTILINEA(texto, lexema+texto[contador], line, column, false);
        }else{ //cualquier otra cosa se mete dentro del comentario
            return CMULTILINEA(texto, lexema+texto[contador], line, column, false);
        }
    }else{
        return [lexema, "MULTILINEA", line, column];
    }
}

function CUNILINEA(texto, lexema, line, column) {
    contador++;
    columnas++;
    if (contador < texto.length) {
        if (texto[contador].charCodeAt() == 10) {
            return [lexema, "CUNILINEA", line, column];
        } else {
            return CUNILINEA(texto, lexema+texto[contador], line, column);
        }
    }else{
        return [lexema, "CUNILINEA", line, column];
    }
}

function PARSERESERVADA(lexema, line, column) {
    for (const [key, value] of reservadas) {
        if (value == lexema.toLowerCase()) {
            return [lexema, key, line, column];
        }
    }
    return [lexema, "IDENTIFICADOR", line, column];
}

function SIGNO(texto, lexema, line, column) {
    contador++;
    columnas++;
    if (contador < texto.length) {
        if (texto[contador].charCodeAt() == 61) { // =
            contador++
            return PARSESIGNO(lexema+texto[contador-1], line, column);
        }else if (texto[contador].charCodeAt() == 124){ // |
            contador++
            return PARSESIGNO(lexema+texto[contador-1], line, column);
        }else if (texto[contador].charCodeAt() == 38){ // &
            contador++
            return PARSESIGNO(lexema+texto[contador-1], line, column);
        }else if (texto[contador].charCodeAt() == 43){ // +
            contador++
            return PARSESIGNO(lexema+texto[contador-1], line, column);
        }else if (texto[contador].charCodeAt() == 45){ // -
            contador++
            return PARSESIGNO(lexema+texto[contador-1], line, column);
        }else{
            return PARSESIGNO(lexema, line, column);
        }
    } else {
        return PARSESIGNO(lexema, line, column);
    }
}

function PARSESIGNO(lexema, line, column) {
    for (const [value, key] of signos) {
        if (lexema == value) {
            return [lexema, key, line, column];
        }
    }
    Errores.AgregarError(lexema, line, column, 1);
    console.log(`El caracter ${lexema} no pertenece al lenguaje`);
    return [lexema, "ERROR", line, column];
}


function Parse() {   
    tokens.push(['ULTIMO', 'ULTIMO',0,0]);
    tokens_parse = tokens.reverse();
    TokenActual = tokens_parse.pop();
    var padre = DOT.GetNodo();
    DOT.Root();
    DOT.Push(padre);
    INSTRUCCIONES();
}

function Tabulador() {
    tab = "";
    for (let i = 0; i < contadorBloques; i++) {
        tab += "    ";        
    }
    return tab;
}

function INSTRUCCIONES() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    if (TokenActual[1] == 'LLAVECERRAR' || TokenActual[1] == 'PUNTOCOMA') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"INSTRUCCIONES"',padre);
    DOT.Push(padre);
    INSTRUCCION();
    DOT.Push(padre);
    INSTRUCCIONES();
}


function INSTRUCCION() {

    var padre = DOT.GetNodo();
    DOT.InsertRoot('"INSTRUCCION"',padre);;    
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    if (TokenActual[1] == 'RFOR') {
        DOT.Push(padre);
        FOR();
    }else if (TokenActual[1] == 'RWHILE') {
        DOT.Push(padre);
        WHILE();
    }else if (TokenActual[1] == 'RDO') {
        DOT.Push(padre);
        DO();

        Match('PUNTOCOMA', padre);
        traduccion += "\n";
    }else if (TokenActual[1] =='RIF') {
        DOT.Push(padre);
        IF();
    }else if (TokenActual[1] == 'IDENTIFICADOR'){
        let Siguiente = tokens_parse[tokens_parse.length - 1]
        if (Siguiente[1] == 'IGUAL' || Siguiente[1] == 'ADICION' || Siguiente[1] == 'SUBSTRACCION') {
            DOT.Push(padre);
            ASG();

            Match('PUNTOCOMA', padre);
            traduccion += "\n";
        }else if (Siguiente[1] == 'PARABRIR'){
            DOT.Push(padre);
            CALL();

            Match('PUNTOCOMA', padre);
            traduccion += "\n";
        }else{
            DOT.Push(padre);
            traduccion += Tabulador();
            EXPA();

            Match('PUNTOCOMA', padre);
            traduccion += "\n";
        }
    }else if (TokenActual[1] == 'RINT' || TokenActual[1] == 'RDOUBLE' || TokenActual[1] == 'RSTRING' || TokenActual[1] == 'RVOID' || TokenActual[1] == 'RBOOLEAN' || TokenActual[1] == 'RCHAR') {
        DOT.Push(padre);
        DEC();

        Match('PUNTOCOMA', padre);
        traduccion += "\n";
    }else if (TokenActual[1] == 'RSYSTEM') {
        DOT.Push(padre);
        PRINT();

        Match('PUNTOCOMA', padre);
        traduccion += "\n";
    }else if (TokenActual[1] == 'RPUBLIC') {
        Match('RPUBLIC', padre);
        if (TokenActual[1] == 'RINT' || TokenActual[1] == 'RDOUBLE' || TokenActual[1] == 'RSTRING' || TokenActual[1] == 'RVOID' || TokenActual[1] == 'RBOOLEAN' || TokenActual[1] == 'RCHAR') {
            DOT.Push(padre);
            FUNC();
        }else if (TokenActual[1] == 'RSTATIC' || TokenActual[1] == 'RCLASS') {
            DOT.Push(padre);
            CLASE();
        }else if (TokenActual[1] == 'RINTERFACE'){
            DOT.Push(padre);
            INTERFAZ();
        } else {
            //ERROR
            Error("'RINT', 'RDOUBLE', 'RSTRING', 'RCHAR', 'RVOID', 'RBOOLEAN', 'RSTATIC', 'RCLASS', 'RINTERFACE'", TokenActual[1]);
           // match("'TIPO', 'STATIC', 'INTERFACE'", padre);
        }
    }else if (TokenActual[1] == 'RRETURN' || TokenActual[1] == 'RBREAK' || TokenActual[1] == 'RCONTINUE') {
        DOT.Push(padre);
        SENTENCIAS();
        Match('PUNTOCOMA', padre);
        traduccion += "\n";
    }else if (TokenActual[1] == 'IDENTIFICADOR' || TokenActual[1] == 'CADENA' || TokenActual[1] == 'CARACTER' || TokenActual[1] == 'RTRUE' || TokenActual[1] == 'RFALSE' || TokenActual[1] == 'ENTERO' || TokenActual[1] == 'DECIMAL' || TokenActual[1] == 'PARABRIR'){
        DOT.Push(padre);
        traduccion += Tabulador();
        EXPA();

        Match('PUNTOCOMA', padre);
        traduccion += "\n";
    }else{
        //ERROR
        Error("'RFOR', 'RWHILE', 'RDO', 'RIF', 'IDENTIFICADOR', 'TIPO', 'RSYSTEM', 'RPUBLIC', 'SENTENCIAS', 'EXP'", TokenActual[1], TokenActual[2], TokenActual[3]);
    }
}

function BLOQUE() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    contadorBloques++;
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"BLOQUE"',padre);
    Match('LLAVEABRIR', padre);
    if (TokenActual[1] == "LLAVECERRAR") {
        Match('LLAVECERRAR', padre);
    } else {
        var insPat = []
        var banderazo = true;
        DOT.Push(padre); 
        //INSTRUCCIONES()
        var padre1 = DOT.GetNodo();
        DOT.InsertRoot('"INSTRUCCIONES"', padre1);      
        insPat.push(padre1);
        insPat.push(padre1);
        DOT.Push(insPat.pop());
        //INSTRUCCION()
        while (banderazo) {
            /*AQUI ESTAMOS EN INSTRUCCION*/
            var padre0 = DOT.GetNodo();
            DOT.InsertRoot('"INSTRUCCION"',padre0);      
            if (TokenActual[1] == 'RFOR') {
                DOT.Push(padre0);
                FOR();
            }else if (TokenActual[1] == 'RWHILE') {
                DOT.Push(padre0);
                WHILE();
            }else if (TokenActual[1] == 'RDO') {
                DOT.Push(padre0);
                DO();
                Match('PUNTOCOMA', padre0);
                traduccion += "\n";
            }else if (TokenActual[1] =='RIF') {
                DOT.Push(padre0);
                IF();
            }else if (TokenActual[1] == 'IDENTIFICADOR'){
                let Siguiente = tokens_parse[tokens_parse.length - 1]
                if (Siguiente[1] == 'IGUAL' || Siguiente[1] == 'ADICION' || Siguiente[1] == 'SUBSTRACCION') {
                    DOT.Push(padre0);
                    ASG();

                    Match('PUNTOCOMA', padre0);
                    traduccion += "\n";
                }else if (Siguiente[1] == 'PARABRIR'){
                    DOT.Push(padre0);
                    CALL();

                    Match('PUNTOCOMA', padre0);
                    traduccion += "\n";
                }else{
                    DOT.Push(padre);
                    traduccion += Tabulador();
                    EXPA();
        
                    Match('PUNTOCOMA', padre);
                    traduccion += "\n";
                }
            }else if (TokenActual[1] == 'RINT' || TokenActual[1] == 'RDOUBLE' || TokenActual[1] == 'RSTRING' || TokenActual[1] == 'RVOID' || TokenActual[1] == 'RBOOLEAN' || TokenActual[1] == 'RCHAR') {
                DOT.Push(padre0);
                DEC();
                Match('PUNTOCOMA', padre0);
                traduccion += "\n";
            }else if (TokenActual[1] == 'RSYSTEM') {
                DOT.Push(padre0);
                PRINT();
                Match('PUNTOCOMA', padre0);
                traduccion += "\n";
            }else if (TokenActual[1] == 'RPUBLIC') {
                Match('RPUBLIC', padre0);
                if (TokenActual[1] == 'RINT' || TokenActual[1] == 'RDOUBLE' || TokenActual[1] == 'RSTRING' || TokenActual[1] == 'RVOID' || TokenActual[1] == 'RBOOLEAN' || TokenActual[1] == 'RCHAR') {
                    DOT.Push(padre0);
                    FUNC();
                }else if (TokenActual[1] == 'RSTATIC' || TokenActual[1] == 'RCLASS') {
                    DOT.Push(padre0);
                    CLASE();
                }else if (TokenActual[1] == 'RINTERFACE'){
                    DOT.Push(padre0);
                    INTERFAZ();
                } else {
                    //ERROR
                    Error("'RINT', 'RDOUBLE', 'RSTRING', 'RCHAR', 'RVOID', 'RBOOLEAN', 'RSTATIC', 'RCLASS', 'RINTERFACE'", TokenActual[1], TokenActual[2], TokenActual[3]);
                    banderazo = false;
                }
            }else if (TokenActual[1] == 'RRETURN' || TokenActual[1] == 'RBREAK' || TokenActual[1] == 'RCONTINUE') {
                DOT.Push(padre0);
                SENTENCIAS();
                Match('PUNTOCOMA', padre0);
                traduccion += "\n";
            }else if (TokenActual[1] == 'IDENTIFICADOR' || TokenActual[1] == 'CADENA' || TokenActual[1] == 'CARACTER' || TokenActual[1] == 'RTRUE' || TokenActual[1] == 'RFALSE' || TokenActual[1] == 'ENTERO' || TokenActual[1] == 'DECIMAL' || TokenActual[1] == 'PARABRIR'){
                DOT.Push(padre0);
                traduccion += Tabulador();
                EXPA();
                Match('PUNTOCOMA', padre0);
                traduccion += "\n";
            }else if (TokenActual[1] == 'LLAVECERRAR'){
                banderazo = false;
                //continue;
            }else{
                //ERROR
                Error("'RFOR', 'RWHILE', 'RDO', 'RIF', 'IDENTIFICADOR', 'TIPO', 'RSYSTEM', 'RPUBLIC', 'SENTENCIAS', 'EXP'", TokenActual[1], TokenActual[2], TokenActual[3]);
                banderazo = false;
            }
            DOT.Push(insPat.pop());
            //INSTRUCCIONES()
            var padre2 = DOT.GetNodo();
            DOT.InsertRoot('"INSTRUCCIONES"', padre2);
            insPat.push(padre2);
            insPat.push(padre2);
            DOT.Push(insPat.pop());
        }      
        Match('LLAVECERRAR', padre);
        contadorBloques--;
    }
}

function DO() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"RDO"',padre);
    Match('RDO', padre);
    DOT.Push(padre);
    var traduccionGuardad = traduccion; //contiene todo lo anterior a do
    traduccion = ""; //traduccion vacia
    BLOQUE();
    //ahora traduccion tiene todo el bloque dentro de do
    Match('RWHILE', padre);
    Match('PARABRIR', padre);
    DOT.Push(padre);
    var bloqueDo = traduccion; //contiene el bloque dentro de do
    traduccion = "";
    EXPA();
    //ahora traduccion contiene la expresion de while
    traduccionGuardad += Tabulador() + `while ${traduccion}:\n${bloqueDo}`;
    traduccion = traduccionGuardad;
    Match('PARACERRAR', padre);
}

function WHILE() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"WHILE"',padre); 
    Match('RWHILE', padre);
    traduccion += Tabulador() +"while ";
    Match('PARABRIR', padre);
    DOT.Push(padre);
    EXPA();
    Match('PARACERRAR', padre);
    traduccion += ":\n";
    DOT.Push(padre);
    BLOQUE();
}

function FOR() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"FOR"',padre);
    Match('RFOR', padre);
    traduccion += Tabulador() +"for ";
    forPrint = 1;
    Match('PARABRIR', padre);
    DOT.Push(padre);
    DEC();
    Match('PUNTOCOMA', padre);
    traduccion +=   ", ";
    forPrint = 1;
    DOT.Push(padre);
    EXPA();
    traduccion +=   "):\n";
    Match('PUNTOCOMA', padre);
    forPrint = 1;
    DOT.Push(padre);
    EXPA();
    forPrint = 0;
    Match('PARACERRAR', padre)
    DOT.Push(padre);
    BLOQUE();
}

function IF() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"IF"',padre);
    ifAnidados? traduccion += "if " : traduccion += Tabulador() +"if ";
    Match('RIF', padre);
    Match('PARABRIR', padre);
    DOT.Push(padre);
    EXPA();
    Match('PARACERRAR', padre);
    traduccion +=   ":\n";
    DOT.Push(padre);
    BLOQUE();
    if (TokenActual[1] == "RELSE") {
        Match('RELSE', padre);
        if (TokenActual[1] == "RIF") {
            DOT.Push(padre);
            traduccion += Tabulador() +"el";
            ifAnidados = true;
            IF();
            ifAnidados = false;
        } else {
            DOT.Push(padre);
            traduccion += Tabulador() +"else:\n";
            BLOQUE();
        }
    }else{
        //epsilon
    }
}

function INTERFAZ() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"INTERFAZ"',padre);
    Match('RINTERFACE', padre);
    Match('IDENTIFICADOR', padre);
    DOT.Push(padre);
    BLOQUE();
}

function CLASE() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"CLASE"',padre);     
    if (TokenActual[1] == "RCLASS") {
        Match('RCLASS', padre);
        traduccion += Tabulador() +`class ${TokenActual[0]}:\n`;
        Match('IDENTIFICADOR', padre);
        DOT.Push(padre);        
        BLOQUE();
    } else if(TokenActual[1] == "RSTATIC") {
        Match('RSTATIC', padre);
        Match('RVOID', padre);
        Match('RMAIN', padre);  
        Match('PARABRIR', padre);
        Match('RSTRING', padre);
        Match('CORABRIR', padre);
        Match('CORCERRAR', padre);
        Match('IDENTIFICADOR', padre);
        Match('PARACERRAR', padre);
        DOT.Push(padre);
        traduccion += Tabulador() +'if __name__="__main__"\n';
        contadorBloques++;
        traduccion += Tabulador() +"main()\n";
        contadorBloques--;
        traduccion += Tabulador() +"def main():\n";
        BLOQUE();
    }
}

function FUNC() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"FUNC"',padre);    
    DOT.Push(padre);
    TIPO();
    traduccion += Tabulador() + `self ${TokenActual[0]}(`;
    Match('IDENTIFICADOR', padre);
    Match('PARABRIR', padre);
    DOT.Push(padre);
    LPD();
    traduccion += ")";
    Match('PARACERRAR', padre);
    if (TokenActual[1] == "PUNTOCOMA") {
        traduccion +=   ";\n";
        Match('PUNTOCOMA', padre);
    }else{
        DOT.Push(padre);
        traduccion +=   ":\n";
        BLOQUE();
    }
}

function CALL() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"CALL"',padre);
    traduccion +=   TokenActual[0]+"(";
    Match('IDENTIFICADOR', padre);
    Match('PARABRIR', padre);
    DOT.Push(padre);
    LPE();
    traduccion +=   ")";
    Match('PARACERRAR', padre);
}

function PRINT() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"PRINT"',padre);
    Match('RSYSTEM', padre);
    Match('PUNTO', padre);
    Match('ROUT', padre);
    Match('PUNTO', padre);
    if (TokenActual[1] == "RPRINT") {
        Match('RPRINT', padre);
        Match('PARABRIR', padre);
        DOT.Push(padre);
        traduccion += Tabulador() + "print(";
        EXPA();
        traduccion +=   ")";
        Match('PARACERRAR', padre);
    } else if (TokenActual[1] == "RPRINTLN"){
        Match('RPRINTLN', padre);
        Match('PARABRIR', padre);
        DOT.Push(padre);
        traduccion += Tabulador() + "print(";
        EXPA();
        traduccion +=   ', end="")';
        Match('PARACERRAR', padre);
    }else{
        //ERROR
        Error("'RPRINT', 'RPRINTLN'", TokenActual[1], TokenActual[2], TokenActual[3])
    }
}

function LPE() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    } 
    if (TokenActual[1] == 'IDENTIFICADOR' || TokenActual[1] == 'CADENA' || TokenActual[1] == 'CARACTER' || TokenActual[1] == 'RTRUE' || TokenActual[1] == 'RFALSE' || TokenActual[1] == 'ENTERO' || TokenActual[1] == 'DECIMAL' || TokenActual[1] == 'PARABRIR') {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"LPE"',padre); 
        DOT.Push(padre);
        EXPA();
        DOT.Push(padre);
        LPEP();
    } else {
        //epsilon
    }
    
}

function LPEP() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    if (TokenActual[1] == "COMA") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"LPE"',padre);
        traduccion +=   ", ";
        Match('COMA', padre);
        DOT.Push(padre);
        EXPA();
        DOT.Push(padre);
        LPEP();
    } else {
        //epsilon
    }
}

function LPD() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    if (TokenActual[1] == 'RINT' || TokenActual[1] == 'RDOUBLE' || TokenActual[1] == 'RSTRING' || TokenActual[1] == 'RVOID' || TokenActual[1] == 'RBOOLEAN' || TokenActual[1] == 'RCHAR') {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"LPD"',padre);     
        DOT.Push(padre);
        TIPO();
        traduccion +=   TokenActual[0];
        Match('IDENTIFICADOR', padre);
        DOT.Push(padre);
        LPDP();
    } else {
        //epsilon
    } 
}

function LPDP() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }  
    if (TokenActual[1] == "COMA") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"LPD"',padre);
        traduccion +=   ", ";
        Match('COMA', padre);
        DOT.Push(padre);
        traduccion +=   "var ";
        TIPO();
        traduccion +=   TokenActual[0];
        Match('IDENTIFICADOR', padre);
        DOT.Push(padre);
        LPDP();
    } else {
        //epsilon
    }
}

function ASG() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"ASIGNACION"',padre);
    traduccion += Tabulador()+ TokenActual[0];
    Match('IDENTIFICADOR', padre);
    if (TokenActual[1] == 'IGUAL') {
        traduccion +=   " = ";
        Match('IGUAL', padre);
        DOT.Push(padre);
        EXPA();
    }else if (TokenActual[1] == 'ADICION'){
        traduccion += " += 1";
        Match('ADICION', padre);
    }else if (TokenActual[1] == 'SUBSTRACCION'){
        traduccion += " -= 1";
        Match('ADICION', padre);
    }else{
        //ERROR
        Error("'IGUAL', 'ADICION', 'SUBSTRACCION'", TokenActual[1], TokenActual[2], TokenActual[3]);
    }  
}

function SENTENCIAS() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    } 
    if (TokenActual[1] == "RBREAK") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"SENTENCIAS"',padre);
        traduccion += Tabulador() + "break";
        Match('RBREAK', padre);
    }else if (TokenActual[1] == "RCONTINUE"){
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"SENTENCIAS"',padre);
        traduccion += Tabulador() + "continue";
        Match('RCONTINUE', padre);
    }else if (TokenActual[1] == "RRETURN"){
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"SENTENCIAS"',padre);
        traduccion += Tabulador() + "return ";
        Match('RRETURN', padre);
        if (TokenActual[1] == "PUNTOCOMA") {
            // epsilon
        } else {
            DOT.Push(padre);
            EXPA();
        }
    }else{
        //epsilon
    }
}

function DEC() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"DEC"',padre);
    DOT.Push(padre);
    if (forPrint == 0) {
        traduccion += Tabulador() + "var "
    }
    TIPO();
    DOT.Push(padre);
    LISTADEC();
}

function LISTADEC() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"DEC"',padre);
    if (forPrint == 1) {
        traduccion +=   TokenActual[0]+" in range(";
    }
    if (forPrint == 0) {
        traduccion +=   TokenActual[0];
    }
    Match('IDENTIFICADOR', padre);
    if (TokenActual[1] == "IGUAL") {   
        if (forPrint == 0) {
            traduccion +=   " = ";
        }
        Match('IGUAL', padre);
        DOT.Push(padre);
        forPrint == 1? forPrint = 2:forPrint=0;
        EXPA();
        DOT.Push(padre);
        LISTADECP();
    }else{
        DOT.Push(padre);
        LISTADECP();
    }
}

function LISTADECP() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    if (TokenActual[1] == "COMA") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"DEC"',padre);
        traduccion +=   ", ";
        Match('COMA', padre);
        traduccion +=   TokenActual[0];
        Match('IDENTIFICADOR', padre);
        if (TokenActual[1] == "IGUAL") {
            traduccion +=   " = ";
            Match('IGUAL', padre);
            DOT.Push(padre);
            EXPA();
            DOT.Push(padre);
            LISTADECP();
        } else if (TokenActual[1] == "COMA") {
            DOT.Push(padre);
            LISTADECP();
        } else{
            //epsilon
        }
    }else{
        //epsilon
    }
}

function TIPO() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"TIPO"',padre);
    switch (TokenActual[1]) {
        case "RINT":
            Match('RINT', padre);            
            break;
        case "RSTRING":  
            Match('RSTRING', padre);          
            break;
        case "RBOOLEAN":
            Match('RBOOLEAN', padre);          
            break;
        case "RDOUBLE":
            Match('RDOUBLE', padre);        
            break;
        case "RCHAR":
            Match('RCHAR', padre);          
            break;
        case "RVOID":
            Match('RVOID', padre);           
            break;    
        default:
            //ERROR
            Error("'RINT', 'RDOUBLE', 'RSTRING', 'RBOOLEAN', 'RCHAR', 'RVOID'",TokenActual[1], TokenActual[2], TokenActual[3]);            
            break;
    }
}

function EXPA() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"EXP"',padre);
    DOT.Push(padre);
    EXPB();
    DOT.Push(padre);
    EXPAP();
}

function EXPAP() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    if (TokenActual[1] == "OR") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        if (forPrint == 0) {
            traduccion +=   " or ";
        }
        Match('OR', padre);   
        DOT.Push(padre);
        EXPB();
        DOT.Push(padre);
        EXPAP();
    } else {
        //epsilon
    }
}

function EXPB() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"EXP"',padre);
    DOT.Push(padre);
    EXPC();
    DOT.Push(padre);
    EXPBP();
}

function EXPBP() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }   
    if (TokenActual[1] == "AND") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        if (forPrint == 0) {
            traduccion +=   " and ";
        }
        Match('AND', padre);
        DOT.Push(padre);
        EXPC();
        DOT.Push(padre);
        EXPBP();
    } else {
        //epsilon
    }
}

function EXPC() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"EXP"',padre); 
    DOT.Push(padre);
    EXPD();
    DOT.Push(padre);
    EXPCP();
}

function EXPCP() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    if (TokenActual[1] == "XOR") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        if (forPrint == 0) {
            traduccion +=   " xor ";
        }
        Match('XOR', padre);
        DOT.Push(padre);
        EXPD();
        DOT.Push(padre);
        EXPCP();
    } else {
        //epsilon
    }
}

function EXPD() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"EXP"',padre);
    DOT.Push(padre);
    EXPE();
    DOT.Push(padre);
    EXPDP();
}

function EXPDP() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    if (TokenActual[1] == "IGUALIGUAL") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        if (forPrint == 0) {
            traduccion +=   " == ";
        }
        Match('IGUALIGUAL', padre);
        DOT.Push(padre);
        EXPE();
        DOT.Push(padre);
        EXPDP();
    } else if (TokenActual[1] == "DIFERENTE"){
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        if (forPrint == 0) {
            traduccion +=   " != ";
        }
        Match('DIFERENTE', padre);
        DOT.Push(padre);
        EXPE();
        DOT.Push(padre);
        EXPDP();
    }else{
        //epsilon
    }
}

function EXPE() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"EXP"',padre);
    DOT.Push(padre);
    EXPF();
    DOT.Push(padre);
    EXPEP();
}

function EXPEP() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    if (TokenActual[1] == "MAYOR") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        if (forPrint == 0) {
            traduccion +=   " > ";
        }
        Match('MAYOR', padre); 
        forPrint == 1? forPrint = 2:forPrint=0;
        DOT.Push(padre);
        EXPF();
        DOT.Push(padre);
        EXPEP();
    }else if (TokenActual[1] == "MENOR") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        if (forPrint == 0) {
            traduccion +=   " > ";
        }
        Match('MENOR', padre);
        forPrint == 1? forPrint = 2:forPrint=0;
        DOT.Push(padre);
        EXPF();
        DOT.Push(padre);
        EXPEP();
    }else if (TokenActual[1] == "MENORIGUAL") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        if (forPrint == 0) {
            traduccion +=   " <= ";
        }
        Match('MENORIGUAL', padre);
        forPrint == 1? forPrint = 2:forPrint=0;
        DOT.Push(padre);
        EXPF();
        DOT.Push(padre);
        EXPEP();
    }else if (TokenActual[1] == "MAYORIGUAL") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        if (forPrint == 0) {
            traduccion +=   " >= ";
        }
        Match('MAYORIGUAL', padre);
        forPrint == 1? forPrint = 2:forPrint=0;
        DOT.Push(padre);
        EXPF();
        DOT.Push(padre);
        EXPEP();
    } else {
        //epsilon
    }
}

function EXPF() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"EXP"',padre);
    DOT.Push(padre);
    EXPG();
    DOT.Push(padre);
    EXPFP();
}

function EXPFP() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    if (TokenActual[1] == "MAS") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        if (forPrint == 0) {
            traduccion +=   " + ";
        }
        Match('MAS', padre);
        DOT.Push(padre);
        EXPG();
        DOT.Push(padre);
        EXPFP();
    }else if (TokenActual[1] == "MENOS") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        if (forPrint == 0) {
            traduccion +=   " - ";
        }
        Match('MENOS', padre);
        DOT.Push(padre);
        EXPG();
        DOT.Push(padre);
        EXPFP();
    } else {
        //epsilon
    }
}

function EXPG() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"EXP"',padre);
    DOT.Push(padre);
    EXPH();
    DOT.Push(padre);
    EXPGP();
}

function EXPGP() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    if (TokenActual[1] == "MULTIPLICACION") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        if (forPrint == 0) {
            traduccion +=   " * ";
        }
        Match('MULTIPLICACION', padre);
        DOT.Push(padre);
        EXPH();
        DOT.Push(padre);
        EXPGP();
    }else if (TokenActual[1] == "DIVISION") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        if (forPrint == 0) {
            traduccion +=   " / ";
        }
        Match('DIVISION', padre);
        DOT.Push(padre);
        EXPH();
        DOT.Push(padre);
        EXPGP();
    } else {
        //epsilon
    }
}

function EXPH(){
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    if (TokenActual[1] == "MENOS") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        if (forPrint == 0) {
            traduccion +=   " - ";
        }
        Match('MENOS', padre);
        DOT.Push(padre);
        EXPA();
    } else {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        DOT.Push(padre);
        EXPI();
    }
}

function EXPI() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    if (TokenActual[1] == "NOT") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        if (forPrint == 0) {
            traduccion +=   "not ";
        }
        Match('NOT', padre);
        DOT.Push(padre);
        EXPA();
    } else {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        DOT.Push(padre);
        EXPJ();
    }
}

function EXPJ() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"EXP"',padre);
    DOT.Push(padre);
    EXPK();
    DOT.Push(padre);
    EXPJP();
}

function EXPJP() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    if (TokenActual[1] == "ADICION") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        if (forPrint == 0) {
            traduccion +=   " += 1";
        }
        Match('ADICION', padre); 
        DOT.Push(padre);
        EXPJP();
    }else if (TokenActual[1] == "SUBSTRACCION") {
        var padre = DOT.GetNodo();
        DOT.InsertRoot('"EXP"',padre);
        if (forPrint == 0) {
            traduccion +=   " -= 1";
        }
        Match('SUBSTRACCION', padre);
        DOT.Push(padre);
        EXPJP();
    } else {
        //epsilon
    }
}

function EXPK() {
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    var padre = DOT.GetNodo();
    DOT.InsertRoot('"EXP"',padre);
    
    if (forPrint == 2 || forPrint == 0) {
        traduccion +=   TokenActual[0];
    }
    switch (TokenActual[1]) {
        case "PARABRIR":
            Match('PARABRIR', padre);            
            DOT.Push(padre);
            EXPA();
            traduccion += ")";
            Match('PARACERRAR', padre);           
            break;
        case "IDENTIFICADOR":
            Match('IDENTIFICADOR', padre);         
            break;
        case "CADENA":
            Match('CADENA', padre);        
            break;
        case "CARACTER":
            Match('CARACTER', padre);         
            break;
        case "ENTERO":
            Match('ENTERO', padre);         
            break;
        case "DECIMAL":
            Match('DECIMAL', padre);       
            break;
        case "RTRUE":
            Match('RTRUE', padre);      
            break;
        case "RFALSE":
            Match('RFALSE', padre);     
            break;
        default:
            //ERROR
            Error("'EXPRESION'", TokenActual[1], TokenActual[2], TokenActual[3]);
           // Match('EXPRESION', padre);     
            break;      
    }
}

function Match(tipo, padre) {
    DOT.InsertNode(`"${TokenActual[0].replace(/\"/gi,"\\\"")}"`,padre);
    if (TokenActual[1] == 'ULTIMO') {
        return
    }
    if (TokenActual[1] != tipo) {
        Error(tipo, TokenActual[1], TokenActual[2], TokenActual[3]);
    } else {
        if (TokenActual[1] != 'ULTIMO') {
            TokenActual = tokens_parse.pop();
        } else {
            return
        }
    }
}

function Error(waiting, getting, line, column) {
    console.log(`Se esperaba ${waiting} se obtuvo ${getting}`)
    Errores.AgregarError(waiting, line, column, 2);
    var puntcoma = false;

    while (!puntcoma) {
        if (TokenActual[1] == 'PUNTOCOMA' || TokenActual[1] == 'LLAVECERRAR' || TokenActual[1] == 'LLAVEABRIR') {
            TokenActual = tokens_parse.pop();
            puntcoma = true;
        }else if(TokenActual[1] == 'ULTIMO'){
            return
        }else{
            TokenActual = tokens_parse.pop();
        }
    }
    INSTRUCCIONES();
}

exports.parser = parser;
exports.Clear = Clear;
exports.Tokens = GetTokens;