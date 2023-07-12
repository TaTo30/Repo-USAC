from gc import callbacks
import time
import scrapy


class QuotesSpider(scrapy.Spider):
    name = "quotes"
    paises=[]

    def __init__(self, name=None, **kwargs):
        super().__init__(name, **kwargs)
        self.paises = []
        self.grupos=[]

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
        
        i=0
        for mundial in mundiales:
            i+=1
            if i!=1:
                yield response.follow(mundial, callback=self.parse_mundial)
                # break
                     

    def cleanFiles(self):
        with open('partidos.csv', 'w') as f:
            f.write("sleccion_a,seleccion_b,goles_a,goles_b,fecha,grupo,mundial\n")


    def parse_mundial(self,response):
        # LLENAMOS EL ARCHIVO PARA POSICIONES
        tabla = response.css('p.margen-l10 a::attr(href)').get()
        yield response.follow(tabla, callback=self.pase_resultados)

    def pase_resultados(self,response):
        tabla = response.css('div.left.a-left.wpx-170 a::attr(href)').getall()
        for etapa in tabla:
            if str(etapa) not in self.grupos:
                self.grupos.append(str(etapa))
                yield response.follow(etapa, callback=self.parse_grupo)
                

    def parse_grupo(self,response):
        time.sleep(0.13)
        solocuartos=["1954","1958","1962","1966","1970"]
        tercerfinal=["1974","1978"]
        mundial=str(response.css('nav.breadcrumb a::text').getall()[-1])
        etapa=str(response.css('nav.breadcrumb::text').getall()[-1]).replace(" > ","").replace(",","")

        fechas_partidos = response.css("div.max-1.margen-b8.bb-2 div.left.a-center.margen-b3 \
        div.left.wpx-100.a-left::text").getall()

        equi = response.css('div.game.margen-b3.clearfix div.left.margen-b2.clearfix\
            div.left::text').getall()
        equipos=[]
        par=0
        for e in range(len(equi)):
            par+=1
            if par==2:
                par=0
                aux=[]
                aux.append(equi[e-1])
                aux.append(equi[e])
                equipos.append(aux)
        
        mar = response.css('div.left.wpx-60 a::text').getall()
        marcadores=[]
        for m in mar:
            marcadores.append(str(m).replace(" ","").split("-"))
        
        f = open('partidos.csv', 'a')
        if etapa!="Fase Final" or mundial=="1950":
            for i in range(int(len(marcadores))):
                f.write(str(equipos[i][0])+","+str(equipos[i][1])+","+str(marcadores[i][0])+","+str(marcadores[i][1])+","+fechas_partidos[i]
                +","+etapa+","+mundial+"\n")
        elif mundial=="1934":
            for i in range(int(len(marcadores))):
                if i<8:
                    etapa="Octavos"
                elif i<13:
                    etapa="Cuartos"
                elif i<15:
                    etapa="Semis"
                elif i<16:
                    etapa="3er Puesto"
                else:
                    etapa="Final"
                f.write(str(equipos[i][0])+","+str(equipos[i][1])+","+str(marcadores[i][0])+","+str(marcadores[i][1])+","+fechas_partidos[i]
                +","+etapa+","+mundial+"\n")
        elif mundial=="1938":
            for i in range(int(len(marcadores))):
                if i<9:
                    etapa="Octavos"
                elif i<14:
                    etapa="Cuartos"
                elif i<16:
                    etapa="Semis"
                elif i<17:
                    etapa="3er Puesto"
                else:
                    etapa="Final"
                f.write(str(equipos[i][0])+","+str(equipos[i][1])+","+str(marcadores[i][0])+","+str(marcadores[i][1])+","+fechas_partidos[i]
                +","+etapa+","+mundial+"\n")
        elif mundial=="1930":
            for i in range(int(len(marcadores))):
                if i<2:
                    etapa="Semis"
                else:
                    etapa="Final"
                f.write(str(equipos[i][0])+","+str(equipos[i][1])+","+str(marcadores[i][0])+","+str(marcadores[i][1])+","+fechas_partidos[i]
                +","+etapa+","+mundial+"\n")
        elif mundial in solocuartos:
            for i in range(int(len(marcadores))):
                if i<4:
                    etapa="Cuartos"
                elif i<6:
                    etapa="Semis"
                elif i<7:
                    etapa="3er Puesto"
                else:
                    etapa="Final"
                f.write(str(equipos[i][0])+","+str(equipos[i][1])+","+str(marcadores[i][0])+","+str(marcadores[i][1])+","+fechas_partidos[i]
                +","+etapa+","+mundial+"\n")
        elif mundial in tercerfinal:
            for i in range(int(len(marcadores))):
                if i<1:
                    etapa="3er Puesto"
                else:
                    etapa="Final"
                f.write(str(equipos[i][0])+","+str(equipos[i][1])+","+str(marcadores[i][0])+","+str(marcadores[i][1])+","+fechas_partidos[i]
                +","+etapa+","+mundial+"\n")
        elif mundial=="1982":
            for i in range(int(len(marcadores))):
                if i<2:
                    etapa="Semis"
                elif i<3:
                    etapa="3er Puesto"
                else:
                    etapa="Final"
                f.write(str(equipos[i][0])+","+str(equipos[i][1])+","+str(marcadores[i][0])+","+str(marcadores[i][1])+","+fechas_partidos[i]
                +","+etapa+","+mundial+"\n")
        else:
            for i in range(int(len(marcadores))):
                if i<8:
                    etapa="Octavos"
                elif i<12:
                    etapa="Cuartos"
                elif i<14:
                    etapa="Semis"
                elif i<15:
                    etapa="3er Puesto"
                else:
                    etapa="Final"
                f.write(str(equipos[i][0])+","+str(equipos[i][1])+","+str(marcadores[i][0])+","+str(marcadores[i][1])+","+fechas_partidos[i]
                +","+etapa+","+mundial+"\n")

