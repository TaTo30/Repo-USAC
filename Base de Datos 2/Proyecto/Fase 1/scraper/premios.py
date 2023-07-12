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
        premiosMundiales = response.css('div.left.margen-r5.margen-l5 a::attr(href)').getall()
        
        i=0
        for link in premiosMundiales:
            if str(link).find("premios") != -1:
                i+=1
                yield response.follow(link, callback=self.parse_premios)  
            # if i==2: break
                                       

    def cleanFiles(self):
        with open('premios.csv', 'w') as f:
            f.write("premio,jugador,mundial,pais\n")

    # PREMIOS
    def parse_premios(self,response):
        mundial = str(response.css('h1.tc-1::text').get()).replace("Premios del Mundial de Fútbol de ","")
        preData = response.css('div.rd-100-30 p a::text').getall()
        if mundial=="2022": 
            return
        mun=int(mundial)
        if mun==1994:
            with open('premios.csv', 'a') as f:
                f.write("Balón de Oro,"+str(preData[0])+","+mundial+",\n")
                f.write("Balón de Plata,"+str(preData[1])+","+mundial+",\n")
                f.write("Balón de Bronce,"+str(preData[2])+","+mundial+",\n")
                f.write("Botín de Oro,"+str(preData[3])+","+mundial+",\n")
                f.write("Botín de Oro,"+str(preData[4])+","+mundial+",\n")
        elif mun>=2006 :
            with open('premios.csv', 'a') as f:
                f.write("Balón de Oro,"+str(preData[0])+","+mundial+",\n")
                f.write("Balón de Plata,"+str(preData[1])+","+mundial+",\n")
                f.write("Balón de Bronce,"+str(preData[2])+","+mundial+",\n")
                f.write("Botín de Oro,"+str(preData[3])+","+mundial+",\n")
                f.write("Botín de Plata,"+str(preData[4])+","+mundial+",\n")
                f.write("Botín de Bronce,"+str(preData[5])+","+mundial+",\n")
        elif mun>=1986 and mun<=2002:
            with open('premios.csv', 'a') as f:
                f.write("Balón de Oro,"+str(preData[0])+","+mundial+",\n")
                f.write("Balón de Plata,"+str(preData[1])+","+mundial+",\n")
                f.write("Balón de Bronce,"+str(preData[2])+","+mundial+",\n")
                f.write("Botín de Oro,"+str(preData[3])+","+mundial+",\n")
        else:
            f=open('premios.csv', 'a') 
            for jugador in preData:
                f.write("Botín de Oro,"+str(jugador)+","+mundial+",\n")
            f.close()

        datos=response.css('div.rd-100-45 div p a::text').getall()
        if mun>=2010:
            with open('premios.csv', 'a') as f:
                f.write("Guante de Oro,"+str(datos[0])+","+mundial+",\n")
                f.write("Mejor Jugador Joven,"+str(datos[1])+","+mundial+",\n")
                f.write("FIFA Fair Play,,"+mundial+","+str(datos[2])+"\n")
        if mun==1998 or mun == 2006:
            with open('premios.csv', 'a') as f:
                f.write("Guante de Oro,"+str(datos[0])+","+mundial+",\n")
                f.write("Mejor Jugador Joven,"+str(datos[1])+","+mundial+",\n")
                f.write("FIFA Fair Play,,"+mundial+","+str(datos[2])+"\n")
                f.write("FIFA Fair Play,,"+mundial+","+str(datos[3])+"\n")
                f.write("Equipo Más Entretenido,,"+mundial+","+str(datos[4])+"\n")
        if mun==2002 or mun==1994:
            with open('premios.csv', 'a') as f:
               f.write("Guante de Oro,"+str(datos[0])+","+mundial+",\n")
               f.write("Mejor Jugador Joven,"+str(datos[1]).strip()+","+mundial+",\n")
               f.write("FIFA Fair Play,,"+mundial+","+str(datos[2])+"\n")
               f.write("Equipo Más Entretenido,,"+mundial+","+str(datos[3])+"\n")
        if mun>=1978 and mun<=1990:
            with open('premios.csv', 'a') as f:
                f.write("Mejor Jugador Joven,"+str(datos[0]).strip()+","+mundial+",\n")
                f.write("FIFA Fair Play,,"+mundial+","+str(datos[1])+"\n")
        else:
            f=open('premios.csv', 'a')
            if len(datos)>1:
                f.write("Mejor Jugador Joven,"+str(datos[0]).strip()+","+mundial+",\n")
                f.write("FIFA Fair Play,,"+mundial+","+str(datos[1])+"\n")
            else:
                if len(datos)!=0:
                    f.write("Mejor Jugador Joven,"+str(datos[0]).strip()+","+mundial+",\n")
            f.close()
        
        jugadores=response.css('div.rd-100-25.margen-b5.a-left a::text').getall()
        f=open('premios.csv', 'a')
        for jugador in jugadores:
            f.write("Equipo Ideal,"+str(jugador).strip()+","+mundial+",\n")
        f.close()