;=====================OPERACIONES ARITMETICAS==============
dividir_numeros MACRO op1, op2, result
    PUSH BX
    PUSH AX
    mov ax, op1
    cwd
    mov bx, op2
    idiv bx ;ax = ax/bx
    mov result, ax
    POP AX
    POP BX
ENDM

multiplicar_numeros MACRO op1, op2, result
    PUSH BX
    PUSH AX
    mov ax, op1
    cwd
    mov bx, op2
    imul bx ; ax = ax*bx
    mov result, ax
    POP AX
    POP BX
ENDM

Delay MACRO number
LOCAL again
    PUSH AX
    PUSH DX
    PUSH BX
    mov ah, 0       ;funcion, no para leer
    int 1Ah         ;obtener el tiempo del dia
    add dx, number  ;añadir la cantidad de segundos a esperar
    mov bx, dx      ;guardar y esperar a que el tiempo se iguale
    again:                           
        int 1Ah
        cmp dx, bx
        jne again
    POP BX
    POP DX
    POP AX
ENDM

Delay_mili macro number
    local D1, D2, EndGC
    PUSH si
    PUSH di

    mov si, number
    D1:
        dec si
            jz EndGC
        mov di, number
    D2:
        dec di
            jnz D2
        JMP d1
    EndGC:
        POP di
        POP si
endm



;==================DEFINICION DE MACROS===================
;DEVUEVLE EL CARACTER EN AL
get_char MACRO
    xor ax, ax ;limpiamos el registro A   
    mov ah, 01h ;movemos a ah la interrupcion
    int 21h
ENDM

;DEVUELVE LA CADENA LEIDA DE UNA ARCHIVO EN EL BUFFER
;parametros: BUFFER
get_string MACRO buffer
    LOCAL LEER, TERMINAR
    xor si, si 
    LEER:
        get_char
        cmp al, 0dh
        je TERMINAR
        mov buffer[si], al
        inc si
        jmp LEER
    TERMINAR:        
        mov buffer[si], 00h   
ENDM

;DEVUELVE LA CADENA LEIDA DE LA CONSOLA EN EL BUFFER
;parametros: BUFFER
get_string_console MACRO buffer
    LOCAL LEER, TERMINAR
    xor si, si 
    LEER:
        get_char
        cmp al, 0dh
        je TERMINAR
        mov buffer[si], al
        inc si
        jmp LEER
    TERMINAR:        
        mov buffer[si], '$'   
ENDM

;IMPRIME EN CONSOLA EL CARACTER ENVIADO
print_char MACRO char
    PUSH AX
    PUSH DX
    mov dl, char
    mov ah, 06h
    int 21h
    POP DX
    POP AX
ENDM

;IMPRIME EN CONSOLA LA CADENA ENVIADA
print_string MACRO string
    xor dx, dx
    mov ah, 09h
    mov dx, offset string
    int 21h
ENDM

;CREA UN FICHERO
;PARAMETROS: buffer --nombre del archivo, handle --variable donde se guardara el codigo del archivo
file_create MACRO buffer, handle
    mov ah, 3ch
    mov cx, 00h
    lea dx, buffer
    int 21h
    mov handle, ax
ENDM

;CIERRA UN FICHERO
;PARAMETROS: handle --codigo del archivo a cerrar
file_close MACRO handle
    mov ah, 3eh
    mov bx, handle
    int 21h
ENDM

;ABRIR UN FICHERO
;PARAMTREOS: buffer --nombre del archivo, handle --varible donde se guardara el codigo del archivo
file_open MACRO buffer, handle
    mov ah, 3dh
    mov al, 10b
    lea dx, buffer
    int 21h
    mov handle, ax
ENDM

;LEER UN FICHERO
;PARAMETROS: buffer --buffer donde se guardara los datos almacenados, handle --variable que contiene el codigo del fichero
file_read MACRO buffer, handle
    mov ah, 3fh
    mov bx, handle
    mov cx, sizeof buffer
    lea dx, buffer
    int 21h
ENDM

;ESCRIBIR UN FICHERO
;PARAMETROS: buffer --buffer que se escribiran en el fichero, handle --variable que contiene el codigo del fichero
file_write MACRO buffer, handle    
    PUSH AX
    PUSH BX
    PUSH CX
    PUSH DX
    mov ah, 40h
    mov bx, handle
    mov cx, sizeof buffer
    lea dx, buffer 
    int 21h
    POP DX
    POP CX
    POP BX
    POP AX
ENDM

file_write_explicit MACRO buffer, handle, n
    mov ah, 40h
    mov bx, handle
    mov cx, n
    lea dx, buffer
    int 21
ENDM

;==============================OPERACIONES PARA TIPOS DE DATOS=============================
;CONVIERTE UN STRING A INT
;RESULTADO SE GUARDA EN AX
string_to_int MACRO string
    LOCAL CONVERTSTR, FINSI, ACTIVARCOMPLEMENTO, LAST, COMPLEMENTO
    ;print_string string
    PUSH di
    PUSH si
    PUSH cx
    PUSH dx
    PUSH bx

    xor di, di
    xor si, si
    xor cx, cx
    xor ax, ax
    CONVERTSTR:
        xor dx, dx ;limpiamos los residuos de dx
        xor bx, bx ;limpiamos los residuos de bx
        mov bx, 10d ;llevamos a bx el multiplicador 10
        mov cl, string[si] ;obtenemos el caracter del buffer
        ;print_char cl
        cmp cl, 45 ;si es negativo
        je ACTIVARCOMPLEMENTO
        cmp cl, 48 ;si es menor de 48 (8) error
        jl LAST
        cmp cl, 57 ;si es mayor de 57 (9) error
        jg LAST
        sub cl, 48 ;restamos para obtener el numero
        mul bx ;multiplicamos ax = ax * bx, 
        add ax, cx ;sumamos a ax el valor que guardamos ax = ax + cl
        inc si ;incrementamos el valor del indice en 1
        jmp CONVERTSTR
    ACTIVARCOMPLEMENTO:
        mov di, 1 ;activamos la negacion para despues
        inc si ;aumentamos el indice en 1
        jmp CONVERTSTR
    LAST:
        cmp di, 1 ;si el complemento a2 esta activado negamos el numero
        je COMPLEMENTO
        jmp FINSI
    COMPLEMENTO:
        neg ax
    FINSI:
        mov di, 0 ;terminamos la conversion devolviendo el complemento siempre a 0
        mov dl, al
        ;print_char dl
        POP bx
        POP dx
        POP cx
        POP si
        POP di
        nop
        
ENDM

;CONVIERTE UN ENTERO A UNA CADENA
int_to_string MACRO numero, string
    LOCAL NEGATIVO, DIVIDIR, TERMINARDIV, CONV, FINDIV
    PUSH si
    PUSH di
    PUSH AX
    PUSH BX
    PUSH CX
    PUSH DX
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
        POP DX
        POP CX
        POP BX
        POP AX
        POP di
        POP si
        nop
ENDM



;==========================OPERACIONES MISCELANEAS SOBRE ARREGLOS==========================
;COMPARA LOS DOS STRING ENVIADOS Y LOS GUARDA EN EL RESULTADO RESULT
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

;LIMPIA LOS DATOS DE UN BUFFER
clean_buff MACRO buffer
    LOCAL CLEAN
    PUSH cx
    PUSH si
    xor si, si
    mov cx, SIZEOF buffer  
    CLEAN:
        mov buffer[si], 36
        inc si
        loop CLEAN
    POP si
    POP cx          
ENDM

;OBTIENE LA CANTIDAD DE DATOS DE UN ARREGLO
;EL RESULTADO LO GUARDA EN LENGTH
string_length MACRO array, length
LOCAL INICIO, TERMINAR
    PUSH si
    PUSH ax
    mov length, 00h
    xor si, si
    INICIO:
        mov al, array[si]
        cmp al, '$'
        je TERMINAR
        cmp al, 00h
        je TERMINAR
        inc length
        inc si
        jmp INICIO
    TERMINAR:
        POP ax
        POP si
ENDM

;OBTIENE EL NUMERO MAXIMO DENTRO DEL ARREGLO
;EL RESULTADO LO GUARDA EN LA VARIABLE m, SE ENVIA n PARA FACILITAR ESTA TAREA
array_max MACRO array, n, m
LOCAL CICLO, NUEVOMAX, INCREMENTAR, TERMINAR
    PUSH CX
    PUSH SI
    PUSH DX
    PUSH BX
    xor dx, dx
    xor si, si ;ponemos el contador a 0
    mov cx, n ;guardamos en cx el tamaño de nuestro arreglo
    CICLO: 
        mov dl, array[si] ;registro dl, tenemos el valor del array en la posicion de si
        mov bx, m 
        cmp dl, bl ;si el nuevo valor es mayor seteamos nuevo
        jng INCREMENTAR
        NUEVOMAX:
            mov m, dx
        INCREMENTAR:
            inc si
            loop CICLO
    TERMINAR:
        POP BX
        POP DX
        POP SI
        POP CX
ENDM



