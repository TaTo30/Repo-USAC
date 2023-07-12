package main

import (
	"context"
	"encoding/json"
	"fmt"
	"io/ioutil"
	"log"
	"net/http"
	"strings"
	"time"

	"github.com/go-redis/redis"
	"github.com/gorilla/mux"

	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
)

type Informacion struct {
	Request_Number int    `json:"request_number"`
	Game           int    `json:"game"`
	Gamename       string `json:"gamename"`
	Winner         string `json:"winner"`
	Players        int    `json:"players"`
	Worker         string `json:"worker"`
}
type GameLog struct {
	RequestNumber int    `json:"request_number"`
	Game          int    `json:game`
	GameName      string `json:gamename`
	Winner        string `json:winner`
	Players       int    `json:players`
}

func HomeRoute(w http.ResponseWriter, r *http.Request) {
	enableCors(&w)
	fmt.Println("PubSub worker Funciona")
	w.WriteHeader(http.StatusOK)
	fmt.Fprintf(w, "PubSub worker Funciona")

}
func insertMongo(data Informacion) {
	mongo_url := "35.185.104.25"
	mongo_port := "27017"
	mongo_database := "proyecto2"
	mongo_collection := "logs"

	uri := fmt.Sprintf("mongodb://%s:%s/%s", mongo_url, mongo_port, mongo_database)

	client, err := mongo.NewClient(options.Client().ApplyURI(uri))

	if err != nil {
		log.Fatal(err)
	}

	ctx, _ := context.WithTimeout(context.Background(), 10*time.Second)
	err = client.Connect(ctx)

	if err != nil {
		log.Fatal(err)
	}
	defer client.Disconnect(ctx)

	collection := client.Database(mongo_database).Collection(mongo_collection)

	doc := []interface{}{
		bson.M{
			"request_number": data.Request_Number,
			"game":           data.Game,
			"gamename":       data.Gamename,
			"winner":         data.Winner,
			"players":        data.Players,
			"worker":         "PubSub",
		},
	}

	res, insertErr := collection.InsertMany(ctx, doc)
	if insertErr != nil {
		log.Fatal(insertErr)
	}
	log.Println("Se inserto en mongo")
	fmt.Println(res)
}

func DBredis(data string) int {
	var addr = "35.232.18.26:6379"
	var password = ""
	c := redis.NewClient(&redis.Options{
		Addr:     addr,
		Password: password,
	})
	p, err := c.Ping().Result()
	if err != nil {
		log.Println(err)
		fmt.Println("redis kill")
		c.Close()
		return -1

	}
	fmt.Println(p)
	c.RPush("datos", data)
	//rs := c.LRange("datos", 0, 1000).Val()
	//fmt.Println(rs)
	c.Close()
	return 1
}
func EnviarDatos(w http.ResponseWriter, r *http.Request) {
	enableCors(&w)
	var dato Informacion

	reqBody, err := ioutil.ReadAll(r.Body)
	if err != nil {
		w.WriteHeader(http.StatusNotFound)
		fmt.Fprintf(w, "error de informacion")
		return
	}
	json.Unmarshal(reqBody, &dato)
	dato.Worker = "PubSub"
	datoJson, err := json.Marshal(dato)
	if err != nil {
		w.WriteHeader(http.StatusNotFound)
		fmt.Fprintf(w, "error no se puedo decodifocar dato")
		return
	}

	var respuestaRedis = DBredis(string(datoJson))
	if respuestaRedis == -1 {
		w.WriteHeader(http.StatusNotFound)
		fmt.Fprintf(w, "error Redis no conectado")
		return
	}
	// aqui estara mongo db
	log.Println("dato Cargado Redis")

	//var data GameLog

	//json.Unmarshal(reqBody, &data)
	log.Println(string(datoJson))
	insertMongo(dato)

	w.WriteHeader(http.StatusOK)
	fmt.Fprintf(w, string(datoJson))
	return

}
func LimpiarLista(w http.ResponseWriter, r *http.Request) {
	enableCors(&w)
	var addr = "35.232.18.26:6379"
	var password = ""

	c := redis.NewClient(&redis.Options{
		Addr:     addr,
		Password: password,
	})
	c.Del("datos")
	c.Close()
	w.WriteHeader(http.StatusOK)
	fmt.Println("Base De Datos Limpia")
	fmt.Fprintf(w, "Base De Datos Limpia")

}
func VerDatos(w http.ResponseWriter, r *http.Request) {
	enableCors(&w)
	var addr = "35.232.18.26:6379"
	var password = ""

	c := redis.NewClient(&redis.Options{
		Addr:     addr,
		Password: password,
	})
	p, err := c.Ping().Result()
	if err != nil {
		log.Println(err)
		fmt.Println("redis kill")
		c.Close()
		return
	}
	fmt.Println(p)
	rs := c.LRange("datos", 0, 1000).Val()
	fmt.Println(rs)
	w.WriteHeader(http.StatusOK)
	fmt.Fprintf(w, strings.Join(rs, " "))

}
func main() {
	router := mux.NewRouter()
	log.Println("Server encendido")
	router.HandleFunc("/", HomeRoute)
	router.HandleFunc("/datos", EnviarDatos).Methods("POST")
	router.HandleFunc("/limpiar", LimpiarLista).Methods("GET")
	router.HandleFunc("/verDatos", VerDatos).Methods("GET")
	log.Fatal(http.ListenAndServe(":4444", router))
}

// esta funcion sirve para poder mandar peticiones a angular ya que habilita los cors
func enableCors(w *http.ResponseWriter) {
	(*w).Header().Set("Access-Control-Allow-Origin", "*")
}
