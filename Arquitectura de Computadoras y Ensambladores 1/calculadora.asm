;---------------DEFINICION DE MACROS-------------------//
;===============MANIPULACION DE DATOS==================//
encabezado MACRO
    imprimirCadena cabecera1
    ;cabecera2
    imprimirCadena cabecera2
    ;cabecera3
    imprimirCadena cabecera3
    ;cabecera4
    imprimirCadena cabecera4
    ;cabecera5
    imprimirCadena cabecera5
    ;cabecera6
    imprimirCadena cabecera6
    ;cabecera7
    imprimirCadena cabecera7
    ;menu0
    imprimirCadena menu0
    ;menu1
    imprimirCadena menu1
    ;menu2
    imprimirCadena menu2
ENDM

obtenerCaracter MACRO
    ;limpiamos el registro A
    xor ax, ax
    ;obtenemos el caracter se guarda en al
    mov ah, 01h
    int 21h
ENDM

obtenerCadena MACRO buffer
    LOCAL LEER, TERMINAR
    xor si, si 
    LEER:
        obtenerCaracter
        cmp al, 0dh
        je TERMINAR
        mov buffer[si], al
        inc si
        jmp LEER
    TERMINAR:        
        mov buffer[si], 00h   
ENDM

obtenerCadena_consola MACRO buffer
    LOCAL LEER, TERMINAR
    xor si, si 
    LEER:
        obtenerCaracter
        cmp al, 0dh
        je TERMINAR
        mov buffer[si], al
        inc si
        jmp LEER
    TERMINAR:        
        mov buffer[si], '$'   
ENDM

imprimirCaracter MACRO caracter
    ;imprimimos el valor
    mov ah, 06h
    mov dl, caracter
    int 21h
ENDM

imprimirCadena MACRO cadena
    xor dx, dx
    mov ah, 09h
    mov dx, offset cadena
    int 21h
ENDM

crearFichero MACRO buffer, handle
    mov ah, 3ch
    mov cx, 00h
    lea dx, buffer
    int 21h
    mov handle, ax
    jc ERRORCREARF
ENDM

cerrarFichero MACRO handle
    mov ah, 3eh
    mov bx, handle
    int 21h
    jc ERRORCREARF
ENDM

abrirFichero MACRO buffer, handle
    mov ah, 3dh
    mov al, 10b
    lea dx, buffer
    int 21h
    mov handle, ax
    jc ERRORCREARF
ENDM

leerFichero MACRO buffer, handle
    mov ah, 3fh
    mov bx, handle
    mov cx, sizeof buffer
    lea dx, buffer
    int 21h
    jc ERRORESCRIBIRF
ENDM

escribirFichero MACRO buffer, handle    
    mov ah, 40h
    mov bx, handle
    mov cx, sizeof buffer
    lea dx, buffer 
    int 21h
    jc ERRORESCRIBIRF
ENDM

string_to_int MACRO string
    LOCAL CONVERTSI, FINSI, ACTIVARC2, ULTIMACION, C2
    PUSH si
    PUSH cx
    xor si, si
    xor cx, cx
    xor ax, ax

    CONVERTSI:
        xor dx, dx ;limpiamos los residuos de dx
        xor bx, bx ;limpiamos los residuos de bx
        mov bx, 10d ;llevamos a bx el multiplicador 10
        mov cl, string[si] ;obtenemos el caracter del buffer
        cmp cl, 45 ;si es negativo
        je ACTIVARC2
        cmp cl, 48 ;si es menor de 48 (8) error
        jl ULTIMACION
        cmp cl, 57 ;si es mayor de 57 (9) error
        jg ULTIMACION
        sub cl, 48 ;restamos para obtener el numero
        mul bx ;multiplicamos ax = ax * bx, 
        add ax, cx ;sumamos a ax el valor que guardamos ax = ax + cl
        inc si ;incrementamos el valor del indice en 1
        jmp CONVERTSI
    ACTIVARC2:
        mov compA2, 1 ;activamos la negacion para despues
        inc si ;aumentamos el indice en 1
        jmp CONVERTSI
    ULTIMACION:
        cmp compA2, 1 ;si el complemento a2 esta activado negamos el numero
        je C2
        jmp FINSI
    C2:
        neg ax
    FINSI:
        mov compA2, 0 ;terminamos la conversion devolviendo el complemento siempre a 0
        POP cx
        POP si
        nop
        
ENDM

