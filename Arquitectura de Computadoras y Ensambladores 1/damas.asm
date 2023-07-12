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

imprimirFila MACRO filaSeleccionada
    LOCAL FILA, BLANCA, NEGRA, VACIO, INCREMENTO, FINAL
    mov di, 0 ;indice a 0
    imprimirCadena separador ;separador primario 
    FILA:
        xor al, al ;limpiamos el registro al
        mov al, filaSeleccionada[di] ;copiamos en dx lo que tengo en la fila
        ;----------------SECCION DE SELECCION---------------
        cmp al, 0 ; se imprime un espacio vacio
        je VACIO
        cmp al, 1 ; se imprime una ficha blanca
        je BLANCA
        cmp al, 2 ; se imprime una ficha negra
        je NEGRA

        ;------------ESPACIO DE PRINT FICHA BLANCA----------
        BLANCA:
            imprimirCadena fichasBlancas   
            imprimirCadena separador         
            jmp INCREMENTO
        ;------------ESPACIO DE PRINT FICHA NEGRA-----------
        NEGRA:
            imprimirCadena fichasNegras
            imprimirCadena separador   
            jmp INCREMENTO
        ;------------ESPACIO DE PRINT FICHA VACIA-----------
        VACIO:
            imprimirCadena fichasVacias 
            imprimirCadena separador
            jmp INCREMENTO

        ;------------ESPACIO MANEJO DEL LOOP----------
        INCREMENTO:
            inc di ; se incrementa di en uno
            cmp di, 8; comparo si di es 8
            je FINAL; si es 8 se salta al final
            jmp FILA; si no es 8 repite el proceso
    FINAL:
        nop        
ENDM

imprimirTablero MACRO
    ;FILA0
    imprimirCadena cero
    imprimirCadena grafico0
    imprimirCaracter 10d
    imprimirCadena ocho
    imprimirFila fila0
    imprimirCaracter 10d
    ;FILA1
    imprimirCadena cero
    imprimirCadena grafico0
    imprimirCaracter 10d
    imprimirCadena siete
    imprimirFila fila1
    imprimirCaracter 10d
    ;FILA2
    imprimirCadena cero
    imprimirCadena grafico0
    imprimirCaracter 10d
    imprimirCadena seis
    imprimirFila fila2
    imprimirCaracter 10d
    ;FILA3
    imprimirCadena cero
    imprimirCadena grafico0
    imprimirCaracter 10d
    imprimirCadena cinco
    imprimirFila fila3
    imprimirCaracter 10d
    ;FILA4
    imprimirCadena cero
    imprimirCadena grafico0
    imprimirCaracter 10d
    imprimirCadena cuatro
    imprimirFila fila4
    imprimirCaracter 10d
    ;FILA5
    imprimirCadena cero
    imprimirCadena grafico0
    imprimirCaracter 10d
    imprimirCadena tres
    imprimirFila fila5
    imprimirCaracter 10d
    ;FILA6
    imprimirCadena cero
    imprimirCadena grafico0
    imprimirCaracter 10d
    imprimirCadena dos
    imprimirFila fila6
    imprimirCaracter 10d
    ;FILA7
    imprimirCadena cero
    imprimirCadena grafico0
    imprimirCaracter 10d
    imprimirCadena uno
    imprimirFila fila7
    imprimirCaracter 10d
    ;Referencia
    imprimirCadena cero
    imprimirCadena grafico0
    imprimirCaracter 10d
    imprimirCadena cero
    imprimirCadena grafico1
    imprimirCaracter 10d
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

escribirHTML MACRO handle
    crearFichero estadoTablero, handle
    escribirFichero table0, handle ;<style>
    escribirFichero table1, handle ;<div>
    escribirFichero table3, handle ;<table>
    htmlFila fila0, handle
    htmlFila fila1, handle
    htmlFila fila2, handle
    htmlFila fila3, handle
    htmlFila fila4, handle
    htmlFila fila5, handle
    htmlFila fila6, handle
    htmlFila fila7, handle
    escribirFichero table4, handle ;</table>
    escribirFichero table2, handle ;</div>

    escribirFichero table1, handle ;<div>
    escribirFichero table9, handle ;<h4>
    ObtenerHora
    escribirFichero hora, handle ;hh
    escribirFichero signos0, handle ;:
    escribirFichero minuto, handle ;MM
    escribirFichero signos2, handle ; - 
    ObtenerFecha
    escribirFichero dia, handle ;DD
    escribirFichero signos1, handle ;/
    escribirFichero mes, handle ;MM
    escribirFichero signos1, handle ;/
    escribirFichero anio, handle ;AAAA
    escribirFichero table10, handle ;</h4>
    escribirFichero table2, handle ;</div>
    cerrarFichero handle

ENDM

htmlFila MACRO hfilaSeleccionada, handle
    LOCAL hFILA, hBLANCA, hNEGRA, hVACIO, hINCREMENTO, hFINAL
    mov di, 0 ;indice a 0
    hFILA:
        xor al, al ;limpiamos el registro al
        mov al, hfilaSeleccionada[di] ;copiamos en dx lo que tengo en la fila
        ;----------------SECCION DE SELECCION---------------
        cmp al, 0 ; se imprime un espacio vacio
        je hVACIO
        cmp al, 1 ; se imprime una ficha blanca
        je hBLANCA
        cmp al, 2 ; se imprime una ficha negra
        je hNEGRA

        escribirFichero table6, handle                   ;<tr>
        ;------------ESPACIO DE PRINT FICHA BLANCA----------
        hBLANCA:
            escribirFichero table7, handle               ;<td>
            escribirFichero fichaHtml0, handle        ;FB
            escribirFichero table8, handle               ;</td>
            jmp hINCREMENTO
        ;------------ESPACIO DE PRINT FICHA NEGRA-----------
        hNEGRA:
            escribirFichero table7, handle               ;<td>
            escribirFichero fichaHtml1, handle         ;FN
            escribirFichero table8, handle               ;</td> 
            jmp hINCREMENTO
        ;------------ESPACIO DE PRINT FICHA VACIA-----------
        hVACIO:
            escribirFichero table7, handle               ;<td>
            escribirFichero table8, handle               ;</td> 
            jmp hINCREMENTO

        ;------------ESPACIO MANEJO DEL LOOP----------
        hINCREMENTO:
            inc di ; se incrementa di en uno
            cmp di, 8; comparo si di es 8
            je hFINAL; si es 8 se salta al final
            jmp hFILA; si no es 8 repite el proceso
    hFINAL:
        escribirFichero table6, handle                   ;</tr>
        nop 
