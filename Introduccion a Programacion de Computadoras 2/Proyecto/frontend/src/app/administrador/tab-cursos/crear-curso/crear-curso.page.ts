import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Servicio } from '../../../servicio';
import { AlertController } from '@ionic/angular';

@Component({
  selector: 'app-crear-curso',
  templateUrl: './crear-curso.page.html',
  styleUrls: ['./crear-curso.page.scss'],
})
export class CrearCursoPage implements OnInit {
  listaCursos;
  editarForm = false;

  constructor(private _servicio: Servicio, private _alert: AlertController) { }

  ngOnInit() {
    this.editarForm = false;
    this.listarCursos()
  }

  ///************AÑADIR CURSO A LA BASE DE DATOS******************//
  crearCurso(curso: NgForm): void{
  	if (curso.value.codigo && curso.value.nombre) {
      if (isNaN(curso.value.codigo)) {
        this.alertaRegistroFuncion("El codigo debe ser numerico", "ERROR")
      } else{
        this._servicio.AgregarCurso(curso.value.codigo, curso.value.nombre).subscribe(
      res=>{
        this.alertaRegistroFuncion("Se ha registrado correctamente el curso", "Registro Exitoso");
        curso.reset();
        this.listarCursos();
      }, (error)=>{
      });
      }
    } else{
      this.alertaRegistroFuncion("Debe ingresar todos los datos", "ERROR");
    };
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

  ///************* EDICION DE CURSO **************************///
  Ecodigo = ""; //Variable que guardara dato codigo de curso
  Enombre = ""; //Variable que guardara dato nombre de curso
  mostrarFormEdicion(codigo: string, nombre: string){
    this.Ecodigo = codigo;
    this.Enombre = nombre;
    this.editarForm = true;
  }

  editarCurso(codigo: string, nombre: string){
    this._servicio.editarCurso(codigo, nombre).subscribe(
      res =>{
        this.listarCursos();
        this.alertaRegistroFuncion("Se ha actualizado el curso correctamente", "Curso actualizado");
        this.editarForm = false;
      }, (error) =>{
        console.error(error);
      });
  }

  cancelarEdicion(){
    this.editarForm = false;
  }

  ///********************* ELIMINAR CURSO ***********************///
  eliminarCurso(codigo: string){
    this._servicio.eliminarCurso(codigo).subscribe(
      res =>{
        this.listarCursos();
        this.alertaRegistroFuncion("Se ha eliminado el curso correctamente", "Curso Eliminado");        
      }, (error) => {
        console.error(error);
      });
  }

}
