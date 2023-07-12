package main

import (
	"bufio"
	"bytes"
	"encoding/binary"
	"fmt"
	"io/ioutil"
	"log"
	"math"
	"os"
	"os/exec"
	"strconv"
	"strings"
	"time"
	"unsafe"
)

func main() {

	bandera := true
	for bandera {
		fmt.Print(string(colorBlue), "$$ ", string(colorGreen))
		comando, _ := bufio.NewReader(os.Stdin).ReadString('\n')
		fmt.Print(string(colorReset))
		bandera = LeerComando(comando, 0)
	}
}

/*
	FUNCIONES CORRESPONDIENTES A LA CONSOLA DE COMANDOS Y AL INTERPRETE DE COMANDOS
	ENTRE OTRAS FUNCIONES RELACIONADAS
*/
func UnificadorComillas(split []string) ([]string, bool) {
	contadorVacios := 0
	verificadorEspacios := false
	for i := 0; i < len(split); i++ {
		//ITERAMOS EL ARRAY
		if strings.Count(split[i], "\"") == 2 {
			continue
		}
		if strings.Contains(split[i], "\"") {
			//SI ENCONTRAMOS COMILLAS ITERAMOS HASTA ENCONTRAR OTRAS
			verificadorEspacios = true
			for j := i + 1; j < len(split); j++ {
				split[i] = split[i] + " " + split[j]
				contadorVacios++
				if strings.Contains(split[j], "\"") {
					verificadorEspacios = false
					break
				}
			}
			//MOVEMOS LOS ESPACIOS LA CANTIDAD DE VACIOS QUE HUBO
			for j := 0; j < len(split)-contadorVacios-i-1; j++ {
				split[i+j+1] = split[i+j+contadorVacios+1]
			}
			//VACIAMOS LOS ESPACIOS QUE QUEDARON VACIOS
			for j := len(split) - contadorVacios; j < len(split); j++ {
				split[j] = ""
			}
		}
		contadorVacios = 0
	}
	return split, verificadorEspacios
}

func LeerComando(comando string, tipo int) bool {
	if strings.Compare(strings.ToUpper(comando), "EXIT\n") != 0 && strings.Compare(comando, "") != 0 {
		if comando[0] == '#' {
			//SI EL COMANDO ES UN COMENTARIO SOLO SE IMPRIME
			fmt.Println(string("\033[32m"), comando, string("\033[0m"))
		} else if strings.Contains(comando, "\\*") {
			//SI EL COMANDO TIENE MULTILINEA SE ESPERA LA NUEVA LINEA
			comandoConcatenado := ""
			if tipo == 0 {
				comandoConcatenado = MultiLinea_Consola(comando)
			} else {
				comandoConcatenado = MultiLinea_Script(comando)
			}
			Interprete(comandoConcatenado)
		} else {
			//CUALQUIER OTRO CASO SERA UN COMANDO DE UNA LINEA
			Interprete(comando)
		}
		return true
	} else {
		//SI EL COMANDO ES "EXIT" SE CIERRA EL PROGRAMA
		return false
	}
}

func MultiLinea_Consola(comando string) string {
	fmt.Print(string("\033[34m"), "*> ", string("\033[0m"))
	comando = strings.Trim(comando, "\\*\n") //eliminamos '\*' para concatenar
	comando = strings.TrimRight(comando, " ")
	temporal, _ := bufio.NewReader(os.Stdin).ReadString('\n') //leemos la nueva linea
	if strings.Contains(temporal, "\\*") {
		//verificamos si hay mas multilinea, en el caso de que lo sea enviamos el comando concatenado
		return MultiLinea_Consola(comando + " " + temporal)
	} else {
		return comando + " " + temporal
	}
}

func MultiLinea_Script(comando string) string {
	comando = strings.Trim(comando, "\\*")
	comando = strings.TrimRight(comando, " ")
	temporal := listaComandos[0]
	comandosEjecutados++
	DesplazarComandos()
	if strings.Contains(temporal, "\\*") {
		return MultiLinea_Script(comando + " " + temporal)
	} else {
		return comando + " " + temporal
	}
}

func Interprete(comando string) {
	var comandoSplitted []string //variable que contiene un array de strings con los parametros
	var panicSplit bool          //variable que verifica que las comillas sean un error
	if strings.Contains(comando, "\"") {
		//SI SE TIENE COMILLAS SE JUNTAN LOS PARAMETROS
		comandoSplitted, panicSplit = UnificadorComillas(strings.Split(comando, " "))
		if panicSplit {
			comandoSplitted = strings.Split(comando, " ")
		}
	} else {
		//CASO CONTRARIO SE TOMA SPLITEA TODO EL COMANDO COMO VIENE
		comandoSplitted = strings.Split(comando, " ")
	}
	funcion := comandoSplitted[0]
	switch strings.ToUpper(funcion) {
	case "EXEC":
		FuncionEXEC(comandoSplitted)
	case "MKDISK":
		FuncionMKDISK(comandoSplitted)
	case "RMDISK":
		FuncionRMDISK(comandoSplitted)
	case "FDISK":
		FuncionFDISK(comandoSplitted)
	case "PAUSE":
		FuncionPAUSE(comandoSplitted)
	case "REV":
		FuncionREV(comandoSplitted)
	case "MOUNT":
		FuncionMOUNT(comandoSplitted)
	case "UNMOUNT":
		FuncionUNMOUNT(comandoSplitted)
	case "REP":
		FuncionREP(comandoSplitted)
	case "MKFS":
		FuncionMKFS(comandoSplitted)
	case "LOGIN":
		FuncionLOGIN(comandoSplitted)
	case "MKGRP":
		FuncionMKGRP(comandoSplitted)
	case "RMGRP":
		FuncionRMGRP(comandoSplitted)
	case "RMUSR":
		FuncionRMUSR(comandoSplitted)
	case "MKUSR":
		FuncionMKUSR(comandoSplitted)
	case "MKFILE":
		FuncionMKFILE(comandoSplitted)
	case "CAT":
		FuncionCAT(comandoSplitted)
	case "EDIT":
		FuncionEDIT(comandoSplitted)
	case "RM":
		FuncionRM(comandoSplitted)
	case "REN":
		FuncionREN(comandoSplitted)
	case "MKDIR":
		FuncionMKDIR(comandoSplitted)
	case "MV":
		FuncionMV(comandoSplitted)
	case "CP":
		FuncionCP(comandoSplitted)
	case "LOSS":
		FuncionLOSS(comandoSplitted)
	case "RECOVERY":
		FuncionRECOVERY(comandoSplitted)
	default:
		if strings.Contains(strings.ToUpper(comando), "PAUSE") {
			FuncionPAUSE(comandoSplitted)
		} else if strings.Contains(strings.ToUpper(comando), "MOUNT") {
			FuncionMOUNT(comandoSplitted)
		} else if strings.Contains(strings.ToUpper(comando), "LOGOUT") {
			FuncionLOGOUT(comandoSplitted)
		} else {
			fmt.Println("No se reconoce el comando: " + funcion)
		}
	}
}

func DesplazarComandos() {
	for i := 1; i < len(listaComandos); i++ {
		listaComandos[i-1] = listaComandos[i]
	}
}

/*
	SECUENCIA DE FUNCIONES PARA EL FUNCIONAMIENTO DEL COMANDO MKDISK
	Y LA CREACION DE DISCOS ARCHIVOS BINARIOS ETC.
*/
func FuncionMKDISK(params []string) {
	//DECLARACION DE PARAMETROS OBLIGATORIOS
	var path string = "none"
	var size int64 = 0
	var name string = ""
	//DECLARACION DE PARAMETROS OPCIONALES
	var unit string = "M"

	//Loop que obtiene los valores de los parametros
	for i := 1; i < len(params); i++ {
		//fmt.Println(params[i])
		parametroSplit := strings.Split(params[i], "->")
		parametro := strings.ToUpper(parametroSplit[0])
		switch parametro {
		case "-PATH":
			path = strings.Trim(parametroSplit[1], "\n")
		case "-SIZE":
			size, _ = strconv.ParseInt(strings.Trim(parametroSplit[1], "\n"), 10, 64)
			//size = strconv.ParseInt(strings.Trim(parametroSplit[1], "\\*"), 10, 64)
		case "-NAME":
			name = strings.Trim(parametroSplit[1], "\n")
		case "-UNIT":
			unit = strings.Trim(parametroSplit[1], "\n")
		case "":
		case " ":
			fmt.Print("")
		default:
			fmt.Println("No se reconoce el parametro: " + parametro)
		}
	}

	//VERIFICACION DE PARAMETROS
	if size > 0 {
		//size valido
		if strings.Compare(path, "none") != 0 {
			//path contiene una direccion
			if strings.Contains(name, ".dsk") {
				//name contiene la extension .dsk
				MKDISKSupport(path, name, size, unit)
			} else {
				fmt.Println("El nombre del disco no tiene la extension .dsk")
			}
		} else {
			fmt.Println("No se ha especificado la ruta")
		}
	} else {
		fmt.Println("El tamaño del disco debe ser mayor que 0")
	}

}

func MKDISKSupport(path string, name string, size int64, unit_OP string) {
	//SE VERIFICA QUE LA CARPETA EXISTA Y ELIMINAMOS COMILLAS SI TUVIERA
	path = strings.Trim(path, "\"")

	_, err := os.Stat(path)
	if os.IsNotExist(err) {
		//SI NO EXISTE EL DIRECTORIO: LO CREAMOS
		direrr := os.MkdirAll(path, 0777)
		if direrr != nil {
			fmt.Println(direrr)
		}
	}
	binaryFile, _ := os.Create(path + name)
	size = size * 1024
	if strings.Contains(strings.ToUpper(unit_OP), "M") {
		size = size * 1024
	}
	var cero int8 = 0
	var binario bytes.Buffer
	binary.Write(&binario, binary.BigEndian, &cero)
	escribirBytes(binaryFile, binario.Bytes())

	//NOS POSICIONAMOS AL TAMANIO DEL ARCHIVO
	binaryFile.Seek(size, 0)
	var binario2 bytes.Buffer
	binary.Write(&binario2, binary.BigEndian, &cero)
	escribirBytes(binaryFile, binario2.Bytes())

	//ESCRIBIMOS EL STRUCT DEL MBR INICIAL
	binaryFile.Seek(0, 0)
	disco := mbr{}
	copy(disco.MBR_TIME[:], time.Now().String())
	disco.MBR_SIZE = size
	disco.MBR_ASSING = contadorDiscos

	var binario3 bytes.Buffer
	binary.Write(&binario3, binary.BigEndian, &disco)
	escribirBytes(binaryFile, binario3.Bytes())

	//LEEMOS EL ARCHIVO PARA VERIFICAR
	binaryFile.Seek(0, 0)
	m := mbr{}
	var mbrsize int = int(unsafe.Sizeof(m))
	data := leerBytes(binaryFile, mbrsize)
	buffer := bytes.NewBuffer(data)
	binary.Read(buffer, binary.BigEndian, &m)
	//	fmt.Print("Info MBR:\n", m)
}

/*
	SECUENCIA DE FUNCIONES PARA EL FUNCIONAMIENTO DEL COMANDO RMDISK
	Y LA ELIMINACION DE DISCOS EN ARCHIVOS BINARIOS.
*/
func FuncionRMDISK(params []string) {
	/*
		Parametros para RMDISK
		OBLIGATORIO "-path" parametro que contiene la direccion del archivo .dsk a borrar
	*/
	//Declaracion de parametros obligatorios
	path := ""
	//Declaracion de parametros opcionales
	/*no hay*/

	//Loop que obtiene los valores de los parametros
	for i := 1; i < len(params); i++ {
		parametroSplit := strings.Split(params[i], "->")
		parametro := strings.ToUpper(parametroSplit[0])
		switch parametro {
		case "-PATH":
			path = strings.Trim(parametroSplit[1], "\n")
		case "":
		case " ":
			fmt.Print("")
		default:
			fmt.Println("No se reconoce el parametro: " + parametro)
		}
	}

	path = strings.Trim(path, "\"")
	_, err := os.Stat(path)
	if os.IsNotExist(err) {
		//SI EL ARCHIVO NO EXISTE ENVIAMOS EL ERROR
		fmt.Printf("El archivo %s no existe, %v\n", path, err)
	} else {
		//EL ARCHIVO EXISTE PROCEDEMOS A BORRARLO
		fmt.Printf("Se borrará %s\nEsta Seguro (S/n):", path)
		opcion, _ := bufio.NewReader(os.Stdin).ReadString('\n')
		if strings.Contains(opcion, "S") {
			os.Remove(path)
		}
	}
}

/*
	SECUENCIA DE FUNCIONES PARA EL FUNCIONAMIENTO DEL COMANDO FDISK
	Y EL ADMINISTRADOR DE PARTICIONES EN GENERAL
*/
func FuncionFDISK(params []string) {
	//DECLARACION DE PARAMETROS OBLITARIOS
	var size int64 = 0
	var path string = ""
	var name string = ""
	//DECLARACION DE PARAMETROS OPCIONALES
	var unit string = "K"
	var typed string = "P"
	var fit string = "WF"
	var delete string = "FULL"
	var add int64 = 0

	//LOOP QUE OBTIENE LOS PARAMETROS
	for i := 1; i < len(params); i++ {
		parametroSplit := strings.Split(params[i], "->")
		parametro := strings.ToUpper(parametroSplit[0])
		switch parametro {
		case "-SIZE":
			size, _ = strconv.ParseInt(strings.Trim(parametroSplit[1], "\n"), 10, 64)
		case "-PATH":
			path = strings.Trim(parametroSplit[1], "\n")
		case "-NAME":
			name = strings.Trim(parametroSplit[1], "\n")
		case "-UNIT":
			unit = strings.Trim(parametroSplit[1], "\n")
		case "-TYPE":
			typed = strings.Trim(parametroSplit[1], "\n")
		case "-FIT":
			fit = strings.Trim(parametroSplit[1], "\n")
		case "-DELETE":
			delete = strings.Trim(parametroSplit[1], "\n")
		case "-ADD":
			add, _ = strconv.ParseInt(strings.Trim(parametroSplit[1], "\n"), 10, 64)
		case "":
		case " ":
			//nada
		default:
			fmt.Println("No se reconoce el parametro: " + parametro)
			return
		}
	}
	//VALIDACION DE DATOS
	path = strings.Trim(path, "\"")
	if size > 0 {
		var tipobyte byte = 'P'
		var fitbyte byte = 'F'
		if strings.Contains(strings.ToUpper(fit), "B") {
			fitbyte = 'B'
		} else if strings.Contains(strings.ToUpper(fit), "W") {
			fitbyte = 'W'
		} else {
			fitbyte = 'F'
		}
		if strings.Contains(strings.ToUpper(typed), "P") {
			tipobyte = 'P'
			CrearParticion(path, name, size, fitbyte, tipobyte, unit)
		} else if strings.Contains(strings.ToUpper(typed), "E") {
			tipobyte = 'E'
			CrearParticion(path, name, size, fitbyte, tipobyte, unit)
		} else {
			tipobyte = 'L'
			CrearParticionLogica(path, name, size, fitbyte, unit)
		}

	} else if add != 0 {
		//agregar o eliminar espacio
		var arr [16]byte
		copy(arr[:], name)
		ModificarParticion(path, arr, unit, add)
	} else {
		//eliminar particion
		var arr [16]byte
		copy(arr[:], name)
		EliminarParticion(path, arr, delete)
	}

}

func ModificarParticion(pathDisk string, name [16]byte, unit string, add int64) {
	_, err := os.Stat(pathDisk)
	if os.IsNotExist(err) {
		//SI EL ARCHIVO NO EXISTE ENVIAMOS EL ERROR
		fmt.Printf("El archivo %s no existe, %v\n", pathDisk, err)
	} else {
		file, _ := os.OpenFile(pathDisk, os.O_RDWR, 0755)
		m := mbr{}
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(m)))), binary.BigEndian, &m)
		size := CalcularSizeParticion(add, unit)
		//BUSCAMOS LA PARTICION CON EL NOMBRE
		for i := 0; i < len(m.PARTS); i++ {
			if m.PARTS[i].PART_TYPE == 'E' && m.PARTS[i].PART_NAME != name {
				//VAMOS A RECORRER LAS LOGICAS
				e := ebr{}
				var inicioaperente int64 = m.PARTS[i].PART_START
				file.Seek(inicioaperente, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e)))), binary.BigEndian, &e)
				for {
					if e.PART_SIZE == 0 {
						break
					} else {
						if e.PART_NAME == name {
							//A ESTA PARTICION LE VAMOS A AGREGAR O QUITAR CHIVAS
							if size > 0 {
								//AGREGAMOS ESPACIO
								if (e.PART_NEXT - e.PART_SIZE - inicioaperente) > size {
									e.PART_SIZE += size
									e.PART_START -= size
								}
							} else {
								//QUITAR ESPACIO
								if e.PART_SIZE > size {
									e.PART_SIZE += size
									e.PART_START -= size
								}
							}
							file.Seek(inicioaperente, 0)
							var redactor bytes.Buffer
							binary.Write(&redactor, binary.BigEndian, &e)
							escribirBytes(file, redactor.Bytes())
							break
						}
						inicioaperente = e.PART_NEXT
						file.Seek(inicioaperente, 0)
						binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e)))), binary.BigEndian, &e)
					}
				}
			} else {
				//VAMOS A RECORRER LAS OTRAS PRIMARIAS O EXTENDIDAS
				if m.PARTS[i].PART_NAME == name {
					//ESTA PARTICION LE VAMOS A AGREGAR O QUITAR CHIVAS
					if size > 0 {
						//AGREGAMOS ESPACIO
						//OBTENEMOS LA POSCION DEL BYTE FINAL CON EL NUEVO ESPACIO
						Nuevo_final := m.PARTS[i].PART_START + m.PARTS[i].PART_SIZE + size
						Nuevo_inicio := m.PARTS[i].PART_START
						banderaAgregado := true
						for i := 0; i < len(m.PARTS); i++ {
							//ESTE LOOP BUSCA POR TODAS LAS PARTICIONES OCUPADAS DIFERENTES DE LA ANALIZADA
							if m.PARTS[i].PART_SIZE > 0 && m.PARTS[i].PART_NAME != name {
								Temp_inicio := m.PARTS[i].PART_START
								Temp_final := m.PARTS[i].PART_START + m.PARTS[i].PART_SIZE
								if (Nuevo_final > Temp_inicio && Nuevo_final < Temp_final) || (Temp_final > Nuevo_inicio && Temp_final < Nuevo_final) {
									//AL AUMENTAR LA PARTICION ESTA SE INTERFIERE POR LO QUE SE ABORTA LA OPERACION
									fmt.Print(string(colorRed), "No hay espacio disponible para aumentar el volumen de la particion\n", string(colorReset))
									banderaAgregado = false
									return
								}
							}
						}
						if banderaAgregado {
							//SE AGREGA EL ESPACIO
							m.PARTS[i].PART_SIZE += size
						}
					} else {
						//QUITAMOS ESPACIO
						//OBTENEMOS EL RESULTADO DE QUITARLE SIZE A LA PARTICION PARA VERFICAR
						Size_final := m.PARTS[i].PART_SIZE + size
						if Size_final > 0 {
							//NO SE VACIA LA PARTICION POR LO QUE SE PUEDE QUITAR
							if m.PARTS[i].PART_TYPE == 'P' {
								//PARTICIONES PRIMARIAS, SOLO SE QUITA
								m.PARTS[i].PART_SIZE += size
							} else {
								//PARTICIONES EXTENDIDAS, SE VE POR LAS LOGICAS
								var nextProximo int64 = 0
								e := ebr{}
								file.Seek(m.PARTS[i].PART_START, 0)
								binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e)))), binary.BigEndian, &e)
								for {
									if e.PART_SIZE == 0 {
										break
									} else {
										if e.PART_NEXT > nextProximo {
											nextProximo = e.PART_NEXT
										}
										file.Seek(e.PART_NEXT, 0)
										binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e)))), binary.BigEndian, &e)
									}
								}
								Nuevo_final := m.PARTS[i].PART_START + Size_final
								if Nuevo_final > nextProximo {
									m.PARTS[i].PART_SIZE += size
								} else {
									fmt.Print(string(colorRed), "EL espacio que intenta reducir interfiere con una particion logica\n", string(colorReset))
									return
								}
							}
						} else {
							fmt.Print(string(colorRed), "El espacio que intenta reducir es mayor que el volumen de la particion\n", string(colorReset))
							return
						}
					}
					file.Seek(0, 0)
					var reescritura bytes.Buffer
					binary.Write(&reescritura, binary.BigEndian, &m)
					escribirBytes(file, reescritura.Bytes())
					file.Close()
					return
				}
			}
			/**/
		}

	}
}

func EliminarParticion(pathDisk string, name [16]byte, tipo string) {
	_, err := os.Stat(pathDisk)
	if os.IsNotExist(err) {
		//SI EL ARCHIVO NO EXISTE ENVIAMOS EL ERROR
		fmt.Printf("El archivo %s no existe, %v\n", pathDisk, err)
	} else {
		//EL ARCHIVO EXISTE PROCEDEMOS A BORRARLO
		fmt.Printf("Se borrará %s\nEsta Seguro (S/n):", name)
		opcion, _ := bufio.NewReader(os.Stdin).ReadString('\n')
		if strings.Contains(strings.ToUpper(opcion), "S") {
			//PROTOCOLO DE ELIMINACION
			file, _ := os.OpenFile(pathDisk, os.O_RDWR, 0755)
			m := mbr{}
			var msize int = int(unsafe.Sizeof(m))
			data := leerBytes(file, msize)
			buffer := bytes.NewBuffer(data)
			binary.Read(buffer, binary.BigEndian, &m)

			if strings.Contains(strings.ToUpper(tipo), "FULL") {
				//eliminacion full
				for i := 0; i < len(m.PARTS); i++ {
					if m.PARTS[i].PART_TYPE == 'E' && m.PARTS[i].PART_NAME != name {
						//navegar por las logicas
						e := ebr{}
						file.Seek(m.PARTS[i].PART_START, 0)
						binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e)))), binary.BigEndian, &e)
						//RECORREMOS LAS PARTICIONES LOGICAS
						for {
							if e.PART_SIZE == 0 {
								break
							} else {
								if e.PART_NAME == name {
									e2 := ebr{}
									file.Seek(e.PART_NEXT, 0)
									binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e2)))), binary.BigEndian, &e2)
									ward := e.PART_START - int64(unsafe.Sizeof(e))
									fmt.Println("Eliminado Particion...")
									for i := ward; i < e.PART_NEXT; i++ {
										file.Seek(i, 0)
										var cero int64 = 0
										var redactor bytes.Buffer
										binary.Write(&redactor, binary.BigEndian, &cero)
										escribirBytes(file, redactor.Bytes())
									}
									//RESETAMOS INFO DEL EBR
									e.PART_FIT = e2.PART_FIT
									e.PART_NAME = e2.PART_NAME
									e.PART_NEXT = e2.PART_NEXT
									e.PART_SIZE = e2.PART_SIZE
									e.PART_START = e2.PART_START
									e.PART_STATUS = e2.PART_STATUS
									file.Seek(ward, 0)
									var redactor bytes.Buffer
									binary.Write(&redactor, binary.BigEndian, &e)
									escribirBytes(file, redactor.Bytes())
								} else {
									file.Seek(e.PART_NEXT, 0)
									binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e)))), binary.BigEndian, &e)
								}
							}
						}
					} else {
						if m.PARTS[i].PART_NAME == name {
							//CUALQUIER OTRA PARTICION PRIMARIA O EXTENDIDA
							//ESTABLECEMOS CERO EN EL AREA DE LA PARTICION
							fmt.Println("Eliminando Particion...")
							for j := m.PARTS[i].PART_START; j < (m.PARTS[i].PART_START + m.PARTS[i].PART_SIZE); j++ {
								file.Seek(j, 0)
								var cero int64 = 0
								var binariodelfull bytes.Buffer
								binary.Write(&binariodelfull, binary.BigEndian, &cero)
								escribirBytes(file, binariodelfull.Bytes())
							}
							//RESETEAMOS LA PARTICION
							var clnaem [16]byte
							m.PARTS[i].PART_SIZE = 0
							m.PARTS[i].PART_START = 0
							m.PARTS[i].PART_STATUS = 0
							m.PARTS[i].PART_TYPE = 'U'
							m.PARTS[i].PART_NAME = clnaem
							m.PARTS[i].PART_FIT = 'U'
						}
					}
				}
			} else {
				//fmt.Println("eliminacion parcial")
				//eliminacion Parcial
				for i := 0; i < len(m.PARTS); i++ {
					if m.PARTS[i].PART_TYPE == 'E' && m.PARTS[i].PART_NAME != name {
						//Navegar por las logicas
						e := ebr{}
						file.Seek(m.PARTS[i].PART_START, 0)
						binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e)))), binary.BigEndian, &e)
						for {
							if e.PART_SIZE == 0 {
								break
							} else {
								if e.PART_NAME == name {
									e2 := ebr{}
									file.Seek(e.PART_NEXT, 0)
									binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e2)))), binary.BigEndian, &e2)
									ward := e.PART_START - int64(unsafe.Sizeof(e))
									fmt.Println("Eliminado Particion...")
									//RESETAMOS INFO DEL EBR
									e.PART_FIT = e2.PART_FIT
									e.PART_NAME = e2.PART_NAME
									e.PART_NEXT = e2.PART_NEXT
									e.PART_SIZE = e2.PART_SIZE
									e.PART_START = e2.PART_START
									e.PART_STATUS = e2.PART_STATUS
									file.Seek(ward, 0)
									var redactor bytes.Buffer
									binary.Write(&redactor, binary.BigEndian, &e)
									escribirBytes(file, redactor.Bytes())
								} else {
									file.Seek(e.PART_NEXT, 0)
									binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e)))), binary.BigEndian, &e)
								}
							}
						}
					} else {
						//Navegar por primarias y extendidas
						if m.PARTS[i].PART_NAME == name {
							if m.PARTS[i].PART_TYPE == 'P' {
								//PARTICION PRIMARIA
								var clnaem [16]byte
								m.PARTS[i].PART_SIZE = 0
								m.PARTS[i].PART_START = 0
								m.PARTS[i].PART_STATUS = 0
								m.PARTS[i].PART_TYPE = 'U'
								m.PARTS[i].PART_NAME = clnaem
								m.PARTS[i].PART_FIT = 'U'

							} else {
								//PARTICION EXTENDIDA SE DEBE HACER FULL
								EliminarParticion(pathDisk, name, "FULL")
								return
							}
						}
					}
				}

			}
			file.Seek(0, 0)
			var reescritura bytes.Buffer
			binary.Write(&reescritura, binary.BigEndian, &m)
			escribirBytes(file, reescritura.Bytes())
			file.Close()
		}
	}
}

func CrearParticion(pathDisk string, name string, size int64, fit byte, tipo byte, unit string) {
	_, err := os.Stat(pathDisk) //verificamos el estado de la ruta de direccion
	if os.IsNotExist(err) {
		//si el error no existe tiramos un error
		fmt.Printf(string(colorRed), "Error!: La ruta %s especificada no existe\n", pathDisk, string(colorReset))
		return
	} else {
		//SI EXISTE ABRIMOS EL ARCHIVO
		file, _ := os.OpenFile(pathDisk, os.O_RDWR, 0755)
		//OBTENEMOS INFORMACION DEL MBR
		file.Seek(0, 0)
		m := mbr{}
		var mbrsize int = int(unsafe.Sizeof(m))
		data := leerBytes(file, mbrsize)
		buffer := bytes.NewBuffer(data)
		binary.Read(buffer, binary.BigEndian, &m)
		//fmt.Println(m)
		//CREAMOS UN OBJETO PARTITION
		//fmt.Println("A CREAR LA PARTICION")
		part := partition{
			PART_STATUS: 0,
			PART_FIT:    fit,
			PART_TYPE:   tipo,
			PART_START:  CalcularInicioParticion(m.PARTS, int64(unsafe.Sizeof(m)), CalcularSizeParticion(size, unit)),
			PART_SIZE:   CalcularSizeParticion(size, unit),
		}
		copy(part.PART_NAME[:], name) //ASIGAMOS NOMBRE

		//VERIFICAMOS QUE LA PARTICION ESTA DENTRO DE LOS LIMITES DEL DISCO Y EL NOMBRE NO ESTE REPETIDO
		if part.PART_START+part.PART_SIZE < m.MBR_SIZE {
			if !VerificarNombreParticion(m.PARTS, part.PART_NAME, file) {
				if VerificarParticionesOcupadas(m.PARTS) {
					estaExtendida, _ := VerificarExtendida(m.PARTS, part)
					if estaExtendida {
						for i := 0; i < len(m.PARTS); i++ {
							if m.PARTS[i].PART_SIZE == 0 {
								//ESTA PARTICION ESTA DESOCUPADA
								m.PARTS[i] = part
								//fmt.Println(m)
								file.Seek(0, 0)
								var binario bytes.Buffer
								binary.Write(&binario, binary.BigEndian, &m)
								escribirBytes(file, binario.Bytes())
								break
							}
						}
					} else {
						fmt.Print(string(colorRed), "Solo se puede tener una particion extendida\n", string(colorReset))
					}
				} else {
					fmt.Print(string(colorRed), "Se ha excedido el numero de particiones permitidas\n", string(colorReset))
				}
			} else {
				fmt.Print(string(colorRed), "Ya existe una particion con ese nombre\n", string(colorReset))
			}
		} else {
			fmt.Print(string(colorRed), "No hay espacio libre en el disco para realizar la particion\n", string(colorReset))
			return
		}
	}
}

