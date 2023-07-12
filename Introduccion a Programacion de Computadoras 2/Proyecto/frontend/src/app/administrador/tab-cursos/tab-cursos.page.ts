import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Servicio} from '../../servicio';
import { Router } from '@angular/router';


@Component({
  selector: 'app-tab-cursos',
  templateUrl: './tab-cursos.page.html',
  styleUrls: ['./tab-cursos.page.scss'],
})
export class TabCursosPage implements OnInit {


  constructor(private _router : Router) { }

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