ENDM


;===============MODULO DE JUEGO TURNO==================//
rebootTablero MACRO
    ;fila0 db 1, 0, 1, 0, 1, 0, 1, 0
    mov fila0[0], 1
    mov fila0[1], 0
    mov fila0[2], 1
    mov fila0[3], 0
    mov fila0[4], 1
    mov fila0[5], 0
    mov fila0[6], 1
    mov fila0[7], 0

    ;fila1 db 0, 1, 0, 1, 0, 1, 0, 1
    mov fila1[0], 0
    mov fila1[1], 1
    mov fila1[2], 0
    mov fila1[3], 1
    mov fila1[4], 0
    mov fila1[5], 1
    mov fila1[6], 0
    mov fila1[7], 1

    ;fila2 db 1, 0, 1, 0, 1, 0, 1, 0
    mov fila2[0], 1
    mov fila2[1], 0
    mov fila2[2], 1
    mov fila2[3], 0
    mov fila2[4], 1
    mov fila2[5], 0
    mov fila2[6], 1
    mov fila2[7], 0

    ;fila3 db 0, 0, 0, 0, 0, 0, 0, 0
    mov fila3[0], 0
    mov fila3[1], 0
    mov fila3[2], 0
    mov fila3[3], 0
    mov fila3[4], 0
    mov fila3[5], 0
    mov fila3[6], 0
    mov fila3[7], 0

    ;fila4 db 0, 0, 0, 0, 0, 0, 0, 0
    mov fila4[0], 0
    mov fila4[1], 0
    mov fila4[2], 0
    mov fila4[3], 0
    mov fila4[4], 0
    mov fila4[5], 0
    mov fila4[6], 0
    mov fila4[7], 0

    ;fila5 db 0, 2, 0, 2, 0, 2, 0, 2
    mov fila5[0], 0
    mov fila5[1], 2
    mov fila5[2], 0
    mov fila5[3], 2
    mov fila5[4], 0
    mov fila5[5], 2
    mov fila5[6], 0
    mov fila5[7], 2

    ;fila6 db 2, 0, 2, 0, 2, 0, 2, 0
    mov fila6[0], 2
    mov fila6[1], 0
    mov fila6[2], 2
    mov fila6[3], 0
    mov fila6[4], 2
    mov fila6[5], 0
    mov fila6[6], 2
    mov fila6[7], 0

    ;fila7 db 0, 2, 0, 2, 0, 2, 0, 2
    mov fila7[0], 0
    mov fila7[1], 2
    mov fila7[2], 0
    mov fila7[3], 2
    mov fila7[4], 0
    mov fila7[5], 2
    mov fila7[6], 0
    mov fila7[7], 2
ENDM

guardarPartida MACRO 
   xor di, di
   guardarFila fila0, 48
   guardarFila fila1, 49
   guardarFila fila2, 50
   guardarFila fila3, 51
   guardarFila fila4, 52
   guardarFila fila5, 53
   guardarFila fila6, 54
   guardarFila fila7, 55
   xor di, di
ENDM

guardarFila MACRO filaSeleccionada, fila
    ;indice 0
    mov partida[di], fila
    inc di
    mov dl, filaSeleccionada[0]
    add dl, 48
    mov partida[di], dl
    inc di

    ;indice 1
    mov partida[di], fila
    inc di
    mov dl, filaSeleccionada[1]
    add dl, 48
    mov partida[di], dl
    inc di

    ;indice 2
    mov partida[di], fila
    inc di
    mov dl, filaSeleccionada[2]
    add dl, 48
    mov partida[di], dl
    inc di

    ;indice 3
    mov partida[di], fila
    inc di
    mov dl, filaSeleccionada[3]
    add dl, 48
    mov partida[di], dl
    inc di

    ;indice 4
    mov partida[di], fila
    inc di
    mov dl, filaSeleccionada[4]
    add dl, 48
    mov partida[di], dl
    inc di

    ;indice 5
    mov partida[di], fila
    inc di
    mov dl, filaSeleccionada[5]
    add dl, 48
    mov partida[di], dl
    inc di

    ;indice 6
    mov partida[di], fila
    inc di
    mov dl, filaSeleccionada[6]
    add dl, 48
    mov partida[di], dl
    inc di

    ;indice 7
    mov partida[di], fila
    inc di
    mov dl, filaSeleccionada[7]
    add dl, 48
    mov partida[di], dl
    inc di

    
ENDM

