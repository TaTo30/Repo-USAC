package main

import (
	"bytes"
	"context"
	"fmt"
	"io/ioutil"
	"math/rand"
	"net/http"
	"os/signal"
	"strconv"

	"log"
	"net"
	"os"

	"github.com/javiramos1/grpcapi"
	"google.golang.org/grpc"
	"google.golang.org/grpc/reflection"

	"encoding/json"
)

type grpcServer struct{}

type Informacion struct {
	Request_Number int    `json:"request_number"`
	Game           int    `json:"game"`
	Gamename       string `json:"gamename"`
	Winner         string `json:"winner"`
	Players        int    `json:"players"`
	Ruta           int    `json:"ruta"`
}

func (*grpcServer) GrpcService(ctx context.Context, req *grpcapi.GrpcRequest) (*grpcapi.GrpcResponse, error) {
	var url = "http://skp"
	fmt.Printf("grpcServer %v\n", req)
	name, _ := os.Hostname()

	input := req.GetInput()
	result := "Got input " + input + " server host: " + name
	strjson := input
	var inf Informacion
	json.Unmarshal([]byte(strjson), &inf)
	if inf.Ruta == 2 {
		LimpiarRuta(url)
		res := &grpcapi.GrpcResponse{
			Response: result,
		}
		return res, nil
	}

	// aqui se mandan a llamar los 3 juegos
	switch inf.Game {
	case 1:
		juego1(&inf) //el juego del multiplo de 3

	case 2:
		juego2(&inf) // el juego de UDC random

	case 3:
		juego3(&inf) //el juego del multiplo de 7

	}
	// en esta parte se enviara a la cola
	enviarDatos(url, inf)
	log.Println(inf)
	res := &grpcapi.GrpcResponse{
		Response: result,
	}
	return res, nil
}
func enviarDatos(url string, inf Informacion) {

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
}

func LimpiarRuta(url string) {
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
}
func juego1(data *Informacion) {
	if data.Players < 3 {
		data.Winner = "00" + strconv.Itoa(rand.Intn(3)+1)
		return
	}

	// el juego uno sera de elegir un jugador que sea multiplo de 3
	cantidadNumeroTres := data.Players / 3
	ganador := (rand.Intn(cantidadNumeroTres) + 1) * 3
	// convertirlo en formato 000

	if ganador < 10 {
		data.Winner = "00" + strconv.Itoa(ganador)
	} else if ganador < 100 {
		data.Winner = "0" + strconv.Itoa(ganador)
	} else {
		data.Winner = strconv.Itoa(ganador)
	}
}
func juego2(data *Informacion) {
	centena := data.Players / 100
	decena := (data.Players - centena*100) / 10
	unidad := (data.Players - centena*100 - decena*10)

	if centena == 0 {
		data.Winner = "0"
	} else {
		data.Winner = strconv.Itoa(rand.Intn(centena) + 1)
	}
	if decena == 0 {
		data.Winner = data.Winner + "0"
	} else {
		data.Winner = data.Winner + strconv.Itoa(rand.Intn(decena)+1)
	}
	if unidad == 0 {
		data.Winner = data.Winner + "0"
	} else {
		data.Winner = data.Winner + strconv.Itoa(rand.Intn(unidad)+1)
	}
}
func juego3(data *Informacion) {
	if data.Players < 7 {
		data.Winner = "00" + strconv.Itoa(rand.Intn(7)+1)
		return
	}

	// el juego uno sera de elegir un jugador que sea multiplo de 7
	cantidadNumeroTres := data.Players / 7
	ganador := (rand.Intn(cantidadNumeroTres) + 1) * 7
	// convertirlo en formato 000

	if ganador < 10 {
		data.Winner = "00" + strconv.Itoa(ganador)
	} else if ganador < 100 {
		data.Winner = "0" + strconv.Itoa(ganador)
	} else {
		data.Winner = strconv.Itoa(ganador)
	}
}
func main() {
	fmt.Println("Starting Server...")

	log.SetFlags(log.LstdFlags | log.Lshortfile)

	hostname := os.Getenv("SVC_HOST_NAME")

	if len(hostname) <= 0 {
		hostname = "serverqueuekafka"
		// "35.202.225.144"
	}

	port := os.Getenv("SVC_PORT")

	if len(port) <= 0 {
		port = "9080"
	}

	lis, err := net.Listen("tcp", ":9080")
	if err != nil {
		log.Fatalf("Failed to listen: %v", err)
	}

	opts := []grpc.ServerOption{}
	s := grpc.NewServer(opts...)
	grpcapi.RegisterGrpcServiceServer(s, &grpcServer{})

	// reflection service on gRPC server.
	reflection.Register(s)

	go func() {
		fmt.Println("Server running on ", (hostname + ":" + port))
		if err := s.Serve(lis); err != nil {
			log.Fatalf("failed to serve: %v", err)
		}
	}()

	// Wait for Control C to exit
	ch := make(chan os.Signal, 1)
	signal.Notify(ch, os.Interrupt)

	// Block until a signal is received
	<-ch
	fmt.Println("Stopping the server")
	s.Stop()
	fmt.Println("Closing the listener")
	lis.Close()
	fmt.Println("Server Shutdown")

}
