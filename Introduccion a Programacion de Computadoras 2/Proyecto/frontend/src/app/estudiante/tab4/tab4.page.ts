import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Servicio } from '../../servicio';
import { AlertController } from '@ionic/angular';

@Component({
  selector: 'app-tab4',
  templateUrl: './tab4.page.html',
  styleUrls: ['./tab4.page.scss'],
})
export class Tab4Page implements OnInit {
  mostrarFormEnviar=false;


  ///************* MENSAJES DE ALERTA **********************///
  async alertaRegistroFuncion(mensaje: string, titulo: string){
    const alertaRegistro = await this._alert.create({
      header: titulo,
      message: mensaje,
      buttons: ['OK']
    });
    await alertaRegistro.present();
  }


  constructor(private _servicio:Servicio, private _datepipe: DatePipe, private _alert: AlertController) { }

  ngOnInit() {
    this.getSeccionData();
    this.obtenerDatosLogin();
    this.listarActividades();
    this.listarRespuestasActividades();
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
  //Listado de actividades
  datosActividades;//variable que almacena datos de las actividades
  listarActividades(){
    this._servicio.listarActividades().subscribe(
      res => {
        this.datosActividades = res;
        console.log(this.datosActividades);
      }, (error) => {
        console.error(error);
      });
  }
  //Listado de respuestas
  datosRespuestas;//variable que almacena datos de las respuestas de actividades
  listarRespuestasActividades(){
    this._servicio.listarRespuestasActividades().subscribe(
      res =>{
        this.datosRespuestas = res;
        console.log(this.datosRespuestas);
      }, (error) => {
        console.error(error);
      });
  }

  //*****************************METODOS PARA INGRESAR ACTIVIDAES*******************************//
  actividadSeleccionada;
  actividadTitulo;
  actividadEnunciado;
  ingresarActividad(id:string, titulo: string, enunciado: string){
    this.mostrarFormEnviar=true;
    this.actividadSeleccionada=id;
    this.actividadTitulo=titulo;
    this.actividadEnunciado=enunciado;
    for (var i = 0; i < this.datosRespuestas.length; i++) {
      if (this.datosRespuestas[i].idActividad == this.actividadSeleccionada && this.datosRespuestas[i].carnet == this.datosUsuario[0].carnet) {
        this.alertaRegistroFuncion("Ya has repsondido esta Actividad", "ATENCION");
        this.mostrarFormEnviar=false;
      }
    };
  }


  regresarAnterior(){
    this.mostrarFormEnviar=false;
  }

  enviarRespuesta(respuesta: string){
    this._servicio.enviarRespuesta(this.actividadSeleccionada, this.datosUsuario[0].carnet, respuesta, this._datepipe.transform(new Date(), 'dd/MM/yyyy, h:mm a')).subscribe(
      res => {
        console.log("respuesta enviada")
        this.listarRespuestasActividades();
        this.mostrarFormEnviar=false;
      }, (error) => {
        console.error(error);
      });
  }
 


}