int_to_string MACRO numero, string
    LOCAL NEGATIVO, DIVIDIR, TERMINARDIV, CONV, FINDIV
    PUSH si
    PUSH di
    xor ax, ax
    xor dx, dx
    xor cx, cx
    xor si, si
    xor di, di
    mov ax, numero
    test ax, 1000000000000000
    js NEGATIVO
    jmp DIVIDIR

    NEGATIVO:
        ;ponemos en el array un negativo
        neg ax ;volvemos ax positivo
        mov string[di], 45
        inc di

    DIVIDIR:
        mov cx, 10d ;movemox a cx el divisor 10
        div cx ;operacion ax/cx = resultado ax; residuo dx
        push dx ;apilamos el valor residuo obtenido en dx
        xor dx, dx;limpiamos dx
        inc si
        cmp ax, 00h ;si ax es igual a 0 se deja de dividir
        je TERMINARDIV
        jmp DIVIDIR
    TERMINARDIV:
        mov cx, si ;movemos en cx lo que contamos en si
        xor si, si ;limpiamos si para dar indice en el string
        mov si, di ;guardamos en si lo que tenemos en di
        xor di, di ;limpiamos el di
        CONV:
            pop dx ;recuperamos nuestros residuo
            add dl, 48d ;sumamos 48 para obtener el valor en ascci
            mov string[si], dl ;movemos a nuestro array el valor de obtenido
            inc si ;aumentamos en 1 el indice
            loop conv
            mov dl, 36d ;añadimos el fin de cadena para consola
            mov string[si], dl
    FINDIV:
        POP di
        POP si
        nop
ENDM

compare_string MACRO string1, string2, result
    LOCAL EQUALC, NOTEQUALC, MATCHC, TRUEMATCHC, FINISHC
    PUSH si
    PUSH cx
    xor si, si
    xor cx, cx
    EQUALC:
        mov ch, string1[si]
        mov cl, string2[si]
        inc si
        cmp ch, '$'
        je MATCHC
        cmp ch, cl
        je EQUALC            
        jmp NOTEQUALC
    NOTEQUALC:
        mov result, 0
        jmp FINISHC
    MATCHC:
        cmp cl, '$'
        je TRUEMATCHC
        jmp NOTEQUALC
        TRUEMATCHC:
            mov result, 1
            jmp FINISHC
    FINISHC:
        POP cx
        POP si
    
ENDM

obtenerValor MACRO stored, string, value
    LOCAL EQUAL, NOTEQUAL, DELAY, BREAK, MATCH, FINISH
    PUSH si
    PUSH di
    PUSH ax
    xor si, si ;si para el array en memoria
    xor di, di ;di para el array de referencia
    xor ax, ax
    EQUAL:
        mov ah, stored[si]
        mov al, string[di]
        inc si
        inc di
        cmp ah, ':' ;cuando obtenemos un : sabemos que hemos llegado al final del string
        je MATCH
        cmp ah, al
        je EQUAL
        jmp NOTEQUAL
    NOTEQUAL:
        mov ah, stored[si]
        cmp ah, ':' ;cuando obtenemos un : aumentamos en 3 el indice
        je DELAY
        cmp ah, '$' ;cuando llega $ es que no se encontro el string y ya no hay mas datos para comparar
        je BREAK
        inc si ;aumentamos el indice en uno
        jmp NOTEQUAL ;y repetimos el ciclo hasta encontrar :
        DELAY:
            add si, 3
            XOR di, di
            jmp EQUAL
        BREAK:
            mov value, 0d
            jmp FINISH
    MATCH:
        mov ah, stored[si]
        inc si
        mov al, stored[si]
        mov value, ax
    FINISH:
        nop
        POP ax
        POP di
        POP si
ENDM

limpiarBuffer MACRO buffer
    LOCAL CLEAN
    PUSH cx
    PUSH si
    xor si, si
    mov cx, length buffer    
    CLEAN:
        mov buffer[si], 36
        inc si
        loop CLEAN
    POP si
    POP cx        
    
ENDM

