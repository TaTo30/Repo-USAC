package main

import (
	"context"
	"encoding/json"
	"fmt"
	"log"
	"os"
	"time"

	"github.com/streadway/amqp"

	"github.com/go-redis/redis"

	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
)

type GameLog struct {
	RequestNumber int    `json:"request_number"`
	Game          int    `json:game`
	GameName      string `json:gamename`
	Winner        string `json:winner`
	Players       int    `json:players`
}

func failOnError(err error, msg string) {
	if err != nil {
		log.Fatalf("%s: %s", msg, err)
	}
}

func insertRedis(data GameLog) {
	var addr = "35.232.18.26:6379"
	var password = ""
	c := redis.NewClient(&redis.Options{
		Addr:     addr,
		Password: password,
	})
	p, err := c.Ping().Result()
	if err != nil {
		fmt.Println("redis kill")
		c.Close()
		return
	}
	fmt.Println(p)
	var datos_string = fmt.Sprintf(`{"request_number": %d,"game": %d,"gamename": "%s", "winner": "%s", "players": %d, "worker": "RabbitMQ"}`, data.RequestNumber, data.Game, data.GameName, data.Winner, data.Players)
	fmt.Println(datos_string)
	c.RPush("datos", datos_string)
	c.Close()
	fmt.Println("Se inserto la data exitosamente en Redis")
}

func insertMongo(data GameLog) {
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
			"request_number": data.RequestNumber,
			"game":           data.Game,
			"gamename":       data.GameName,
			"winner":         data.Winner,
			"players":        data.Players,
			"worker":         "RabbitMQ",
		},
	}

	res, insertErr := collection.InsertMany(ctx, doc)
	if insertErr != nil {
		log.Fatal(insertErr)
	}
	fmt.Println("Se inserto el log exitosamente en MongoDB")
	fmt.Println(res)
}

func main() {
	address := os.Getenv("RABBIT_IP")
	uri := fmt.Sprintf("amqp://guest:guest@%s:5672/", address)
	fmt.Println(uri)
	conn, err := amqp.Dial(uri)
	failOnError(err, "Falló para conectar a rabbit")
	defer conn.Close()

	ch, err := conn.Channel()
	failOnError(err, "Falló creando el canal")
	defer ch.Close()

	q, err := ch.QueueDeclare(
		"rabbit", // name
		false,    // durable
		false,    // delete when unused
		false,    // exclusive
		false,    // no-wait
		nil,      // arguments
	)
	failOnError(err, "Falló declarando la cola")

	msgs, err := ch.Consume(
		q.Name, // queue
		"",     // consumer
		true,   // auto-ack
		false,  // exclusive
		false,  // no-local
		false,  // no-wait
		nil,    // args
	)
	failOnError(err, "Falló en registrar el consumidro")

	forever := make(chan bool)

	go func() {
		for d := range msgs {
			data := GameLog{}
			json.Unmarshal([]byte(string(d.Body)), &data)
			insertMongo(data)
			insertRedis(data)
			log.Printf("Received a message: %s", d.Body)
		}
	}()

	log.Printf(" [*] Esperando mensajes")
	<-forever
}
