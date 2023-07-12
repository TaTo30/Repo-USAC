import { Component, OnInit } from '@angular/core';
import { ServerConnectService } from "../servicio/server-connect.service";
import { NgForm } from "@angular/forms";
import { Publicacion } from "../models/publicacion";
import { Producto } from "../models/producto";
import { User } from "../models/user";

@Component({
  selector: 'app-administrador',
  templateUrl: './administrador.component.html',
  styleUrls: ['./administrador.component.css']
})
export class AdministradorComponent implements OnInit {

  ADMIN_SET_MODE: number = 0;
  PRODUCTOS_LIST: Array<Producto> = []
  USER_LIST: Array<User> = []
  PUBLICACION_LIST: Array<Publicacion> = []


  constructor(private servicio: ServerConnectService) { }

  ngOnInit(): void {
    this.obtenerCategorias();
  }


  categorias_list;
  
  agregarCategoria(set: NgForm){
    this.servicio.SetCategorias(set.value.categoria).subscribe(res => {
      this.obtenerCategorias();
      set.reset();
    }, error => {
      console.error(error);
      
    })
  }

  obtenerCategorias(){
    this.servicio.GetCategorias().subscribe(res => {
      this.categorias_list = res;        
    }, error =>{
      console.error(error);
      
    })
  }

  obtenerProductosMasVendidos(){
    this.servicio.GetProductosVendidos().subscribe(res => {
      this.PRODUCTOS_LIST.splice(0, this.PRODUCTOS_LIST.length)
      for (const i of <any>res) {
        this.PRODUCTOS_LIST.push(new Producto(i[1], i[2], i[0], '',0,0,'',''))
      }
    }, error => {
      console.error(error);
      
    })
  }

  obtenerProductosLikes(){
    this.servicio.GetProductosLikes().subscribe(res => {
      this.PUBLICACION_LIST.splice(0, this.PUBLICACION_LIST.length)
      for (const i of <any>res) {
        let user = new User(0,i[2], i[3],'','','','',0)
        let producto =  new Producto(0,i[0],0,'',i[1],0,'','')
        this.PUBLICACION_LIST.push(new Publicacion(user, producto))
      }
    })
  }

  obtenerProductosDislikes(){
    this.servicio.GetProductosDislikes().subscribe(res => {
      this.PUBLICACION_LIST.splice(0, this.PUBLICACION_LIST.length)
      for (const i of <any>res) {
        let user = new User(0,i[2], i[3],'','','','',0)
        let producto =  new Producto(0,i[0],0,'',0,i[1],'','')
        this.PUBLICACION_LIST.push(new Publicacion(user, producto))
      }
    })
  }

  obtenerClientesCreditos(){
    this.servicio.GetCreditosClientes().subscribe(res => {
      this.USER_LIST.splice(0, this.USER_LIST.length)
      for (const i of <any>res) {
        this.USER_LIST.push(new User(i[0], i[1], i[2], i[3], i[4], '', '', i[5]))
      }
    })
  }

  obtenerPaisesCreditos(){
    this.servicio.GetCreditosPaises().subscribe(res => {
      this.PRODUCTOS_LIST.splice(0, this.PRODUCTOS_LIST.length)
      for (const i of <any>res) {
        this.PRODUCTOS_LIST.push(new Producto(i[1], i[0], 0, '', i[2], i[3], '', ''))
      }
    })
  }

  obtenerClientesVentas(){
    this.servicio.GetClientesVentas().subscribe(res => {
      this.USER_LIST.splice(0, this.USER_LIST.length)
      for (const i of <any>res) {
        this.USER_LIST.push(new User(i[0], i[1], i[2], i[3], '', '', '', i[4]))
      }
    })
  }

  obtenerClientesComentarios(){
    this.servicio.GetClientesComentarios().subscribe(res => {
      this.USER_LIST.splice(0, this.USER_LIST.length)
      for (const i of <any>res) {
        this.USER_LIST.push(new User(i[3], i[0], i[1], '', i[2], '', '', 0))
      }
      console.log(this.USER_LIST);
      
    })
  }

  setMode(value: number){
    switch (value) {
      case 0:
        this.obtenerCategorias();
        break;
      case 1:
        this.obtenerProductosMasVendidos();
      break;
      case 2:
        this.obtenerProductosLikes();
      break;
      case 3:
        this.obtenerProductosDislikes();
      break;
      case 4:
        this.obtenerClientesCreditos();
      break;
      case 5:
        this.obtenerPaisesCreditos();
      break;
      case 6:
        this.obtenerClientesVentas()
      break;
      case 7:
        this.obtenerClientesComentarios()
      break;
      default:
      break;
    }
    this.ADMIN_SET_MODE = value;
  }
}