;============================ORDENAMIENTOS PARA ARREGLOS NUMERICOS=========================
;ORDENAMIENTO BURBUJA
ordenar_BUBBLE MACRO buffer, descAsc, n, d
LOCAL DECLOOP, DECSUBLOOP, COMPSUBLOOP, GETDATA, CHANGEDATA, INCSUBLOOP, INCLOOP, ASCENDENTE, DESCENDENTE, TERMINAR
    xor si, si
    xor di, di
    mov cx, n
    ;string_length buffer, cx
    sub cx, 1
    inc si
    DECLOOP:
        cmp si, cx
        jl DECSUBLOOP
        JMP TERMINAR
    DECSUBLOOP:
        mov dx, n
        sub dx, 1
        sub dx, si
        xor di, di
        COMPSUBLOOP:
            set_graph_graphics_memory
            clear_marco
            set_grah_data
            pintar_array buffer
            Delay d
            cmp di, dx
            jl GETDATA
            cmp di, dx
            je GETDATA
            JMP INCLOOP
        GETDATA:
            mov al, buffer[di]
            mov ah, buffer[di+1]
            cmp descAsc, 0 ;ascendente
            je ASCENDENTE
            jmp DESCENDENTE ;descendente
            ASCENDENTE:
                cmp al, ah
                jg CHANGEDATA
                jmp INCSUBLOOP
            DESCENDENTE:
                cmp al, ah
                jl CHANGEDATA
                jmp INCSUBLOOP
            CHANGEDATA:
                mov buffer[di], ah
                mov buffer[di+1], al
                inc di
                jmp COMPSUBLOOP
        INCSUBLOOP:
            inc di
            jmp COMPSUBLOOP
    INCLOOP:
        inc si
        jmp DECLOOP
    TERMINAR:
        nop
ENDM

;ORDENAMIENTO SHELL
ordenar_SHELL MACRO buffer, descAsc, n, gap, d
LOCAL DECCICLO, DECSUBCICLO_1, DECSUBCICLO_2, DECSUBCICLO_2_2, ASCENDENTE, DESCENDENTE, CHANGEDATA, INCSUBCICLO_2, INCSUBCICLO_1, TERMINAR, INCCICLO
    xor si, si
    xor di, di
    dividir_numeros n, 2, gap   ;gap = n/2
    DECCICLO:
        cmp gap, 0              ;gap > 0
        jng TERMINAR  
        mov si, gap             ;i = gap
        DECSUBCICLO_1:
            set_graph_graphics_memory
            clear_marco
            set_grah_data
            pintar_array buffer
            Delay d
            cmp si, n           ;i < n
            jnl INCCICLO       
            mov al, buffer[si]  ;temp = arr[i]
            xor di, di          ;int j
            mov di, si          ;j = i
            DECSUBCICLO_2:
                cmp di, gap         ;j >= gap
                jg DECSUBCICLO_2_2
                cmp di, gap             
                je DECSUBCICLO_2_2
                jmp INCSUBCICLO_1
                DECSUBCICLO_2_2:
                    mov bx, di          ;bx = j
                    sub di, gap         ;j-gap
                    mov ah, buffer[di]  ;temp1 = arr[j-gap]
                    mov di, bx          ;j = bx
                    cmp descAsc, 0  ;ascendente
                    je ASCENDENTE
                    cmp descAsc, 1  ;descendente
                    je DESCENDENTE
                    jmp DESCENDENTE
                ASCENDENTE:
                    cmp ah, al
                    jg CHANGEDATA
                    jmp INCSUBCICLO_1
                DESCENDENTE:
                    cmp ah, al
                    jl CHANGEDATA
                    jmp INCSUBCICLO_1                
                CHANGEDATA:
                    mov buffer[di], ah  ;arr[j] = arr [j-gap]
            INCSUBCICLO_2:
                sub di, gap
                jmp DECSUBCICLO_2
        INCSUBCICLO_1:
            mov buffer[di], al
            inc si
            jmp DECSUBCICLO_1
    INCCICLO:
        dividir_numeros gap, 2, gap
        jmp DECCICLO
    TERMINAR:
        nop
ENDM



;==================================OPERACIONES SOBRE ARREGLOS==============================
;IMPRIME TODOS LOS DATOS DE UN ARRAY
print_num_ARRAY macro array
LOCAL INICIO, TERMINAR
    xor si, si
    INICIO:
    mov dl, array[si]
    cmp dl, '$'
    je TERMINAR
    cmp dl, 00h
    je TERMINAR

    add dl, 32
    print_char dl
    inc si
    jmp INICIO
    TERMINAR:
        NOP
ENDM

;OBTIENE LOS DATOS DE UN ARCHIVO Y LOS GUARDA EN UN ARRAY
parsear_Resultados MACRO entrada, salida, buffnum, fc, lc
    LOCAL INICIO, FINAL, OBTENERRESULTADO, PARSE, IGNORAR
    xor di, di ;limpiamos el contador para acceder a entrada
    xor si, si ;limpiamos el contador si para acceder a salida
    xor dx, dx ;registros de para leer el caracter
    xor cx, cx ;otro contador
    INICIO:
        mov dl, entrada[di]
        cmp dl, '$'
        je FINAL
        cmp dl, 00h
        je FINAL
        cmp dl, fc
        je OBTENERRESULTADO
        jmp IGNORAR
        OBTENERRESULTADO:
            inc di ;empezamos a leer el numero
            mov dl, entrada[di]
            cmp dl, lc
            je PARSE
            mov buffnum[si], dl
            inc si
            jmp OBTENERRESULTADO
        PARSE:
            string_to_int buffnum
            clean_buff buffnum
            mov si, cx
            mov salida[si], al
            inc cx
            xor si, si
        IGNORAR:
            inc di
            jmp INICIO
    FINAL:
        NOP
ENDM

;=============================MODOS DE ESTADO DE LA APLICACION=============================
;UTILIZA EL MODO GRAFICO CON LA MEMORIA DE DATOS
set_grah_data MACRO
    PUSH AX
    mov ax, @data
    mov ds, ax ; modo grafico en memoria de datos
    POP AX
ENDM

;UTILIZA EL MODO GRAFICO CON LA MEMORIA DE GRAFICOS
set_graph_graphics_memory MACRO
    PUSH AX
    mov ax, 0A000h
    mov ds, ax ; modo grafico en memoria de graficos
    POP AX
ENDM

;CAMBIA A MODO GRAFICO
grap_mode MACRO
    PUSH AX
    mov ax, 13h
    int 10h
    POP AX
ENDM

;CAMBIA A MODO DATOS
data_mode MACRO
    PUSH AX
    mov ax, 03h
    int 10h
    mov ax, @data
    mov ds, ax
    POP AX
ENDM



;========================MACROS SOBRE EL MODO GRAFICO PARA PINTAR LAS GRAFICAS============================
;PINTA UN MARCO CUADRADO SOBRE LA PANTALLA
pintar_marco MACRO
    LOCAL HORARR, HORABJ, VERIZQ, VERDER
    ;==========LINEA HORIZONTAL DE ARRIBA
    mov cx, 300 ;numero de pixeles a pintar
    mov di, 12CAh ;situamos el puntero en la linea 15 columna 10
    HORARR:
        mov [di], 0fh ; poner color en A000:DI
        inc di
        loop HORARR
    ;==========LINEA HORIZONTAL DE ABAJO
    mov cx, 300 ;numero de pixeles a pintar
    MOV DI, 0E74Ah ;situamos el puntero en la linea 185 columna 10
    HORABJ:
        mov [di], 0fh ;poner color blanco en el pixel
        inc di
        loop HORABJ

    ;==========LINEA VERTICAL IZQUIERDA
    mov cx, 170 ;pixeles a pintar
    mov di, 12CAh ;situamos el puntero en la linea 15 columna 10
    VERIZQ:
        mov [di], 0fh ;poner color blanco en el pixel
        add di, 320d
        loop VERIZQ

    ;==========LINEA VERTICAL DERECHA
    mov cx, 170 ;pixeles a pintar
    mov di, 13F6h ;situamos el puntero en la linea 15 columna 310
    VERDER:
        mov [di], 0fh ;poner color blanco en el pixel
        add di, 320d
        loop VERDER
ENDM

;PINTA UN LINEA VERTICAL HACIA SEGUN LOS SIGUIENTES PARAMETROS
;N = numero de pixeles a pintar
;INICIO = donde se va empezar a pintar la linea
;COLOR = color de la linea
graficar_vertical MACRO N, INICIO, COLOR
    LOCAL GRAPH
    PUSH CX
    PUSH DI
    mov cx, N ;pixeles a pintar
    mov di, INICIO ;situamos el puntero
    GRAPH:
        mov [di], COLOR
        sub di, 320d
        loop GRAPH
    POP DI
    POP CX
ENDM

;SELECCIONA UN COLOR SEGUN EL RAGO DE VALORES
select_color MACRO number
LOCAL SETAZUL, SETBLANCO, SETVERDE, SETAMARILLO, TERMINAR
    mov COLORSELECTED, 28h
    cmp number, 81 ;blanco
    jg SETBLANCO
    cmp number, 61 ;verde
    jg SETVERDE
    cmp number, 41 ;amarillo
    jg SETAMARILLO
    cmp number, 21 ;azul
    jg SETAZUL
    jmp TERMINAR ;rojo
    SETAZUL:
        mov COLORSELECTED, 37h
        jmp TERMINAR
    SETAMARILLO:
        Mov COLORSELECTED, 2Ch
        jmp TERMINAR
    SETVERDE:
        mov COLORSELECTED, 02h
        jmp TERMINAR
    SETBLANCO:    
        mov COLORSELECTED, 0fh
        jmp TERMINAR
    TERMINAR:
        nop
ENDM

