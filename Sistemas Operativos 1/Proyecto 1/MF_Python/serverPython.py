#primero instalar https://www.python.org/downloads/
#segundo instalar py -m venv env     
#tercero instalar py -m pip install requests
import json
import requests
import time
import random
def cargarDatos(ruta):
    contadorEnviados=0
    contadorError=0
    requests.get('http://34.117.248.209/iniciarCarga',"")
    with open(ruta)as contenido:
        datos=json.load(contenido)
        for dato in datos:
            try:
                headers={'Content-Type':'application/json'}
                
                resp = requests.post('http://34.117.248.209/publicar', data = json.dumps(dato),headers=headers)
                sleeptime= random.uniform(0.1, 0.9)
                temporal={"time": str(sleeptime)}
                time.sleep(sleeptime)
                
                if resp.status_code<300:
                    resp1 = requests.post('http://34.133.229.81:8056/metrics', data = temporal)
                    contadorEnviados=contadorEnviados+1
                else:
                    contadorError=contadorError+1
            except:
                contadorError=contadorError+1
    print("cantidad de datos enviados ",contadorEnviados)
    print("cantidad de datos con error ",contadorError)
    requests.get('http://34.117.248.209/finalizarCarga',"")
                
            



 

if __name__=="__main__":
    #esta ruta se cambia dependiendo del archivo de entrada
   ruta="generated.json"
   cargarDatos(ruta)
 