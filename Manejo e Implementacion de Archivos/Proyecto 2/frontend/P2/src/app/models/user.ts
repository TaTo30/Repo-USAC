
export class User {
    id: number;
    nombre: string;
    apellido: string;
    mail: string;
    nac: string;
    pais: string;
    foto: string;
    creditos: number;
    constructor(id: number, nombre: string, apellido: string, mail: string, nac: string, pais: string, foto: string, creditos: number){
        this.id = id;
        this.nombre = nombre;
        this.apellido = apellido;
        this.mail = mail;
        this.nac = nac;
        this.pais = pais;
        this.foto = foto;
        this.creditos = creditos;
    }
}