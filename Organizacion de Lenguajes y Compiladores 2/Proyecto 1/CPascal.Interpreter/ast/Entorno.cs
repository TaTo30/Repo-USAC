using System.Collections.Generic;
using System.Linq;
using System;
class Entorno : LinkedList<Simbolo>
{

    public string nombre;
    public LinkedList<string> padres;


    public Entorno() : base() { 
        this.nombre = "Global";
        this.padres = new LinkedList<string>();
    }
    public Entorno(string nombre){
        this.nombre = nombre;
    }
    public Entorno(Entorno global, string nombre, LinkedList<string> padres, bool INFS = true){
        //nombre de nuestro entorno local
        this.nombre = nombre;
        //nombre de los padres
        this.padres = padres;
        if (!global.nombre.Equals(this.nombre))
            this.padres.AddLast(global.nombre);
        foreach (var item in global)
        {
            if (INFS)
            {
                this.AddLast(item);
            } else {
                if (item.GetTipo() == Simbolo.Tipo.IOBJECT || item.GetTipo() == Simbolo.Tipo.IARRAY)
                {
                    this.AddLast(item);
                }
            }
            
        }
    }   
    
    
    public Entorno DeleteInterfaces(){
        Entorno variables = new Entorno(this.nombre);
        foreach (var item in this)
        {
            if (item.GetTipo() != Simbolo.Tipo.IOBJECT && item.GetTipo() != Simbolo.Tipo.IARRAY)
            {
                variables.AddLast(item);
            }
        }        
        return variables;
    }

    public void NewReplace(Simbolo smb){
        //SI LA VARIABLE EXISTE LA AGREGAMOS
        foreach (var item in this)
        {
            if (item.GetId().Equals(smb.GetId()) && smb.GetEnv().Equals(item.GetEnv()))
            {
                item.SetValor(smb.GetValor());
                return;               
            }
        }
        //SI NO EXISTE AÑADIMOS NUEVO NODO
        this.AddLast(smb);
    }
    public void SetValor(string id, object valor, bool reference = false){
        if (reference)
            goto padres;
        foreach (var item in this)
        {
            if (item.GetId().Equals(id) && item.GetEnv().Equals(this.nombre))
            {
                item.SetValor(valor);
                return;               
            }
        }
        padres:
         //buscamos en los padres
        for (int i = this.padres.Count - 1; i >= 0; i--)
        {
            foreach (var item in this)
            {
                if (item.GetId().Equals(id) && item.GetEnv().Equals(this.padres.ElementAt(i)))
                {
                    item.SetValor(valor);
                    return;               
                }
            }
        }
        throw new SemanticException("Simbolo no encontrado");
    }
    public Simbolo.Tipo GetTipo(string id, bool declaracion = false){
        foreach (var item in this)
        {
            if (item.GetId().Equals(id) && item.GetEnv().Equals(this.nombre))
            {
                return item.GetTipo();
            }
        }
        if (declaracion)
            return Simbolo.Tipo.ERROR;
        //buscamos en los padres
        for (int i = this.padres.Count - 1; i >= 0; i--)
        {
            foreach (var item in this)
            {
                if (item.GetId().Equals(id) && item.GetEnv().Equals(this.padres.ElementAt(i)))
                {
                    return item.GetTipo();
                }
            }
        }
        return Simbolo.Tipo.ERROR;
    }
    public bool GetConst(string id){
        foreach (var item in this)
        {
            if (item.GetId().Equals(id) && item.GetEnv().Equals(this.nombre))
            {
                return item.GetConst();
            }
        }
        //buscamos en los padres
        for (int i = this.padres.Count - 1; i >= 0; i--)
        {
            foreach (var item in this)
            {
                if (item.GetId().Equals(id) && item.GetEnv().Equals(this.padres.ElementAt(i)))
                {
                    return item.GetConst();
                }
            }
        }
        return true;
    }
    public object GetValor(string id){
        //buscamos en el local
        foreach (var item in this)
        {
            if (item.GetId().Equals(id) && item.GetEnv().Equals(this.nombre))
            {
                if (item.GetValor() is Funcion)
                {
                    return item.GetValor();
                }
                switch (item.GetTipo())
                {
                    case Simbolo.Tipo.INTEGER:
                        return double.Parse(item.GetValor().ToString()); 
                    case Simbolo.Tipo.REAL:
                        return double.Parse(item.GetValor().ToString()); 
                    case Simbolo.Tipo.BOOLEAN:
                        return bool.Parse(item.GetValor().ToString());
                    case Simbolo.Tipo.STRING:
                        return item.GetValor().ToString();
                    default:
                        return item.GetValor();
                }                              
            }
        }
        //buscamos en los padres
        for (int i = this.padres.Count - 1; i >= 0; i--)
        {
            foreach (var item in this)
            {
                if (item.GetId().Equals(id) && item.GetEnv().Equals(this.padres.ElementAt(i)))
                {
                    if (item.GetValor() is Funcion)
                    {
                        return item.GetValor();
                    }
                    switch (item.GetTipo())
                    {
                        case Simbolo.Tipo.INTEGER:
                            return double.Parse(item.GetValor().ToString());
                        case Simbolo.Tipo.REAL:
                            return double.Parse(item.GetValor().ToString());
                        case Simbolo.Tipo.BOOLEAN:
                            return bool.Parse(item.GetValor().ToString());
                        case Simbolo.Tipo.STRING:
                            return item.GetValor().ToString();
                        default:
                            return item.GetValor();
                    }
                }
            }
        }        
        return Simbolo.Tipo.ERROR;
    }

    override 
    public String ToString(){
        String cadena = "";
        foreach (var item in this)
        {
            cadena = String.Format("{0}{1}: {2}\n", cadena, item.GetId(), (item.GetValor() is Entorno? "LocalEnv": item.GetValor().ToString()));
        }
        return cadena;
    }
}