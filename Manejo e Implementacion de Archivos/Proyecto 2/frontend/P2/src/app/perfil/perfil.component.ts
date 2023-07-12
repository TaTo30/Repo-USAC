import { Component, OnInit } from '@angular/core';
import { ServerConnectService } from "../servicio/server-connect.service";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { User } from '../models/user';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.css']
})
export class PerfilComponent implements OnInit {

  user: User;

  constructor(private servicio: ServerConnectService, private router: Router) { }

  ngOnInit(): void {
    this.ObtenerDatosUser()
  }

  ObtenerDatosUser(){
    this.user = this.servicio.GetUser();
  }

  ActualizarDatos(update: NgForm){
    console.log(update.value)
    this.servicio.ActualizarPerfil(update.value.pass, update.value.cpass, update.value.nac, update.value.pais, this.user.id.toString(), update.value.nombre, update.value.apellido).subscribe(res =>{
      this.servicio.GetCliente(this.user.id)
      alert("Datos Actualizados");
      this.ObtenerDatosUser()
    }, err =>{
      alert("Ha ocurrido un error");
      console.log(err);
    });
  }
  

}
