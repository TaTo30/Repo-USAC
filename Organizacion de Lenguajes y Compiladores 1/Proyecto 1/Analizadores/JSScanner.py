from tkinter import *
import os

class jsScanner:
    Reservadas = ['function','var', 'if', 'else', 'for', 'while', 'break', 'continue', 'return', 'constructor', 'class']
    Signos = {"IGUAL":'\=', "COMILLASDOBLES":'\"', "COMILLAS":'\'', "MAYOR":'\>', "MENOR":'\<',"NEGACION":'\!', "CONJUNCION":'\&',"PARA":'\(',"PARC":'\)',"SUMA":'\+',"RESTA":'\-',"MULTIPLICACION":'\*', "DIVISION":'\/', "PUNTOCOMA":'\;',"COMA":'\,', "LLAVEA":'\{', "LLAVEC":'\}', "PUNTO":'\.', "DOSPUNTOS":'\:',"DISYUNCION":'\|',}
    error = []
    linea = 0
    columna = 0 
    contador = 0
    salida = ""
    path = ""

    '''
        Estructura de los tokens
        [LINES, COLUMNA, ID, CONTENIDO]
    '''

    def __init__(self, texto, filename, ventana):
        print("Analizador JavaScript\n")
        ventana.salida.insert(INSERT,"Preparando Analizador de archivos JS...\n\n")
        # OBTENEMOS LA RUTA DE ARCHIVOS
        self.path = self.getPath(texto)
        # CREAMOS EL DIRECTORIO DE ARCHIVOS DADO
        os.makedirs(self.path,exist_ok=True)
        # OBTENEMOS EL LISTADO DE TOKENS
        self.error.clear()
        listado = self.scanner(texto)
        # IDENTIFICAMOS RESERVADAS
        self.Identificacion(listado)
        # ESCRIBIMOS LA SALIDA LIMPIA
        fileCleaned = open(self.path+filename,"w")
        fileCleaned.write(self.salida)
        fileCleaned.close()
        # ECRIBIMOS EL REPORTE DE ERRORES
        fileError = open(self.path+"errores.html","w")
        fileError.write(self.reporteErrores())
        fileError.close()
        # INGRESAMOS LOS TOKENS A LA CONSOLA SALIDA
        tokensrt = ""
        for token in listado:      
            tokensrt += "Linea: "+str(token[0])+" Columna: "+str(token[1])+" Tipo: "+str(token[2])+" Valor: "+str(token[3])+"\n" 
        ventana.salida.insert(INSERT,tokensrt)

# -> /home/user/output/js/
    def getPath(self, texto):
        inicio = texto.find("PATHL") + 5
        while inicio < len(texto):
            if (ord(texto[inicio]) == 47): # TRIGGER DE MI PATH
                #self.path += texto[inicio]
                return self.pathBstate(inicio,texto,texto[inicio])
            else:
                inicio += 1
            
    def pathBstate(self, inicio, texto, path):
        inicio += 1
        if (ord(texto[inicio]) >= 65 and ord(texto[inicio]) <= 90) or (ord(texto[inicio]) >= 97 and ord(texto[inicio]) <= 122): # TRIGGER DE PATH
            return self.pathCstate(inicio, texto, path+texto[inicio])
        else:
            return path