func CrearParticionLogica(pathDisk string, name string, size int64, fit byte, unit string) {
	_, err := os.Stat(pathDisk) //verificamos el estado de la ruta de direccion
	if os.IsNotExist(err) {
		//si el error no existe tiramos un error
		fmt.Printf(string(colorRed), "Error!: La ruta %s especificada no existe\n", pathDisk, string(colorReset))
		return
	} else {
		//SI EXISTE ABRIMOS EL ARCHIVO
		file, _ := os.OpenFile(pathDisk, os.O_RDWR, 0755)
		//OBTENEMOS INFORMACION DEL MBR
		file.Seek(0, 0)
		m := mbr{}
		var mbrsize int = int(unsafe.Sizeof(m))
		data := leerBytes(file, mbrsize)
		buffer := bytes.NewBuffer(data)
		binary.Read(buffer, binary.BigEndian, &m)
		p := partition{}
		_, extend := VerificarExtendida(m.PARTS, p)
		if extend.PART_SIZE != 0 {
			//hay particion extendida se puede realizar la asignacion
			eA := ebr{}
			eS := ebr{}
			NI, NN := ParticionLogica(extend.PART_START, int64(unsafe.Sizeof(eA)), CalcularSizeParticion(size, unit), file)

			//VERIFICAMOS QUE ESTE DENTRO DE LOS LIMITES DE LA PARTICION EXTENDIDA
			if NN < extend.PART_START+extend.PART_SIZE {
				var arrname [16]byte
				copy(arrname[:], name)
				if !VerificarNombreParticion(m.PARTS, arrname, file) {
					//SE PUEDE ESCRIBIR EL EBR
					//ESCRIBIMOS EL EBR ACTUAL
					//Obtenemos una imagen ebr que se detecto puede usarse para meter una particion
					file.Seek(NI, 0)
					binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(eS)))), binary.BigEndian, &eA)
					eS.PART_NAME = eA.PART_NAME
					eS.PART_FIT = eA.PART_FIT
					eS.PART_NEXT = eA.PART_NEXT
					eS.PART_SIZE = eA.PART_SIZE
					eS.PART_STATUS = eA.PART_STATUS
					eS.PART_START = eA.PART_START

					file.Seek(NN, 0)
					var reescritura2 bytes.Buffer
					binary.Write(&reescritura2, binary.BigEndian, &eS)
					escribirBytes(file, reescritura2.Bytes())
					//AHORA ESCRIBIMOS LA NUEVA PARTICION
					eA.PART_FIT = fit
					eA.PART_NEXT = NN
					eA.PART_SIZE = CalcularSizeParticion(size, unit)
					eA.PART_START = eA.PART_NEXT - eA.PART_SIZE
					eA.PART_STATUS = 0
					copy(eA.PART_NAME[:], name)

					file.Seek(NI, 0)
					var reescritura3 bytes.Buffer
					binary.Write(&reescritura3, binary.BigEndian, &eA)
					escribirBytes(file, reescritura3.Bytes())
					file.Close()
				} else {
					fmt.Print(string(colorRed), "Ya hay una particion con este nombre\n", string(colorReset))
					return
				}
			} else {
				fmt.Print(string(colorRed), "No hay espacio libre en la particion\n", string(colorReset))
				return
			}
		} else {
			fmt.Print(string(colorRed), "No hay una particion Extendida donde asignar una particion Logica\n", string(colorReset))
			return
		}
	}
}

func ParticionLogica(inicio int64, ebrS int64, size int64, file *os.File) (IA int64, NA int64) {
	//LEEMOS EL EBR EN EL PUNTO DE INICIO INDICADO
	//INICIO PARTE DONDE EMPIEZA EBR
	e := ebr{}
	file.Seek(inicio, 0)
	binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e)))), binary.BigEndian, &e)
	//OBTENEMOS EL EBR
	if (e.PART_START - inicio) == 5 {
		return ParticionLogica(e.PART_NEXT, ebrS, size, file)
	} else {
		if e.PART_SIZE == 0 {
			return inicio, inicio + size + ebrS
		} else {
			if (e.PART_NEXT - e.PART_SIZE - inicio) > size {
				return inicio, inicio + size + ebrS
			} else {
				return ParticionLogica(e.PART_NEXT, ebrS, size, file)
			}
		}
	}
}

func CalcularInicioParticion(m [4]partition, PN_inicio int64, size int64) int64 {
	PN_final := PN_inicio + size
	//fmt.Println(PN_inicio, "->", PN_final)
	//fmt.Println(m)
	//ESTE LOOP BUSCA PARTICIONES OCUPADAS
	for i := 0; i < len(m); i++ {
		if m[i].PART_SIZE > 0 {
			//fmt.Println(m[i])
			//ESTA PARTICION ESTA OCUPADA VAMOS A VERIFICAR
			PV_inicio := m[i].PART_START
			PV_final := PV_inicio + m[i].PART_SIZE
			if (PN_final > PV_inicio && PN_final <= PV_final) || (PV_final > PN_inicio && PV_final < PN_final) {
				//EL FINAL DE MI PARTICION ESTA ENTRE MEDIAS DE ESTA PARTICION
				//O EL FINAL DE ESTA PARTICION ESTA ENTRE MI MEDIAS DE MI PARTICION
				inicionuevo := m[i].PART_START + m[i].PART_SIZE
				return CalcularInicioParticion(m, inicionuevo, size)
			}
		}
	}
	return PN_inicio
}

func VerificarExtendida(m [4]partition, p partition) (bool, partition) {
	flag := true
	var extendida partition
	if p.PART_TYPE == 'E' {
		for i := 0; i < len(m); i++ {
			if m[i].PART_TYPE == 'E' {
				flag = false
			}
		}
	}
	for i := 0; i < len(m); i++ {
		if m[i].PART_TYPE == 'E' {
			extendida = m[i]
		}
	}
	return flag, extendida
}

func VerificarNombreParticion(m [4]partition, name [16]byte, file *os.File) bool {
	flag := false
	for i := 0; i < len(m); i++ {
		if m[i].PART_TYPE == 'E' && m[i].PART_NAME != name {
			//HAY UNA EXTENDIDA Y SU NOMBRE NO EQUIVALES BUSCAMOS EN LAS LOGICAS
			//fmt.Println("ha buscar logicas")
			e := ebr{}
			file.Seek(m[i].PART_START, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e)))), binary.BigEndian, &e)
			for {
				if e.PART_SIZE == 0 {
					break
				} else {
					//	fmt.Println(e.PART_NAME, "->", name, "->")
					if e.PART_NAME == name {
						flag = true
						break
					}
					file.Seek(e.PART_NEXT, 0)
					binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e)))), binary.BigEndian, &e)
				}
			}
		} else {
			if m[i].PART_NAME == name {
				flag = true
			}
		}
	}
	return flag
}

func VerificarParticionesOcupadas(m [4]partition) bool {
	flag := false
	for i := 0; i < len(m); i++ {
		if m[i].PART_SIZE == 0 {
			flag = true
		}
	}
	return flag
}

func CalcularSizeParticion(size int64, unit string) int64 {
	if strings.Contains(strings.ToUpper(unit), "K") {
		return size * 1024
	} else if strings.Contains(strings.ToUpper(unit), "M") {
		return size * 1024 * 1024
	} else {
		return size
	}
}

func SizeParticion(name [16]byte, m [4]partition, file *os.File) (size int64, start int64) {
	for i := 0; i < len(m); i++ {
		if m[i].PART_SIZE != 0 {
			if m[i].PART_TYPE == 'E' && m[i].PART_NAME != name {
				//por las logicas
				e := ebr{}
				file.Seek(m[i].PART_START, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e)))), binary.BigEndian, &e)
				for {
					if e.PART_SIZE == 0 {
						break
					} else {
						if e.PART_NAME == name {
							return e.PART_SIZE, e.PART_START
						}
						file.Seek(e.PART_NEXT, 0)
						binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e)))), binary.BigEndian, &e)
					}
				}
			} else {
				//por las principales
				if m[i].PART_NAME == name {
					return m[i].PART_SIZE, m[i].PART_START
				}
			}
		}
	}
	return 0, 0
}

/*
	FUNCIONES DE CORRESPONDIENTES A LA COMANDOS RELACIONADOS CON LA INTERACCION CON LA
	CONSOLA DE COMANDOS Y ARCHIVOS SCRIPT ADEMAS DE OTRAS FUNCIONES UTILES
*/

func FuncionEXEC(params []string) {
	//Declaracion de parametros obligatorios
	path := ""

	//Loop que obtiene los valores de los parametros
	for i := 1; i < len(params); i++ {
		parametroSplit := strings.Split(params[i], "->")
		parametro := strings.ToUpper(parametroSplit[0])
		switch parametro {
		case "-PATH":
			path = strings.Trim(parametroSplit[1], "\n")
		default:
			fmt.Println("No se reconoce el parametro: " + parametro)
			return
		}
	}

	//Si se llega aqui se ejecuta el codigo
	path = strings.Trim(path, "\"")
	archivo, _ := ioutil.ReadFile(path)
	listaComandos = strings.Split(string(archivo), "\n")
	//procedemos a ejecutar los comandos
	cantidadComandos := len(listaComandos)
	comandosEjecutados = 0
	for comandosEjecutados < cantidadComandos {
		primerComando := listaComandos[0] //obtenemos el primer comando del array
		DesplazarComandos()               //los comandos se mueven una unidad hacia arriba
		LeerComando(primerComando, 1)     //enviamos el primer comando del array
		comandosEjecutados++              //aumentamos en 1 los comandos ejecutados
	}
}

func FuncionPAUSE(params []string) {
	fmt.Println("Se ha pausado la ejecucion...")
	fmt.Println("PULSE CUALQUIER ENTER PARA CONTINUAR")
	tecla, _ := bufio.NewReader(os.Stdin).ReadString('\n')
	if strings.Contains(tecla, "") {
		return
	}
	return
}

func FuncionREV(params []string) {
	path := ""
	for i := 1; i < len(params); i++ {
		parametroSplit := strings.Split(params[i], "->")
		parametro := strings.ToUpper(parametroSplit[0])
		switch parametro {
		case "-PATH":
			path = strings.Trim(parametroSplit[1], "\n")
		default:
			fmt.Println("No se reconoce el parametro: " + parametro)
			return
		}
	}

	//OBTENEMOS INFO DEL MBR
	path = strings.Trim(path, "\"")
	file, _ := os.OpenFile(path, os.O_RDWR, 0755)
	file.Seek(0, 0)
	m := mbr{}
	var msize int = int(unsafe.Sizeof(m))
	data := leerBytes(file, msize)
	buffer := bytes.NewBuffer(data)
	binary.Read(buffer, binary.BigEndian, &m)
	fmt.Println("MBR INFO:")
	fmt.Printf("Tamaño %d \n", m.MBR_SIZE)
	fmt.Printf("Fecha %s \n", m.MBR_TIME)
	for i := 0; i < len(m.PARTS); i++ {
		if m.PARTS[i].PART_TYPE == 'P' {
			fmt.Print(string(colorYellow))
			fmt.Printf("Particion Primaria en %d:\n", i)
			fmt.Print(string(colorReset))
			fmt.Printf("Nombre: %s, Size: %d, Tipo: %c, Inicio: %d \n", m.PARTS[i].PART_NAME, m.PARTS[i].PART_SIZE, m.PARTS[i].PART_TYPE, m.PARTS[i].PART_START)
		} else if m.PARTS[i].PART_TYPE == 'E' {
			fmt.Print(string(colorYellow))
			fmt.Printf("Particion Extendida en %d:\n", i)
			fmt.Print(string(colorReset))
			fmt.Printf("Nombre: %s, Size: %d, Tipo: %c, Inicio: %d \n", m.PARTS[i].PART_NAME, m.PARTS[i].PART_SIZE, m.PARTS[i].PART_TYPE, m.PARTS[i].PART_START)
			file.Seek(m.PARTS[i].PART_START, 0)
			e := ebr{}
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e)))), binary.BigEndian, &e)
			for {
				if e.PART_SIZE == 0 {
					break
				} else {
					fmt.Print(string(colorYellow))
					fmt.Printf("Particion Logica en %d:\n", i)
					fmt.Print(string(colorReset))
					fmt.Printf("Nombre: %s, Size: %d, Next: %d, Inicio: %d \n", e.PART_NAME, e.PART_SIZE, e.PART_NEXT, e.PART_START)
					file.Seek(e.PART_NEXT, 0)
					binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e)))), binary.BigEndian, &e)
				}
			}
		}
	}
}