recorrerArray MACRO array
    xor si, si
    INICIO:
        mov dl, array[si]
        cmp dl, 32 ; caso espacio
        je IGNORAR
        cmp dl, 8 ; retroceso
        je IGNORAR
        cmp dl, 9 ; tab
        je IGNORAR
        cmp dl, 10 ; nueva linea
        je IGNORAR
        cmp dl, 36 ; fin de print
        je TERMINAR
        cmp dl, 34 ; comillas
        je IDENTIFICADOR
        cmp dl, 123 ; abrirllaves
        je SUMADORLLAVE
        cmp dl, 125 ; cerrarllaves
        je RESTADORLLAVE
        ;es un caracter
        JMP IGNORAR
    SUMADORLLAVE:
        add contadorLlave, 1
        inc si
        jmp INICIO
    RESTADORLLAVE:
        sub contadorLlave, 1
        inc si
        jmp INICIO
    IDENTIFICADOR:
        inc si
        mov dl, array[si]
        cmp dl, 34 ; comillas
        je IGNORAR
        ;caracterdentro de comillas
        cmp contadorLlave, 1
        je OBJETOPADRE
        cmp contadorLlave, 2
        je OBJETOOPERACION
        ;OPERADORES
        cmp dl, 97 ;caso de caracter 'a'
        je IOPSUMA
        cmp dl, 43 ;caso de caracter '+'
        je OPGENERIC
        cmp dl, 115 ;caso de caracter 's'
        je IOPRESTA
        cmp dl, 45 ;caso de caracter '-'
        je OPGENERIC
        cmp dl, 109 ;caso de caracter 'm'
        je IOPMULTIPLICACION
        cmp dl, 42 ;caso de caracter '*'
        je OPGENERIC
        cmp dl, 100 ;caso de caracter 'd'
        je IOPDIVISION
        cmp dl, 47 ;caso de caracter '/'
        je OPGENERIC
        cmp dl, 35 ;caso de numeral '#'
        je OPDIGITO
        cmp dl, 105 ;cado de id
        je OPID
        jmp TERMINAR

            OPGENERIC:                                
                xor dh, dh 
                PUSH dx
                xor dx, dx
                mov dl, 1
                PUSH dx
                inc si
                jmp IGNORAR     
            IOPMULTIPLICACION: 
                ;imprimirCaracter 42
                xor dx, dx
                mov dl, 42
                PUSH dx
                xor dx, dx
                mov dl, 1
                PUSH dx
                add si, 3
                jmp IGNORAR               
            IOPDIVISION:
                ;imprimirCaracter 47
                xor dx, dx
                mov dl, 47
                PUSH dx
                xor dx, dx
                mov dl, 1
                PUSH dx
                add si, 3
                jmp IGNORAR
            IOPSUMA:
                ;imprimirCaracter 43
                xor dx, dx
                mov dl, 43
                PUSH dx
                xor dx, dx
                mov dl, 1
                PUSH dx
                add si, 3
                jmp IGNORAR
            IOPRESTA:
                ;imprimirCaracter 45
                xor dx, dx
                mov dl, 45
                PUSH dx
                xor dx, dx
                mov dl, 1
                PUSH dx
                add si, 3
                jmp IGNORAR
            OPDIGITO:
                ;lectura de un digito
                xor di, di
                limpiarBuffer buffnumero
                add si, 3 ;nos ponemos en el primer digito
                OPDIGITODEC:
                mov dl, array[si]
                cmp dl, 45 ;es un negativo
                je OPDIGITONEG
                cmp dl, 48 ;0 como el minimo
                jl OPDIGITOTER
                cmp dl, 57 ;9 como el maximo
                jg OPDIGITOTER
                jmp OPDIGITONUM
                OPDIGITONEG:
                    mov buffnumero[di], '-' ;guardamos el negativo
                    inc di
                    inc si
                    jmp OPDIGITODEC
                OPDIGITONUM:
                    mov buffnumero[di], dl ;guardamos el caracter numerico
                    inc di
                    inc si
                    jmp OPDIGITODEC
                OPDIGITOTER:
                    ;procedemos a convertirlo y a guardarlo
                    string_to_int buffnumero ;ax
                    mov resultado, ax ;guardamos el numero en resultado
                OPERACIONPILA:
                    POP bx ;sacamos el identrificador de tipo
                    cmp bx, 0
                    je TIPOF
                    cmp bx, 1 ;TIPO 1 -> OPERADORES
                    je TIPOP
                    cmp bx, 2 ;TIPO 2 -> NUMEROS
                    je TIPON
                    jmp TIPOF ;NADA
                        TIPOP: ;hay un operador solo se guarda
                            PUSH bx ;regresamos el tipo
                            PUSH resultado ;guardamos el valor del resultado obtenido
                            xor dx, dx
                            mov dl, 2
                            PUSH dx ;ponemos tipo 2 de numero
                            jmp TIPOT
                        TIPON: ;hay un numero se opera
                            POP cx ;numero guardado
                            mov operador1, cx ;guardamos en el operador 1
                            mov cx, resultado
                            mov operador2, cx ;guardamos el resultado anterior en operador 2
                            POP cx ;sacamos el tipo 1
                            POP bx ;sacamos la variable en ascci
                            cmp bl, '*'
                            je OPERARMUL
                            cmp bl, '/'
                            je OPERARDIV
                            cmp bl, '+'
                            je OPERARADD
                            cmp bl, '-'
                            je OPERARSUB
                            jmp TERMINAR
                                OPERARMUL:
                                    mov ax, operador1
                                    cwd
                                    mov bx, operador2
                                    imul bx ; ax = ax*bx
                                    mov resultado, ax  
                                    jmp OPERACIONPILA                               
                                OPERARDIV:
                                    mov ax, operador1
                                    cwd
                                    mov bx, operador2
                                    idiv bx ; ax = ax/cx
                                    mov resultado, ax
                                    jmp OPERACIONPILA    
                                OPERARADD:
                                    mov ax, operador1
                                    mov bx, operador2
                                    add ax, bx ; ax = ax + bx 
                                    mov resultado, ax
                                    jmp OPERACIONPILA    
                                OPERARSUB:
                                    mov ax, operador1
                                    mov bx, operador2
                                    sub ax, bx ; ax = ax - bx 
                                    mov resultado, ax
                                    jmp OPERACIONPILA
                        TIPOF:
                            
                            mov ax, resultado
                            ;OPERACIONES SOBRE MENOR Y MAYOR
                            cmp media1, 0 ;si es 0 entonces menor y mayor seran iguales
                            jne SETMAYOR                           
                            mov mayor, ax
                            mov menor, ax
                            jmp MEDIA
                            SETMAYOR:
                                cmp mayor, ax
                                jg SETMAYORN
                                jmp SETMENOR
                                SETMAYORN:
                                    mov mayor, ax
                            SETMENOR:
                                cmp menor, ax
                                jl SETMENORP
                                jmp MEDIA
                                SETMENORP:
                                    mov menor, ax
                            ;OPERACIONES SOBRE LA MEDIA
                            MEDIA:
                            add media0, ax
                            add media1, 1

                            mov di, indiceOp ;para guardar los bit del mayor
                            mov operacionesGuardadas[di], ah
                            inc indiceOp 
                            MOV di, indiceOp ;para guardar los bit del menor
                            mov operacionesGuardadas[di], al
                            inc indiceOp  
                            jmp IGNORAR                        
                        TIPOT:
                            dec si
                            jmp IGNORAR

            OPID:
                ;obtencion de digito de una operacion previa
                xor di, di
                limpiarBuffer claveTemporal
                add si, 5 ;nos posicionamos donde empieza el id
                READKEY:
                mov dl, array[si]
                cmp dl, 34 ;es una comilla termina la cadena
                je GETVALUE
                ;no es una comilla llenamos
                mov claveTemporal[di], dl
                inc di
                inc si
                jmp READKEY
                GETVALUE:
                    inc si
                    obtenerValor operacionesGuardadas, claveTemporal, resultado
                    jmp OPERACIONPILA

        OBJETOPADRE:
            xor di, di
            SAVEPARETN:
            mov consolaReporte[di], dl ;guardamos el caracter leido
            inc si
            inc di
            mov dl, array[si]
            cmp dl, 34 ;si son comillas cerramos el ciclo
            jne SAVEPARETN
            jmp IGNORAR
            ;jmp IDENTIFICADOR

        OBJETOOPERACION:
            ;imprimirCaracter dl
            mov di, indiceOp
            mov operacionesGuardadas[di], dl ;guardamos el caracter leido
            inc indiceOp     
            inc si
            mov dl, array[si]
            cmp dl, 34 ;si son comillas cerramos el ciclo
            jne OBJETOOPERACION
            mov di, indiceOp
            mov operacionesGuardadas[di], 58 ;ponemos un : de quiebre
            inc indiceOp
            jmp IGNORAR
    IGNORAR:
        inc si
        jmp INICIO
    TERMINAR:
        nop
