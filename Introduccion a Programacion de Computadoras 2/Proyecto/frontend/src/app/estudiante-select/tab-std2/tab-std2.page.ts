import { Component, OnInit } from '@angular/core';
import { Servicio } from '../../servicio';
import { DatePipe } from '@angular/common';
import { Router } from	'@angular/router';


@Component({
  selector: 'app-tab-std2',
  templateUrl: './tab-std2.page.html',
  styleUrls: ['./tab-std2.page.scss'],
})
export class TabStd2Page implements OnInit {
  cursoSeleccionado=false;
  constructor(private _servicio:Servicio, private _router: Router) { }

  ngOnInit() {
    this.obtenerDatosLogin();
    this.ListarAsignados();   
  }

  //*******************METODOS PARA LA RECUPERACION Y LISTADO DE DATOS*******************//
  //recuperacion de datos usuario
  datosUsuario;//variable que alamacen los datos del usuario logeado  
  obtenerDatosLogin(){
    this._servicio.GetData().subscribe(
      res => {
        this.datosUsuario = res;
        console.log(this.datosUsuario);
      }, (error) => {
        console.error(error);
      });
  }
  //ListarAsignados
  listaAsginados; //variable que almacena los estudiantes inscritos a una seccion
  ListarAsignados(){
    this._servicio.listarSeccionesAsginar().subscribe(
      res=>{
        this.listaAsginados = res;
        console.log(this.listaAsginados);
      }, (error) =>{
        console.error(error);
      });
  }

  //**************************METODOS PARA EL GUARDADO E INGRESO DE CURSOS******************//
  ingresarCurso(seccion: string){
    this.cursoSeleccionado = true;
    this._servicio.SaveSeccionData(seccion);
    this._router.navigate(['estudiante']);
  }




}