func FuncionREP(params []string) {
	//VARIABLES OBLIGATORIAS
	var path string
	var nombre string
	var id string
	//VARIABLES OPCIONALES
	var ruta string
	fmt.Print(path, ruta)
	for i := 1; i < len(params); i++ {
		parametroSplit := strings.Split(params[i], "->")
		parametro := strings.ToUpper(parametroSplit[0])
		switch parametro {
		case "-PATH":
			path = strings.Trim(parametroSplit[1], "\n")
		case "-NOMBRE":
			nombre = strings.Trim(parametroSplit[1], "\n")
		case "-ID":
			id = strings.Trim(parametroSplit[1], "\n")
		case "-RUTA":
			ruta = strings.Trim(parametroSplit[1], "\n")
		case " ":
		case "":
			//NADA
		default:
			fmt.Println("No se reconoce el parametro: " + parametro)
		}
	}
	path = strings.Trim(path, "\"")
	ruta = strings.Trim(ruta, "\"")
	disco, part := ObtenerIndices(id)
	if Discos[disco].D_PARTS[part].STATUS == 0 {
		fmt.Println("El ID indicado no Existe")
		return
	}
	//EVALUAMOS EL NOMBRE
	if strings.Contains(strings.ToUpper(nombre), "DISK") {
		fmt.Println("Reporte para DISK")
		//REPORTE DEL DISCO
		//disco, _ := ObtenerIndices(id)
		file, _ := os.Open(Discos[disco].D_PATH)
		m := mbr{}
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(m)))), binary.BigEndian, &m)
		var arraytemporal [4]partition
		for i := 0; i < len(m.PARTS); i++ {
			arraytemporal[i] = m.PARTS[i]
		}
		//ORDENAMOS EL ARRAY TEMPORAL
		for i := 0; i < len(arraytemporal)-1; i++ {
			for j := i + 1; j < len(arraytemporal); j++ {
				if arraytemporal[i].PART_START > arraytemporal[j].PART_START {
					temp := arraytemporal[i]
					arraytemporal[i] = arraytemporal[j]
					arraytemporal[j] = temp
				}
			}
		}
		//IMPRIMIMOS LOS VALORES
		finAnterior := int64(unsafe.Sizeof(m))
		primarias := ""
		logicas := ""
		for i := 0; i < len(arraytemporal); i++ {
			if arraytemporal[i].PART_SIZE != 0 {
				if (arraytemporal[i].PART_START - finAnterior) < 1 {
					//PARTICION
					if arraytemporal[i].PART_TYPE == 'E' {
						//buscamos logicas
						cantidadLogicas := 0
						e := ebr{}
						inicioFisico := arraytemporal[i].PART_START
						file.Seek(inicioFisico, 0)
						binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e)))), binary.BigEndian, &e)
						for {
							if e.PART_SIZE == 0 {
								break
							} else {

								if (e.PART_START - inicioFisico) == 5 {
									//NO HAY ESPACION LIBRE
									logicas += "<td>EBR</td>\n"
									logicas += "<td>Logica</td>\n"
									cantidadLogicas += 2
									inicioFisico = e.PART_NEXT
								} else {
									//HAY ESPACIO LIBRE
									logicas += "<td>EBR</td>\n"
									logicas += "<td>Libre</td>\n"
									logicas += "<td>Logica</td>\n"
									cantidadLogicas += 3
									inicioFisico = e.PART_NEXT
								}
								file.Seek(inicioFisico, 0)
								binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(e)))), binary.BigEndian, &e)
							}
						}
						logicas += "<td>EBR</td>\n"
						cantidadLogicas++
						primarias += "<td colspan='" + fmt.Sprint(cantidadLogicas) + "'>Extendida</td>\n"
					} else {
						//son primarias
						primarias += "<td rowspan='2'>Primaria</td>\n"
					}
					finAnterior = arraytemporal[i].PART_START + arraytemporal[i].PART_SIZE
				} else {
					//ESPACION LIBRE
					primarias += "<td rowspan='2'>Libre</td>\n"
					finAnterior = arraytemporal[i].PART_START
					i--
				}
			}
		}
		if strings.Compare(primarias, "") != 0 {
			primarias = fmt.Sprint("<tr>\n<td rowspan='2'>MBR</td>\n" + primarias + "</tr>\n")
		}
		if strings.Compare(logicas, "") != 0 {
			logicas = fmt.Sprint("<tr>" + logicas + "</tr>\n")
		}
		tablaHTML := "<table border='1'>\n" +
			primarias +
			logicas +
			"</table>\n"
		archivoDOT := "digraph G {\n" +
			"node [shape=plaintext]\n" +
			"arset [label=<\n" + tablaHTML + ">];\n}"
		//fmt.Println(archivoDOT)
		fileDot, _ := os.OpenFile("tempo.dot", os.O_RDWR|os.O_CREATE, 0755)
		fileDot.Write(bytes.NewBufferString(archivoDOT).Bytes())
		fileDot.Close()

		//ESCRIBIMOS EL ARCHIVO DOT DE GRAPHVIZ
		_, status := os.Stat(path)
		if os.IsNotExist(status) {
			//fmt.Print("El Directorio No Existe\n")
			os.MkdirAll(extraerDirectorio(path), 0755)
			//fmt.Print("Directorio Creado")
			cmd := exec.Command("dot", "-Tpng", "tempo.dot", "-o", path)
			err := cmd.Run()
			if err != nil {
				panic(err)
			}
		} else {
			//fmt.Print("El archivo Existe\n")
			cmd := exec.Command("dot", "-Tpng", "tempo.dot", "-o", path)
			err := cmd.Run()
			if err != nil {
				panic(err)
			}
		}
		//ELIMINAOS TEMPO.DOT
		os.Remove("tempo.dot")
	} else if strings.Contains(strings.ToUpper(nombre), "MBR") {
		fmt.Println("Reporte para MBR")
		//REPORTE DEL MBR
		file, _ := os.Open(Discos[disco].D_PATH)
		m := mbr{}
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(m)))), binary.BigEndian, &m)
		//REPORTAMOS TAMAÑO
		report := ""
		report = fmt.Sprintf("%s<tr><td>mbr_size</td><td>%d</td></tr>\n", report, m.MBR_SIZE)
		report = fmt.Sprintf("%s<tr><td>mbr_disk_signature</td><td>%d</td></tr>\n", report, m.MBR_ASSING)
		report = fmt.Sprintf("%s<tr><td>mbr_create_date</td><td>%s</td></tr>\n", report, m.MBR_TIME)
		for i := 0; i < len(m.PARTS); i++ {
			if m.PARTS[i].PART_SIZE != 0 {
				p := m.PARTS[i]
				for j := 0; j < 16; j++ {
					if p.PART_NAME[j] == 0 {
						p.PART_NAME[j] = 32
					}
				}
				report = fmt.Sprintf("%s<tr><td>part_name %d</td><td>%s</td></tr>\n", report, i, p.PART_NAME)
				report = fmt.Sprintf("%s<tr><td>part_size %d</td><td>%d</td></tr>\n", report, i, p.PART_SIZE)
				report = fmt.Sprintf("%s<tr><td>part_start %d</td><td>%d</td></tr>\n", report, i, p.PART_START)
				report = fmt.Sprintf("%s<tr><td>part_status %d</td><td>%d</td></tr>\n", report, i, p.PART_STATUS)
				report = fmt.Sprintf("%s<tr><td>part_type %d</td><td>%c</td></tr>\n", report, i, p.PART_TYPE)
				report = fmt.Sprintf("%s<tr><td>part_fit %d</td><td>%c</td></tr>\n", report, i, p.PART_FIT)
			}
		}
		report = fmt.Sprintf("<table border='1'>%s</table>", report)
		tempoMBR := "digraph G {\n" +
			"node [shape=plaintext]\n" +
			"arset [label=<\n" + report + ">];\n}"
		//fmt.Println(archivoDOT)
		filereport, _ := os.OpenFile("tempoMBR.dot", os.O_RDWR|os.O_CREATE, 0755)
		filereport.Write(bytes.NewBufferString(tempoMBR).Bytes())
		filereport.Close()
		//ESCRIBIMOS EL ARCHIVO DOT DE GRAPHVIZ
		_, status := os.Stat(path)
		if os.IsNotExist(status) {
			fmt.Println("dot -Tpng tempoMBR.dot -o " + path)
			os.MkdirAll(extraerDirectorio(path), 0755)
			cmd := exec.Command("dot", "-Tpng", "tempoMBR.dot", "-o", path)
			err := cmd.Run()
			if err != nil {
				panic(err)
			}
		} else {
			//fmt.Println("existe dot -Tpng tempoMBR.dot -o " + path)
			cmd := exec.Command("dot", "-Tpng", "tempoMBR.dot", "-o", path)
			err := cmd.Run()
			if err != nil {
				panic(err)
			}
		}
		//ELIMINAOS TEMPO.DOT
		os.Remove("tempoMBR.dot")
	} else if strings.Contains(strings.ToUpper(nombre), "BM_ARBDIR") {
		fmt.Println("Reporte para BITMAP AVD")
		//REPORTE BITMAP DEL ARBOLES AVD
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
		file.Seek(Discos[disco].D_PARTS[part].START, 0) //NOS SITUAMOS EN EL INICIO DE LA PARTICION
		sb := SB{}
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		//VAMOS A LEER EL BITMAP B)
		var bitmapa string = "" //ESTE CONTIENE NUESTRO BITMAP
		var controlo int = 0    //CUANDO LLEGEMOS A 20 COSAS ESCRITAS PONEMOS UN \N
		for i := sb.SBAP_BitMap_AVD; i < sb.SBAP_BitMap_AVD+sb.SB_AVD_Count; i++ {
			file.Seek(i, 0)
			byteleidos := leerBytes(file, 1)
			bitmapa += fmt.Sprintf("%d	", byteleidos[0])
			if controlo == 20 {
				bitmapa += "\n"
				controlo = 0
			} else {
				controlo++
			}
		}
		//YA TERMINAMOS DE ESCRIBIR NUESTRO BITMAPA PROCEDEMOS A ESCRIBIRLO
		dest, _ := os.OpenFile(path, os.O_RDWR|os.O_CREATE, 0755) //instanciamos un archivo en la ruta enviada
		dest.WriteString(bitmapa)
	} else if strings.Contains(strings.ToUpper(nombre), "BM_DETDIR") {
		fmt.Println("Reporte para BITMAP DETALLE DE DIRECTORIO")
		//REPORTE BITMAP DE DETALLE DE DIRECTORIO
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
		file.Seek(Discos[disco].D_PARTS[part].START, 0) //NOS SITUAMOS EN EL INICIO DE LA PARTICION
		sb := SB{}
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		//VAMOS A LEER EL BITMAP B)
		var bitmapa string = "" //ESTE CONTIENE NUESTRO BITMAP
		var controlo int = 0    //CUANDO LLEGEMOS A 20 COSAS ESCRITAS PONEMOS UN \N
		for i := sb.SBAP_BitMap_DD; i < sb.SBAP_BitMap_DD+sb.SB_DD_Count; i++ {
			file.Seek(i, 0)
			byteleidos := leerBytes(file, 1)
			bitmapa += fmt.Sprintf("%d	", byteleidos[0])
			if controlo == 20 {
				bitmapa += "\n"
				controlo = 0
			} else {
				controlo++
			}
		}
		//YA TERMINAMOS DE ESCRIBIR NUESTRO BITMAPA PROCEDEMOS A ESCRIBIRLO
		dest, _ := os.OpenFile(path, os.O_RDWR|os.O_CREATE, 0755) //instanciamos un archivo en la ruta enviada
		dest.WriteString(bitmapa)
	} else if strings.Contains(strings.ToUpper(nombre), "BM_INODE") {
		fmt.Println("Reporte para BITMAP INODO")
		//REPORTE BITMAP DE INODO
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
		file.Seek(Discos[disco].D_PARTS[part].START, 0) //NOS SITUAMOS EN EL INICIO DE LA PARTICION
		sb := SB{}
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		//VAMOS A LEER EL BITMAP B)
		var bitmapa string = "" //ESTE CONTIENE NUESTRO BITMAP
		var controlo int = 0    //CUANDO LLEGEMOS A 20 COSAS ESCRITAS PONEMOS UN \N
		for i := sb.SBAP_BitMap_INodo; i < sb.SBAP_BitMap_INodo+sb.SB_INodo_Count; i++ {
			file.Seek(i, 0)
			byteleidos := leerBytes(file, 1)
			bitmapa += fmt.Sprintf("%d	", byteleidos[0])
			if controlo == 20 {
				bitmapa += "\n"
				controlo = 0
			} else {
				controlo++
			}
		}
		//YA TERMINAMOS DE ESCRIBIR NUESTRO BITMAPA PROCEDEMOS A ESCRIBIRLO
		dest, _ := os.OpenFile(path, os.O_RDWR|os.O_CREATE, 0755) //instanciamos un archivo en la ruta enviada
		dest.WriteString(bitmapa)
	} else if strings.Contains(strings.ToUpper(nombre), "BM_BLOCK") {
		fmt.Println("Reporte para BITMAP BLOCK")
		//REPORTE BITMAP DE INODO
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
		file.Seek(Discos[disco].D_PARTS[part].START, 0) //NOS SITUAMOS EN EL INICIO DE LA PARTICION
		sb := SB{}
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		//VAMOS A LEER EL BITMAP B)
		var bitmapa string = "" //ESTE CONTIENE NUESTRO BITMAP
		var controlo int = 0    //CUANDO LLEGEMOS A 20 COSAS ESCRITAS PONEMOS UN \N
		for i := sb.SBAP_BitMap_Bloque; i < sb.SBAP_BitMap_Bloque+sb.SB_Bloque_Count; i++ {
			file.Seek(i, 0)
			byteleidos := leerBytes(file, 1)
			bitmapa += fmt.Sprintf("%d	", byteleidos[0])
			if controlo == 20 {
				bitmapa += "\n"
				controlo = 0
			} else {
				controlo++
			}
		}
		//YA TERMINAMOS DE ESCRIBIR NUESTRO BITMAPA PROCEDEMOS A ESCRIBIRLO
		dest, _ := os.OpenFile(path, os.O_RDWR|os.O_CREATE, 0755) //instanciamos un archivo en la ruta enviada
		dest.WriteString(bitmapa)
	} else if strings.Contains(strings.ToUpper(nombre), "BITACORA") {
		fmt.Println("Reporte para BITACORA")
		//REPORTE DE BITACORA
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
		file.Seek(Discos[disco].D_PARTS[part].START, 0) //NOS SITUAMOS EN EL INICIO DE LA PARTICION
		sb := SB{}
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		//STRING QUE CONTIENE MI INFO
		logstring := ""
		//CREAMOS UNA INSTANCIA DE BITACORA
		log := Bitacora{}
		//POR CADA BITACORA AÑADIDA REPORTAMOS SU INFO
		for i := int64(0); i < sb.SB_Bitacora_Count; i++ {
			inicio := sb.SBAP_Bitacora + int64(unsafe.Sizeof(log))*i
			file.Seek(inicio, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(log)))), binary.BigEndian, &log)
			//TIPO DE OPERACION
			switch log.Log_Operacion {
			case 1:
				logstring += "Tipo de Operacion: MKFILE\n"
			case 2:
				logstring += "Tipo de Operacion: RM\n"
			case 3:
				logstring += "Tipo de Operacion: EDIT\n"
			case 4:
				logstring += "Tipo de Operacion: REN\n"
			case 5:
				logstring += "Tipo de Operacion: MKDIR\n"
			case 6:
				logstring += "Tipo de Operacion: CP\n"
			case 7:
				logstring += "Tipo de Operacion: MV\n"
			}
			//TIPO DE ACCION
			switch log.Log_Tipo {
			case 0:
				logstring += "Tipo: Archivo\n"
			case 1:
				logstring += "Tipo: Directorio\n"
			}
			//NOMBRE
			logstring += fmt.Sprintf("Nombre: %s\n", SuprimidorEspacios100(log.Log_Nombre))
			//CONTENIDO
			logstring += fmt.Sprintf("Contenido: %s\n", SuprimidorEspacios100(log.Log_Contenido))
			//FECHA
			logstring += fmt.Sprintf("Fecha: %s\n", SuprimidorEspacios20(log.Log_Fecha))
			logstring += "\n"
		}
		//YA TERMINAMOS DE ESCRIBIR NUESTRA BITACORA PROCEDEMOS A CREARLA
		dest, _ := os.OpenFile(path, os.O_RDWR|os.O_CREATE, 0755)
		dest.WriteString(logstring)
	} else if strings.Contains(strings.ToUpper(nombre), "SB") {
		fmt.Println("Reporte para SB")
		//REPORTAMOS SUPER BLOQUE
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
		file.Seek(Discos[disco].D_PARTS[part].START, 0) //NOS SITUAMOS EN EL INICIO DE LA PARTICION
		//LEEMOS EL SB
		sb := SB{}
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		//PROCEDEMOS A REPORTAR
		report := ""
		report = fmt.Sprintf("%s<tr><td>sb_nombre</td><td>%s</td></tr>\n", report, SuprimidorEspacios20(sb.SB_Name_HD))
		report = fmt.Sprintf("%s<tr><td>sb_avd_count</td><td>%d</td></tr>\n", report, sb.SB_AVD_Count)
		report = fmt.Sprintf("%s<tr><td>sb_detalle_directorio_count</td><td>%d</td></tr>\n", report, sb.SB_DD_Count)
		report = fmt.Sprintf("%s<tr><td>sb_inodo_count</td><td>%d</td></tr>\n", report, sb.SB_INodo_Count)
		report = fmt.Sprintf("%s<tr><td>sb_block_count</td><td>%d</td></tr>\n", report, sb.SB_Bloque_Count)
		report = fmt.Sprintf("%s<tr><td>sb_avd_free</td><td>%d</td></tr>\n", report, sb.SB_AVD_Free)
		report = fmt.Sprintf("%s<tr><td>sb_detalle_directorio_free</td><td>%d</td></tr>\n", report, sb.SB_DD_Free)
		report = fmt.Sprintf("%s<tr><td>sb_inodo_free</td><td>%d</td></tr>\n", report, sb.SB_INodo_Free)
		report = fmt.Sprintf("%s<tr><td>sb_bloque_free</td><td>%d</td></tr>\n", report, sb.SB_Bloque_Free)
		report = fmt.Sprintf("%s<tr><td>sb_date_create</td><td>%s</td></tr>\n", report, SuprimidorEspacios20(sb.SB_Date_Create))
		report = fmt.Sprintf("%s<tr><td>sb_last_mount</td><td>%s</td></tr>\n", report, SuprimidorEspacios20(sb.SB_Last_Date_Mount))
		report = fmt.Sprintf("%s<tr><td>sb_mount_count</td><td>%d</td></tr>\n", report, sb.SB_Mount_Count)
		report = fmt.Sprintf("%s<tr><td>sb_ap_avd_bitmap</td><td>%d</td></tr>\n", report, sb.SBAP_BitMap_AVD)
		report = fmt.Sprintf("%s<tr><td>sb_ap_avd</td><td>%d</td></tr>\n", report, sb.SBAP_AVD)
		report = fmt.Sprintf("%s<tr><td>sb_ap_dd_bitmap</td><td>%d</td></tr>\n", report, sb.SBAP_BitMap_DD)
		report = fmt.Sprintf("%s<tr><td>sb_ap_dd</td><td>%d</td></tr>\n", report, sb.SBAP_DD)
		report = fmt.Sprintf("%s<tr><td>sb_ap_inodo_bitmap</td><td>%d</td></tr>\n", report, sb.SBAP_BitMap_INodo)
		report = fmt.Sprintf("%s<tr><td>sb_ap_inodo</td><td>%d</td></tr>\n", report, sb.SBAP_INodo)
		report = fmt.Sprintf("%s<tr><td>sb_ap_bloque_bitmap</td><td>%d</td></tr>\n", report, sb.SBAP_BitMap_Bloque)
		report = fmt.Sprintf("%s<tr><td>sb_ap_bloque</td><td>%d</td></tr>\n", report, sb.SBAP_Bloque)
		report = fmt.Sprintf("%s<tr><td>sb_ap_log</td><td>%d</td></tr>\n", report, sb.SBAP_Bitacora)
		report = fmt.Sprintf("%s<tr><td>sb_size_avd</td><td>%d</td></tr>\n", report, sb.SB_AVD_Size)
		report = fmt.Sprintf("%s<tr><td>sb_size_dd</td><td>%d</td></tr>\n", report, sb.SB_DD_Size)
		report = fmt.Sprintf("%s<tr><td>sb_size_inodo</td><td>%d</td></tr>\n", report, sb.SB_INodo_Size)
		report = fmt.Sprintf("%s<tr><td>sb_size_block</td><td>%d</td></tr>\n", report, sb.SB_Bloque_Size)
		report = fmt.Sprintf("%s<tr><td>sb_avd_free_bit</td><td>%d</td></tr>\n", report, sb.SB_AVD_FreeBit)
		report = fmt.Sprintf("%s<tr><td>sb_dd_free_bit</td><td>%d</td></tr>\n", report, sb.SB_DD_FreeBit)
		report = fmt.Sprintf("%s<tr><td>sb_inodo_free_bit</td><td>%d</td></tr>\n", report, sb.SB_INodo_FreeBit)
		report = fmt.Sprintf("%s<tr><td>sb_bloque_free_bit</td><td>%d</td></tr>\n", report, sb.SB_Bloque_FreeBit)
		report = fmt.Sprintf("%s<tr><td>carnet</td><td>%d</td></tr>\n", report, sb.SB_Master_Programmer)
		report = fmt.Sprintf("<table border='1'>%s</table>", report)
		tempoMBR := "digraph G {\n" +
			"node [shape=plaintext]\n" +
			"arset [label=<\n" + report + ">];\n}"
		filereport, _ := os.OpenFile("tempoMBR.dot", os.O_RDWR|os.O_CREATE, 0755)
		filereport.Write(bytes.NewBufferString(tempoMBR).Bytes())
		filereport.Close()
		//ESCRIBIMOS EL ARCHIVO DOT DE GRAPHVIZ
		_, status := os.Stat(path)
		if os.IsNotExist(status) {
			//fmt.Println("dot -Tpng tempoMBR.dot -o " + path)
			os.MkdirAll(extraerDirectorio(path), 0755)
			cmd := exec.Command("dot", "-Tpng", "tempoMBR.dot", "-o", path)
			err := cmd.Run()
			if err != nil {
				panic(err)
			}
		} else {
			//fmt.Println("existe dot -Tpng tempoMBR.dot -o " + path)
			cmd := exec.Command("dot", "-Tpng", "tempoMBR.dot", "-o", path)
			err := cmd.Run()
			if err != nil {
				panic(err)
			}
		}
		//ELIMINAOS TEMPO.DOT
		os.Remove("tempoMBR.dot")

	} else if strings.Contains(strings.ToUpper(nombre), "TREE_COMPLETE") {
		fmt.Println("Reporte para TREE_COMPLETE")
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_CREATE|os.O_RDWR, 0755)
		file.Seek(Discos[disco].D_PARTS[part].START, 0)
		sb := SB{}
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		avd := AVD{}
		file.Seek(sb.SBAP_AVD, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
		dot := ""
		if sb.SBAP_AVD == 0 {
			dot = fmt.Sprintf("digraph g{\nnode[shape = record];\n%s\n}", "")
		} else {
			dot = fmt.Sprintf("digraph g{\nnode[shape = record];\n%s\n}", directorioRepo("", avd, file, true))
		}

		filereport, _ := os.OpenFile("tempoDIR.dot", os.O_RDWR|os.O_CREATE, 0755)
		filereport.Write(bytes.NewBufferString(dot).Bytes())
		filereport.Close()
		//ESCRIBIMOS EL ARCHIVO DOT DE GRAPHVIZ
		_, status := os.Stat(path)
		if os.IsNotExist(status) {
			//fmt.Println("dot -Tpng tempoMBR.dot -o " + path)
			os.MkdirAll(extraerDirectorio(path), 0755)
			cmd := exec.Command("dot", "-Tpng", "tempoDIR.dot", "-o", path)
			err := cmd.Run()
			if err != nil {
				panic(err)
			}
		} else {
			//fmt.Println("existe dot -Tpng tempoMBR.dot -o " + path)
			cmd := exec.Command("dot", "-Tpng", "tempoDIR.dot", "-o", path)
			err := cmd.Run()
			if err != nil {
				panic(err)
			}
		}
		//ELIMINAOS TEMPO.DOT
		os.Remove("tempoDIR.dot")
	} else if strings.Contains(strings.ToUpper(nombre), "TREE_DIRECTORIO") {
		fmt.Println("Reporte para TREE DIRECTORIO")
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_CREATE|os.O_RDWR, 0755)
		file.Seek(Discos[disco].D_PARTS[part].START, 0)
		sb := SB{}
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		avd := AVD{}
		file.Seek(sb.SBAP_AVD, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
		dot := ""
		if sb.SBAP_AVD == 0 {
			dot = fmt.Sprintf("digraph g{\nnode[shape = record];\n%s\n}", "")
		} else {
			treefile, _ := TreeDirectorio("", ruta, avd, file, sb)
			dot = fmt.Sprintf("digraph g {\nnode[shape = record];\n%s\n}", treefile)
		}
		filereport, _ := os.OpenFile("tempoDIR.dot", os.O_RDWR|os.O_CREATE, 0755)
		filereport.Write(bytes.NewBufferString(dot).Bytes())
		filereport.Close()
		//ESCRIBIMOS EL ARCHIVO DOT DE GRAPHVIZ
		_, status := os.Stat(path)
		if os.IsNotExist(status) {
			//fmt.Println("dot -Tpng tempoMBR.dot -o " + path)
			os.MkdirAll(extraerDirectorio(path), 0755)
			cmd := exec.Command("dot", "-Tpng", "tempoDIR.dot", "-o", path)
			err := cmd.Run()
			if err != nil {
				panic(err)
			}
		} else {
			//fmt.Println("existe dot -Tpng tempoMBR.dot -o " + path)
			cmd := exec.Command("dot", "-Tpng", "tempoDIR.dot", "-o", path)
			err := cmd.Run()
			if err != nil {
				panic(err)
			}
		}
		//ELIMINAOS TEMPO.DOT
		os.Remove("tempoDIR.dot")
	} else if strings.Contains(strings.ToUpper(nombre), "DIRECTORIO") {
		fmt.Println("Reporte para DIRECTORIO")
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_CREATE|os.O_RDWR, 0755)
		file.Seek(Discos[disco].D_PARTS[part].START, 0)
		sb := SB{}
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		avd := AVD{}
		file.Seek(sb.SBAP_AVD, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
		dot := ""
		if sb.SBAP_AVD == 0 {
			dot = fmt.Sprintf("digraph g{\nnode[shape = record];\n%s\n}", "")
		} else {
			dot = fmt.Sprintf("digraph g{\nnode[shape = record];\n%s\n}", directorioRepo("", avd, file, false))
		}

		filereport, _ := os.OpenFile("tempoDIR.dot", os.O_RDWR|os.O_CREATE, 0755)
		filereport.Write(bytes.NewBufferString(dot).Bytes())
		filereport.Close()
		//ESCRIBIMOS EL ARCHIVO DOT DE GRAPHVIZ
		_, status := os.Stat(path)
		if os.IsNotExist(status) {
			//fmt.Println("dot -Tpng tempoMBR.dot -o " + path)
			os.MkdirAll(extraerDirectorio(path), 0755)
			cmd := exec.Command("dot", "-Tpng", "tempoDIR.dot", "-o", path)
			err := cmd.Run()
			if err != nil {
				panic(err)
			}
		} else {
			//fmt.Println("existe dot -Tpng tempoMBR.dot -o " + path)
			cmd := exec.Command("dot", "-Tpng", "tempoDIR.dot", "-o", path)
			err := cmd.Run()
			if err != nil {
				panic(err)
			}
		}
		//ELIMINAOS TEMPO.DOT
		os.Remove("tempoDIR.dot")
	} else if strings.Contains(strings.ToUpper(nombre), "TREE_FILE") {
		fmt.Println("Reporte para TREE FILE")
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_CREATE|os.O_RDWR, 0755)
		file.Seek(Discos[disco].D_PARTS[part].START, 0)
		sb := SB{}
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		avd := AVD{}
		file.Seek(sb.SBAP_AVD, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
		dot := ""
		if sb.SBAP_AVD == 0 {
			dot = fmt.Sprintf("digraph g{\nnode[shape = record];\n%s\n}", "")
		} else {
			dot = fmt.Sprintf("digraph g {\nnode[shape = record];\nrankdir = LR;\n%s\n}", TreeFileRepo("", ruta, avd, file, sb))
		}
		filereport, _ := os.OpenFile("tempoDIR.dot", os.O_RDWR|os.O_CREATE, 0755)
		filereport.Write(bytes.NewBufferString(dot).Bytes())
		filereport.Close()
		//ESCRIBIMOS EL ARCHIVO DOT DE GRAPHVIZ
		_, status := os.Stat(path)
		if os.IsNotExist(status) {
			//fmt.Println("dot -Tpng tempoMBR.dot -o " + path)
			os.MkdirAll(extraerDirectorio(path), 0755)
			cmd := exec.Command("dot", "-Tpng", "tempoDIR.dot", "-o", path)
			err := cmd.Run()
			if err != nil {
				panic(err)
			}
		} else {
			//fmt.Println("existe dot -Tpng tempoMBR.dot -o " + path)
			cmd := exec.Command("dot", "-Tpng", "tempoDIR.dot", "-o", path)
			err := cmd.Run()
			if err != nil {
				panic(err)
			}
		}
		//ELIMINAOS TEMPO.DOT
		os.Remove("tempoDIR.dot")
	} else if strings.Contains(strings.ToUpper(nombre), "LS") {
		fmt.Println("Reporte para LS")
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_CREATE|os.O_RDWR, 0755)
		file.Seek(Discos[disco].D_PARTS[part].START, 0)
		sb := SB{}
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		avd := AVD{}
		file.Seek(sb.SBAP_AVD, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
		if sb.SBAP_AVD == 0 {
			fmt.Println("")
		} else {
			_, ls := TreeDirectorio("", ruta, avd, file, sb)
			println(ls)
		}

	}
}

func directorioRepo(dot string, dir AVD, file *os.File, complete bool) string {
	subdirectorios := ""
	//GRAFICA POR SUBDIRECTORIOS
	for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
		if dir.AVD_Sub_Directorios[i] != 0 {
			//habemus subdirectorio
			aux := AVD{} //subdirectorio
			file.Seek(dir.AVD_Sub_Directorios[i], 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(aux)))), binary.BigEndian, &aux)
			dot = directorioRepo(dot, aux, file, complete)
			subdirectorios = fmt.Sprintf("%s<f%d>%d|", subdirectorios, i, aux.AVD_Index)
			dot = fmt.Sprintf("%s\"node%d\":f%d -> \"node%d\"\n", dot, dir.AVD_Index, i, aux.AVD_Index)
		}
	}
	//GRAFICA AUXILIAR
	if dir.AVDAP != 0 {
		//habemus auxiliar
		aux := AVD{}
		file.Seek(dir.AVDAP, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(aux)))), binary.BigEndian, &aux)
		dot = directorioRepo(dot, aux, file, complete)
		dot = fmt.Sprintf("%s\"node%d\":f100 -> \"node%d\"\n", dot, dir.AVD_Index, aux.AVD_Index)
	}
	if complete {
		//DETALLE DE DIRECTORIO
		if dir.AVD_DD != 0 {
			dd := DD{}
			file.Seek(dir.AVD_DD, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(dd)))), binary.BigEndian, &dd)
			dot = DDRepo(dot, dd, file)
			dot = fmt.Sprintf("%s\"node%d\":f101 -> \"node%d\"\n", dot, dir.AVD_Index, dd.DD_Index)
		}
	}
	dot = fmt.Sprintf("%s node%d[label = \"{<f100>%s|{%s<f101>}}\"];\n", dot, dir.AVD_Index, SuprimidorEspacios20(dir.AVD_Name), subdirectorios)
	return dot
}

func DDRepo(dot string, dd DD, file *os.File) string {
	archivos := ""
	//GRAFICA POR ARCHIVOS DETALLE DE DIRECOTRIO
	for i := 0; i < len(dd.Files); i++ {
		if dd.Files[i].FIle_Inodo != 0 {
			//ESTO FORMA EL BLOQUE DETALLE DE DIRECTORIO
			archivos = fmt.Sprintf("%s{%s|<f%d>%d}|", archivos, SuprimidorEspacios20(dd.Files[i].File_Name), i, dd.Files[i].FIle_Inodo)
			dot = fmt.Sprintf("%s\"node%d\":f%d -> \"node%d\"\n", dot, dd.DD_Index, i, dd.Files[i].FIle_Inodo) //apuntador
			//INODO BEGIN
			inodo := INodo{}
			file.Seek(dd.Files[i].FIle_Inodo, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(inodo)))), binary.BigEndian, &inodo)
			bloques := ""
			//CREAMOS UN GRAFO PARA EL INODO
			apuntadorInodo := dd.Files[i].FIle_Inodo
			for {
				//ITERAMOS SOBRE LA CANTIDAD DE BLOQUES QUE TENGAMOS EN ESTE INODO
				for j := 0; j < int(inodo.ICount_Bloque); j++ {
					bloque := Bloque{}
					file.Seek(inodo.IArray_Bloque[j], 0)
					binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(bloque)))), binary.BigEndian, &bloque)
					//DONDE MIRAMOS NUESTRO BLOQUE
					bloques = fmt.Sprintf("%s{%d|<f%d>}|", bloques, inodo.IArray_Bloque[j], j)
					dot = fmt.Sprintf("%s node%d[label = \"%s\"];\n", dot, inodo.IArray_Bloque[j], strings.ReplaceAll(SuprimidorEspacios25(bloque.DB_Data), "\n", " ")) //crea grafo bloque
					dot = fmt.Sprintf("%s \"node%d\":f%d -> \"node%d\"\n", dot, apuntadorInodo, j, inodo.IArray_Bloque[j])                                              //apunta inodo a bloque
				}
				dot = fmt.Sprintf("%s node%d[label = \"{{%s}|%s<f100>}\"];\n", dot, apuntadorInodo, "INodo", bloques) //crea grafo inodo
				bloques = ""
				//VERIFICAMOS SI EXISTE UN INODO AUXILIAR, SI NO HAY SE TERMINA EL PROCESO
				if inodo.IAP == 0 {
					break
				} else {
					dot = fmt.Sprintf("%s\"node%d\":f100 -> \"node%d\"\n", dot, apuntadorInodo, inodo.IAP)
					apuntadorInodo = inodo.IAP
					file.Seek(inodo.IAP, 0)
					binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(inodo)))), binary.BigEndian, &inodo)
				}
			}

			//INODO END
		}
	}
	//DETALLE AUXILIAR
	if dd.DDAP != 0 {
		aux := DD{}
		file.Seek(dd.DDAP, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(aux)))), binary.BigEndian, &aux)
		dot = fmt.Sprintf("%s\"node%d\":f100 -> \"node%d\"\n", dot, dd.DD_Index, aux.DD_Index)
	}
	dot = fmt.Sprintf("%s node%d[label = \"{{%s}|%s<f100>}\"];\n", dot, dd.DD_Index, "DD", archivos)
	return dot
}

func TreeFileRepo(dot string, ruta string, dir AVD, file *os.File, sb SB) string {
	splitDirectorio := strings.Split(ruta, "/")
	if len(splitDirectorio) == 2 {
		//DIRECTORIO BUENO
		var name [20]byte
		copy(name[:], splitDirectorio[1])
		dot = fmt.Sprintf("%s node%d[label = \"{<f100>%s|{<f101>}}\"];\n", dot, dir.AVD_Index, SuprimidorEspacios20(dir.AVD_Name))
		//BUSCAMOS EN EL DETALLE DE DIRECTORIO
		apuntadorDD := dir.AVD_DD
		ddaux := DD{}
		for {
			file.Seek(apuntadorDD, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(ddaux)))), binary.BigEndian, &ddaux)
			for i := 0; i < len(ddaux.Files); i++ {
				if ddaux.Files[i].File_Name == name {
					//ESTE ES NUESTRO ARCHIVO
					//DIRECTORIO APUNTA AL DETALLE DE DIRECTORIO
					dot = fmt.Sprintf("%s\"node%d\":f101 -> \"node%d\"\n", dot, dir.AVD_Index, ddaux.DD_Index) //DIRECTORIO APUNTA AL DETALLE DE DIRECTORIO
					dot = fmt.Sprintf("%s node%d[label = \"{{DD}|{%s|<f0>%d}|<f100>}\"];\n", dot, ddaux.DD_Index, SuprimidorEspacios20(ddaux.Files[i].File_Name), ddaux.Files[i].FIle_Inodo)
					apuntadorInodo := ddaux.Files[i].FIle_Inodo
					inodo := INodo{}
					file.Seek(ddaux.Files[i].FIle_Inodo, 0)
					binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(inodo)))), binary.BigEndian, &inodo)
					bloques := ""
					for {
						//ITERAMOS SOBRE LA CANTIDAD DE BLOQUES QUE TENGAMOS EN ESTE INODO
						for j := 0; j < int(inodo.ICount_Bloque); j++ {
							bloque := Bloque{}
							file.Seek(inodo.IArray_Bloque[j], 0)
							binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(bloque)))), binary.BigEndian, &bloque)
							//DONDE MIRAMOS NUESTRO BLOQUE
							bloques = fmt.Sprintf("%s{%d|<f%d>}|", bloques, inodo.IArray_Bloque[j], j)
							dot = fmt.Sprintf("%s node%d[label = \"%s\"];\n", dot, inodo.IArray_Bloque[j], strings.ReplaceAll(SuprimidorEspacios25(bloque.DB_Data), "\n", " ")) //crea grafo bloque
							dot = fmt.Sprintf("%s \"node%d\":f%d -> \"node%d\"\n", dot, apuntadorInodo, j, inodo.IArray_Bloque[j])                                              //apunta inodo a bloque
						}
						dot = fmt.Sprintf("%s node%d[label = \"{{%s}|%s<f100>}\"];\n", dot, apuntadorInodo, "INodo", bloques) //crea grafo inodo
						bloques = ""
						//VERIFICAMOS SI EXISTE UN INODO AUXILIAR, SI NO HAY SE TERMINA EL PROCESO
						if inodo.IAP == 0 {
							break
						} else {
							dot = fmt.Sprintf("%s\"node%d\":f100 -> \"node%d\"\n", dot, apuntadorInodo, inodo.IAP)
							file.Seek(inodo.IAP, 0)
							binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(inodo)))), binary.BigEndian, &inodo)
							apuntadorInodo = inodo.IAP
						}
					}
					//APUNTAMOS EL DETALLE DE DIRECTORIO AL PRIMER INODO
					dot = fmt.Sprintf("%s\"node%d\":f0 -> \"node%d\"\n", dot, ddaux.DD_Index, ddaux.Files[i].FIle_Inodo)
					return dot //retornames el dot
				}
			}
			//SE ASUME QUE EN ESE DETALLE DE DIRECTORIO NO HABIA ARCHIVO
			if ddaux.DDAP != 0 {
				aux := DD{}
				file.Seek(ddaux.DDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(aux)))), binary.BigEndian, &aux)
			} else {
				return dot
			}
		}
	} else {
		//NAVEGAR POR SUB DIRECOTRIOS
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])
		//BUSCAMOS SI EN LOS SUBDIRECOTRIOS HAY UN ARBOL CON EL NOMBRE
		for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
			if dir.AVD_Sub_Directorios[i] != 0 {
				//AQUI HAY UN DIRECTORIO PROCEDEMOS A VERIFICAR SU NOMBRE
				avd := AVD{}
				file.Seek(dir.AVD_Sub_Directorios[i], 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				if avd.AVD_Name == name {
					//ESTE SUB DIRECTORIO COINCIDEN MANDAMOS A MODIFICAR EL ARCHIVO A ESE SUBDIRECTORIO
					encontrado = true //hemos encontrado el sudirectorio
					nuevaRuta := ""
					for j := 2; j < len(splitDirectorio); j++ {
						nuevaRuta += "/" + splitDirectorio[j]
					}
					/*
					 */
					//GRAFO DE ESTE DIRECTORIO
					dot = fmt.Sprintf("%s node%d[label = \"{<f100>%s|{<f101>}}\"];\n", dot, dir.AVD_Index, SuprimidorEspacios20(dir.AVD_Name))
					//APUNTADOR AL HIJO
					dot = fmt.Sprintf("%s\"node%d\":f101 -> \"node%d\"\n", dot, dir.AVD_Index, avd.AVD_Index)
					return TreeFileRepo(dot, nuevaRuta, avd, file, sb)
				}
			}
		}
		if !encontrado {
			if dir.AVDAP != 0 {
				/*
				 */
				//SI EXISTE UNA EXTENSION DEL DIRECTORIO PROCEDEMOS A BUSCAR EN ESE AMBITO
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				//GRAFO DE ESTE DIRECTORIO
				dot = fmt.Sprintf("%s node%d[label = \"{<f100>%s|{<f101>}}\"];\n", dot, dir.AVD_Index, SuprimidorEspacios20(dir.AVD_Name))
				//APUNTADOR AL HIJO
				dot = fmt.Sprintf("%s\"node%d\":f101 -> \"node%d\"\n", dot, dir.AVD_Index, avd.AVD_Index)
				return TreeFileRepo(dot, ruta, avd, file, sb)
			} else {
				//SI NO HAY EXTENSION ENTONCES EL DIRECTORIO NO EXISTE
				return dot
			}
		}
	}
	return dot
}

