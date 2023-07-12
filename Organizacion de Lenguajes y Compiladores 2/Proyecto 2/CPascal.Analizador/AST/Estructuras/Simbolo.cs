using System.Collections.Generic;
using System;
public class Simbolo
{
    /*public enum Tipo
    {
        BOOLEAN,
        REAL,
        INTEGER,
        STRING,
        ACCESO,
        NONE,
        VOID,
        ERROR
    }*/
    public string Nombre {get; set;}
    public string Tipo {get; set;}
    public string Ambito {get; set;}
    public string Rol {get; set;}
    public int? Apuntador {get; set;}
    public Posicion Posicion {get; set;}
    public List<int> Dimensiones {get; set;}
    public List<int> Minimo {get; set;}

    public Simbolo(string nombre, string Tipo, string ambitos, string rol, int? apuntador, Posicion posicion){
        this.Nombre = nombre;
        this.Tipo = Tipo;
        this.Rol = rol;
        this.Apuntador = apuntador;
        this.Ambito = ambitos;
        this.Posicion = posicion;
        this.Dimensiones = new List<int>();
        this.Minimo = new List<int>();
    }

    public Simbolo(string nombre, string Tipo, string ambitos, string rol, int? apuntador, Posicion posicion, List<int> Dim, List<int> min){
        this.Nombre = nombre;
        this.Tipo = Tipo;
        this.Rol = rol;
        this.Apuntador = apuntador;
        this.Ambito = ambitos;
        this.Posicion = posicion;
        this.Dimensiones = Dim;
        this.Minimo = min;
    }
}