ENDM

crearReporte MACRO handle
    ObtenerFecha
    ObtenerHora
    crearFichero reportejson, handle
    escribirFichero abrirllave, handle
    escribirFichero espaciador, handle
    escribirFichero comillas, handle
    escribirFichero reporte, handle
    escribirFichero comillas, handle
    escribirFichero dospuntos, handle
    escribirFichero saltolinea, handle
    escribirFichero espaciador, handle
    escribirFichero abrirllave, handle
    ;alumno
    escribirFichero espaciador2, handle
    escribirFichero comillas, handle
    escribirFichero alumno, handle
    escribirFichero comillas, handle
    escribirFichero dospuntos, handle
    escribirFichero saltolinea, handle
    escribirFichero espaciador2, handle
    escribirFichero abrirllave, handle
    ;datos alumnto
    ;nombre
    escribirFichero espaciador3, handle
    escribirFichero comillas, handle
    escribirFichero nombre0, handle
    escribirFichero comillas, handle
    escribirFichero dospuntos, handle
    escribirFichero comillas, handle
    escribirFichero nombre1, handle
    escribirFichero comillas, handle
    escribirFichero coma, handle
    escribirFichero saltolinea, handle
    ;carnet
    escribirFichero espaciador3, handle
    escribirFichero comillas, handle
    escribirFichero carnet0, handle
    escribirFichero comillas, handle
    escribirFichero dospuntos, handle
    escribirFichero comillas, handle
    escribirFichero carnet1, handle
    escribirFichero comillas, handle
    escribirFichero coma, handle
    escribirFichero saltolinea, handle
    ;seccion
    escribirFichero espaciador3, handle
    escribirFichero comillas, handle
    escribirFichero seccion0, handle
    escribirFichero comillas, handle
    escribirFichero dospuntos, handle
    escribirFichero comillas, handle
    escribirFichero seccion1, handle
    escribirFichero comillas, handle
    escribirFichero coma, handle
    escribirFichero saltolinea, handle
    ;curso
    escribirFichero espaciador3, handle
    escribirFichero comillas, handle
    escribirFichero curso0, handle
    escribirFichero comillas, handle
    escribirFichero dospuntos, handle
    escribirFichero comillas, handle
    escribirFichero curso1, handle
    escribirFichero comillas, handle
    escribirFichero saltolinea, handle

    escribirFichero espaciador2, handle
    escribirFichero cerrarllave, handle
    escribirFichero coma, handle
    escribirFichero saltolinea, handle
    ;fecha
    escribirFichero espaciador2, handle
    escribirFichero comillas, handle
    escribirFichero fecha, handle
    escribirFichero comillas, handle
    escribirFichero dospuntos, handle
    escribirFichero saltolinea, handle
    escribirFichero espaciador2, handle
    escribirFichero abrirllave, handle
    ;dia
    escribirFichero espaciador3, handle
    escribirFichero comillas, handle
    escribirFichero DIA0, handle
    escribirFichero comillas, handle
    escribirFichero dospuntos, handle    
    escribirFichero dia, handle
    escribirFichero coma, handle    
    escribirFichero saltolinea, handle
    ;mes
    escribirFichero espaciador3, handle
    escribirFichero comillas, handle
    escribirFichero MES0, handle
    escribirFichero comillas, handle
    escribirFichero dospuntos, handle    
    escribirFichero mes, handle
    escribirFichero coma, handle    
    escribirFichero saltolinea, handle
    ;año
    escribirFichero espaciador3, handle
    escribirFichero comillas, handle
    escribirFichero ANIO0, handle
    escribirFichero comillas, handle
    escribirFichero dospuntos, handle    
    escribirFichero anio, handle
    escribirFichero coma, handle    
    escribirFichero saltolinea, handle

    escribirFichero espaciador2, handle
    escribirFichero cerrarllave, handle
    escribirFichero coma, handle
    escribirFichero saltolinea, handle
    ;hora
    escribirFichero espaciador2, handle
    escribirFichero comillas, handle
    escribirFichero HORA0, handle
    escribirFichero comillas, handle
    escribirFichero dospuntos, handle
    escribirFichero saltolinea, handle
    escribirFichero espaciador2, handle
    escribirFichero abrirllave, handle

    ;hora numeral
    escribirFichero espaciador3, handle
    escribirFichero comillas, handle
    escribirFichero HORA0, handle
    escribirFichero comillas, handle
    escribirFichero dospuntos, handle    
    escribirFichero hora, handle
    escribirFichero coma, handle    
    escribirFichero saltolinea, handle
    ;minuto
    escribirFichero espaciador3, handle
    escribirFichero comillas, handle
    escribirFichero MINUTOS, handle
    escribirFichero comillas, handle
    escribirFichero dospuntos, handle    
    escribirFichero minuto, handle
    escribirFichero coma, handle    
    escribirFichero saltolinea, handle

    escribirFichero espaciador2, handle
    escribirFichero cerrarllave, handle
    escribirFichero coma, handle
    escribirFichero saltolinea, handle

    ;resultados
    escribirFichero espaciador2, handle
    escribirFichero comillas, handle
    escribirFichero resultados, handle
    escribirFichero comillas, handle
    escribirFichero dospuntos, handle
    escribirFichero saltolinea, handle
    escribirFichero espaciador2, handle
    escribirFichero abrirllave, handle

    ;media
    limpiarBuffer buffnumero
    escribirFichero espaciador3, handle
    escribirFichero comillas, handle
    escribirFichero reportMedia, handle
    escribirFichero comillas, handle
    escribirFichero dospuntos, handle    
    mov ax, media0
    cwd
    mov bx, media1
    idiv bx ; ax = ax/cx
    mov media3, ax
    int_to_string media3, buffnumero
    ;escribirFichero buffnumero, handle
    PUSH di
    xor di, di        
    M0:
    mov dl, buffnumero[di]
    inc di
    cmp dl, '$'
    je N0
    mov charpars, dl
    escribirFichero charpars, handle
    jmp M0
    N0:
    POP di

    escribirFichero coma, handle    
    escribirFichero saltolinea, handle
    ;menor
    limpiarBuffer buffnumero
    escribirFichero espaciador3, handle
    escribirFichero comillas, handle
    escribirFichero reportMenor, handle
    escribirFichero comillas, handle
    escribirFichero dospuntos, handle  
    int_to_string mayor, buffnumero
    ;escribirFichero buffnumero, handle
    PUSH di
    xor di, di        
    M1:
    mov dl, buffnumero[di]
    inc di
    cmp dl, '$'
    je N1
    mov charpars, dl
    escribirFichero charpars, handle
    jmp M1
    N1:
    POP di
    escribirFichero coma, handle    
    escribirFichero saltolinea, handle
    ;mayor
    limpiarBuffer buffnumero
    escribirFichero espaciador3, handle
    escribirFichero comillas, handle
    escribirFichero reportMayor, handle
    escribirFichero comillas, handle
    escribirFichero dospuntos, handle  
    int_to_string menor, buffnumero
    ;escribirFichero buffnumero, handle
    PUSH di
    xor di, di        
    M2:
    mov dl, buffnumero[di]
    inc di
    cmp dl, '$'
    je N2
    mov charpars, dl
    escribirFichero charpars, handle
    jmp M2
    N2:
    POP di
    escribirFichero coma, handle    
    escribirFichero saltolinea, handle

    escribirFichero espaciador2, handle
    escribirFichero cerrarllave, handle
    escribirFichero coma, handle
    escribirFichero saltolinea, handle

    ;operaciones
    escribirFichero espaciador2, handle
    escribirFichero comillas, handle
    escribirFichero reportOperaciones, handle
    escribirFichero comillas, handle
    escribirFichero dospuntos, handle
    escribirFichero saltolinea, handle
    escribirFichero espaciador2, handle
    escribirFichero abrirCorchete, handle

    escribirFichero espaciador3, handle
    parsearResultados operacionesGuardadas, operacionesParsed, handle
    
    
    escribirFichero saltolinea, handle
    escribirFichero espaciador2, handle
    escribirFichero cerrarCorchete, handle

    escribirFichero saltolinea, handle
    escribirFichero espaciador, handle
    escribirFichero cerrarllave, handle

    escribirFichero saltolinea, handle
    escribirFichero cerrarllave, handle

    ;escribirFichero operacionesParsed, handle
    
    cerrarFichero handle
