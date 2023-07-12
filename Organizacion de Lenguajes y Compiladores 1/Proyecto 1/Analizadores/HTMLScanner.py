from tkinter import *
import os


class htmlScanner:

    Reservadas = ['html', 'head', 'title','body','h1','h2','h3','h4','h5','h6','p','br','img','a','ul','li','href','src','style','table','th','tr','td','caption','colgroup','col','thead','tbody','tfoot','border']
    Signos = {"ABRIR":'\<', "CERRAR":'\>', "DIAGONAL":'\/',"COMILLAD":'\"',"COMILLAS":'\'', "IGUAL":'\='}
    error = []
    linea = 0
    columna = 0
    contador = 0
    salida = ""
    path = ""
    etiqueta = False

    def __init__(self, texto, filename, ventana):
        print("Analizador HTML\n")
        ventana.salida.insert(INSERT,"Preparando Analizador de archivos HTML...\n\n")
        # OBTENEMOS EL LISTADO DE TOKENS
        self.error.clear()
        listado = self.scanner(texto)
        # IDENTIFICAMOS RESERVADAS
        self.Identificacion(listado)
        # OBTENEMOS LA RUTA DE ARCHIVOS
        self.path = self.getPath(texto)
        # CREAMOS EL DIRECTORIO DE ARCHIVOS DADO
        os.makedirs(self.path,exist_ok=True)
        # ESCRIBIMOS LA SALIDA LIMPIA
        fileCleaned = open(self.path+filename,"w")
        fileCleaned.write(self.salida)
        fileCleaned.close()
        # ECRIBIMOS EL REPORTE DE ERRORES
        fileError = open(self.path+"errores.html","w")
        fileError.write(self.reportesErrores())
        fileError.close()
        # INGRESAMOS LOS TOKENS A LA CONSOLA SALIDA
        tokensrt = ""
        for token in listado:      
            tokensrt += "Linea: "+str(token[0])+" Columna: "+str(token[1])+" Tipo: "+str(token[2])+" Valor: "+str(token[3])+"\n" 
        ventana.salida.insert(INSERT,tokensrt)

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
        self.contador = 0
        self.columna = 1
        tokens = []

        # LOOP DEL ESTADO INICIAL
        while self.contador < len(texto):
            if self.etiqueta:
                # ESTO SE EJECUTARA CUANDO ESTEMOS DENTRO DE UNA ETIQUETA
                if (ord(texto[self.contador]) == 10): # saltos de linea
                    self.salida += texto[self.contador]
                    self.linea += 1
                    self.contador += 1
                    self.columna = 1
                elif (ord(texto[self.contador]) == 32) or (ord(texto[self.contador]) == 9): # Espacios y tabulaciones
                    self.salida += texto[self.contador]
                    self.contador += 1
                    self.columna += 1
                elif (ord(texto[self.contador]) == 34) or (ord(texto[self.contador]) == 39): # ATRIBUTOS
                    self.salida += texto[self.contador]
                    tokens.append(self.Cadenas(self.linea,self.columna,texto,texto[self.contador]))
                elif (ord(texto[self.contador]) >= 65 and ord(texto[self.contador]) <= 90) or (ord(texto[self.contador]) >= 97 and ord(texto[self.contador]) <= 122): # IDENTIFICADORES
                    self.salida += texto[self.contador]
                    tokens.append(self.Identificador(self.linea,self.columna,texto,texto[self.contador]))
                else: # SIGNOS
                    signo = False
                    for clave in self.Signos:
                        valor = self.Signos[clave]                   
                        if valor.replace('\\','') == texto[self.contador]:
                            if (ord(texto[self.contador]) == 60):
                                self.etiqueta = True
                            elif (ord(texto[self.contador]) == 62):
                                self.etiqueta = False
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
            else:
                # ESTO SE EJECUTARA CUANDO ESTEMOS FUERA DE UNA ETIQUETA
                # FUERA DE LA ETIQUETA TODO CUENTA COMO UN TEXTO
                if (ord(texto[self.contador]) == 10): # saltos de linea
                    self.salida += texto[self.contador]
                    self.linea += 1
                    self.contador += 1
                    self.columna = 1
                elif (ord(texto[self.contador]) == 32) or (ord(texto[self.contador]) == 9): # Espacios y tabulaciones
                    self.salida += texto[self.contador]
                    self.contador += 1
                    self.columna += 1
                elif (ord(texto[self.contador]) == 60):
                    self.etiqueta = True
                else:
                    self.salida += texto[self.contador]
                    tokens.append(self.Textos(self.linea, self.columna, texto, texto[self.contador]))
                
        return tokens

    def Textos(self, Ilinea, Icolumna, texto, lexema):
        self.contador += 1
        self.columna += 1
        if self.contador < len(texto):
            if (ord(texto[self.contador]) == 60):
                self.etiqueta = True
                return [Ilinea,Icolumna,'TEXTO', lexema]
            else:
                self.salida += texto[self.contador]
                return self.Textos(Ilinea,Icolumna,texto, lexema+texto[self.contador])
        else:
            return [Ilinea, Icolumna, 'TEXTO', lexema]
    
    def Cadenas(self, Ilinea, Icolumna, texto, lexema):
        self.contador += 1
        self.columna += 1
        if self.contador < len(texto):
            if (ord(texto[self.contador]) == 34) or (ord(texto[self.contador]) == 39): # TERMINA LA CADENA 
                self.salida += texto[self.contador]
                self.contador += 1
                self.columna += 1
                return [Ilinea, Icolumna, 'ATRIBUTO', lexema+texto[self.contador-1]]
            else: # SE CONCATENA CUALQUIER COSA
                self.salida += texto[self.contador]
                return self.Cadenas(Ilinea,Icolumna,texto, lexema+texto[self.contador])                
        else:
            return [Ilinea, Icolumna, 'ATRIBUTO', lexema]

    def Identificador(self, Ilinea, Icolumna, texto, lexema):
        self.columna += 1
        self.contador += 1
        if self.contador < len(texto):
            if (ord(texto[self.contador]) >= 65 and ord(texto[self.contador]) <= 90) or (ord(texto[self.contador]) >= 97 and ord(texto[self.contador]) <= 122) or (ord(texto[self.contador]) >= 48 and ord(texto[self.contador]) <= 57) or (ord(texto[self.contador]) == 95): # IDENTIFICADOR
                self.salida += texto[self.contador]
                return self.Identificador(Ilinea, Icolumna, texto, lexema+texto[self.contador])
            else:
                return [Ilinea, Icolumna, 'IDENTIFICADOR', lexema]
        else:
            return [Ilinea, Icolumna, 'IDENTIFICADOR', lexema]

    def reportesErrores(self):
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