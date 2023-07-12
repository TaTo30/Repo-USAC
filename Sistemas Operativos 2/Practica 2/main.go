package main

import (
	"crypto/sha256"
	"encoding/csv"
	"encoding/hex"
	"encoding/json"
	"fmt"
	"github.com/gocolly/colly"
	"io/ioutil"
	"log"
	"os"
	"strconv"
	"strings"
	"time"
)

type datos struct {
	Monos   int
	Cola    int
	Nr      int
	Url     string
	Archivo string
}

type URL struct {
	Url    string
	Origen string
	Nr     int
}

type Articulo struct {
	Origen   string
	Palabras int
	Enlaces  int
	Sha      string
	Url      string
	Mono     int
}

type FOUNDED_LINK struct {
	link     string
	repetido int
}

func main() {

	argsWithProg := os.Args[1:]
	info := readConsole(argsWithProg)
	fmt.Println("monos ", info.Monos)

	m1 := false

	// Colas
	urls := []URL{{info.Url, "", info.Nr}}
	articulos := []Articulo{}

	// Channels
	nuevas_url := make(chan []URL, 1)
	articulo := make(chan Articulo, 1)

	monitos := make([]int, info.Monos)

	for {

		// Cuando ya no hay urls en la cola se rompe el ciclo
		if len(urls) == 0 {
			break
		}

		for len(urls) > 0 || monosOcupados(&monitos) { // hace el ciclo si hay monos ocupados o si hay url en espera

			if len(urls) == 0 {

				articulos = append(articulos, <-articulo)
				break
			}
			for n := range monitos {

				if len(urls) == 0 {
					break //rompe el ciclo para evitar un error
				}

				if monitos[n] == 0 {
					//fmt.Println("aqui", len(articulos))
					//asigna a un mono :p
					url := urls[0] // hacemos POP a la nueva url a scrapear
					urls = urls[1:]
					//validar si el link existe
					fmt.Println("En cola: ", len(urls))
					if leido(&articulos, url.Url) {
						break
					}
					monitos[n] = 1
					//fmt.Println("aqui", len(articulos), "dd")
					go scrap(nuevas_url, articulo, url, n, &monitos[n])

				}

			}
			fmt.Println("Descanso seguro")
			time.Sleep(800 * time.Millisecond)
			fmt.Println("fin descanso")

			if m1 {
				_ = append(urls, (<-nuevas_url)...)

			} else {
				if len(urls) < info.Cola {
					urls = append(urls, (<-nuevas_url)...) // las urls encontradas se agregan a las que ya existian
				} else {
					//una vez llegue a este punto solo se parsearan las pendientes
					_ = append(urls, (<-nuevas_url)...)

					m1 = true
				}
			}

			articulos = append(articulos, <-articulo) // Agregamos el articulo analizado

		}

	}

	close(nuevas_url)
	close(articulo)

	fmt.Println("Artículos analizados: ", len(articulos))

	// CUANDO SE TERMINEN DE ANALIZAR TODAS LAS URLS GENERAR EL REPORTE
	fmt.Println("----------------Generando reporte------------------")
	printFile(articulos, info.Archivo)
	fmt.Println("------Finalizo aca-----------")

}

func leido(mn *[]Articulo, link string) bool {

	for s := range *mn {
		if (*mn)[s].Url == link {
			return true
		}
	}

	return false
}

func sePuedeLeer(m string) bool {
	return false
}

func monosOcupados(mn *[]int) bool {

	for n := range *mn {
		if (*mn)[n] == 1 {
			return true
		}
	}

	return false
}

//CREACION DE JSON
func printFile(articulos []Articulo, nombre string) {

	json, _ := json.MarshalIndent(articulos, "", " ")
	err := ioutil.WriteFile(nombre, json, 0644)
	if err != nil {
		fmt.Println(err)
	}

}

func readConsole(mensajes []string) datos {
	a1, a2, a3, a4, a5 := 0, 0, 0, "", ""

	//aux := ""
	for i := 0; i < len(mensajes); i++ {
		aux := strings.Split(mensajes[i], "=")
		switch aux[0] {
		case "monos":
			a1, _ = strconv.Atoi(strings.TrimSpace(aux[1]))
		case "cola":
			a2, _ = strconv.Atoi(strings.TrimSpace(aux[1]))
		case "nr":
			a3, _ = strconv.Atoi(strings.TrimSpace(aux[1]))
		case "url":
			a4 = strings.TrimSpace(aux[1])
		case "archivo":
			a5 = strings.TrimSpace(aux[1])
		}
		//fmt.Println(aux[1])

	}
	x := datos{
		Monos:   a1,
		Cola:    a2,
		Nr:      a3,
		Url:     a4,
		Archivo: a5,
	}

	return x
}
func dup_count(list []string) map[string]int {

	duplicate_frequency := make(map[string]int)

	for _, item := range list {

		_, exist := duplicate_frequency[item]

		if exist {
			duplicate_frequency[item] += 1
		} else {
			duplicate_frequency[item] = 1
		}
	}
	return duplicate_frequency
}

