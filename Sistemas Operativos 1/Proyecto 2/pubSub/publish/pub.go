package main

import (
	"encoding/json"
	"fmt"
	"io/ioutil"
	"log"
	"net/http"

	"github.com/garyburd/redigo/redis"
	"github.com/gorilla/mux"
)

func HomeRoute(w http.ResponseWriter, r *http.Request) {
	enableCors(&w)
	fmt.Println("publish Funciona")
	var datos Informacion
	var respuesta = redisInsert(datos, 1)
	if respuesta < 0 {
		w.WriteHeader(http.StatusBadRequest)
		fmt.Fprintf(w, "Error en subscribe o worker")
		return
	}
	w.WriteHeader(http.StatusOK)
	fmt.Fprintf(w, "publish  Funciona")
}
func EnviarDatos(w http.ResponseWriter, r *http.Request) {
	enableCors(&w)
	var datos Informacion
	reqBody, err := ioutil.ReadAll(r.Body)
	if err != nil {
		w.WriteHeader(http.StatusNotFound)
		fmt.Fprintf(w, "error de informacion")
		return
	}
	json.Unmarshal(reqBody, &datos)
	var respuesta = redisInsert(datos, 2)
	if respuesta < 0 {
		w.WriteHeader(http.StatusBadRequest)
		fmt.Fprintf(w, "Error en subscribe o worker")
		return
	}
	fmt.Println("publish  enviarDatos Funciona")
	w.WriteHeader(http.StatusOK)
	fmt.Fprintf(w, "publish enviarDatos Funciona")
}
func LimpiarLista(w http.ResponseWriter, r *http.Request) {
	enableCors(&w)
	var datos Informacion
	var respuesta = redisInsert(datos, 3)
	if respuesta < 0 {
		w.WriteHeader(http.StatusBadRequest)
		fmt.Fprintf(w, "Error en subscribe o worker")
		return
	}
	fmt.Println("publish limpiar Funciona")
	w.WriteHeader(http.StatusOK)
	fmt.Fprintf(w, "publish  limpiar Funciona")
}

func redisInsert(datos Informacion, peticion int) int {
	datos.Ruta = peticion
	conn, err := redis.Dial("tcp", "35.232.18.26:6379")
	if err != nil {
		fmt.Println("error de conexión a la base de datos redis", err)
		return -1
	}
	datoJson, err := json.Marshal(datos)
	if err != nil {
		fmt.Println("error al convertir json")
		return -3
	}
	rep1, err1 := conn.Do("PUBLISH", "pubsub2", datoJson)
	if err1 != nil {
		return -2
	}
	fmt.Println("funciona", rep1)
	return 1

}
func main() {
	router := mux.NewRouter()
	router.HandleFunc("/", HomeRoute).Methods("GET")
	router.HandleFunc("/datos", EnviarDatos).Methods("POST")
	router.HandleFunc("/limpiar", LimpiarLista).Methods("GET")
	log.Fatal(http.ListenAndServe(":3333", router))
}

// esta funcion sirve para poder mandar peticiones a angular
func enableCors(w *http.ResponseWriter) {
	(*w).Header().Set("Access-Control-Allow-Origin", "*")
}

type Informacion struct {
	Request_Number int    `json:"request_number"`
	Game           int    `json:"game"`
	Gamename       string `json:"gamename"`
	Winner         string `json:"winner"`
	Players        int    `json:"players"`
	Worker         string `json:"worker"`
	Ruta           int    `json:"ruta"`
}