func TreeDirectorio(dot string, ruta string, dir AVD, file *os.File, sb SB) (tree string, ls string) {
	splitDirectorio := strings.Split(ruta, "/")
	if len(splitDirectorio) == 2 {
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])
		//BUSCAMOS EL DIRECTORIO EN LOS SUBDIRECTORIOS DE LA CARPETA ACTUAL
		for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
			if dir.AVD_Sub_Directorios[i] != 0 {
				//OBTENEMOS INFO DEL SUBDIRECTORIO
				avd := AVD{}
				file.Seek(dir.AVD_Sub_Directorios[i], 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				if avd.AVD_Name == name {
					reportels := ""
					encontrado = true //Hemos encontrado nuestra carpeta a reportar
					//RECORREMOS LOS SUBDIRECTORIOS
					for j := 0; j < len(avd.AVD_Sub_Directorios); j++ {
						if avd.AVD_Sub_Directorios[j] != 0 {
							avdAux := AVD{}
							file.Seek(avd.AVD_Sub_Directorios[j], 0)
							binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avdAux)))), binary.BigEndian, &avdAux)
							reportels = fmt.Sprintf("%s%s\n", reportels, SuprimidorEspacios20(avdAux.AVD_Name))
						}
					}
					//CONSTRUIR EL DIRECTORIO
					dot = fmt.Sprintf("%s node%d[label = \"{<f100>%s|{<f101>}}\"];\n", dot, avd.AVD_Index, SuprimidorEspacios20(avd.AVD_Name))
					dd := DD{}
					apuntadorDD := avd.AVD_DD
					for {
						file.Seek(apuntadorDD, 0)
						binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(dd)))), binary.BigEndian, &dd)
						archivos := ""
						for j := 0; j < len(dd.Files); j++ {
							if dd.Files[j].FIle_Inodo != 0 {
								//ESTO FORMA EL BLOQUE DETALLE DE DIRECTORIO
								//GUARDAMOS LOS ARCHIVOS
								reportels = fmt.Sprintf("%s%s\n", reportels, SuprimidorEspacios20(dd.Files[j].File_Name))
								archivos = fmt.Sprintf("%s{%s|<f%d>%d}|", archivos, SuprimidorEspacios20(dd.Files[j].File_Name), j, dd.Files[j].FIle_Inodo)
							}
						}
						//CREAMOS EL GRAFO DEL DD
						dot = fmt.Sprintf("%s node%d[label = \"{{%s}|%s<f100>}\"];\n", dot, apuntadorDD, "DD", archivos)
						if dd.DDAP != 0 {
							//APUNTADOR DEL DD ACTUAL AL SIGUIENTE
							dot = fmt.Sprintf("%s\"node%d\":f100 -> \"node%d\"\n", dot, apuntadorDD, dd.DDAP)
							apuntadorDD = dd.DDAP
						} else {
							break
						}
					}
					//APUNTADOR DEL DIRECTORIO AL PRIMER DETALLE DE DIRECTORIO
					dot = fmt.Sprintf("%s\"node%d\":f101 -> \"node%d\"\n", dot, avd.AVD_Index, avd.AVD_DD)
					//SE VA A RETORNAR
					return dot, reportels
				}
			}
		}
		if !encontrado {
			//NO SE ENCONTRO LA SUB CARPETA
			if dir.AVDAP != 0 {
				//SI HAY AVD DE APOYO BUSCAMOS ALLI
				avdSUB := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avdSUB)))), binary.BigEndian, &avdSUB)
				return TreeDirectorio(dot, ruta, avdSUB, file, sb)
			} else {
				return dot, ""
			}
		}
	} else {
		//NAVEGAR POR SUB DIRECOTRIOS
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])
		//BUSCAMOS SI EN LOS SUBDIRECOTRIOS HAY UN ARBOL CON EL NOMBRE
		for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
			if dir.AVD_Sub_Directorios[i] != 0 {
				//AQUI HAY UN DIRECTORIO PROCEDEMOS A VERIFICAR SU NOMBRE
				avd := AVD{}
				file.Seek(dir.AVD_Sub_Directorios[i], 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				if avd.AVD_Name == name {
					//ESTE SUB DIRECTORIO COINCIDEN MANDAMOS A MODIFICAR EL ARCHIVO A ESE SUBDIRECTORIO
					encontrado = true //hemos encontrado el sudirectorio
					nuevaRuta := ""
					for j := 2; j < len(splitDirectorio); j++ {
						nuevaRuta += "/" + splitDirectorio[j]
					}
					return TreeDirectorio(dot, nuevaRuta, avd, file, sb)
				}
			}
		}
		if !encontrado {
			if dir.AVDAP != 0 {
				//SI EXISTE UNA EXTENSION DEL DIRECTORIO PROCEDEMOS A BUSCAR EN ESE AMBITO
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				return TreeDirectorio(dot, ruta, avd, file, sb)
			} else {
				//SI NO HAY EXTENSION ENTONCES EL DIRECTORIO NO EXISTE
				return dot, ""
			}
		}
	}
	return dot, ""
}

func SuprimidorEspacios100(sliceBytes [100]byte) string {
	var cadena string
	for i := 0; i < len(sliceBytes); i++ {
		if sliceBytes[i] == 0 {
			continue
		}
		cadena += fmt.Sprintf("%c", sliceBytes[i])
	}
	return cadena
}

func SuprimidorEspacios20(sliceBytes [20]byte) string {
	var cadena string
	for i := 0; i < len(sliceBytes); i++ {
		if sliceBytes[i] == 0 {
			continue
		}
		cadena += fmt.Sprintf("%c", sliceBytes[i])
	}
	return cadena
}

func SuprimidorEspacios25(sliceBytes [25]byte) string {
	var cadena string
	for i := 0; i < len(sliceBytes); i++ {
		if sliceBytes[i] == 0 {
			continue
		}
		cadena += fmt.Sprintf("%c", sliceBytes[i])
	}
	return cadena
}

func escribirBytes(file *os.File, bytes []byte) {
	_, err := file.Write(bytes)
	//fmt.Println(n)
	if err != nil {
		log.Fatal("->:", err)
	}
}

func leerBytes(file *os.File, number int) []byte {
	bytes := make([]byte, number) //array de bytes
	_, err := file.Read(bytes)    // Leido -> bytes
	if err != nil {
		log.Fatal("Error en leer Bytes", err, " -> ", number)
	}
	return bytes
}

func extraerDirectorio(path string) string {
	split := strings.Split(path, "/")
	sendString := ""
	for i := 0; i < len(split)-1; i++ {
		sendString += split[i] + "/"
	}
	return sendString
}

/*
	DECLARACION DE LAS ESTRUCTURAS A UTILIZAR PARA: PARTICIONES, MBR, EBR, ETC
	ESPACIO PARA LA DECLARACION DE VARIABLES GLOBALES
*/

var colorReset string = "\033[0m"
var colorRed string = "\033[31m"
var colorGreen string = "\033[32m"
var colorYellow string = "\033[33m"
var colorBlue string = "\033[34m"
var colorPurple string = "\033[35m"
var colorCyan string = "\033[36m"
var colorWhite string = "\033[37m"

var listaComandos []string     //Array global que apunta a los comandos de scripts
var comandosEjecutados int = 0 //contador de comandos que se han ejecutado
var contadorDiscos int64 = 0

type partition struct {
	PART_STATUS byte
	PART_FIT    byte
	PART_START  int64
	PART_SIZE   int64
	PART_TYPE   byte
	PART_NAME   [16]byte
}

type mbr struct {
	MBR_SIZE   int64
	MBR_ASSING int64
	MBR_TIME   [20]byte
	PARTS      [4]partition
}

type ebr struct {
	PART_STATUS byte
	PART_FIT    byte
	PART_START  int64
	PART_SIZE   int64
	PART_NEXT   int64
	PART_NAME   [16]byte
}

/*
	A PARTIR DE AQUI SE TRABAJA EL LOS COMANDOS MOUNT
*/

func FuncionMOUNT(params []string) {
	//DECLARACION DE VARIABLES OBLIGATORIAS
	var path string = "none"
	var name string = "none"
	var arrname [16]byte
	//LOOP QUE RECUPERA LOS PARAMETROS
	for i := 1; i < len(params); i++ {
		parametroSplit := strings.Split(params[i], "->")
		parametro := strings.ToUpper(parametroSplit[0])
		switch parametro {
		case "-PATH":
			path = strings.Trim(parametroSplit[1], "\n")
		case "-NAME":
			name = strings.Trim(parametroSplit[1], "\n")
		case "":
		case " ":
			fmt.Print("")
		default:
			fmt.Println("No se reconoce el parametro")
		}
	}
	path = strings.Trim(path, "\"")
	//VERIFICAMOS LOS PARAMETROS
	if (strings.Compare(path, "none") == 0) || (strings.Compare(name, "none") == 0) {
		//SI ALGUNO DE LOS PARAMETROS NO SE ESPECIFICA, SE MUESTRA LA LISTA DE PARTICIONES MONTADAS
		ListarParticionesMontadas()
	} else {
		//SE MANDA HACER EL MONTAJE
		copy(arrname[:], name)
		MOUNTSupport(path, arrname)
	}
}

func ListarParticionesMontadas() {
	for i := 0; i < len(Discos); i++ {
		if Discos[i].D_SIZE != 0 {
			//hay un disco guardado aqui
			for j := 0; j < len(Discos[i].D_PARTS); j++ {
				if Discos[i].D_PARTS[j].STATUS != 0 {
					//hay una particion aqui
					fmt.Printf("ID: vd%c%d; DISK: %s; PART: %s\n", Discos[i].INDEX+ASCII, Discos[i].D_PARTS[j].INDEX, Discos[i].D_PATH, Discos[i].D_PARTS[j].P_NAME)
				}
			}
		}
	}
}

func MOUNTSupport(path string, name [16]byte) {
	dsk := BuscarDisco(path)
	if strings.Compare(dsk.D_PATH, "none") == 0 {
		//NUEVO DISCO
		//VERIFICAMOS QUE EXISTA EL DISCO
		_, err := os.Stat(path)
		if os.IsNotExist(err) {
			//el archivo no existe
			fmt.Print(string(colorRed), "El disco indicado no existe\n", string(colorReset))
		} else {
			//el archivo existe
			file, _ := os.Open(path)
			size := int(unsafe.Sizeof(file))
			mdisk := MDisk{
				D_PATH: path,
				D_SIZE: size,
				INDEX:  PosicionDisco(),
			}
			MontarParticion(name, mdisk)
		}
	} else {
		//SOLO OBTENER EL DISCO
		MontarParticion(name, dsk)
	}
}

func MontarParticion(name [16]byte, disco MDisk) {
	//VERIFICAMOS QUE LA PARTICION EXISTA EN EL DISCO
	file, _ := os.Open(disco.D_PATH)
	m := mbr{}
	binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(m)))), binary.BigEndian, &m)
	if VerificarNombreParticion(m.PARTS, name, file) {
		//LA PARTICION EXISTE EN EL DISCO
		if BuscarParticion(name, disco).STATUS == 0 {
			//LA PARTICION NO ESTA MONTADA, SE PUEDE MONTAR
			p := MPart{}
			p.INDEX = PosicionParticion(disco)
			p.P_NAME = name
			p.STATUS = 1
			p.SIZE, p.START = SizeParticion(name, m.PARTS, file)
			disco.D_PARTS[p.INDEX] = p
			//fmt.Printf("Se montara la particion: %s, en la posicion %d. del disco ubicado en %s en la posicion %d \n", p.P_NAME, p.INDEX, disco.D_PATH, disco.INDEX)
			Discos[disco.INDEX] = disco
			fmt.Printf("Particion Montada ID->vd%c%d\n", disco.INDEX+ASCII, p.INDEX)
		} else {
			fmt.Print(string(colorRed), "La particion ya esta montada\n", string(colorReset))
		}
	} else {
		fmt.Print(string(colorRed), "La particion no existe en el disco indicado\n", string(colorReset))
	}
}

func BuscarDisco(path string) MDisk {
	for i := 0; i < len(Discos); i++ {
		if strings.Compare(Discos[i].D_PATH, path) == 0 {
			//ESTE DISCO LO TENEMOS
			return Discos[i]
		}
	}
	d := MDisk{}
	d.D_PATH = "none"
	return d
}

func BuscarParticion(name [16]byte, disco MDisk) MPart {
	for i := 0; i < len(disco.D_PARTS); i++ {
		if disco.D_PARTS[i].P_NAME == name {
			//ESTA PARTICION ESTA
			return disco.D_PARTS[i]
		}
	}
	return MPart{}
}

func PosicionParticion(disco MDisk) int {
	for i := 0; i < len(disco.D_PARTS); i++ {
		if disco.D_PARTS[i].STATUS == 0 {
			return i
		}
	}
	return 0
}

func PosicionDisco() int {
	for i := 0; i < len(Discos); i++ {
		if Discos[i].D_SIZE == 0 {
			return i
		}
	}
	return 0
}

func ObtenerIndices(id string) (disco int, particion int) {
	var numero string
	if len(id) > 4 {
		numero = string(id[3]) + string(id[4])
	} else {
		numero = string(id[3])
	}
	a, _ := strconv.ParseInt(numero, 10, 64)
	return int(id[2]) - ASCII, int(a)
}

/*
	FUNCIONES PARA EL MANEJO DE COMANDO UNMOUNT
*/
func FuncionUNMOUNT(params []string) {
	for i := 1; i < len(params); i++ {
		parametroSplit := strings.Split(params[i], "->")
		parametroSplit[1] = strings.Trim(parametroSplit[1], "\n")
		disco, part := ObtenerIndices(parametroSplit[1])
		UnmountSUPPORT(disco, part)
	}
}

func UnmountSUPPORT(discoIndex int, partIndex int) {
	if Discos[discoIndex].D_SIZE != 0 {
		//EL DISCO ESTA
		if Discos[discoIndex].D_PARTS[partIndex].STATUS != 0 {
			//LA PARTICION ESTA
			Discos[discoIndex].D_PARTS[partIndex] = MPart{}
		} else {
			fmt.Print(string(colorRed))
			fmt.Printf("El ID: vd%c%d es invalido\n", discoIndex+ASCII, partIndex)
			fmt.Print(string(colorReset))
		}
	} else {
		fmt.Print(string(colorRed))
		fmt.Printf("El ID: vd%c%d es invalido\n", discoIndex+ASCII, partIndex)
		fmt.Print(string(colorReset))
	}
}

const ASCII int = 97

var Discos [26]MDisk

type MPart struct {
	P_NAME [16]byte
	INDEX  int
	STATUS int
	SIZE   int64
	START  int64
}

type MDisk struct {
	D_PATH  string
	D_SIZE  int
	D_PARTS [100]MPart
	INDEX   int
}

/*
	FUNCIONES SOBRE ARCHIVOS Y DIRECTORIOS
*/

func FuncionMKFS(params []string) {
	//Variables Obligatorias
	var id string
	//Variables Opcionales
	var tipo string
	var add string
	var unit string
	//LOOP QUE RECOGE LAS VARIABLES
	for i := 1; i < len(params); i++ {
		split := strings.Split(params[i], "->")
		parametro := strings.ToUpper(split[0])
		switch parametro {
		case "-ID":
			id = strings.Trim(split[1], "\n")
		case "-TIPO":
			tipo = strings.Trim(split[1], "\n")
		case "-ADD":
			add = strings.Trim(split[1], "\n")
		case "-UNIT":
			unit = strings.Trim(split[1], "\n")
		case "":
		case " ":
			fmt.Print("")
		default:
			fmt.Println("No se reconoce el parametro ", parametro)
		}
	}
	desechar := tipo + add + unit
	desechar = ""
	fmt.Print(desechar)
	disco, part := ObtenerIndices(id)
	particion := Discos[disco].D_PARTS[part]
	//fmt.Printf("Size %d, Start %d", particion.SIZE, particion.START)
	if particion.STATUS == 1 {
		//FORMATEAR EL DISCO
		sizeSB := int(unsafe.Sizeof(SB{}))
		sizeAVD := int(unsafe.Sizeof(AVD{}))
		sizeDD := int(unsafe.Sizeof(DD{}))
		sizeIN := int(unsafe.Sizeof(INodo{}))
		sizeB := int(unsafe.Sizeof(Bloque{}))
		sizeLOG := int(unsafe.Sizeof(Bitacora{}))
		noEstructuras := (int(particion.SIZE) - (2 * sizeSB)) / (27 + sizeAVD + sizeDD + (5*sizeIN + (20 * sizeB) + sizeLOG))
		SuperBloque := SB{}
		//TAMAÑO Y CANTIDAD DE LAS ESTRUCTURAS
		SuperBloque.SB_AVD_Count = int64(noEstructuras)
		SuperBloque.SB_AVD_Size = int64(sizeAVD)
		SuperBloque.SB_DD_Count = int64(noEstructuras)
		SuperBloque.SB_DD_Size = int64(sizeDD)
		SuperBloque.SB_INodo_Count = int64(5 * noEstructuras)
		SuperBloque.SB_INodo_Size = int64(sizeIN)
		SuperBloque.SB_Bloque_Count = int64(20 * noEstructuras)
		SuperBloque.SB_Bloque_Size = int64(sizeSB)
		//APUNTADORES
		SuperBloque.SBAP_BitMap_AVD = particion.START + int64(sizeSB)
		SuperBloque.SBAP_AVD = SuperBloque.SBAP_BitMap_AVD + int64(noEstructuras)
		SuperBloque.SBAP_BitMap_DD = SuperBloque.SBAP_AVD + int64(noEstructuras*sizeAVD)
		SuperBloque.SBAP_DD = SuperBloque.SBAP_BitMap_DD + int64(noEstructuras)
		SuperBloque.SBAP_BitMap_INodo = SuperBloque.SBAP_DD + int64(noEstructuras*sizeDD)
		SuperBloque.SBAP_INodo = SuperBloque.SBAP_BitMap_INodo + int64(5*noEstructuras)
		SuperBloque.SBAP_BitMap_Bloque = SuperBloque.SBAP_INodo + int64(5*noEstructuras*sizeIN)
		SuperBloque.SBAP_Bloque = SuperBloque.SBAP_BitMap_Bloque + int64(20*noEstructuras)
		SuperBloque.SBAP_Bitacora = SuperBloque.SBAP_Bloque + int64(20*noEstructuras*sizeB)
		//PRIMER BIT LIBRE
		SuperBloque.SB_AVD_FreeBit = 0
		SuperBloque.SB_DD_FreeBit = 0
		SuperBloque.SB_INodo_FreeBit = 0
		SuperBloque.SB_Bloque_FreeBit = 0
		//ESTRUCTURAS LIBRES
		SuperBloque.SB_AVD_Free = SuperBloque.SB_AVD_Count
		SuperBloque.SB_DD_Free = SuperBloque.SB_DD_Count
		SuperBloque.SB_INodo_Free = SuperBloque.SB_INodo_Count
		SuperBloque.SB_Bloque_Free = SuperBloque.SB_Bloque_Count
		//OTRA INFO
		copy(SuperBloque.SB_Name_HD[:], Discos[disco].D_PATH)
		copy(SuperBloque.SB_Date_Create[:], time.Now().String())
		copy(SuperBloque.SB_Last_Date_Mount[:], time.Now().String())
		SuperBloque.SB_Mount_Count = 1
		SuperBloque.SB_Master_Programmer = 201800585
		SuperBloque.SB_Bitacora_Count = 0

		//PROCEDEMOS A ESCRIBIR ESTE BLOQUE EN EL INICIO DE LA PARTICION Y AL FINAL DE LA BITACORA
		file, err := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR|os.O_CREATE, 0755)
		if err != nil {
			return
		}
		//ESCRIBIMOS EL PRIMER SB
		primerSB := particion.START
		file.Seek(primerSB, 0)
		var redactor bytes.Buffer
		binary.Write(&redactor, binary.BigEndian, &SuperBloque)
		escribirBytes(file, redactor.Bytes())
		//ESCRIBIMOS EL SB COPIA
		compiaSB := SuperBloque.SBAP_Bitacora + int64(noEstructuras*sizeLOG)
		file.Seek(compiaSB, 0)
		var redactor2 bytes.Buffer
		binary.Write(&redactor2, binary.BigEndian, &SuperBloque)
		escribirBytes(file, redactor2.Bytes())
		//CREAMOS EL DIRECTORIO ROOT//
		var nombreroot [20]byte
		copy(nombreroot[:], "/")
		crearDirectorio(nombreroot, 1, file, SuperBloque)
		//CREAMOS UN ARCHIVO users.txt //
		avd := AVD{}
		file.Seek(SuperBloque.SBAP_AVD, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
		crearFichero("/users.txt", 1, "1,G,root\n1,U,root,root,201800585\n", file, avd, true, SuperBloque)

	} else {
		fmt.Println("El ID indicado no Existe")
	}

}

var logeado bool = false
var user string = ""
var iduser string = ""

