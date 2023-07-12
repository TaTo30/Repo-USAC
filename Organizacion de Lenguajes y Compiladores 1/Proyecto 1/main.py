from tkinter import *
from tkinter import ttk
from tkinter.filedialog import askopenfilename
from tkinter.filedialog import asksaveasfilename
from tkinter import messagebox
import Analizadores.JSScanner as JS
import Analizadores.HTMLScanner as HTML
import Analizadores.CSScanner as CSS
import Analizadores.RMTScanner as RMT
import os

class main:
    '''
    METODO QUE ME INICIA LA VENTANA
    '''
    def __init__(self, window):
        self.ventana = window
        self.ventana.title("Proyecto 1")

        frame = LabelFrame(self.ventana, text = '')
        frame.grid(row=1,column=0,columnspan=20,pady=10)

        framebutton = LabelFrame(self.ventana, text = 'Opciones')
        framebutton.grid(row = 0, column = 0, columnspan=20, pady=5)

        #############################################_MENU_#############################################
        self.cargar = Button(framebutton, text ="Cargar", command = self.Cargar)
        self.cargar.grid(row=0,column=0)

        self.nuevo = Button(framebutton, text = "Nuevo", command = self.Nuevo)
        self.nuevo.grid(row=0,column=1)

        self.ejecutar = Button(framebutton, text ="Ejecutar", command = self.Analizar)
        self.ejecutar.grid(row=0,column=2)

        self.nuevo = Button(framebutton, text = "Guardar", command = self.Guardar)
        self.nuevo.grid(row=0,column=3)

        self.salir = Button(framebutton, text ="Salir", command = self.Terminar)
        self.salir.grid(row=0,column=4)

        self.nuevo = Button(framebutton, text = "Guardar Como", command = self.GuardarComo)
        self.nuevo.grid(row=0,column=5)
        

        ############################################_ENTRADA_############################################
        Label(frame,text='Archivo de Entrada:').grid(row=3,column=5)
        self.entrada = Text(frame, height=30, width=70)
        self.entrada.grid(row=4,column=5)
        self.entrada.delete("1.0",END)

        Label(frame,text='   =>   ').grid(row=4,column=18)

        Label(frame,text='Resultado:').grid(row=3,column=20)
        self.salida = Text(frame, height=30, width=70)
        self.salida.grid(row=4,column=20)

        Label(frame,text='              ').grid(row=3,column=20)
    #END

    '''
    CARGA MI ARCHIVO DE ENTRADA A MI TEXTBOX
    '''
    def Cargar(self):       
        filename = askopenfilename()
        self.rutaEntrada = filename
        archivo = open(filename,"r")
        texto = archivo.read()
        archivo.close()
        self.Nuevo()
        self.entrada.insert(INSERT,texto)        
        return
    #END

    '''
    INICIA MI ANALISIS DEL ARCHIVO DE ENTRADA
    '''
    def Analizar(self):
        filename = os.path.basename(self.rutaEntrada)
        texto = self.entrada.get("1.0",END)
        if self.rutaEntrada.endswith("js"):
            JS.jsScanner(texto, filename, self)
        elif self.rutaEntrada.endswith("css"):
            CSS.cssScanner(texto, filename, self)
        elif self.rutaEntrada.endswith("html"):
            HTML.htmlScanner(texto, filename, self)
        elif self.rutaEntrada.endswith("rmt"):
            RMT.rmtScanner(texto, self)
        else:
            print("La ruta no especifica el lenguaje a analizar")
        # JS.jsScanner(self.rutaEntrada)
    #END

    '''
    TERMINA LA EJECUCION DEL PROGRAMA
    '''
    def Terminar(self):
        self.ventana.destroy()
        return
    #ENDs

    def Nuevo(self):
        self.entrada.delete("1.0",END)
        self.salida.delete("1.0",END)
        return
    
    def Guardar(self):
        ward = open(self.rutaEntrada,"w")
        ward.write(self.entrada.get("1.0",END))
        ward.close()

    def GuardarComo(self):
        filename = asksaveasfilename()
        archivo = open(filename,"w")
        archivo.write(self.entrada.get("1.0",END))
        archivo.close()
#END

###################################################################################################
if __name__ == '__main__':
    window = Tk()
    app = main(window)
    window.mainloop()
    