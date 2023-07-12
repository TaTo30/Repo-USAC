package main

import (
	"bytes"
	"encoding/json"
	"fmt"
	"html/template"
	"io/ioutil"
	"log"
	"net/http"
	"os"
	"os/exec"
)

type TemplateDesing struct {
	Entrada  string
	Filename string

	//PARA LA SALIDA JAVASCRIPT
	JSOutput string
	//PARA LA SALIDA PYTHON
	PYOutput string

	//TABLA DE ERROR JS
	HTMLErrjs template.HTML
	//TABLA DE TOKENS JS
	HTMLTknjs template.HTML

	//TABLA DE ERROR PY
	HTMLErrpy template.HTML
	//TABLA DE TOKENS PY
	HTMLTknpy template.HTML
}

type JEntrada struct {
	Texto string
}

type Errores struct {
	Token   string
	Linea   int
	Columna int
	Tipo    int
}

type Tokens struct {
	Valor   string
	Tipo    string
	Linea   int
	Columna int
}

type Retorno struct {
	Errores    []Errores
	Tokens     []Tokens
	Traduccion string
	Dot        string
}

var templatedesing TemplateDesing

func scanner(w http.ResponseWriter, r *http.Request) {
	entrada := r.FormValue("inputJavaText")
	filename := r.FormValue("inputFileName")
	templatedesing.Entrada = entrada
	templatedesing.Filename = filename

	//DECLARAR LAS URLS HACIA LA API

	urljs := "http://localhost:3000/javascript"
	urlpy := "http://localhost:3030/python"

	//=======================DEFINIIR LOS DATOS DE ENTRADA========================
	jsonparse := JEntrada{
		Texto: templatedesing.Entrada,
	}
	datos, err0 := json.Marshal(jsonparse)
	if err0 != nil {
		log.Println("Ha ocurrido un error en el parseo de datos")
		log.Println(err0)
		return
	}

	//==================ENVIAR DATOS AL TRADUCTOR JAVASCRIPT=====================
	res, err1 := http.Post(urljs, "application/json", bytes.NewBuffer(datos))
	if err1 != nil {
		log.Println("Ha ocurrido un error con la peticion JS")
		log.Println(err1)
		return
	}
	defer res.Body.Close()
	bodyBytes, _ := ioutil.ReadAll(res.Body)
	resJsonParseJS := Retorno{}
	json.Unmarshal(bodyBytes, &resJsonParseJS)

	//==================ENVIAR DATOS AL TRADUCTOR PYTHON=========================
	res0, err2 := http.Post(urlpy, "application/json", bytes.NewBuffer(datos))
	if err2 != nil {
		log.Println("Ha ocurrido un error con la peticion PY")
		log.Println(err2)
		return
	}
	defer res0.Body.Close()
	bodyBytes0, _ := ioutil.ReadAll(res0.Body)
	resJsonParsePY := Retorno{}
	json.Unmarshal(bodyBytes0, &resJsonParsePY)

	/*================ESTABLECER LOS DATOS DE TRADUCTOR JAVASCRIPT================*/
	//Seteamos la salida javascript
	templatedesing.JSOutput = resJsonParseJS.Traduccion
	//Seteamos la tabla de Tokens javascript
	tokenhtmlist := ""
	for i := 0; i < len(resJsonParseJS.Tokens); i++ {
		token := resJsonParseJS.Tokens[i]
		tokenhtmlist = fmt.Sprintf("%s<tr>\n<th scope=\"row\">%d</th>\n<td>%d</td>\n<td>%d</td>\n<td>%s</td>\n<td>%s</td>\n</tr>", tokenhtmlist, i+1, token.Linea, token.Columna, token.Tipo, token.Valor)
	}
	templatedesing.HTMLTknjs = template.HTML(tokenhtmlist)
	//seteamos la tabla de errores javascript
	errorhtmlist := ""
	for i := 0; i < len(resJsonParseJS.Errores); i++ {
		erro := resJsonParseJS.Errores[i]
		if erro.Tipo == 1 {
			errorhtmlist = fmt.Sprintf("%s<tr>\n<th scope=\"row\">%d</th>\n<td>%s</td>\n<td>%d</td>\n<td>%d</td>\n<td>El caracter %s no pertenece al lenguaje</td>\n</tr>", errorhtmlist, i+1, "Lexico", erro.Linea, erro.Columna, erro.Token)
		} else {
			errorhtmlist = fmt.Sprintf("%s<tr>\n<th scope=\"row\">%d</th>\n<td>%s</td>\n<td>%d</td>\n<td>%d</td>\n<td>Se esperaba %s</td>\n</tr>", errorhtmlist, i+1, "Sintactico", erro.Linea, erro.Columna, erro.Token)
		}
	}
	templatedesing.HTMLErrjs = template.HTML(errorhtmlist)

	/*=================ESTABLECER LOS DATOS DEL TRADUCTOR PYTHON==================*/
	//Seteamos la salida python
	templatedesing.PYOutput = resJsonParsePY.Traduccion
	//Seteamos la tabla de Tokens python
	tokenhtmlistpy := ""
	for i := 0; i < len(resJsonParsePY.Tokens); i++ {
		token := resJsonParsePY.Tokens[i]
		tokenhtmlistpy = fmt.Sprintf("%s<tr>\n<th scope=\"row\">%d</th>\n<td>%d</td>\n<td>%d</td>\n<td>%s</td>\n<td>%s</td>\n</tr>", tokenhtmlistpy, i+1, token.Linea, token.Columna, token.Tipo, token.Valor)
	}
	templatedesing.HTMLTknpy = template.HTML(tokenhtmlistpy)
	//Seteamos la tabla de errores python
	errorhtmlistpy := ""
	for i := 0; i < len(resJsonParsePY.Errores); i++ {
		erro := resJsonParsePY.Errores[i]
		if erro.Tipo == 1 {
			errorhtmlistpy = fmt.Sprintf("%s<tr>\n<th scope=\"row\">%d</th>\n<td>%s</td>\n<td>%d</td>\n<td>%d</td>\n<td>El caracter %s no pertenece al lenguaje</td>\n</tr>", errorhtmlistpy, i+1, "Lexico", erro.Linea, erro.Columna, erro.Token)
		} else {
			errorhtmlistpy = fmt.Sprintf("%s<tr>\n<th scope=\"row\">%d</th>\n<td>%s</td>\n<td>%d</td>\n<td>%d</td>\n<td>Se esperaba %s</td>\n</tr>", errorhtmlistpy, i+1, "Sintactico", erro.Linea, erro.Columna, erro.Token)
		}
	}
	templatedesing.HTMLErrpy = template.HTML(errorhtmlistpy)

	/*============================GENERAR ARCHIVO DOT PYTHON=============================*/
	//log.Println(resJsonParsePY.Dot)
	//ioutil.WriteFile("AST.dot", bytes.NewBufferString(resJsonParsePY.Dot).Bytes(), 0755)
	file, _ := os.OpenFile("ASTPY.dot", os.O_CREATE|os.O_RDWR|os.O_TRUNC, 0755)
	file.WriteString(resJsonParsePY.Dot)
	file.Close()
	cmd := exec.Command("dot", "-Tjpg", "ASTPY.dot", "-o", "./images/ASTPY.jpg")
	err3 := cmd.Run()
	if err3 != nil {
		println(err3.Error())
	}

	/*==========================GENERAR ARCHIVO DOT JAVASCRIPT===========================*/
	file0, _ := os.OpenFile("ASTJS.dot", os.O_CREATE|os.O_RDWR|os.O_TRUNC, 0755)
	file0.WriteString(resJsonParseJS.Dot)
	file0.Close()
	cmd0 := exec.Command("dot", "-Tjpg", "ASTJS.dot", "-o", "./images/ASTJS.jpg")
	err4 := cmd0.Run()
	if err4 != nil {
		println(err4.Error())
	}

	t := template.Must(template.ParseFiles("./template/index.html"))
	t.Execute(w, templatedesing)
}

func index(w http.ResponseWriter, r *http.Request) {
	templatedesing.Entrada = ""
	templatedesing.JSOutput = ""
	templatedesing.PYOutput = ""
	templatedesing.Filename = "Seleccionar un Archivo"
	templatedesing.HTMLErrjs = ""
	templatedesing.HTMLTknjs = ""
	templatedesing.HTMLErrpy = ""
	templatedesing.HTMLTknpy = ""

	t := template.Must(template.ParseFiles("./template/index.html"))
	t.Execute(w, templatedesing)
}

func ASTPY(w http.ResponseWriter, r *http.Request) {
	http.ServeFile(w, r, "./images/ASTPY.jpg")
}

func ASTJS(w http.ResponseWriter, r *http.Request) {
	http.ServeFile(w, r, "./images/ASTJS.jpg")
}

func main() {
	log.Print("Escuchando en el puerto 8000\n")

	http.HandleFunc("/", index)
	http.HandleFunc("/scan", scanner)
	http.HandleFunc("/ASTPY.jpg", ASTPY)
	http.HandleFunc("/ASTJS.jpg", ASTJS)
	http.ListenAndServe(":8000", nil)
	//asi es como se trabaja con git uwu
}
