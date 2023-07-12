import { Component, OnInit } from '@angular/core';
import { Servicio } from '../../servicio';
import { Router } from	'@angular/router';

@Component({
  selector: 'app-tab-std1',
  templateUrl: './tab-std1.page.html',
  styleUrls: ['./tab-std1.page.scss'],
})
export class TabStd1Page implements OnInit {

	listaSecciones2;
	estudianteData:object;
	listaAsignaciones;
	showTable=0;

  constructor(private _servicio: Servicio, private _router:Router) { }

  ngOnInit() {
    this.obtenerDatosLogin(); 	
  }

  //*******************METODOS PARA LA RECUPERACION Y LISTADO DE DATOS*******************//
  //recuperacion de datos usuario
  datosUsuario;//variable que alamacen los datos del usuario logeado
  obtenerDatosLogin(){
    this._servicio.GetData().subscribe(
      res => {
        this.datosUsuario = res;
        this.listarSeccionesSinAsignar(this.datosUsuario[0].carnet);
        console.log(this.datosUsuario);
      }, (error) => {
        console.error(error);
      });
  }
  //ListarAsignados
  listaSecciones;//variable que almacena la lista de secciones
  listarSeccionesSinAsignar(carnet: string){
    this._servicio.listarSeccionesSinAsignar(carnet).subscribe(
      res=>{
        this.listaSecciones = res;
        console.log(this.listaSecciones);
      }, (error) =>{
        console.error(error);
      });
  }


  //***************************ASIGNACION DE CURSO SELECCIONADO***************************//
  asignarCurso(idSeccion: string){
    this._servicio.asignarCurso(idSeccion, this.datosUsuario[0].carnet).subscribe(
      res => {
        this.listarSeccionesSinAsignar(this.datosUsuario[0].carnet);
        console.log("asignado");
      }, (error) => {
        console.error(error);
      });
  }


}
