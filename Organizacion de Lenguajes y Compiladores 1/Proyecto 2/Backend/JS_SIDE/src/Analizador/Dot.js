let dot = ""
var nodo = 0;
var nodos = []



var SetDot = {
    IF: function SetIf(setif) {
        switch (setif) {
            case 0:
                var padre = nodo;
                nodo++
                var hijo0 = nodo;
                nodo++
                var hijo1 = nodo;
                nodo++
                var hijo2 = nodo;
                nodo++
                console.log(`node${padre}[label = "IF"];`);
                console.log(`node${hijo0}[label = "if"];`);
                console.log(`node${hijo1}[label = "("];`);
                console.log(`node${hijo2}[label = ")"];`);
                console.log(`node${padre} -> node${hijo0};`);
                console.log(`node${padre} -> node${hijo1};`);
                console.log(`node${padre} -> node${nodos[nodos.length - 2]};`);
                console.log(`node${padre} -> node${hijo2};`);
                console.log(`node${padre} -> node${nodos[nodos.length - 1]};`);
                nodos.pop()
                nodos.pop()
                nodos.push(padre);
                break;
        
            case 1:
                var padre = nodo;
                nodo++
                var hijo0 = nodo;
                nodo++
                var hijo1 = nodo;
                nodo++
                var hijo2 = nodo;
                nodo++
                var hijo3 = nodo;
                nodo++
                console.log(`node${padre}[label = "IF"];`);
                console.log(`node${hijo0}[label = "if"];`);
                console.log(`node${hijo1}[label = "("];`);
                console.log(`node${hijo2}[label = ")"];`);
                console.log(`node${hijo3}[label = "else"];`);
                console.log(`node${padre} -> node${hijo0};`);
                console.log(`node${padre} -> node${hijo1};`);
                console.log(`node${padre} -> node${nodos[nodos.length - 3]};`);
                console.log(`node${padre} -> node${hijo2};`);
                console.log(`node${padre} -> node${nodos[nodos.length - 2]};`);
                console.log(`node${padre} -> node${hijo3};`);
                console.log(`node${padre} -> node${nodos[nodos.length - 1]};`);
                nodos.pop()
                nodos.pop()
                nodos.pop()
                nodos.push(padre);
                break;
        }
    },
    DO: function SetDo() {
        var padre = nodo;
        nodo++;
        var hijo0 = nodo;
        nodo++;
        var hijo1 = nodo;
        nodo++;
        var hijo2 = nodo;
        nodo++;
        var hijo3 = nodo;
        nodo++;

        console.log(`node${padre}[label = "DOWHILE"];`);
        console.log(`node${hijo0}[label = "do"];`);
        console.log(`node${hijo1}[label = "("];`);
        console.log(`node${hijo2}[label = ")"];`);
        console.log(`node${hijo3}[label = "while"];`);
        console.log(`node${padre} -> node${hijo0};`);
        console.log(`node${padre} -> node${nodos[nodos.length - 2]};`);
        console.log(`node${padre} -> node${hijo3};`);
        console.log(`node${padre} -> node${hijo1};`);
        console.log(`node${padre} -> node${nodos[nodos.length - 1]};`);
        console.log(`node${padre} -> node${hijo2};`);
        nodos.pop()
        nodos.pop()
        nodos.push(padre);
    }, 
    WHILE: function SetWhile() {
        var padre = nodo;
        nodo++;
        var hijo0 = nodo;
        nodo++;
        var hijo1 = nodo;
        nodo++;
        var hijo2 = nodo;
        nodo++;

        console.log(`node${padre}[label = "WHILE"];`);
        console.log(`node${hijo0}[label = "while"];`);
        console.log(`node${hijo1}[label = "("];`);
        console.log(`node${hijo2}[label = ")"];`);

        console.log(`node${padre} -> node${hijo0};`);
        console.log(`node${padre} -> node${hijo1};`);
        console.log(`node${padre} -> node${nodos[nodos.length - 2]};`);
        console.log(`node${padre} -> node${hijo2};`);
        console.log(`node${padre} -> node${nodos[nodos.length - 1]};`);
        nodos.pop()
        nodos.pop()
        nodos.push(padre);
    },
    FOR: function SetFor() {
        var padre = nodo;
        nodo++;
        var hijo0 = nodo;
        nodo++;
        var hijo1 = nodo;
        nodo++;
        var hijo2 = nodo;
        nodo++;
        var hijo3 = nodo;
        nodo++;
        var hijo4 = nodo;
        nodo++;
        console.log(`node${padre}[label = "FOR"];`);
        console.log(`node${hijo0}[label = "for"];`);
        console.log(`node${hijo1}[label = "("];`);
        console.log(`node${hijo2}[label = ";"];`);
        console.log(`node${hijo3}[label = ";"];`);
        console.log(`node${hijo4}[label = ")"];`);

        console.log(`node${padre} -> node${hijo0};`);
        console.log(`node${padre} -> node${hijo1};`);
        console.log(`node${padre} -> node${nodos[nodos.length - 4]};`);
        console.log(`node${padre} -> node${hijo2};`);
        console.log(`node${padre} -> node${nodos[nodos.length - 3]};`);
        console.log(`node${padre} -> node${hijo3};`);
        console.log(`node${padre} -> node${nodos[nodos.length - 2]};`);
        console.log(`node${padre} -> node${hijo4};`);
        console.log(`node${padre} -> node${nodos[nodos.length - 1]};`);
        nodos.pop()
        nodos.pop()
        nodos.pop()
        nodos.pop()
        nodos.push(padre);
    },
    INTERFAZ: function SetInterfaz(value) {
        var padre = nodo;
        nodo++;
        var hijo0 = nodo;
        nodo++;
        var hijo1 = nodo;
        nodo++;
        var hijo2 = nodo;
        nodo++;

        console.log(`node${padre}[label = "INTERFACE"];`);
        console.log(`node${hijo0}[label = "public"];`);
        console.log(`node${hijo1}[label = "interface"];`);
        console.log(`node${hijo2}[label = "${value}"];`);

        console.log(`node${padre} -> node${hijo0};`);
        console.log(`node${padre} -> node${hijo1};`);
        console.log(`node${padre} -> node${hijo2};`);
        console.log(`node${padre} -> node${nodos.pop()};`);

        nodos.push(padre);
    },
    CLASS: function SetClass(setClass, value) {
        switch (setClass) {
            case 0:
                var padre = nodo;
                nodo++;
                var hijo0 = nodo;
                nodo++;
                var hijo1 = nodo;
                nodo++;
                var hijo7 = nodo;
                nodo++;
                console.log(`node${padre}[label = "MAIN"];`);
                console.log(`node${hijo0}[label = "public"];`);
                console.log(`node${hijo1}[label = "class"];`);
                console.log(`node${hijo7}[label = "${value}"];`);
                console.log(`node${padre} -> node${hijo0};`);
                console.log(`node${padre} -> node${hijo1};`);
                console.log(`node${padre} -> node${hijo7};`);
                console.log(`node${padre} -> node${nodos.pop()};`);
                nodos.push(padre);
                break;
        
            case 1:
                var padre = nodo;
                nodo++;
                var hijo0 = nodo;
                nodo++;
                var hijo1 = nodo;
                nodo++;
                var hijo2 = nodo;
                nodo++;
                var hijo3 = nodo;
                nodo++;
                var hijo4 = nodo;
                nodo++;
                var hijo5 = nodo;
                nodo++;
                var hijo6 = nodo;
                nodo++;
                var hijo7 = nodo;
                nodo++;
                var hijo8 = nodo;
                nodo++;
                console.log(`node${padre}[label = "MAIN"];`);
                console.log(`node${hijo0}[label = "public"];`);
                console.log(`node${hijo1}[label = "static"];`);
                console.log(`node${hijo2}[label = "void"];`);
                console.log(`node${hijo3}[label = "main"];`);
                console.log(`node${hijo4}[label = "("];`);
                console.log(`node${hijo5}[label = "String"];`);
                console.log(`node${hijo6}[label = "["];`);
                console.log(`node${hijo7}[label = "]"];`);
                console.log(`node${hijo8}[label = ")"];`);
                console.log(`node${padre} -> node${hijo0};`);
                console.log(`node${padre} -> node${hijo1};`);
                console.log(`node${padre} -> node${hijo2};`);
                console.log(`node${padre} -> node${hijo3};`);
                console.log(`node${padre} -> node${hijo4};`);
                console.log(`node${padre} -> node${hijo5};`);
                console.log(`node${padre} -> node${hijo6};`);
                console.log(`node${padre} -> node${hijo7};`);
                console.log(`node${padre} -> node${hijo8};`);
                console.log(`node${padre} -> node${nodos.pop()};`);
                nodos.push(padre);
                break;
        }
    },
    FUNCION: function SetFuncion(set) {
        switch (set) {
            case 0:
                var padre = nodo;
                nodo++;
                var hijo0 = nodo;
                nodo++;
                var hijo1 = nodo;
                nodo++;
                var hijo2 = nodo;
                nodo++;
                var hijo3 = nodo;
                nodo++;
                console.log(`node${padre}[label = "FUNCION"];`);
                console.log(`node${hijo0}[label = "public"];`);
                console.log(`node${hijo1}[label = "${$$[$0-4]}"];`);
                console.log(`node${hijo2}[label = "("];`);
                console.log(`node${hijo3}[label = ")"];`);
                console.log(`node${padre} -> node${hijo0};`);
                console.log(`node${padre} -> node${nodos[nodos.length - 3]};`);
                console.log(`node${padre} -> node${hijo1};`);
                console.log(`node${padre} -> node${hijo2};`);
                console.log(`node${padre} -> node${nodos[nodos.length - 2]};`);
                console.log(`node${padre} -> node${hijo3};`);
                console.log(`node${padre} -> node${nodos[nodos.length - 1]};`);
                nodos.pop()
                nodos.pop()
                nodos.pop()
                nodos.push(padre)
                break;
        
            case 1:
                var padre = nodo;
                nodo++;
                var hijo0 = nodo;
                nodo++;
                var hijo1 = nodo;
                nodo++;
                var hijo2 = nodo;
                nodo++;
                var hijo3 = nodo;
                nodo++;
                console.log(`node${padre}[label = "FUNCION"];`);
                console.log(`node${hijo0}[label = "public"];`);
                console.log(`node${hijo1}[label = "${$$[$0-4]}"];`);
                console.log(`node${hijo2}[label = "("];`);
                console.log(`node${hijo3}[label = ")"];`);
                console.log(`node${padre} -> node${hijo0};`);
                console.log(`node${padre} -> node${nodos[nodos.length - 2]};`);
                console.log(`node${padre} -> node${hijo1};`);
                console.log(`node${padre} -> node${hijo2};`);
                console.log(`node${padre} -> node${nodos[nodos.length - 1]};`);
                console.log(`node${padre} -> node${hijo3};`);
                nodos.pop()
                nodos.pop()
                nodos.push(padre)
                break;
        }
    },
    CALL: function SetCall(value) {
        var padre = nodo;
        nodo++;
        var hijo0 = nodo;
        nodo++;
        var hijo1 = nodo;
        nodo++;
        var hijo2 = nodo;
        nodo++;
        console.log(`node${padre}[label = "CALL"];`);
        console.log(`node${hijo0}[label = "("];`);
        console.log(`node${hijo1}[label = ")"];`);
        console.log(`node${hijo2}[label = "${value}"];`);
        console.log(`node${padre} -> node${hijo2};`);
        console.log(`node${padre} -> node${hijo0};`);
        console.log(`node${padre} -> node${nodos.pop()};`);
        console.log(`node${padre} -> node${hijo1};`);
        nodos.push(padre);
    },
    PRINT: function SetPrint() {
        var padre = nodo;
        nodo++;
        var hijo0 = nodo;
        nodo++;
        var hijo1 = nodo;
        nodo++;
        var hijo2 = nodo;
        nodo++;
        var hijo3 = nodo;
        nodo++;
        var hijo4 = nodo;
        nodo++;
        var hijo5 = nodo;
        nodo++;
        var hijo6 = nodo;
        nodo++;
        console.log(`node${padre}[label = "PRINT"];`);
        console.log(`node${hijo0}[label = "system"];`);
        console.log(`node${hijo1}[label = "."];`);
        console.log(`node${hijo2}[label = "out"];`);
        console.log(`node${hijo3}[label = "."];`);
        console.log(`node${hijo4}[label = "println"];`);
        console.log(`node${hijo5}[label = "("];`);
        console.log(`node${hijo6}[label = ")"];`);
        console.log(`node${padre} -> node${hijo0};`);
        console.log(`node${padre} -> node${hijo1};`);
        console.log(`node${padre} -> node${hijo2};`);
        console.log(`node${padre} -> node${hijo3};`);
        console.log(`node${padre} -> node${hijo4};`);
        console.log(`node${padre} -> node${hijo5};`);
        console.log(`node${padre} -> node${nodos.pop()};`);
        console.log(`node${padre} -> node${hijo6};`);
        nodos.push(padre)
    }, 
    LISTA: function setLista(set, value) {
        switch (set) {
            case 0:
                var padre = nodo;
                nodo++;
                var hijo0 = nodo;
                nodo++;
                var hijo1 = nodo;
                nodo++
                console.log(`node${padre}[label = "LISTA"];`);
                console.log(`node${hijo0}[label = ","];`);
                console.log(`node${padre} -> node${nodos[nodos.length - 2]};`);
                console.log(`node${padre} -> node${hijo0};`);
                console.log(`node${padre} -> node${nodos[nodos.length - 1]};`);
                nodos.pop()
                nodos.pop()
                nodos.push(padre)
                break;
        
            case 1:
                var padre = nodo;
                nodo++;
                var hijo0 = nodo;
                nodo++;
                var hijo1 = nodo;
                nodo++
                console.log(`node${padre}[label = "LISTA"];`);
                console.log(`node${hijo0}[label = ","];`);
                console.log(`node${hijo1}[label = "${value}"];`);
                console.log(`node${padre} -> node${nodos.pop()};`);
                console.log(`node${padre} -> node${hijo0};`);
                console.log(`node${padre} -> node${hijo1};`);
                nodos.push(padre)
                break;
            case 2:
                var padre = nodo;
                nodo++;
               
                console.log(`node${padre}[label = "LISTA"];`);
                console.log(`node${padre} -> node${nodos.pop()};`);
                nodos.push(padre);
                break;
            case 3:
                var padre = nodo;
                nodo++;
                var hijo0 = nodo;
                nodo++;
                console.log(`node${padre}[label = "LISTA"];`);
                console.log(`node${hijo0}[label = "${$$[$0]}"];`);
                console.log(`node${padre} -> node${nodos.pop()};`);
                console.log(`node${padre} -> node${hijo0};`);
                nodos.push(padre);
                break;
            case 4:
                var padre = nodo;
                nodo++;
                var hijo0 = nodo;
                nodo++;
                var hijo1 = nodo;
                nodo++
                console.log(`node${padre}[label = "LISTA"];`);
                console.log(`node${hijo0}[label = ","];`);
                console.log(`node${hijo1}[label = "${value}"];`);
                console.log(`node${padre} -> node${nodos[nodos.length - 2]};`);
                console.log(`node${padre} -> node${hijo0};`);
                console.log(`node${padre} -> node${nodos[nodos.length - 1]};`);
                console.log(`node${padre} -> node${hijo1};`);
                nodos.pop()
                nodos.pop()
                nodos.push(padre);
                break;
            case 5:
                var padre = nodo;
                nodo++
                var hijo0 = nodo;
                nodo++;
                console.log(`node${padre}[label = "LISTA"];`);
                console.log(`node${hijo0}[label = "="];`);
                console.log(`node${nodo}[label = "${$$[$0-2]}"];`);
                console.log(`node${padre} -> node${nodo};`);
                console.log(`node${padre} -> node${hijo0};`);
                console.log(`node${padre} -> node${nodos.pop()};`);
                nodo++
                nodos.push(padre)
                break;
            case 6:
                var padre = nodo;
                nodo++;
                var hijo0 = nodo;
                nodo++;
                var hijo1 = nodo;
                nodo++;
                console.log(`node${padre}[label = "LISTA"];`);
                console.log(`node${hijo0}[label = ","];`);
                console.log(`node${hijo1}[label = "="];`);
                console.log(`node${padre} -> node${nodos[nodos.length - 3]};`);
                console.log(`node${padre} -> node${hijo0};`);
                console.log(`node${padre} -> node${nodos[nodos.length - 2]};`);
                console.log(`node${padre} -> node${hijo1};`);
                console.log(`node${padre} -> node${nodos[nodos.length - 1]};`);
                nodos.pop()
                nodos.pop()
                nodos.pop()
                nodos.push(padre);
                break;

        }
        
    },
    DATO: function setDato(value) {
        nodos.push(nodo)
        console.log(`node${nodo}[label = "${value.replace(/\"/gi,"\\\"")}"]`);
        nodo++
    },
    ASIGNACION: function SetAsg(val) {
        var padre = nodo;
        nodo++
        var hijo0 = nodo;
        nodo++;
        console.log(`node${padre}[label = "ASIGNACION"];`);
        console.log(`node${hijo0}[label = "="];`);
        console.log(`node${nodo}[label = "${val}"];`);
        console.log(`node${padre} -> node${nodo};`);
        console.log(`node${padre} -> node${hijo0};`);
        console.log(`node${padre} -> node${nodos.pop()};`);
        nodo++
        nodos.push(padre)
    },
    SENTENCIA: function SetSentencia(set, val) {
        switch (set) {
            case 0:
                var padre = nodo;
                nodo++
                console.log(`node${padre}[label = "Sentencia"];`);
                console.log(`node${nodo}[label = "${val}"];`);
                console.log(`node${padre} -> node${nodo};`);
                nodo++
                nodos.push(padre);
                break;
        
            case 1:
                var padre = nodo;
                nodo++
                var hijo0 = nodo;
                nodo++
                console.log(`node${padre}[label = "Sentencia"];`);
                console.log(`node${nodo}[label = "return"];`);
                console.log(`node${hijo0}[label = "${value}"]`);
                console.log(`node${padre} -> node${nodo};`);
                nodo++
                nodos.push(padre);
                break;
        }
    },
    DECLARACION: function SetDeclaracion() {
        var padre = nodo;
        nodo++;
        console.log(`node${padre}[label = "DECLARACION"];`);
        console.log(`node${padre} -> node${nodos[nodos.length - 2]};`);
        console.log(`node${padre} -> node${nodos[nodos.length - 1]};`);
        nodos.pop()
        nodos.pop()
        nodos.push(padre)
    },
    EXPRESION: function SetExpresion(set, val) {
        switch (set) {
            case 0:
                var padre = nodo;
                nodo++;
                console.log(`node${nodo}[label = "${val}"];`);
                console.log(`node${padre}[label = "EXP"];`);
                console.log(`node${padre} -> node${nodos[nodos.length - 2]};`);
                console.log(`node${padre} -> node${nodo};`);
                console.log(`node${padre} -> node${nodos[nodos.length - 1]};`);
                nodos.pop()
                nodos.pop()
                nodos.push(padre);
                nodo++
                break;
        
            case 1:
                var padre = nodo;
                nodo++;
                console.log(`node${nodo}[label = "${val}"];`);
                console.log(`node${padre}[label = "EXP"];`);
                console.log(`node${padre} -> node${nodos.pop()};`);
                console.log(`node${padre} -> node${nodo};`);
                nodos.push(padre)
                nodo++
                break;
            case 2:
                var padre = nodo;
                nodo++;
                console.log(`node${nodo}[label = "${val}"];`);
                console.log(`node${padre}[label = "EXP"];`);
                console.log(`node${padre} -> node${nodo};`);
                console.log(`node${padre} -> node${nodos.pop()};`);
                nodos.push(padre)
                nodo++
                break;
            case 3:
                var padre = nodo;
                nodo++;
                var hijo0 = nodo;
                nodo++;
                var hijo1 = nodo; 
                nodo++;

                console.log(`node${hijo0}[label = "("];`);
                console.log(`node${hijo1}[label = ")"];`);
                console.log(`node${padre}[label = "EXP"];`);

                console.log(`node${padre} -> node${hijo0};`);
                console.log(`node${padre} -> node${nodos.pop()};`);
                console.log(`node${padre} -> node${hijo1};`);
                nodos.push(padre)
        }
    },
    BLOQUE: function SetBloque(set) {
        switch (set) {
            case 0:
                var padre = nodo;
                nodo++;
                var hijo0 = nodo;
                nodo++;
                var hijo1 = nodo;
                nodo++;
                console.log(`node${padre}[label = "BLOQUESENTENCIAS"];`);
                console.log(`node${hijo0}[label = "{"];`);
                console.log(`node${hijo1}[label = "}"];`);
                console.log(`node${padre} -> node${hijo0};`);
                console.log(`node${padre} -> node${nodos.pop()};`);
                console.log(`node${padre} -> node${hijo1};`);
                nodos.push(padre)
                break;
        
            case 1:
                var padre = nodo;
                nodo++;
                var hijo0 = nodo;
                nodo++;
                var hijo1 = nodo;
                nodo++;
                console.log(`node${padre}[label = "BLOQUESENTENCIAS"];`);
                console.log(`node${hijo0}[label = "{"];`);
                console.log(`node${hijo1}[label = "}"];`);
                console.log(`node${padre} -> node${hijo0};`);
                console.log(`node${padre} -> node${hijo1};`);
                nodos.push(padre)
                break;
        }
    },
    INSTRUCCION: function SetInstrucciones(set) {
        switch (set) {
            case 0:
                var padre = nodo;
                nodo++
                console.log(`node${padre}[label = "INSTRUCCION"];`);
                console.log(`node${nodo}[label = ";"];`);
                console.log(`node${padre} -> node${nodos.pop()};`);
                console.log(`node${padre} -> node${nodo};`);
                nodo++
                nodos.push(padre)
                break;
        
            case 1:
                var padre = nodo;
                nodo++
                console.log(`node${padre}[label = "INSTRUCCION"];`);
                console.log(`node${padre} -> node${nodos.pop()};`);
                nodos.push(padre)
                break;
        }
    },
    INSTRUCCIONES: function SetInstrucciones(set) {
        switch (set) {
            case 0:
                var padre = nodo;
                nodo++
                console.log(`node${padre}[label = "INSTRUCCIONES"];`);
                console.log(`node${padre} -> node${nodos[nodos.length - 2]};`);
                console.log(`node${padre} -> node${nodos[nodos.length - 1]};`);
                nodos.pop()
                nodos.pop()
                nodos.push(padre)
                break;
        
            case 1:
                var padre = nodo;
                nodo++
                console.log(`node${padre}[label = "INSTRUCCIONES"];`);
                console.log(`node${padre} -> node${nodos.pop()};`);
                nodos.push(padre)
                break;
        }
    }

}


exports.DOT = SetDot;