func FuncionLOGIN(params []string) {
	//DECLARACION DE PARAMETROS OBLIGATORIOS
	var usr string
	var psw string
	var id string
	//LOOP QUE RECOLECTA LOS DATOS
	for i := 1; i < len(params); i++ {
		split := strings.Split(params[i], "->")
		switch strings.ToUpper(split[0]) {
		case "-USR":
			usr = strings.Trim(split[1], "\n")
		case "-PWD":
			psw = strings.Trim(split[1], "\n")
		case "-ID":
			id = strings.Trim(split[1], "\n")
		case "":
		case " ":
			fmt.Println("")
		default:
			fmt.Println("No se reconoce el parametro ", strings.ToUpper(split[0]))
		}
	}
	//OBTENEMOS EL VALOR EN PARTICION DEL ID
	disco, part := ObtenerIndices(id)
	particion := Discos[disco].D_PARTS[part]
	if particion.STATUS == 1 {
		//OBTENEMOS EL SUPER BLOQUE DE LA PARTICION
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
		sb := SB{}
		file.Seek(particion.START, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		//OBTENEMOS EL DIRECTORIO RAIZ
		avd := AVD{}
		file.Seek(sb.SBAP_AVD, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
		//OBTENEMOS LOD DATOS DEL ARCHIVO user.txt
		usuarios := leerArchivo("/users.txt", file, avd, sb)
		datosUsers := strings.Split(usuarios, "\n") //OBTENEMOS CADA LINEA EN EL ARCHIVO DE TEXTO
		for i := 0; i < len(datosUsers); i++ {
			parametrosUsuario := strings.Split(datosUsers[i], ",")
			if len(parametrosUsuario) == 1 {
				continue
			}
			if strings.Contains(parametrosUsuario[1], "U") {
				//ESTE ES DE UNA LINEA DE USUARIO
				if strings.Contains(parametrosUsuario[3], usr) {
					//EL USUARIO ESTA BIEN
					if strings.Contains(parametrosUsuario[4], psw) {
						//LA CONTRASEÑA CORRECTA
						fmt.Println("usuario logeado")
						logeado = true
						user = usr
						iduser = parametrosUsuario[0]
						return
					} else {
						fmt.Println("La contraseña es incorrecta")
						return
					}
				} else {
					fmt.Println("El usuario es incorrecto")
					return
				}
			}
		}
	} else {
		fmt.Println("Esta particion no esta montada")
	}

}

func FuncionLOGOUT(params []string) {
	if logeado {
		logeado = false
		user = ""
		iduser = ""
	} else {
		fmt.Println("No hay una sesion activa")
	}
}

func FuncionMKGRP(params []string) {
	//DECLARACION DE PARAMETROS OBLIGATORIOS
	var id string
	var name string
	//LOOP DE RECOLECCION DE DATOS
	for i := 1; i < len(params); i++ {
		split := strings.Split(params[i], "->")
		switch strings.ToUpper(split[0]) {
		case "-ID":
			id = strings.Trim(split[1], "\n")
		case "-NAME":
			name = strings.Trim(split[1], "\n")
		case "":
		case " ":
			fmt.Print("")
		default:
			fmt.Println("No se reconoce el parametro ", strings.ToUpper(split[0]))

		}
	}
	disco, particion := ObtenerIndices(id)
	if strings.Contains(user, "root") {
		//PROCEDMOS A CREAR EL GRUPO
		part := Discos[disco].D_PARTS[particion]
		if part.STATUS == 1 {
			//EXISTE EL MONTAJE
			file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
			//OBTENEMOS EL SUPERBLOQUE
			sb := SB{}
			file.Seek(part.START, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
			//OBTENEMOS EL DIRECTORIO RAIZ
			avd := AVD{}
			file.Seek(sb.SBAP_AVD, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
			//OBTENEMOS LOS DATOS DEL ARCHIVO users.txt
			info := leerArchivo("/users.txt", file, avd, sb)
			splitUG := strings.Split(info, "\n")
			nuevoGrupo := 0
			existeGrupo := false
			nuevoArchivo := ""
			for i := 0; i < len(splitUG); i++ {
				parametros := strings.Split(splitUG[i], ",")
				if len(parametros) == 1 {
					continue
				}
				if strings.Compare(parametros[1], "G") == 0 {
					//ESTA ES UNA LINEA DE DE GRUPO
					//ACTUALIZAMOS EL ID DE GRUPO
					temp, _ := strconv.ParseInt(parametros[0], 10, 64)
					nuevoGrupo = int(temp) + 1
					//VERIFICAMOS SI EL NOMBRE COINCIDEN
					if strings.Compare(parametros[2], name) == 0 {
						existeGrupo = true
					}
				}
				nuevoArchivo += splitUG[i] + "\n"
			}
			if !existeGrupo {
				//AGREGAMOS EL NUEVO GRUPO
				line := fmt.Sprint(nuevoGrupo) + ",G," + name + "\n"
				//ENVIAMOS A MODIFICAR EL ARCHIVO users.txt
				ModificarArchivo("/users.txt", nuevoArchivo+line, file, avd, sb)
			} else {
				fmt.Println("El grupo ya existe")
			}
		} else {
			fmt.Println("La particion no esta montada")
		}
	} else {
		fmt.Println("No estas logeado como root")
	}
}

func FuncionRMGRP(params []string) {
	//DECLARACION DE PARAMETROS OBLIGATORIOS
	var id string
	var name string
	//LOOP DE RECOLECCION DE DATOS
	for i := 1; i < len(params); i++ {
		split := strings.Split(params[i], "->")
		switch strings.ToUpper(split[0]) {
		case "-ID":
			id = strings.Trim(split[1], "\n")
		case "-NAME":
			name = strings.Trim(split[1], "\n")
		case "":
		case " ":
			fmt.Print("")
		default:
			fmt.Println("No se reconoce el parametro ", strings.ToUpper(split[0]))
		}
	}
	disco, particion := ObtenerIndices(id)
	if strings.Contains(user, "root") {
		//PROCEDMOS A CREAR EL GRUPO
		part := Discos[disco].D_PARTS[particion]
		if part.STATUS == 1 {
			//EXISTE EL MONTAJE
			file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
			//OBTENEMOS EL SUPERBLOQUE
			sb := SB{}
			file.Seek(part.START, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
			//OBTENEMOS EL DIRECTORIO RAIZ
			avd := AVD{}
			file.Seek(sb.SBAP_AVD, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
			//OBTENEMOS LOS DATOS DEL ARCHIVO users.txt
			info := leerArchivo("/users.txt", file, avd, sb)
			splitUG := strings.Split(info, "\n")
			nuevoArchivo := ""
			existeGrupo := false
			for i := 0; i < len(splitUG); i++ {
				parametros := strings.Split(splitUG[i], ",")
				if len(parametros) == 1 {
					continue
				}
				if strings.Compare(parametros[1], "G") == 0 {
					//ESTA ES UNA LINEA DE DE GRUPO
					//VERIFICAMOS SI EL NOMBRE COINCIDEN
					if strings.Compare(parametros[2], name) == 0 {
						//EL NOMBRE COINCIDE ASI QUE NO ESCRIBIMOS EN EL ARCHIVO
						existeGrupo = true
					} else {
						//SI NO COINCIDEN CONSERVAMOS EL GRUPO
						nuevoArchivo += splitUG[i] + "\n"
					}
				} else {
					//CONSERVAMOS LOS USUARIOS
					nuevoArchivo += splitUG[i] + "\n"
				}
			}
			if existeGrupo {
				//SE ENCONTRO EL GRUPO POR LO QUE MANDAMOS A MODIFICAR EL ARCHIVO
				ModificarArchivo("/users.txt", nuevoArchivo, file, avd, sb)
			} else {
				fmt.Println("No se encontro el grupo especificado")
			}
		} else {
			fmt.Println("La particion no esta montada")
		}
	} else {
		fmt.Println("No estas logeado como root")
	}
}

func FuncionMKUSR(params []string) {
	//VARIABLES OBLIGATORIAS
	var usr string
	var pwd string
	var id string
	var grp string
	//LOOP DE RECOLECCION DE DATOS
	for i := 1; i < len(params); i++ {
		split := strings.Split(params[i], "->")
		switch strings.ToUpper(split[0]) {
		case "-ID":
			id = strings.Trim(split[1], "\n")
		case "-USR":
			usr = strings.Trim(split[1], "\n")
		case "-PWD":
			pwd = strings.Trim(split[1], "\n")
		case "-GRP":
			grp = strings.Trim(split[1], "\n")
		case "":
		case " ":
			fmt.Print("")
		default:
			fmt.Println("No se reconoce el parametro ", strings.ToUpper(split[0]))
		}
	}
	disco, particion := ObtenerIndices(id)
	if strings.Contains(user, "root") {
		//PROCEDMOS A CREAR EL GRUPO
		part := Discos[disco].D_PARTS[particion]
		if part.STATUS == 1 {
			//EXISTE EL MONTAJE
			file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
			//OBTENEMOS EL SUPERBLOQUE
			sb := SB{}
			file.Seek(part.START, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
			//OBTENEMOS EL DIRECTORIO RAIZ
			avd := AVD{}
			file.Seek(sb.SBAP_AVD, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
			//OBTENEMOS LOS DATOS DEL ARCHIVO users.txt
			info := leerArchivo("/users.txt", file, avd, sb)
			grupoExiste, userExiste := false, false
			noGrupo := -1
			splitUG := strings.Split(info, "\n")
			for i := 0; i < len(splitUG); i++ {
				parametros := strings.Split(splitUG[i], ",")
				if len(parametros) == 1 {
					continue
				}
				if strings.Compare(parametros[1], "G") == 0 {
					//GRUPO
					if strings.Compare(parametros[2], grp) == 0 {
						temp, _ := strconv.ParseInt(parametros[0], 10, 64) //asignamos el id
						grupoExiste = true                                 //el grupo existe
						noGrupo = int(temp)
					}
				} else {
					//USUARIO
					if strings.Compare(parametros[3], usr) == 0 {
						userExiste = true //el usuario existe
					}
				}
			}

			if grupoExiste {
				if !userExiste {
					nuevaLinea := fmt.Sprintf("%d,U,%s,%s,%s\n", noGrupo, grp, usr, pwd)
					info += nuevaLinea
					ModificarArchivo("/users.txt", info, file, avd, sb)
				} else {
					fmt.Println("Ya existe este usuario")
				}
			} else {
				fmt.Println("El grupo no existe")
			}
		} else {
			fmt.Println("La particion no esta montada")
		}
	} else {
		fmt.Println("No estas logeado como root")
	}
}

func FuncionRMUSR(params []string) {
	//DECLARACION DE PARAMETROS OBLIGATORIOS
	var id string
	var usr string
	//LOOP DE RECOLECCION DE DATOS
	for i := 1; i < len(params); i++ {
		split := strings.Split(params[i], "->")
		switch strings.ToUpper(split[0]) {
		case "-ID":
			id = strings.Trim(split[1], "\n")
		case "-USR":
			usr = strings.Trim(split[1], "\n")
		case "":
		case " ":
			fmt.Print("")
		default:
			fmt.Println("No se reconoce el parametro ", strings.ToUpper(split[0]))
		}
	}
	disco, particion := ObtenerIndices(id)
	if strings.Contains(user, "root") {
		//PROCEDMOS A CREAR EL GRUPO
		part := Discos[disco].D_PARTS[particion]
		if part.STATUS == 1 {
			//EXISTE EL MONTAJE
			file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
			//OBTENEMOS EL SUPERBLOQUE
			sb := SB{}
			file.Seek(part.START, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
			//OBTENEMOS EL DIRECTORIO RAIZ
			avd := AVD{}
			file.Seek(sb.SBAP_AVD, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
			//OBTENEMOS LOS DATOS DEL ARCHIVO users.txt
			info := leerArchivo("/users.txt", file, avd, sb)
			splitUG := strings.Split(info, "\n")
			nuevoArchivo := ""
			existeUsuario := false
			for i := 0; i < len(splitUG); i++ {
				parametros := strings.Split(splitUG[i], ",")
				if len(parametros) == 1 {
					continue
				}
				if strings.Compare(parametros[1], "G") == 0 {
					//LOS GRUPOS SIEMPRE LOS CONSERVAMOS
					nuevoArchivo += splitUG[i] + "\n"

				} else {
					if strings.Compare(parametros[2], usr) == 0 {
						//EL NOMBRE COINCIDE ASI QUE NO ESCRBIMOS EN EL ARCHIVO
						existeUsuario = true
					} else {
						//SI NO COINCIDEN CONSERVAMOS AL USUARIO
						nuevoArchivo += splitUG[i] + "\n"
					}

				}
			}
			if existeUsuario {
				//SE ENCONTRO EL GRUPO POR LO QUE MANDAMOS A MODIFICAR EL ARCHIVO
				ModificarArchivo("/users.txt", nuevoArchivo, file, avd, sb)
			} else {
				fmt.Println("No se encontro el grupo especificado")
			}
		} else {
			fmt.Println("La particion no esta montada")
		}
	} else {
		fmt.Println("No estas logeado como root")
	}
}

func FuncionCHMOD(params []string) {

}

func FuncionMKFILE(params []string) {
	//VARIABLES OBLIGATORIA
	var id string
	var path string
	//VARIABLES OPCIONALES
	var p bool = false
	var size int64
	var cont string = ""
	//LOOP QUE RECOLECTA LOS PARAMETROS
	for i := 1; i < len(params); i++ {
		split := strings.Split(params[i], "->")
		switch strings.ToUpper(split[0]) {
		case "-ID":
			id = strings.Trim(split[1], "\n")
		case "-PATH":
			path = strings.Trim(split[1], "\n")
		case "-P":
			p = true
		case "-SIZE":
			size, _ = strconv.ParseInt(strings.Trim(split[1], "\n"), 10, 64)
		case "-CONT":
			cont = strings.Trim(split[1], "\n")
		case "":
		case " ":
			fmt.Print("")
		default:
			fmt.Println("No se reconoce el parametro " + strings.ToUpper(split[0]))
			return
		}
	}
	disco, particion := ObtenerIndices(id)                  //Obtenemos info de la particion
	path = strings.TrimRight(strings.Trim(path, "\""), "/") //Limpiamos la ruta de comillas si tuviera y del ultimo slash
	cont = strings.Trim(cont, "\"")                         //Limpiamos el contenido de comillas
	part := Discos[disco].D_PARTS[particion]                //Obtenemos la particion
	if part.STATUS == 1 {
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
		//OBTENEMOS EL SUPERBLOQUE
		sb := SB{}
		file.Seek(part.START, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		//OBTENEMOS EL DIRECTORIO RAIZ
		avd := AVD{}
		file.Seek(sb.SBAP_AVD, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
		//ESCLARECEMOS LOS DATOS A ENVIAR
		datos := ""
		if len(cont) < int(size) {
			//EL CONTENIDO ES MENOR, ADJUNTAMOS EL CONTENIDO Y RELLENAMOS
			datos = cont
			aux := 65
			for i := len(cont); i < int(size); i++ {
				datos += fmt.Sprintf("%c", aux)
				if aux == 90 {
					aux = 65
				} else {
					aux++
				}
			}
		} else {
			if size > 0 {
				aux := 65
				for i := 0; i < int(size); i++ {
					datos += fmt.Sprintf("%c", aux)
					if aux == 90 {
						aux = 65
					} else {
						aux++
					}
				}
			} else {
				fmt.Println("No hay tamaño para asignarle al archivo")
			}
		}
		fmt.Println("Fichero Creado")
		crearFichero(path, 664, datos, file, avd, p, sb)
		//HACEMOS LA BITACORA
		log := Bitacora{}
		log.Log_Operacion = 1
		log.Log_Tipo = 0
		copy(log.Log_Fecha[:], time.Now().String())
		copy(log.Log_Nombre[:], path)
		copy(log.Log_Contenido[:], datos)
		crearBitacora(log, file, sb)
	} else {
		fmt.Println("La particion no esta montada")
		return
	}
}

func FuncionCAT(params []string) {
	//VARIABLES
	var id string
	var idindex = 0
	//LOOP QUE RECOGE DATOS
	for i := 1; i < len(params); i++ {
		split := strings.Split(params[i], "->")
		switch strings.ToUpper(split[0]) {
		case "-ID":
			idindex = i
			id = strings.Trim(split[1], "\n")
		}
	}
	disco, particion := ObtenerIndices(id)
	if Discos[disco].D_PARTS[particion].STATUS == 1 {
		//LOOP POR CADA PARAMETRO
		for i := 1; i < len(params); i++ {
			if i == idindex {
				continue
			} else {
				//AQUI RECIBIREMOS UN PARAMETRO RUTA
				split := strings.Split(params[i], "->")
				if len(split) == 1 {
					continue
				}
				ruta := strings.TrimRight(strings.Trim(strings.Trim(split[1], "\n"), "\""), "/") //ELIMINAMOS CUALQUIER SALTO DE LINEA O COMILLAS QUE PUEDA TENER LA RUTA MAS EL ULTIMO SLASH
				//OBTENEMOS EL ARCHIVO
				file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
				//OBTENEMOS EL SUPERBLOQUE
				sb := SB{}
				file.Seek(Discos[disco].D_PARTS[particion].START, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
				//OBTENEMOS EL DIRECTORIO RAIZ
				avd := AVD{}
				file.Seek(sb.SBAP_AVD, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				fmt.Println(leerArchivo(ruta, file, avd, sb))
			}
		}
	} else {
		fmt.Println("La particion no esta montada")
	}
}

func FuncionRM(params []string) {
	//VARIABLES OBLIGATORIAS
	var id string
	var path string
	//VARIABLES OPCIONALES
	var p bool = false
	//LOOP QUE RECOLECTA PARAMETROS
	for i := 1; i < len(params); i++ {
		split := strings.Split(params[i], "->")
		switch strings.ToUpper(split[0]) {
		case "-ID":
			id = strings.Trim(split[1], "\n")
		case "-PATH":
			path = strings.Trim(split[1], "\n")
		case "-RF":
			p = true
		case "":
		case " ":
			fmt.Print("")
		default:
			fmt.Println("No se reconoce el parametro " + strings.ToUpper(split[0]))
			return
		}
	}
	disco, particion := ObtenerIndices(id)
	if Discos[disco].D_PARTS[particion].STATUS == 1 {
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
		//OBTENEMOS EL SUPERBLOQUE
		sb := SB{}
		file.Seek(Discos[disco].D_PARTS[particion].START, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		//OBTENEMOS EL DIRECTORIO RAIZ
		avd := AVD{}
		file.Seek(sb.SBAP_AVD, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
		//LIMPIAMOS EL PATH DE COMILLAS SI TUVIERA Y EL ULTIMO SLASH
		path = strings.TrimRight(strings.Trim(path, "\""), "/")
		if !EliminarArchivo(path, file, avd, sb) { //SI NO SE ELIMINO EL ARCHIVO
			dire := EliminarCarpeta(path, p, file, avd, sb) //SE BUSCA ELIMINAR LA CARPETA
			if dire {
				fmt.Println("Se elimino la carpeta")
				//CREAMOS BITACORA
				log := Bitacora{}
				log.Log_Operacion = 2
				log.Log_Tipo = 1
				copy(log.Log_Fecha[:], time.Now().String())
				copy(log.Log_Nombre[:], path)
				crearBitacora(log, file, sb)

			}

		} else {
			fmt.Println("Se elimino el archivo")
			//CREAMOS BITACORA
			log := Bitacora{}
			log.Log_Operacion = 2
			log.Log_Tipo = 0
			copy(log.Log_Fecha[:], time.Now().String())
			copy(log.Log_Nombre[:], path)
			crearBitacora(log, file, sb)
		}
	} else {
		fmt.Println("La particion no esta montada")
	}
}

func FuncionEDIT(params []string) {
	//VARIABLES OBLIGATORIAS
	//VARIABLES OBLIGATORIA
	var id string
	var path string
	//VARIABLES OPCIONALES
	var size int64
	var cont string = ""
	//LOOP QUE RECOLECTA LOS PARAMETROS
	for i := 1; i < len(params); i++ {
		split := strings.Split(params[i], "->")
		switch strings.ToUpper(split[0]) {
		case "-ID":
			id = strings.Trim(split[1], "\n")
		case "-PATH":
			path = strings.Trim(split[1], "\n")
		case "-SIZE":
			size, _ = strconv.ParseInt(strings.Trim(split[1], "\n"), 10, 64)
		case "-CONT":
			cont = strings.Trim(split[1], "\n")
		case "":
		case " ":
			fmt.Print("")
		default:
			fmt.Println("No se reconoce el parametro " + strings.ToUpper(split[0]))
			return
		}
	}
	disco, particion := ObtenerIndices(id)                  //Obtenemos info de la particion
	path = strings.TrimRight(strings.Trim(path, "\""), "/") //Limpiamos la ruta de comillas si tuviera y ultimo slash
	cont = strings.Trim(cont, "\"")                         //Limpiamos el contenido de comillas
	part := Discos[disco].D_PARTS[particion]                //Obtenemos la particion
	if part.STATUS == 1 {
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
		//OBTENEMOS EL SUPERBLOQUE
		sb := SB{}
		file.Seek(part.START, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		//OBTENEMOS EL DIRECTORIO RAIZ
		avd := AVD{}
		file.Seek(sb.SBAP_AVD, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
		//ESCLARECEMOS LOS DATOS A ENVIAR
		datos := ""
		if len(cont) < int(size) {
			//EL CONTENIDO ES MENOR, ADJUNTAMOS EL CONTENIDO Y RELLENAMOS
			datos = cont
			aux := 65
			for i := len(cont); i < int(size); i++ {
				datos += fmt.Sprintf("%c", aux)
				if aux == 90 {
					aux = 65
				} else {
					aux++
				}
			}
		} else {
			if size > 0 {
				aux := 65
				for i := 0; i < int(size); i++ {
					datos += fmt.Sprintf("%c", aux)
					if aux == 90 {
						aux = 65
					} else {
						aux++
					}
				}
			} else {
				fmt.Println("No hay tamaño para asignarle al archivo")
			}
		}
		ModificarArchivo(path, datos, file, avd, sb)
		//CREAR BITACORA
		log := Bitacora{}
		log.Log_Operacion = 3
		log.Log_Tipo = 0
		copy(log.Log_Fecha[:], time.Now().String())
		copy(log.Log_Nombre[:], path)
		copy(log.Log_Contenido[:], datos)
		crearBitacora(log, file, sb)
	} else {
		fmt.Println("La particion no esta montada")
		return
	}

}

func FuncionREN(params []string) {
	//DECLARACION DE VARIABLES OBLIGATORIAS
	var id string
	var name string
	var path string
	//LOOP QUE RECOLECTA LOS PARAMETROS
	for i := 1; i < len(params); i++ {
		split := strings.Split(params[i], "->")
		switch strings.ToUpper(split[0]) {
		case "-ID":
			id = strings.Trim(split[1], "\n")
		case "-NAME":
			name = strings.Trim(split[1], "\n")
		case "-PATH":
			path = strings.Trim(split[1], "\n")
		case "":
		case " ":
			fmt.Println("")
		default:
			fmt.Println("No se reconoce el parametro ", strings.ToUpper(split[0]))
		}
	}

	disco, particion := ObtenerIndices(id)
	if Discos[disco].D_PARTS[particion].STATUS == 1 {
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
		//OBTENEMOS EL SUPERBLOQUE
		sb := SB{}
		file.Seek(Discos[disco].D_PARTS[particion].START, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		//OBTENEMOS EL DIRECTORIO RAIZ
		avd := AVD{}
		file.Seek(sb.SBAP_AVD, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
		//LIMPIAMOS EL PATH DE COMILLAS SI TUVIERA
		path = strings.TrimRight(strings.Trim(path, "\""), "/")
		if !RenombrarArchivo(path, name, file, avd, sb) { //NO ERA UN ARCHIVO
			if RenombrarCarpeta(path, name, file, avd, sb) {
				//CREAMOS UNA BITACORA
				log := Bitacora{}
				log.Log_Operacion = 4
				log.Log_Tipo = 1
				copy(log.Log_Fecha[:], time.Now().String())
				copy(log.Log_Nombre[:], path)
				copy(log.Log_Contenido[:], name)
				crearBitacora(log, file, sb)
			}
		} else {
			//ERA UN ARCHIVO
			//CREAMOS UNA BITACORA
			log := Bitacora{}
			log.Log_Operacion = 4
			log.Log_Tipo = 0
			copy(log.Log_Fecha[:], time.Now().String())
			copy(log.Log_Nombre[:], path)
			copy(log.Log_Contenido[:], name)
			crearBitacora(log, file, sb)
		}
	} else {
		fmt.Println("La particion no esta montada")
	}
}

func FuncionMKDIR(params []string) {
	//DECLARACION DE VARIABLES OBLIGATORIAS
	var id string
	var path string
	var p bool = false
	//LOOP QUE RECOLECTA LOS PARAMETROS
	for i := 1; i < len(params); i++ {
		split := strings.Split(params[i], "->")
		switch strings.ToUpper(split[0]) {
		case "-ID":
			id = strings.Trim(split[1], "\n")
		case "-P":
			p = true
		case "-PATH":
			path = strings.Trim(split[1], "\n")
		case "":
		case " ":
			fmt.Println("")
		default:
			fmt.Println("No se reconoce el parametro ", strings.ToUpper(split[0]))
		}
	}

	disco, particion := ObtenerIndices(id)
	if Discos[disco].D_PARTS[particion].STATUS == 1 {
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
		//OBTENEMOS EL SUPERBLOQUE
		sb := SB{}
		file.Seek(Discos[disco].D_PARTS[particion].START, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		//OBTENEMOS EL DIRECTORIO RAIZ
		avd := AVD{}
		file.Seek(sb.SBAP_AVD, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
		//LIMPIAMOS EL PATH DE COMILLAS SI TUVIERA
		path = strings.TrimRight(strings.Trim(path, "\""), "/")
		crearCarpeta(path, 664, file, avd, p, sb)
		//CREAMOS UNA BITACORA
		//CREAMOS UNA BITACORA
		log := Bitacora{}
		log.Log_Operacion = 5
		log.Log_Tipo = 1
		copy(log.Log_Fecha[:], time.Now().String())
		copy(log.Log_Nombre[:], path)
		crearBitacora(log, file, sb)
	} else {
		fmt.Println("La particion no esta montada")
	}
}

func FuncionCP(params []string) {
	//DECLARACION DE VARIABLES OBLIGATORIAS
	var id string
	var path string
	var dest string
	//LOOP QUE RECOLECTA LOS PARAMETROS
	for i := 1; i < len(params); i++ {
		split := strings.Split(params[i], "->")
		switch strings.ToUpper(split[0]) {
		case "-ID":
			id = strings.Trim(split[1], "\n")
		case "-PATH":
			path = strings.Trim(split[1], "\n")
		case "-DEST":
			dest = strings.Trim(split[1], "\n")
		case "":
		case " ":
			fmt.Println("")
		default:
			fmt.Println("No se reconoce el parametro ", strings.ToUpper(split[0]))
		}
	}
	disco, particion := ObtenerIndices(id)
	if Discos[disco].D_PARTS[particion].STATUS == 1 {
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
		//OBTENEMOS EL SUPERBLOQUE
		sb := SB{}
		file.Seek(Discos[disco].D_PARTS[particion].START, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		//OBTENEMOS EL DIRECTORIO RAIZ
		avd := AVD{}
		file.Seek(sb.SBAP_AVD, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
		//LIMPIAMOS EL PATH DE COMILLAS SI TUVIERA
		path = strings.TrimRight(strings.Trim(path, "\""), "/")
		dest = strings.TrimRight(strings.Trim(dest, "\""), "/")
		directorioObjetivo := encontrarCarpeta(path, false, file, avd, sb)
		if directorioObjetivo != 0 {
			//ENTONCES TENGO QUE COPIAR EL DIRECTORIO
			directorioDestino := encontrarCarpeta(dest, false, file, avd, sb)
			if directorioDestino != 0 {
				//PROCEDEMOS
				avdO := AVD{} //ESTE ES EL QUE SE VA A COPIAR
				file.Seek(directorioObjetivo, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avdO)))), binary.BigEndian, &avdO)
				avdI := AVD{} //ESTE EL DESTINO DONDE VAMOS A COPIAR
				file.Seek(directorioDestino, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avdI)))), binary.BigEndian, &avdI)
				for {
					for i := 0; i < len(avdI.AVD_Sub_Directorios); i++ {
						if avdI.AVD_Sub_Directorios[i] == 0 {
							//fmt.Println(avdO, "Este se va a copiar")
							avdI.AVD_Sub_Directorios[i] = CopiaAVDRecursiva(avdO, file, sb)
							fmt.Println("Directorio copiado")
							//REESCRIBIMOS EL AVD
							file.Seek(avdI.AVD_Index, 0)
							var redactor bytes.Buffer
							binary.Write(&redactor, binary.BigEndian, &avdI)
							escribirBytes(file, redactor.Bytes())
							//CREAMOS UNA BITACORA
							log := Bitacora{}
							log.Log_Operacion = 6
							log.Log_Tipo = 1
							copy(log.Log_Fecha[:], time.Now().String())
							copy(log.Log_Nombre[:], path)
							copy(log.Log_Contenido[:], dest)
							crearBitacora(log, file, sb)
							return
						}
					}
					//SE ASUME QUE EN EL AMBITO DE AVDI NO SE ENCONTRO ESPACIO LIBRE
					if avdI.AVDAP == 0 {
						//NO HAY DIRECTORIO AUXILIAR, CREAMOS UNO
						avdI.AVDAP = crearDirectorio(avdI.AVD_Name, avdI.AVD_Proper, file, sb)
						file.Seek(avdI.AVDAP, 0)
						binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avdI)))), binary.BigEndian, &avdI)
						//REESCRIBIMOS EL AVDI
						file.Seek(avdI.AVD_Index, 0)
						var redactor bytes.Buffer
						binary.Write(&redactor, binary.BigEndian, &avdI)
						escribirBytes(file, redactor.Bytes())
					} else {
						file.Seek(avdI.AVDAP, 0)
						binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avdI)))), binary.BigEndian, &avdI)
					}
				}
			}
		} else {
			//TENGO QUE COPIAR EL ARCHIVO
			info := leerArchivo(path, file, avd, sb) //obtenemos la informacion que queremos copiar
			splitPath := strings.Split(path, "/")
			copyFile := dest + "/" + splitPath[len(splitPath)-1]
			crearFichero(copyFile, 664, info, file, avd, true, sb) //mandamos a crear otro fichero a dest cone
			fmt.Println("Hemos copiado el fichero")
			//CREAMOS UNA BITACORA
			log := Bitacora{}
			log.Log_Operacion = 6
			log.Log_Tipo = 0
			copy(log.Log_Fecha[:], time.Now().String())
			copy(log.Log_Nombre[:], path)
			copy(log.Log_Contenido[:], dest)
			crearBitacora(log, file, sb)
			return
		}
	} else {
		fmt.Println("La particion no esta montada")
	}
}

func CopiaAVDRecursiva(copia AVD, file *os.File, sb SB) int64 {
	//INTANCIAMOS UN AVD NUEVO
	avd := AVD{}
	//CREAMOS NUESTRO AVD COPIA
	avd.AVD_Index = crearDirectorio(copia.AVD_Name, copia.AVD_Proper, file, sb)
	//COPIAMOS TODAS LAS PROPIEDADES DEL AVD
	avd.AVD_Name = copia.AVD_Name
	avd.AVD_Proper = copia.AVD_Proper
	copy(avd.AVD_Create[:], time.Now().String())
	//COPIAMOS LOS SUBDIRECOTORIOS
	for i := 0; i < len(copia.AVD_Sub_Directorios); i++ {
		if copia.AVD_Sub_Directorios[i] != 0 {
			//INTANCIAMOS UN AUXILIAR DE COPIA
			auxiliar := AVD{}
			file.Seek(copia.AVD_Sub_Directorios[i], 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(auxiliar)))), binary.BigEndian, &auxiliar)
			avd.AVD_Sub_Directorios[i] = CopiaAVDRecursiva(auxiliar, file, sb)
		}
	}
	//COPIAMOS EL DIRECTORIO AUXILIAR
	if copia.AVDAP != 0 {
		//INSTANCIAMOS UN AUXILIAR DE COPIA
		auxiliar := AVD{}
		file.Seek(copia.AVDAP, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(auxiliar)))), binary.BigEndian, &auxiliar)
		avd.AVDAP = CopiaAVDRecursiva(auxiliar, file, sb)
	}
	//HACEMOS COPIAS DEL DD
	if copia.AVD_DD != 0 {
		//INTANCIAMOS UN DD AUXILIAR
		auxiliar := DD{}
		file.Seek(copia.AVD_DD, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(auxiliar)))), binary.BigEndian, &auxiliar)
		avd.AVD_DD = CopiaDDRecursiva(auxiliar, file, sb)
	}
	//REECRIBIMOS NUESTRO AVD COPIA
	file.Seek(avd.AVD_Index, 0)
	var redactor bytes.Buffer
	binary.Write(&redactor, binary.BigEndian, &avd)
	escribirBytes(file, redactor.Bytes())
	//RETORNAMOS EL INDICE DE LA COPIA
	return avd.AVD_Index
}

func CopiaDDRecursiva(copia DD, file *os.File, sb SB) int64 {
	//INTANCIAMOS UN DD
	dd := DD{}
	//CREAMOS NUESTRO DE COPIA
	dd.DD_Index = crearDetalleDirectorio(file, sb)
	//COPIAMOS LOS ARCHIVOS
	for i := 0; i < len(copia.Files); i++ {
		if copia.Files[i].FIle_Inodo != 0 {
			//POR CADA ARCHIVO QUE HAY EN DD A COPIAR VAMOS A LEER LOS BLOQUES DE SU INODO
			datos := "" //vamos a construir una cadena con los bytes que vayamos a leer
			inodo := INodo{}
			file.Seek(copia.Files[i].FIle_Inodo, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(inodo)))), binary.BigEndian, &inodo)
			for {
				//ITERAMOS SOBRE LA CANTIDAD DE BLOQUES QUE TENGAMOS EN ESTE INODO
				for j := 0; j < int(inodo.ICount_Bloque); j++ {
					bloque := Bloque{}
					file.Seek(inodo.IArray_Bloque[j], 0)
					binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(bloque)))), binary.BigEndian, &bloque)
					for k := 0; k < len(bloque.DB_Data); k++ {
						datos += string(bloque.DB_Data[k])
					}
				}
				//VERIFICAMOS SI EXISTE UN INODO AUXILIAR, SI NO HAY SE TERMINA EL PROCESO
				if inodo.IAP == 0 {
					break
				} else {
					file.Seek(inodo.IAP, 0)
					binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(inodo)))), binary.BigEndian, &inodo)
				}
			}
			//HACEMOS UN ARCHIVO NUEVO CON LOS DATOS OBTENIDOS
			dd.Files[i].FIle_Inodo = crearINodo(datos, file, sb)
			dd.Files[i].File_Name = copia.Files[i].File_Name
			copy(dd.Files[i].File_Create[:], time.Now().String())
			copy(dd.Files[i].File_Modify[:], time.Now().String())
		}
	}
	//COPIAMOS EL DD AUXILIAR
	if copia.DDAP != 0 {
		auxiliar := DD{}
		file.Seek(copia.DDAP, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(auxiliar)))), binary.BigEndian, &auxiliar)
		dd.DDAP = CopiaDDRecursiva(auxiliar, file, sb)
	}
	//REESCRIBIMOS NUESTRO DD
	file.Seek(dd.DD_Index, 0)
	var redactor bytes.Buffer
	binary.Write(&redactor, binary.BigEndian, &dd)
	escribirBytes(file, redactor.Bytes())
	//RETORNAMOS EL DD
	return dd.DD_Index
}