cargarPartida MACRO buffer
    xor di, di;dejamos el contador a 0
    xor si, si;otro contador a 0

    SACARFILA:
        mov dl, buffer[di] ;el dato de buffer lo guardamos en dl el dato correspondiente a la fila
        sub dl, 48
        cmp dl, 0 ;si corresponde a 0 un jmp
        je SACARCOLUMNA0
        cmp dl, 1
        je SACARCOLUMNA1
        cmp dl, 2
        je SACARCOLUMNA2
        cmp dl, 3
        je SACARCOLUMNA3
        cmp dl, 4
        je SACARCOLUMNA4
        cmp dl, 5
        je SACARCOLUMNA5
        cmp dl, 6
        je SACARCOLUMNA6
        cmp dl, 7
        je SACARCOLUMNA7
        JMP TERMINARCARGA

    SACARCOLUMNA0:
        inc di
        mov dl, buffer[di] ;guardamos el dato correspondiente a la ficha ; '1'
        sub dl, 48 ;restamos al registro obtenido para tener 1 a secas
        mov fila0[si], dl ;ponemos en fila 0 el valor obtenido en dl
        inc di ;incrementamos en uno di para el siguiente dato del array
        inc si ;incremantemos en uno si para el siguiente dato de la fila
        jmp CONDICIONES ;recursividad

    SACARCOLUMNA1:
        inc di
        mov dl, buffer[di] ;guardamos el dato correspondiente a la ficha ; '1'
        sub dl, 48 ;restamos al registro obtenido para tener 1 a secas
        mov fila1[si], dl ;ponemos en fila 0 el valor obtenido en dl
        inc di ;incrementamos en uno di para el siguiente dato del array
        inc si ;incremantemos en uno si para el siguiente dato de la fila
        jmp CONDICIONES ;recursividad

    SACARCOLUMNA2:
        inc di
        mov dl, buffer[di] ;guardamos el dato correspondiente a la ficha ; '1'
        sub dl, 48 ;restamos al registro obtenido para tener 1 a secas
        mov fila2[si], dl ;ponemos en fila 0 el valor obtenido en dl
        inc di ;incrementamos en uno di para el siguiente dato del array
        inc si ;incremantemos en uno si para el siguiente dato de la fila
        jmp CONDICIONES ;recursividad
    
    SACARCOLUMNA3:
        inc di
        mov dl, buffer[di] ;guardamos el dato correspondiente a la ficha ; '1'
        sub dl, 48 ;restamos al registro obtenido para tener 1 a secas
        mov fila3[si], dl ;ponemos en fila 0 el valor obtenido en dl
        inc di ;incrementamos en uno di para el siguiente dato del array
        inc si ;incremantemos en uno si para el siguiente dato de la fila
        jmp CONDICIONES ;recursividad
    
    SACARCOLUMNA4:
        inc di
        mov dl, buffer[di] ;guardamos el dato correspondiente a la ficha ; '1'
        sub dl, 48 ;restamos al registro obtenido para tener 1 a secas
        mov fila4[si], dl ;ponemos en fila 0 el valor obtenido en dl
        inc di ;incrementamos en uno di para el siguiente dato del array
        inc si ;incremantemos en uno si para el siguiente dato de la fila
        jmp CONDICIONES ;recursividad
    
    SACARCOLUMNA5:
        inc di
        mov dl, buffer[di] ;guardamos el dato correspondiente a la ficha ; '1'
        sub dl, 48 ;restamos al registro obtenido para tener 1 a secas
        mov fila5[si], dl ;ponemos en fila 0 el valor obtenido en dl
        inc di ;incrementamos en uno di para el siguiente dato del array
        inc si ;incremantemos en uno si para el siguiente dato de la fila
        jmp CONDICIONES ;recursividad

    SACARCOLUMNA6:
        inc di
        mov dl, buffer[di] ;guardamos el dato correspondiente a la ficha ; '1'
        sub dl, 48 ;restamos al registro obtenido para tener 1 a secas
        mov fila6[si], dl ;ponemos en fila 0 el valor obtenido en dl
        inc di ;incrementamos en uno di para el siguiente dato del array
        inc si ;incremantemos en uno si para el siguiente dato de la fila
        jmp CONDICIONES ;recursividad

    SACARCOLUMNA7:
        inc di
        mov dl, buffer[di] ;guardamos el dato correspondiente a la ficha ; '1'
        sub dl, 48 ;restamos al registro obtenido para tener 1 a secas
        mov fila7[si], dl ;ponemos en fila 0 el valor obtenido en dl
        inc di ;incrementamos en uno di para el siguiente dato del array
        inc si ;incremantemos en uno si para el siguiente dato de la fila
        jmp CONDICIONES ;recursividad

    CONDICIONES:
        CONTADORFILA:
            cmp si, 8 ;si el contador de indice si es 8 se formateo
            je LIMPIARSI
            jmp CONTADORALL
            LIMPIARSI:
                xor si, si ; limpiamos si
        CONTADORALL:
            cmp di, 128 ; si el contador del array llega a 128 se termina el proceso
            je TERMINARCARGA
            jmp SACARFILA
        TERMINARCARGA:
            xor di, di
            xor si, si
            nop
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
    menu0 db 0ah, 0dh, '1) Iniciar Juego', '$'
    menu1 db 0ah, 0dh, '2) Cargar Juego', '$'
    menu2 db 0ah, 0dh, '3) Salir', '$'

    gamemsg0 db 0ah, 0dh, 'Turno Negras: ', '$'
    gamemsg1 db 0ah, 0dh, 'Turno Blancas: ', '$'
    set db 0

    table0 db '<style>table{background-image: url("tablero.png");background-size: 100% 100%;background-position: center center;border:10px solid #c7572f;}td{height: 60px;width: 60px;}img{width: 100%;}</style>',0ah, 0dh
    table1 db '<div align="center">',0ah, 0dh
    table2 db '</div>',0ah, 0dh
    table3 db '<table>',0ah, 0dh
    table4 db '</table>',0ah, 0dh
    table5 db '<tr>',0ah, 0dh
    table6 db '</tr>',0ah, 0dh
    table7 db '<td>'
    table8 db '</td>', 0ah, 0dh
    table9 db '<h4>', 0ah, 0dh
    table10 db '</h4>', 0ah, 0dh
    fichaHtml0 db '<img src="fichaBlanca.png">'
    fichaHtml1 db '<img src="fichaNegra.png">'
    signos0 db ':'
    signos1 db '/'
    signos2 db ' - '

    error0 db 0ah, 0dh, 'Error en el manejo de Archivos', '$'

    msg0 db 'Ruta del Archivo > ', '$'
    msg1 db 'La ruta Escrita fue: ','$'
    msg2 db 'Escoga entre los dos parametros:   ', '$'
    msg3 db 'El Tablero se ha generado!', '$'
    msg4 db 'El Juego se ha guardado con exito!', '$'

    archive db 'El Reporte se genero con Exito: ','$'

    fila0 db 1, 0, 1, 0, 1, 0, 1, 0
    fila1 db 0, 1, 0, 1, 0, 1, 0, 1
    fila2 db 1, 0, 1, 0, 1, 0, 1, 0
    fila3 db 0, 0, 0, 0, 0, 0, 0, 0
    fila4 db 0, 0, 0, 0, 0, 0, 0, 0
    fila5 db 0, 2, 0, 2, 0, 2, 0, 2
    fila6 db 2, 0, 2, 0, 2, 0, 2, 0
    fila7 db 0, 2, 0, 2, 0, 2, 0, 2

    partida db 128 dup('$')

    hora db 2 dup('$')
    minuto db 2 dup('$')

    dia db 2 dup('$')
    mes db 2 dup('$')
    anio db 4 dup('$')

    SobreColumna db 0
    SobreFila db 0
    
    gamerror0 db 'No puede realizar este movimiento', 0ah, 0dh, '$'
    gamerror1 db 'Comando no reconocido o incorrecto', 0ah, 0dh, '$'

    cero db '    ', '$'
    uno  db '1   ', '$'
    dos  db '2   ', '$'
    tres  db '3   ', '$'
    cuatro  db '4   ', '$'
    cinco  db '5   ', '$'
    seis  db '6   ', '$'
    siete  db '7   ', '$'
    ocho  db '8   ', '$'

    letraA db 'A', '$'
    letraB db 'B', '$'
    letraC db 'C', '$'
    letraD db 'D', '$'
    letraF db 'F', '$'
    letraG db 'G', '$'
    letraH db 'H', '$'



    grafico0 db '-------------------------', '$'
    grafico1 db '  A  B  C  D  E  F  G  H', '$'
    separador db '|', '$'
    fichasVacias db '  ', '$'
    fichasBlancas db 'FB', '$'
    fichasNegras db 'FN', '$'

    estadoTablero db 'estado.htm', 00h
    rutaArchivo db 100 dup('$')
    rutaArchivoLectura db 100 dup('$')
    handleArchivo dw ?

    recoveryFila dw ?
    recoveryColumna dw ?
