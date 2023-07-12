from tkinter import *

class rmtScanner:

    Signos = {"PARA":'\(', "PARC":'\)',"SUMA":'\+', "RESTA":'\-',"MULTIPLICACION":'\*', "DIVISION":'\/'}
    Resultados = []
    contador = 0
    contadorErrores = 0
    contadorTokens = 0



    def __init__(self, archivo:str, ventana):
        print("Analizador RMT\n")
        ventana.salida.insert(INSERT,"Preparando Analizador de archivos RMT...\n\n")
        self.Resultados.clear()
        operaciones = archivo.splitlines()
        for op in operaciones:
            if not op.__eq__(""):
                lista = self.Scanner(op)
                self.contadorErrores = 0
                self.contadorTokens = 0
                self.OP_E(lista)
                if len(lista) != self.contadorTokens:
                    self.contadorErrores += 1
                if self.contadorErrores > 0:
                    self.Resultados.append([op, 'INCORRECTO'])
                else:
                    self.Resultados.append([op, 'CORRECTO'])
        salida = ""
        for result in self.Resultados:
            salida += "Oparacion:\n"+result[0]+" \n=>\n "+result[1]+"\n\n"
        ventana.salida.insert(INSERT,salida)                 

    def Scanner(self, texto):
        self.contador = 0
        tokens = []

        # LOOP
        while self.contador < len(texto):
            if (ord(texto[self.contador]) >= 48 and ord(texto[self.contador]) <= 57) or (ord(texto[self.contador]) >= 65 and ord(texto[self.contador]) <= 90) or (ord(texto[self.contador]) >= 97 and ord(texto[self.contador]) <= 122) or (ord(texto[self.contador]) == 46):
                tokens.append(self.Expresion(texto, texto[self.contador]))
            
            else:
                signo = False
                for clave in self.Signos:
                    valor = self.Signos[clave]
                    if re.search(valor, texto[self.contador]):
                        tokens.append([clave, valor.replace('\\','')])
                        self.contador += 1
                        signo = True
                        break
                if not signo:
                    self.contador += 1
        return tokens

    def Expresion(self, texto, lexema):
        self.contador += 1
        if self.contador < len(texto):
            if (ord(texto[self.contador]) >= 48 and ord(texto[self.contador]) <= 57) or (ord(texto[self.contador]) >= 65 and ord(texto[self.contador]) <= 90) or (ord(texto[self.contador]) >= 97 and ord(texto[self.contador]) <= 122) or (ord(texto[self.contador]) == 46):
                return self.Expresion(texto,lexema+texto[self.contador])
            else:
                return ['ID', lexema]
        else:
            return ['ID', lexema]
        return ""

    
    def OP_E(self, tokens):
        self.OP_T(tokens)
        self.OP_E2(tokens)

    def OP_E2(self, tokens):
        if self.contadorTokens < len(tokens):
            token = tokens[self.contadorTokens]
            if token[0] == 'SUMA':
                self.Match(tokens,'SUMA')
                self.OP_T(tokens)
                self.OP_E2(tokens)
            elif token[0] == 'RESTA':
                self.Match(tokens,'RESTA')
                self.OP_T(tokens)
                self.OP_E2(tokens)
            else:
                pass

    def OP_T(self, tokens):
        self.OP_F(tokens)
        self.OP_T2(tokens)

    def OP_T2(self, tokens):
        if self.contadorTokens < len(tokens):
            token = tokens[self.contadorTokens]
            if token[0] == 'MULTIPLICACION':
                self.Match(tokens,'MULTIPLICACION')
                self.OP_F(tokens)
                self.OP_T2(tokens)
            elif token[0] == 'DIVISION':
                self.Match(tokens,'DIVISION')
                self.OP_F(tokens)
                self.OP_T2(tokens)
            else:
                pass

    def OP_F(self, tokens):
        if self.contadorTokens < len(tokens):
            token = tokens[self.contadorTokens]
            if token[0] == 'PARA':
                self.Match(tokens,'PARA')
                self.OP_E(tokens)
                self.Match(tokens, 'PARC')
            else:
                self.Match(tokens,'ID')
    
    
    def Match(self, tokens, tipo):
        if self.contadorTokens < len(tokens):
            token = tokens[self.contadorTokens]
            self.contadorTokens += 1
            if token[0]!=tipo:
                print("Se esperaba: "+tipo+" Se obtuvo: "+token[0])
                self.contadorErrores += 1
            
