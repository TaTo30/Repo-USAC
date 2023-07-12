import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Servicio } from '../servicio';
import { AlertController } from '@ionic/angular';

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage {
  registro=false;

  constructor(private _router: Router, private _servicio: Servicio, private _alert: AlertController) {}

//LOGIN 
  iniciarSesion(logear: NgForm): void{ 
    if (logear.value.carnet && logear.value.pass) {
      if (logear.value.carnet=="Admin" && logear.value.pass =="Admin201800585") {
        this._router.navigate(['administrador/cursos']);
      }else{
        this._servicio.login(logear.value.carnet, logear.value.pass).subscribe(
          res=>{
            console.log(res);
            if (res==0) {
              this._router.navigate(['estudiante_select']);
              this._servicio.SaveData(logear.value.carnet, logear.value.pass);
            } else {
              if (res==1) {
                this._router.navigate(['auxiliar']);
                this._servicio.SaveData(logear.value.carnet, logear.value.pass);
              } else {
                alert("USUARIO O CONTRASEÑA NO VALIDOS");
              }
            }
          },(error)=>{
          	console.error(error);
          });      
      }
    }else{
     alert("DEBE LLENAR TODOS LOS CAMPOS");
    }
  }

//REGISTRO
  async registar(registar: NgForm){
    this._servicio.registro(registar.value.carnet, registar.value.nombre, registar.value.apellido, registar.value.email, registar.value.pass).subscribe(
      res=>{
        if (res == 0) {
          this.alertaRegistroFuncion();
          registar.reset();
        }
      }
      );
  }

//MENSAJE DE ALERTA
  async alertaRegistroFuncion(){
    const alertaRegistro = await this._alert.create({
      header: 'Registro Exitoso',
      buttons: ['OK']
    });
    await alertaRegistro.present();
  }

//MANEJO DE FORMULARIOS
  formRegistro(){
    this.registro=true;
  }

  formLogin(){
    this.registro=false;
  }

}
