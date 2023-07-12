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
                # break      

    def cleanFiles(self):
        with open('detalle.csv', 'w') as f:
            f.write("grupo/etapa,pais,mundial\n")
        


    def parse_mundial(self,response):
        # LLENAMOS EL ARCHIVO PARA POSICIONES
        tabla = response.css('p.margen-l10 a::attr(href)').get()
        yield response.follow(tabla, callback=self.pase_resultados)

    def pase_resultados(self,response):
        tabla = response.css('div.left.a-left.wpx-170 a::text').getall()
        mundial=str(response.css('nav.breadcrumb a::text').getall()[-1])
        paises=response.css("div.left.margen-b2.clearfix div.left::text").getall()
        f = open('detalle.csv', 'a') 
        i=0
        for etapa in tabla:
            i+=2
            etapa=str(etapa).replace(",","")
            pais1=paises[i-2]
            pais2=paises[i-1]
            f.write(etapa+","+pais1+","+mundial+"\n")
            f.write(etapa+","+pais2+","+mundial+"\n")
        f.close()
        