import { Component, OnInit } from '@angular/core';
import { Servicio } from '../../../servicio';
import { NgForm } from '@angular/forms';
import { AlertController } from '@ionic/angular';

@Component({
  selector: 'app-editar-auxiliar',
  templateUrl: './editar-auxiliar.page.html',
  styleUrls: ['./editar-auxiliar.page.scss'],
})
export class EditarAuxiliarPage implements OnInit {
	listaAuxiliares;
	formEditar = false;

  ///******************METODO PARA DESPLIEGE DE MENSAJES**********************************///
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


  carnetUsuario="";
  nombreUsuario="";
  apellidoUsuario="";
  emailUsuario="";

  editarForm(carnet: string){
  	for (var i = 0; i < this.listaAuxiliares.length; i++) {
  		if (this.listaAuxiliares[i].carnet == carnet) {
  			this.carnetUsuario=this.listaAuxiliares[i].carnet;
  			this.nombreUsuario=this.listaAuxiliares[i].nombre;
  			this.apellidoUsuario=this.listaAuxiliares[i].apellido;
  			this.emailUsuario=this.listaAuxiliares[i].email;
  			this.formEditar=true;
  		}
  	};
  }
  editarAuxiliar(carnet:string, nombre:string, apellido:string, email:string){
  	this._servicio.editarAuxiliar(carnet, nombre, apellido, email).subscribe(
  		res=>{
  			this.mensajeAlertaFuncion("Los datos del usuario han sido actualizados", "Atencion");
			this.formEditar=false;
  			this.listarAuxiliares();
  			this.carnetUsuario="";
  			this.nombreUsuario="";
  			this.apellidoUsuario="";
  			this.emailUsuario="";
  		}, (error)=>{

  			console.error(error);
  		});
  }
}
