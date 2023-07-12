import { Component, OnInit } from '@angular/core';
import { Servicio } from '../../../servicio';
import { AlertController } from '@ionic/angular';

@Component({
  selector: 'app-asignar-curso',
  templateUrl: './asignar-curso.page.html',
  styleUrls: ['./asignar-curso.page.scss'],
})
export class AsignarCursoPage implements OnInit {
	listaSeccionesAuxiliar;

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
  	this.listarSeccionesAuxiliar();
  }

  ///*****************METODOS PARA EL LISTADO DE TODOS LOS USUARIOS****************************///
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

  desasignarCurso(carnet: string, ID: string){
  	this._servicio.desasignarCurso(carnet, ID).subscribe(
  		res => {
  			this.mensajeAlertaFuncion("El auxiliar ha sido desasignado de este curso","Atencion");
  			this.listarSeccionesAuxiliar();  			
  		}, (error) => {
  			console.error(error);
  		});
  }

  refrescarPagina(){
  	this.listarSeccionesAuxiliar();
  }

}
