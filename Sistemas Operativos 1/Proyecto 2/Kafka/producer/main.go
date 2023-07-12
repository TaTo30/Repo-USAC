package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"net/http"
	"os"

	kafka "github.com/segmentio/kafka-go"
)

func HomeHandler(w http.ResponseWriter, r *http.Request) {
	fmt.Println("publish Funciona")
	w.WriteHeader(http.StatusOK)
	fmt.Fprintf(w, "publish Kafka funcionando")
}

func producerHandler(kafkaWriter *kafka.Writer) func(http.ResponseWriter, *http.Request) {
	return http.HandlerFunc(func(wrt http.ResponseWriter, req *http.Request) {
		body, err := ioutil.ReadAll(req.Body)
		if err != nil {
			log.Fatalln(err)
		}
		msg := kafka.Message{
			Key:   []byte(fmt.Sprintf("address-%s", req.RemoteAddr)),
			Value: body,
		}
		err = kafkaWriter.WriteMessages(req.Context(), msg)

		if err != nil {
			wrt.Write([]byte(err.Error()))
			log.Fatalln(err)
		}
	})
}

func getKafkaWriter(kafkaURL, topic string) *kafka.Writer {
	return &kafka.Writer{
		Addr:     kafka.TCP(kafkaURL),
		Topic:    topic,
		Balancer: &kafka.LeastBytes{},
	}
}

func main() {
	// get kafka writer using environment variables.
	kafkaURL := os.Getenv("KAFKA_URL")
	fmt.Println(kafkaURL)
	topic := "kafka1"
	kafkaWriter := getKafkaWriter(kafkaURL, topic)

	defer kafkaWriter.Close()

	// Add handle func for producer.
	http.HandleFunc("/", HomeHandler)
	http.HandleFunc("/datos", producerHandler(kafkaWriter))

	// Run the web server.
	port := os.Getenv("PORT")
	fmt.Println(port)
	fmt.Printf("Productor iniciado en el puerto %s\n", port)
	log.Fatal(http.ListenAndServe(fmt.Sprintf(":%s", port), nil))
}