ENDM

ObtenerHora MACRO
    mov ah, 2ch
    int 21h
    ;procedimiento para la hora; CH
    xor ah, ah
    mov al, ch
    mov bl, 10
    div bl
    add al, 48
    add ah, 48
    mov hora[0], al
    mov hora[1], ah
    ;procedimiento para los minutos CL
    xor ah, ah
    mov al, cl
    mov bl, 10
    div bl
    add al, 48
    add ah, 48
    mov minuto[0], al
    mov minuto[1], ah
ENDM

ObtenerFecha MACRO
    mov ah, 2ah
    int 21h
    ;procedimiento para el dia del mes DL
    xor ah, ah
    mov al, dl
    mov bl, 10
    div bl
    add al, 48
    add ah, 48
    mov dia[0], al
    mov dia[1], ah
    ;procedimiento para el mes DH
    xor ah, ah
    mov al, dh
    mov bl, 10
    div bl
    add al, 48
    add ah, 48
    mov mes[0], al
    mov mes[1], ah
    ;procedimineto para el año CX
    ;para obtener el milenio
    xor dx, dx

    mov ax, cx
    mov cx, 1000
    div cx
    add al, 48
    mov anio[0], al
    ;para obtener la centena
    mov ax, dx
    xor dx, dx
    mov cx, 100
    div cx
    add al, 48
    mov anio[1], al
    ;para obtener la decada
    mov ax, dx
    xor dx, dx
    mov cx, 10
    div cx
    add al, 48
    mov anio[2], al
    ;para los dias el residu
    add dl, 48
    mov anio[3], dl
