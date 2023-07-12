export class Usuario{	
	nombre: string;
	apellido: string;
	carnet: string;
	email: string;
	pass: string;

	constructor(nombre:string, apellido:string, carnet:string, email:string, pass:string){
		this.nombre=nombre;
		this.apellido=apellido;
		this.carnet=carnet;
		this.email=email;
		this.pass=pass;
	}
}