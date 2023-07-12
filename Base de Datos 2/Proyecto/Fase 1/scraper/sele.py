from gc import callbacks
import time
import scrapy


class QuotesSpider(scrapy.Spider):
    name = "quotes"
    paises=[]

    def __init__(self, name=None, **kwargs):
        super().__init__(name, **kwargs)
        self.paises = []

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
                     

    def cleanFiles(self):
        with open('jugadores.csv', 'w') as f:
            f.write("jugador,pais,mundial\n")


    def parse_mundial(self,response):

        # LLENAMOS EL ARCHIVO PARA POSICIONES
        tabla = response.css('p.a-right.pad-r10.ita.margen-t3 a::attr(href)').get()
        yield response.follow(tabla, callback=self.parse_posiciones)

        
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
        # JUGADORES POR PAIS  TARDA MUCHO Y LO COMENTE
        linkPaises = response.css('tr.a-top td.a-left a::attr(href)').getall()
        for pais in linkPaises:
            yield response.follow(pais, callback=self.parse_jugadores)
            
            

    #JUGADORES POR PAIS
    def parse_jugadores(self,response):
        time.sleep(0.13)
        preData = response.css('div.pad-y5.clearfix.bb-2 div.left-sm.w-40-sm div.overflow-x-auto a::text').getall()
        pais=response.css('div.rd-100-33.a-center.margen-t3 a::text').getall()
        pais=str(pais[-1]).replace("\t","").replace("\r","").replace("\n","").strip()
        mundial = str(response.css('h1.tc-1::text').get()).replace("Jugadores de "+pais+" en el Mundial ","").replace(".","")
        for jugador in preData:
            with open('jugadores.csv', 'a') as f:
                f.write(str(jugador)+","+pais+","+mundial+"\n")
            


    