ENDM

parsearResultados MACRO operaciones, parsing, handle
    LOCAL EQUAL, NEWOP, MATCH, FINISH
    PUSH si
    PUSH di
    PUSH ax
    xor si, si ;si para el array en memoria
    xor di, di ;di para el array de parseo
    xor ax, ax
    NEWOP:
        mov charpars, '{'
        escribirFichero charpars, handle
        mov charpars, '"'
        escribirFichero charpars, handle
        ;mov parsing[di], '{'
        ;inc di
        ;mov parsing[di], '"'
        ;inc di
    EQUAL:
        mov ah, operaciones[si] ;obtenemos un carcater guardado       
        inc si        
        cmp ah, ':' ;cuando obtenemos un : sabemos que hemos llegado al final del string
        je MATCH
        mov charpars, ah
        escribirFichero charpars, handle
        ;mov parsing[di], ah ;lo guardamos en el parseo
        ;inc di
        jmp EQUAL
    MATCH:
        mov ah, operaciones[si]
        inc si
        mov al, operaciones[si]
        inc si
        mov ward, ax
        ;mov value, ax
        mov charpars, '"'
        escribirFichero charpars, handle
        mov charpars, ':'
        escribirFichero charpars, handle

        limpiarBuffer buffnumero
        int_to_string ward, buffnumero


        PUSH di
        xor di, di        
        LNI:
        mov dl, buffnumero[di]
        inc di
        cmp dl, '$'
        je LNF
        mov charpars, dl
        escribirFichero charpars, handle
        jmp LNI
        LNF:
        POP di


        ;escribirFichero buffnumero, handle

        mov charpars, '}'
        escribirFichero charpars, handle

        mov charpars, ','
        escribirFichero charpars, handle

        mov ah, operaciones[si]
        cmp ah, '$'
        je FINISH
        jmp NEWOP
    FINISH:
        nop
        POP ax
        POP di
        POP si
