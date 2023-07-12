import { Component, OnInit } from '@angular/core';
import { Servicio } from '../../../servicio';
import { NgForm } from '@angular/forms';
import { AlertController } from '@ionic/angular';

@Component({
  selector: 'app-crear-auxiliar',
  templateUrl: './crear-auxiliar.page.html',
  styleUrls: ['./crear-auxiliar.page.scss'],
})
export class CrearAuxiliarPage implements OnInit {
	listaAuxiliares;
	listaEstudiantes;
	formDatosEstudiante = false;

  constructor(private _servicio: Servicio, private _alert: AlertController) { }

  ///******************METODO PARA DESPLIEGE DE MENSAJES**********************************///
  async mensajeAlertaFuncion(mensaje: string, titulo: string){
  	const mensajeAlerta = await this._alert.create({
  		message: mensaje,
  		header: titulo,
  		buttons: ['Ok'],
  	});
  	await mensajeAlerta.present();
  }

  ngOnInit() {
  	this.listarAuxiliares();
  	this.listarUsuarios();
  }
//Variable que guardan los datos del estudiante buscado
carnetEstudiante;
nombreEstudiante;
apellidoEstudiante;
emailEstudiante;
//variables que determinan si el estudiante existe
encontradoAuxiliar=false;
encontradoEstudiante=false;

  ///****************BUSCA EL ESTUDIANTE QUE HA SELECCIONADO EL ADMINISTRADOR******************///
  buscarEstudiante(buscarForm: NgForm): void{
  	this.carnetEstudiante= "";
  	this.nombreEstudiante= "";
  	this.apellidoEstudiante= "";
  	this.emailEstudiante= "";
  	this.formDatosEstudiante=false;
  	this.encontradoAuxiliar=false;
	this.encontradoEstudiante=false;
  	for (var i = 0; i < this.listaAuxiliares.length; i++) {
  		if (this.listaAuxiliares[i].carnet == buscarForm.value.carnet) {
  			this.encontradoAuxiliar=true;
  			console.log(this.encontradoAuxiliar);
  			this.mensajeAlertaFuncion("El usuario ya esta asignado en la lista de auxiliares", "Atencion");
  			break;
  		}
  	}
  	if (this.encontradoAuxiliar==false) {
  		for (var i = 0; i < this.listaEstudiantes.length; i++) {
  			if (this.listaEstudiantes[i].carnet == buscarForm.value.carnet) {
  				this.encontradoEstudiante=true;
  				console.log(this.encontradoEstudiante);
  				this.mensajeAlertaFuncion("", "Usuario encontrado");
  				this.carnetEstudiante= this.listaEstudiantes[i].carnet;
  				this.nombreEstudiante= this.listaEstudiantes[i].nombre;
  				this.apellidoEstudiante= this.listaEstudiantes[i].apellido;
  				this.emailEstudiante= this.listaEstudiantes[i].email;
  				this.formDatosEstudiante=true;
  			}
  		}
  	}
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
  //Listado de usuarios
  listarUsuarios(){
  	this._servicio.listarUsuarios().subscribe(
  		res=>{
  			this.listaEstudiantes = res;
  			console.log(this.listaEstudiantes);
  		});
  }

  ////******************************METODO PARA AGREGAR AUXILIAR*******************************////
  cambiarAuxiliar(){
  	this._servicio.agregarAuxiliar(this.carnetEstudiante).subscribe(
  		res =>{  			
  			this.mensajeAlertaFuncion("Se añadio a la lista de auxiliares", "Atencion");
  			this.carnetEstudiante= "";
  			this.nombreEstudiante= "";
  			this.apellidoEstudiante= "";
  			this.emailEstudiante= "";
  			this.listarAuxiliares();
  			this.listarUsuarios();
  			this.formDatosEstudiante=false;
  		},(error)=>{
  			console.error(error);
  		}
  	);
  }



  ////*********************ELIMINAR AUXILIAR EXISTENTE*************************************///
  eliminarAuxiliar(carnet: string){
  	this._servicio.eliminarAuxiliar(carnet).subscribe(
  		res=>{
  			this.mensajeAlertaFuncion("Se elimino este auxiliar de la lista", "Atencion");
			this.listarAuxiliares();
  			this.listarUsuarios();
  		});
  }

}
