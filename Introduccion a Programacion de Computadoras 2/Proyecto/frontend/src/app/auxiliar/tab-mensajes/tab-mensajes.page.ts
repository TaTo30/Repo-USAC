import { Component, OnInit } from '@angular/core';
import { Servicio } from '../../servicio';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-tab-mensajes',
  templateUrl: './tab-mensajes.page.html',
  styleUrls: ['./tab-mensajes.page.scss'],
})
export class TabMensajesPage implements OnInit {
  estudianteSeleccionado = false;

  constructor(private _servicio: Servicio, private _datepipe: DatePipe) { }

  ngOnInit() {
    this.getSeccionData();
    this.obtenerDatosLogin();
    this.ListarInscritos();
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
  //Listado de estudiantes inscritos en secciones
  datosInscritos;//variable que almacena la lista de inscritos
  ListarInscritos(){

    this._servicio.listarInscritos().subscribe(
      res => {
        this.datosInscritos = res;
        console.log(this.datosInscritos);
      }, (error) => {
        console.error(error);
      });
  }
  //Listado de todos los mensajes privados
  datosMensajes;//variable que almacena los mensajes
  listarMensajes(){
    this._servicio.listarMensajes().subscribe(
      res => {
        this.datosMensajes = res;
        console.log(this.datosMensajes);
      }, (error) => {
        console.error(error);
      });
  }


  //*********************METODOS PARA EL ENVIO DE MENSAJES*********************//
  //Enviar mensaje
  mensajePrivado;//variable que almacena el texto del mensaje
  enviarMensajePrivado(mensaje: string){
    let fecha = this._datepipe.transform(new Date(), 'dd/MM/yyyy, h:mm a');
    this._servicio.enviarMensajePrivado(this.datosUsuario[0].carnet, this.carnetSeleccionado, mensaje, fecha, this.datosUsuario[0].nombre + " " + this.datosUsuario[0].apellido).subscribe(
      res => {
        console.log("mensaje enviado");
        this.mensajePrivado="";
        this.listarMensajes();
      });
  }
  //Usuario seleccionado para chatear
  carnetSeleccionado;//variable que almacena el usuario seleccionado
  contactoNombre;
  seleccionarUsuario(carnet: string, nombre: string){
    this.carnetSeleccionado=carnet;
    this.contactoNombre=nombre;
    this.listarMensajes();
    this.estudianteSeleccionado =true;
  }
  //Regresar a la pantall anterior
  regresarAnterior(){
    this.estudianteSeleccionado= false;
  }







}
