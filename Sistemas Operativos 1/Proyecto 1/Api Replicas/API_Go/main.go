package main

import (
	"bytes"
	"context"
	"database/sql"
	"encoding/json"
	"fmt"
	"io/ioutil"
	"log"
	"net/http"
	"strconv"
	"strings"
	"time"

	_ "github.com/go-sql-driver/mysql"
	"github.com/gorilla/mux"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
)

type twit struct {
	Nombre     string   `json:nombre`
	Comentario string   `json:comentario`
	Fecha      string   `json:fecha`
	Hashtags   []string `json:hashtags`
	Upvotes    int      `json:upvotes`
	Downvotes  int      `json:downvotes`
}

type pubsub struct {
	Api string `json:api`
}

func conexionDB() (conexion *sql.DB) {
	Driver := "mysql"
	Usuario := "root"
	Contrasenia := "1234"
	Nombre := "Proyecto1"

	conexion, err := sql.Open(Driver, Usuario+":"+Contrasenia+"@tcp(35.184.7.29)/"+Nombre)

	if err != nil {
		panic(err.Error())
	}

	return conexion
}

func indexRoute(w http.ResponseWriter, r *http.Request) {
	fmt.Fprintf(w, "Welcome")
}

func insertCosmos(nombre string, comentario string, fecha string, hashtags []string, upvotes int, downvotes int) {
	//Cosmos DB
	cosmos_user_name := "cosmos-calificacion"
	cosmos_password := "3IlHj1WMnBFTvlc0MeAG9rWLMwNu14bkevGnwIS4VcxEmKHA3o3czPMhcXbnXGCOYyNLAyjVyGczZ1esgc0Vmg=="
	cosmos_url := "cosmos-calificacion.mongo.cosmos.azure.com"
	cosmos_database_name := "sopes1"
	cosmos_collection_name := "proyecto1"
	cosmos_port := 10255
	cosmos_args := "ssl=true&retrywrites=false&ssl_cert_reqs=CERT_NONE"

	uri := fmt.Sprintf("mongodb://%s:%s@%s:%s/%s?%s",
		cosmos_user_name, cosmos_password, cosmos_url, strconv.Itoa(cosmos_port), cosmos_database_name, cosmos_args)

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

	collection := client.Database(cosmos_database_name).Collection(cosmos_collection_name)

	newArrayHash := bson.A{}

	for _, e := range hashtags {
		newArrayHash = append(newArrayHash, e)
	}

	docs := []interface{}{
		bson.M{"nombre": nombre,
			"comentario": comentario,
			"fecha":      fecha,
			"hashtags":   newArrayHash,
			"upvotes":    upvotes,
			"downvotes":  downvotes},
	}

	res, insertErr := collection.InsertMany(ctx, docs)
	if insertErr != nil {
		log.Fatal(insertErr)
	}
	fmt.Println(res)

	/*cur, currErr := collection.Find(ctx, bson.M{})

	if currErr != nil {
		panic(currErr)
	}
	defer cur.Close(ctx)

	var posts []twit
	if err = cur.All(ctx, &posts); err != nil {
		panic(err)
	}
	fmt.Println(posts)*/
}

func iniciarCarga(w http.ResponseWriter, r *http.Request) {
	res, err := http.Get("http://35.184.136.235:4444/iniciarCarga")
	if err != nil {
		log.Fatalln(err)
	} else {
		body, err := ioutil.ReadAll(res.Body)
		if err != nil {
			log.Fatalln(err)
		}
		fmt.Fprintf(w, string(body))
	}
}

func finalizarCarga(w http.ResponseWriter, r *http.Request) {
	res, err := http.Get("http://35.184.136.235:4444/finalizarCarga")
	if err != nil {
		log.Fatalln(err)
	} else {
		body, err := ioutil.ReadAll(res.Body)
		if err != nil {
			log.Fatalln(err)
		}
		fmt.Fprintf(w, string(body))

	}
}