;PINTA UN ARREGLO SEGUN EL ARRAY ENVIADO
pintar_array MACRO array
LOCAL CICLO, INC_CICLO, CICLO_2
    PUSH BX
    PUSH CX
    PUSH DX
    PUSH SI
    PUSH DI
    PUSH LEN

    inc ITCOUNT ;incrementamos el numero de iteraciones
    int_to_string ITCOUNT, TempTop
    Graph_String TempTop, 147, 0, 0Fh

    string_length array, LEN            ;obtenemos la longitud de nuestro array
    array_max array, LEN, MAX           ;obtenemos el numero maximo ubicado en nuestro array
    dividir_numeros 140d, MAX, FACTOR   ;obtenmos un factor de multiplicacion dado por 140/MAX
    mov LINEAGRAFO, 0D494h
    PUSH BX
    mov BX, LEN     ;ponemos en bx la longitud de nuestro array
    add BX, BX      ;multiplicamos por dos esa longitud
    ;inc BX          
    mov GROSOR, BX  ;el resultado lo guardamos en una variable
    POP BX
    dividir_numeros 280d, GROSOR, GROSOR    ;obtenenmos el grosor de cada barra
    MOV BX, GROSOR      ;guardamos el grosor en bx
    SUB LINEAGRAFO, bx  ;restamos ese grosor a la linea inicial para obtener el inicio absoluto
    xor cx, cx         ;el contador de cx estara dado por la longitud del array
    xor dx, dx          ;se limpia dx
    xor si, si          ;se limpia si
    CICLO:
        mov dl, array[si]   ;dl guarda nuestro dato del aray
        mov TIPO, dx        ;el dato lo guardamos en la variable TIPO
        multiplicar_numeros FACTOR, TIPO, VSIZE ;multiplicamos para obtener la altura de la barray
        mov dx, VSIZE ;DX RESERVADA contiene la altura relativa de la barra
        mov bx, GROSOR ;en bx guardamos el valor de nuestro grosor
        add LINEAGRAFO, bx ;sumamos al punto inicial el grosor de las barras
        mov bx, LINEAGRAFO ;BX contiene DONDE EMPIEZA LA BARRA
        xor di, di
        CICLO_2:
            cmp di, GROSOR  ;comparamos si ya tiene el grosor normal
            je INC_CICLO    ;si son iguales rompemos el ciclo
            select_color TIPO
            PUSH AX
            mov al, COLORSELECTED
            set_graph_graphics_memory   
            graficar_vertical dx, bx, al
            set_grah_data
            ;strigify_Resultados array, AuxSTR
            POP AX
            inc di  
            inc bx
            jmp CICLO_2
        INC_CICLO:
            mov LINEAGRAFO, bx ;guardamos la posicion nuevo en LINEA GRAFO
            inc si
            inc cx
            cmp cx, LEN
            jne CICLO
            ;loop CICLO
    POP LEN
    POP DI
    POP SI
    POP DX
    POP CX
    POP BX
ENDM

;MUEVE UN CURSOR A UNA POSICION EN x Y y
move_Cursor macro row, column
    PUSH AX
    PUSH DX
    mov ah, 02h
    mov dh, row
    mov dl, column
    int 10h
    POP DX
    POP AX
endm

;IMPRIME UNA CADENA EN LAS CORDENADAS x Y y
Graph_String macro cadena, x, y, COLOR
local WhileDraw
    PUSH AX
    PUSH BX
    PUSH CX
    PUSH SI
    xor ax, ax
    xor cx, cx
    xor si, si
    mov xPos, x
    mov cx, SIZEOF cadena
    move_Cursor y, x
    WhileDraw:
        Push cx
        mov ah, 09h
        mov al, cadena[si]
        ;add al, 50
        mov bl, COLOR
        mov cx, 01h
        int 10h
        inc xPos
        move_Cursor y, xPos
        inc si
        Pop cx
    Loop WhileDraw

    POP SI
    POP CX
    POP BX
    POP AX
endm

Graph_String_Set macro cadena, x, y, COLOR, size
local WhileDraw
    xor ax, ax
    xor cx, cx
    xor si, si
    mov xPos, x
    mov cx, size
    move_Cursor y, x
    WhileDraw:
        Push cx
        mov ah, 09h
        mov al, cadena[si]
        ;add al, 50
        mov bl, COLOR
        mov cx, 01h
        int 10h
        inc xPos
        move_Cursor y, xPos
        inc si
        Pop cx
    Loop WhileDraw
endm

;LIMPIA LA PANTALLA DENTRO DEL MARCO
clear_marco MACRO
LOCAL CICLO, REPEAT
    PUSH CX
    PUSH DX
    PUSH DI
    mov cx, 9A38h ;pixeles que voy a pintar
    mov di, 2594h ;donde empiezo a pintar
    xor dx, dx ;donde voy contar
    CICLO:
        mov [di], 00h ;poner colo negro
        inc di
        inc dx
        cmp dx, 280
        jne REPEAT
        xor dx, dx
        add di, 40d ;sumamos 40
        REPEAT:
            loop CICLO
    POP DI
    POP DX
    POP CX
ENDM

clear_marco_expand MACRO
LOCAL CICLO, REPEAT
    PUSH CX
    PUSH DX
    PUSH DI
    mov cx, 0AFC6h ;pixeles que voy a pintar
    mov di, 140Bh ;donde empiezo a pintar
    xor dx, dx ;donde voy contar
    CICLO:
        mov [di], 00h ;poner colo negro
        inc di
        inc dx
        cmp dx, 298d
        jne REPEAT
        xor dx, dx
        add di, 22d ;sumamos 22
        REPEAT:
            loop CICLO
    POP DI
    POP DX
    POP CX
ENDM

limpiar_barra MACRO
    LOCAL CICLO, REPEAT
    PUSH CX
    PUSH DX
    PUSH DI
    mov cx, 1788d ;pixeles que voy a pintar
    mov di, 53451d ;donde empiezo a pintar
    xor dx, dx ;donde voy contar
    CICLO:
        mov [di], 00h ;poner colo negro
        inc di
        inc dx
        cmp dx, 298
        jne REPEAT
        xor dx, dx
        add di, 22d ;sumamos 60
        REPEAT:
            loop CICLO
    POP DI
    POP DX
    POP CX
ENDM

;DIBUJA LA BARRA EN LA PANTALLA 
pintar_barra MACRO INICIO
LOCAL CICLO, REPEAT
    PUSH CX
    PUSH DX
    PUSH DI
    mov cx, 300d ;pixeles que voy a pintar
    mov di, INICIO ;donde empiezo a pintar
    xor dx, dx ;donde voy contar
    CICLO:
        mov [di], 0Fh ;poner colo blanco
        inc di
        inc dx
        cmp dx, 60
        jne REPEAT
        xor dx, dx
        add di, 260d ;sumamos 260
        REPEAT:
            loop CICLO
    POP DI
    POP DX
    POP CX    
ENDM

;DIBUJA LA PELOTE EN LA PANTALL
printPixel MACRO x, y, color
    PUSH AX
    PUSH BX
    PUSH CX
    PUSH DX
    mov ah, 0ch
    mov al, color
    mov bh, 0h
    mov dx, y
    mov cx, x
    int 10h
    POP DX
    POP CX
    POP BX
    POP AX
ENDM

pintar_bloque MACRO INICIO, COLOR
LOCAL CICLO, REPEAT
    PUSH CX
    PUSH DX
    PUSH DI
    mov cx, 360d    ;pixeles que voy a pintar
    mov di, INICIO  ;donde empiezo a pintar
    xor dx, dx      ;dondw voy a contar
    CICLO:
        mov [di], COLOR ;poner colo blanco
        inc di
        inc dx
        cmp dx, 72
        jne REPEAT
        xor dx, dx
        add di, 248d ;sumamos 60
        REPEAT:
            loop CICLO
    POP DI
    POP DX
    POP CX  
ENDM

;=====================FUNCIONES DE LA APLICACION====================
ingresar_usuario MACRO
    LOCAL USERADMIN, PASSADMIN, TERMINAR, USER, PASS, CICLO_0, CICLO_1, MATCH_PASS, MATCH_USER, SUMIDERO, REPEAT_CICLO_0, EXITO_USER, ENDLOG
    print_char 10d
    print_string GetUser
    get_string_console UserArray
    print_string GetPass
    get_string_console PassArray
    compare_string UserArray, AdminUser, UserResult
    USERADMIN:
        cmp UserResult, 1
        jne USER
    PASSADMIN:
        compare_string PassArray, AdminPass, PassResult
        cmp PassResult, 1
        jne USER
        admin_top
        jmp ENDLOG
    USER:   ;empezamos por asegurar que existe el usuari
        file_open FileUser, HandleArchivo
        file_read DatosUser, HandleArchivo
        xor si, si
        xor di, di
        CICLO_0:
            mov al, DatosUser[si]
            cmp al, ','             ;si es , significa que ya termino de leer el usuario
            je MATCH_USER
            cmp al, '$'
            je TERMINAR
            mov INGUser[di], al
            inc si
            inc di
            jmp CICLO_0
        MATCH_USER:
            compare_string UserArray, INGUser, UserResult   ;comparamos los usuarios
            cmp UserResult, 1                               ;si es true vamos a matchar la contraseña
            je PASS
                                                            ;si es false vamos a buscar otro usuario
            SUMIDERO:
                mov al, DatosUser[si]
                cmp al, ';'
                je REPEAT_CICLO_0
                inc si
                jmp SUMIDERO
            REPEAT_CICLO_0:
                inc si
                inc si                  ;incrementamos dos veces si para ponernos en la siguiente linea
                xor di, di              ;0 di para volver ingresar al array de usuario tempora
                clean_buff INGUser      ;limpiamos el buffer
                jmp CICLO_0
    PASS:
        xor di, di
        inc si
        CICLO_1:
            mov al, DatosUser[si]
            cmp al, ';'
            je MATCH_PASS
            cmp al, '$'
            je TERMINAR
            mov INGPass[di], al
            inc si
            inc di
            jmp CICLO_1
        MATCH_PASS:
            compare_string PassArray, INGPass, PassResult
            cmp PassResult, 1
            je EXITO_USER
            jmp TERMINAR
    TERMINAR:
        print_char 10d
        print_string ErrorLog
        print_char 10d
        get_char
        jmp ENDLOG

    EXITO_USER:
        play_game

    ENDLOG:
        nop
ENDM

