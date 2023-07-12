package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"net/http"
	"os"

	"github.com/streadway/amqp"
)

func failOnError(err error, msg string) {
	if err != nil {
		log.Fatalf("%s: %s", msg, err)
	}
}

func HomeHandler(w http.ResponseWriter, r *http.Request) {
	fmt.Println("publish Funciona")
	w.WriteHeader(http.StatusOK)
	fmt.Fprintf(w, "publish Kafka funcionando")
}

func RabbitHandler(w http.ResponseWriter, r *http.Request) {
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

	body, err := ioutil.ReadAll(r.Body)
	if err != nil {
		log.Fatalln(err)
	}

	err = ch.Publish(
		"",     // exchange
		q.Name, // routing key
		false,  // mandatory
		false,  // immediate
		amqp.Publishing{
			ContentType: "text/plain",
			Body:        body,
		})
	failOnError(err, "Falló en la publicacion del mensaje")
}

func main() {
	// Add handle func for producer.
	http.HandleFunc("/", HomeHandler)
	http.HandleFunc("/datos", RabbitHandler)

	// Run the web server.
	port := os.Getenv("PORT")
	fmt.Println(port)
	fmt.Printf("Productor iniciado en el puerto %s\n", port)
	log.Fatal(http.ListenAndServe(fmt.Sprintf(":%s", port), nil))
}
