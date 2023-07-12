import { Component } from '@angular/core';
import { ServerConnectService } from "./servicio/server-connect.service";
import { OnInit } from "@angular/core";
import { User } from "./models/user";


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  user: User

  constructor(private servicio: ServerConnectService){ }

  ngOnInit(){
    if (this.servicio.GetLog() == 'true') {
      this.ObtenerDatosUsuario();
      if (this.user.nombre == 'Administrador' && this.user.apellido == 'Administrador') {
        this.loguser = 2;
      }else{
        this.loguser = 1;
      }
    }
  }

  loguser = 0;

  title = 'P2';

  ObtenerDatosUsuario(){
    this.user = this.servicio.GetUser();
  }

  loginuser(){
    this.loguser = 1;
    this.servicio.TrueLog();
    this.ObtenerDatosUsuario();
  }

  loginadmin(){
    this.loguser = 2;
    this.servicio.TrueLog();
  }

  deslogin(){
    this.loguser = 0;
    this.servicio.FalseLog();
    this.servicio.deleteUser();
  }
}
