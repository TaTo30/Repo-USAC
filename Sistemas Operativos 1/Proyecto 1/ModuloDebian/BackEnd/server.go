package main

import (
	"encoding/json"
	"fmt"
	"io/ioutil"
	"log"
	"net/http"
	"strconv"
	"time"

	"github.com/gorilla/mux"
	"github.com/shirou/gopsutil/cpu"
	"github.com/shirou/gopsutil/mem"
)

type Ram struct {
	Total      string `json:"total"`
	Porcentaje string `json:"porcentaje"`
	Usado      string `json:"usado"`
	Libre      string `json:"libre"`
}

type Cpu struct {
	Nucleo1 string `json:"nucleo1"`
	Libre1  string `json:"libre1"`
}

type Datos struct {
	Processos  string `json:"procesos"`
	StrProceso string `json:"strprocesos"`
}

//funcion que realizara los datos de la ram
func homeRAM(w http.ResponseWriter, req *http.Request) {
	enableCors(&w) // habilitamos cors
	//var Datoram Ram

	println("******** Cargar Datos RAM******")
	println("sopes")

	// se utilizo mem.virtualmemory()   ya que los valores del archivo memo no mostraron valores exactos
	v, _ := mem.VirtualMemory()
	var Total = v.Total / (1024 * 1024)                  //valor total de ram en mb
	var Porcentaje = v.UsedPercent                       // porcentaje de utiliacion
	var used = uint64(float64(Total) * Porcentaje / 100) // cantidad de memoria utilizada
	var free = Total - used                              // cantidad de memoria libre
	datos := Ram{
		Total:      fmt.Sprintf("%v", Total),
		Porcentaje: fmt.Sprintf("%f", Porcentaje),
		Usado:      fmt.Sprintf("%v", used),
		Libre:      fmt.Sprintf("%v", free),
	}

	b, err := json.MarshalIndent(datos, "", "  ")
	if err != nil {
		fmt.Println(err)
	}
	// valores de la ram en consola
	fmt.Print(string(b))

	// enviamos en formato json los datos de la ram mediante peticion http
	json.NewEncoder(w).Encode(datos)

}

//funcion que realizara Grafana
func metrica(w http.ResponseWriter, req *http.Request) {
	enableCors(&w) // habilitamos cors
	println("******** Cargar Datos para Grafana******")
	println("sopes")
	//////////////////////RAM////////////////////////
	// se utilizo mem.virtualmemory()   ya que los valores del archivo memo no mostraron valores exactos
	v, _ := mem.VirtualMemory()
	var Total = v.Total / (1024 * 1024)                  //valor total de ram en mb
	var Porcentaje = v.UsedPercent                       // porcentaje de utiliacion
	var used = uint64(float64(Total) * Porcentaje / 100) // cantidad de memoria utilizada
	var free = Total - used                              // cantidad de memoria libre
	datos := Ram{
		Total:      fmt.Sprintf("%v", Total),
		Porcentaje: fmt.Sprintf("%f", Porcentaje),
		Usado:      fmt.Sprintf("%v", used),
		Libre:      fmt.Sprintf("%v", free),
	}
	////////////////////CPU////////////////////////
	percent, _ := cpu.Percent(time.Second, true)
	var nucleo1 = percent[0]   // obtenemos el uso del procesador
	var libre1 = 100 - nucleo1 // obtenemos el espacio libre del procesador
	// si se tiene mas de 1 nucleo se puede poner de la siguiente manera
	//percent[1],percent[2],percent[n]
	fmt.Printf("nucleo 1: %v\n", nucleo1)
	fmt.Printf("libre 1: %v,\n", libre1)
	dato := Cpu{
		Nucleo1: fmt.Sprintf("%v", nucleo1),
		Libre1:  fmt.Sprintf("%v", libre1),
	}
	//////////////////PROCESOS CPU//////////////////
	b, err := ioutil.ReadFile("/proc/procesos") // obtenemos el archivo
	if err != nil {
		fmt.Print(err)
	}

	var ctprocs = 0

	for i := 0; i < len(b); i++ {
		ctprocs = ctprocs + 1
	}
	strfinal := `# HELP go_total_ram cantidad total.
# TYPE go_total_ram gauge
go_total_ram ` + string(datos.Total) + `
# HELP go_porcentaje_ram cantidad de porcentaje.
# TYPE go_porcentaje_ram gauge
go_porcentaje_ram ` + string(datos.Porcentaje) + `
# HELP go_usado_ram cantidad de memoria usado.
# TYPE go_usado_ram gauge
go_usado_ram ` + string(datos.Usado) + `
# HELP go_libre_ram cantidad de memoria libre.
# TYPE go_libre_ram gauge
go_libre_ram ` + string(datos.Libre) + `
# HELP go_porcentaje_cpu cantidad porcentaje utilizado.
# TYPE go_porcentaje_cpu gauge
go_porcentaje_cpu ` + string(dato.Nucleo1) + `
# HELP go_libre_cpu cantidad porcentaje libre.
# TYPE go_libre_cpu gauge
go_libre_cpu ` + string(dato.Libre1) + `
# HELP go_proc_cpu cantidad procesos cpu.
# TYPE go_proc_cpu gauge
go_proc_cpu ` + strconv.Itoa(ctprocs)

	println(strfinal)

	// enviamos en formato json los datos de la ram mediante peticion http
	fmt.Fprintf(w, strfinal)

}