play_game MACRO
    grap_mode
    set_graph_graphics_memory
    pintar_marco    
    set_grah_data
    multiplicar_numeros BarraCount, 24d, BarraFactor
    add BarraFactor, 0D0CAh ;le sumamos la constante al factor
    mov dx, BarraFactor     ;movemos el inicio al registro dx
    set_graph_graphics_memory   ;nos ponemos en la memoria de graficos
    limpiar_barra   ;limpiamos la barra
    pintar_barra dx ;graficamos la barra
    set_grah_data   ;volvemos a la memoria de datos
    printPixel 149d, 86d, 0Fh
    string_length UserArray, LEN
    Graph_String_Set UserArray, 0, 0, 0Fh, LEN
    set_N1;ponemos el nivel 1
    mov BloqueContador, 0
    mov BloqueTiempo, 0
    mov BloquePuntos, 0
    mov PelotaYPos, 86d
    mov PelotaXPos, 149d
    clean_buff TiempoPlay
    clean_buff PuntosPlay
    juego_barra_bloque Bloques1, BloqueI1, 21h
    juego_barra_bloque Bloques2, BloqueI2, 21h
    juego_barra_bloque Bloques3, BloqueI3, 0Eh
    juego_barra_bloque Bloques4, BloqueI4, 0Eh
    juego_barra_bloque Bloques5, BloqueI5, 27h
    juego_barra_bloque Bloques6, BloqueI6, 27h
    jmp CICLO_PAUSA
    CICLO_JUEGO:
        juego_pelota
        juego_barra
       ; Delay 00h
        mov ah, 01h ;verificamos si se presiono una tecla
        int 16h
        jz CICLO_JUEGO
        mov ah, 00h ;verificamos cual teclo se presiono
        int 16h
        cmp al, 32  ;Barra Espaciadora
        je CICLO_PAUSA
        cmp al, 27  ;ESCAPE
        jne CICLO_JUEGO
        jmp TERMINAR_JUEGO
    CICLO_PAUSA:
        mov ah, 01h ;verificamos si se presiono una tecla
        int 16h
        jz CICLO_PAUSA
        mov ah, 00h ;verificamos cual tecla se presiono
        int 16h
        cmp al, 32  ;Barra Espaciadora
        je CICLO_JUEGO
        cmp al, 27
        je TERMINAR_JUEGO
        jmp CICLO_PAUSA
    TERMINAR_JUEGO:
        nop
        data_mode
ENDM

juego_barra_bloque MACRO barras, init, color
LOCAL CICLO, PINTAR, AVANZAR, NO_PINTAR, TERMINAR
    set_grah_data
    xor si, si
    CICLO:
        mov al, barras[si]  ;obtenemos el valor del array
        cmp al, 1           ;si resulta que es uno graficamos
        je PINTAR
        jmp NO_PINTAR
        PINTAR:
            multiplicar_numeros 74d, si, BloquesFactor
            mov bx, color
            mov dx, BloquesFactor
            add dx, init
            set_graph_graphics_memory
            pintar_bloque dx, bx
            set_grah_data
            jmp AVANZAR
        NO_PINTAR:
            multiplicar_numeros 74d, si, BloquesFactor
            mov bx, 00h
            mov dx, BloquesFactor
            add dx, init
            set_graph_graphics_memory
            pintar_bloque dx, bx
            set_grah_data
        AVANZAR:
            inc si
            cmp si, 4
            jne CICLO
        TERMINAR:
ENDM

juego_pelota MACRO
LOCAL MOVER_PELOTEXI, MOVER_PELOTAXD, MOVER_PELOTAYU, MOVER_PELOTAYD, EJEY, IMPRIMIR_PELOTA
    ;primero eliminamos la posicion actual de la pelota
    printPixel PelotaXPos, PelotaYPos, 00h ;00
    inc PelotaXPos
    printPixel PelotaXPos, PelotaYPos, 00h  ;10
    inc PelotaYPos
    printPixel PelotaXPos, PelotaYPos, 00h  ;11
    dec PelotaXPos
    printPixel PelotaXPos, PelotaYPos, 00h  ;01
    dec PelotaYPos
    cmp PelotaXDir, 0
    je MOVER_PELOTEXI
    cmp PelotaXDir, 1
    je MOVER_PELOTAXD
    MOVER_PELOTEXI:
        dec PelotaXPos
        jmp EJEY
    MOVER_PELOTAXD:
        inc PelotaXPos
    EJEY:
        cmp PelotaYDir, 0
        je MOVER_PELOTAYD
        cmp PelotaYDir, 1
        je MOVER_PELOTAYU
    MOVER_PELOTAYU:
        dec PelotaYPos
        cmp PelotaYPos, 62
        jg DETERMINAR_CAMBIO
        ;Aqui vamos a validar el choque con los bloques de arriba
    CHOQUE_BARRAS_ARR:
        xor si, si
        COL_X_4:
            cmp PelotaXPos, 235
            jl COL_X_3
            ;la pelota es mayor que el minimo de la barra mayor se hace la colicion
            mov si, 03h
            jmp COL_Y
        COL_X_3:
            cmp PelotaXPos, 161
            jl COL_X_2
            ;la pelota es mayor que el minimo de esta barra se hace la colision
            mov si, 02h
            jmp COL_Y
        COL_X_2:
            cmp PelotaXPos, 87
            jl COL_X_1
            ;la pelota es mayor que el minimo de esta barra se hace la colision
            mov si, 01h
            jmp COL_Y
        COL_X_1:
            ;la pelota es mayor que el minimo de esta barra se hace la colision
            mov si, 00h
        COL_Y:
            COL_Y_6:
                cmp PelotaYPos, 60  ;ultima fila
                jne COL_Y_5
                ;si es igual verificamos que haya un punto bueno o malo
                mov al, Bloques6[si]
                cmp al, 1
                je COL_CHANGE_6
                jmp IMPRIMIR_PELOTA ;no hay ningun bloque podemos seguiR
            COL_Y_5:
                cmp PelotaYPos, 53  ;ultima fila
                jne COL_Y_4
                ;si es igual verificamos que haya un punto bueno o malo
                mov al, Bloques5[si]
                cmp al, 1
                je COL_CHANGE_5
                jmp IMPRIMIR_PELOTA ;no hay ningun bloque podemos seguir
            COL_Y_4:
                cmp PelotaYPos, 46  ;ultima fila
                jne COL_Y_3
                ;si es igual verificamos que haya un punto bueno o malo
                mov al, Bloques4[si]
                cmp al, 1
                je COL_CHANGE_4
                jmp IMPRIMIR_PELOTA ;no hay ningun bloque podemos seguir
            COL_Y_3:
                cmp PelotaYPos, 39  ;tercer fila
                jne COL_Y_2
                ;si es igual verificamos 
                mov al, Bloques3[si]
                cmp al, 1
                je COL_CHANGE_3
                jmp IMPRIMIR_PELOTA
            COL_Y_2:
                cmp PelotaYPos, 32  ;segunda fila
                jne COL_Y_1
                ;si es igual verificamos 
                mov al, Bloques2[si]
                cmp al, 1
                je COL_CHANGE_2
                jmp IMPRIMIR_PELOTA
            COL_Y_1:
                cmp PelotaYPos, 25  ;tercer fila
                jne DETERMINAR_CAMBIO
                ;si es igual verificamos 
                mov al, Bloques1[si]
                cmp al, 1
                je COL_CHANGE_1
                jmp IMPRIMIR_PELOTA
        COL_CHANGE_1:
            mov Bloques1[si], 0
            jmp COL_CHANGE
        COL_CHANGE_2:
            mov Bloques2[si], 0
            jmp COL_CHANGE
        COL_CHANGE_3:
            mov Bloques3[si], 0
            jmp COL_CHANGE
        COL_CHANGE_4:
            mov Bloques4[si], 0
            jmp COL_CHANGE
        COL_CHANGE_5:
            mov Bloques5[si], 0
            jmp COL_CHANGE
        COL_CHANGE_6:
            mov Bloques6[si], 0
        COL_CHANGE:
            mov PelotaYDir, 0
            dec BloqueDeleted
            jmp REPRINT_BLOQUES
    MOVER_PELOTAYD:
        inc PelotaYPos
        cmp PelotaYPos, 62
        jg DETERMINAR_CAMBIO
        CHOQUE_BARRAS_AB:
        xor si, si
        ACOL_X_4:
            cmp PelotaXPos, 235
            jl ACOL_X_3
            ;la pelota es mayor que el minimo de la barra mayor se hace la colicion
            mov si, 03h
            jmp ACOL_Y
        ACOL_X_3:
            cmp PelotaXPos, 161
            jl ACOL_X_2
            ;la pelota es mayor que el minimo de esta barra se hace la colision
            mov si, 02h
            jmp ACOL_Y
        ACOL_X_2:
            cmp PelotaXPos, 87
            jl ACOL_X_1
            ;la pelota es mayor que el minimo de esta barra se hace la colision
            mov si, 01h
            jmp ACOL_Y
        ACOL_X_1:
            ;la pelota es mayor que el minimo de esta barra se hace la colision
            mov si, 00h
        ACOL_Y:
            ACOL_Y_6:
                cmp PelotaYPos, 55  ;ultima fila
                jne ACOL_Y_5
                ;si es igual verificamos que haya un punto bueno o malo
                mov al, Bloques6[si]
                cmp al, 1
                je ACOL_CHANGE_6
                jmp IMPRIMIR_PELOTA ;no hay ningun bloque podemos seguiR
            ACOL_Y_5:
                cmp PelotaYPos, 48  ;ultima fila
                jne ACOL_Y_4
                ;si es igual verificamos que haya un punto bueno o malo
                mov al, Bloques5[si]
                cmp al, 1
                je ACOL_CHANGE_5
                jmp IMPRIMIR_PELOTA ;no hay ningun bloque podemos seguir
            ACOL_Y_4:
                cmp PelotaYPos, 41  ;ultima fila
                jne ACOL_Y_3
                ;si es igual verificamos que haya un punto bueno o malo
                mov al, Bloques4[si]
                cmp al, 1
                je ACOL_CHANGE_4
                jmp IMPRIMIR_PELOTA ;no hay ningun bloque podemos seguir
            ACOL_Y_3:
                cmp PelotaYPos, 34  ;tercer fila
                jne ACOL_Y_2
                ;si es igual verificamos 
                mov al, Bloques3[si]
                cmp al, 1
                je ACOL_CHANGE_3
                jmp IMPRIMIR_PELOTA
            ACOL_Y_2:
                cmp PelotaYPos, 27  ;segunda fila
                jne ACOL_Y_1
                ;si es igual verificamos 
                mov al, Bloques2[si]
                cmp al, 1
                je ACOL_CHANGE_2
                jmp IMPRIMIR_PELOTA
            ACOL_Y_1:
                cmp PelotaYPos, 20  ;tercer fila
                jne DETERMINAR_CAMBIO
                ;si es igual verificamos 
                mov al, Bloques1[si]
                cmp al, 1
                je ACOL_CHANGE_1
                jmp IMPRIMIR_PELOTA
        ACOL_CHANGE_1:
            mov Bloques1[si], 0
            jmp ACOL_CHANGE
        ACOL_CHANGE_2:
            mov Bloques2[si], 0
            jmp ACOL_CHANGE
        ACOL_CHANGE_3:
            mov Bloques3[si], 0
            jmp ACOL_CHANGE
        ACOL_CHANGE_4:
            mov Bloques4[si], 0
            jmp ACOL_CHANGE
        ACOL_CHANGE_5:
            mov Bloques5[si], 0
            jmp ACOL_CHANGE
        ACOL_CHANGE_6:
            mov Bloques6[si], 0
        ACOL_CHANGE:
            mov PelotaYDir, 1
            dec BloqueDeleted
            jmp REPRINT_BLOQUES        
    DETERMINAR_CAMBIO:
        cmp PelotaXPos, 11
        je CHNG_XI
        cmp PelotaXPos, 308
        je CHNG_XD
        jmp CHGN_Y
        CHNG_XD:
            mov PelotaXDir, 0
            jmp CHGN_Y
        CHNG_XI:
            mov PelotaXDir, 1
        CHGN_Y:
            cmp PelotaYPos, 16
            je CHNG_YD
            cmp PelotaYPos, 165
            je CHNG_YU
            cmp PelotaYPos, 183
            je VALORES_INICIALES
            jmp IMPRIMIR_PELOTA
        CHNG_YU:
            ;aqui vamos a verificar que la pelota pueda rebotar
            multiplicar_numeros BarraCount, 24d, BarraFactor
            add BarraFactor, 10 ;sumamos 10 para saber si esta en el rango
            mov dx, BarraFactor
            cmp PelotaXPos, dx ;comparamos si la pelota es mayor que el inicio del rango
            jl IMPRIMIR_PELOTA
            ;resulta que si es mayor
            add BarraFactor, 60 ;sumamos 60 para saber si esta en el rango
            mov dx, BarraFactor
            cmp PelotaXPos, dx
            jg IMPRIMIR_PELOTA
            ;resulta que efectivamente la pelota puede rebotar
            mov PelotaYDir, 1
            jmp IMPRIMIR_PELOTA
        CHNG_YD:
            mov PelotaYDir, 0
            jmp IMPRIMIR_PELOTA
    VALORES_INICIALES:
        mov PelotaYPos, 86d
        mov PelotaXPos, 149d
        jmp IMPRIMIR_PELOTA
    REPRINT_BLOQUES:
        add BloquePuntos, 2 ;sumamos 2 puntos por cada vez que eliminamos un bloque
        juego_barra_bloque Bloques1, BloqueI1, 21h
        juego_barra_bloque Bloques2, BloqueI2, 21h
        juego_barra_bloque Bloques3, BloqueI3, 0Eh
        juego_barra_bloque Bloques4, BloqueI4, 0Eh
        juego_barra_bloque Bloques5, BloqueI5, 27h
        juego_barra_bloque Bloques6, BloqueI6, 27h
    IMPRIMIR_PELOTA:
        ;pintamos la nueva posicion de la pelota
        printPixel PelotaXPos, PelotaYPos, 0Fh ;00
        inc PelotaXPos
        printPixel PelotaXPos, PelotaYPos, 0Fh  ;10
        inc PelotaYPos
        printPixel PelotaXPos, PelotaYPos, 0Fh  ;11
        dec PelotaXPos
        printPixel PelotaXPos, PelotaYPos, 0Fh  ;01
        dec PelotaYPos

        cmp BloqueDeleted, 0
        jne JUEGO_FIN_BUCLE
        changeN
        mov BloqueContador, 0
        printPixel PelotaXPos, PelotaYPos, 00h ;00
        inc PelotaXPos
        printPixel PelotaXPos, PelotaYPos, 00h  ;10
        inc PelotaYPos
        printPixel PelotaXPos, PelotaYPos, 00h  ;11
        dec PelotaXPos
        printPixel PelotaXPos, PelotaYPos, 00h  ;01
        dec PelotaYPos
        mov PelotaYPos, 86d
        mov PelotaXPos, 149d
        JUEGO_FIN_BUCLE:
        Delay_Mili BloqueDelay
        mov dx, BloqueContador
        add dx, BloqueDelay
        mov BloqueContador, dx
        cmp BloqueContador, 5000d ;hemos contado un segundo
        jl NO_CONTAR_SEGUNDO
        inc BloqueTiempo
        mov BloqueContador, 0
        NO_CONTAR_SEGUNDO:
        ;pintamos el tiempo transcurrido
        int_to_string BloqueTiempo, TiempoPlay
        Graph_String TiempoPlay, 147, 0, 0Fh
        ;pintamos el punteo obtenido
        int_to_string BloquePuntos, PuntosPlay
        Graph_String PuntosPlay, 100, 0, 0Fh
        nop
