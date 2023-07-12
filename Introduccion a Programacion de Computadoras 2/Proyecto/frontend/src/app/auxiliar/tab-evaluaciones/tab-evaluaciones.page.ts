import { Component, OnInit } from '@angular/core';
import { Servicio } from '../../servicio';

import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-tab-evaluaciones',
  templateUrl: './tab-evaluaciones.page.html',
  styleUrls: ['./tab-evaluaciones.page.scss'],
})
export class TabEvaluacionesPage implements OnInit {
  configuracionForm=false;


  constructor(private _servicio:Servicio) { }

  ngOnInit() {
	  this.obtenerDatosLogin();
    this.getSeccionData();
    this.listarEvaluaciones();
    this.listarPreguntas();
    this.listarRespuestas();
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
  //Listar todas las evaluaciones creadas
  datosEvaluaciones;
  listarEvaluaciones(){
    this._servicio.listarEvaluaciones().subscribe(
      res =>{
        this.datosEvaluaciones = res;
        console.log(this.datosEvaluaciones);
      }, (error) => {
        console.error(error);
      });
  }
  //Listar todas las preguntas de evaluaciones
  datosPreguntas;
  listarPreguntas(){
    this._servicio.listarPreguntas().subscribe(
      res => {
        this.datosPreguntas = res;
        this.contarPreguntas();
        console.log(this.datosPreguntas);
      }, (error) => {
        console.error(error);
      });
  }
  //Listar todas las respuestas de preguntas
  datosRespuestas;
  listarRespuestas(){
    this._servicio.listarRespuestas().subscribe(
      res => {
        this.datosRespuestas = res;
        console.log(this.datosRespuestas);
      }, (error) => {
        console.error(error);
      });
  }
//********************METODOS PARA LA CREACION DE EVALUACIONES******************//
contadorDePreguntas=0;//variable que guarda el numero de preguntas obtenidas
falsoVerdaderoForm=false;
OpcionMultipleForm=false;
//Crear nueva evaluacion
  crearEvaluacion(nombre: string){
    this._servicio.agregarEvaluacion(nombre, this.datosSeccion[0].idSeccion).subscribe(
      res => {
        console.log("evaluacion agregada");
        this.listarEvaluaciones();
      }, (error) =>{
        console.error(error);
      });
  }
  //ingresar a la configuracion de la evaluacion
  evaluacionSeleccionada=0; //variable que guarda la evaluacion seleccionada
  evaluacionNombre;//variable que guarda el nombre de la evaluacion
  habilitado=false;
  aleatorio=false;
  ingresarEvaluacion(id: string, nombre: string){
    this.contadorDePreguntas=0;
    this.evaluacionNombre=nombre;
    this.evaluacionSeleccionada=id;
    for (var i = 0; i < this.datosPreguntas.length; i++) {
      if (this.datosPreguntas[i].idEvaluacion == id) {
        this.contadorDePreguntas++;
      }
    };
    for (var i = this.datosEvaluaciones.length - 1; i >= 0; i--) {
      if (this.datosEvaluaciones[i].idEvaluacion == id) {
        if (this.datosEvaluaciones[i].habilitado==1) {
          this.habilitado=true;
        } else{
          this.habilitado=false;
        };
        if (this.datosEvaluaciones[i].aleatorio==1) {
          this.aleatorio=true;
        } else{
          this.aleatorio=false;
        };
      } 
    };
    this.configuracionForm=true;
  }
  //Despliege de formulario falso verdadero
  desplegarVerdaderoFalso(){
    this.falsoVerdaderoForm=true;
    this.OpcionMultipleForm=false;
  }
  //Despliege de formulario opcion multiple
  desplegarOpcionMultiple(){
    this.OpcionMultipleForm=true;
    this.falsoVerdaderoForm=false;
  }
  //Agregar nueva Preguta
  enunciadoPregunta;//variable que almacena el enunciado de la pregunta
  agregarPregunta(enunciado:string, respuesta:string, tipo:string){
    this._servicio.agregarPregunta(this.contadorDePreguntas, this.evaluacionSeleccionada, enunciado, respuesta, tipo).subscribe(
      res=>{
        this.listarPreguntas();
        console.log("pregunta añadida");
        console.log(this.contadorDePreguntas)
        this.falsoVerdaderoForm=false;
        this.OpcionMultipleForm=false;
        this.enunciadoPregunta="";
        
      },(error) =>{
        console.error(error);
      });
  }
  contarPreguntas(){
    this.contadorDePreguntas=0;
        for (var i = 0; i < this.datosPreguntas.length; i++) {
          if (this.datosPreguntas[i].idEvaluacion == this.evaluacionSeleccionada) {
            this.contadorDePreguntas++;
          }
        };
  }
  //Agregar posible respuestas
  posibleRespuesta;//variable que almacena la respuesta
  agregarRespuesta(res: string){
    this._servicio.agregarRespuesta(this.contadorDePreguntas, this.evaluacionSeleccionada, res).subscribe(
      res => {
        console.log("respuesta añadida");
        this.listarRespuestas();
        this.posibleRespuesta="";
      });
  }

  regresarAnterior(){
    this.configuracionForm=false;
  }

  aleatorioSet(valor: boolean){
    console.log(valor);
    if (valor) {
      this._servicio.setAleatorioValor("1", this.evaluacionSeleccionada).subscribe(
        res => {
          this.listarEvaluaciones();
          console.log("aleatorio Si");
        });
    }else{
      this._servicio.setAleatorioValor("0", this.evaluacionSeleccionada).subscribe(
        res => {
          this.listarEvaluaciones();
          console.log("aleatorio No");
        });
    }    
  }

  habilitadoSet(valor: boolean){
    console.log(valor);
    if (valor) {
      this._servicio.setHabilitadoValor("1", this.evaluacionSeleccionada).subscribe(
        res => {
          this.listarEvaluaciones();
          console.log("habilitado Si");
        });
    }else{
      this._servicio.setHabilitadoValor("0", this.evaluacionSeleccionada).subscribe(
        res => {
          this.listarEvaluaciones();
          console.log("habilitado No");
        });
    }  
  }

}
