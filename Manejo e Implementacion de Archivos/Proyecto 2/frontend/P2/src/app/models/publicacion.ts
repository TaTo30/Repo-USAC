import { Producto } from "./producto";
import { User } from "./user";

export class Publicacion {
    Vendedor: User;
    Producto: Producto;

    constructor(vendedor: User, producto: Producto){
        this.Vendedor = vendedor;
        this.Producto = producto;
    }

    UpdateProducto(producto: Producto){
        this.Producto = producto
    }
}