ENDM

juego_barra MACRO
LOCAL TECLA_IZQ, TECLA_DER, GRAFICAR_BARRA, NO_KEY 
    set_grah_data
    mov ah, 01h
    int 16h     ;verificamos el si se ha presionado una tecla
    jz NO_KEY   ;si no se presiono una tecla no hacemos nada mas
    ;se presiono una tecla verificamos cual
    mov ah, 00h
    int 16h
    cmp al, 32d
    je CICLO_PAUSA
    cmp al, 65d
    je TECLA_IZQ
    cmp al, 97d
    je TECLA_IZQ
    cmp al, 68d
    je TECLA_DER
    cmp al, 100d
    je TECLA_DER
    jmp NO_KEY  ;si la tecla que se presiono no corresponde solo ignoramos
        TECLA_IZQ:  ;si se presiono a o A movemos la barra hacia la izquierda
            cmp BarraCount, 0   ;se verifica si la barra esta en el limite izquierdo
            jle NO_KEY
            ;si es mayor hacemos el movimiento
            dec BarraCount      ;reducimos a 1 su posicion
            jmp GRAFICAR_BARRA
        TECLA_DER:  ;si se presiono d o D movemos la barra hacia la derecha
            cmp BarraCount, 10  ;se verifica si la barra esta en el limite derecho
            jge NO_KEY
            ;si es menor hacemos el movimiento
            inc BarraCount      ;incrementamos a 1 su posicion
        GRAFICAR_BARRA:
            multiplicar_numeros BarraCount, 24d, BarraFactor
            add BarraFactor, 0D0CAh ;le sumamos la constante al factor
            mov dx, BarraFactor     ;movemos el inicio al registro dx
            set_graph_graphics_memory   ;nos ponemos en la memoria de graficos
            limpiar_barra   ;limpiamos la barra
            pintar_barra dx ;graficamos la barra
            set_grah_data   ;volvemos a la memoria de datos
    NO_KEY:
        nop
ENDM

set_N1 MACRO 
    MOV Bloques1[0], 1
    MOV Bloques1[1], 1
    MOV Bloques1[2], 1
    MOV Bloques1[3], 1 

    MOV Bloques2[0], 1
    MOV Bloques2[1], 1
    MOV Bloques2[2], 1
    MOV Bloques2[3], 1 

    MOV Bloques3[0], 1
    MOV Bloques3[1], 1
    MOV Bloques3[2], 1
    MOV Bloques3[3], 1 

    MOV Bloques4[0], 0
    MOV Bloques4[1], 0
    MOV Bloques4[2], 0
    MOV Bloques4[3], 0 

    MOV Bloques5[0], 0
    MOV Bloques5[1], 0
    MOV Bloques5[2], 0
    MOV Bloques5[3], 0 

    MOV Bloques6[0], 0
    MOV Bloques6[1], 0
    MOV Bloques6[2], 0
    MOV Bloques6[3], 0

    juego_barra_bloque Bloques1, BloqueI1, 21h
    juego_barra_bloque Bloques2, BloqueI2, 21h
    juego_barra_bloque Bloques3, BloqueI3, 0Eh
    juego_barra_bloque Bloques4, BloqueI4, 0Eh
    juego_barra_bloque Bloques5, BloqueI5, 27h
    juego_barra_bloque Bloques6, BloqueI6, 27h

    Graph_String strN3, 50, 0, 00h
    Graph_String strN1, 50, 0, 0fh 
    mov BloqueDelay, 200d
    mov BloqueDeleted, 12
ENDM