.code

;description
main PROC
        mov ax,@data
        mov ds,ax     
    INICIO:
        encabezado
        mov dh, 10
        imprimirCaracter dh
        obtenerCaracter
        cmp al, '1'
        jne Q0 
        jmp JUEGO
        Q0:
        cmp al, '2'
        jne Q1
        jmp CARGAR
        Q1:
        cmp al, '3'
        jne Q2
        jmp SALIR
        Q2:
        jmp INICIO
    JUEGO:
        imprimirCadena gamemsg1
        imprimirCaracter 10d
        jmp TurnoN
        jmp SALIR
    CARGAR:
        imprimirCaracter 10d
        imprimirCadena msg0
        obtenerCadena rutaArchivoLectura
        abrirFichero rutaArchivoLectura, handleArchivo
        leerFichero partida, handleArchivo
        cerrarFichero handleArchivo
        cargarPartida partida
        imprimirCadena gamemsg1
        imprimirCaracter 10d
        jmp TurnoN
        jmp INICIO
    SALIR:
        mov ah, 04ch
        int 21h
    ERRORCREARF:
    ERRORESCRIBIRF:
        imprimirCadena error0
        jmp SALIR 

    TurnoN:
        imprimirTablero ;Imprimimos el tablero actual
        ;imprimirCadena gamemsg0 ; imprimos el mensaje de peticion de comando
        xor di, di
        xor ax, ax
        xor dx, dx

        PILOTON:
            obtenerCaracter ;interface del primer caracter
            cmp al, 69
            je EXITN ;caso E
            cmp al, 83
            je SCODEN ;caso S
            jmp MOVIMIENTON2 ;otro caso

        EXITN:
            ;guardamos el valor leido por si acaso se trata de un movimiento
            mov SobreColumna, al
            sub al, 65
            xor ah, ah
            mov di, ax
            PUSH di; guardamos el valor que en pila que corresponde a la columna E

            obtenerCaracter ;interface de segundo caracter en caso exit X
            cmp al, 88
            jne MOVIMIENTON1 ;no es ningun comando exit enviamos al movimiento ,LN
            ;suponiendo que seguimos en exit ;liberamos lo que tengamos en pila
            POP di
            xor di, di
            obtenerCaracter ;leemos I
            obtenerCaracter ;leemos T
            obtenerCaracter ;modulo de espera
            rebootTablero
            ;procedemos hacer todo lo que corresponda a exit
            jmp INICIO
        SCODEN:
            obtenerCaracter ;leemos y redireccionamos si es A o H
            cmp al, 65
            je SAVEN
            cmp al, 72
            je SHOWN
            jmp GAMEERRORCM
        SAVEN:
            obtenerCaracter ;leemos V
            obtenerCaracter ;leemos E
            obtenerCaracter ;caracter de espera
            imprimirCaracter 10d
            imprimirCadena msg0 ;escriba la ruta > 
            obtenerCadena rutaArchivo
            guardarPartida
            crearFichero rutaArchivo, handleArchivo
            escribirFichero partida, handleArchivo
            cerrarFichero handleArchivo
            imprimirCaracter 10d
            imprimirCadena msg4
            imprimirCaracter 10d
            obtenerCaracter ;caracter de espera
            jmp COMPLEMENTON
            ;procedemos hacer todo lo que corresponda a save
        SHOWN:
            obtenerCaracter ;leemos O
            obtenerCaracter ;leemos W
            obtenerCaracter
            escribirHTML handleArchivo
            imprimirCaracter 10d
            imprimirCadena msg3
            imprimirCaracter 10d
            obtenerCaracter ;caracter de espera
            jmp COMPLEMENTON
        MOVIMIENTON1: ;viniendo de exit se establece el segundo caracter
            mov SobreFila, al
            xor ah, ah ;guardamos el valor que no fue X
            PUSH ax 
            jmp COMAN
        MOVIMIENTON2: ;este siempre se asume que iniciara con el destino
            ;obtenerCaracter
            mov SobreColumna, al ;obtenemos la primera columna, ej, B = 66
            sub al, 65
            xor ah, ah
            mov di, ax
            PUSH di; guardamos el valor que en pila que corresponde a la columna E
            
            ;fila es el que indicara que fila modificar
            obtenerCaracter
            mov SobreFila, al
            xor ah, ah ; limpiamos lo que tenga el bit mayor de 
            PUSH ax ; guardamos el registro ax que contiene el valor de la fila

        COMAN:
            obtenerCaracter
            cmp al, ','
            je FUENTEN
            ;un solo Parametro
            POP ax
            mov recoveryFila, ax ;guardamos ax que contiene la fila del destino
            POP di
            mov recoveryColumna, di ;guardamos en di el indice del destino 

            cmp set, 0 ; estan jugando las blancas
            je DESTB
            cmp set, 1 ; estan jugando las negras
            je DESTN
                DESTB:
                    ; juegan las blancas, sumamos a la fila 1
                    add al, 1d       
                    cmp al, '2'
                    je DB2
                    cmp al, '3'
                    je DB3
                    cmp al, '4'
                    je DB4
                    cmp al, '5'
                    je DB5
                    cmp al, '6'
                    je DB6
                    cmp al, '7'
                    je DB7
                    cmp al, '8'
                    je DB8
                    jmp GAMEERRORCM

                    DB2: ;DESTINO EN FILA7 FUENTE EN FILA 6
                        mov si, di
                        sub di, 1 ; para buscar a la izquierda
                        add si, 1 ; para buscar a la derecha

                        cmp fila6[di], 1 ;buscando izquierda
                        je DB2IT
                        jmp DB2IF
                            DB2IT:
                                cmp fila6[si], 1 ;buscando la derecha
                                je DB2DT1
                                jmp DB2DF1
                                DB2DT1:
                                    ;HAY EN LA IZQUIERDA Y EN LA DERECHA
                                    imprimirCadena msg2
                                    mov bx, di
                                    mov cx, si
                                    add bl, 65
                                    add cl, 65
                                    imprimirCaracter bl
                                    imprimirCadena dos
                                    imprimirCaracter cl
                                    imprimirCadena dos
                                    ;volvemos apilar el destino
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    jmp FUENTEN
                                DB2DF1:
                                    ;HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH di
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA    
                            DB2IF:
                                cmp fila6[si], 1 ;buscando la derecha
                                je DB2DT2
                                jmp DB2DF2
                                DB2DT2:
                                    ;NO HAY EN LA IZQUIERDA Y HAY EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH si
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA
                                DB2DF2:
                                    ;NO HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    jmp GAMERRORMOV

                    DB3: ;DESTINO EN fila6 FUENTE EN FILA 5      
                        mov si, di
                        sub di, 1 ; para buscar a la izquierda
                        add si, 1 ; para buscar a la derecha

                        cmp fila5[di], 1 ;buscando izquierda
                        je DB3IT
                        jmp DB3IF
                            DB3IT:
                                cmp fila5[si], 1 ;buscando la derecha
                                je DB3DT1
                                jmp DB3DF1
                                DB3DT1:
                                    ;HAY EN LA IZQUIERDA Y EN LA DERECHA
                                    imprimirCadena msg2
                                    mov bx, di
                                    mov cx, si
                                    add bl, 65
                                    add cl, 65
                                    imprimirCaracter bl
                                    imprimirCadena tres
                                    imprimirCaracter cl
                                    imprimirCadena tres
                                    ;volvemos apilar el destino
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    jmp FUENTEN
                                DB3DF1:
                                    ;HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH di
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA    
                            DB3IF:
                                cmp fila5[si], 1 ;buscando la derecha
                                je DB3DT2
                                jmp DB3DF2
                                DB3DT2:
                                    ;NO HAY EN LA IZQUIERDA Y HAY EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH si
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA
                                DB3DF2:
                                    ;NO HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    jmp GAMERRORMOV

                    DB4: ;DESTINO EN fila5 FUENTE EN FILA 4
                        mov si, di
                        sub di, 1 ; para buscar a la izquierda
                        add si, 1 ; para buscar a la derecha

                        cmp fila4[di], 1 ;buscando izquierda
                        je DB4IT
                        jmp DB4IF
                            DB4IT:
                                cmp fila4[si], 1 ;buscando la derecha
                                je DB4DT1
                                jmp DB4DF1
                                DB4DT1:
                                    ;HAY EN LA IZQUIERDA Y EN LA DERECHA
                                    imprimirCadena msg2
                                    mov bx, di
                                    mov cx, si
                                    add bl, 65
                                    add cl, 65
                                    imprimirCaracter bl
                                    imprimirCadena cuatro
                                    imprimirCaracter cl
                                    imprimirCadena cuatro
                                    ;volvemos apilar el destino
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    jmp FUENTEN
                                DB4DF1:
                                    ;HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH di
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA    
                            DB4IF:
                                cmp fila4[si], 1 ;buscando la derecha
                                je DB4DT2
                                jmp DB4DF2
                                DB4DT2:
                                    ;NO HAY EN LA IZQUIERDA Y HAY EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH si
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA
                                DB4DF2:
                                    ;NO HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    jmp GAMERRORMOV
                    DB5: ;DESTINO EN fila4 FUENTE EN FILA 3
                        mov si, di
                        sub di, 1 ; para buscar a la izquierda
                        add si, 1 ; para buscar a la derecha

                        cmp fila3[di], 1 ;buscando izquierda
                        je DB5IT
                        jmp DB5IF
                            DB5IT:
                                cmp fila3[si], 1 ;buscando la derecha
                                je DB5DT1
                                jmp DB5DF1
                                DB5DT1:
                                    ;HAY EN LA IZQUIERDA Y EN LA DERECHA
                                    imprimirCadena msg2
                                    mov bx, di
                                    mov cx, si
                                    add bl, 65
                                    add cl, 65
                                    imprimirCaracter bl
                                    imprimirCadena cinco
                                    imprimirCaracter cl
                                    imprimirCadena cinco
                                    ;volvemos apilar el destino
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    jmp FUENTEN
                                DB5DF1:
                                    ;HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH di
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA    
                            DB5IF:
                                cmp fila3[si], 1 ;buscando la derecha
                                je DB5DT2
                                jmp DB5DF2
                                DB5DT2:
                                    ;NO HAY EN LA IZQUIERDA Y HAY EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH si
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA
                                DB5DF2:
                                    ;NO HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    jmp GAMERRORMOV
                    DB6: ;DESTINO EN fila3 FUENTE EN FILA 2
                        mov si, di
                        sub di, 1 ; para buscar a la izquierda
                        add si, 1 ; para buscar a la derecha

                        cmp fila2[di], 1 ;buscando izquierda
                        je DB6IT
                        jmp DB6IF
                            DB6IT:
                                cmp fila2[si], 1 ;buscando la derecha
                                je DB6DT1
                                jmp DB6DF1
                                DB6DT1:
                                    ;HAY EN LA IZQUIERDA Y EN LA DERECHA
                                    imprimirCadena msg2
                                    mov bx, di
                                    mov cx, si
                                    add bl, 65
                                    add cl, 65
                                    imprimirCaracter bl
                                    imprimirCadena seis
                                    imprimirCaracter cl
                                    imprimirCadena seis
                                    ;volvemos apilar el destino
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    jmp FUENTEN
                                DB6DF1:
                                    ;HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH di
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA    
                            DB6IF:
                                cmp fila2[si], 1 ;buscando la derecha
                                je DB6DT2
                                jmp DB6DF2
                                DB6DT2:
                                    ;NO HAY EN LA IZQUIERDA Y HAY EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH si
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA
                                DB6DF2:
                                    ;NO HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    jmp GAMERRORMOV
                    DB7: ;DESTINO EN fila2 FUENTE EN FILA 1
                        mov si, di
                        sub di, 1 ; para buscar a la izquierda
                        add si, 1 ; para buscar a la derecha

                        cmp fila1[di], 1 ;buscando izquierda
                        je DB7IT
                        jmp DB7IF
                            DB7IT:
                                cmp fila1[si], 1 ;buscando la derecha
                                je DB7DT1
                                jmp DB7DF1
                                DB7DT1:
                                    ;HAY EN LA IZQUIERDA Y EN LA DERECHA
                                    imprimirCadena msg2
                                    mov bx, di
                                    mov cx, si
                                    add bl, 65
                                    add cl, 65
                                    imprimirCaracter bl
                                    imprimirCadena siete
                                    imprimirCaracter cl
                                    imprimirCadena siete
                                    ;volvemos apilar el destino
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    jmp FUENTEN
                                DB7DF1:
                                    ;HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH di
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA    
                            DB7IF:
                                cmp fila1[si], 1 ;buscando la derecha
                                je DB7DT2
                                jmp DB7DF2
                                DB7DT2:
                                    ;NO HAY EN LA IZQUIERDA Y HAY EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH si
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA
                                DB7DF2:
                                    ;NO HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    jmp GAMERRORMOV
                    DB8: ;DESTINO EN fila1 FUENTE EN FILA 0
                        mov si, di
                        sub di, 1 ; para buscar a la izquierda
                        add si, 1 ; para buscar a la derecha

                        cmp fila0[di], 1 ;buscando izquierda
                        je DB8IT
                        jmp DB8IF
                            DB8IT:
                                cmp fila0[si], 1 ;buscando la derecha
                                je DB8DT1
                                jmp DB8DF1
                                DB8DT1:
                                    ;HAY EN LA IZQUIERDA Y EN LA DERECHA
                                    imprimirCadena msg2
                                    mov bx, di
                                    mov cx, si
                                    add bl, 65
                                    add cl, 65
                                    imprimirCaracter bl
                                    imprimirCadena ocho
                                    imprimirCaracter cl
                                    imprimirCadena ocho
                                    ;volvemos apilar el destino
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    jmp FUENTEN
                                DB8DF1:
                                    ;HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH di
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA    
                            DB8IF:
                                cmp fila0[si], 1 ;buscando la derecha
                                je DB8DT2
                                jmp DB8DF2
                                DB8DT2:
                                    ;NO HAY EN LA IZQUIERDA Y HAY EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH si
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA
                                DB8DF2:
                                    ;NO HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    jmp GAMERRORMOV                
                
                DESTN:
                    ; juegan las negras, restamos a la fila 1
                    sub al, 1d       
                    cmp al, '1'
                    je DN1
                    cmp al, '2'
                    je DN2
                    cmp al, '3'
                    je DN3
                    cmp al, '4'
                    je DN4
                    cmp al, '5'
                    je DN5
                    cmp al, '6'
                    je DN6
                    cmp al, '7'
                    je DN7
                    jmp GAMEERRORCM

                    DN1: ;DESTINO FILA6 FUENTE EN FILA 7
                        mov si, di
                        sub di, 1 ; para buscar a la izquierda
                        add si, 1 ; para buscar a la derecha

                        cmp fila7[di], 2 ;buscando izquierda
                        je DN1IT
                        jmp DN1IF
                            DN1IT:
                                cmp fila7[si], 2 ;buscando la derecha
                                je DN1DT1
                                jmp DN1DF1
                                DN1DT1:
                                    ;HAY EN LA IZQUIERDA Y EN LA DERECHA
                                    imprimirCadena msg2
                                    mov bx, di
                                    mov cx, si
                                    add bl, 65
                                    add cl, 65
                                    imprimirCaracter bl
                                    imprimirCadena uno
                                    imprimirCaracter cl
                                    imprimirCadena uno
                                    ;volvemos apilar el destino
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    jmp FUENTEN
                                DN1DF1:
                                    ;HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH di
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA    
                            DN1IF:
                                cmp fila7[si], 2 ;buscando la derecha
                                je DN1DT2
                                jmp DN1DF2
                                DN1DT2:
                                    ;NO HAY EN LA IZQUIERDA Y HAY EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH si
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA
                                DN1DF2:
                                    ;NO HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    jmp GAMERRORMOV
                    DN2: ;DESTINO FILA5 FUENTE EN FILA 6
                        mov si, di
                        sub di, 1 ; para buscar a la izquierda
                        add si, 1 ; para buscar a la derecha

                        cmp fila6[di], 2 ;buscando izquierda
                        je DN2IT
                        jmp DN2IF
                            DN2IT:
                                cmp fila6[si], 2 ;buscando la derecha
                                je DN2DT1
                                jmp DN2DF1
                                DN2DT1:
                                    ;HAY EN LA IZQUIERDA Y EN LA DERECHA
                                    imprimirCadena msg2
                                    mov bx, di
                                    mov cx, si
                                    add bl, 65
                                    add cl, 65
                                    imprimirCaracter bl
                                    imprimirCadena dos
                                    imprimirCaracter cl
                                    imprimirCadena dos
                                    ;volvemos apilar el destino
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    jmp FUENTEN
                                DN2DF1:
                                    ;HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH di
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA    
                            DN2IF:
                                cmp fila6[si], 2 ;buscando la derecha
                                je DN2DT2
                                jmp DN2DF2
                                DN2DT2:
                                    ;NO HAY EN LA IZQUIERDA Y HAY EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH si
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA
                                DN2DF2:
                                    ;NO HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    jmp GAMERRORMOV
                    DN3: ;DESTINO FILA4 FUENTE EN FILA 5
                        mov si, di
                        sub di, 1 ; para buscar a la izquierda
                        add si, 1 ; para buscar a la derecha

                        cmp fila5[di], 2 ;buscando izquierda
                        je DN3IT
                        jmp DN3IF
                            DN3IT:
                                cmp fila5[si], 2 ;buscando la derecha
                                je DN3DT1
                                jmp DN3DF1
                                DN3DT1:
                                    ;HAY EN LA IZQUIERDA Y EN LA DERECHA
                                    imprimirCadena msg2
                                    mov bx, di
                                    mov cx, si
                                    add bl, 65
                                    add cl, 65
                                    imprimirCaracter bl
                                    imprimirCadena tres
                                    imprimirCaracter cl
                                    imprimirCadena tres
                                    ;volvemos apilar el destino
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    jmp FUENTEN
                                DN3DF1:
                                    ;HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH di
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA    
                            DN3IF:
                                cmp fila5[si], 2 ;buscando la derecha
                                je DN3DT2
                                jmp DN3DF2
                                DN3DT2:
                                    ;NO HAY EN LA IZQUIERDA Y HAY EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH si
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA
                                DN3DF2:
                                    ;NO HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    jmp GAMERRORMOV
                    DN4: ;DESTINO FILA3 FUENTE EN FILA 4
                        mov si, di
                        sub di, 1 ; para buscar a la izquierda
                        add si, 1 ; para buscar a la derecha

                        cmp fila4[di], 2 ;buscando izquierda
                        je DN4IT
                        jmp DN4IF
                            DN4IT:
                                cmp fila4[si], 2 ;buscando la derecha
                                je DN4DT1
                                jmp DN4DF1
                                DN4DT1:
                                    ;HAY EN LA IZQUIERDA Y EN LA DERECHA
                                    imprimirCadena msg2
                                    mov bx, di
                                    mov cx, si
                                    add bl, 65
                                    add cl, 65
                                    imprimirCaracter bl
                                    imprimirCadena cuatro
                                    imprimirCaracter cl
                                    imprimirCadena cuatro
                                    ;volvemos apilar el destino
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    jmp FUENTEN
                                DN4DF1:
                                    ;HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH di
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA    
                            DN4IF:
                                cmp fila4[si], 2 ;buscando la derecha
                                je DN4DT2
                                jmp DN4DF2
                                DN4DT2:
                                    ;NO HAY EN LA IZQUIERDA Y HAY EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH si
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA
                                DN4DF2:
                                    ;NO HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    jmp GAMERRORMOV
                    DN5: ;DESTINO FILA2 FUENTE EN FILA 3
                        mov si, di
                        sub di, 1 ; para buscar a la izquierda
                        add si, 1 ; para buscar a la derecha

                        cmp fila3[di], 2 ;buscando izquierda
                        je DN5IT
                        jmp DN5IF
                            DN5IT:
                                cmp fila3[si], 2 ;buscando la derecha
                                je DN5DT1
                                jmp DN5DF1
                                DN5DT1:
                                    ;HAY EN LA IZQUIERDA Y EN LA DERECHA
                                    imprimirCadena msg2
                                    mov bx, di
                                    mov cx, si
                                    add bl, 65
                                    add cl, 65
                                    imprimirCaracter bl
                                    imprimirCadena cinco
                                    imprimirCaracter cl
                                    imprimirCadena cinco
                                    ;volvemos apilar el destino
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    jmp FUENTEN
                                DN5DF1:
                                    ;HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH di
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA    
                            DN5IF:
                                cmp fila3[si], 2 ;buscando la derecha
                                je DN5DT2
                                jmp DN5DF2
                                DN5DT2:
                                    ;NO HAY EN LA IZQUIERDA Y HAY EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH si
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA
                                DN5DF2:
                                    ;NO HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    jmp GAMERRORMOV
                    DN6: ;DESTINO FILA1 FUENTE EN FILA 2
                        mov si, di
                        sub di, 1 ; para buscar a la izquierda
                        add si, 1 ; para buscar a la derecha

                        cmp fila2[di], 2 ;buscando izquierda
                        je DN6IT
                        jmp DN6IF
                            DN6IT:
                                cmp fila2[si], 2 ;buscando la derecha
                                je DN6DT1
                                jmp DN6DF1
                                DN6DT1:
                                    ;HAY EN LA IZQUIERDA Y EN LA DERECHA
                                    imprimirCadena msg2
                                    mov bx, di
                                    mov cx, si
                                    add bl, 65
                                    add cl, 65
                                    imprimirCaracter bl
                                    imprimirCadena seis
                                    imprimirCaracter cl
                                    imprimirCadena seis
                                    ;volvemos apilar el destino
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    jmp FUENTEN
                                DN6DF1:
                                    ;HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH di
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA    
                            DN6IF:
                                cmp fila2[si], 2 ;buscando la derecha
                                je DN6DT2
                                jmp DN6DF2
                                DN6DT2:
                                    ;NO HAY EN LA IZQUIERDA Y HAY EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH si
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA
                                DN6DF2:
                                    ;NO HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    jmp GAMERRORMOV
                    DN7: ;DESTINO FILA0 FUENTE EN FILA 1
    
                        mov si, di
                        sub di, 1 ; para buscar a la izquierda
                        add si, 1 ; para buscar a la derecha

                        cmp fila1[di], 2 ;buscando izquierda
                        je DN7IT
                        jmp DN7IF
                            DN7IT:
                                cmp fila1[si], 2 ;buscando la derecha
                                je DN7DT1
                                jmp DN7DF1
                                DN7DT1:
                                    ;HAY EN LA IZQUIERDA Y EN LA DERECHA
                                    imprimirCadena msg2
                                    mov bx, di
                                    mov cx, si
                                    add bl, 65
                                    add cl, 65
                                    imprimirCaracter bl
                                    imprimirCadena siete
                                    imprimirCaracter cl
                                    imprimirCadena siete
                                    ;volvemos apilar el destino
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    jmp FUENTEN
                                DN7DF1:
                                    ;HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH di
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA    
                            DN7IF:
                                cmp fila1[si], 2 ;buscando la derecha
                                je DN7DT2
                                jmp DN7DF2
                                DN7DT2:
                                    ;NO HAY EN LA IZQUIERDA Y HAY EN LA DERECHA
                                    PUSH recoveryColumna
                                    PUSH recoveryFila
                                    PUSH si
                                    xor ah, ah
                                    PUSH ax
                                    jmp PARAFILA
                                DN7DF2:
                                    ;NO HAY EN LA IZQUIERDA, NO EN LA DERECHA
                                    jmp GAMERRORMOV
                    
            


        FUENTEN: ;esta etiquete indica el ingreso de la casilla fuente
            ;columna es el que indicara el indice de mi fila
            obtenerCaracter
            sub SobreColumna, al ;restamos el destino columna, ej, B-A=1
            sub al, 65
            xor ah, ah ;limpiamos lo que tenga el bit mayor
            mov di, ax
            PUSH di ;rescatamos el indice FUENTE

            ;fila es el que indicara que fila modificar
            obtenerCaracter
            sub SobreFila, al
            xor ah, ah ; limpiamos lo que tenga el bit mayor de 
            PUSH ax ; guardamos el registro ax que contiene el valor de la fila
            obtenerCaracter ;caracter de espera
        CONDICIONMOV:
            CONDCOLUMNA:
                cmp SobreColumna, -1d
                je CONDFILA
                cmp SobreColumna, 1d
                je CONDFILA
                jmp GAMERRORMOV
            CONDFILA:
                cmp set, 0 ;si es 0 estan jugando las blancas
                je MOVB
                cmp set, 1 ;si es 1 estan jugando las negras
                je MOVN
                    MOVB:
                        cmp SobreFila, -1d
                        je PARAFILA
                        jmp GAMERRORMOV
                    MOVN:
                        cmp SobreFila, 1d
                        je PARAFILA
                        jmp GAMERRORMOV

        PARAFILA:
            POP AX ;sacamos el registro que corresponde a ax
            cmp al, '1'
            je IN0
            cmp al, '2'
            je IN1
            cmp al, '3'
            je IN2
            cmp al, '4'
            je IN3
            cmp al, '5'
            je IN4
            cmp al, '6'
            je IN5
            cmp al, '7'
            je IN6
            cmp al, '8'
            je IN7
            jmp GAMEERRORCM

            IN0:
                POP di ;rescatamos el indice de la fila
                mov dl, fila7[di] ; guardamos en dx el dato de la fila indicado por di
                mov fila7[di], 0       
                jmp PARACOLUMNA
            IN1:
                POP di ;rescatamos el indice de la fila
                mov dl, fila6[di] ; guardamos en dx el dato de la fila indicado por di       
                mov fila6[di], 0
                jmp PARACOLUMNA
            IN2:
                POP di ;rescatamos el indice de la fila
                mov dl, fila5[di] ; guardamos en dx el dato de la fila indicado por di       
                mov fila5[di], 0
                jmp PARACOLUMNA
            IN3:
                POP di ;rescatamos el indice de la fila
                mov dl, fila4[di] ; guardamos en dx el dato de la fila indicado por di       
                mov fila4[di], 0
                jmp PARACOLUMNA
            IN4:
                POP di ;rescatamos el indice de la fila
                mov dl, fila3[di] ; guardamos en dx el dato de la fila indicado por di       
                mov fila3[di], 0
                jmp PARACOLUMNA
            IN5:
                POP di ;rescatamos el indice de la fila
                mov dl, fila2[di] ; guardamos en dx el dato de la fila indicado por di       
                mov fila2[di], 0
                jmp PARACOLUMNA
            IN6:
                POP di ;rescatamos el indice de la fila
                mov dl, fila1[di] ; guardamos en dx el dato de la fila indicado por di       
                mov fila1[di], 0
                jmp PARACOLUMNA
            IN7:
                POP di ;rescatamos el indice de la fila
                mov dl, fila0[di] ; guardamos en dx el dato de la fila indicado por di       
                mov fila0[di], 0
                jmp PARACOLUMNA

        PARACOLUMNA:

            POP AX ;sacamos el registro que corresponde a la columna
            cmp al, '1'
            je JN0
            cmp al, '2'
            je JN1
            cmp al, '3'
            je JN2
            cmp al, '4'
            je JN3
            cmp al, '5'
            je JN4
            cmp al, '6'
            je JN5
            cmp al, '7'
            je JN6
            cmp al, '8'
            je JN7 
            jmp GAMEERRORCM

            JN0:
                POP di ;rescatamos el indice de la columna
                mov fila7[di], dl ; guardamos en la seccion de la fila indicado por di el dato en dx
                jmp INTERCAMBIO
            JN1:
                POP di ;rescatamos el indice de la columna
                mov fila6[di], dl ; guardamos en la seccion de la fila indicado por di el dato en dx
                jmp INTERCAMBIO
            JN2:
                POP di ;rescatamos el indice de la columna
                mov fila5[di], dl ; guardamos en la seccion de la fila indicado por di el dato en dx
                jmp INTERCAMBIO
            JN3:
                POP di ;rescatamos el indice de la columna
                mov fila4[di], dl ; guardamos en la seccion de la fila indicado por di el dato en dx
                jmp INTERCAMBIO
            JN4:
                POP di ;rescatamos el indice de la columna
                mov fila3[di], dl ; guardamos en la seccion de la fila indicado por di el dato en dx
                jmp INTERCAMBIO
            JN5:
                POP di ;rescatamos el indice de la columna
                mov fila2[di], dl ; guardamos en la seccion de la fila indicado por di el dato en dx
                jmp INTERCAMBIO
            JN6:
                POP di ;rescatamos el indice de la columna
                mov fila1[di], dl ; guardamos en la seccion de la fila indicado por di el dato en dx
                jmp INTERCAMBIO
            JN7:
                POP di ;rescatamos el indice de la columna
                mov fila0[di], dl ; guardamos en la seccion de la fila indicado por di el dato en dx
                jmp INTERCAMBIO
        
        INTERCAMBIO:
            imprimirCaracter 10d
            imprimirTablero
            jmp CAMBIARTURNO
        
        COMPLEMENTON:
            cmp set, 0 ;si es 0 jugaron las blancas
            je COMPB
            cmp set, 1 ;si es 1 jugaron las negras
            je COMPN
            jmp INICIO

            COMPB:
                imprimirCadena gamemsg1
                imprimirCaracter 10d
                jmp TurnoN
            COMPN:
                imprimirCadena gamemsg0
                imprimirCaracter 10d
                jmp TurnoN

        CAMBIARTURNO:
            cmp set, 0 ;si es 0 jugaron las blancas
            je CAMBIOBLANCAS
            cmp set, 1 ;si es 1 jugaron las negras 
            je CAMBIONEGRAS
            jmp INICIO

            CAMBIOBLANCAS:
                mov set, 1
                imprimirCadena gamemsg0 ;texto de turno negras
                imprimirCaracter 10d
                jmp TurnoN
            CAMBIONEGRAS:
                mov set, 0
                imprimirCadena gamemsg1 ;texto de turno blancas
                imprimirCaracter 10d
                jmp TurnoN

        GAMERRORMOV:
            imprimirCadena gamerror0
            jmp COMPLEMENTON
        GAMEERRORCM:
            imprimirCadena gamerror1
            jmp COMPLEMENTON

    
main ENDP
end main