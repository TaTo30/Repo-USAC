export class Producto {
    id: number;
    nombre: string;
    precio: number;
    categoria: string;
    likes: number;
    dislikes: number;
    descripcion: string;
    tags: string;

    constructor(id: number, nombre: string, precio: number, categoria: string, likes: number, dislikes: number, descripcion: string, tags: string){
        this.id = id;
        this.nombre = nombre;
        this.precio = precio;
        this.categoria = categoria;
        this.likes = likes;
        this.dislikes = dislikes;
        this.descripcion = descripcion;
        this.tags = tags
    }
}
