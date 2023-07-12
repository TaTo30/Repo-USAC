package main

import (
	"bufio"
	"fmt"
	"log"
	"math/rand"
	"net/http"
	"os"
	"strconv"
	"strings"
	"time"
)

type Games struct {
	Id   string
	Name string
}

/*Utils*/
var dominio string = "http://34.139.12.212/datos/"

/*Colors*/
var colorReset string = "\033[0m"
var colorRed string = "\033[31m"
var colorGreen string = "\033[32m"
var colorYellow string = "\033[33m"
var colorBlue string = "\033[34m"
var colorPurple string = "\033[35m"
var colorCyan string = "\033[36m"
var colorWhite string = "\033[37m"

//Messages
var error0 string = "Incomplete Command."
var error1 string = "Incorrect Command."
var error2 string = "Time Out."
var error3 string = "Error En La Peticion."

/*Game Variables*/
var AllGames []Games
var players string = "null"
var rungames string = "null"
var concurrence string = "null"
var timeout string = "null"

func getStrings(comando string) []string {
	chars := strings.Split(comando, "")
	var words []string

	word := ""
	lectura := false

	for _, char := range chars {

		if char == "\"" {
			if lectura == true {
				lectura = false
			} else {
				lectura = true
			}
		} else if char != " " && char != "$" {
			word += char
		} else {
			if lectura == false {
				words = append(words, word)
				word = ""
			}
		}
	}

	return words
}

func getNames(text string) {
	names := strings.Split(text, "|")

	var games []Games

	for i := 0; i < len(names); i = i + 2 {
		if i+1 < len(names) {
			games = append(games, Games{names[i], names[i+1]})
		}
	}
	AllGames = games
}

func printError(eror string) {
	fmt.Println(string(colorRed), eror, string(colorReset))
}

func analizator(comando string) {
	words := getStrings(comando + "$")

	if words[0] == "rungame" {
		for i := 1; i < len(words); i = i + 2 {
			word := words[i]

			if word == "--gamename" {
				if i+1 < len(words) {
					getNames(words[i+1])
				} else {
					printError(error0)
				}
			} else if word == "--players" {
				if i+1 < len(words) {
					players = words[i+1]
				} else {
					printError(error0)
				}
			} else if word == "--rungames" {
				if i+1 < len(words) {
					rungames = words[i+1]
				} else {
					printError(error0)
				}
			} else if word == "--concurrence" {
				if i+1 < len(words) {
					concurrence = words[i+1]
				} else {
					printError(error0)
				}
			} else if word == "--timeout" {
				if i+1 < len(words) {
					timeout = words[i+1]
				} else {
					printError(error0)
				}
			} else {
				printError(error1)
			}
		}
		gameGen()
	} else {
		printError(error1)
	}
}

func limpiar() {
	AllGames = nil
	players = "null"
	rungames = "null"
	concurrence = "null"
	timeout = "null"
}

func cli() {
	inicio := "  _________________   ____ ___.___________      ________    _____      _____  ___________\n" +
		" /   _____/\\_____  \\ |    |   \\   \\______ \\    /  _____/   /  _  \\    /     \\ \\_   _____/\n" +
		" \\_____  \\  /  / \\  \\|    |   /   ||    |  \\  /   \\  ___  /  /_\\  \\  /  \\ /  \\ |    __)_ \n" +
		" /        \\/   \\_/.  \\    |  /|   ||    `   \\ \\    \\_\\  \\/    |    \\/    Y    \\|        \\\n" +
		"/_______  /\\_____\\ \\_/______/ |___/_______  /  \\______  /\\____|__  /\\____|__  /_______  /\n" +
		"	  \\/        \\__>                    \\/          \\/         \\/         \\/        \\/ \n"

	fmt.Println(string(colorPurple), inicio, string(colorReset))

	for {
		limpiar()
		fmt.Print(string(colorGreen), "\nsquid_game> ", string(colorReset))

		scanner := bufio.NewScanner(os.Stdin)
		scanner.Scan()
		comando := strings.ToLower(strings.TrimRight(scanner.Text(), "\n"))

		if comando == "exit" {
			fmt.Print(string(colorRed), "Fin Del Juego!!!\n\n", string(colorReset))
			break
		} else {
			analizator(comando)
		}
	}
}

func getTimeOut(time string) int {
	dimensional := time[len(time)-1:]
	tiempo, _ := strconv.Atoi(time[:len(time)-1])

	if dimensional == "m" {
		return tiempo * 60 * 1000
	} else if dimensional == "s" {
		return tiempo * 1000
	} else if dimensional == "h" {
		return tiempo * 60 * 60 * 1000
	}
	return 0
}

func doPetition(enlace string) {
	res, err := http.Get(enlace)
	if err != nil {
		printError(fmt.Sprintln(error3, " ", enlace))
		log.Println(err)
		log.Println("juan")
		return
	}
	defer res.Body.Close()

	if res.StatusCode == 404 {
		printError(fmt.Sprintln(error3, " Code: 404 ", enlace))
		//return
	}
	if res.StatusCode == 202 {
		log.Printf(fmt.Sprintln("Ruta Faulty", " Code: 404 ", enlace))
		log.Printf("Código de respuesta: 404")
	}
	if res.StatusCode == 200 {

		log.Printf("Código de respuesta: %d", res.StatusCode)
	}
	//log.Printf("Encabezados: '%q'", respuesta.Header)
	//contentType := respuesta.Header.Get("Content-Type")
	//log.Printf("El tipo de contenido: '%s'", contentType)
}

