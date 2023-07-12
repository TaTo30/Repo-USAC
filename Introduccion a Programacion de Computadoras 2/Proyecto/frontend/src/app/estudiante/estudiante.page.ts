import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Servicio } from '../servicio';
import { Router } from '@angular/router';

@Component({
  selector: 'app-estudiante',
  templateUrl: './estudiante.page.html',
  styleUrls: ['./estudiante.page.scss'],
})
export class EstudiantePage implements OnInit {
  
  

  constructor(private _servicio:Servicio, private _router: Router) { }

  ngOnInit() {

  }
  //MANEJO DE ROUTING
  paginaInicio(){
    this._router.navigate(['home']);
  }


}
