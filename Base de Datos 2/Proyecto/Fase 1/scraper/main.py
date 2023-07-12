from gc import callbacks
import time
import scrapy


class QuotesSpider(scrapy.Spider):
    name = "quotes"
    paises=[]

    def __init__(self, name=None, **kwargs):
        super().__init__(name, **kwargs)
        self.paises = []
        self.jugadores=[]

    def start_requests(self):
        urls = [
            'https://www.losmundialesdefutbol.com/mundiales.php',
        ]
        for url in urls:
            yield scrapy.Request(url=url, callback=self.parse)

    def parse(self, response):
        #limpiando archivos
        self.cleanFiles()
        # recolectando todos los mundiales
        mundiales =response.css('td.a-top div.a-center a::attr(href)').getall()
        premiosMundiales = response.css('div.left.margen-r5.margen-l5 a::attr(href)').getall()
        
        # for link in premiosMundiales:
        #     if str(link).find("premios") != -1:
        #         yield response.follow(link, callback=self.parse_premios)
                
        i=0
        for mundial in mundiales:
            i+=1
            if i!=1:
                yield response.follow(mundial, callback=self.parse_mundial)
                
                

    def cleanFiles(self):
        print("")
        with open('mundiales.json', 'w') as f:
            f.write("[")
        # with open('paises.json', 'w') as f:
        #     f.write("[")
        # with open('posiciones.json', 'w') as f:
        #     f.write("[")
        # with open("jugadores.json","w") as f:
        #     f.write("[")
        # with open('goleadores.csv', 'w') as f:
        #     f.write("Mundial,Jugador,Goles,Partidos,Promedio,Pais\n")
        # with open('premios.csv', 'w') as f:
        #     f.write("mundial,jugador,premio\n")

    
    # PARA LA TABLA MUNDIAL
    def dataMundial(self,cadena,subcadena):
        text="{"
        posiciones=[]
        posicion=0
        while posicion != -1:
            posicion = cadena.find(subcadena,posicion)
            if posicion != -1:
                posiciones.append(posicion)
                posicion +=1
        posicion = cadena.find("Organizador")
        text+="\"organizador\":\""+cadena[posicion+13:posiciones[0]]+"\","

        posicion = cadena.find("Selecciones")
        text+="\"selecciones\":"+cadena[posicion+13:posiciones[1]].replace(" ","")+","

        posicion = cadena.find("Partidos")
        text+="\"partidos\":"+cadena[posicion+10:posiciones[2]].replace("(0 ya jugados)","")+","

        posicion = cadena.find("Goles")
        text+="\"goles\":"+cadena[posicion+7:posiciones[3]].replace(" ","")+","

        posicion = cadena.find("Promedio")
        text+="\"promedio_gol\":"+cadena[posicion+17:posiciones[4]].replace(" ","")+","

        return text

    def parse_mundial(self,response):
        preData = response.css('div.rd-100-50.a-left.clearfix div.margen-xauto p').get()
        a = str(preData).replace("<br>","").replace("<p class=\"margen-l10\">\n","")
        dat = self.dataMundial(a,"\n")
        date_mundial=str(response.css('h1.tc-1::text').get()).replace("Mundial de Fútbol ","")
        # escribe el archivo
        with open('mundiales.json', 'a') as f:
            f.write(str(dat)+"\"año\":"+date_mundial+"},\n")

        # LLENAMOS EL ARCHIVO DE PAISES
        # self.parse_pais(response)

        # LLENAMOS EL ARCHIVO PARA POSICIONES
        # tabla = response.css('p.a-right.pad-r10.ita.margen-t3 a::attr(href)').get()
        # if date_mundial!="2022":
        #     yield response.follow(tabla, callback=self.parse_posiciones)

        # LLENAR LA TABLA GOLES
        # if date_mundial!="2022":
        #     link = response.css('p.a-right.pad-r10.ita.margen-t3 a::attr(href)').getall()
        #     yield response.follow(link[-1], callback=self.goles)
        
    # PARA LA TABLA PAIS
    def parse_pais(self,response):
        predata = response.css('div.margen-y15.a-center.clearfix.max-4 a::text').getall()
        aux = []
        for pais in predata:
            aux.append(str(pais).replace(".",""))

        for pais in aux:
            if pais not in self.paises:
                self.paises.append(pais)
                print(pais)
                with open('paises.json', 'a') as f:
                    f.write("{\"nombre\":\""+str(pais)+"\"},\n")
    
        
    # PARA TABLA POSICIONES
    def parse_posiciones(self,response):
        preData = response.css('tr.a-top td.a-left a::text').getall()
        resultado={}
        resultado[0]=str(response.css('h1.tc-1::text').get()).replace("Mundial de Fútbol ","").replace("Posiciones Finales del ","")
        # f= open('posiciones.json', 'a')
        # for i in range(len(preData)):
        #     resultado[i+1]=str(preData[i])[1:-1]
        #     aux="{\"posicion\":"+str(i+1)+",\"seleccion\":\""+str(preData[i])[1:-1]+"\",\"mundial\":"+resultado[0]+"},\n"
        #     f.write(aux)
        # f.close()

        # JUGADORES POR PAIS  TARDA MUCHO Y LO COMENTE
        # linkPaises = response.css('tr.a-top td.a-left a::attr(href)').getall()
        # for pais in linkPaises:
        #     yield response.follow(pais, callback=self.parse_jugadores)
        #     break
            

    #JUGADORES POR PAIS
    def parse_jugadores(self,response):
        time.sleep(0.13)
        preData = response.css('div.pad-y5.clearfix.bb-2 div.left-sm.w-40-sm div.overflow-x-auto a::attr(href)').getall()
        for jugador in preData:
            yield response.follow(jugador, callback=self.parse_jugador)
            

    def parse_jugador(self,response):
        time.sleep(0.08)
        preData = response.css('table.w-auto.margen-xauto tr td::text').getall()
        columnas = []
        for a in response.css('table.w-auto.margen-xauto tr td b::text').getall():
            columnas.append(str(a).replace(":","").strip())
        nombreJugador = str(response.css('h1.tc-1::text').get()).replace(" en los Mundiales de Fútbol","")
        nacionalidad = str(response.css("p.margen-b5 a::text").get())
        tw = response.css("table.w-auto.margen-xauto tr td a::attr(href)").get()
        texto=nombreJugador+","
        data="{\"nombre\":\""+nombreJugador+"\",\"fecha_nacimiento\":\""
        if nombreJugador not in self.jugadores:
            if columnas[0]=="Nombre completo": columnas=columnas[1:-1]
            enTablaColumnas = ['Fecha de Nacimiento','Lugar de nacimiento','Posición','Números de camiseta','Altura','Apodo','Twitter']
            self.jugadores.append(nombreJugador)
            for i in range(len(enTablaColumnas)):
                colActual = enTablaColumnas[i]
                if i == 6:
                    if tw is not None:
                        texto+=str(tw)+","
                    else: texto+=","
                elif colActual in columnas:
                    j = columnas.index(colActual)
                    content = str(preData[j]).strip().replace(","," ").replace(" mts.","")
                    texto+=content+","
                else: texto+=","
            # print(columnas)
            with open('jugadores.csv', 'a') as f:
                    texto+=nacionalidad+"\n"
                    f.write(texto)

    # GOLEADORES
    def goles(self,response):
        mundial=str(response.css('nav.breadcrumb a::text').getall()[-1])
        nombres = response.css('table.c0s5.a-center tr.a-top td a::text').getall()
        goles = response.css('table.c0s5.a-center tr.a-top td b::text').getall()

        otraData=response.css('table.c0s5.a-center tr.a-top td::text').getall()
        pre=[]
        for i in otraData:
            i=str(i).replace("\t","").replace("\r","").replace("\n","")
            if i != "" and i[-1]!=".":
                pre.append(i)
        auxData={}
        resultData=[]
        for i in range(len(pre)):
            if ((i+1) % 3) == 0:
                auxData={}
                auxData["partidos"]=pre[i-2]
                auxData["promedio"]=pre[i-1]
                auxData["seleccion"]=pre[i]
                resultData.append(auxData)

        for i in range(len(nombres)):
            aux=""
            with open('goleadores.csv', 'a') as f:
                aux+=mundial+","+str(nombres[i]).strip()+","+goles[i]+","
                aux+=resultData[i]["partidos"]+","+resultData[i]["promedio"]+","
                aux+=resultData[i]["seleccion"]
                f.write(aux+"\n")

    # PREMIOS
    def parse_premios(self,response):
        mundial = str(response.css('h1.tc-1::text').get()).replace("Premios del Mundial de Fútbol de ","")
        preData = response.css('div.rd-100-30 p a::text').getall()
        if mundial=="2022": return
        # with open('premios.csv', 'a') as f:
        #     f.write(mundial+","+str(preData)+"\n")
