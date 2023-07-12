package main

import (
	"bytes"
	"encoding/json"
	"errors"
	"fmt"
	"log"
	"net/http"
	"os"
	"os/signal"
	"strconv"
	"strings"
	"syscall"
	"unsafe"
)

// Ayuda a escribir en la respuesta

// Imprimir en consola

// El paquete HTTP

type pids struct {
	Pid int
}

var pid int = -1

//
type SyscallTable map[int]func(syscall.PtraceRegs) func(syscall.PtraceRegs)

const (
	SysRead    = 0
	SysWrite   = 1
	SysOpen    = 2
	SysClose   = 3
	SysSocket  = 41
	SysConnect = 42
	SysSendto  = 44
	SysSendmsg = 46
	SysRecvmsg = 47
	SysClone   = 56
	SysFork    = 57
	SysVfork   = 58
	SysGetuid  = 102
	SysSyslog  = 103
	SysGetgid  = 104
	SysSetuid  = 105
	SysSetgid  = 106
	SysRestart = 219
	SysExit    = 231
	SysMmap    = 9
)

var families = []string{
	"PF_UNSPEC",
	"PF_UNIX",
	"PF_INET",
	"PF_AX25",
	"PF_IPX",
	"PF_APPLETALK",
	"PF_NETROM",
	"PF_BRIDGE",
	"PF_ATMPVC",
	"PF_X25",
	"PF_INET6",
	/* more */
}

var types = []string{
	"",
	"SOCK_STREAM",
	"SOCK_DGRAM",
	"SOCK_RAW",
	"SOCK_RDM",
	"SOCK_SEQPACKET",
	"SOCK_DCCP",
	"SOCK_PACKET",
}

func escribirLog(x string) int {
	f, err := os.OpenFile("text.log", os.O_APPEND|os.O_CREATE|os.O_WRONLY, 0644)
	if err != nil {
		fmt.Println(err)
	}
	_, errf := f.WriteString(x + "\n")
	if errf != nil {
		fmt.Println(errf)
	}
	f.Close()
	return 0
}

