using System.Collections.Generic;
public class Definidas : Instruccion, Definicion
{
    public Posicion posicion {get; set;}
    public string name {get; set;}
    public List<Tipicas> declaraciones {get; set;}
    public List<RangoArray> rangos {get; set;}
    private string tipo;

    public Definidas(string name, List<Tipicas> dec, Posicion posicion){
        this.name = name;
        this.declaraciones = dec;
        this.tipo = "";
        this.posicion = posicion;
    }

    public Definidas(string name, List<RangoArray> rangoArrays, string tipo, Posicion posicion){
        this.name = name;
        this.rangos = rangoArrays;
        this.tipo = tipo;
        this.posicion = posicion;
    }
    public void MapearGlobales(Tabla tabla, Ambito ambito){ 
        if (this.tipo == "")
        {
            tabla.Add(new Simbolo(this.name, this.tipo, ambito.Nombre, this.tipo == ""? "Struct":"Arreglo", null, posicion));
            Ambito ambito1 = new Ambito(this.name);
            if (this.tipo == "")
                foreach (var dec in this.declaraciones)
                    foreach (var v in dec.variables)                
                        tabla.Add(new Simbolo(v.identificador, v.tipo, ambito1.Nombre, "Variable", ambito1.Correlativo, dec.posicion));
    
        }else{
            int size = 1;
            if (this.rangos != null)
                foreach (var rango in this.rangos)
                    size *= (int)rango.Lenght;
            List<int> dimensiones = new List<int>();
            if (this.rangos != null)
                foreach (var rango in this.rangos)            
                    dimensiones.Add((int)rango.Lenght);
            List<int> minimos = new List<int>();
            if (this.rangos != null)
                foreach (var rango in this.rangos)            
                    minimos.Add((int)rango.Minimo);
            tabla.Add(new Simbolo(this.name, this.tipo, ambito.Nombre, this.tipo == ""? "Struct":"Arreglo", size, posicion, dimensiones, minimos));
        }
        
     
    }
    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        return new List<C3D>();
    }
}

//Seleccionar El tamaño
//Temporal tiene el tamaño del arreglo
//El stack tiene el indice 0
//Tabla de simbolos NOMBRE - SIZE - POSICIONSTACK
