import { Component, OnInit } from '@angular/core';
import { Servicio } from '../../../servicio';
import { AlertController } from '@ionic/angular';

@Component({
  selector: 'app-editar-curso',
  templateUrl: './editar-curso.page.html',
  styleUrls: ['./editar-curso.page.scss'],
})
export class EditarCursoPage implements OnInit {
  listaCursos;
  listaSecciones;

  constructor(private _servicio: Servicio, private _alert: AlertController) { }

  ngOnInit() {
  	this.listarCursos();
  	this.listarSecciones();
  }


  ///************* MENSAJES DE ALERTA **********************///
  async alertaRegistroFuncion(mensaje: string, titulo: string){
    const alertaRegistro = await this._alert.create({
      header: titulo,
      message: mensaje,
      buttons: ['OK']
    });
    await alertaRegistro.present();
  }

  ///*********** LLAMADA DE TODOS LOS CURSOS *******************///
  listarCursos(){
    this._servicio.listarCursos().subscribe(
        res=>{
          this.listaCursos = res;
          console.log(this.listaCursos);
        }, (error) => {
          console.error(error);
        }
      );
  }
  
  ///*************LLAMADA DE TODAS LAS SECCIONES**************///
  listarSecciones(){
  	this._servicio.listarSecciones().subscribe(
  		res=>{
  			this.listaSecciones=res;
  			console.log(this.listarSecciones);
  		}, (error) => {
  			console.error(error);
  		});
  }

  ///*********** DESPLIEGE DE INFORMACION PARA SECCION*****************///
  agregarSeccionForm = false; //Variable que despliega el formulario
  codigo="";//Variable que almacena el codigo seleccionado
  nombre="";//Variable que almacena el nombre seleccionado
  seleccionarCurso(id, nombre){
  	this.codigo= id;
  	this.nombre=nombre;
  	this.agregarSeccionForm = true;
  }

  agregarSeccion(codigo:string, nombre:string, horario1:string, horario2:string, seccion:string, semestre:string, ciclo:string){
  	var horario = horario1+" - "+horario2;
    var seccionUpper = this.upperSeccion(seccion);    
  	this._servicio.agregarSeccion(codigo, nombre, horario, seccionUpper, semestre, ciclo).subscribe(
  		res =>{
  		  if (res == 0) {
           	this.alertaRegistroFuncion("La seccion que intenta agregar ya existe", "ERROR");
          } else{
          	this.alertaRegistroFuncion("","Seccion Agregada");
          	this.listarSecciones();
          	this.listarCursos();
          	this.agregarSeccionForm=false;
          }
  		}, (error)=>{
  			console.error(error);
  		});
  }

  cancelarEdicion(){
  	this.agregarSeccionForm=false;
  }

  eliminarSeccion(codigo: string){
  	this._servicio.eliminarSeccion(codigo).subscribe(
  		res=>{
  			this.alertaRegistroFuncion("","Seccion Eliminada");
  			this.listarSecciones();
  		}, (error) =>{
  			console.error(error);
  		});
  }

  //Metodo que devuelve la seccion siempre en mayusculas
  upperSeccion(seccion:string){
    return seccion.toUpperCase();
  }


  ///*********** DETERMINANTE DE HORA SALID **************************///
  secondHoraView = 0;
  selectP1(){
  	this.secondHoraView=0;
  }
  selectP2(){
  	this.secondHoraView=1;
  }
  selectP3(){
  	this.secondHoraView=2;
  }
  selectP4(){
  	this.secondHoraView=3;
  }
  selectP5(){
  	this.secondHoraView=4;
  }
  selectP6(){
  	this.secondHoraView=5;
  }


}
