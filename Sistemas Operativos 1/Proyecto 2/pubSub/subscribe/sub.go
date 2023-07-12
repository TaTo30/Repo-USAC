package main

import (
	"bytes"
	"encoding/json"
	"fmt"
	"io/ioutil"
	"log"
	"net/http"

	"github.com/garyburd/redigo/redis"
)

type Informacion struct {
	Request_Number int    `json:"request_number"`
	Game           int    `json:"game"`
	Gamename       string `json:"gamename"`
	Winner         string `json:"winner"`
	Players        int    `json:"players"`
	Worker         string `json:"worker"`
	Ruta           int    `json:"ruta"`
}

func main() {
	c, err := redis.Dial("tcp", "35.232.18.26:6379")
	if err != nil {
		fmt.Println("error de conexión a la base de datos redis", err)

	}
	psc := redis.PubSubConn{Conn: c}
	psc.Subscribe("pubsub2")
	for {
		switch v := psc.Receive().(type) {
		case redis.Message:
			var inf Informacion
			json.Unmarshal([]byte(v.Data), &inf)
			//fmt.Printf("%s: message: %s\n", v.Channel, v.Data)
			//var url = "http://34.135.212.42:80"
			var url = "http://pubsubworker"
			switch inf.Ruta {
			case 1:
				clienteHttp := &http.Client{}
				peticion, err := http.NewRequest("GET", url+"/", nil)
				if err != nil {
					log.Fatalf("Error creando petición: %v", err)
					return
				}
				peticion.Header.Add("Content-Type", "application/json")
				respuesta, err := clienteHttp.Do(peticion)
				if err != nil {
					log.Fatalf("Error haciendo petición: %v", err)
					return
				}
				defer respuesta.Body.Close()
				cuerpoRespuesta, err := ioutil.ReadAll(respuesta.Body)
				if err != nil {
					log.Fatalf("Error leyendo respuesta: %v", err)
				}
				fmt.Println(string(cuerpoRespuesta))
				break
			case 2:
				clienteHttp := &http.Client{}
				datoJson, err := json.Marshal(inf)
				if err != nil {
					log.Fatalf("Error codificando usuario como JSON: %v", err)
				}

				peticion, err := http.NewRequest("POST", url+"/datos", bytes.NewBuffer(datoJson))
				if err != nil {
					log.Fatalf("Error creando petición: %v", err)
				}

				peticion.Header.Add("Content-Type", "application/json")
				respuesta, err := clienteHttp.Do(peticion)
				if err != nil {
					log.Fatalf("Error haciendo petición: %v", err)
				}
				defer respuesta.Body.Close()
				cuerpoRespuesta, err := ioutil.ReadAll(respuesta.Body)
				if err != nil {
					log.Fatalf("Error leyendo respuesta: %v", err)
				}
				fmt.Println(string(cuerpoRespuesta))
				break
			case 3:
				clienteHttp := &http.Client{}
				peticion, err := http.NewRequest("GET", url+"/limpiar", nil)
				if err != nil {
					log.Fatalf("Error creando petición: %v", err)
					return
				}
				peticion.Header.Add("Content-Type", "application/json")
				respuesta, err := clienteHttp.Do(peticion)
				if err != nil {
					log.Fatalf("Error haciendo petición: %v", err)
					return
				}
				defer respuesta.Body.Close()
				cuerpoRespuesta, err := ioutil.ReadAll(respuesta.Body)
				if err != nil {
					log.Fatalf("Error leyendo respuesta: %v", err)
				}
				fmt.Println(string(cuerpoRespuesta))
				break
			}

		case redis.Subscription:
			fmt.Printf("%s: %s %d\n", v.Channel, v.Kind, v.Count)
		case error:
			fmt.Printf("error", v)
		}
	}

}