func initSyscalls() SyscallTable {

	//definiendo funciones que se ejecutan al encontrar la invocacion de este systemcall
	read_or_write := func(name string, regs syscall.PtraceRegs) func(syscall.PtraceRegs) {
		var addr = regs.Rsi
		var count = int32(regs.Rdx)

		fd := regs.Rdi
		fmt.Printf(`%s(%d, `, name, fd)
		//escribirLog(fmt.Sprint("write o read"))

		return func(regs syscall.PtraceRegs) {
			result := int32(regs.Rax)
			var buf []byte = []byte{}

			if result > 0 {
				buf = make([]byte, result+4)
				for i := 0; i <= int(result); i += 4 {

					//obtener datos del registro
					syscall.PtracePeekData(pid, uintptr(addr), buf[i:i+4])
					addr += 4
				}
				fmt.Printf("%s, %d) = %d\n", strconv.Quote(string(buf[0:result])), count, result)
			} else {
				fmt.Printf("\"\", %d) = %d\n", count, result)
			}
		}
	}

	fork_or_vfork := func(name string, regs syscall.PtraceRegs) func(regs syscall.PtraceRegs) {
		fmt.Printf("%s()", name)
		escribirLog(fmt.Sprint("fork"))
		return func(regs syscall.PtraceRegs) {
			var code = int32(regs.Rax)

			fmt.Printf(" = %d", code)
		}
	}

	parseSockAddr := func(bs []byte) (error, *syscall.RawSockaddr) {
		if len(bs) < 2 {
			return errors.New("ETOOSHORT"), nil
		}
		var family uint16 = uint16(bs[1])<<8 | uint16(bs[0])

		log.Printf("FAMILY IS %d\n", family)
		switch family {
		case syscall.AF_LOCAL:
			var rsau *syscall.RawSockaddrUnix
			rsau = (*syscall.RawSockaddrUnix)(unsafe.Pointer(&bs))
			log.Printf("%#v", rsau)
		}
		return errors.New("LUL"), nil
	}

	readBytes := func(addr uintptr, length int) []byte {
		var buf = make([]byte, length+4)

		log.Printf("READBYTES addr: %v length: %d\n", addr, length)

		for i := 0; i < length; i += 4 {
			syscall.PtracePeekData(pid, addr, buf[i:i+4])
			addr += 4
		}
		return buf
	}

	readString := func(addr uintptr, length int) string {
		buf := readBytes(addr, length)

		return strconv.Quote(string(buf))
	}

	//Se definel la syscalltable
	m := SyscallTable{

		SysRead: func(regs syscall.PtraceRegs) func(syscall.PtraceRegs) {
			return read_or_write("read", regs)
		},

		SysWrite: func(regs syscall.PtraceRegs) func(syscall.PtraceRegs) {
			return read_or_write("write", regs)
		},

		SysClose: func(regs syscall.PtraceRegs) func(syscall.PtraceRegs) {
			fd := regs.Rdi
			fmt.Printf("close(%d)", fd)
			return func(regs syscall.PtraceRegs) {
				var ret = int32(regs.Rax)

				fmt.Printf(" = %d\n", ret)
			}
		},

		SysMmap: func(regs syscall.PtraceRegs) func(syscall.PtraceRegs) {
			escribirLog(fmt.Sprint("mmap"))
			fd := regs.Rdi
			fmt.Printf("mmap(%d)", fd)
			return func(regs syscall.PtraceRegs) {
				var ret = int32(regs.Rax)

				fmt.Printf(" = %d\n", ret)
			}
		},

		SysOpen: func(regs syscall.PtraceRegs) func(syscall.PtraceRegs) {
			//escribo en el log solo el nombre del system call
			escribirLog(fmt.Sprint("open"))
			var fnameptr = regs.Rdi
			var flags = regs.Rsi
			var mode = regs.Rdx

			var buf = bytes.NewBuffer([]byte{})
			var mbuf = make([]byte, 4)
			var nullFound = false

			for {
				syscall.PtracePeekData(pid, uintptr(fnameptr), mbuf[0:4])
				j := 0
				for _, v := range mbuf {
					if v == '\x00' {
						nullFound = true
						break
					}
					j++
				}
				buf.Write(mbuf[0:j])
				if nullFound {
					break
				}
				fnameptr += 4
			}
			fmt.Printf("open(%s, %x, %x)", strconv.Quote(string(buf.Bytes())), flags, mode)

			return func(regs syscall.PtraceRegs) {
				ret := int32(regs.Rax)
				fmt.Printf(" = %d\n", ret)
			}
		},

		SysSocket: func(regs syscall.PtraceRegs) func(regs syscall.PtraceRegs) {
			family := regs.Rdi
			typ := regs.Rsi
			proto := regs.Rdx

			fmt.Printf("socket(%s, %s, %d)", families[family], types[typ&0xf], proto)

			return func(regs syscall.PtraceRegs) {
				ret := int32(regs.Rax)

				fmt.Printf(" = %d\n", ret)
			}
		},

		SysConnect: func(regs syscall.PtraceRegs) func(regs syscall.PtraceRegs) {
			fd := regs.Rdi
			sockaddr := regs.Rsi
			addrlen := regs.Rdx

			if sockaddr != 0 && addrlen != 0 {
				buf := readBytes(uintptr(sockaddr), int(addrlen))
				parseSockAddr(buf)
			}
			fmt.Printf("connect(%d, %x, %x)", fd, sockaddr, addrlen)

			return func(regs syscall.PtraceRegs) {
				ret := int32(regs.Rax)

				fmt.Printf(" = %d\n", ret)
			}
		},

		SysSendto: func(regs syscall.PtraceRegs) func(regs syscall.PtraceRegs) {
			fd := regs.Rdi
			bufptr := regs.Rsi
			length := regs.Rdx
			flags := regs.Rcx
			sockaddr := regs.R8
			addrlen := regs.R9

			buf := readString(uintptr(bufptr), int(length))

			fmt.Printf("sendto(%d, %s, %d, %d, %v, %d)", fd, buf, length, flags, sockaddr, addrlen)

			return func(regs syscall.PtraceRegs) {
				ret := int32(regs.Rax)

				fmt.Printf("= %d\n", ret)
			}
		},

		SysSendmsg: func(regs syscall.PtraceRegs) func(regs syscall.PtraceRegs) {
			fd := regs.Rdi
			msg := regs.Rsi
			flags := regs.Rdx

			fmt.Printf("sendmsg(%d, %v, %v)", fd, msg, flags)
			return func(regs syscall.PtraceRegs) {
				ret := int32(regs.Rax)

				fmt.Printf("= %d\n", ret)
			}
		},

		SysRecvmsg: func(regs syscall.PtraceRegs) func(regs syscall.PtraceRegs) {
			fd := regs.Rdi
			msg := regs.Rsi
			flags := regs.Rdx

			fmt.Printf("recvmsg(%d, ", fd)
			return func(regs syscall.PtraceRegs) {
				ret := int32(regs.Rax)

				if ret >= 0 {
					fmt.Printf("\"%v\", %s) = %d\n", msg, flags, ret)
				} else {
					fmt.Printf("\"\", %s) = %d\n", flags, ret)
				}
			}
		},

		SysSyslog: func(regs syscall.PtraceRegs) func(regs syscall.PtraceRegs) {
			typ := regs.Rdi
			msgptr := regs.Rsi
			length := regs.Rdx

			msg := readString(uintptr(msgptr), int(length))

			fmt.Printf("syslog(%d, %s, %d)", typ, msg, length)

			return func(regs syscall.PtraceRegs) {
				ret := int32(regs.Rax)

				fmt.Printf(" = %d\n", ret)
			}
		},

		SysGetuid: func(regs syscall.PtraceRegs) func(regs syscall.PtraceRegs) {
			fmt.Printf("getuid()")

			return func(regs syscall.PtraceRegs) {
				ret := int32(regs.Rax)

				fmt.Printf(" = %d\n", ret)
			}
		},

		SysGetgid: func(regs syscall.PtraceRegs) func(regs syscall.PtraceRegs) {
			fmt.Printf("getgid()")

			return func(regs syscall.PtraceRegs) {
				ret := int32(regs.Rax)

				fmt.Printf(" = %d\n", ret)
			}
		},

		SysSetuid: func(regs syscall.PtraceRegs) func(regs syscall.PtraceRegs) {
			uid := int32(regs.Rdi)
			fmt.Printf("setuid(%d)", uid)

			return func(regs syscall.PtraceRegs) {
				ret := int32(regs.Rax)

				fmt.Printf(" = %d\n", ret)
			}
		},

		SysExit: func(regs syscall.PtraceRegs) func(regs syscall.PtraceRegs) {
			var code = regs.Rdi
			fmt.Printf("exit_group(%d)\n", code)

			return func(regs syscall.PtraceRegs) {}
		},

		SysFork: func(regs syscall.PtraceRegs) func(regs syscall.PtraceRegs) {
			return fork_or_vfork("fork", regs)
		},

		SysVfork: func(regs syscall.PtraceRegs) func(regs syscall.PtraceRegs) {
			return fork_or_vfork("vfork", regs)
		},

		SysRestart: func(regs syscall.PtraceRegs) func(regs syscall.PtraceRegs) {
			fmt.Printf("<restarting syscall>")
			return func(regs syscall.PtraceRegs) {
			}
		},

		SysClone: func(regs syscall.PtraceRegs) func(regs syscall.PtraceRegs) {
			var flags = regs.Rdi
			var sp = regs.Rsi
			var envp = regs.Rdx
			fmt.Printf("clone(%x, %x, %x)", flags, sp, envp)

			return func(regs syscall.PtraceRegs) {
				var code = int32(regs.Rax)

				fmt.Printf(" = %d", code)
			}
		},
	}

	return m
}

