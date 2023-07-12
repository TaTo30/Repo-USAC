import { Component, OnInit } from '@angular/core';
import { Servicio } from '../../servicio';

import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-tab-actividades',
  templateUrl: './tab-actividades.page.html',
  styleUrls: ['./tab-actividades.page.scss'],
})
export class TabActividadesPage implements OnInit {


  constructor(private _servicio:Servicio, private _datepipe: DatePipe) { }

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
      }, (error) => {
        console.error(error);
      });
  }

  //**************************METODO PARA ACTIVIDADES****************************//
  //Agregar nueva calculadora
  agregarActividad(titulo: string, enunciado: string, fecha: string){
    this._servicio.agregarActividad(this.datosSeccion[0].idSeccion, titulo, enunciado, this._datepipe.transform(fecha, 'dd/MM/yyyy, h:mm a')).subscribe(
      res => {
        console.log("actividad agregada");
        this.listarActividades();
      }, (error) =>{
        console.error(error);
      });
  }

  actividadSeleccionada;
  listaRespuesta=false;
  verActividad(id: string){
    this.actividadSeleccionada = id;
    this.listaRespuesta=true;

  }

  actualizarNota(nota: string, carnet: string){
    this._servicio.actualizarNotaActividad(nota, this.actividadSeleccionada, carnet).subscribe(
      res => {
        this.listarRespuestasActividades();
        console.log("nota actualizada");
      });
  }

  regresar(){
    this.listaRespuesta=false;
  }
 

  


}