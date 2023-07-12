import { Component, OnInit } from '@angular/core';
import { Servicio } from '../../servicio';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-tab-foro',
  templateUrl: './tab-foro.page.html',
  styleUrls: ['./tab-foro.page.scss'],
})
export class TabForoPage implements OnInit {
  mensajesForoForm=false;

  constructor(private _servicio: Servicio, private _datepipe: DatePipe) { }

  ngOnInit() {
    this.mensajesForoForm=false;
    this.obtenerDatosLogin();
    this.getSeccionData();
  	this.ListarForo();
  }

  colorAleatorio(): string{
    return "#e7e7e7";

  }

  //************METODO QUE RECUPERA LOS DATOS DEL CURSO SELECCIONADO******************//
  //Recuperacion de datos de la seccion seleccionada
  datosSeccion;//variable que almacena datos del curso
  getSeccionData(){
    this._servicio.GetSeccionData().subscribe(
      res => {
        this.datosSeccion = res;
        console.log(this.datosSeccion);
      }, (error) => {
        console.error(error);
      });
  }
  //Recuperacion de datos del usuario logeado
  datosUsuario;//variable que almacena datos del usuario
  obtenerDatosLogin(){
    this._servicio.GetData().subscribe(
      res => {
        this.datosUsuario = res;
        console.log(this.datosUsuario);
      }, (error) => {
        console.error(error);
      });
  }
  //Recuperacion de foros creados
  datosForo;//Varibale que almacena los foros
  ListarForo(){
    this._servicio.listarForos().subscribe(
      res =>{
        this.datosForo=res;
        console.log(this.datosForo);
      }, (error) => {
        console.error(error);
      });
  }
  //Recuperacion de mensajes de foros
  datosMensajesForo;//Variable que almacena los mensajes
  ListarMensajesForo(){
    this._servicio.listarMensajesForos().subscribe(
      res => {
        this.datosMensajesForo = res;
        console.log(this.datosMensajesForo);
      }, (error) => {
        console.error(error);
      });
  }

  /*******************METODOS DE MANEJO DE NAVEGACION DE FORO******************
  ********************Y CREACION DE TEMAS Y MENSAJES DE FORO******************/
  //Seleccion de un Foro
  foroSeleccionado="0";//variable que guarda el id del foro seleccionado
  foroNombre="";
  ingresarTema(id: string, nombre: string){
    this.foroSeleccionado = id;
    this.foroNombre=nombre;
    this.ListarMensajesForo();
    this.mensajesForoForm = true;
  }
  //Crear un nuevo Foro
  nombreTemaForo="";//variable que guarda los datos del texto del nombre del foro
  crearTemaForo(nombre: string){
    this._servicio.agregarTemaForo(nombre, this.datosSeccion[0].idSeccion).subscribe(
      res => {
        console.log("foro creado");
        this.ListarForo();
        this.nombreTemaForo="";
      }, (error) => {
        console.error(error);
      });
  }
  //Enviar mensaje a un tema seleccionado
  mensajeForo=""//variable guardada para los mensajes
  enviarMensajeForo(mensaje: string){
    let date = this._datepipe.transform(new Date(), 'dd/MM/yyyy, h:mm a');
    this._servicio.enviarMensajeForo(this.foroSeleccionado, mensaje, date, this.datosUsuario[0].carnet).subscribe(
      res => {
        this.mensajeForo="";
        console.log("mensaje enviado");
        this.ListarMensajesForo();
      });
  }
  regresarAnterior(){
    this.mensajesForoForm=false;
  }



}