func FuncionMV(params []string) {
	//DECLARACION DE VARIABLES OBLIGATORIAS
	var id string
	var path string
	var dest string
	//LOOP QUE RECOLECTA LOS PARAMETROS
	for i := 1; i < len(params); i++ {
		split := strings.Split(params[i], "->")
		switch strings.ToUpper(split[0]) {
		case "-ID":
			id = strings.Trim(split[1], "\n")
		case "-PATH":
			path = strings.Trim(split[1], "\n")
		case "-DEST":
			dest = strings.Trim(split[1], "\n")
		case "":
		case " ":
			fmt.Println("")
		default:
			fmt.Println("No se reconoce el parametro ", strings.ToUpper(split[0]))
		}
	}
	disco, particion := ObtenerIndices(id)
	if Discos[disco].D_PARTS[particion].STATUS == 1 {
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR, 0755)
		//OBTENEMOS EL SUPERBLOQUE
		sb := SB{}
		file.Seek(Discos[disco].D_PARTS[particion].START, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		//OBTENEMOS EL DIRECTORIO RAIZ
		avd := AVD{}
		file.Seek(sb.SBAP_AVD, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
		//LIMPIAMOS EL PATH DE COMILLAS SI TUVIERA
		path = strings.TrimRight(strings.Trim(path, "\""), "/")
		dest = strings.TrimRight(strings.Trim(dest, "\""), "/")
		directorioObjetivo := encontrarCarpeta(path, true, file, avd, sb)
		if directorioObjetivo != 0 {
			//ENTONCES TENGO QUE COPIAR EL DIRECTORIO
			directorioDestino := encontrarCarpeta(dest, false, file, avd, sb)
			if directorioDestino != 0 {
				//PENDIENTE MOVER DIRECTORIOS de path a dest
				destino := AVD{}
				file.Seek(directorioDestino, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(destino)))), binary.BigEndian, &destino)
				var seteado bool = false
				for {
					if seteado {
						break
					} else {
						//ANALIZAMOS EL AUXILIAR ACTUAL
						for i := 0; i < len(destino.AVD_Sub_Directorios); i++ {
							if destino.AVD_Sub_Directorios[i] == 0 {
								seteado = true
								destino.AVD_Sub_Directorios[i] = directorioObjetivo
								//REESCRIBIMOS EL AVD
								file.Seek(destino.AVD_Index, 0)
								var redactor bytes.Buffer
								binary.Write(&redactor, binary.BigEndian, &destino)
								escribirBytes(file, redactor.Bytes())
								fmt.Println("Carpeta movida")
								//CREAMOS UNA BITACORA
								log := Bitacora{}
								log.Log_Operacion = 7
								log.Log_Tipo = 1
								copy(log.Log_Fecha[:], time.Now().String())
								copy(log.Log_Nombre[:], path)
								copy(log.Log_Contenido[:], dest)
								crearBitacora(log, file, sb)
								return
							}
						}
						//ASUMIENDO QUE LOS SUBDIRECTORIOS ESTAN LLENOS
						if destino.AVDAP != 0 {
							//HAY UN AUXILIAR VAMOS A VER AQUI
							file.Seek(destino.AVDAP, 0)
							binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(destino)))), binary.BigEndian, &destino)
						} else {
							//CREAMOS UN DIRECOTRIO AUXILIAR
							destino.AVDAP = crearDirectorio(destino.AVD_Name, destino.AVD_Proper, file, sb)
							//REESCRIBIMOS EL AVD
							file.Seek(destino.AVD_Index, 0)
							var redactor bytes.Buffer
							binary.Write(&redactor, binary.BigEndian, &destino)
							escribirBytes(file, redactor.Bytes())
							//LEEMOS EL DIRECTORIO AUXILIAR
							file.Seek(destino.AVDAP, 0)
							binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(destino)))), binary.BigEndian, &destino)
						}
					}
				}
			}
		} else {
			//TENGO QUE MOVER EL ARCHIVO CONTENIDO EN Path
			info := leerArchivo(path, file, avd, sb) //obtenemos la informacion que queremos copiar
			EliminarArchivo(path, file, avd, sb)
			splitPath := strings.Split(path, "/")
			copyFile := dest + "/" + splitPath[len(splitPath)-1]
			crearFichero(copyFile, 664, info, file, avd, true, sb) //mandamos a crear otro fichero a dest cone
			fmt.Println("Hemos movido el fichero")
			//CREAMOS UNA BITACORA
			log := Bitacora{}
			log.Log_Operacion = 7
			log.Log_Tipo = 0
			copy(log.Log_Fecha[:], time.Now().String())
			copy(log.Log_Nombre[:], path)
			copy(log.Log_Contenido[:], dest)
			crearBitacora(log, file, sb)
			return
		}
	} else {
		fmt.Println("La particion no esta montada")
	}
}

func FuncionFIND(params []string) {

}

func FuncionCHOWN(params []string) {

}

func FuncionCHGRP(params []string) {

}

/*
	Estructuras de archivos del menos al mas significativo
*/

type Bitacora struct {
	Log_Operacion int64
	Log_Tipo      int64
	Log_Nombre    [100]byte
	Log_Contenido [100]byte
	Log_Fecha     [20]byte
}

type Bloque struct {
	DB_Data [25]byte
}

type INodo struct {
	ICount        int64
	ISize         int64
	ICount_Bloque int64
	IArray_Bloque [4]int64
	IAP           int64
}

type Archivo struct {
	File_Name   [20]byte
	FIle_Inodo  int64
	File_Create [20]byte
	File_Modify [20]byte
}

type DD struct {
	Files    [5]Archivo
	DDAP     int64
	DD_Index int64
}

type AVD struct {
	AVD_Create          [20]byte
	AVD_Name            [20]byte
	AVD_Sub_Directorios [6]int64
	AVD_DD              int64
	AVDAP               int64
	AVD_Proper          int64
	AVD_Index           int64
}

type SB struct {
	SB_Name_HD           [20]byte
	SB_AVD_Count         int64
	SB_DD_Count          int64
	SB_INodo_Count       int64
	SB_Bloque_Count      int64
	SB_AVD_Free          int64
	SB_DD_Free           int64
	SB_INodo_Free        int64
	SB_Bloque_Free       int64
	SB_Date_Create       [20]byte
	SB_Last_Date_Mount   [20]byte
	SB_Mount_Count       int64
	SBAP_BitMap_AVD      int64
	SBAP_AVD             int64
	SBAP_BitMap_DD       int64
	SBAP_DD              int64
	SBAP_BitMap_INodo    int64
	SBAP_INodo           int64
	SBAP_BitMap_Bloque   int64
	SBAP_Bloque          int64
	SBAP_Bitacora        int64
	SB_Bitacora_Count    int64
	SB_AVD_Size          int64
	SB_DD_Size           int64
	SB_INodo_Size        int64
	SB_Bloque_Size       int64
	SB_AVD_FreeBit       int64
	SB_DD_FreeBit        int64
	SB_INodo_FreeBit     int64
	SB_Bloque_FreeBit    int64
	SB_Master_Programmer int64
}

func crearDirectorio(nombre [20]byte, usrId int64, file *os.File, sb SB) int64 {
	//ACTUALIZAMOS NUESTRO SB
	file.Seek(sb.SBAP_BitMap_AVD-int64(unsafe.Sizeof(sb)), 0)
	binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
	//fmt.Println(sb)
	//CREAMOS UN OBJETO AVD
	avd := AVD{}
	copy(avd.AVD_Create[:], time.Now().String())
	avd.AVD_Name = nombre
	avd.AVD_Proper = usrId

	//OBTENEMOS EL PRIMER BIT LIBRE EN EL MAPA DE BITS
	bitReference := sb.SB_AVD_FreeBit
	//OBTENMOS EL INICIO DE NUESTRO AVD SEGUN EL BITMAP
	inicioAVD := sb.SBAP_AVD + bitReference*sb.SB_AVD_Size
	avd.AVD_Index = inicioAVD
	//ESCRIBIMOS EL AVD
	file.Seek(inicioAVD, 0)
	var redactor bytes.Buffer
	binary.Write(&redactor, binary.BigEndian, &avd)
	escribirBytes(file, redactor.Bytes())
	//REDUCIMOS LOS AVD LIBRES A UNO
	sb.SB_AVD_Free -= 1
	//PONER EN UN EL BIT DE REFERENCIA DEL BITMAP EL VALOR 1
	file.Seek(sb.SBAP_BitMap_AVD+bitReference, 0)
	var uno [1]byte
	uno[0] = 1
	var redactor2 bytes.Buffer
	binary.Write(&redactor2, binary.BigEndian, &uno)
	escribirBytes(file, redactor2.Bytes())
	//ACTUALIZAMOS EL PRIMER BIT DE LIBRE
	//file.Seek(sb.SBAP_BitMap_AVD, 0)
	for i := int64(0); i < sb.SB_AVD_Count; i++ {
		file.Seek(sb.SBAP_BitMap_AVD+i, 0)
		byteleidos := leerBytes(file, 1)
		if byteleidos[0] == 0 {
			//SI EL BITMAP RESULTA SER 0 ENTONCES ACTUALIZAMOS EL PRIMER BIT
			//fmt.Println("Nuevo bit Libre es: ", i)
			sb.SB_AVD_FreeBit = i
			break
		}
	}
	//REESCRIBIMOS EL SB
	file.Seek(sb.SBAP_BitMap_AVD-int64(unsafe.Sizeof(sb)), 0)
	var redactor3 bytes.Buffer
	binary.Write(&redactor3, binary.BigEndian, &sb)
	escribirBytes(file, redactor3.Bytes())
	//RETORNAMOS EL APUNTADOR DEL NUEVO DIRECTORIO CREADO
	return inicioAVD
}

func crearDetalleDirectorio(file *os.File, sb SB) int64 {
	//LEEMOS EL SB PARA TENERLO ACTUALIZADO
	file.Seek(sb.SBAP_BitMap_AVD-int64(unsafe.Sizeof(sb)), 0)
	binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
	//fmt.Println(sb)
	//REAMOS UN OBJETO DD
	dd := DD{}
	//fmt.Println("Creando Detalle Directorio")
	//OBTENEMOS EL PRIMER BIT LIBRE DEL MAPA DE BITS DE DD
	bitReference := sb.SB_DD_FreeBit
	//fmt.Println(bitReference)
	//OBTENEMOS EL INICIO DEL DETALLE DE DIRECTORIO SEGUN EL MAPA DE BITS
	inicioDD := sb.SBAP_DD + bitReference*sb.SB_DD_Size
	dd.DD_Index = inicioDD
	//ESCRIBIMOS EL DD
	file.Seek(inicioDD, 0)
	var redactor bytes.Buffer
	binary.Write(&redactor, binary.BigEndian, &dd)
	escribirBytes(file, redactor.Bytes())
	//REDUCIMOS DD LIBRES A UNO
	sb.SB_DD_Free -= 1
	//PONER EN EL BIT DE REFERENCIA DEL BITMAP EL VALOR 1
	file.Seek(sb.SBAP_BitMap_DD+bitReference, 0)
	var uno [1]byte
	uno[0] = 1
	var redactor2 bytes.Buffer
	binary.Write(&redactor2, binary.BigEndian, &uno)
	escribirBytes(file, redactor2.Bytes())
	//ACTUALIZAMOS EL PRIMER BIT DE LIBRE
	file.Seek(sb.SBAP_BitMap_DD, 0)
	for i := int64(0); i < sb.SB_DD_Count; i++ {
		file.Seek(sb.SBAP_BitMap_DD+i, 0)
		byteleidos := leerBytes(file, 1)
		if byteleidos[0] == 0 {
			//SI EL BITMAP RESULTA SER 0 ENTONCES ACTUALIZAMOS EL PRIMER BIT
			//fmt.Println("Nuevo bit Libre es: ", i)
			sb.SB_DD_FreeBit = i
			break
		}
	}
	//REESCRIBIMOS EL SB
	file.Seek(sb.SBAP_BitMap_AVD-int64(unsafe.Sizeof(sb)), 0)
	var redactor3 bytes.Buffer
	binary.Write(&redactor3, binary.BigEndian, &sb)
	escribirBytes(file, redactor3.Bytes())
	//RETORNAMOS EL APUNTADOR DEL NUEVO DETALLE DE DIRECTORIO CREADO
	return inicioDD
}

func crearINodo(contenido string, file *os.File, sb SB) int64 {
	//LEEMOS EL SUPERBLOQUE PARA TENER LA INFO ACTUALIZADA
	file.Seek(sb.SBAP_BitMap_AVD-int64(unsafe.Sizeof(sb)), 0)
	binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
	//OBTENEMOS EL PRIMER BIT LIBRE DEL MAPA DE BITS DEL INODO
	bitReference := sb.SB_INodo_FreeBit
	//OBTENEMOS EL INICIO FISICO DEL INODO SEGUN EL MAPA DE BITS
	inicioINODO := sb.SBAP_INodo + bitReference*sb.SB_INodo_Size
	//REDUCIMOS LOS INODOS A UNO
	sb.SB_INodo_Free -= 1
	//PONER EN EL BIT DE REFERENCIA DEL BITMAP EL VALOR 1
	file.Seek(sb.SBAP_BitMap_INodo+bitReference, 0)
	var uno [1]byte
	uno[0] = 1
	var redactor2 bytes.Buffer
	binary.Write(&redactor2, binary.BigEndian, &uno)
	escribirBytes(file, redactor2.Bytes())
	//ACTUALIZAMOS EL PRIMER BIT DE LIBRE
	file.Seek(sb.SBAP_BitMap_INodo, 0)
	for i := int64(0); i < sb.SB_INodo_Count; i++ {
		file.Seek(sb.SBAP_BitMap_INodo+i, 0)
		byteleidos := leerBytes(file, 1)
		if byteleidos[0] == 0 {
			//SI EL BITMAP RESULTA SER 0 ENTONCES ACTUALIZAMOS EL PRIMER BIT
			//fmt.Println("Nuevo bit Libre es: ", i)
			sb.SB_INodo_FreeBit = i
			break
		}
	}
	//REESCRIBIMOS EL SB
	file.Seek(sb.SBAP_BitMap_AVD-int64(unsafe.Sizeof(sb)), 0)
	var redactor3 bytes.Buffer
	binary.Write(&redactor3, binary.BigEndian, &sb)
	escribirBytes(file, redactor3.Bytes())
	//INSTANCIAMOS UN OBJETO INODO NUEVO
	inodo := INodo{}
	cantidadBloques := math.Ceil(float64(len(contenido)) / 25.0)
	inodo.ICount_Bloque = int64(cantidadBloques)
	inodo.ISize = int64(len(contenido))
	inodo.ICount = 1
	if inodo.ICount_Bloque > 4 {
		//OCUPAREMOS OTRO INODO POR LO QUE CONSTRUIMOS OTRO STRING DE 100 PARA LO QUE TENGA
		temp := ""
		for i := 100; i < len(contenido); i++ {
			temp += string(contenido[i])
		}
		inodo.IAP = crearINodo(temp, file, sb)
		//ACTUALIZAMOS LOS DATOS DE ESTE INODO
		inodo.ICount_Bloque = 4
		//METEMOS LOS BLOQUES QUE SI PODEMOS METER
		//EL ARCHIVO CABE EN ESTE INODO
		for i := 0; i < len(inodo.IArray_Bloque); i++ {
			var datos [25]byte
			for j := 0; j < len(datos); j++ {
				datos[j] = contenido[j+i*25]
			}
			inodo.IArray_Bloque[i] = crearBloque(datos, file, sb)
		}
	} else {
		for i := 0; i < int(inodo.ICount_Bloque); i++ {
			var datos [25]byte
			for j := 0; j < len(datos); j++ {
				indice := j + i*25
				if indice < len(contenido) {
					datos[j] = contenido[indice]
				}
			}
			inodo.IArray_Bloque[i] = crearBloque(datos, file, sb)
		}
	}

	//ESCRIBIMOS EL INODO
	file.Seek(inicioINODO, 0)
	var redactor bytes.Buffer
	binary.Write(&redactor, binary.BigEndian, &inodo)
	escribirBytes(file, redactor.Bytes())

	//RETORNAMOS EL APUNTADOR DE BLOQUE
	return inicioINODO
}

func crearBloque(data [25]byte, file *os.File, sb SB) int64 {
	//LEEMOS EL SUPERBLOQUE PARA TENER INFO ACTUALIZADA
	file.Seek(sb.SBAP_BitMap_AVD-int64(unsafe.Sizeof(sb)), 0)
	binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
	//fmt.Println(sb)
	//INSTACIAMOS UN OBJETO BLOQUE
	b := Bloque{}
	b.DB_Data = data
	//OBTENEMOS EL PRIMER BIT LIBRE DEL MAPA DE BITS DEL BLOQUE
	bitReference := sb.SB_Bloque_FreeBit
	//OBTENEMOS EL INICIO FISICO DEL BLOQEU SEGUN EL MAPA DE BITS
	inicioBloque := sb.SBAP_Bloque + bitReference*sb.SB_Bloque_Size
	//ESCRIBIMOS EL BLOQUE
	file.Seek(inicioBloque, 0)
	var redactor bytes.Buffer
	binary.Write(&redactor, binary.BigEndian, &b)
	escribirBytes(file, redactor.Bytes())
	//REDUCIMOS LOS BLOQUES LIBRES A UNO
	sb.SB_Bloque_Free -= 1
	//PONER EN EL BIT DE REFERENCIA DEL BITMAP EL VALOR 1
	file.Seek(sb.SBAP_BitMap_Bloque+bitReference, 0)
	var uno [1]byte
	uno[0] = 1
	var redactor2 bytes.Buffer
	binary.Write(&redactor2, binary.BigEndian, &uno)
	escribirBytes(file, redactor2.Bytes())
	//ACTUALIZAMOS EL PRIMER BIT DE LIBRE
	for i := int64(0); i < sb.SB_Bloque_Count; i++ {
		file.Seek(sb.SBAP_BitMap_Bloque+i, 0)
		byteleidos := leerBytes(file, 1)
		if byteleidos[0] == 0 {
			//SI EL BITMAP RESULTA SER 0 ENTONCES ACTUALIZAMOS EL PRIMER BIT
			//fmt.Println("Nuevo bit Libre es: ", i)
			sb.SB_Bloque_FreeBit = i
			break
		}
	}
	//REESCRIBIMOS EL SB
	file.Seek(sb.SBAP_BitMap_AVD-int64(unsafe.Sizeof(sb)), 0)
	var redactor3 bytes.Buffer
	binary.Write(&redactor3, binary.BigEndian, &sb)
	escribirBytes(file, redactor3.Bytes())
	//RETORNAMOS EL APUNTADOR DE BLOQUE
	return inicioBloque
}

func crearBitacora(data Bitacora, file *os.File, sb SB) {
	//LEEMOS EL SUPERBLOQUE PARA TENER INFO ACTUALIZADA
	file.Seek(sb.SBAP_BitMap_AVD-int64(unsafe.Sizeof(sb)), 0)
	binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
	//INSTANCIAMOS UN BLOQUE VACIO
	b := Bitacora{}
	b = data
	//OBTENEMOS EL PRIMER BIT LIBRE DEL MAPA DE BITS
	bitReference := sb.SB_Bitacora_Count
	//OBTENEMOS DONDE INICIA EL NUEVO REGISTRO
	inicio := sb.SBAP_Bitacora + bitReference*int64(unsafe.Sizeof(data))
	//ESCRIBIMOS LA BITACORA
	file.Seek(inicio, 0)
	var redactor bytes.Buffer
	binary.Write(&redactor, binary.BigEndian, &b)
	escribirBytes(file, redactor.Bytes())
	//AUMENTAMOS EN UNO NUESTRA BITACORA
	sb.SB_Bitacora_Count += 1
	//REESCRIBIMOS EL SB
	file.Seek(sb.SBAP_BitMap_AVD-int64(unsafe.Sizeof(sb)), 0)
	var redactor3 bytes.Buffer
	binary.Write(&redactor3, binary.BigEndian, &sb)
	escribirBytes(file, redactor3.Bytes())
	return
}

func eliminarINodo(apuntador int64, file *os.File, sb SB) {
	//RELEEMOS EL SUPER BLOQUE PARA TENER LA INFO ACTUALIZADA
	file.Seek(sb.SBAP_BitMap_AVD-int64(unsafe.Sizeof(sb)), 0)
	binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
	//AUMENTAMOS EN 1 EL CONTADOR DE ESTRUCTURAS LIBRES
	sb.SB_INodo_Free += 1
	//SETEAMOS 0 EN EL BITMAP
	bitReference := (apuntador - sb.SBAP_INodo) / sb.SB_INodo_Size
	file.Seek(sb.SBAP_BitMap_INodo+bitReference, 0)
	var cero [1]byte
	cero[0] = 0
	var redactor2 bytes.Buffer
	binary.Write(&redactor2, binary.BigEndian, &cero)
	escribirBytes(file, redactor2.Bytes())
	//REESCRIBIMOS EL SB
	file.Seek(sb.SBAP_BitMap_AVD-int64(unsafe.Sizeof(sb)), 0)
	var redactor3 bytes.Buffer
	binary.Write(&redactor3, binary.BigEndian, &sb)
	escribirBytes(file, redactor3.Bytes())

	//INSTANCIAMOS EL INODO QUE SE MANDO A ELIMINAR
	inodo := INodo{}
	file.Seek(apuntador, 0)
	binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(inodo)))), binary.BigEndian, &inodo)
	//INSTANCIAMOS UN INODO VACIO
	vacio := INodo{}
	//ELIMINAMOS EL ESPACIO
	file.Seek(apuntador, 0)
	var redactor bytes.Buffer
	binary.Write(&redactor, binary.BigEndian, &vacio)
	escribirBytes(file, redactor.Bytes())

	//ELIMINAMOS LOS BLOQUES QUE TENGA OCUPADAS
	for i := 0; i < len(inodo.IArray_Bloque); i++ {
		if inodo.IArray_Bloque[i] != 0 {
			eliminarBloque(inodo.IArray_Bloque[i], file, sb)
		}
	}
	//ELIMINAMOS EL APUNTADOR DE APOYO SI TUVIERA
	if inodo.IAP != 0 {
		eliminarINodo(inodo.IAP, file, sb)
	}
}

func eliminarBloque(apuntador int64, file *os.File, sb SB) {
	//LEEMOS EL SUPERBLOQUE PARA TENER INFO ACTUALIZADA
	file.Seek(sb.SBAP_BitMap_AVD-int64(unsafe.Sizeof(sb)), 0)
	binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
	//INSTANCIAMOS UN BLOQUE VACIO
	bloque := Bloque{}
	//ELIMINAMOS EL ESPACIO
	file.Seek(apuntador, 0)
	var redactor bytes.Buffer
	binary.Write(&redactor, binary.BigEndian, &bloque)
	escribirBytes(file, redactor.Bytes())
	//SETEAMOS 0 EN EL BITMAP
	bitReference := (apuntador - sb.SBAP_Bloque) / sb.SB_Bloque_Size
	file.Seek(sb.SBAP_BitMap_Bloque+bitReference, 0)
	var cero [1]byte
	cero[0] = 0
	var redactor2 bytes.Buffer
	binary.Write(&redactor2, binary.BigEndian, &cero)
	escribirBytes(file, redactor2.Bytes())
	//AUMENTAMOS EN 1 EL CONTADOR DE ESTRUCTURAS LIBRES
	sb.SB_Bloque_Free += 1
	//REESCRIBIMOS EL SB
	file.Seek(sb.SBAP_BitMap_AVD-int64(unsafe.Sizeof(sb)), 0)
	var redactor3 bytes.Buffer
	binary.Write(&redactor3, binary.BigEndian, &sb)
	escribirBytes(file, redactor3.Bytes())
}

func eliminarAVD(apuntador int64, file *os.File, sb SB) {
	//LEEMOS EL SUPERBLOQUE PARA TENER INFO ACTUALIZADA
	file.Seek(sb.SBAP_BitMap_AVD-int64(unsafe.Sizeof(sb)), 0)
	binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
	//AUMENTAMOS EN 1 EL CONTADOR DE ESTRUCTURAS LIBRES
	sb.SB_AVD_Free += 1
	//SETEAMOS 0 EN EL BITMAP
	bitReference := (apuntador - sb.SBAP_AVD) / sb.SB_AVD_Size
	file.Seek(sb.SBAP_BitMap_AVD+bitReference, 0)
	var cero [1]byte
	cero[0] = 0
	var redactor2 bytes.Buffer
	binary.Write(&redactor2, binary.BigEndian, &cero)
	escribirBytes(file, redactor2.Bytes())
	//REESCRIBIMOS EL SB
	file.Seek(sb.SBAP_BitMap_AVD-int64(unsafe.Sizeof(sb)), 0)
	var redactor3 bytes.Buffer
	binary.Write(&redactor3, binary.BigEndian, &sb)
	escribirBytes(file, redactor3.Bytes())
	//INSTANCIAMOS EL AVD QUE SE MANDO A ELIMINAR
	avd := AVD{}
	file.Seek(apuntador, 0)
	binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
	//INSTANCIAMOS UN AVD VACIO
	vacio := AVD{}
	//ELIMINAMOS EL ESPACIO
	file.Seek(apuntador, 0)
	var redactor bytes.Buffer
	binary.Write(&redactor, binary.BigEndian, &vacio)
	escribirBytes(file, redactor.Bytes())
	//ELIMINAMOS LOS SUBARBOLES QUE TENGA
	for i := 0; i < len(avd.AVD_Sub_Directorios); i++ {
		if avd.AVD_Sub_Directorios[i] != 0 {
			//TENGO EL SUBDIRECTORIO
			eliminarAVD(avd.AVD_Sub_Directorios[i], file, sb)
		}
	}
	//ELIMINAMOS EL ARBOL DE APOYO SI TIENE
	if avd.AVDAP != 0 {
		eliminarAVD(avd.AVDAP, file, sb)
	}
	//ELIMINAMOS EL DETALLE DE DIRECTORIO SI TUVIERA
	if avd.AVD_DD != 0 {
		eliminarDD(avd.AVD_DD, file, sb)
	}

}

func eliminarDD(apuntador int64, file *os.File, sb SB) {
	//LEEMOS EL SUPERBLOQUE PARA TENER INFO ACTUALIZADA
	file.Seek(sb.SBAP_BitMap_AVD-int64(unsafe.Sizeof(sb)), 0)
	binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
	//AUMENTAMOS EN 1 EL CONTADOR DE ESTRUCTURAS LIBRES
	sb.SB_DD_Free += 1
	//SETEAMOS 0 EN EL BITMAP
	bitReference := (apuntador - sb.SBAP_DD) / sb.SB_DD_Size
	file.Seek(sb.SBAP_BitMap_DD+bitReference, 0)
	var cero [1]byte
	cero[0] = 0
	var redactor2 bytes.Buffer
	binary.Write(&redactor2, binary.BigEndian, &cero)
	escribirBytes(file, redactor2.Bytes())
	//REESCRIBIMOS EL SB
	file.Seek(sb.SBAP_BitMap_AVD-int64(unsafe.Sizeof(sb)), 0)
	var redactor3 bytes.Buffer
	binary.Write(&redactor3, binary.BigEndian, &sb)
	escribirBytes(file, redactor3.Bytes())
	//INSTANCIAMOS EL DD QUE SE MANDO A ELIMINAR
	dd := DD{}
	file.Seek(apuntador, 0)
	binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(dd)))), binary.BigEndian, &dd)
	//INSTANCIAMOS UN DD VACIO
	vacio := DD{}
	//ELIMINAMOS EL ESPACIO
	file.Seek(apuntador, 0)
	var redactor bytes.Buffer
	binary.Write(&redactor, binary.BigEndian, &vacio)
	escribirBytes(file, redactor.Bytes())
	//ELIMINAMOS LOS INODOS QUE TUVIERA
	for i := 0; i < len(dd.Files); i++ {
		if dd.Files[i].FIle_Inodo != 0 {
			//AQUI HAY UN INODO LOS ELIMINAMOS
			eliminarINodo(dd.Files[i].FIle_Inodo, file, sb)
		}
	}
	//ELIMINAMOS EL APUNTADOR DE APOYO SI TUVIERA
	if dd.DDAP != 0 {
		//SE USO UN APUNTADOR DE APOYO
		eliminarDD(dd.DDAP, file, sb)
	}
}