func generarEndPoint(maxPlayers int, quit chan bool, terminados *int, maxGames int, trabajando *int, startTime time.Time, timeOut int) {
	*trabajando++
	for {

		players := rand.Intn(maxPlayers) + 1
		juego := rand.Intn(len(AllGames))
		game := AllGames[juego]

		endPoint := dominio + "game/" + game.Id + "/gamename/" + game.Name + "/players/" + strconv.Itoa(players) + "/" + strconv.Itoa(*terminados)

		fmt.Println(endPoint)

		doPetition(endPoint)

		*terminados++

		if *terminados == maxGames || *trabajando == 1 {
			quit <- true
			return
		}

		total := time.Since(startTime)

		if total > time.Duration(timeOut) {
			printError(fmt.Sprintln(error2, "Time Waited: ", time.Duration(timeOut), " Time Total: ", total))
			*trabajando--
			return
		}

		if (*terminados + *trabajando - 1) >= maxGames {
			*trabajando--
			return
		}
	}
}

func gameGen() {
	if len(AllGames) == 0 || players == "null" || rungames == "null" || concurrence == "null" || timeout == "null" {
		printError(error0)
	} else {
		fmt.Println(AllGames, players+rungames+concurrence+timeout)
		maxPlayers, _ := strconv.Atoi(players)
		maxTime := getTimeOut(timeout) * int(time.Millisecond)
		maxGames, _ := strconv.Atoi(rungames)
		maxConcurrence, _ := strconv.Atoi(concurrence)

		quit := make(chan bool)

		terminados := 0
		trabajando := 0
		start := time.Now()
		for i := 0; i < maxConcurrence; i++ {

			if i < maxGames {
				go generarEndPoint(maxPlayers, quit, &terminados, maxGames, &trabajando, start, maxTime)
			}
		}

		<-quit
		close(quit)
		fmt.Println("\n", colorYellow, terminados, " Juegos Generados... ", time.Since(start), "\n\n", colorReset)
	}
}

func main() {
	cli()
	/*// leer el arreglo de bytes del archivo
	jsonFile, err := os.Open("generated.json")

	if err != nil {
		fmt.Println(err.Error())
		os.Exit(1)
	}

	defer jsonFile.Close()

	datosComoBytes, _ := ioutil.ReadAll(jsonFile)

	var decoded []interface{}

	err = json.Unmarshal(datosComoBytes, &decoded)

	if err != nil {
		fmt.Println(err.Error())
	}

	for i := 0; i < len(decoded); i++ {
		clienteHttp := &http.Client{}
		// Si quieres agregar parámetros a la URL simplemente haz una
		// concatenación :)
		res1, err := http.Get("http://34.117.248.209/iniciarCarga")
		if err != nil {
			log.Fatal(err)
		}
		defer res1.Body.Close()

		url := "http://34.117.248.209/publicar"

		tweetComoJson, err := json.Marshal(decoded[i])
		if err != nil {
			// Maneja el error de acuerdo a tu situación
			log.Fatalf("Error codificando usuario como JSON: %v", err)
		}

		peticion, err := http.NewRequest("POST", url, bytes.NewBuffer(tweetComoJson))
		if err != nil {
			// Maneja el error de acuerdo a tu situación
			log.Fatalf("Error creando petición: %v", err)
		}

		peticion.Header.Add("Content-Type", "application/json")
		peticion.Header.Add("X-Hola-Mundo", "Ejemplo")
		respuesta, err := clienteHttp.Do(peticion)

		if err != nil {
			// Maneja el error de acuerdo a tu situación
			log.Fatalf("Error haciendo petición: %v", err)
		}

		// No olvides cerrar el cuerpo al terminar
		defer respuesta.Body.Close()

		cuerpoRespuesta, err := ioutil.ReadAll(respuesta.Body)
		if err != nil {
			log.Fatalf("Error leyendo respuesta: %v", err)
		}

		respuestaString := string(cuerpoRespuesta)
		//log.Printf("Código de respuesta: %d", respuesta.StatusCode)
		//log.Printf("Encabezados: '%q'", respuesta.Header)
		//contentType := respuesta.Header.Get("Content-Type")
		//log.Printf("El tipo de contenido: '%s'", contentType)
		log.Printf("Cuerpo de respuesta del servidor: '%s'", respuestaString)

		stamptime := rand.Float64()
		time.Sleep(time.Duration(stamptime) * time.Second)
		f := fmt.Sprint(stamptime)

		var jsonStr = []byte(`{"time": ` + f + "}")
		peticiones, err := http.NewRequest("POST", "http://34.133.229.81:8056/metrics", bytes.NewBuffer(jsonStr))
		if err != nil {
			// Maneja el error de acuerdo a tu situación
			log.Fatalf("Error creando petición: %v", err)
		}
		peticiones.Header.Add("Content-Type", "application/json")
		peticiones.Header.Add("X-Hola-Mundo", "Ejemplo")
		respuestas, err := clienteHttp.Do(peticiones)

		if err != nil {
			// Maneja el error de acuerdo a tu situación
			log.Fatalf("Error haciendo petición: %v", err)
		}

		// No olvides cerrar el cuerpo al terminar
		defer respuestas.Body.Close()
		cuerpoRespuestas, err := ioutil.ReadAll(respuestas.Body)
		if err != nil {
			log.Fatalf("Error leyendo respuesta: %v", err)
		}

		respuestaStrings := string(cuerpoRespuestas)
		log.Printf("Cuerpo de respuesta del servidor: '%s'", respuestaStrings)
		res, err := http.Get("http://34.117.248.209/finalizarCarga")
		if err != nil {
			log.Fatal(err)
		}
		defer res.Body.Close()

	}*/
}