ENDM

.model small
.stack
.data
    cabecera1 db 0ah, 0dh, 'UNIVERSIDAD DE SAN CARLOS DE GUATEMALA', '$'
    cabecera2 db 0ah, 0dh, 'FACULTAD DE INGENIERIA', '$'
    cabecera3 db 0ah, 0dh, 'CIENCIAS Y SISTEMAS', '$'
    cabecera4 db 0ah, 0dh, 'Arquitectura de computadoras y Ensambladores A', '$'
    cabecera5 db 0ah, 0dh, 'NOMBRE: Aldo Rigoberto Hernandez Avila', '$'
    cabecera6 db 0ah, 0dh, 'CARNET: 201800585', '$'
    cabecera7 db 0ah, 0dh, 'SECCION: A', '$'
    menu0 db 0ah, 0dh, '1) CARGAR ARCHIVO', '$'
    menu1 db 0ah, 0dh, '2) CONSOLA', '$'
    menu2 db 0ah, 0dh, '3) SALIR', '$'

    ;=============REPORTE================
    reportejson db 'reporte.jso', 00h
    espaciador db '    '
    espaciador2 db '        '
    espaciador3 db '            '
    saltolinea db 0ah
    abrirllave db '{', 0ah
    cerrarllave db '}'
    abrirCorchete db '[', 0ah
    cerrarCorchete db ']'
    dospuntos db ':'
    comillas db '"'
    coma db ','
    reporte db 'Reporte'
    alumno db 'Alumno'
    nombre0 db 'Nombre'
    nombre1 db 'Aldo Rigoberto Hernandez Avila'
    carnet0 db 'Carnet'
    carnet1 db '201800585'
    seccion0 db 'Seccion'
    seccion1 db 'A'
    curso0 db 'Curso'
    curso1 db 'Arquitectura de computadoras y Ensambladores'
    fecha db 'Fecha'
    DIA0 db 'Dia'
    MES0 db 'Mes'
    ANIO0 db 'Año'
    HORA0 db 'Hora'
    MINUTOS db 'Minutos'
    resultados db 'Resultados'
    reportMedia db 'Media'
    reportMenor db 'Menor'
    reportMayor db 'Mayor'
    reportOperaciones db 'Operaciones'
    operacionesParsed db 1000 dup('$')
    charpars db 0

    ;=============FECHA Y HORA===========
    hora db 2 dup('$')
    minuto db 2 dup('$')

    dia db 2 dup('$')
    mes db 2 dup('$')
    anio db 4 dup('$')

    ;=============MENSAJES===============
    succes db 0ah, 0dh, 'EL resultado es: ' ,'$'
    command0 db 0ah, 0dh, '>> ', '$'
    readFile0 db 0ah, 0dh, 'Ruta del archivo >> ', '$'
    readFile1 db 0ah, 0dh, 'El archivo se ha cargado con exito!', 0ah, 0dh, '$'
    writeFile db 'Reporte creado!', 0ah, 0dh , '$'


    buffnumero db 4 dup('$'), '$'

    ward dw ?
    compA2 dw 0

    contadorLlave db 0
    stringigualdad db 0
    ;===========VARIABLES A UTILIZAR PARA CALCULAR LA MEDIA
    media0 dw 0 ;contiene la suma de resultados
    media1 dw 0 ;contiene el numero de resultados
    media3 dw 0 ;contiene el resultado
    ;===========VARIABLES A UTILIZAR PARA MENOR Y MAYOR
    menor dw 0
    mayor dw 0
    ;se utilizara media1 para determinar el primer dato
    ;===========SE UTILIZAN PARA EL CACULO DE OPERACIONES
    operador1 dw ?
    operador2 dw ?
    resultado dw ?
    operacionesGuardadas db 1000 dup('$'), '$' ;en este array se van guardando los resultados de las n operaciones
   

    ;===========CADENAS DE USO GENERICO
    cadena0 db 100 dup('$'), '$'
    cadena1 db 100 dup('$'), '$'

    ;===========VARIABLES PARA EL USO DE LA CONSOLA
    consolaMedia db 'media', '$'
    consolaModa db 'moda', '$'
    consolaMediana db 'mediana', '$'
    consolaMayor db 'mayor', '$'
    consolaMenor db 'menor', '$'
    consolaReporte db 100 dup('$'), '$'
    claveTemporal db 100 dup('$'), '$'
    
    indiceOp dw 0
    archivoEntrada db 100 dup('$')
    datosjson db 10000 dup('$')
    handleArchivo dw ?
