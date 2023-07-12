import { Component, OnInit } from '@angular/core';
import { Servicio } from '../../../servicio';
import { AlertController } from	'@ionic/angular';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-asignar-auxiliar',
  templateUrl: './asignar-auxiliar.page.html',
  styleUrls: ['./asignar-auxiliar.page.scss'],
})
export class AsignarAuxiliarPage implements OnInit {
	listaSeccionesAuxiliar;//lista de secciones
	listaAuxiliares;//lista de auxiliares
	listaSecciones;//lista de secciones sin auxiliar
	carnetConsultado;//Variable que guarda el carnet consultado
	asignarForm=false;
	auxiliarConsultado=false;

  ///***************************FUNCION DE MENSAJES DE ALERTA**********************************///
  async mensajeAlertaFuncion(mensaje: string, titulo: string){
  	const mensajeAlerta = await this._alert.create({
  		message: mensaje,
  		header: titulo,
  		buttons: ['Ok'],
  	});
  	await mensajeAlerta.present();
  }


  constructor(private _servicio: Servicio, private _alert: AlertController) { }

  ngOnInit() {
  	this.listarAuxiliares();
  	this.listarSeccionesAuxiliar();
  	this.listarSeccionesSinAuxiliar();
  	this.auxiliarConsultado=false;
  	this.asignarForm=false;
  }

  ///*********************METODOS PARA LA CONSULTA Y DESPLIEGE DE AUXILIAR*********************///

  consultarAuxiliar(carnet:string){
  	this.carnetConsultado=carnet;
  	this.auxiliarConsultado=true;
  }

  ///*********************METODOS PARA LA ASIGNACION DE UN AUXILIAR***************************///
  asignarCurso(carnet: string){
  	this.carnetConsultado=carnet;
  	this.auxiliarConsultado=false;
  	this.asignarForm=true;
  }
  agregarAuxiliarSeccion(id: string){
  	this._servicio.agregarAuxiliarSeccion(id, this.carnetConsultado).subscribe(
  		res =>{
  			this.mensajeAlertaFuncion("El auxiliar ha sido asignado a esta seccion","Atencion")
  			this.asignarForm=false;
  			this.auxiliarConsultado=false;
  			this.listarAuxiliares();
  			this.listarSeccionesSinAuxiliar();
  			this.listarSeccionesAuxiliar();
  		}, (error) => {
  			console.error(error);
  		});
  }

  ///*************************METODO PARA DESASIGNAR UN AUXILIAR*******************************///
  desasignarCurso(carnet: string, ID: string){
  	this._servicio.desasignarCurso(carnet, ID).subscribe(
  		res => {
  			this.mensajeAlertaFuncion("El auxiliar ha sido desasignado de este curso","Atencion");
  			this.asignarForm=false;
  			this.auxiliarConsultado=false;
  			this.listarAuxiliares();
  			this.listarSeccionesSinAuxiliar();
  			this.listarSeccionesAuxiliar();  			
  		}, (error) => {
  			console.error(error);
  		});
  }



  ///*****************METODOS PARA EL LISTADO DE TODOS LOS USUARIOS****************************///
  //Listado de auxiliares
  listarAuxiliares(){
  	this._servicio.listarAuxiliares().subscribe(
  		res =>{
  			this.listaAuxiliares = res;
  			console.log(this.listaAuxiliares);
  		});
  }
  //Listado de secciones con auxiliares
  listarSeccionesAuxiliar(){
  	this._servicio.listarSeccionesAuxiliar().subscribe(
  		res => {
  			this.listaSeccionesAuxiliar = res;
  			console.log(this.listaSeccionesAuxiliar);
  		}, (error)=>{
  			console.error(error);
   		}  	
 	);
  }
  //Listado de secciones sin auxiliares
  listarSeccionesSinAuxiliar(){
  	this._servicio.listarSeccionesSinAuxiliar().subscribe(
  		res =>{	
  			this.listaSecciones = res;
  			console.log(this.listaSecciones);
  		}, (error) => {
  			console.error(error);
  		});
  }


  refrescarPagina(){
    this.listarSeccionesAuxiliar();
    this.listarSeccionesSinAuxiliar();
    this.listarAuxiliares();
  }

}
