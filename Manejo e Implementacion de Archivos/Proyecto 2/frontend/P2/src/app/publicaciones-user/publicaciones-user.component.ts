import { Component, OnInit, SimpleChange, SimpleChanges } from '@angular/core';
import { ServerConnectService } from "../servicio/server-connect.service";
import { NgForm } from "@angular/forms";
import { User } from "../models/user";
import { Producto } from "../models/producto";
import { Publicacion } from "../models/publicacion";

@Component({
  selector: 'app-publicaciones-user',
  templateUrl: './publicaciones-user.component.html',
  styleUrls: ['./publicaciones-user.component.css']
})
export class PublicacionesUserComponent implements OnInit {

  user: User;
  publicacion: Publicacion = new Publicacion(new User(0,'','','','','','',0), new Producto(0,'',0,'',0,0,'',''))
  lista_publicaciones: Array<Publicacion> = []

  constructor(private servicio: ServerConnectService) { }

  menuMode = 0;

  categorias_list;

  ngOnInit(): void {  
    this.obtenerDatosUsuario();
    this.obtenerPublicaciones();
  }

  updateChar(changes: SimpleChanges){
    console.log(changes);  
  }

  obtenerCategorias(){
    this.servicio.GetCategorias().subscribe(res => {
      this.categorias_list = res;        
    }, error =>{
      console.error(error);
    })
  }

  obtenerPublicaciones(){
    this.servicio.GetPublicaciones().subscribe(res => {
      this.lista_publicaciones.splice(0, this.lista_publicaciones.length)  
      for (const p of <any>res) {
        let user = new User(p[10], p[11], p[12], p[13], p[14], p[15], p[16], p[17]);
        let producto = new Producto(p[0], p[2], p[3], p[9], p[5], p[6], p[7], p[8]);
        this.lista_publicaciones.push(new Publicacion(user, producto));      
      }
    }, error => {
      console.error(error);      
    })
  }

  crearPublicacion(set: NgForm){
    this.servicio.SetPublicacion(this.user.id, set.value.nombre, set.value.precio, set.value.categoria, set.value.descripcion, set.value.tags).subscribe(res => {
      alert("Publicacion creada");
      window.location.reload();
    }, error => {
      console.error(error);
    });
  }



  obtenerDatosUsuario(){
    this.user = this.servicio.GetUser();
  }

  setMenuMode(mode: number){
    this.menuMode = mode;
  }

}
