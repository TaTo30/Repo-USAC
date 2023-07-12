import { Component, OnInit, ɵConsole } from '@angular/core';
import { NgForm } from "@angular/forms";
import { ServerConnectService } from "../servicio/server-connect.service";
import { AppComponent } from "../app.component";
import { Router } from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private servicio: ServerConnectService, private app: AppComponent, private router: Router) { }

  ngOnInit(): void {
  }

  Login(log: NgForm){
    //console.log(log.value);
    let user = log.value.correo;
    let pass = log.value.pass;
    if (user == 'Administrador' && pass == 'Administrador') {
      this.router.navigate(['administrador']);
      this.servicio.saveUser(0, 'Administrador', 'Administrador','','','','',0);
      this.app.loginadmin();
      log.reset();
      this.router.navigate(['administrador']);
    } else {
      this.servicio.logear(user, pass).subscribe(res =>{
        if (res[0] != undefined) {   
          console.log(res[0]);
          this.servicio.GetCliente(res[0][0]); 
          alert("Has ingresado como usuario");
          this.app.loginuser();
          this.router.navigate(['perfil']);
          log.reset();
        } else {
          alert("Usuario o contraseña incorrecto");
        }
      }, err => {
        alert("Ha ocurrido un error" + err);
      });
    }
  }

}