func crearFichero(ruta string, idusr int64, contenido string, file *os.File, dir AVD, crear bool, sb SB) {
	splitDirectorio := strings.Split(ruta, "/")
	if len(splitDirectorio) == 2 {
		var name [20]byte
		copy(name[:], splitDirectorio[1])
		//ALGORITMO PARA CREAR FICHERO EN EL AMBITO DEL ARBOL ACTUAL
		if dir.AVD_DD == 0 {
			//creamos el detalle de directorio
			dir.AVD_DD = crearDetalleDirectorio(file, sb)
		}
		banderNombre := false
		DDTrabajar := dir.AVD_DD
		dd := DD{}
		file.Seek(dir.AVD_DD, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(dd)))), binary.BigEndian, &dd)
		for {
			for i := 0; i < len(dd.Files); i++ {
				if dd.Files[i].File_Name == name {
					banderNombre = true
					break
				}
			}
			if dd.DDAP == 0 {
				break
			} else {
				DDTrabajar = dd.DDAP
				file.Seek(dd.DDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(dd)))), binary.BigEndian, &dd)
			}
		}
		if !banderNombre {
			//NO HAY NOMBRE POR LO QUE PODEMOS CREAR EL ARCHIVO EMPZANDO DESDE EL DD DEL DIRECTORIO
			DDTrabajar = dir.AVD_DD
			file.Seek(DDTrabajar, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(dd)))), binary.BigEndian, &dd)
			for {
				for i := 0; i < len(dd.Files); i++ {
					if dd.Files[i].FIle_Inodo == 0 {
						//ESTE ESTA VACIO AQUI VAMOS A ESCRIBIR
						dd.Files[i].FIle_Inodo = crearINodo(contenido, file, sb)
						dd.Files[i].File_Name = name
						copy(dd.Files[i].File_Create[:], time.Now().String())
						copy(dd.Files[i].File_Modify[:], time.Now().String())
						//REESCRIBIMOS EL DD
						file.Seek(dd.DD_Index, 0)
						var redactor bytes.Buffer
						binary.Write(&redactor, binary.BigEndian, &dd)
						escribirBytes(file, redactor.Bytes())
						//REESCRIBIMOS EL AVD
						file.Seek(dir.AVD_Index, 0)
						var redactor2 bytes.Buffer
						binary.Write(&redactor2, binary.BigEndian, &dir)
						escribirBytes(file, redactor2.Bytes())
						return
					}
				}
				//SE ASUME QUE EL DIRECTORIO ANALIZADO ESTABA LLENO
				if dd.DDAP == 0 {
					//CREAMOS UN NUEVO DIRECTORIO
					dd.DDAP = crearDetalleDirectorio(file, sb)
					file.Seek(dd.DDAP, 0)
					binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(dd)))), binary.BigEndian, &dd)
				} else {
					file.Seek(dd.DDAP, 0)
					binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(dd)))), binary.BigEndian, &dd)
				}
			}
		} else {
			//HAY UN NOMBRE, PROCEDEMOS A SOBREESCRIBIR EL ARCHIVO EN DD ANALIZADO
			file.Seek(DDTrabajar, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(dd)))), binary.BigEndian, &dd)
			for i := 0; i < len(dd.Files); i++ {
				if dd.Files[i].File_Name == name {
					eliminarINodo(dd.Files[i].FIle_Inodo, file, sb)
					dd.Files[i].FIle_Inodo = crearINodo(contenido, file, sb)
					copy(dd.Files[i].File_Create[:], time.Now().String())
					copy(dd.Files[i].File_Modify[:], time.Now().String())
					//REESCRIBIMOS EL DD
					file.Seek(dd.DD_Index, 0)
					var redactor bytes.Buffer
					binary.Write(&redactor, binary.BigEndian, &dd)
					escribirBytes(file, redactor.Bytes())
					//REESCRIBIMOS EL AVD
					file.Seek(dir.AVD_Index, 0)
					var redactor2 bytes.Buffer
					binary.Write(&redactor2, binary.BigEndian, &dir)
					escribirBytes(file, redactor2.Bytes())
					return
				}
			}
		}
	} else {
		//ALGORITMO PARA DIRECTORIOS
		var temporalname [20]byte
		var huboDir bool = false
		copy(temporalname[:], splitDirectorio[1])
		//BUSCAMOS SI EN LOS SUBDIRECTORIOS HAY UN ARBOL CON EL NOMBRE
		for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
			if dir.AVD_Sub_Directorios[i] != 0 {
				//AQUI HAY DIRECTORIOS
				avd := AVD{}
				file.Seek(dir.AVD_Sub_Directorios[i], 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				if avd.AVD_Name == temporalname {
					//este sub direcotrio coinciden con el que se busca
					huboDir = true
					nuevaRuta := ""
					for j := 2; j < len(splitDirectorio); j++ {
						nuevaRuta += "/" + splitDirectorio[j]
					}
					crearFichero(nuevaRuta, idusr, contenido, file, avd, crear, sb)
					return
				}
			}
		}
		if !huboDir {
			//SI NO SE ENCONTRO EN EL AMBITO ACTUAL VERIFICAMOS QUE HAYA UNA EXTENSION Y BUSCAMOS AHI
			if dir.AVDAP != 0 {
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				crearFichero(ruta, idusr, contenido, file, avd, crear, sb)
				return
			} else {
				//NO HAY EXTENSION ENTONCES PODEMOS CREAR EL ARCHIVO EN ESTE AMBITO
				if crear {
					var dirExtension bool = true
					for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
						if dir.AVD_Sub_Directorios[i] == 0 {
							//ESTE SEGMENTO NO APUNTA A NADA ASI QUE LO PODEMOS CREAR
							dirExtension = false //hacemos false el que no necesitemos de una extension
							//CREAMOS EL DIRECTORIO
							dir.AVD_Sub_Directorios[i] = crearDirectorio(temporalname, idusr, file, sb)
							//RECURSIVIDAD AL ENTORNO DEL SUB DIRECTORIO CREADO
							avd := AVD{}
							file.Seek(dir.AVD_Sub_Directorios[i], 0)
							binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
							nuevaRuta := ""
							for j := 2; j < len(splitDirectorio); j++ {
								nuevaRuta += "/" + splitDirectorio[j]
							}
							//REESCRIBIMOS DIR
							file.Seek(dir.AVD_Index, 0)
							var redactor bytes.Buffer
							binary.Write(&redactor, binary.BigEndian, &dir)
							escribirBytes(file, redactor.Bytes())
							crearFichero(nuevaRuta, idusr, contenido, file, avd, crear, sb)
							return
						}
					}
					if dirExtension {
						//SE HA SOBREPASADO EL LIMITE DE DE SUBDIRECTORIOS REFERENCIAMOS UNO NUEVO
						dir.AVDAP = crearDirectorio(dir.AVD_Name, idusr, file, sb)
						avd := AVD{}
						file.Seek(dir.AVDAP, 0)
						binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
						//REESCRIBIMOS DIR
						file.Seek(dir.AVD_Index, 0)
						var redactor bytes.Buffer
						binary.Write(&redactor, binary.BigEndian, &dir)
						escribirBytes(file, redactor.Bytes())
						crearFichero(ruta, idusr, contenido, file, avd, crear, sb)
						return
					}
				} else {
					fmt.Println("No existe el directorio")
				}
			}
		}
	}
}

func crearCarpeta(ruta string, idusr int64, file *os.File, dir AVD, crear bool, sb SB) {
	splitDirectorio := strings.Split(ruta, "/")
	if len(splitDirectorio) == 2 {
		var nombre [20]byte         //contiene el nombre del nuevo directorio
		var encontrado bool = false //true si logramos escribir el directorio
		copy(nombre[:], splitDirectorio[1])
		//CASO donde /user no es un archivo sino otro directorio
		//del direcotrio actual buscamos en sus subdirectorios
		for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
			if dir.AVD_Sub_Directorios[i] == 0 {
				//AQUI HAY UN DIRECTORIO VACIO PODEMOS CREARLO AQUI
				encontrado = true
				dir.AVD_Sub_Directorios[i] = crearDirectorio(nombre, idusr, file, sb)
				//REESCRIBIMOS EL ARBOL AVD
				file.Seek(dir.AVD_Index, 0)
				var redactor bytes.Buffer
				binary.Write(&redactor, binary.BigEndian, &dir)
				escribirBytes(file, redactor.Bytes())
				fmt.Println("Carpeta Creada")
				return
			}
		}
		if !encontrado {
			//SI NO ENCONTRAMOS ESPACIO PARA CREAR LA CARPETA BUSCAMOS SI HAY UNA EXTENSION DEL DIRECTORIO ACTUAL
			if dir.AVDAP != 0 {
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				crearCarpeta(ruta, idusr, file, avd, crear, sb)
				return
			} else {
				//SI NO HAY EXTENSION CREAMOS UNA Y ENVIAMOS A CREAR EN LA EXTENSION
				dir.AVDAP = crearDirectorio(dir.AVD_Name, idusr, file, sb)
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				crearCarpeta(ruta, idusr, file, avd, crear, sb)
				//REESCRIBIMOS EL DIR
				file.Seek(dir.AVD_Index, 0)
				var redactor bytes.Buffer
				binary.Write(&redactor, binary.BigEndian, &dir)
				escribirBytes(file, redactor.Bytes())
				crearCarpeta(ruta, idusr, file, avd, crear, sb)
				return
			}
		}
	} else {
		//ALGORITMO PARA DIRECTORIOS
		var temporalname [20]byte
		var huboDir bool = false
		copy(temporalname[:], splitDirectorio[1])
		//BUSCAMOS SI EN LOS SUBDIRECTORIOS HAY UN ARBOL CON EL NOMBRE
		for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
			if dir.AVD_Sub_Directorios[i] != 0 {
				//AQUI HAY DIRECTORIOS
				avd := AVD{}
				file.Seek(dir.AVD_Sub_Directorios[i], 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				if avd.AVD_Name == temporalname {
					//este sub direcotrio coinciden con el que se busca
					huboDir = true
					nuevaRuta := ""
					for j := 2; j < len(splitDirectorio); j++ {
						nuevaRuta += "/" + splitDirectorio[j]
					}
					crearCarpeta(nuevaRuta, idusr, file, avd, crear, sb)
					return
				}
			}
		}
		if !huboDir {
			//SI NO SE ENCONTRO EN EL AMBITO ACTUAL VERIFICAMOS QUE HAYA UNA EXTENSION Y BUSCAMOS AHI
			if dir.AVDAP != 0 {
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				crearCarpeta(ruta, idusr, file, avd, crear, sb)
				return
			} else {
				//NO HAY EXTENSION ENTONCES PODEMOS CREAR EL DIRECTORIO EN ESTE AMBITO
				if crear {
					var dirExtension bool = true
					for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
						if dir.AVD_Sub_Directorios[i] == 0 {
							//ESTE SEGMENTO NO APUNTA A NADA ASI QUE LO PODEMOS CREAR
							dirExtension = false //hacemos false el que no necesitemos de una extension
							//CREAMOS EL DIRECTORIO
							dir.AVD_Sub_Directorios[i] = crearDirectorio(temporalname, idusr, file, sb)
							//RECURSIVIDAD AL ENTORNO DEL SUB DIRECTORIO CREADO
							avd := AVD{}
							file.Seek(dir.AVD_Sub_Directorios[i], 0)
							binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
							nuevaRuta := ""
							for j := 2; j < len(splitDirectorio); j++ {
								nuevaRuta += "/" + splitDirectorio[j]
							}
							//REESCRIBIMOS DIR
							file.Seek(dir.AVD_Index, 0)
							var redactor bytes.Buffer
							binary.Write(&redactor, binary.BigEndian, &dir)
							escribirBytes(file, redactor.Bytes())
							crearCarpeta(nuevaRuta, idusr, file, avd, crear, sb)
							return
						}
					}
					if dirExtension {
						//SE HA SOBREPASADO EL LIMITE DE DE SUBDIRECTORIOS REFERENCIAMOS UNO NUEVO
						dir.AVDAP = crearDirectorio(dir.AVD_Name, idusr, file, sb)
						avd := AVD{}
						file.Seek(dir.AVDAP, 0)
						binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
						//REESCRIBIMOS DIR
						file.Seek(dir.AVD_Index, 0)
						var redactor bytes.Buffer
						binary.Write(&redactor, binary.BigEndian, &dir)
						escribirBytes(file, redactor.Bytes())
						crearCarpeta(ruta, idusr, file, avd, crear, sb)
						return
					}
				} else {
					fmt.Println("No existe el directorio")
				}
			}
		}
	}
}

func leerArchivo(ruta string, file *os.File, dir AVD, sb SB) string {
	splitDirectorio := strings.Split(ruta, "/")
	if len(splitDirectorio) == 2 {
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])
		//BUSCAMOS EL ARCHIVO EN EL DIRECTORIO DE DIRECTORIO DEL DIRECTORIO ACTUAL
		if dir.AVD_DD != 0 {
			//EL DETALLE DE DIRECTORIO EXISTE
			dd := DD{}
			file.Seek(dir.AVD_DD, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(dd)))), binary.BigEndian, &dd)
			for i := 0; i < len(dd.Files); i++ {
				if dd.Files[i].File_Name == name {
					//ESTE ES NUESTRO ARCHIVO QUE BUSCAMOS
					encontrado = true //hemos encontrado nuestro archivo
					datos := ""       //vamos a construir una cadena con los bytes que vayamos a leer
					inodo := INodo{}
					file.Seek(dd.Files[i].FIle_Inodo, 0)
					binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(inodo)))), binary.BigEndian, &inodo)
					for {
						//ITERAMOS SOBRE LA CANTIDAD DE BLOQUES QUE TENGAMOS EN ESTE INODO
						for j := 0; j < int(inodo.ICount_Bloque); j++ {
							bloque := Bloque{}
							file.Seek(inodo.IArray_Bloque[j], 0)
							binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(bloque)))), binary.BigEndian, &bloque)
							for k := 0; k < len(bloque.DB_Data); k++ {
								datos += string(bloque.DB_Data[k])
							}
							//fmt.Println(datos)
						}
						//VERIFICAMOS SI EXISTE UN INODO AUXILIAR, SI NO HAY SE TERMINA EL PROCESO
						//fmt.Println(inodo.IAP)
						if inodo.IAP == 0 {
							break
						} else {
							file.Seek(inodo.IAP, 0)
							binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(inodo)))), binary.BigEndian, &inodo)
						}
					}
					return datos
				}
			}
			if !encontrado {
				//NO SE ENCONTRO EL ARCHIVO EN ESTE DD VERIFICAMOS SI EXISTE UNA AVD AUXILIAR
				if dir.AVDAP != 0 {
					//EXISTE UN AVD AUXILIAR
					avd := AVD{}
					file.Seek(dir.AVDAP, 0)
					binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
					return leerArchivo(ruta, file, avd, sb)
				} else {
					fmt.Println("No existe el archivo 1")
				}
			}
		} else {
			//EL DETALLE DE DIRECTORIO NO EXISTE EN ESTE DIRECTORIO
			//VERIFICAMOS SI ESTE DIRECTORIO DISPONE DE UNO AUXILIAR
			if dir.AVDAP != 0 {
				//EXISTE UN AVD AUXILIAR
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				return leerArchivo(ruta, file, avd, sb)
			} else {
				fmt.Println("No existe el archivo 2")
			}
		}
	} else {
		//BUSCAMOS EL ARCHIVO EN UN SUBDIRECTORIO
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])
		//BUSCAMOS SI EN LOS SUBDIRECTORIOS HAY UN ARBOL CON EL NOMBRE
		for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
			if dir.AVD_Sub_Directorios[i] != 0 {
				//AQUI HAY UN DIRECTORIO PROCEDEMOS A VERIFICAR SU NOMBRE
				avd := AVD{}
				file.Seek(dir.AVD_Sub_Directorios[i], 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				if avd.AVD_Name == name {
					//ESTE SUB DIRECTORIO COINCIDEN MANDAMOS A LEER EL ARCHIVO A ESE SUBDIRECTORIO
					encontrado = true //hemos encontrado el sudirectorio
					nuevaRuta := ""
					for j := 2; j < len(splitDirectorio); j++ {
						nuevaRuta += "/" + splitDirectorio[j]
					}
					return leerArchivo(nuevaRuta, file, avd, sb)
				}
			}
		}
		if !encontrado {
			if dir.AVDAP != 0 {
				//SI EXISTE UNA EXTENSION DEL DIRECTORIO PROCEDEMOS A BUSCAR EN ESE AMBIRO
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				return leerArchivo(ruta, file, avd, sb)
			} else {
				//SI NO HAY EXTENSION ENTONCES EL DIRECTORIO NO EXISTE
				fmt.Println("No existe el directorio")
			}
		}

	}
	return ""
}

func ModificarArchivo(ruta string, contenido string, file *os.File, dir AVD, sb SB) {
	splitDirectorio := strings.Split(ruta, "/")
	if len(splitDirectorio) == 2 {
		//MODIFICAR EN ESTE DIRECOTORIO
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])
		//BUSCAMOS EL ARCHIVO EN EL DETALLE DE DIRECTORIO DEL DIRECTORIO ACTUAL
		if dir.AVD_DD != 0 {
			//EL DETALLE DE DIRECTORIO EXISTE
			dd := DD{}
			file.Seek(dir.AVD_DD, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(dd)))), binary.BigEndian, &dd)
			for i := 0; i < len(dd.Files); i++ {
				if dd.Files[i].File_Name == name {
					//ESTE ES NUESTRO ARCHIVO QUE BUSCAMOS PROCEDMOS A MODIFICARLO
					encontrado = true //hemos encontrado nuestro archivo
					//PROCEDEMOS A ELIMINAR ESTE INODO
					eliminarINodo(dd.Files[i].FIle_Inodo, file, sb)
					//MODIFICAMOS DEL ARCHIVO VIEJO SU FECHA DE MODIFIACION E INODO
					copy(dd.Files[i].File_Modify[:], time.Now().String())
					dd.Files[i].FIle_Inodo = crearINodo(contenido, file, sb)
					//REESCRIBIMOS EL DD
					file.Seek(dd.DD_Index, 0)
					var redactor bytes.Buffer
					binary.Write(&redactor, binary.BigEndian, &dd)
					escribirBytes(file, redactor.Bytes())
					//REESCRIBIMOS EL AVD
					file.Seek(dir.AVD_Index, 0)
					var redactor2 bytes.Buffer
					binary.Write(&redactor2, binary.BigEndian, &dir)
					escribirBytes(file, redactor2.Bytes())
					return
				}
			}
			if !encontrado {
				//NO SE ENCONTRO EL ARCHIVO EN ESTE DD VERIFICAMOS SI EXISTE UNA AVD AUXILIAR
				if dir.AVDAP != 0 {
					//EXISTE UN AVD AUXILIAR
					avd := AVD{}
					file.Seek(dir.AVDAP, 0)
					binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
					ModificarArchivo(ruta, contenido, file, avd, sb)
					return
				} else {
					fmt.Println("No existe el archivo")
				}
			}
		} else {
			//EL DETALLE DE DIRECTORIO NO EXISTE EN ESTE DIRECTORIO
			//VERIFICAMOS SI ESTE DIRECTORIO DISPONE DE UNO AUXILIAR
			if dir.AVDAP != 0 {
				//EXISTE UN AVD AUXILIAR
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				ModificarArchivo(ruta, contenido, file, avd, sb)
				return
			} else {
				fmt.Println("No existe el archivo")
			}
		}
	} else {
		//NAVEGAR POR SUB DIRECOTRIOS
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])
		//BUSCAMOS SI EN LOS SUBDIRECOTRIOS HAY UN ARBOL CON EL NOMBRE
		for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
			if dir.AVD_Sub_Directorios[i] != 0 {
				//AQUI HAY UN DIRECTORIO PROCEDEMOS A VERIFICAR SU NOMBRE
				avd := AVD{}
				file.Seek(dir.AVD_Sub_Directorios[i], 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				if avd.AVD_Name == name {
					//ESTE SUB DIRECTORIO COINCIDEN MANDAMOS A MODIFICAR EL ARCHIVO A ESE SUBDIRECTORIO
					encontrado = true //hemos encontrado el sudirectorio
					nuevaRuta := ""
					for j := 2; j < len(splitDirectorio); j++ {
						nuevaRuta += "/" + splitDirectorio[j]
					}
					ModificarArchivo(nuevaRuta, contenido, file, avd, sb)
					return
				}
			}
		}
		if !encontrado {
			if dir.AVDAP != 0 {
				//SI EXISTE UNA EXTENSION DEL DIRECTORIO PROCEDEMOS A BUSCAR EN ESE AMBITO
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				ModificarArchivo(ruta, contenido, file, avd, sb)
				return
			} else {
				//SI NO HAY EXTENSION ENTONCES EL DIRECTORIO NO EXISTE
				fmt.Println("No existe el directorio")
			}
		}
	}
}

func EliminarArchivo(ruta string, file *os.File, dir AVD, sb SB) bool {
	splitDirectorio := strings.Split(ruta, "/")
	if len(splitDirectorio) == 2 {
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])
		//BUSCAMOS EL ARCHIVO EN EL DIRECTORIO DE DIRECTORIO DEL DIRECTORIO ACTUAL
		if dir.AVD_DD != 0 {
			//EL DETALLE DE DIRECTORIO EXISTE
			dd := DD{}
			file.Seek(dir.AVD_DD, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(dd)))), binary.BigEndian, &dd)
			for i := 0; i < len(dd.Files); i++ {
				if dd.Files[i].File_Name == name {
					//ESTE ES NUESTRO ARCHIVO QUE BUSCAMOS
					encontrado = true //hemos encontrado nuestro archivo procedemos a eliminarlo
					//ELIMINAMOS EL INODO CORRESPONDIENTE A ESTE ARCHIVO
					eliminarINodo(dd.Files[i].FIle_Inodo, file, sb)
					//ELIMINAMOS EL ARCHIVO INSTANCIANDO UNO NUEVO
					dd.Files[i] = Archivo{}
					//REESCRIBIMOS EL DD
					file.Seek(dd.DD_Index, 0)
					var redactor bytes.Buffer
					binary.Write(&redactor, binary.BigEndian, &dd)
					escribirBytes(file, redactor.Bytes())
					//REESCRIBIMOS EL AVD
					file.Seek(dir.AVD_Index, 0)
					var redactor2 bytes.Buffer
					binary.Write(&redactor2, binary.BigEndian, &dir)
					escribirBytes(file, redactor2.Bytes())
					fmt.Println("Archivo Eliminado")
					return true //retornamos que el archivo fue borrado
				}
			}
			if !encontrado {
				//NO SE ENCONTRO EL ARCHIVO EN ESTE DD VERIFICAMOS SI EXISTE UNA AVD AUXILIAR
				if dir.AVDAP != 0 {
					//EXISTE UN AVD AUXILIAR
					avd := AVD{}
					file.Seek(dir.AVDAP, 0)
					binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)

					return EliminarArchivo(ruta, file, avd, sb)
				} else {
					fmt.Println("No existe el archivo")
				}
			}
		} else {
			//EL DETALLE DE DIRECTORIO NO EXISTE EN ESTE DIRECTORIO
			//VERIFICAMOS SI ESTE DIRECTORIO DISPONE DE UNO AUXILIAR
			if dir.AVDAP != 0 {
				//EXISTE UN AVD AUXILIAR
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)

				return EliminarArchivo(ruta, file, avd, sb)
			} else {
				fmt.Println("No existe el archivo")
			}
		}
	} else {
		//BUSCAMOS EL ARCHIVO EN UN SUBDIRECTORIO
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])
		//BUSCAMOS SI EN LOS SUBDIRECTORIOS HAY UN ARBOL CON EL NOMBRE
		for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
			if dir.AVD_Sub_Directorios[i] != 0 {
				//AQUI HAY UN DIRECTORIO PROCEDEMOS A VERIFICAR SU NOMBRE
				avd := AVD{}
				file.Seek(dir.AVD_Sub_Directorios[i], 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				if avd.AVD_Name == name {
					//ESTE SUB DIRECTORIO COINCIDEN MANDAMOS A LEER EL ARCHIVO A ESE SUBDIRECTORIO
					encontrado = true //hemos encontrado el sudirectorio
					nuevaRuta := ""
					for j := 2; j < len(splitDirectorio); j++ {
						nuevaRuta += "/" + splitDirectorio[j]
					}

					return EliminarArchivo(nuevaRuta, file, avd, sb)
				}
			}
		}
		if !encontrado {
			if dir.AVDAP != 0 {
				//SI EXISTE UNA EXTENSION DEL DIRECTORIO PROCEDEMOS A BUSCAR EN ESE AMBIRO
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)

				return EliminarArchivo(ruta, file, avd, sb)
			} else {
				//SI NO HAY EXTENSION ENTONCES EL DIRECTORIO NO EXISTE
				fmt.Println("No existe el directorio")
			}
		}

	}
	return false
}

func EliminarCarpeta(ruta string, borrar bool, file *os.File, dir AVD, sb SB) bool {
	splitDirectorio := strings.Split(ruta, "/")
	if len(splitDirectorio) == 2 {
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])
		//BUSCAMOS EL DIRECTORIO EN LOS SUBDIRECTORIOS DE LA CARPETA ACTUAL
		for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
			if dir.AVD_Sub_Directorios[i] != 0 {
				//OBTENEMOS INFO DEL SUBDIRECTORIO
				avd := AVD{}
				file.Seek(dir.AVD_Sub_Directorios[i], 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				if avd.AVD_Name == name {
					encontrado = true    //Hemos encontrado nuestra carpeta a eliminar
					subcarpetas := false //booleano que indica true si hay subcarpetas
					//ANTES DE ELIMINAR TENEMOS QUE VER SI NUESTRA CARPETA TIENE SUBDIRECTORIOS
					for j := 0; j < len(avd.AVD_Sub_Directorios); j++ {
						if avd.AVD_Sub_Directorios[i] != 0 {
							subcarpetas = true
							break
						}
					}
					if subcarpetas {
						//SI HAY SUB CARPETAS COMPROBAMOS EL PARAMETRO RF
						if borrar {
							eliminarAVD(dir.AVD_Sub_Directorios[i], file, sb)
							dir.AVD_Sub_Directorios[i] = 0
							fmt.Println("Carpeta Borrada")
							//REESCRIBIMOS EL DIR
							file.Seek(dir.AVD_Index, 0)
							var redactor bytes.Buffer
							binary.Write(&redactor, binary.BigEndian, &dir)
							escribirBytes(file, redactor.Bytes())
							return true
						} else {
							fmt.Println("No tiene permisos para borrar las subcarpetas")
						}
					} else {
						//SI NO HAY SUB CARPETAS PODEMOS ELIMINAR TODO
						eliminarAVD(dir.AVD_Sub_Directorios[i], file, sb)
						dir.AVD_Sub_Directorios[i] = 0
						fmt.Println("Carpeta Borrada")
						//REESCRIBIMOS EL DIR
						file.Seek(dir.AVD_Index, 0)
						var redactor bytes.Buffer
						binary.Write(&redactor, binary.BigEndian, &dir)
						escribirBytes(file, redactor.Bytes())
						return true
					}
				}
			}
		}
		if !encontrado {
			//NO SE ENCONTRO LA SUB CARPETA
			if dir.AVDAP != 0 {
				//SI HAY AVD DE APOYO BUSCAMOS ALLI
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				return EliminarCarpeta(ruta, borrar, file, avd, sb)
			} else {
				fmt.Println("No existe el directorio 1")
			}
		}
	} else {
		//BUSCAMOS LA CARPETA EN UN SUBDIRECTORIO
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])
		//BUSCAMOS SI EN LOS SUBDIRECTORIOS HAY UN ARBOL CON EL NOMBRE
		for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
			if dir.AVD_Sub_Directorios[i] != 0 {
				//AQUI HAY UN DIRECTORIO PROCEDEMOS A VERIFICAR SU NOMBRE
				avd := AVD{}
				file.Seek(dir.AVD_Sub_Directorios[i], 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				if avd.AVD_Name == name {
					//ESTE SUB DIRECTORIO COINCIDEN MANDAMOS A LEER EL ARCHIVO A ESE SUBDIRECTORIO
					encontrado = true //hemos encontrado el sudirectorio
					nuevaRuta := ""
					for j := 2; j < len(splitDirectorio); j++ {
						nuevaRuta += "/" + splitDirectorio[j]
					}
					return EliminarCarpeta(nuevaRuta, borrar, file, avd, sb)
				}
			}
		}
		if !encontrado {
			if dir.AVDAP != 0 {
				//SI EXISTE UNA EXTENSION DEL DIRECTORIO PROCEDEMOS A BUSCAR EN ESE AMBIRO
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				return EliminarCarpeta(ruta, borrar, file, avd, sb)
			} else {
				//SI NO HAY EXTENSION ENTONCES EL DIRECTORIO NO EXISTE
				fmt.Println("No existe el directorio 2")
			}
		}

	}
	return false
}