set_N2 MACRO 
    MOV Bloques1[0], 1
    MOV Bloques1[1], 1
    MOV Bloques1[2], 1
    MOV Bloques1[3], 1 

    MOV Bloques2[0], 1
    MOV Bloques2[1], 1
    MOV Bloques2[2], 1
    MOV Bloques2[3], 1 

    MOV Bloques3[0], 1
    MOV Bloques3[1], 1
    MOV Bloques3[2], 1
    MOV Bloques3[3], 1 

    MOV Bloques4[0], 1
    MOV Bloques4[1], 1
    MOV Bloques4[2], 1
    MOV Bloques4[3], 1 

    MOV Bloques5[0], 0
    MOV Bloques5[1], 1
    MOV Bloques5[2], 1
    MOV Bloques5[3], 0 

    MOV Bloques6[0], 0
    MOV Bloques6[1], 0
    MOV Bloques6[2], 0
    MOV Bloques6[3], 0

    juego_barra_bloque Bloques1, BloqueI1, 21h
    juego_barra_bloque Bloques2, BloqueI2, 21h
    juego_barra_bloque Bloques3, BloqueI3, 0Eh
    juego_barra_bloque Bloques4, BloqueI4, 0Eh
    juego_barra_bloque Bloques5, BloqueI5, 27h
    juego_barra_bloque Bloques6, BloqueI6, 27h
 
    
    Graph_String strN1, 50, 0, 00h
    Graph_String strN2, 50, 0, 0fh 
    mov BloqueDelay, 150d
    mov BloqueDeleted, 18
ENDM

set_N3 MACRO 
    MOV Bloques1[0], 1
    MOV Bloques1[1], 1
    MOV Bloques1[2], 1
    MOV Bloques1[3], 1 

    MOV Bloques2[0], 1
    MOV Bloques2[1], 1
    MOV Bloques2[2], 1
    MOV Bloques2[3], 1 

    MOV Bloques3[0], 1
    MOV Bloques3[1], 1
    MOV Bloques3[2], 1
    MOV Bloques3[3], 1 

    MOV Bloques4[0], 1
    MOV Bloques4[1], 1
    MOV Bloques4[2], 1
    MOV Bloques4[3], 1 

    MOV Bloques5[0], 1
    MOV Bloques5[1], 1
    MOV Bloques5[2], 1
    MOV Bloques5[3], 1 

    MOV Bloques6[0], 1
    MOV Bloques6[1], 1
    MOV Bloques6[2], 1
    MOV Bloques6[3], 1

    juego_barra_bloque Bloques1, BloqueI1, 21h
    juego_barra_bloque Bloques2, BloqueI2, 21h
    juego_barra_bloque Bloques3, BloqueI3, 0Eh
    juego_barra_bloque Bloques4, BloqueI4, 0Eh
    juego_barra_bloque Bloques5, BloqueI5, 27h
    juego_barra_bloque Bloques6, BloqueI6, 27h


    Graph_String strN2, 50, 0, 00h
    Graph_String strN3, 50, 0, 0fh 
    mov BloqueDelay, 100d
    mov BloqueDeleted, 24
ENDM

changeN MACRO
    cmp BloqueLevel, 1 ;estamos en nivel 1 cambiamos a 2
    je CN1
    cmp BloqueLevel, 2 ;estamos en nivel 2 cambiamos a 3
    je CN2
    cmp BloqueLevel, 3 ;estamos en nivel 2 cambiamos a 1
    je CN3
    CN1:
        mov BloqueLevel, 2
        set_N2
        jmp CNN
    CN2:
        mov BloqueLevel, 3
        set_N3
        jmp CNN
    CN3:
        mov BloqueLevel, 1
        set_N1
        jmp TERMINAR_JUEGO
        jmp CNN
    CNN:
ENDM

admin_top MACRO
    LOCAL PUNTAJE, TIEMPOS, SALIR
    print_string AdminOpciones
    print_string ChooseOpcion
    get_char
    cmp al, '1'
    je PUNTAJE
    cmp al, '2'
    je TIEMPOS
    cmp al, '3'
    je SALIR
    PUNTAJE:
        mostrar_top_puntos
        jmp SALIR
    TIEMPOS:
        mostrar_top_tiempos
        jmp SALIR
    SALIR:
        nop

ENDM

Registrar_Usuario MACRO
    LOCAL CICLO_1, CICLO_2
    print_char 10d
    print_string GetUser
    get_string UserArray
    print_string GetPass
    get_string PassArray
    file_open FileUser, HandleArchivo
    file_read DatosUser, HandleArchivo
    string_length UserArray, LEN
    mov cx, LEN
    xor si, si
    CICLO_1:
        mov dl, UserArray[si]
        mov UserTypeAux, dl
        file_write UserTypeAux, HandleArchivo
        inc si
        loop CICLO_1
    file_write UserTypeComa, HandleArchivo

    string_length PassArray, LEN
    mov cx, LEN
    xor si, si
    CICLO_2:
        mov dl, PassArray[si]
        mov UserTypeAux, dl
        file_write UserTypeAux, HandleArchivo
        inc si
        loop CICLO_2
    file_write UserTypePComa, HandleArchivo
    mov UserTypeAux, 10d
    file_write UserTypeAux, HandleArchivo
    file_close HandleArchivo
    print_char 10d
    print_string SuccesUser
    get_char
ENDM

;===============================REPORTES===========================
mostrar_top_puntos MACRO
    LOCAL BUBBLE, SHELL, QUICK, TERMINAR
    xor ax, ax
    print_string ChooseSort
    print_string ChooseOpcion
    get_char
    sub al, 48
    mov Sort, al
    print_string ChooseSortType
    print_string ChooseOpcion
    get_char
    sub al, 48
    mov SortType, al
    print_string ChooseSortVelc
    get_char
    mov strVelc2[0], al
    sub al, 48
    xor ah, ah
    add ax, ax
    mov SortVelc, ax
    ;obtenemos los resultados
    clean_buff Resultados
    file_open FileTop, HandleArchivo
    file_read DatosTop, HandleArchivo
    parsear_Resultados DatosTop, Resultados, TempTop, 2Ch, 2Dh
    mov ITCOUNT, 0
    cmp Sort, 0
    je BUBBLE
    cmp Sort, 1
    je SHELL
    cmp Sort, 2
    je QUICK
    jmp TERMINAR
    BUBBLE:
        grap_mode                           ;nos ponemos en modo grafico
        set_graph_graphics_memory
        pintar_marco                        ;pintamos el marco
        set_grah_data
        Graph_String strBubble, 0, 0, 0Fh   ;pintamos el encabezado
        Graph_String strVelc, 152, 0, 0Fh   ;pintamos la velocidad
        Graph_String strVelc2, 157, 0, 0Fh
        string_length Resultados, LEN
        ordenar_BUBBLE Resultados, SortType, LEN, SortVelc ;hacemos el loop
        mov ah,10h    
        int 16h
        data_mode
        mostrar_top_puntos_reporte
        get_char
        jmp TERMINAR
    SHELL:
        grap_mode                           ;modo grafico
        set_graph_graphics_memory
        pintar_marco                        ;pintamos el marco
        set_grah_data
        Graph_String strShell, 0, 0, 0Fh    ;pintamos el encabezado
        Graph_String strVelc, 152, 0, 0Fh   ;pintamos la velocidad
        Graph_String strVelc2, 157, 0, 0Fh
        string_length Resultados, LEN
        ordenar_SHELL Resultados, SortType, LEN, GAP, SortVelc ;hacemos el loop
        mov ah,10h    
        int 16h
        data_mode 
        mostrar_top_puntos_reporte
        get_char
        jmp TERMINAR
    QUICK:
        grap_mode
        set_graph_graphics_memory
        pintar_marco
        set_grah_data
        Graph_String strQuick, 0, 0, 0Fh
        Graph_String strVelc, 152, 0, 0Fh   ;pintamos la velocidad
        Graph_String strVelc2, 157, 0, 0Fh
        string_length Resultados, QHIGH
        dec QHIGH
        mov dl, SortType
        mov QDA, dl
        mov dx, SortVelc
        mov QVELC, dx
        call quicksort
        mov ah, 10h
        int 16h
        mov QPIVOTE, 0
        mov QI, 0
        mov QJ, 0
        mov QLOW, 0
        mov QHIGH, 0
        mov q, 0
        mov QDA, 0
        mov QVELC, 0
        data_mode
        mostrar_top_puntos_reporte
        get_char
    TERMINAR:   
    ;admin_top
ENDM

