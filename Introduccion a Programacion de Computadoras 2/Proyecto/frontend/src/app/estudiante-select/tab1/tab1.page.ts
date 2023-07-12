import { Component, OnInit } from '@angular/core';
import { Servicio } from '../../servicio';

@Component({
  selector: 'app-tab1',
  templateUrl: './tab1.page.html',
  styleUrls: ['./tab1.page.scss'],
})
export class Tab1Page implements OnInit {

  constructor(private _servicio: Servicio) { }

  ngOnInit() {
  	this.obtenerDatosLogin();
  	this.listarTicket();
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


  enviarTicket(asunto: string, queja: string){
  	this._servicio.agregarTickets(asunto, queja, this.datosUsuario[0].carnet).subscribe(
  		res =>{
  			this.listarTicket();
  		}, (error) => {
  			console.error(error);
  		});
  }


  datosTickets;
  listarTicket(){
  	this._servicio.listarTickets().subscribe(
  		res =>{
  			this.datosTickets = res;
  			console.log(this.datosTickets);
  		}, (error) =>{
  			console.error(error);
  		});
  }

}