func RenombrarArchivo(ruta string, ren string, file *os.File, dir AVD, sb SB) bool {
	splitDirectorio := strings.Split(ruta, "/")
	if len(splitDirectorio) == 2 {
		//MODIFICAR EN ESTE DIRECOTORIO
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])
		//BUSCAMOS EL ARCHIVO EN EL DETALLE DE DIRECTORIO DEL DIRECTORIO ACTUAL
		if dir.AVD_DD != 0 {
			//EL DETALLE DE DIRECTORIO EXISTE
			dd := DD{}
			file.Seek(dir.AVD_DD, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(dd)))), binary.BigEndian, &dd)
			for i := 0; i < len(dd.Files); i++ {
				if dd.Files[i].File_Name == name {
					//ESTE ES NUESTRO ARCHIVO QUE BUSCAMOS PROCEDMOS A RENOMBRARLO
					encontrado = true //hemos encontrado nuestro archivo
					//PROCEDEMOS A RENOMBRAR EL ARCHIVO
					copy(dd.Files[i].File_Name[:], ren)
					//REESCRIBIMOS EL DD
					file.Seek(dd.DD_Index, 0)
					var redactor bytes.Buffer
					binary.Write(&redactor, binary.BigEndian, &dd)
					escribirBytes(file, redactor.Bytes())
					//REESCRIBIMOS EL AVD
					file.Seek(dir.AVD_Index, 0)
					var redactor2 bytes.Buffer
					binary.Write(&redactor2, binary.BigEndian, &dir)
					escribirBytes(file, redactor2.Bytes())
					fmt.Println("Archivo renombrado")
					return true
				}
			}
			if !encontrado {
				//NO SE ENCONTRO EL ARCHIVO EN ESTE DD VERIFICAMOS SI EXISTE UNA AVD AUXILIAR
				if dir.AVDAP != 0 {
					//EXISTE UN AVD AUXILIAR
					avd := AVD{}
					file.Seek(dir.AVDAP, 0)
					binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)

					return RenombrarArchivo(ruta, ren, file, avd, sb)
				} else {
					fmt.Println("No existe el archivo")
				}
			}
		} else {
			//EL DETALLE DE DIRECTORIO NO EXISTE EN ESTE DIRECTORIO
			//VERIFICAMOS SI ESTE DIRECTORIO DISPONE DE UNO AUXILIAR
			if dir.AVDAP != 0 {
				//EXISTE UN AVD AUXILIAR
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				return RenombrarArchivo(ruta, ren, file, avd, sb)
			} else {
				fmt.Println("No existe el archivo")
			}
		}
	} else {
		//NAVEGAR POR SUB DIRECOTRIOS
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])
		//BUSCAMOS SI EN LOS SUBDIRECOTRIOS HAY UN ARBOL CON EL NOMBRE
		for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
			if dir.AVD_Sub_Directorios[i] != 0 {
				//AQUI HAY UN DIRECTORIO PROCEDEMOS A VERIFICAR SU NOMBRE
				avd := AVD{}
				file.Seek(dir.AVD_Sub_Directorios[i], 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				if avd.AVD_Name == name {
					//ESTE SUB DIRECTORIO COINCIDEN MANDAMOS A MODIFICAR EL ARCHIVO A ESE SUBDIRECTORIO
					encontrado = true //hemos encontrado el sudirectorio
					nuevaRuta := ""
					for j := 2; j < len(splitDirectorio); j++ {
						nuevaRuta += "/" + splitDirectorio[j]
					}
					return RenombrarArchivo(nuevaRuta, ren, file, avd, sb)
				}
			}
		}
		if !encontrado {
			if dir.AVDAP != 0 {
				//SI EXISTE UNA EXTENSION DEL DIRECTORIO PROCEDEMOS A BUSCAR EN ESE AMBITO
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				return RenombrarArchivo(ruta, ren, file, avd, sb)
			} else {
				//SI NO HAY EXTENSION ENTONCES EL DIRECTORIO NO EXISTE
				fmt.Println("No existe el directorio")
			}
		}
	}
	return false
}

func RenombrarCarpeta(ruta string, ren string, file *os.File, dir AVD, sb SB) bool {
	splitDirectorio := strings.Split(ruta, "/")
	if len(splitDirectorio) == 2 {
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])
		//BUSCAMOS EL DIRECTORIO EN LOS SUBDIRECTORIOS DE LA CARPETA ACTUAL
		for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
			if dir.AVD_Sub_Directorios[i] != 0 {
				//OBTENEMOS INFO DEL SUBDIRECTORIO
				avd := AVD{}
				file.Seek(dir.AVD_Sub_Directorios[i], 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				if avd.AVD_Name == name {
					encontrado = true //Hemos encontrado nuestra carpeta a renombrar
					//RENOMBRAMOS NUESTRA CARPETA
					copy(avd.AVD_Name[:], ren)
					//REESCRIBIMOS EL AVD
					file.Seek(avd.AVD_Index, 0)
					var redactor bytes.Buffer
					binary.Write(&redactor, binary.BigEndian, &avd)
					escribirBytes(file, redactor.Bytes())
					fmt.Println("Carpeta renombrada")
					return true
				}
			}
		}
		if !encontrado {
			//NO SE ENCONTRO LA SUB CARPETA
			if dir.AVDAP != 0 {
				//SI HAY AVD DE APOYO BUSCAMOS ALLI
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				return RenombrarCarpeta(ruta, ren, file, avd, sb)
			} else {
				fmt.Println("No existe el directorio 1")
			}
		}
	} else {
		//BUSCAMOS LA CARPETA EN UN SUBDIRECTORIO
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])
		//BUSCAMOS SI EN LOS SUBDIRECTORIOS HAY UN ARBOL CON EL NOMBRE
		for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
			if dir.AVD_Sub_Directorios[i] != 0 {
				//AQUI HAY UN DIRECTORIO PROCEDEMOS A VERIFICAR SU NOMBRE
				avd := AVD{}
				file.Seek(dir.AVD_Sub_Directorios[i], 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				if avd.AVD_Name == name {
					//ESTE SUB DIRECTORIO COINCIDEN MANDAMOS A LEER EL ARCHIVO A ESE SUBDIRECTORIO
					encontrado = true //hemos encontrado el sudirectorio
					nuevaRuta := ""
					for j := 2; j < len(splitDirectorio); j++ {
						nuevaRuta += "/" + splitDirectorio[j]
					}
					return RenombrarCarpeta(nuevaRuta, ren, file, avd, sb)
				}
			}
		}
		if !encontrado {
			if dir.AVDAP != 0 {
				//SI EXISTE UNA EXTENSION DEL DIRECTORIO PROCEDEMOS A BUSCAR EN ESE AMBIRO
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				return RenombrarCarpeta(ruta, ren, file, avd, sb)
			} else {
				//SI NO HAY EXTENSION ENTONCES EL DIRECTORIO NO EXISTE
				fmt.Println("No existe el directorio 2")
			}
		}

	}
	return false
}

func encontrarCarpeta(ruta string, delete bool, file *os.File, dir AVD, sb SB) (indice int64) {
	splitDirectorio := strings.Split(ruta, "/")
	if len(splitDirectorio) == 2 {
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])

		//BUSCAMOS EL DIRECTORIO EN LOS SUBDIRECTORIOS DE LA CARPETA ACTUAL
		for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
			if dir.AVD_Sub_Directorios[i] != 0 {
				//OBTENEMOS INFO DEL SUBDIRECTORIO
				avd := AVD{}
				file.Seek(dir.AVD_Sub_Directorios[i], 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				if avd.AVD_Name == name {
					encontrado = true //Hemos encontrado nuestra carpeta a copiar
					if delete {
						dir.AVD_Sub_Directorios[i] = 0
						//REESCRIBIMOS EL AVD
						file.Seek(dir.AVD_Index, 0)
						var redactor bytes.Buffer
						binary.Write(&redactor, binary.BigEndian, &dir)
						escribirBytes(file, redactor.Bytes())
					}
					return avd.AVD_Index //retornamos el valor donde esta mi carpeta a copiar
				}
			}
		}
		if !encontrado {
			//NO SE ENCONTRO LA SUB CARPETA
			if dir.AVDAP != 0 {
				//SI HAY AVD DE APOYO BUSCAMOS ALLI
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				return encontrarCarpeta(ruta, delete, file, avd, sb)
			} else {
				fmt.Println("No existe el directorio 1")
			}
		}
	} else {
		//BUSCAMOS LA CARPETA EN UN SUBDIRECTORIO
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])
		//BUSCAMOS SI EN LOS SUBDIRECTORIOS HAY UN ARBOL CON EL NOMBRE
		for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
			if dir.AVD_Sub_Directorios[i] != 0 {
				//AQUI HAY UN DIRECTORIO PROCEDEMOS A VERIFICAR SU NOMBRE
				avd := AVD{}
				file.Seek(dir.AVD_Sub_Directorios[i], 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				if avd.AVD_Name == name {
					//ESTE SUB DIRECTORIO COINCIDEN MANDAMOS A LEER EL ARCHIVO A ESE SUBDIRECTORIO
					encontrado = true //hemos encontrado el sudirectorio
					nuevaRuta := ""
					for j := 2; j < len(splitDirectorio); j++ {
						nuevaRuta += "/" + splitDirectorio[j]
					}
					return encontrarCarpeta(nuevaRuta, delete, file, avd, sb)
				}
			}
		}
		if !encontrado {
			if dir.AVDAP != 0 {
				//SI EXISTE UNA EXTENSION DEL DIRECTORIO PROCEDEMOS A BUSCAR EN ESE AMBIRO
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				return encontrarCarpeta(ruta, delete, file, avd, sb)
			} else {
				//SI NO HAY EXTENSION ENTONCES EL DIRECTORIO NO EXISTE
				fmt.Println("No existe el directorio 2")
			}
		}

	}
	return 0
}

func encontrarArchivo(ruta string, delete bool, file *os.File, dir AVD, sb SB) (f Archivo) {
	splitDirectorio := strings.Split(ruta, "/")
	if len(splitDirectorio) == 2 {
		//MODIFICAR EN ESTE DIRECOTORIO
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])
		//BUSCAMOS EL ARCHIVO EN EL DETALLE DE DIRECTORIO DEL DIRECTORIO ACTUAL
		if dir.AVD_DD != 0 {
			//EL DETALLE DE DIRECTORIO EXISTE
			dd := DD{}
			file.Seek(dir.AVD_DD, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(dd)))), binary.BigEndian, &dd)
			for i := 0; i < len(dd.Files); i++ {
				if dd.Files[i].File_Name == name {
					//ESTE ES NUESTRO ARCHIVO QUE BUSCAMOS PROCEDMOS A RETORNARLO
					if delete {
						dd.Files[i] = Archivo{}
						//RESCRIBIMOS DD
						file.Seek(dd.DD_Index, 0)
						var redactor bytes.Buffer
						binary.Write(&redactor, binary.BigEndian, &dd)
						escribirBytes(file, redactor.Bytes())
					}
					return dd.Files[i]
				}
			}
			if !encontrado {
				//NO SE ENCONTRO EL ARCHIVO EN ESTE DD VERIFICAMOS SI EXISTE UNA AVD AUXILIAR
				if dir.AVDAP != 0 {
					//EXISTE UN AVD AUXILIAR
					avd := AVD{}
					file.Seek(dir.AVDAP, 0)
					binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)

					return encontrarArchivo(ruta, delete, file, avd, sb)
				} else {
					fmt.Println("No existe el archivo")
				}
			}
		} else {
			//EL DETALLE DE DIRECTORIO NO EXISTE EN ESTE DIRECTORIO
			//VERIFICAMOS SI ESTE DIRECTORIO DISPONE DE UNO AUXILIAR
			if dir.AVDAP != 0 {
				//EXISTE UN AVD AUXILIAR
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				return encontrarArchivo(ruta, delete, file, avd, sb)
			} else {
				fmt.Println("No existe el archivo")
			}
		}
	} else {
		//NAVEGAR POR SUB DIRECOTRIOS
		var name [20]byte
		var encontrado bool = false
		copy(name[:], splitDirectorio[1])
		//BUSCAMOS SI EN LOS SUBDIRECOTRIOS HAY UN ARBOL CON EL NOMBRE
		for i := 0; i < len(dir.AVD_Sub_Directorios); i++ {
			if dir.AVD_Sub_Directorios[i] != 0 {
				//AQUI HAY UN DIRECTORIO PROCEDEMOS A VERIFICAR SU NOMBRE
				avd := AVD{}
				file.Seek(dir.AVD_Sub_Directorios[i], 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				if avd.AVD_Name == name {
					//ESTE SUB DIRECTORIO COINCIDEN MANDAMOS A MODIFICAR EL ARCHIVO A ESE SUBDIRECTORIO
					encontrado = true //hemos encontrado el sudirectorio
					nuevaRuta := ""
					for j := 2; j < len(splitDirectorio); j++ {
						nuevaRuta += "/" + splitDirectorio[j]
					}
					return encontrarArchivo(nuevaRuta, delete, file, avd, sb)
				}
			}
		}
		if !encontrado {
			if dir.AVDAP != 0 {
				//SI EXISTE UNA EXTENSION DEL DIRECTORIO PROCEDEMOS A BUSCAR EN ESE AMBITO
				avd := AVD{}
				file.Seek(dir.AVDAP, 0)
				binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
				return encontrarArchivo(ruta, delete, file, avd, sb)
			} else {
				//SI NO HAY EXTENSION ENTONCES EL DIRECTORIO NO EXISTE
				fmt.Println("No existe el directorio")
			}
		}
	}
	return Archivo{}
}

func FuncionLOSS(params []string) {
	//DECLARACION DE VARIABLES OBLIGATORIAS
	var id string
	//LOOP QUE RECOLECTA LOS PARAMETROS
	for i := 1; i < len(params); i++ {
		split := strings.Split(params[i], "->")
		switch strings.ToUpper(split[0]) {
		case "-ID":
			id = strings.Trim(split[1], "\n")
		case "":
		case " ":
			fmt.Println("")
		default:
			fmt.Println("No se reconoce el parametro ", strings.ToUpper(split[0]))
		}
	}
	disco, particion := ObtenerIndices(id)
	if Discos[disco].D_PARTS[particion].STATUS == 1 {
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR|os.O_CREATE, 0755)
		//OBTENEMOS EL SB
		sb := SB{}
		file.Seek(Discos[disco].D_PARTS[particion].START, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(sb)))), binary.BigEndian, &sb)
		inicio := Discos[disco].D_PARTS[particion].START
		final := sb.SBAP_Bitacora
		/*
			file.Seek(sb.SBAP_BitMap_Bloque+bitReference, 0)
			var uno [1]byte
			uno[0] = 1
			var redactor2 bytes.Buffer
			binary.Write(&redactor2, binary.BigEndian, &uno)
			escribirBytes(file, redactor2.Bytes())*/

		//FORMATEAMOS
		tipo := make([]byte, final-inicio)
		for i := 0; i < len(tipo); i++ {
			tipo[i] = 0
		}
		file.Seek(Discos[disco].D_PARTS[particion].START, 0)
		file.Write(tipo)
		/*var redactor bytes.Buffer
		binary.Write(&redactor, binary.BigEndian, &tipo)
		escribirBytes(file, redactor.Bytes())*/

		fmt.Println("Perdida de datos completada")
	} else {
		fmt.Println("La particion no esta montada")
	}
}

func FuncionRECOVERY(params []string) {
	//DECLARACION DE VARIABLES OBLIGATORIAS
	var id string
	//LOOP QUE RECOLECTA LOS PARAMETROS
	for i := 1; i < len(params); i++ {
		split := strings.Split(params[i], "->")
		switch strings.ToUpper(split[0]) {
		case "-ID":
			id = strings.Trim(split[1], "\n")
		case "":
		case " ":
			fmt.Println("")
		default:
			fmt.Println("No se reconoce el parametro ", strings.ToUpper(split[0]))
		}
	}
	disco, particion := ObtenerIndices(id)
	if Discos[disco].D_PARTS[particion].STATUS == 1 {
		file, _ := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR|os.O_CREATE, 0755)
		//FORMATEAR EL DISCO
		sizeSB := int(unsafe.Sizeof(SB{}))
		sizeAVD := int(unsafe.Sizeof(AVD{}))
		sizeDD := int(unsafe.Sizeof(DD{}))
		sizeIN := int(unsafe.Sizeof(INodo{}))
		sizeB := int(unsafe.Sizeof(Bloque{}))
		sizeLOG := int(unsafe.Sizeof(Bitacora{}))
		noEstructuras := (int(Discos[disco].D_PARTS[particion].SIZE) - (2 * sizeSB)) / (27 + sizeAVD + sizeDD + (5*sizeIN + (20 * sizeB) + sizeLOG))
		SuperBloque := SB{}
		//TAMAÑO Y CANTIDAD DE LAS ESTRUCTURAS
		SuperBloque.SB_AVD_Count = int64(noEstructuras)
		SuperBloque.SB_AVD_Size = int64(sizeAVD)
		SuperBloque.SB_DD_Count = int64(noEstructuras)
		SuperBloque.SB_DD_Size = int64(sizeDD)
		SuperBloque.SB_INodo_Count = int64(5 * noEstructuras)
		SuperBloque.SB_INodo_Size = int64(sizeIN)
		SuperBloque.SB_Bloque_Count = int64(20 * noEstructuras)
		SuperBloque.SB_Bloque_Size = int64(sizeSB)
		//APUNTADORES
		SuperBloque.SBAP_BitMap_AVD = Discos[disco].D_PARTS[particion].START + int64(sizeSB)
		SuperBloque.SBAP_AVD = SuperBloque.SBAP_BitMap_AVD + int64(noEstructuras)
		SuperBloque.SBAP_BitMap_DD = SuperBloque.SBAP_AVD + int64(noEstructuras*sizeAVD)
		SuperBloque.SBAP_DD = SuperBloque.SBAP_BitMap_DD + int64(noEstructuras)
		SuperBloque.SBAP_BitMap_INodo = SuperBloque.SBAP_DD + int64(noEstructuras*sizeDD)
		SuperBloque.SBAP_INodo = SuperBloque.SBAP_BitMap_INodo + int64(5*noEstructuras)
		SuperBloque.SBAP_BitMap_Bloque = SuperBloque.SBAP_INodo + int64(5*noEstructuras*sizeIN)
		SuperBloque.SBAP_Bloque = SuperBloque.SBAP_BitMap_Bloque + int64(20*noEstructuras)
		SuperBloque.SBAP_Bitacora = SuperBloque.SBAP_Bloque + int64(20*noEstructuras*sizeB)
		//PRIMER BIT LIBRE
		SuperBloque.SB_AVD_FreeBit = 0
		SuperBloque.SB_DD_FreeBit = 0
		SuperBloque.SB_INodo_FreeBit = 0
		SuperBloque.SB_Bloque_FreeBit = 0
		//ESTRUCTURAS LIBRES
		SuperBloque.SB_AVD_Free = SuperBloque.SB_AVD_Count
		SuperBloque.SB_DD_Free = SuperBloque.SB_DD_Count
		SuperBloque.SB_INodo_Free = SuperBloque.SB_INodo_Count
		SuperBloque.SB_Bloque_Free = SuperBloque.SB_Bloque_Count
		//OTRA INFO
		copy(SuperBloque.SB_Name_HD[:], Discos[disco].D_PATH)
		copy(SuperBloque.SB_Date_Create[:], time.Now().String())
		copy(SuperBloque.SB_Last_Date_Mount[:], time.Now().String())
		SuperBloque.SB_Mount_Count = 1
		SuperBloque.SB_Master_Programmer = 201800585
		SuperBloque.SB_Bitacora_Count = 0

		//PROCEDEMOS A ESCRIBIR ESTE BLOQUE EN EL INICIO DE LA PARTICION Y AL FINAL DE LA BITACORA
		file, err := os.OpenFile(Discos[disco].D_PATH, os.O_RDWR|os.O_CREATE, 0755)
		if err != nil {
			return
		}
		//ESCRIBIMOS EL PRIMER SB
		primerSB := Discos[disco].D_PARTS[particion].START
		file.Seek(primerSB, 0)
		var redactor bytes.Buffer
		binary.Write(&redactor, binary.BigEndian, &SuperBloque)
		escribirBytes(file, redactor.Bytes())
		//ESCRIBIMOS EL SB COPIA
		compiaSB := SuperBloque.SBAP_Bitacora + int64(noEstructuras*sizeLOG)
		file.Seek(compiaSB, 0)
		var redactor2 bytes.Buffer
		binary.Write(&redactor2, binary.BigEndian, &SuperBloque)
		escribirBytes(file, redactor2.Bytes())
		//CREAMOS EL DIRECTORIO ROOT//
		var nombreroot [20]byte
		copy(nombreroot[:], "/")
		crearDirectorio(nombreroot, 1, file, SuperBloque)
		//CREAMOS UN ARCHIVO users.txt //
		avd := AVD{}
		file.Seek(SuperBloque.SBAP_AVD, 0)
		binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avd)))), binary.BigEndian, &avd)
		crearFichero("/users.txt", 1, "1,G,root\n1,U,root,root,201800585\n", file, avd, true, SuperBloque)
		//RESTAURAMOS LA BITACORA
		bitacora := Bitacora{}
		Log := SuperBloque.SBAP_Bitacora
		for {
			file.Seek(Log, 0)
			binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(bitacora)))), binary.BigEndian, &bitacora)
			if bitacora.Log_Operacion == 0 {
				break
			} else {
				switch bitacora.Log_Operacion {
				case 1:
					//mkFILE
					crearFichero(fmt.Sprintf("%s", SuprimidorEspacios100(bitacora.Log_Nombre)), 664, fmt.Sprintf("%s", SuprimidorEspacios100(bitacora.Log_Contenido)), file, avd, true, SuperBloque)
				case 2:
					//RM
					if bitacora.Log_Tipo == 0 {
						EliminarArchivo(fmt.Sprintf("%s", SuprimidorEspacios100(bitacora.Log_Nombre)), file, avd, SuperBloque)
					} else {
						EliminarCarpeta(fmt.Sprintf("%s", SuprimidorEspacios100(bitacora.Log_Nombre)), true, file, avd, SuperBloque)
					}
				case 3:
					//EDIT
					ModificarArchivo(fmt.Sprintf("%s", SuprimidorEspacios100(bitacora.Log_Nombre)), fmt.Sprintf("%s", SuprimidorEspacios100(bitacora.Log_Contenido)), file, avd, SuperBloque)
				case 4:
					//REN
					if bitacora.Log_Tipo == 0 {
						RenombrarArchivo(fmt.Sprintf("%s", SuprimidorEspacios100(bitacora.Log_Nombre)), fmt.Sprintf("%s", SuprimidorEspacios100(bitacora.Log_Contenido)), file, avd, SuperBloque)
					} else {
						RenombrarCarpeta(fmt.Sprintf("%s", SuprimidorEspacios100(bitacora.Log_Nombre)), fmt.Sprintf("%s", SuprimidorEspacios100(bitacora.Log_Contenido)), file, avd, SuperBloque)
					}
				case 5:
					//MKDIR
					crearCarpeta(fmt.Sprintf("%s", SuprimidorEspacios100(bitacora.Log_Nombre)), 664, file, avd, true, SuperBloque)
				case 6:
					//CP
					path := fmt.Sprintf("%s", SuprimidorEspacios100(bitacora.Log_Nombre))
					dest := fmt.Sprintf("%s", SuprimidorEspacios100(bitacora.Log_Contenido))
					directorioObjetivo := encontrarCarpeta(path, false, file, avd, SuperBloque)
					if directorioObjetivo != 0 {
						//TENGO QUE COPIAR EL DIRECTORIO
						directorioDestino := encontrarCarpeta(dest, false, file, avd, SuperBloque)
						if directorioDestino != 0 {
							//PROCEDEMOS
							avdO := AVD{} //ESTE ES EL QUE SE VA A COPIAR
							file.Seek(directorioObjetivo, 0)
							binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avdO)))), binary.BigEndian, &avdO)
							avdI := AVD{} //ESTE EL DESTINO DONDE VAMOS A COPIAR
							file.Seek(directorioDestino, 0)
							binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avdI)))), binary.BigEndian, &avdI)
							for {
								copiado := false
								for i := 0; i < len(avdI.AVD_Sub_Directorios); i++ {
									if avdI.AVD_Sub_Directorios[i] == 0 {
										avdI.AVD_Sub_Directorios[i] = CopiaAVDRecursiva(avdO, file, SuperBloque)
										copiado = true
										//REESCRIBIMOS EL AVD
										file.Seek(avdI.AVD_Index, 0)
										var redactor bytes.Buffer
										binary.Write(&redactor, binary.BigEndian, &avdI)
										escribirBytes(file, redactor.Bytes())
										break
									}
								}
								if copiado {
									break
								}
								//SE ASUME QUE EN EL AMBITO DE AVDI NO SE ENCONTRO ESPACIO LIBRE
								if avdI.AVDAP == 0 {
									//NO HAY DIRECTORIO AUXILIAR, CREAMOS UNO
									avdI.AVDAP = crearDirectorio(avdI.AVD_Name, avdI.AVD_Proper, file, SuperBloque)
									file.Seek(avdI.AVDAP, 0)
									binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avdI)))), binary.BigEndian, &avdI)
									//REESCRIBIMOS EL AVDI
									file.Seek(avdI.AVD_Index, 0)
									var redactor bytes.Buffer
									binary.Write(&redactor, binary.BigEndian, &avdI)
									escribirBytes(file, redactor.Bytes())
								} else {
									file.Seek(avdI.AVDAP, 0)
									binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(avdI)))), binary.BigEndian, &avdI)
								}
							}
						}
					} else {
						//TENGO QUE COPIAR EL ARCHIVO
						info := leerArchivo(path, file, avd, SuperBloque) //obtenemos la informacion que queremos copiar
						splitPath := strings.Split(path, "/")
						copyFile := dest + "/" + splitPath[len(splitPath)-1]
						crearFichero(copyFile, 664, info, file, avd, true, SuperBloque) //mandamos a crear otro fichero a dest cone
					}
				case 7:
					//MV
					path := fmt.Sprintf("%s", SuprimidorEspacios100(bitacora.Log_Nombre))
					dest := fmt.Sprintf("%s", SuprimidorEspacios100(bitacora.Log_Contenido))
					directorioObjetivo := encontrarCarpeta(path, false, file, avd, SuperBloque)
					if directorioObjetivo != 0 {
						//ENTONCES TENGO QUE COPIAR EL DIRECTORIO
						directorioDestino := encontrarCarpeta(dest, false, file, avd, SuperBloque)
						if directorioDestino != 0 {
							//PENDIENTE MOVER DIRECTORIOS de path a dest
							destino := AVD{}
							file.Seek(directorioDestino, 0)
							binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(destino)))), binary.BigEndian, &destino)
							var seteado bool = false
							for {
								if seteado {
									break
								} else {
									//ANALIZAMOS EL AUXILIAR ACTUAL
									for i := 0; i < len(destino.AVD_Sub_Directorios); i++ {
										if destino.AVD_Sub_Directorios[i] == 0 {
											seteado = true
											destino.AVD_Sub_Directorios[i] = directorioObjetivo
											//REESCRIBIMOS EL AVD
											file.Seek(destino.AVD_Index, 0)
											var redactor bytes.Buffer
											binary.Write(&redactor, binary.BigEndian, &destino)
											escribirBytes(file, redactor.Bytes())
											fmt.Println("Carpeta movida")
											break
										}
									}
									if seteado {
										break
									}
									//ASUMIENDO QUE LOS SUBDIRECTORIOS ESTAN LLENOS
									if destino.AVDAP != 0 {
										//HAY UN AUXILIAR VAMOS A VER AQUI
										file.Seek(destino.AVDAP, 0)
										binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(destino)))), binary.BigEndian, &destino)
									} else {
										//CREAMOS UN DIRECOTRIO AUXILIAR
										destino.AVDAP = crearDirectorio(destino.AVD_Name, destino.AVD_Proper, file, SuperBloque)
										//REESCRIBIMOS EL AVD
										file.Seek(destino.AVD_Index, 0)
										var redactor bytes.Buffer
										binary.Write(&redactor, binary.BigEndian, &destino)
										escribirBytes(file, redactor.Bytes())
										//LEEMOS EL DIRECTORIO AUXILIAR
										file.Seek(destino.AVDAP, 0)
										binary.Read(bytes.NewBuffer(leerBytes(file, int(unsafe.Sizeof(destino)))), binary.BigEndian, &destino)
									}
								}
							}
						}
					} else {
						//TENGO QUE MOVER EL ARCHIVO CONTENIDO EN Path
						info := leerArchivo(path, file, avd, SuperBloque) //obtenemos la informacion que queremos copiar
						EliminarArchivo(path, file, avd, SuperBloque)
						splitPath := strings.Split(path, "/")
						copyFile := dest + "/" + splitPath[len(splitPath)-1]
						crearFichero(copyFile, 664, info, file, avd, true, SuperBloque) //mandamos a crear otro fichero a dest cone
					}
				}
				Log += int64(unsafe.Sizeof(bitacora))
			}
		}
		//LIMPIAMOS LA BITACORA
		inicio := SuperBloque.SBAP_Bitacora
		final := SuperBloque.SBAP_Bitacora + int64(noEstructuras*int(unsafe.Sizeof(bitacora)))
		var cero [1]byte
		cero[0] = 0
		for i := inicio; i < final; i++ {
			file.Seek(i, 0)
			var redactor3 bytes.Buffer
			binary.Write(&redactor3, binary.BigEndian, &cero)
			escribirBytes(file, redactor3.Bytes())
		}
		fmt.Println("Se ha recuperado el sistema")
	} else {
		fmt.Println("La particion no esta montada")
	}
}