.code

;PROCESO DE
main PROC
    mov ax, @data
    mov ds, ax
        
    MENU:
        encabezado
        imprimirCaracter 10d
        obtenerCaracter
        cmp al, '1'
        je CARGARCHIVO
        cmp al, '2'
        je CONSOLA
        cmp al, '3'
        je SALIR
    CONSOLA:

        imprimirCadena command0
        obtenerCaracter
        cmp al, 's'
        je SHOWC
        jmp EXITC
        SHOWC:
            obtenerCaracter ;h
            obtenerCaracter ;o
            obtenerCaracter ;w
            obtenerCaracter ; (ESPACIO)
            obtenerCadena_consola cadena0
            ;=======COMPARAMOS CON MEDIA
            compare_string cadena0, consolaMedia, stringigualdad
            cmp stringigualdad, 1
            je SHOWMEDIA
            ;=======COMPARAMOS CON MEDIANA
            compare_string cadena0, consolaMediana, stringigualdad
            cmp stringigualdad, 1
            je SHOWMEDIANA
            ;=======COMPARAMOS CON MODA
            compare_string cadena0, consolaModa, stringigualdad
            cmp stringigualdad, 1
            je SHOWMODA
            ;=======COMPARAMOS CON MENOR
            compare_string cadena0, consolaMenor, stringigualdad
            cmp stringigualdad, 1
            je SHOWMENOR
            ;=======COMPARAMOS CON MAYOR
            compare_string cadena0, consolaMayor, stringigualdad
            cmp stringigualdad, 1
            je SHOWMAYOR
            ;=======COMPARAMOS CON REPORTES
            compare_string cadena0, consolaReporte, stringigualdad
            cmp stringigualdad, 1
            je SHOWPARENT
            ;=======ES UN ID
            jmp SHOWID

            SHOWMEDIA:
                mov ax, media0
                cwd
                mov bx, media1
                idiv bx ; ax = ax/cx
                mov media3, ax
                int_to_string media3, buffnumero
                imprimirCadena succes
                imprimirCadena buffnumero
                obtenerCaracter
                ;jmp OPERACIONPILA 
                jmp CONSOLA
            SHOWMEDIANA:
                imprimirCaracter 66
                jmp CONSOLA
            SHOWMODA:
                imprimirCaracter 67
                jmp CONSOLA
            SHOWMENOR:
                int_to_string mayor, buffnumero
                imprimirCadena succes
                imprimirCadena buffnumero
                obtenerCaracter
                jmp CONSOLA
            SHOWMAYOR:
                int_to_string menor, buffnumero
                imprimirCadena succes
                imprimirCadena buffnumero
                obtenerCaracter
                jmp CONSOLA
            SHOWPARENT:
                crearReporte handleArchivo
                ;imprimirCaracter 70
                imprimirCadena writeFile
                jmp CONSOLA
            SHOWID:
                obtenerValor operacionesGuardadas, cadena0, ward
                int_to_string ward, buffnumero
                imprimirCadena succes
                imprimirCadena buffnumero
                obtenerCaracter
                jmp CONSOLA

        EXITC:
            obtenerCaracter ;x
            obtenerCaracter ;i
            obtenerCaracter ;t
            obtenerCaracter ;enter y jmp
            jmp SALIR
        
        obtenerCaracter
        jmp MENU;
    CARGARCHIVO:
        imprimirCadena readFile0
        obtenerCadena archivoEntrada
        abrirFichero archivoEntrada, handleArchivo
        leerFichero datosjson, handleArchivo       
        recorrerArray datosjson, buffnumero
        imprimirCadena readFile1
        obtenerCaracter
        jmp MENU
    ERRORCREARF:
        ;imprimirCaracter 48d
    ERRORESCRIBIRF:
        ;imprimirCaracter 49d
    SALIR:
        mov ah, 4ch
        int 21h
main ENDP
end main

