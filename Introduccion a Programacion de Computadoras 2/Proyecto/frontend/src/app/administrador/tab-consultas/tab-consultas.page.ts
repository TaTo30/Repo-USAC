import { Component, OnInit } from '@angular/core';
import { Servicio } from '../../servicio';
import { Router } from  '@angular/router';

@Component({
  selector: 'app-tab-consultas',
  templateUrl: './tab-consultas.page.html',
  styleUrls: ['./tab-consultas.page.scss'],
})
export class TabConsultasPage implements OnInit {

  constructor(private _servicio: Servicio, private _router: Router) { }

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

  actualizarTicket(id: string, estado:string){
    this._servicio.acutalizarTickets(estado, id).subscribe(
      res =>{
        console.log("actualizadoTicket");
        this._servicio.listarTickets().subscribe(
        res =>{
          this.datosTickets = res;
        });
      });
  }

  datosAsignados;
  datosEvaluaciones;
  datosPublicaciones;
  datosTickets;
  publicacionesList=false;
  evaluacionesList=false;
  asignadosList=false;
  ticketList=false
  consultar(tipo: string){
    if (tipo == 'Asignaciones') {
      this._servicio.listarSeccionesAsginar().subscribe(
      res =>{
        this.datosAsignados = res;
        this.asignadosList=true;
        this.evaluacionesList=false;
        this.publicacionesList=false;
        this.ticketList=false
        console.log(this.datosAsignados);
      });
    }else if (tipo == 'Evaluaciones') {
      this._servicio.listarEvaluaciones().subscribe(
        res =>{
          this.datosEvaluaciones = res;
          this.publicacionesList=false;
          this.evaluacionesList=true;
          this.asignadosList=false;
          this.ticketList=false
          console.log(this.datosEvaluaciones);
        });
    }else if (tipo == 'Publicaciones') {
      this._servicio.listarMensajesForos().subscribe(
        res =>{
          this.datosPublicaciones = res;
          this.publicacionesList=true;
          this.evaluacionesList=false;
          this.asignadosList=false;
          this.ticketList=false
          console.log(this.datosPublicaciones);
        });
    }else if (tipo == 'Ticket'){
      this._servicio.listarTickets().subscribe(
        res =>{
          this.datosTickets = res;
          this.publicacionesList=false;
          this.evaluacionesList=false;
          this.asignadosList=false;
          this.ticketList=true
        });
    }
  }

}