func (sc SyscallTable) Call(regs syscall.PtraceRegs) func(syscall.PtraceRegs) {
	f, ok := sc[int(regs.Orig_rax)]
	if ok {
		return f(regs)
	} else {
		//log.Printf("unknown syscall %d", int(regs.Orig_rax))
		return nil
	}
}

func check(c chan os.Signal, die *bool) {
	for {
		<-c
		log.Printf("dying")
		*die = true
	}
}

//funcion para atachar strace de go a un pid
func atachar(pid int) {
	var wstat syscall.WaitStatus
	var complete func(syscall.PtraceRegs) = nil
	var die = false

	//estructura que tiene informacion de los registros del syscall
	regs := syscall.PtraceRegs{}

	isSyscall := func(wstat syscall.WaitStatus) bool {
		return (((uint32(wstat) & 0xff00) >> 8) & 0x80) != 0
	}

	//utilizamos initSyscalls para llenar la tabla de syscalls
	sc := initSyscalls()

	c := make(chan os.Signal, 1)
	signal.Notify(c, os.Kill, os.Interrupt)
	go check(c, &die)

	if pid == -1 {
		log.Fatal("No pid set")
	}

	//usamos syscall.PtraceAttach para pegarlo a un pid
	err := syscall.PtraceAttach(pid)
	if err != nil {
		log.Print("attach")
		log.Print(err)
		goto fail
	}

	_, err = syscall.Wait4(pid, &wstat, 0, nil)
	if err != nil {
		log.Printf("wait %d err %s\n", pid, err)
		goto fail
	}

	//opcion para poder utilizar strace sobre un pid
	err = syscall.PtraceSetOptions(pid, syscall.PTRACE_O_TRACESYSGOOD)
	if err != nil {
		log.Print("ptrace set options")
		log.Print(err)
		goto fail
	}

	//bucle para obtener informacion
	for !die {
		err = syscall.PtraceSyscall(pid, 0)
		if err != nil {
			log.Print("syscall")
			log.Print(err)
			goto fail
		}

		_, err = syscall.Wait4(pid, &wstat, 0, nil)
		if err != nil {
			log.Printf("wait %d err %s\n", pid, err)
			goto fail
		}

		// ENTER
		if wstat.Stopped() {
			if isSyscall(wstat) {
				//llenamos la estructura con la info de los registros
				err = syscall.PtraceGetRegs(pid, &regs)
				if err != nil {
					log.Print("regs")
					log.Print(err)
					goto fail
				}

				//ejecutamos la tabla de system calls
				complete = sc.Call(regs)
			}
		}

		err = syscall.PtraceSyscall(pid, 0)
		if err != nil {
			log.Print("syscall 2")
			log.Print(err)
			goto fail
		}

		_, err = syscall.Wait4(pid, &wstat, 0, nil)
		if err != nil {
			log.Printf("wait %d err %s\n", pid, err)
			goto fail
		}

		os.Stdout.Sync()
		if wstat.Stopped() {
			if isSyscall(wstat) {
				err = syscall.PtraceGetRegs(pid, &regs)
				if err != nil {
					log.Print("regs")
					log.Print(err)
					goto fail
				}

				if complete != nil {
					complete(regs)
					complete = nil
				}
			}
		}
	}

fail:
	syscall.Kill(pid, 18)
	err = syscall.PtraceDetach(pid)
	if err != nil {
		log.Print("detach")
		log.Print(err)
	}
}

