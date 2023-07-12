package main

import (
	"fmt"
	"log"
	"net/http"

	"github.com/gorilla/mux"
)

func EnviarDatos(w http.ResponseWriter, r *http.Request) {

	enableCors(&w)
	fmt.Println("faultyTrafic")
	w.WriteHeader(http.StatusAccepted)
	fmt.Fprintf(w, "faulty Trafic")
}
func HomeRoute(w http.ResponseWriter, r *http.Request) {

	enableCors(&w)
	fmt.Println("faultyTrafic")
	w.WriteHeader(http.StatusOK)
	fmt.Fprintf(w, "faulty Trafic")
}
func main() {
	log.Println("server corriendo en el puerto 4444")
	router := mux.NewRouter()
	router.HandleFunc("/", HomeRoute).Methods("GET")
	router.HandleFunc("/datos/{game}/{gameV}/{gamename}/{gamenameV}/{players}/{playersV}/{contadorV}", EnviarDatos).Methods("GET")
	log.Fatal(http.ListenAndServe(":4444", router))
}

// esta funcion sirve para poder mandar peticiones a angular
func enableCors(w *http.ResponseWriter) {
	(*w).Header().Set("Access-Control-Allow-Origin", "*")
}
