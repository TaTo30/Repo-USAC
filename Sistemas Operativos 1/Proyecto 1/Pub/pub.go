package main

import (
	"bytes"
	"context"
	"encoding/json"
	"fmt"
	"net/http"
	"strconv"

	"strings"
	"time"

	"cloud.google.com/go/pubsub"
)

func publish(msg string) error {
	projectId := "proyecto1sopes-326001"
	topicId := "sopes1"

	ctx := context.Background()

	client, err := pubsub.NewClient(ctx, projectId)
	if err != nil {
		fmt.Print("Error :( ")
		fmt.Print(err)
		return fmt.Errorf("Error al conectarse %%v", err)
	}

	t := client.Topic(topicId)
	result := t.Publish(ctx, &pubsub.Message{Data: []byte(msg)})

	id, err := result.Get(ctx)
	if err != nil {
		fmt.Print("error")
		fmt.Print(err)
		return fmt.Errorf("Error: %v", err)
	}

	fmt.Print("Publicado: ", id)
	return nil
}

type Data struct {
	Api string `json:"api"`
}
type Publicar struct {
	Guardados     int    `json:"guardados"`
	Api           string `json:"api"`
	TiempoDeCarga int    `json:"tiempoDeCarga"`
	Db            string `json:"db"`
}
type contador struct {
	Cantidad int    `json:"cantidad"`
	Api      string `json:"api"`
}

type Tiempo struct {
	Time string `json:"time"`
}

var arreglo [6]Publicar

var dt time.Time
var contadores [3]contador

func limpiar() int {
	contadores[0].Cantidad = 0
	contadores[1].Cantidad = 0
	contadores[2].Cantidad = 0
	contadores[0].Api = "Python"
	contadores[1].Api = "Go"
	contadores[2].Api = "Rust"
	arreglo[0] = Publicar{Guardados: 0, Api: "Python", TiempoDeCarga: 0, Db: "CloudSQL"}
	arreglo[1] = Publicar{Guardados: 0, Api: "Python", TiempoDeCarga: 0, Db: "Cosmo"}
	arreglo[2] = Publicar{Guardados: 0, Api: "Go", TiempoDeCarga: 0, Db: "CloudSQL"}
	arreglo[3] = Publicar{Guardados: 0, Api: "Go", TiempoDeCarga: 0, Db: "Cosmo"}
	arreglo[4] = Publicar{Guardados: 0, Api: "Rust", TiempoDeCarga: 0, Db: "CloudSQL"}
	arreglo[5] = Publicar{Guardados: 0, Api: "Rust", TiempoDeCarga: 0, Db: "Cosmo"}

	return 1
}
func fecha() int {
	dt2 := time.Now()
	tiempo := (dt2.Hour()-dt.Hour())*3600 + (dt2.Minute()-dt.Minute())*60 + (dt2.Second() - dt.Second())
	return tiempo

}