// Servidor HTTP
func http_server(w http.ResponseWriter, r *http.Request) {

	if r.URL.Path == "/" {
		http.Error(w, "404 not found.", http.StatusNotFound)
		return
	}
	//Agregar header
	w.Header().Set("Content-Type", "application/json")
	w.Header().Set("Access-Control-Allow-Origin", "*")
	w.Header().Set("Access-Control-Allow-Headers", "Content-Type")

	switch r.Method {

	case "GET":

		//Tipos de peticiones
		if r.URL.Path == "/memory" {
			dat, errfile := os.ReadFile("/proc/ram_mem_g6")
			fmt.Print(errfile)

			jsonmem := string(dat)
			w.WriteHeader(http.StatusCreated)
			w.Write([]byte(jsonmem))
			return
		} else if r.URL.Path == "/process" || r.URL.Path == "/tree" {
			dat, errfile := os.ReadFile("/proc/procs_grupo6")
			fmt.Print(errfile)
			jsonmem := string(dat)

			val := strings.Split(jsonmem, ",\n]")[0]
			result := string(val) + "]\n}"

			w.WriteHeader(http.StatusCreated)
			w.Write([]byte(result))
			return
		}
		return
	case "POST":

		//Parsing body
		var body map[string]interface{}
		err := json.NewDecoder(r.Body).Decode(&body)
		if err != nil {
			fmt.Println("error al parsear el body")
			return
		}
		data, err := json.Marshal(body)
		newData := string(data)
		var pids_r pids
		json.Unmarshal([]byte(newData), &pids_r)
		//pid_result := strconv.Itoa(pids_r.Pid)

		if r.URL.Path == "/killprocess" {
			err := syscall.Kill(pids_r.Pid, syscall.SIGKILL)
			//err := exec.Command("kill ", "-9 ", pid_result).Process

			if err != nil {
				jsonmem := string("{\"res\" : \"Operation not permitted\"}")
				w.WriteHeader(http.StatusCreated)
				w.Write([]byte(jsonmem))
				fmt.Println(err)
			} else {
				jsonmem := string("{\"res\" : \"Ha sido detenido Correctamente\"}")
				w.WriteHeader(http.StatusCreated)
				w.Write([]byte(jsonmem))
			}

			return
		}

		if r.URL.Path == "/attach/" {
			atachar(pids_r.Pid)
			jsonmem := string("{\"res\" : \"Operation not permitted\"}")
			w.WriteHeader(http.StatusCreated)
			w.Write([]byte(jsonmem))
		}

		return
	default:
		fmt.Fprintf(w, "Metodo %s no soportado \n", r.Method)
		return
	}
}

func main() {

	http.HandleFunc("/", http_server)

	print("Servidor levantado en el puerto 3000")
	//Si hay error se apaga
	if err := http.ListenAndServe(":5000", nil); err != nil {
		log.Fatal(err)
	}

}
