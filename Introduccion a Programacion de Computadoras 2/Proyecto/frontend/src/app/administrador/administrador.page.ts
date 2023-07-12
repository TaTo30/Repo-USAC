import { Component, OnInit } from '@angular/core';
import { Router } from	'@angular/router';

@Component({
  selector: 'app-administrador',
  templateUrl: './administrador.page.html',
  styleUrls: ['./administrador.page.scss'],
})
export class AdministradorPage implements OnInit {

  constructor(private _router: Router) { }

  ngOnInit() {
  console.log("se inicio esta madre");
  }


  paginaAuxiliares(){
  	this._router.navigate(['administrador/auxiliares']);
  }
  paginaAsignaciones(){
  	this._router.navigate(['administrador/asignaciones']);
  }
  paginaCursos(){
  	this._router.navigate(['administrador/cursos']);
  }
  paginaConsultas(){
    this._router.navigate(['administrador/consultas']);
  }

  paginaInicio(){
    this._router.navigate(['home']);
  }

}
