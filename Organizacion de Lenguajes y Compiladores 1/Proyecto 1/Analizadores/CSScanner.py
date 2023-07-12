from tkinter import *
import os

class cssScanner:

    Reservadas = ['color', 'border', 'text-align','font-weight','padding-left','padding-top','line-height','margin-top','margin-left','display','top','float','min-width','background-color','Opacity','font-family','font-size','padding-right','padding','width','margin-right','margin','position','right','clear','max-height','background-image','background','font-style','font','padding-bottom','margin-bottom','border-style','bottom','left','max-width','min-height']
    Signos = {"COMILLASDOBLES":'\"', "COMILLAS":'\'', "PARA":'\(',"PARC":'\)',"SUMA":'\+',"RESTA":'\-',"MULTIPLICACION":'\*', "DIVISION":'\/', "PUNTOCOMA":'\;',"COMA":'\,', "LLAVEA":'\{', "LLAVEC":'\}', "PUNTO":'\.', "DOSPUNTOS":'\:', "PORCENTAJE":'\%',"NUMERAL":'\#'}
    error = []
    linea = 0
    columna = 0
    contador = 0
    salida = ""
    path = ""
    bitacora = ""

    '''
        Estructura de los tokens
        [LINES, COLUMNA, ID, CONTENIDO]
    '''

    def __init__(self, texto, filename, ventana):
        print("Analizador CSS\n")
        ventana.salida.insert(INSERT,"Preparando Analizador de archivos CSS...\n\n")
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
        '''tokensrt = ""
        for token in listado:      
            tokensrt += "Linea: "+str(token[0])+" Columna: "+str(token[1])+" Tipo: "+str(token[2])+" Valor: "+str(token[3])+"\n" 
        ventana.salida.insert(INSERT,tokensrt)'''
        # REPORTE DE TRANSICIONES
        ventana.salida.delete("1.0",END)
        self.bitacora = "Bitacora de transiciones \n\n"+self.bitacora
        ventana.salida.insert(INSERT,self.bitacora)

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
    
    # ESTADO 0
    def scanner(self, texto):
        self.linea = 1
        self.contador = 0
        self.columna = 1
        tokens = []

        # ESTADO 0
        while self.contador < len(texto):
            if (ord(texto[self.contador]) == 10): # SALTOS DE LINEA
                self.salida += texto[self.contador]
                self.linea += 1
                self.contador += 1
                self.columna = 1
            elif (ord(texto[self.contador]) == 32) or (ord(texto[self.contador]) == 9): # TABULACIONES Y ESPACIOS
                self.salida += texto[self.contador]
                self.contador += 1
                self.columna += 1
            elif (ord(texto[self.contador]) == 47): # COMENTARIOS -> ESTADO 5
                self.bitacora += "S0 -> S5 CON "+texto[self.contador]+"\n"
                self.salida += texto[self.contador]
                tokens.append(self.Slash(self.linea,self.columna,texto,texto[self.contador]))
            elif (ord(texto[self.contador]) == 34): # CADENAS
                self.bitacora += "S0 -> S1 CON "+texto[self.contador]+"\n"
                self.salida += texto[self.contador]
                tokens.append(self.Cadenas(self.linea,self.columna,texto,texto[self.contador]))
            elif (ord(texto[self.contador]) >= 48 and ord(texto[self.contador]) <= 57): # DIGITOS
                self.bitacora += "S0 -> S3 CON "+texto[self.contador]+"\n"
                self.salida += texto[self.contador]
                tokens.append(self.Numero(self.linea,self.columna,texto,texto[self.contador]))
            elif (ord(texto[self.contador]) >= 65 and ord(texto[self.contador]) <= 90) or (ord(texto[self.contador]) >= 97 and ord(texto[self.contador]) <= 122): #IDENTIFIACODRES
                self.bitacora += "S0 -> S2 CON "+texto[self.contador]+"\n"
                self.salida += texto[self.contador]
                tokens.append(self.Identificador(self.linea,self.columna,texto,texto[self.contador]))
            else: # SIGNOS
                signo = False
                for clave in self.Signos:
                    valor = self.Signos[clave]                 
                    if valor.replace('\\','') == texto[self.contador]:
                        self.bitacora += "S0 -> S0 CON "+texto[self.contador]+"\n"
                        self.bitacora += "SE ACEPTA TOKEN EN S0: "+valor.replace('\\','')+" TIPO "+clave+"\n"
                        tokens.append([self.linea, self.columna, clave, valor.replace('\\','')])
                        self.salida += texto[self.contador]
                        self.contador += 1
                        self.columna += 1
                        signo = True
                        break
                if not signo:
                    self.bitacora += "ERROR EN S0 NO SE RECONOCE: "+texto[self.contador]+"\n"
                    self.error.append([self.linea, self.columna, texto[self.contador]])
                    self.columna += 1
                    self.contador += 1
        return tokens

    # ESTADO 1  
    def Cadenas(self, Ilinea, Icolumna, texto, lexema):
        self.contador += 1
        self.columna += 1
        if self.contador < len(texto):
            if (ord(texto[self.contador]) == 34): # TERMINA LA CADENA 
                self.bitacora += "SE ACEPTA TOKEN EN S1: "+lexema+texto[self.contador]+" TIPO cadena\n"
                self.salida += texto[self.contador]
                self.contador += 1
                self.columna += 1
                return [Ilinea, Icolumna, 'CADENA', lexema+texto[self.contador-1]]
            else: # SE CONCATENA CUALQUIER COSA
                self.salida += texto[self.contador]
                self.bitacora += "S1 -> S1 CON "+texto[self.contador]+"\n"
                return self.Cadenas(Ilinea,Icolumna,texto, lexema+texto[self.contador])                
        else:
            return [Ilinea, Icolumna, 'CADENA', lexema]

    # ESTADO 2
    def Identificador(self, Ilinea, Icolumna, texto, lexema):
        self.columna += 1
        self.contador += 1
        if self.contador < len(texto):
            if (ord(texto[self.contador]) >= 65 and ord(texto[self.contador]) <= 90) or (ord(texto[self.contador]) >= 97 and ord(texto[self.contador]) <= 122) or (ord(texto[self.contador]) >= 48 and ord(texto[self.contador]) <= 57) or (ord(texto[self.contador]) == 95): # IDENTIFICADOR
                self.bitacora += "S2 -> S2 CON "+texto[self.contador]+"\n"
                self.salida += texto[self.contador]
                return self.Identificador(Ilinea, Icolumna, texto, lexema+texto[self.contador])
            else:
                self.bitacora += "SE ACEPTA TOKEN EN S2: "+lexema+" TIPO identificador\n"
                return [Ilinea, Icolumna, 'IDENTIFICADOR', lexema]
        else:
            self.bitacora += "SE ACEPTA TOKEN EN S2: "+lexema+" TIPO identificador\n"
            return [Ilinea, Icolumna, 'IDENTIFICADOR', lexema]

    # ESTADO 3
    def Numero(self, Ilinea, Icolumna, texto, lexema):
        self.columna += 1
        self.contador += 1
        if self.contador < len(texto):
            if (ord(texto[self.contador]) >= 48 and ord(texto[self.contador]) <= 57): # ENTERO
                self.bitacora += "S3 -> S3 CON "+texto[self.contador]+"\n"
                self.salida += texto[self.contador]
                return self.Numero(Ilinea, Icolumna, texto, lexema+texto[self.contador])
            elif (ord(texto[self.contador]) == 46): # DECIMAL
                self.bitacora += "S3 -> S4 CON "+texto[self.contador]+"\n"
                self.salida += texto[self.contador]
                return self.Decimal(Ilinea, Icolumna, texto, lexema+texto[self.contador])
            else:
                self.bitacora += "SE ACEPTA TOKEN EN S3: "+lexema+" TIPO entero\n"
                return [Ilinea, Icolumna, "ENTERO", lexema]
        else:
            self.bitacora += "SE ACEPTA TOKEN EN S3: "+lexema+" TIPO entero\n"
            return [Ilinea, Icolumna, "ENTERO", lexema]

    # ESTADO 4
    def Decimal(self, ILinea, Icolumna, texto, lexema):
        self.columna += 1
        self.contador += 1
        if self.contador < len(texto):
            if (ord(texto[self.contador]) >= 48 and ord(texto[self.contador]) <= 57): # DECIMAL
                self.bitacora += "S4 -> S4 CON "+texto[self.contador]+"\n"
                self.salida += texto[self.contador]
                return self.Decimal(ILinea,Icolumna,texto,lexema+texto[self.contador])
            else:
                self.bitacora += "SE ACEPTA TOKEN EN S4: "+lexema+" TIPO decimal\n"
                return [ILinea,Icolumna, "DECIMAL", lexema]
        else:
            self.bitacora += "SE ACEPTA TOKEN EN S4: "+lexema+" TIPO decimal\n"
            return [ILinea, Icolumna, "DECIMAL", lexema]
  
    # ESTADO 5
    def Slash(self, ILinea, Icolumna, texto, lexema):
        self.columna += 1
        self.contador += 1
        if self.contador < len(texto):
            if (ord(texto[self.contador]) == 42): # COMENTARIO 
                self.bitacora += "S5 -> S6 CON "+texto[self.contador]+"\n"
                self.salida += texto[self.contador]
                return self.Multilinea(ILinea,Icolumna,texto, lexema+texto[self.contador], False)
            else: # SIGNO /
                self.bitacora += "SE ACEPTA TOKEN EN S5: "+lexema+" TIPO DIVISION\n"
                return [ILinea, Icolumna, "DIVISION", lexema]
        else:   # SIGNO /
            self.bitacora += "SE ACEPTA TOKEN EN S5: "+lexema+" TIPO DIVISION\n"
            return [ILinea, Icolumna, "DIVISION", lexema]
    
    # ESTADO 6
    def Multilinea(self, Ilinea, Icolumna, texto, lexema, bandera):
        self.contador += 1
        self.columna += 1
        if self.contador < len(texto):
            if (ord(texto[self.contador]) == 42): #BANDERA PARA TERMINAR EL COMENTARIO
                self.bitacora += "S6 -> S7 CON "+texto[self.contador]+"\n"
                self.salida += texto[self.contador]
                return self.Multilinea(Ilinea,Icolumna,texto,lexema+texto[self.contador],True)
            elif (ord(texto[self.contador]) == 47): 
                if bandera: # SI SE DETECTO * ANTES SE TERMINA EL COMENTARIO
                    self.bitacora += "S7 -> S8 CON "+texto[self.contador]+"\n"
                    self.salida += texto[self.contador]
                    self.contador += 1
                    self.columna += 1
                    self.bitacora += "SE ACEPTA TOKEN EN S8: "+lexema+texto[self.contador-1]+" TIPO comentario\n"
                    return [Ilinea, Icolumna, "COMENTARIO", lexema+texto[self.contador-1]]
                else: # NO SE DETECTO * SE CONTINUA CONCATENANDO 
                    self.bitacora += "S6 -> S6 CON "+texto[self.contador]+"\n"
                    self.salida += texto[self.contador]
                    return self.Multilinea(Ilinea,Icolumna,texto,lexema+texto[self.contador],False)
            elif (ord(texto[self.contador]) == 10): # SE CONCATENAN LOS SALTOS DE LINEA
                self.salida += texto[self.contador]
                self.columna = 1
                self.linea += 1
                return self.Multilinea(Ilinea,Icolumna,texto,lexema+texto[self.contador], False)
            else: # SI NO ES * (ACTIVA LA BANDERA DE TERMINAR) O / (TERMINA) SE SIGUE CONCATENANDO
                self.bitacora += "S6 -> S6 CON "+texto[self.contador]+"\n"
                self.salida += texto[self.contador]
                return self.Multilinea(Ilinea,Icolumna,texto,lexema+texto[self.contador], False)
        else:
            self.bitacora += "SE ACEPTA TOKEN EN S8: "+lexema+" TIPO comentario\n"
            return [Ilinea, Icolumna, "COMENTARIO", lexema] 

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