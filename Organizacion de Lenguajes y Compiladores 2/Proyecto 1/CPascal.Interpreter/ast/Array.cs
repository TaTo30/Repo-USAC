using System.Collections.Generic;
using System.Linq;
using System;
class OpArray : Instruccion{
    public int Linea {get; set;}
    public int Columna {get; set;}
    public string Id {get; set;}
    private Operacion index;
    public OpArray(string id, Operacion index, int x, int y){
        this.Id = id;
        this.index = index;
        this.Linea = x;
        this.Columna = y;
    }
    public void ejecutar(Entorno env, object valor){
        var i = this.index.ejecutar(env);
        if (!(i is double || i is int))
            throw new SemanticException("El indice debe ser un tipo ordinal", this.Linea, this.Columna);
        int index = Convert.ToInt32(i);
        Array dictionary = (Array)env.GetValor(this.Id);
        dictionary.SetArray(index, valor);
    }

    public object ejecutar(Entorno env){
        var i = this.index.ejecutar(env);
        if (!(i is double || i is int))
            throw new SemanticException("El indice debe ser un tipo ordinal", this.Linea, this.Columna);
        int index = Convert.ToInt32(i);
        Array dictionary = (Array)env.GetValor(this.Id);
        return dictionary.GetArray(index);
    }
}
class Array : ICloneable
{
    private Operacion minimo;
    private Operacion maximo;
    private Simbolo.Tipo tipo;
    private int minindex;
    private int maxindex;
    private Dictionary<int, object> arr;


    public object Clone(){
        return this.MemberwiseClone();
    }
    public Array(Operacion minimo, Operacion maximo, Simbolo.Tipo tipo){
        this.minimo = minimo;
        this.maximo = maximo;
        this.tipo = tipo;
    }
    public void SetArray(int index, object value){
        if (!((index >= this.minindex) && (index <= this.maxindex)))
            throw new SemanticException($"Indice {index} se encuentra fuera del rango permitido ({this.minindex} - {this.maxindex})");
        if (this.arr.ContainsKey(index))        
            //modifiacion del array
            this.arr.Remove(index);
        
        this.arr.Add(index, value);
    }
    public object GetArray(int index){
        if (!((index >= this.minindex) && (index <= this.maxindex)))
            throw new SemanticException($"Indice {index} se encuentra fuera del rango permitido ({this.minindex} - {this.maxindex})");
        if (!this.arr.ContainsKey(index))
            throw new SemanticException($"Indice {index} no se encuentra en el array");
        return this.arr[index];
    }
    public void setup(Entorno env){
        var mini = this.minimo.ejecutar(env);
        if (!(mini is double))
            throw new SemanticException("Solo se pueden usar valores ordinales numericos en el indice");
        var maxi = this.maximo.ejecutar(env);
        if (!(maxi is double))
            throw new SemanticException("Solo se pueden usar valores ordinales numericos en el indice");
        this.minindex = Convert.ToInt32(mini);
        this.maxindex = Convert.ToInt32(maxi);
        arr = new Dictionary<int, object>();  
    }
}