func scrap(urls chan []URL, articulo chan Articulo, url URL, mono int, vlx *int) {
	//sleep al inicio para el descanso del mono :p
	time.Sleep(10 * time.Millisecond)
	// En este metodo se realiza el scrapping de una url
	fmt.Printf("Analizando url: %s, con origen: %s y Nr %d\n", url.Url, url.Origen, url.Nr)

	c := colly.NewCollector(
		colly.AllowedDomains("es.wikipedia.org"),
	)

	c.OnHTML(".mw-parser-output", func(e *colly.HTMLElement) {
		//fmt.Println("aqui", e)
		links := e.ChildAttrs("a", "href")

		//fmt.Println(links)
		words := e.ChildText("p")
		//fmt.Println(words)
		data := []byte(words)
		hash := sha256.Sum256(data)

		/*fmt.Println(len(links))
		fmt.Println(len(words))
		fmt.Printf("%x", hash[:])*/

		//Lista de links y cantidad de veces repetido

		dup_map := dup_count(links)

		var links_ordenados []FOUNDED_LINK

		for k, v := range dup_map {
			c := FOUNDED_LINK{
				link:     k,
				repetido: v,
			}
			links_ordenados = append(links_ordenados, c)
		}

		//Ordenando referencias mas mencionadas en el articulo
		for i := 0; i < len(links_ordenados)-1; i++ {
			for j := 0; j < len(links_ordenados)-i-1; j++ {
				if links_ordenados[j].repetido > links_ordenados[j+1].repetido {
					links_ordenados[j], links_ordenados[j+1] = links_ordenados[j+1], links_ordenados[j]
				}
			}
		}

		//Lista de urls que se asignaran al canal de urls encontradas
		var lista []URL
		var link string
		list_length := 0
		for i := len(links_ordenados) - 1; i >= 0; i-- {
			//si Nr es 0 no agregamos más link
			//Nr - cantidad de links a ingresar en lista
			if url.Nr == 0 || list_length == url.Nr {
				break
			}
			if strings.Contains(links_ordenados[i].link, "https://es.wikipedia.org/wiki/") {
				list_length++
				link = links_ordenados[i].link

				c := URL{
					Url:    link,
					Origen: url.Url,
					Nr:     url.Nr - 1,
				}
				lista = append(lista, c)
			} else if strings.Contains(links_ordenados[i].link, "wikidata.org") || strings.Contains(links_ordenados[i].link, "jpg") || strings.Contains(links_ordenados[i].link, "png") {
				continue
			} else if strings.Contains(links_ordenados[i].link, "/wiki/") {
				list_length++
				link = "https://es.wikipedia.org" + links_ordenados[i].link
				c := URL{
					Url:    link,
					Origen: url.Url,
					Nr:     url.Nr - 1,
				}
				lista = append(lista, c)

			}

		}

		// cuando se termine el scraping se asigna al canal las urls encontradas

		urls <- lista

		articulo <- Articulo{
			Origen:   url.Origen,
			Palabras: len(words),
			Enlaces:  len(links),
			Sha:      hex.EncodeToString(hash[:]),
			Url:      url.Url,
			Mono:     mono + 1, // se le suma porque el canal consurrente maneja de 0 a monos-1
		}

		*vlx = 0 // ya se consumió

	})
	c.OnError(func(r *colly.Response, err error) {
		fmt.Println("Request URL:", r.Request.URL, "failed with response:", r, "\nError:", err)
	})

	c.Visit(url.Url)

}

func scrapper() {
	fName := "data.csv"
	file, err := os.Create(fName)
	if err != nil {
		log.Fatalf("could not create the file, err :%q", err)
		return
	}
	defer file.Close()
	writer := csv.NewWriter(file)
	defer writer.Flush()

	c := colly.NewCollector(
		colly.AllowedDomains("es.wikipedia.org"),
	)

	c.OnHTML(".mw-parser-output", func(e *colly.HTMLElement) {
		links := e.ChildAttrs("a", "href")
		//fmt.Println(links)
		words := e.ChildText("p")
		fmt.Println(words)
		writer.Write([]string{
			strconv.Itoa(len(links)),
		})
		writer.Write([]string{
			strconv.Itoa(len(words)),
		})
		data := []byte(words)
		hash := sha256.Sum256(data)

		fmt.Println(len(links))
		fmt.Println(len(words))
		fmt.Printf("%x", hash[:])

		writer.Write([]string{
			hex.EncodeToString(hash[:]),
		})
	})

	c.Visit("https://es.wikipedia.org/wiki/Chuck_Norris")

}
