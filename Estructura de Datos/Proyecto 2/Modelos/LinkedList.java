/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Modelos;

public class LinkedList<T>{   
    class Nodo<T>{   
        T dato;
        Nodo Anterior;
        Nodo Siguiente;        
    };

   private Nodo<T> First;
   private Nodo<T> Last;
   public int Size;
//Metodo privado general para insertar datos
private void Add(Nodo<T> n, T dato_, boolean pos){
    Nodo<T> aux = new Nodo<>();
    aux.dato = dato_;   
    if (Contain() && n == null)
    {
        //cout<<"Error: esta intentado agregar un Nodo vacio, verifique la lista"<<endl;
    }else{
        this.Size++;
        if (First == null)
        {
            //LISTA VACIA
            First = aux;
            Last = aux;
        }else if(n == First && pos){
            //INSERTAR INICIO DE LA LISTA
            aux.Siguiente = First;
            First.Anterior = aux;
            First = aux;
        }else if(n == Last && !pos){
            //INSERTAR FINAL DE LA LISTA
            aux.Anterior = Last;
            Last.Siguiente = aux;
            Last = aux;
        }else if(pos){
            //INSERTAR ANTES DE UN NODO
            aux.Anterior = n.Anterior;
            n.Anterior.Siguiente = aux;
            n.Anterior = aux;
            aux.Siguiente = n;
        }else{
            //INSERTAR DESPUES DE UN NODO
            aux.Siguiente = n.Siguiente;
            n.Siguiente.Anterior = aux;
            n.Siguiente = aux;
            aux.Anterior = n;     
        }  
    }
}
//Metodo privado para la ubicacion de nodos por datos
private Nodo<T> Find(T dato){
    Nodo<T> aux = First;
    Nodo<T> encontrado=null;
    while (aux != null)
    {
        if (aux.dato == dato)
        {
            encontrado = aux;
            aux=null;
        }else{
            aux=aux.Siguiente;
        }        
    }
    return encontrado;    
}
//Metodo privado para la ubicacion de nodos por indicie
private Nodo<T> Index(int index){
    Nodo<T> aux = First;
    Nodo<T> encontrado = null;
    int contadorIndex = 0;
    if (index > Size())
    {
        //cout<<"El indice introducido es mayor a la longitud de la lista"<<endl;
    }else{
        while (aux != null)
        {
            if (contadorIndex == index)
            {
                encontrado = aux;
                aux=null;
            }else{
                contadorIndex++;
                aux = aux.Siguiente;
            }        
        }
    }    
    return encontrado;
}
//Metodo privado general para eliminar datos
private void Remove(Nodo<T> n){
    if (!Contain() || n != null)
    {   
        if (n == First && n == Last)
        {
            //ELIMINA EL ULTIMO NODO: LISTA VACIA
            First = null;
            Last = null;
        }        
        else if (n == First)
        {
            //ELIMINAR EL PRIMER NODO
            First = First.Siguiente;
            First.Anterior = null;
        }
        else if (n == Last)
        {
            //ELIMINAR EL SEGUNDO NODO
            Last = Last.Anterior;
            Last.Siguiente = null;
        }
        else
        {
            //ELIMINAR CUALQUIER NODO
            n.Anterior.Siguiente = n.Siguiente;
            n.Siguiente.Anterior = n.Anterior;
        }        
    }else{
        //cout<<"Error: el nodo no se encuentra en la lista"<<endl;
    }    
}


//Constructor de Lista
public LinkedList(){
    First = null;
    Last = null;
    Size = 0;
}

/*************************************************************************
    METODOS PARA LA INSERSION DE DATOS EN LA LISTA DOBLEMENTE ENLAZADA
*************************************************************************/

//Inserta un valor al inicio de la lista
public void AddFirst(T data){
    Add(First, data, true);
}
//Inserta un valor al final de la lista
public void AddLast(T data){
    Add(Last, data, false);
}
//Inserta un valor antes de un valor especificado por el usuario
public void AddBeforeAt(T reference, T data){
    Add(Find(reference),data, true);
}
//Inserta un valor despues de un valor especificado por el usuario
public void AddAfterAt(T reference, T data){
    Add(Find(reference), data, false);
}


/***************************************************************************
    METODOS PARA LA ELIMINACION DE DATOS EN LA LISTA DOBLEMENTE ENLAZADA
***************************************************************************/

//Eliminar un valor en el indice establecido
public void RemoveAt(int index){
    Remove(Index(index));
}
//Elimina el ultimo valor de la lista
public void RemoveLast(){
    Remove(Last);
}
//Elimina el primer valor de la lista
public void RemoveFirst(){
    Remove(First);
}
//Elimina un valor por elemento especificado por el usuario
public void RemoveElement(T data){
    Remove(Find(data));
}


/***********************************************************************
    METODOS PARA LA BUSCAR DE DATOS EN LA LISTA DOBLEMENTE ENLAZADA
            TODOS LOS METODOS DE BUSCAR DEVUELVEN UN NODO
***********************************************************************/

//Devuelve el valor del dato en el indice especificado por el usuario
public T ElementAt(int index){
    Nodo<T> aux = First;
    Nodo<T> encontrado = null;
    int contadorIndex = 0;
    if (index > Size())
    {
        //cout<<"El indice introducido es mayor a la longitud de la lista"<<endl;
    }else{
        while (aux != null)
        {
            if (contadorIndex == index)
            {
                encontrado = aux;
                aux=null;
            }else{
                contadorIndex++;
                aux = aux.Siguiente;
            }        
        }
    }    
    return encontrado.dato;
}
//Devuelve el primer dato de la lista
public T GetFirst(){
    return First.dato;
}
//Devuelve el ultimo dato de la lista
public T GetLast(){
    return Last.dato;
}


/***********************************************************************
    METODOS PARA LA COMPROBACION DE DATOS Y DESPLIEGE EN PANTALLA
***********************************************************************/

//Devuelve un valor booleanean, true si la lista contiene datos, false si no contiene datos
public boolean Contain(){
        return First != null;
}
//Devuelve el valor correspondiente al tamaño de la lista
public int Size(){
    
    return this.Size;    
}
//Vacia la lista
public void Clear(){    
    Nodo<T> aux = First;
    while (aux!=null)
    {
        Remove(aux);
        aux = First;
    }
    First = null;
    Last = null;
}
};