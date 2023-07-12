import { Component, OnInit } from '@angular/core';
import { Servicio } from '../../servicio';
import { Router } from	'@angular/router';

@Component({
  selector: 'app-tab-asignar',
  templateUrl: './tab-asignar.page.html',
  styleUrls: ['./tab-asignar.page.scss'],
})
export class TabAsignarPage implements OnInit {


  constructor(private _AdminServicio:Servicio, private _router:Router) { }

  ngOnInit() {

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