func homeCPU(w http.ResponseWriter, r *http.Request) {
	enableCors(&w)
	println("******** Cargar Datos CPU******")
	println("sopes")
	//w.Write([]byte("Datos Del CPU")

	percent, _ := cpu.Percent(time.Second, true)
	var nucleo1 = percent[0]   // obtenemos el uso del procesador
	var libre1 = 100 - nucleo1 // obtenemos el espacio libre del procesador
	// si se tiene mas de 1 nucleo se puede poner de la siguiente manera
	//percent[1],percent[2],percent[n]
	fmt.Printf("nucleo 1: %v\n", nucleo1)
	fmt.Printf("libre 1: %v,\n", libre1)
	dato := Cpu{
		Nucleo1: fmt.Sprintf("%v", nucleo1),
		Libre1:  fmt.Sprintf("%v", libre1),
	}
	b, err := json.MarshalIndent(dato, "", "  ")
	if err != nil {
		fmt.Println(err)
	}
	// valores en consola
	fmt.Print(string(b))
	// enviamos en formato json los datos del cpu mediante peticion http
	json.NewEncoder(w).Encode(dato)
}
func datosCPU(w http.ResponseWriter, r *http.Request) {
	enableCors(&w)
	println("******** Cargar Datos CPU******")
	println("sopes")
	b, err := ioutil.ReadFile("/proc/procesos") // obtenemos el archivo
	if err != nil {
		fmt.Print(err)
	}

	var ctproc = 0

	for i := 0; i < len(b); i++ {
		ctproc = ctproc + 1
	}
	str := string(b) // convert content to a 'string'

	data := Datos{
		Processos:  fmt.Sprintf("%v", ctproc),
		StrProceso: fmt.Sprintf("%v", str),
	}
	b, errors := json.MarshalIndent(data, "", "  ")
	if errors != nil {
		fmt.Println(errors)
	}
	// valores en consola
	fmt.Print(string(b))
	// enviamos en formato json los datos del cpu mediante peticion http
	json.NewEncoder(w).Encode(data)
}

func main() {
	router := mux.NewRouter()
	router.HandleFunc("/Ram", homeRAM).Methods("GET")
	router.HandleFunc("/DatoCpu", datosCPU).Methods("GET")
	router.HandleFunc("/Cpu", homeCPU).Methods("GET")
	router.HandleFunc("/metrics", metrica).Methods("GET")
	// levantamos el servidor en el puerto 4444
	fmt.Print("encendido")
	log.Fatal(http.ListenAndServe(":7894", router))
}

// esta funcion sirve para poder mandar peticiones a angular ya que habilita los cors
func enableCors(w *http.ResponseWriter) {
	(*w).Header().Set("Access-Control-Allow-Origin", "*")
}