func publicar(w http.ResponseWriter, r *http.Request) {
	var newTwit twit
	reqBody, err := ioutil.ReadAll(r.Body)

	if err != nil {
		fmt.Fprintf(w, "Insert a Valid Twit")
	}

	json.Unmarshal(reqBody, &newTwit)

	fechaParts := strings.Split(newTwit.Fecha, "/")
	newFecha := fechaParts[2] + "-" + fechaParts[1] + "-" + fechaParts[0]
	newTwit.Fecha = newFecha

	conexion := conexionDB()
	insertarRegistros, err := conexion.Prepare("INSERT INTO twit(nombre, comentario, fecha, upvotes, downvotes, api) VALUES(?,?,?,?,?,'go')")
	if err != nil {
		panic(err)
	}

	resultado, err := insertarRegistros.Exec(newTwit.Nombre, newTwit.Comentario, newTwit.Fecha, newTwit.Upvotes, newTwit.Downvotes)

	if err != nil {
		fmt.Println(resultado)
		panic(err)
	}

	registro, err := conexion.Query("SELECT MAX(id) FROM twit")

	if err != nil {
		panic(err)
	}

	var idTwit int

	for registro.Next() {
		err = registro.Scan(&idTwit)
		if err != nil {
			panic(err)
		}
	}

	for i := 0; i < len(newTwit.Hashtags); i++ {
		hashtag := newTwit.Hashtags[i]
		insertarHashtag, err := conexion.Prepare("INSERT INTO hashtags (hashtag) VALUES (?)")

		if err != nil {
			registroIdHashtag, err := conexion.Query("SELECT id FROM hashtags WHERE hashtag = ?", hashtag)

			if err != nil {
				panic(err)
			}

			var idHash int
			for registroIdHashtag.Next() {
				err = registroIdHashtag.Scan(&idHash)
				if err != nil {
					panic(err)
				}
			}

			insertarTwitHash, err := conexion.Prepare("INSERT INTO hash_twit (idTwit, idHash) VALUES (?, ?)")
			if err != nil {
				panic(err)
			}
			resultado, err := insertarTwitHash.Exec(idTwit, idHash)
			if err != nil {
				fmt.Println(resultado)
				panic(err)
			}
		}

		insertarHashtag.Exec(hashtag)

		registroIdHashtag, err := conexion.Query("SELECT MAX(id) FROM hashtags")

		if err != nil {
			panic(err)
		}

		var idHash int
		for registroIdHashtag.Next() {
			err = registroIdHashtag.Scan(&idHash)
			if err != nil {
				panic(err)
			}
		}

		insertarTwitHash, err := conexion.Prepare("INSERT INTO hash_twit (idTwit, idHash) VALUES (?, ?)")
		if err != nil {
			panic(err)
		}
		resultado, err := insertarTwitHash.Exec(idTwit, idHash)

		if err != nil {
			fmt.Println(resultado)
			panic(err)
		}
	}

	insertCosmos(newTwit.Nombre, newTwit.Comentario, newTwit.Fecha, newTwit.Hashtags, newTwit.Upvotes, newTwit.Downvotes)

	p, err2 := json.Marshal(pubsub{"go"})
	if err2 != nil {
		fmt.Print(err)
	} else {
		http.Post("http://35.184.136.235:4444/publicar", "application/json", bytes.NewBuffer(p))
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	json.NewEncoder(w).Encode(newTwit)
}

func main() {

	fmt.Println("Hello Go")

	router := mux.NewRouter().StrictSlash(true)
	router.HandleFunc("/", indexRoute)
	router.HandleFunc("/iniciarCarga", iniciarCarga).Methods("GET")
	router.HandleFunc("/finalizarCarga", finalizarCarga).Methods("GET")
	router.HandleFunc("/publicar", publicar).Methods("POST")
	log.Fatal(http.ListenAndServe(":5000", router))
}
