package main

import (
	"context"
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"os"
	"strconv"

	"github.com/gorilla/mux"
	"github.com/javiramos1/grpcapi"
	"google.golang.org/grpc"
)

type Informacion struct {
	Request_Number int    `json:"request_number"`
	Game           int    `json:"game"`
	Gamename       string `json:"gamename"`
	Winner         string `json:"winner"`
	Players        int    `json:"players"`
	Ruta           int    `json:"ruta"`
}

func EnviarDatos(w http.ResponseWriter, r *http.Request) {
	enableCors(&w) // habilitamos cors
	var datos Informacion
	parametros := mux.Vars(r)
	game := parametros["gameV"]
	gamename := parametros["gamenameV"]
	players := parametros["playersV"]
	contador := parametros["contadorV"]
	datos.Ruta = 1
	// casteo de string a int
	gameI, err := strconv.Atoi(game)
	if err != nil {
		log.Println(err)
	}
	playersI, err1 := strconv.Atoi(players)
	if err1 != nil {
		log.Println(err1)
	}

	contadorI, err2 := strconv.Atoi(contador)
	if err2 != nil {
		log.Println(err2)
	}
	datos.Request_Number = contadorI

	datos.Game = gameI
	datos.Gamename = gamename
	datos.Players = playersI
	// aqui creo la coneccion con grpc
	datoJson, err := json.Marshal(datos)
	if err != nil {
		w.WriteHeader(http.StatusNotFound)
		fmt.Fprintf(w, "error no se puede decodifocar dato")
		return
	}
	respuesta := client(datoJson)
	if respuesta != nil {
		fmt.Fprintf(w, "%v", respuesta)
	}
	if respuesta == nil {
		fmt.Fprintf(w, "funciona")
	}

}

func client(data []byte) (respuesta error) {
	hostname := os.Getenv("SVC_HOST_NAME")

	if len(hostname) <= 0 {
		hostname = "serverqueuerabbit"
	}

	port := os.Getenv("SVC_PORT")

	if len(port) <= 0 {
		port = "9080"
	}

	cc, err := grpc.Dial(hostname+":"+port, grpc.WithInsecure())
	if err != nil {
		respuesta = err
		return
		//log.Fatalf("could not connect: %v", err)
	}
	defer cc.Close()

	c := grpcapi.NewGrpcServiceClient(cc)
	//fmt.Printf("Created client: %f", c)

	respuesta = callService(c, data)
	return

}
func callService(c grpcapi.GrpcServiceClient, data []byte) (respuesta error) {
	//	fmt.Println("callService...")
	req := &grpcapi.GrpcRequest{
		Input: string(data),
	}
	res, err := c.GrpcService(context.Background(), req)
	if err != nil {
		respuesta = err
		return
		//log.Fatalf("error while calling gRPC: %v", err)

	}
	err = nil
	//muestra en consola los datos
	log.Printf("Response from Service: %v", res.Response)
	return
}
func LimpiarRuta(w http.ResponseWriter, r *http.Request) {
	enableCors(&w)
	// aqui creo la coneccion con grpc
	var datos Informacion
	datos.Ruta = 2
	datoJson, err := json.Marshal(datos)
	if err != nil {
		w.WriteHeader(http.StatusNotFound)
		fmt.Fprintf(w, "error no se puede decodifocar dato")
		return
	}
	respuesta := client(datoJson)
	if respuesta != nil {
		fmt.Fprintf(w, "%v", respuesta)
	}
	if respuesta == nil {
		fmt.Fprintf(w, "ruta limpia")
	}

}
func HomeRoute(w http.ResponseWriter, r *http.Request) {

	enableCors(&w)
	fmt.Println("grpc client Funciona RABBIT")
	w.WriteHeader(http.StatusOK)
	fmt.Fprintf(w, "grpc client RABBIT")
}
func main() {
	log.Println("server corriendo en el puerto 4444")
	router := mux.NewRouter()
	router.HandleFunc("/", HomeRoute).Methods("GET")
	router.HandleFunc("/limpiar", LimpiarRuta).Methods("GET")
	router.HandleFunc("/datos/{game}/{gameV}/{gamename}/{gamenameV}/{players}/{playersV}/{contadorV}", EnviarDatos).Methods("GET")
	log.Fatal(http.ListenAndServe(":4444", router))
}

// esta funcion sirve para poder mandar peticiones a angular
func enableCors(w *http.ResponseWriter) {
	(*w).Header().Set("Access-Control-Allow-Origin", "*")
}
