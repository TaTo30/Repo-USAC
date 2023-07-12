import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Servicio } from '../../servicio';
import { AlertController } from '@ionic/angular';

@Component({
  selector: 'app-tab3',
  templateUrl: './tab3.page.html',
  styleUrls: ['./tab3.page.scss'],
})
export class Tab3Page implements OnInit {

    ///************* MENSAJES DE ALERTA **********************///
  async alertaRegistroFuncion(mensaje: string, titulo: string){
    const alertaRegistro = await this._alert.create({
      header: titulo,
      message: mensaje,
      buttons: ['OK']
    });
    await alertaRegistro.present();
  }

	 configuracionForm=false;


  constructor(private _servicio: Servicio, private _alert: AlertController) { }

  ngOnInit() {
    this.obtenerDatosLogin();
    this.getSeccionData();
    this.listarEvaluaciones();
    this.listarPreguntas();
    this.listarRespuestas();
    this.listarNotas();
    this.numeradorDeRespuestas=1 - (1 * 2);
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
  //Listar nota evaluaciones
  datosNotas;
  listarNotas(){
    this._servicio.listarNotas().subscribe(
      res => {
        this.datosNotas = res;
        console.log(this.datosNotas);
      }, (error) => {
        console.error(error);
      });
  }

  //ingresar a la configuracion de la evaluacion
  evaluacionSeleccionada; //variable que guarda la evaluacion seleccionada
  evaluacionNombre;//variable que guarada el nombre de la evaluacion
  contadorDePreguntas=0;//variable que almacena la cantidad de preguntas
  ingresarEvaluacion(id: string, nombre: string){
  this.respuestasCorrectas=0;
  this.contadorDePreguntas=0;
  this.numeradorDeRespuestas=1 - (1 * 2);
  this.evaluacionSeleccionada=id;
  this.evaluacionNombre=nombre;
  this.configuracionForm=true;
    for (var i = 0; i < this.datosPreguntas.length; i++) {
      if (this.datosPreguntas[i].idEvaluacion == id) {
        this.contadorDePreguntas++;
      }
    };
    for (var i = 0; i < this.datosNotas.length; i++) {
      if (this.datosNotas[i].idEvaluacion == this.evaluacionSeleccionada && this.datosNotas[i].carnet == this.datosUsuario[0].carnet) {
        this.alertaRegistroFuncion("Ya has repsondido esta evaluacion", "ATENCION");
        this.configuracionForm=false;
      }
    };
    
    
  }

  //Devulve a la pantalla anterior
  regresarAnterior(){
    this.configuracionForm=false;
  }

  //********************METODO PARA RESPONDER EVALUACION***************************//
  respuestasCorrectas=0;
  numeradorDeRespuestas=1 - (1 * 2);
  respondiendoPreguntas(no:number, res: string, valor: string){
    this.numeradorDeRespuestas=no;
    console.log(this.contadorDePreguntas+" jajaj xd "+no);
    for (var i = 0; i < this.datosPreguntas.length; i++) {
      if (this.datosPreguntas[i].idEvaluacion == this.evaluacionSeleccionada) {
        if ((this.datosPreguntas[i].noPregunta == no) && (this.datosPreguntas[i].respuesta == valor)) {
          this.respuestasCorrectas++;
          this.actualizarNota(this.respuestasCorrectas);
        }
      };
    };

    if (no == this.contadorDePreguntas-1) {
      this.alertaRegistroFuncion("Has terminado la evaluacion", "ATENCION");
      this.configuracionForm=false;
      this.actualizarNota(this.respuestasCorrectas);
      this.listarNotas();
    };
    

  }

  disponibilidad(index: number): boolean{
    if (index > this.numeradorDeRespuestas) {
      return false;
    } else{
      return true;
    }
  }


  actualizarNota(nota: number){
    console.log(nota);
    console.log(this.contadorDePreguntas);
    var notita = (nota*100)/this.contadorDePreguntas;
    this._servicio.actualizarNota(notita, this.datosUsuario[0].carnet, this.evaluacionSeleccionada).subscribe(
      res => {
        console.log("actualizado")
      });
    console.log(notita);
  }
}