func main() {

	fmt.Println("Iniciando envio...")
	limpiar()
	http.HandleFunc("/iniciarCarga", func(w http.ResponseWriter, r *http.Request) {
		limpiar()
		fmt.Fprintf(w, "iniciarCarga")
	})

	http.HandleFunc("/notificar", func(w http.ResponseWriter, r *http.Request) {
		json.NewEncoder(w).Encode(arreglo)
	})

	http.HandleFunc("/publicar", func(w http.ResponseWriter, r *http.Request) {

		if contadores[0].Cantidad == 0 && contadores[1].Cantidad == 0 && contadores[2].Cantidad == 0 {
			dt = time.Now()
		}
		var d Data
		err := json.NewDecoder(r.Body).Decode(&d)
		if err != nil {
			fmt.Print(err)
		} else {

			if strings.ToLower(d.Api) == "python" {
				contadores[0].Cantidad = contadores[0].Cantidad + 1
			} else if strings.ToLower(d.Api) == "go" {
				contadores[1].Cantidad = contadores[1].Cantidad + 1
			} else if strings.ToLower(d.Api) == "rust" {
				contadores[2].Cantidad = contadores[2].Cantidad + 1
			}

		}
	})

	http.HandleFunc("/finalizarCarga", func(w http.ResponseWriter, r *http.Request) {
		_fecha := fecha()
		var _python Publicar
		_python.Guardados = contadores[0].Cantidad
		_python.Api = contadores[0].Api
		_python.Db = "Cosmo"
		_python.TiempoDeCarga = _fecha

		_fechago := fecha()
		var _go Publicar
		_go.Guardados = contadores[1].Cantidad
		_go.Api = contadores[1].Api
		_go.Db = "Cosmo"
		_go.TiempoDeCarga = _fechago

		_fecharust := fecha()
		var _rust Publicar
		_rust.Guardados = contadores[2].Cantidad
		_rust.Api = contadores[2].Api
		_rust.Db = "Cosmo"
		_rust.TiempoDeCarga = _fecharust
		//se cargan datos con mysql
		var _python5 Publicar
		_python5.Guardados = contadores[0].Cantidad
		_python5.Api = contadores[0].Api
		_python5.Db = "CloudSQL"
		_python5.TiempoDeCarga = _fecha

		var _go5 Publicar
		_go5.Guardados = contadores[1].Cantidad
		_go5.Api = contadores[1].Api
		_go5.Db = "CloudSQL"
		_go5.TiempoDeCarga = _fechago

		var _rust5 Publicar
		_rust5.Guardados = contadores[2].Cantidad
		_rust5.Api = contadores[2].Api
		_rust5.Db = "CloudSQL"
		_rust5.TiempoDeCarga = _fecharust

		_python2, err2 := json.Marshal(_python)
		if err2 != nil {
			fmt.Print(err2)
		} else {
			publish(string(_python2))
		}
		_go2, err2 := json.Marshal(_go)
		if err2 != nil {
			fmt.Print(err2)
		} else {
			publish(string(_go2))
		}
		_rust2, err2 := json.Marshal(_rust)
		if err2 != nil {
			fmt.Print(err2)
		} else {
			publish(string(_rust2))
		}
		//enviar datos a la base de google

		_python3, err2 := json.Marshal(_python5)
		if err2 != nil {
			fmt.Print(err2)
		} else {
			publish(string(_python3))
		}
		_go3, err2 := json.Marshal(_go5)
		if err2 != nil {
			fmt.Print(err2)
		} else {
			publish(string(_go3))
		}
		_rust3, err2 := json.Marshal(_rust5)
		if err2 != nil {
			fmt.Print(err2)
		} else {
			publish(string(_rust3))
		}

		strstring := `# HELP go_fecha_tiempo fecha api py.

# TYPE go_fecha_tiempo gauge

go_fecha_tiempo ` + strconv.Itoa(_fecha) + `

# HELP go_fechago_tiempo fecha api go.

# TYPE go_fechago_tiempo gauge

go_fechago_tiempo ` + strconv.Itoa(_fechago) + `

# HELP go_fecharust_tiempo fecha api rust.

# TYPE go_fecharust_tiempo gauge

go_fecharust_tiempo ` + strconv.Itoa(_fecharust)

		datos := Tiempo{
			Time: fmt.Sprintf("%v", strstring),
		}

		b, err := json.MarshalIndent(datos, "", "  ")
		if err != nil {
			fmt.Println(err)
		}
		// este me sirve para grafana
		position, err12 := http.Post("http://34.133.229.81:8070/metrics", "application/json", bytes.NewBuffer(b))
		if err12 != nil {
			fmt.Print(err12)
		} else {
			fmt.Print(position)
		}

		print(string(b))

		arreglo = [6]Publicar{_python, _rust, _go, _python5, _rust5, _go5}

		p, err5 := json.Marshal(arreglo)
		if err5 != nil {
			fmt.Print(err5)
		} else {

			pos, err10 := http.Post("https://halogen-segment-328016.uc.r.appspot.com/setNotifications", "application/json", bytes.NewBuffer(p))
			if err10 != nil {
				fmt.Print(err10)
			} else {
				fmt.Println("")
				fmt.Println("")
				fmt.Print(pos)
			}

		}

		fmt.Fprintf(w, "finalizarCarga")
	})
	http.ListenAndServe(":4444", nil)
}