# -> /home/user/output/js/=   
    def pathCstate(self, inicio, texto, path):
        inicio += 1
        if (ord(texto[inicio]) >= 65 and ord(texto[inicio]) <= 90) or (ord(texto[inicio]) >= 97 and ord(texto[inicio]) <= 122): # MANTENER EN C
            return self.pathCstate(inicio,texto, path+texto[inicio])
        elif (ord(texto[inicio]) == 47): # IRTE A B
            return self.pathBstate(inicio,texto,path+texto[inicio])
    
    def scanner(self, texto):
        self.linea = 1
        self.columna = 1
        self.contador = 0
        tokens = []

        # ESTADO 0
        while self.contador < len(texto):
            if (ord(texto[self.contador]) >= 65 and ord(texto[self.contador]) <= 90) or (ord(texto[self.contador]) >= 97 and ord(texto[self.contador]) <= 122):  # PARA INDENTIFICADORES -> ESTADO B
                self.salida += texto[self.contador]
                tokens.append(self.Identificador(self.linea,self.columna,texto,texto[self.contador]))
            elif (ord(texto[self.contador]) >= 48 and ord(texto[self.contador]) <= 57):  # PARA NUMEROS ENTEROS O DECIMALES -> ESTADO C
                self.salida += texto[self.contador]
                tokens.append(self.Numero(self.linea,self.columna,texto,texto[self.contador]))
            elif (ord(texto[self.contador]) == 10):   # PARA SALTOS DE LINEA
                self.salida += texto[self.contador]
                self.linea += 1
                self.columna = 1
                self.contador += 1
                
            elif (ord(texto[self.contador]) == 32) or (ord(texto[self.contador]) == 9):  # PARA ESPACIOS O TABULACIONES
                self.salida += texto[self.contador]
                self.contador += 1
                self.columna += 1
                
            elif (ord(texto[self.contador]) == 47):    # PARA COMENTARIOS DE UNA LINEA, MULTILINEA o SIGNO / -> ESTADO D
                self.salida += texto[self.contador]
                tokens.append(self.Slash(self.linea,self.columna,texto,texto[self.contador]))
            else: # SIGNOS
                signo = False
                for clave in self.Signos:
                    valor = self.Signos[clave]           
                    if valor.replace('\\','') == texto[self.contador]:
                        tokens.append([self.linea, self.columna, clave, valor.replace('\\','')])
                        self.salida += texto[self.contador]
                        self.contador += 1
                        self.columna += 1
                        signo = True
                        break
                if not signo:
                    self.error.append([self.linea, self.columna, texto[self.contador]])
                    self.columna += 1
                    self.contador += 1
                   
        return tokens
      
    # ESTADO 1 
    def Identificador(self, Ilinea, Icolumna, texto, lexema):
        self.columna += 1
        self.contador += 1
        if self.contador < len(texto):
            if (ord(texto[self.contador]) >= 65 and ord(texto[self.contador]) <= 90) or (ord(texto[self.contador]) >= 97 and ord(texto[self.contador]) <= 122) or (ord(texto[self.contador]) >= 48 and ord(texto[self.contador]) <= 57) or (ord(texto[self.contador]) == 95): # IDENTIFICADOR
                self.salida += texto[self.contador]
                return self.Identificador(Ilinea, Icolumna, texto, lexema+texto[self.contador])
            else:
                self.GraficarIdentificador(self.path)
                return [Ilinea, Icolumna, 'IDENTIFICADOR', lexema]
        else:
            self.GraficarIdentificador(self.path)
            return [Ilinea, Icolumna, 'IDENTIFICADOR', lexema]

    # ESTADO 2
    def Numero(self, Ilinea, Icolumna, texto, lexema):
        self.columna += 1
        self.contador += 1
        if self.contador < len(texto):
            if (ord(texto[self.contador]) >= 48 and ord(texto[self.contador]) <= 57): # ENTERO
                self.salida += texto[self.contador]
                return self.Numero(Ilinea, Icolumna, texto, lexema+texto[self.contador])
            elif (ord(texto[self.contador]) == 46): # DECIMAL
                self.salida += texto[self.contador]
                return self.Decimal(Ilinea, Icolumna, texto, lexema+texto[self.contador])
            else:
                return [Ilinea, Icolumna, "ENTERO", lexema]
        else:
            return [Ilinea, Icolumna, "ENTERO", lexema]

    # ESTADO 3
    def Decimal(self, ILinea, Icolumna, texto, lexema):
        self.columna += 1
        self.contador += 1
        if self.contador < len(texto):
            if (ord(texto[self.contador]) >= 48 and ord(texto[self.contador]) <= 57): # DECIMAL
                self.salida += texto[self.contador]
                return self.Decimal(ILinea,Icolumna,texto,lexema+texto[self.contador])
            else:
                self.GraficarDecimales(self.path)
                return [ILinea,Icolumna, "DECIMAL", lexema]
        else:
            self.GraficarDecimales(self.path)
            return [ILinea, Icolumna, "DECIMAL", lexema]
    
    # ESTADO 4
    def Slash(self, ILinea, Icolumna, texto, lexema):
        self.columna += 1
        self.contador += 1
        if self.contador < len(texto):
            if (ord(texto[self.contador]) == 47): # COMENTARIO UNA LINEA
                self.salida += texto[self.contador]
                return self.Unilinea(ILinea,Icolumna,texto, lexema+texto[self.contador])
            elif (ord(texto[self.contador]) == 42): # COMENTARIO MULTILINEA
                self.salida += texto[self.contador]
                return self.Multilinea(ILinea,Icolumna,texto, lexema+texto[self.contador], False)
            else: # SIGNO /
                return [ILinea, Icolumna, "DIVISION", lexema]
        else:   # SIGNO /
            return [ILinea, Icolumna, "DIVISION", lexema]
    
    # ESTADO 5
    def Unilinea(self, Ilinea, Icolumna, texto, lexema):
        self.contador += 1
        self.columna += 1
        if self.contador < len(texto):
            if (ord(texto[self.contador]) == 10): #TERMINA EL COMENTARIO UNILINIA
                return [Ilinea, Icolumna, "COMENTARIO", lexema]
            else:
                self.salida += texto[self.contador]
                return self.Unilinea(Ilinea,Icolumna,texto,lexema+texto[self.contador])
        else:
            return [Ilinea, Icolumna, "COMENTARIO", lexema]
    
    # ESTADO 6
    def Multilinea(self, Ilinea, Icolumna, texto, lexema, bandera):
        self.contador += 1
        self.columna += 1
        if self.contador < len(texto):
            if (ord(texto[self.contador]) == 42): #BANDERA PARA TERMINAR EL COMENTARIO
                self.salida += texto[self.contador]
                return self.Multilinea(Ilinea,Icolumna,texto,lexema+texto[self.contador],True)
            elif (ord(texto[self.contador]) == 47): 
                if bandera: # SI SE DETECTO * ANTES SE TERMINA EL COMENTARIO MULTILINEA
                    self.salida += texto[self.contador]
                    self.contador += 1
                    self.columna += 1
                    self.GraficarComentarios(self.path)
                    return [Ilinea, Icolumna, "MCOMENTARIO", lexema+texto[self.contador-1]]
                else: # NO SE DETECTO * SE CONTINUA CONCATENANDO PERO RECONOCEMOS UNA RUTA
                    self.salida += texto[self.contador]
                    return self.Multilinea(Ilinea,Icolumna,texto,lexema+texto[self.contador],False)
            elif (ord(texto[self.contador]) == 10): # SE CONCATENAN LOS SALTOS DE LINEA
                self.salida += texto[self.contador]
                self.columna = 1
                self.linea += 1
                return self.Multilinea(Ilinea,Icolumna,texto,lexema+texto[self.contador], False)
            else: # SI NO ES * (ACTIVA LA BANDERA DE TERMINAR) O / (TERMINA) SE SIGUE CONCATENANDO
                self.salida += texto[self.contador]
                return self.Multilinea(Ilinea,Icolumna,texto,lexema+texto[self.contador], False)
        else:
            self.GraficarComentarios(self.path)
            return [Ilinea, Icolumna, "MCOMENTARIO", lexema]
    
    def reporteErrores(self):
        reporte = "<table border='1'>\n<tr>\n<td>Linea</td>\n<td>Columna</td>\n<td>Caracter</td>\n</tr>\n"
        for err in self.error:
            reporte += "<tr>\n<td>"+str(err[0])+"</td>\n<td>"+str(err[1])+"</td>\n<td>"+str(err[2])+"</td>\n</tr>\n"
        reporte += "</table>"
        return reporte
           
    def Identificacion(self, tokenList):
        for token in tokenList:
            if token[2] == 'IDENTIFICADOR':
                for reservada in self.Reservadas:
                    if reservada == token[3]:
                        token[2] = 'RESERVADA'
                        break

    comentariosMultilinea = '''digraph finite_state_machine {\n
	rankdir=LR;\n
	size="8,5"\n
	node [shape = doublecircle]; S4;\n
	node [shape = circle];\n
	S0 -> S1 [label="/"];\n
    S1 -> S2 [label="*"];\n
    S2 -> S2 [label="C"];\n
    S2 -> S3 [label="*"];\n
    S3 -> S2 [label="C"];\n
    S3 -> S4 [label="/"];\n
    }'''

    digitosDecimales = '''digraph finite_state_machine {
	rankdir=LR;
	size="8,5"
	node [shape = doublecircle]; S3;
	node [shape = circle];
	S0 -> S1 [label="D"];
    S1 -> S1 [label="D"];
    S1 -> S2 [label="."];
    S2 -> S3 [label="D"];
    S3 -> S3 [label="D"];
    }'''

    identificador = '''digraph finite_state_machine {
	rankdir=LR;
	size="8,5"
	node [shape = doublecircle]; S1;
	node [shape = circle];
	S0 -> S1 [label="L"];
    S1 -> S1 [label="L | _ | D"];
    }'''


    def GraficarIdentificador(self, path):
        # CREAMOS EL DIRECTORIO DE ARCHIVOS DADO
        os.makedirs(path,exist_ok=True)
        # ESCRIBIMOS LA SALIDA LIMPIA
        fileCleaned = open("id.dot","w")
        fileCleaned.write(self.identificador)
        fileCleaned.close()
        # EXECUTAMOS UN COMANDO DOT
        os.system("dot -Tpng id.dot -o "+path+"id.png")
        os.remove("id.dot")

    def GraficarDecimales(self, path):
        # CREAMOS EL DIRECTORIO DE ARCHIVOS DADO
        os.makedirs(path,exist_ok=True)
        # ESCRIBIMOS LA SALIDA LIMPIA
        fileCleaned = open("decimales.dot","w")
        fileCleaned.write(self.digitosDecimales)
        fileCleaned.close()
        # EXECUTAMOS UN COMANDO DOT
        os.system("dot -Tpng decimales.dot -o "+path+"decimales.png")
        os.remove("decimales.dot")

    def GraficarComentarios(self, path):
        # CREAMOS EL DIRECTORIO DE ARCHIVOS DADO
        os.makedirs(path,exist_ok=True)
        # ESCRIBIMOS LA SALIDA LIMPIA
        fileCleaned = open("comment.dot","w")
        fileCleaned.write(self.comentariosMultilinea)
        fileCleaned.close()
        # EXECUTAMOS UN COMANDO DOT
        os.system("dot -Tpng comment.dot -o "+path+"comment.png")
        os.remove("comment.dot")