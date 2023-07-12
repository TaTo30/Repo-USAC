using System.Collections.Generic;
public class Funcion
{
    public string Nombre {get; set;}
    public string Tipo {get; set;}
    public List<C3D> Codigo {get; set;}
    public string SaltoReturn {get; set;} 

    public Funcion(string nombre, string saltoReturn){
        this.Nombre = nombre;
        this.SaltoReturn = saltoReturn;
    }


}