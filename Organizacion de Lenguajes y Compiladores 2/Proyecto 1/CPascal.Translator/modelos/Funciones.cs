using System.Collections.Generic;
using System.Linq;
using System;
public class Funciones
{
    private string nombre;
    private LinkedList<Funciones> hijas;
    private LinkedList<Var> Declarados;
    private LinkedList<Var> Necesitados;
    private LinkedList<string> variablesDeclarados;
    private LinkedList<string> variablesNecesitados;
    private string TraduccionAsociada;


    public Funciones(string nombre){
        this.nombre = nombre;
        this.TraduccionAsociada = "";
        this.hijas = new LinkedList<Funciones>();
        variablesDeclarados = new LinkedList<string>();
        variablesNecesitados = new LinkedList<string>();
        Declarados = new LinkedList<Var>();
        Necesitados = new LinkedList<Var>();
    }

    public string SetNecesitados(string call, bool zero = false){
        string value = "";
        foreach (var hija in this.hijas)
        {
            if (hija.nombre.Equals(call))
            {
                if (zero)
                {
                    if (hija.Necesitados.Count > 0)
                    {
                        value += String.Format("{0}", hija.Necesitados.ElementAt(0).GetId());
                        for (int i = 1; i < hija.Necesitados.Count; i++)
                        {
                            value += String.Format(", {0}", hija.Necesitados.ElementAt(i).GetId());
                        }
                    }
                }else{
                    foreach (var dec in hija.Necesitados)
                    {
                        value += string.Format(", {0}", dec.GetId());
                    }
                }
                return value;             
            }
        }
        if (this.Declarados.Count != 0)
            value = ", ";
        if (this.Necesitados.Count > 0)
        {
            value += String.Format("{0}", this.Necesitados.ElementAt(0).GetId());
            for (int i = 1; i < this.Necesitados.Count; i++)
            {
                value += string.Format(", {0}", this.Necesitados.ElementAt(i).GetId(), this.Necesitados.ElementAt(i).GetTipo());
            }
        } else {
            value = "";
        }
            
        return value;
    }
    public string GetNecesitados(){
        string value = "";
        if (this.Declarados.Count != 0)
            value = "; ";
        if (this.Necesitados.Count > 0)
        {
            value += string.Format("{0}: {1}", this.Necesitados.ElementAt(0).GetId(), this.Necesitados.ElementAt(0).GetTipo());
            for (int i = 1; i < this.Necesitados.Count; i++)
            {
                value += string.Format("; {0}: {1}", this.Necesitados.ElementAt(i).GetId(), this.Necesitados.ElementAt(i).GetTipo());
            }
        } else {
            value = "";
        }
        return value;
    }

    public void AgregarVariable(string valor){
        this.variablesDeclarados.AddLast(valor);
    }
    public void AgregarVariableTipo(string tipo){
        foreach (var item in this.variablesDeclarados)
        {
            this.Declarados.AddLast(new Var(item, tipo));
        }
        this.variablesDeclarados.Clear();
    }
    public void AgregarNecesidad(string valor){
        foreach (var item in this.Declarados)
        {
            if (item.GetId().Equals(valor))
            {
                return;
            }
        }
        this.variablesNecesitados.AddLast(valor);
    }

    public void AgregarNecesidadTipo(string id, string tipo){
        this.Necesitados.AddLast(new Var(id, tipo));
    }

    public void ConectarPadreHija(){
        foreach (var hija in this.hijas)
        {
            foreach (var var in hija.variablesNecesitados)
            {
                foreach (var dec in this.Declarados)
                {
                    if (dec.GetId().Equals(var))
                    {
                        hija.AgregarNecesidadTipo(var, dec.GetTipo());
                    }
                }
            }
        }
        //this.variablesNecesitados.Clear();
    }
    public void AgregarTraduccion(string valor){
        this.TraduccionAsociada += valor + "\n";
    }
    public void AgregarHija(Funciones hija){
        this.hijas.AddLast(hija);
    }
    public string ObtenerTraduccion(){
        //retornamos la traduccion de sus hijas
        string value = "";
        foreach (var item in this.hijas)
        {
            value += item.ObtenerTraduccion();
        }
        //retornamos esta traduccion
        value += this.TraduccionAsociada;
        return value;
    }
}