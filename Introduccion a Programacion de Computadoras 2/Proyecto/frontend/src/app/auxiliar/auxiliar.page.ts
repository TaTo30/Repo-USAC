import { Component, OnInit } from '@angular/core';
import { Servicio } from '../servicio';
import { DatePipe } from '@angular/common';
import { Router } from	'@angular/router';


@Component({
  selector: 'app-auxiliar',
  templateUrl: './auxiliar.page.html',
  styleUrls: ['./auxiliar.page.scss'],
})
export class AuxiliarPage implements OnInit {
	cursoSeleccionado=false;

  constructor(private _servicio: Servicio, private _router: Router) { }

  ngOnInit() {
    this.obtenerDatosLogin();
    this.listarSecciones();
  }

  //MANEJO DE ROUTING
  paginaInicio(){
    this._router.navigate(['home']);
  }

  //*******************METODOS PARA LA RECUPERACION Y LISTADO DE DATOS*******************//
  datosUsuario;//variable que alamacen los datos del usuario logeado
  listaSeccionesAuxiliar;//variable que almacena listado de cursos con auxiliar
  //recuperacion de datos usuario
  obtenerDatosLogin(){
    this._servicio.GetData().subscribe(
      res => {
        this.datosUsuario = res;
        console.log(this.datosUsuario);
      }, (error) => {
        console.error(error);
      });
  }
  //Listado de los cursos con auxiliar
  listarSecciones(){
    this._servicio.listarSeccionesAuxiliar().subscribe(
      res => {
        this.listaSeccionesAuxiliar = res;
        console.log(this.listaSeccionesAuxiliar);
      }, (error) =>{
        console.error(error);
      });
  }

  //**************************METODOS PARA EL GUARDADO E INGRESO DE CURSOS******************//
  ingresarCurso(seccion: string){
    this.cursoSeleccionado = true;
    this._servicio.SaveSeccionData(seccion);
  }


}
