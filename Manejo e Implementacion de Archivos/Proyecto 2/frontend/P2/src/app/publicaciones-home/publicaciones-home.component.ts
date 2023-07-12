import { Component, OnInit } from '@angular/core';
import { ServerConnectService } from "../servicio/server-connect.service";
import { NgForm } from "@angular/forms";
import { DatePipe } from '@angular/common';
import { User } from "../models/user";
import { Producto } from "../models/producto";
import { Publicacion } from "../models/publicacion";


@Component({
  selector: 'app-publicaciones-home',
  templateUrl: './publicaciones-home.component.html',
  styleUrls: ['./publicaciones-home.component.css']
})
export class PublicacionesHomeComponent implements OnInit {

  user: User
  publicacion: Publicacion = new Publicacion(new User(0,'','','','','','',0), new Producto(0,'',0,'',0,0,'',''))
  lista_publicaciones: Array<Publicacion> = []

  constructor(private servicio: ServerConnectService, private datepipe: DatePipe) { }

  ngOnInit(): void {
    this.ObtenerDatosUsuario();
    this.ObtenerPublicaciones();
  }

  lista_categorias;
  lista_comentario;

  modalmode = true;
  likeSet = false;
  DislikeSet = false;
  modalType = "modal-xl"
  texto = "";


  ObtenerDatosUsuario(){
    this.user = this.servicio.GetUser();    
  }

  changeMode(){
    if (this.user != null) {
      if (this.user.id != 0) {
        if (this.modalmode) {
          this.modalmode = false;
          this.modalType = "modal-md"
        } else {
          this.modalmode = true;
          this.modalType = "modal-xl"
        }
        this.ObtenerComentario()
      } else {
        alert("Solo usuarios pueden hacer esta accion");
      }
    } else {
      alert("Necesitas estar logeado para hacer esta accion");
    }
  }

  Comentar(){
    this.servicio.SetComentario(this.user.id, this.publicacion.Producto.id, this.datepipe.transform(new Date(),"yyyy-MM-dd"), this.texto).subscribe(res => {
      this.texto = ""
      this.ObtenerComentario()
    }, error => {
      alert("Ha ocurrido un Error");
    })  
  }

  Denunciar(){
    this.servicio.SetDenuncia(this.publicacion.Producto.id, this.texto).subscribe(res => {
      this.texto = ""
      alert("La ha publicacion se ha enviado a revisar");
    }, error => {
      alert("Ha ocurrido un Error");
    })      
  }

  ObtenerComentario(){
    this.servicio.GetComentario().subscribe(res => {
      this.lista_comentario = res
    }, error => {
      console.error(error);
    })
  }


  AgregarCarrito(){
    if (this.user != null) {
      if (this.user.id != 0) {
        this.servicio.setDetalleCarrilt(this.user.id, this.publicacion.Producto.id).subscribe(res => {
          alert("Añadido al carrito de compras");
        }, error => {
          alert("Ha ocurrido un Error");
        }) 
      } else {
        alert("Solo usuarios pueden hacer esta accion");
      }
    }else{
      alert("Necesitas estar logeado para hacer esta accion");
    }
  }


  ObtenerCategorias(){
    this.servicio.GetCategorias().subscribe(res => {
      this.lista_categorias = res;
    }, error => {
      console.error(error);
    })
  }

  ObtenerPublicaciones(){
    this.servicio.GetPublicaciones().subscribe(res => {
      this.lista_publicaciones.splice(0, this.lista_publicaciones.length)  
      for (const p of <any>res) {
        let user = new User(p[10], p[11], p[12], p[13], p[14], p[15], p[16], p[17]);
        let producto = new Producto(p[0], p[2], p[3], p[9], p[5], p[6], p[7], p[8]);
        this.lista_publicaciones.push(new Publicacion(user, producto));      
      }       
      this.ObtenerCategorias();
    }, error => {
      console.error(error);
    })
  }

  FiltrarPublicaciones(set: NgForm){
    this.servicio.GetPublicacionesFiltro(set.value.busqueda, set.value.categoria, set.value.orden).subscribe(res => {
      this.lista_publicaciones.splice(0, this.lista_publicaciones.length)
      for (const p of <any>res) {
        let user = new User(p[10], p[11], p[12], p[13], p[14], p[15], p[16], p[17]);
        let producto = new Producto(p[0], p[2], p[3], p[9], p[5], p[6], p[7], p[8]);
        this.lista_publicaciones.push(new Publicacion(user, producto));      
      } 
    }, error => {
      console.error(error);
    });
  }

  SetPublicacionENV(data: Publicacion){
    this.publicacion = data;
    this.ObtenerReacciones();
  }

  ObtenerReacciones(){
    this.servicio.GetReaccion(this.user.id, this.publicacion.Producto.id).subscribe(res => {
      let respuesta = <any>res;
      console.log(respuesta.length);
      console.log(respuesta[0]);
      if (respuesta.length == 1) {
        if (respuesta[0][2] == 1) {
          this.likeSet = true
        } else {
          this.likeSet = false
        }
        if (respuesta[0][3] == 1) {
          this.DislikeSet = true
        } else {
          this.DislikeSet = false
        }
      }else{
        this.likeSet = false
        this.DislikeSet = false
      }
      
    }, err => {
      console.error(err);
    })
  }

  DarLike(){
    if (!this.likeSet) {
      if (!this.DislikeSet) {
        //0-0
        this.servicio.SetReaccion(this.user.id, this.publicacion.Producto.id, 1, 0).subscribe(response => {
          this.PostReaccion()
        }, error => {
          alert("Ha ocurrido un error")
        })
      } else {
        //0-1
        this.servicio.SetReaccion(this.user.id, this.publicacion.Producto.id, 1, -1).subscribe(response => {
          this.PostReaccion()
        }, error => {
          alert("Ha ocurrido un error")
        })
      }
    } else {
      if (!this.DislikeSet) {
        //1-0
        this.servicio.SetReaccion(this.user.id, this.publicacion.Producto.id, -1, 0).subscribe(response => {
          this.PostReaccion()
        }, error => {
          alert("Ha ocurrido un error")
        })
      } else {
        //1-1
        //caso imposible de llegar
      }
    }
  }

  DarDislike(){
    if (!this.likeSet) {
      if (!this.DislikeSet) {
        //0-0
        this.servicio.SetReaccion(this.user.id, this.publicacion.Producto.id, 0, 1).subscribe(response => {
          this.PostReaccion()
        }, error => {
          alert("Ha ocurrido un error")
        })
      } else {
        //0-1
        this.servicio.SetReaccion(this.user.id, this.publicacion.Producto.id, 0, -1).subscribe(response => {
          this.PostReaccion()
        }, error => {
          alert("Ha ocurrido un error")
        })
      }
    } else {
      if (!this.DislikeSet) {
        //1-0
        this.servicio.SetReaccion(this.user.id, this.publicacion.Producto.id, -1, 1).subscribe(response => {
          this.PostReaccion()
        }, error => {
          alert("Ha ocurrido un error")
        })
      } else {
        //1-1
        //caso imposible de llegar
      }
    }
  }

  PostReaccion(){
    this.servicio.GetPublicacion(this.publicacion.Producto.id).subscribe(res => {
      console.log(res);
      let updatedProducto = new Producto(res[0][0], res[0][2], res[0][3], res[0][9], res[0][5], res[0][6], res[0][7], res[0][8])
      this.publicacion.UpdateProducto(updatedProducto)
      this.ObtenerReacciones();
    })
  }


}
