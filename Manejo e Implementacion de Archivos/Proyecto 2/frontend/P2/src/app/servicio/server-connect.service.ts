import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { User } from "../models/user";

@Injectable({
  providedIn: 'root'
})
export class ServerConnectService {

  constructor(private http: HttpClient) { }

  server = 'http://localhost:3000';

    //==================================SERVICIOS DE SERVIDOR=============================//
  registrar(nombre: string, apellido: string, mail: string, pass: string, cpass: string, nac: string, pais:string, foto:string){
    return this.http.post(`${this.server}/registrar`,{
      nombre: nombre,
      apellido: apellido,
      mail: mail,
      pass: pass, 
      cpass: cpass,
      nac: nac,
      pais: pais,
      foto: foto
    });
  }

  logear(correo: string, pass: string){
    return this.http.post(`${this.server}/login`,{
      correo: correo,
      pass: pass
    });
  }

  ActualizarPerfil(pass: string, cpass: string, nac: string, pais: string, id: string, nombre:string, apellido:string){
    return this.http.post(`${this.server}/update_cliente`,{
      id: id,
      pass: pass,
      cpass: cpass,
      nac: nac,
      pais: pais,
      nombre: nombre,
      apellido: apellido
    });
  }

  GetCategorias(){
    return this.http.get(`${this.server}/get_categorias`);
  }

  SetCategorias(nombre: string){
    return this.http.post(`${this.server}/set_categoria`,{
      nombre: nombre
    })
  }

  SetPublicacion(vendedor: number, nombre: string, precio: string, categoria: string, descripcion: string, tags: string){
    return this.http.post(`${this.server}/set_publicacion`,{
      vendedor: vendedor,
      nombre: nombre,
      precio: precio,
      categoria: categoria,
      descripcion: descripcion,
      tags: tags
    })
  }

  GetPublicaciones(){
    return this.http.get(`${this.server}/get_publicaciones`);
  }

  GetPublicacionesFiltro(busqueda: string, categoria: string, orden: string){
    return this.http.post(`${this.server}/get_publicaciones_filter`,{
      busqueda: busqueda,
      categoria: categoria,
      orden: orden
    })
  }

  GetPublicacion(id: number){
    return this.http.post(`${this.server}/get_publicacion`, {
      id: id
    })
  }

  setDetalleCarrilt(cliente: number, publicacion:number){
    return this.http.post(`${this.server}/set_detalle_carrito`,{
      cliente: cliente,
      publicacion: publicacion
    })
  }

  GetDetalleCarrito(id: number){
    return this.http.post(`${this.server}/get_detalle_carrito`,{
      id: id
    });
  }

  VaciarCarrito(id: number){
    return this.http.post(`${this.server}/vaciar_carrito`,{
      id: id
    })
  }

  TransferirCreditos(publicacion:number, vendedor: number, comprador: number, creditos: number){
    return this.http.post(`${this.server}/transferir_creditos`, {
      publicacion: publicacion,
      vendedor: vendedor,
      cliente: comprador,
      creditos: creditos
    })
  }

  SetComentario(emisor: number, publicacion: number, fecha: string, contenido: string){
    return this.http.post(`${this.server}/set_comentario`, {
      emisor: emisor,
      publicacion: publicacion,
      fecha: fecha,
      contenido: contenido
    })
  }

  GetComentario(){
    return this.http.get(`${this.server}/get_comentario`);
  }

  SetDenuncia(publicacion: number, contenido: string){
    return this.http.post(`${this.server}/set_denuncia`, {
      publicacion: publicacion,
      contenido: contenido
    })
  }

  SetReaccion(cliente: number, publicacion: number, likeset: number, dislikeset: number){
    return this.http.post(`${this.server}/set_reaccion`, {
      cliente: cliente,
      publicacion: publicacion,
      likeset: likeset,
      dislikeset: dislikeset
    })
  }

  GetReaccion(cliente: number, publicacion: number){
    return this.http.post(`${this.server}/get_reaccion`,{
      cliente: cliente,
      publicacion: publicacion
    })
  }

  GetCliente(id: number){
    this.http.post(`${this.server}/get_cliente`, {
      id: id
    }).subscribe(res => {
      this.deleteUser();
      this.saveUser(res[0][0], res[0][1], res[0][2], res[0][3], res[0][4], res[0][5], res[0][6], res[0][7])
    }, err => {
      console.error(err);
    })
  }

  GetProductosVendidos(){
    return this.http.get(`${this.server}/productos_vendidos`)
  }

  GetProductosLikes(){
    return this.http.get(`${this.server}/productos_likes`)
  }

  GetProductosDislikes(){
    return this.http.get(`${this.server}/productos_dislikes`)
  }

  GetCreditosClientes(){
    return this.http.get(`${this.server}/clientes_creditos`)
  }
  
  GetCreditosPaises(){
    return this.http.get(`${this.server}/paises_creditos`)
  }

  GetClientesVentas(){
    return this.http.get(`${this.server}/ventas_clientes`)
  }

  GetClientesComentarios(){
    return this.http.get(`${this.server}/comentarios_clientes`)
  }


  //==================================SERVICIOS DE LOCALIDAD=============================//
  Clear(){
    sessionStorage.clear();
  }

  TrueLog(){
    sessionStorage.removeItem('log');
    sessionStorage.setItem('log', 'true');
  }

  FalseLog(){
    sessionStorage.removeItem('log');
    sessionStorage.setItem('log', 'false');
  }

  GetLog(){
    return sessionStorage.getItem('log'); 
  }

  saveUser(id: number,nombre: string, apellido: string, mail:string, nac:string, pais:string, foto:string, creditos:number){
    let user = new User(id, nombre, apellido, mail, nac, pais, foto, creditos)
    console.log(user);
    sessionStorage.setItem('user', JSON.stringify(user));
  }

  GetUser(){
    let user = JSON.parse(sessionStorage.getItem('user'));
    return user;
  }

  deleteUser(){
    sessionStorage.removeItem('user');
  }
  
}