mostrar_top_tiempos MACRO 
    LOCAL BUBBLE, SHELL, QUICK, TERMINAR
    xor ax, ax
    print_string ChooseSort
    print_string ChooseOpcion
    get_char
    sub al, 48
    mov Sort, al
    print_string ChooseSortType
    print_string ChooseOpcion
    get_char
    sub al, 48
    mov SortType, al
    print_string ChooseSortVelc
    get_char
    mov strVelc2[0], al
    sub al, 48
    xor ah, ah
    add ax, ax
    mov SortVelc, ax
    ;obtenemos los resultados
    clean_buff Resultados
    file_open FileTop, HandleArchivo
    file_read DatosTop, HandleArchivo
    parsear_Resultados DatosTop, Resultados, TempTop, 2Dh, 3Ah
    ;print_num_ARRAY Resultados
    mov ITCOUNT, 0
    cmp Sort, 0
    je BUBBLE
    cmp Sort, 1
    je SHELL
    cmp Sort, 2
    je QUICK
    jmp TERMINAR
    BUBBLE:
        grap_mode                           ;nos ponemos en modo grafico
        set_graph_graphics_memory
        pintar_marco                        ;pintamos el marco
        set_grah_data
        Graph_String strBubble, 0, 0, 0Fh   ;pintamos el encabezado
        Graph_String strVelc, 152, 0, 0Fh   ;pintamos la velocidad
        Graph_String strVelc2, 157, 0, 0Fh
        string_length Resultados, LEN
        ordenar_BUBBLE Resultados, SortType, LEN, SortVelc ;hacemos el loop
        mov ah,10h    
        int 16h
        data_mode
        mostrar_top_tiempos_reporte
        get_char
        jmp TERMINAR
    SHELL:
        grap_mode                           ;modo grafico
        set_graph_graphics_memory
        pintar_marco                        ;pintamos el marco
        set_grah_data
        Graph_String strShell, 0, 0, 0Fh    ;pintamos el encabezado
        Graph_String strVelc, 152, 0, 0Fh   ;pintamos la velocidad
        Graph_String strVelc2, 157, 0, 0Fh
        string_length Resultados, LEN
        ordenar_SHELL Resultados, SortType, LEN, GAP, SortVelc ;hacemos el loop
        mov ah,10h    
        int 16h
        data_mode 
        mostrar_top_tiempos_reporte
        get_char
        jmp TERMINAR
    QUICK:
        grap_mode
        set_graph_graphics_memory
        pintar_marco
        set_grah_data
        Graph_String strQuick, 0, 0, 0Fh
        Graph_String strVelc, 152, 0, 0Fh   ;pintamos la velocidad
        Graph_String strVelc2, 157, 0, 0Fh
        string_length Resultados, QHIGH
        dec QHIGH
        mov dl, SortType
        mov QDA, dl
        mov dx, SortVelc
        mov QVELC, dx
        call quicksort
        mov ah, 10h
        int 16h
        mov QPIVOTE, 0
        mov QI, 0
        mov QJ, 0
        mov QLOW, 0
        mov QHIGH, 0
        mov q, 0
        mov QDA, 0
        mov QVELC, 0
        data_mode
        mostrar_top_tiempos_reporte
        get_char
    TERMINAR:
ENDM


mostrar_top_puntos_reporte MACRO
LOCAL CICLO_REP, CICLO_USER, PRE_CICLO_NUM, CICLO_NUM, CMP_NUMS, PRINT_RESULT, NO_PRINT_RESULT, SUMIDERO, SEND_REP, TERMINAR, CICLO_RP1, CICLO_RP2
    file_create FilePuntos, HandleArchivo
    file_write REncabezado, HandleArchivo
    file_write FRepLine, HandleArchivo
    print_string RepLine
    print_char 10d
    file_write FRepTopA, HandleArchivo
    print_string RepTopA
    print_char 10d
    file_write FRepLine, HandleArchivo
    print_string RepLine
    print_char 10d
    ;print_string DatosTop
    xor si, si              ;para recorrer el archivo de texto
    xor di, di              ;para acceder al array de usuario
    xor cx, cx              ;lleva el contador de posiciones de Resultados
    string_length Resultados, LEN

    CICLO_REP:
        cmp cx, LEN
        je TERMINAR     ;si cx es igual que el tamaño del array dejamos de hacer el procedimient
        ;si es menor
        mov di, cx
        mov dl, Resultados[di]   ;movemos el numero correspondiente a dl
        inc cx
        xor di, di              ;limpiamos el acceso de memoria
    CICLO_USER:
       
        mov ah, DatosTop[si]    ;hacemos una copia del caracter
        cmp ah, '$'
        je TERMINAR
        cmp ah, ','
        je PRE_CICLO_NUM        ;una vez tenemos el nombre saltamos a leer el numero
        ;no no es , guardamos en el aray
        mov RepUser[di], ah     ;guardamos el caracter
        inc di                  ;incrementamos di para seguir guardando
        inc si                  ;incrementamos si para seguir leyendo
        jmp CICLO_USER

    PRE_CICLO_NUM:
        xor di, di              ;limpiamos el indice
        inc si                  ;nos posicionamos en el primer caracter numerico
    CICLO_NUM:
        mov ah, DatosTop[si]    ;leemos un caracter numerico
        cmp ah, '$'
        je TERMINAR
        cmp ah, '-'
        je CMP_NUMS
        ;no es - guardamos en el array
        mov RepNum[di], ah      ;guardamos el caracter
        inc di
        inc si
        jmp CICLO_NUM
    CMP_NUMS:
        string_to_int RepNum    ;resultado lo guarda en ax
        cmp al, dl              ;si los resultados son iguales imprimios
        je PRINT_RESULT         
        jmp NO_PRINT_RESULT
    PRINT_RESULT:               ;si los resultados coinciden imprimimos
        print_string RepUser
        print_string RepEspacio
        print_string RepNum
        print_string RepEspacio

        PUSH CX
        
        string_length RepUser, RLEN
        mov cx, RLEN
        xor si, si
        CICLO_RP1:
        mov dl, RepUser[si]
        mov UserTypeAux, dl
        file_write UserTypeAux, HandleArchivo
        inc si
        loop CICLO_RP1

        file_write FRepEspacio, HandleArchivo

        string_length RepNum, RLEN
        mov cx, RLEN
        xor si, si
        CICLO_RP2:
        mov dl, RepNum[si]
        mov UserTypeAux, dl
        file_write UserTypeAux, HandleArchivo
        inc si
        loop CICLO_RP2

        file_write FRepEspacio, HandleArchivo

        POP CX

        clean_buff RepNum       ;se limpian los bufferes
        clean_buff RepUser
        xor si, si              ;se volvera a leer al archivo desde el inicio
        jmp CICLO_REP           ;se repite para el siguiente valor de los resultados
    NO_PRINT_RESULT:            ;si no coinciden solo limpiamos
        clean_buff RepNum
        clean_buff RepUser
    SUMIDERO:                   ;pasamos todos los caracteres haste el ;
        mov ah, DatosTop[si]
        cmp ah, ';'
        je SEND_REP
        inc si
        jmp SUMIDERO
    SEND_REP:
        inc si                  ;incrementamos si para ponernos el proximo usuario
        xor di, di              ;limpiamos indice
        jmp CICLO_USER
    TERMINAR:
        print_char 10d
        file_write FRepLine, HandleArchivo
        print_string RepLine
        print_char 10d
        file_close HandleArchivo
        file_open FileTop, HandleArchivo
        file_read DatosTop, HandleArchivo
        nop
ENDM

mostrar_top_tiempos_reporte MACRO
    LOCAL CICLO_REP, CICLO_USER, PRE_CICLO_NUM, CICLO_NUM, CMP_NUMS, PRINT_RESULT, NO_PRINT_RESULT, SUMIDERO, SEND_REP, TERMINAR, AVANCE_NUM, SEND_CICLO_NUM, CICLO_RP2, CICLO_RP1
    file_create FileTiempos, HandleArchivo
    file_write REncabezado, HandleArchivo
    file_write FRepLine, HandleArchivo
    print_string RepLine
    print_char 10d
    file_write FRepTopB, HandleArchivo
    print_string RepTopB
    print_char 10d
    file_write FRepLine, HandleArchivo
    print_string RepLine
    print_char 10d
    xor si, si              ;para recorrer el archivo de texto
    xor di, di              ;para acceder al array de usuario
    xor cx, cx              ;lleva el contador de posiciones de Resultados
    string_length Resultados, LEN
    CICLO_REP:
        cmp cx, LEN         ;comparamos si cx es igual a la cantidad de datos
        je TERMINAR         ;si es igual terminamos el loo
        mov di, cx          ;si no movemos a di, lo que tenemos en cx y sacamos un dato
        mov dl, Resultados[di]
        ;print_char dl
        inc cx
        xor di, di
    CICLO_USER:                 ;loop que usaremos para obtener los datos a imprimir
        mov ah, DatosTop[si]
        cmp ah, '$'
        je TERMINAR
        cmp ah, ','
        je AVANCE_NUM
        mov RepUser[di], ah
        inc di
        inc si
        jmp CICLO_USER
    AVANCE_NUM:
        mov ah, DatosTop[si]
        cmp ah, '-'
        je SEND_CICLO_NUM
        inc si
        jmp AVANCE_NUM
    SEND_CICLO_NUM:
        inc si
        xor di, di
    CICLO_NUM:
        mov ah, DatosTop[si]
        cmp ah, '$'
        je TERMINAR
        cmp ah, ':'
        je CMP_NUMS
        mov RepNum[di], ah
        inc di
        inc si
        jmp CICLO_NUM
    CMP_NUMS:
        string_to_int RepNum
        cmp al, dl
        je PRINT_RESULT
        jmp NO_PRINT_RESULT
    PRINT_RESULT:
        print_string RepUser
        print_string RepEspacio
        print_string RepNum
        print_string RepEspacio
        PUSH CX
        
        string_length RepUser, RLEN
        mov cx, RLEN
        xor si, si
        CICLO_RP1:
        mov dl, RepUser[si]
        mov UserTypeAux, dl
        file_write UserTypeAux, HandleArchivo
        inc si
        loop CICLO_RP1

        file_write FRepEspacio, HandleArchivo

        string_length RepNum, RLEN
        mov cx, RLEN
        xor si, si
        CICLO_RP2:
        mov dl, RepNum[si]
        mov UserTypeAux, dl
        file_write UserTypeAux, HandleArchivo
        inc si
        loop CICLO_RP2

        file_write FRepEspacio, HandleArchivo

        POP CX
        clean_buff RepNum       ;se limpian los bufferes
        clean_buff RepUser
        xor si, si
        jmp CICLO_REP
    NO_PRINT_RESULT:
        clean_buff RepNum
        clean_buff RepUser
    SUMIDERO:
        mov ah, DatosTop[si]
        cmp ah, ';'
        je SEND_REP
        inc si
        jmp SUMIDERO
    SEND_REP:
        inc si
        xor di, di
        jmp CICLO_USER
    TERMINAR:
        print_char 10d
        file_write FRepLine, HandleArchivo
        print_string RepLine
        print_char 10d
        file_close HandleArchivo
        file_open FileTop, HandleArchivo
        file_read DatosTop, HandleArchivo
        nop
