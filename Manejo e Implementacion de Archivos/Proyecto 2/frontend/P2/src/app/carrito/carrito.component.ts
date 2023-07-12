import { Component, OnInit } from '@angular/core';
import { ServerConnectService } from "../servicio/server-connect.service";
import { User } from "../models/user";

@Component({
  selector: 'app-carrito',
  templateUrl: './carrito.component.html',
  styleUrls: ['./carrito.component.css']
})
export class CarritoComponent implements OnInit {

  user: User;

  constructor(private servicio: ServerConnectService) { }

  ngOnInit(): void {
    this.ObtenerDatosUsuario();
    this.obtenerCarrito();
  }

  carrito_data: any;
  carrito_total: number = 0;

  ObtenerDatosUsuario(){
    this.user = this.servicio.GetUser();
  }

  obtenerCarrito(){
    this.servicio.GetDetalleCarrito(this.user.id).subscribe( res => {
      this.carrito_data = res;
      console.log(this.carrito_data);
      this.carrito_total = 0;
      this.servicio.GetCliente(this.user.id);
      for (const arr of this.carrito_data) {
        this.carrito_total += arr[4];
      } 
    })
  }


  comprar(){
    if (this.carrito_total <= this.user.creditos) {
      this.servicio.TransferirCreditos(this.carrito_data[0][1], this.carrito_data[0][2], this.user.id, this.carrito_total).subscribe(res => {
        this.obtenerCarrito();
        alert("Productos comprados");
        this.ObtenerDatosUsuario();
      })
    }else{
      alert("No tienes suficientes creditos para realizar la compra");
    }
  }

  vaciarCarrito(){
    this.servicio.VaciarCarrito(this.user.id).subscribe(res => {
      this.obtenerCarrito();
    }, error => {
      console.error(error);
      
    })
  }


}
