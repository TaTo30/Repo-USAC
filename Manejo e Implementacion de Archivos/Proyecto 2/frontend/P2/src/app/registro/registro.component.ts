import { Component } from '@angular/core';
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { ServerConnectService } from "../servicio/server-connect.service";

@Component({
  selector: 'app-registro',
  templateUrl: './registro.component.html',
  styleUrls: ['./registro.component.css']
})
export class RegistroComponent {

  constructor(private servidor: ServerConnectService, private router: Router) { }


  Registro(registro: NgForm): void{
    console.log(registro.value);
    this.servidor.registrar(registro.value.nombre, registro.value.apellido, registro.value.correo, registro.value.pass, registro.value.cpass, registro.value.date, registro.value.pais, "NaN").subscribe(res => {
      console.log(res);
      alert("Registro Completado");
      this.router.navigate(['login']);
    }, err => {
      console.log(err);
      alert("Ha ocurrido un error " + err);
    });
    registro.resetForm();
  }

}