ENDM



;====================DEFINICION DE MEMORIA RAM=======================
.model small
.stack
.data
    Encabezado db 0ah, 0dh, 'UNIVERSIDAD DE SAN CARLOS DE GUATEMALA', 0ah, 0dh, 'FACULTAD DE INGENIERIA', 0ah, 0dh, 'CIENCIAS Y SISTEMAS', 0ah, 0dh, 'Arquitectura de computadoras y Ensambladores A', 0ah, 0dh, 'NOMBRE: Aldo Rigoberto Hernandez Avila', 0ah, 0dh, 'CARNET: 201800585', 0ah, 0dh, 'SECCION: A', '$'
    REncabezado db 0ah, 0dh, 'UNIVERSIDAD DE SAN CARLOS DE GUATEMALA', 0ah, 0dh, 'FACULTAD DE INGENIERIA', 0ah, 0dh, 'CIENCIAS Y SISTEMAS', 0ah, 0dh, 'Arquitectura de computadoras y Ensambladores A', 0ah, 0dh, 'NOMBRE: Aldo Rigoberto Hernandez Avila', 0ah, 0dh, 'CARNET: 201800585', 0ah, 0dh, 'SECCION: A'
    Opciones db 0ah, 0dh, '1) Ingresar',0ah,0dh,'2) Registrar', 0ah,0dh, '3) Salir', 0ah, 0dh, '$'
    AdminOpciones db 0ah, 0dh, '1) Top Puntajes',0ah, 0dh, '2) Top Tiempos', 0ah, 0dh, '3) Salir', 0ah, 0dh, '$'
    ChooseOpcion db 0ah, 0dh, 'Escoga una Opcion: ', '$'
    ChooseSort db 0ah, 0dh, 'Escoga un Tipo de Ordenamiento',0ah, 0dh,'0) BUBBLESORT',0ah, 0dh,'1) SHELLSORT',0ah, 0dh,'2) QUICKSORT',0ah, 0dh, '$'
    ChooseSortType db 0ah, 0dh, '0) Ascendente',0ah, 0dh, '1) Descendente',0ah, 0dh,'$'
    ChooseSortVelc db 0ah, 0dh, 'Escoga una velocidad entre [0-9]: ', '$'

    AdminUser db 'adminAI', '$'
    AdminPass db '4321', '$'
    INGUser db 15 dup('$')
    INGPass db 15 dup('$')
    GetUser db 'Escriba su Usuario > ', '$'
    GetPass db 'Escriba su Password > ', '$'
    SuccesUser db 'Usuario Registrado!', '$'
    ErrorLog db 'Usuario o Password Incorrecta!', '$'
    Play db 'ZONA DE JUEGO', '$'
    UserArray db 50 dup('$')
    PassArray db 50 dup('$')
    UserResult db 1
    PassResult db 1

    HandleArchivo dw ?

    strN1 db 'N1'
    strN2 db 'N2'
    strN3 db 'N3'
    strBubble db 'ORDENAMIENTO: BURBBLESORT'
    strShell db 'ORDENAMIENTO: SHELLSORT'
    strQuick db 'ORDENAMIENTO: QUICKSORT'
    strVelc db 'VELC: '
    strVelc2 db '0'
    xPos db 0
    AuxSTR db 3 dup('$')

    ;Parametros de ordenamiento
    Sort db 0
    SortType db 0
    SortVelc dw 5


    FileTop db 'Juegos.txt', 00h
    FileUser db 'User.txt', 00h
    FilePuntos db 'puntos.txt', 00h
    FileTiempos db 'tiempo.txt', 00h
    UserTypeComa db ','
    UserTypePComa db ';'
    UserTypeAux db ' '

    DatosTop db 500 dup('$')
    DatosUser db 500 dup('$')
    Resultados db 100 dup('$')

    ;variables a usar para el desarrollo de ordenamientos
    ITCOUNT dw 0
    TempTop db 4 dup('$')

    ASC db 0
    DESC db 1
    LEN dw 0
    GAP dw 0
    MAX dw 0
    FACTOR dw 0
    VSIZE dw 0
    TIPO dw 0
    LINEAGRAFO dw 0D494h
    GROSOR dw 0
    COLORSELECTED db 0fh

    QPIVOTE db 0
    QI dw 0
    QJ dw 0
    QLOW dw 0
    QHIGH dw 0
    q dw 0
    QDA db 0
    QVELC dw 0
    ;variables a usar para el desarrolo del juego
    BarraCount dw 5
    BarraFactor dw ?
    PelotaYPos dw 86d
    PelotaXPos dw 149d
    PelotaYDir dw 1
    PelotaXDir dw 0
    BloquesFactor dw 0
    Bloques1 db 1, 1, 1, 1
    Bloques2 db 1, 1, 1, 1
    Bloques3 db 1, 1, 1, 1
    Bloques4 db 1, 1, 1, 1
    Bloques5 db 1, 1, 1, 1
    Bloques6 db 1, 1, 1, 1
    TiempoPlay db 4 dup('$')
    PuntosPlay db 4 dup('$')
    BloqueDeleted db 0
    BloqueLevel db 1
    BloqueDelay dw 00h
    BloqueContador dw 0
    BloqueTiempo dw 0
    BloquePuntos dw 0
    BloqueI1 dw 190Dh
    BloqueI2 dw 21CDh
    BloqueI3 dw 2A8Dh
    BloqueI4 dw 334Dh
    BloqueI5 dw 15373d
    BloqueI6 dw 17613d
    ;variables a usar para la generacion de reportes
    RepNum db 4 dup('$')
    RepUser db 15 dup('$')
    RepLine db '--------------------------------------------------------------------------------','$'
    RepTopA db '                                 TOP 10: PUNTOS                                 ','$'
    RepTopB db '                                 TOP 10: TIEMPOS                                ','$'
    FRepLine db 0ah, '--------------------------------------------------------------------------------'
    FRepTopA db 0ah, '                                 TOP 10: PUNTOS                                 '
    FRepTopB db 0ah, '                                 TOP 10: TIEMPOS                                '
    RepEspacio db '     -----     ','$'
    FRepEspacio db '     -----     '
    RLEN dw 0
.code

    mov ax, @data
    mov ds, ax
    ;jmp PRUBAS
    print_string Encabezado
    MENU:
        print_string Opciones
        print_string ChooseOpcion
        get_char
        cmp al, '1'
        je INGRESAR
        cmp al, '2'
        je REGISTRAR
        cmp al, '3'
        je SALIR
        jmp MENU

    INGRESAR:
        ingresar_usuario
        jmp MENU

    REGISTRAR:
        Registrar_Usuario
        jmp MENU

    PRUBAS:


    SALIR:
        mov ah, 4ch
        int 21h


quicksort proc
    ;COMPARE P WITH R.
    mov  ax, QLOW 
    cmp  ax, QHIGH                  ;COMPARE P WITH R
    jge  bigger1                ;IF P ≥ R, SORT IS DONE.
    ;CALL PARTITION(A, QLOW, QHIGH).
    call partition
    set_graph_graphics_memory
    clear_marco
    set_grah_data
    pintar_array Resultados
    Delay QVELC
    ;GET Q = PARTITION(A, QLOW, QHIGH).
    mov  q, ax
    ;PUSH Q+1, QHIGH INTO STACK FOR LATER USAGE.
    inc  ax
    push ax
    push QHIGH
    ;CALL QUICKSORT(A, QLOW, Q-1).
    mov  ax, q
    mov  QHIGH, ax
    dec  QHIGH
    call quicksort
    ;CALL QUICKSORT(A, Q+1, QHIGH).
    pop  QHIGH
    pop  QLOW
    call quicksort 
    ;WHEN SORT IS DONE.
    bigger1:
    ret
quicksort endp

partition proc
    ;GET X = ARR[ R ].
    mov di, QHIGH
    mov cl, Resultados[di]
    mov QPIVOTE, cl     ;pivote = arr[QHIGH]
    mov cx, QLOW
    dec cx             
    mov QI, cx          ;i = QLOW -1
    mov cx, QLOW
    mov QJ, cx          ;j = QLOW
    QCICLO:
        mov cx, QHIGH
        cmp QJ, cx      ;j < QHIGH
        jnl QPOST_CICLO  ;j < QHIGH = false
        mov si, QJ      ;TRUE
        mov ah, Resultados[si]  ; ah = arr[j]
        cmp QDA, 0
        je QASCENDENTE
        cmp QDA, 1
        je QDESCENDENTE
        jmp QDESCENDENTE
        QASCENDENTE:
            cmp ah, QPIVOTE     ; arr[j] < QPIVOTE
            jnl QINCCICLO        ;FALSE
            jmp QCHANGEDATA
        QDESCENDENTE:
            cmp ah, QPIVOTE     ; arr[j] < QPIVOTE
            jng QINCCICLO        ;FALSE
            jmp QCHANGEDATA
        QCHANGEDATA:
        inc QI              ;i++, TRUE
        mov di, QI
        mov al, Resultados[di]  ;temp = arr[i]
        mov Resultados[di], ah  ;arr[i] = arr[j]
        mov Resultados[si], al  ;arr[j] = temp
    QINCCICLO:
        inc QJ
        jmp QCICLO
    QPOST_CICLO:
        inc QI              ;i++
        mov di, QI
        mov al, Resultados[di]  ;temp = arr[i]
        mov si, QHIGH        
        mov ah, Resultados[si]  ;arr[QHIGH]
        mov Resultados[di], ah  ;arr[i] = arr[QHIGH]
        mov Resultados[si], al  ;arr[QHIGH] = temp   
    
        mov ax, QI          ;return QI
        ret
partition endp
end