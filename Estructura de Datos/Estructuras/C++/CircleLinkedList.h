#include <iostream>
#include <stdio.h>

using namespace std;



template <typename T> class CircleLinkedList
{
    template <typename T> struct Nodo
    {   
        T dato;
        Nodo* Anterior;
        Nodo* Siguiente;        
    };
private:   
    Nodo<T>* First;
    Nodo<T>* Last;
    //Metodo privado general para insertar datos
void Add(Nodo<T>* n, T dato_, bool pos){
    Nodo<T>* aux = new Nodo<T>();
    aux->dato = dato_;    
    if (Verificar() && n == NULL)
    {
        cout<<"Error: esta intentado agregar un Nodo vacio, verifique la lista"<<endl;
    }else{    
        if (First == NULL)
        {
            //LISTA VACIA
            First = aux;
            Last = aux;
            First->Siguiente = aux;
            First->Anterior = aux;
            Last->Siguiente = aux;
            Last->Anterior = aux;

        }else if(n == First && pos){
            //INSERTAR INICIO DE LA LISTA
            aux->Siguiente = First;
            First->Anterior = aux;
            First = aux;
            Last->Siguiente = First;
            First->Anterior = Last;
        }else if(n == Last && !pos){
            //INSERTAR FINAL DE LA LISTA
            aux->Anterior = Last;
            Last->Siguiente = aux;
            Last = aux;
            Last->Siguiente = First;
            First->Anterior = Last;
        }else if(pos){
            //INSERTAR ANTES DE UN NODO
            aux->Anterior = n->Anterior;
            n->Anterior->Siguiente = aux;
            n->Anterior = aux;
            aux->Siguiente = n;
        }else{
            //INSERTAR DESPUES DE UN NODO
            aux->Siguiente = n->Siguiente;
            n->Siguiente->Anterior = aux;
            n->Siguiente = aux;
            aux->Anterior = n;     
        }  
    }
}

public:  
//Constructor
CircleLinkedList(){
    First = NULL;
    Last = NULL;
}


/*************************************************************************
    METODOS PARA LA INSERSION DE DATOS EN LA LISTA DOBLEMENTE ENLAZADA
*************************************************************************/
//Metodo publico para insertar al inicio de la lista
void AddFirst(T dato){
    Add(First, dato, true);
}
//Metodo publico para insertar al final de la lista
void AddLast(T dato){
    Add(Last, dato, false);
}
//Metodo publico para insertar antes de un nodo dado (el nodo se obtiene con los metodos de buscar)
void AddBeforeAt(Nodo<T>* n, T dato){
    Add(n,dato, true);
}
//Metodo publico para insertar despues de un nodo dado (el nodo se obtiene con los metodos de buscar)
void AddAfterAt(Nodo<T>* n, T dato){
    Add(n, dato, false);
}


/***************************************************************************
    METODOS PARA LA ELIMINACION DE DATOS EN LA LISTA DOBLEMENTE ENLAZADA
***************************************************************************/
//Metodo para Eliminar un Nodo si se tiene el nodo
void Remove(Nodo<T>* n){
    if (!Verificar() || n != NULL)
    {   
        if (n == First && n == Last)
        {
            //ELIMINA EL ULTIMO NODO: LISTA VACIA
            First = NULL;
            Last = NULL;
            
        }        
        else if (n == First)
        {
            //ELIMINAR EL PRIMER NODO
            First = First->Siguiente;
            First->Anterior = Last;
            Last->Siguiente = First;
        }
        else if (n == Last)
        {
            //ELIMINAR EL ULTIMO NODO
            Last = Last->Anterior;
            Last->Siguiente = First;
            First->Anterior = Last;
        }
        else
        {
            //ELIMINAR CUALQUIER NODO
            n->Anterior->Siguiente = n->Siguiente;
            n->Siguiente->Anterior = n->Anterior;
        }        
    }else{
        cout<<"Error: el nodo no se encuentra en la lista"<<endl;
    }    
}
//Metodo para eliminar uno nodo segun su posicion
void RemoveAt(int index){
    Remove(Index(index));
}
/***********************************************************************
    METODOS PARA LA BUSCAR DE DATOS EN LA LISTA DOBLEMENTE ENLAZADA
            TODOS LOS METODOS DE BUSCAR DEVUELVEN UN NODO
***********************************************************************/
//Metodo publico para buscar un nodo SEGUN: su contenido
Nodo<T>* Find(T dato){
    Nodo<T>* aux = First;
    Nodo<T>* encontrado=NULL;
    do
    {
        if (aux->dato == dato)
        {
            encontrado = aux;
            aux=First;
        }else{
            aux=aux->Siguiente;
        }
    } while (aux != First);
    
    return encontrado;    
}
//Metodo publico para buscar un nodo SEGUN: su posicion
Nodo<T>* Index(int index){
    Nodo<T>* aux = First;
    Nodo<T>* encontrado = NULL;
    int contadorIndex = 0;
    if (index > Size())
    {
        cout<<"El indice introducido es mayor a la longitud de la lista"<<endl;
    }else{        
        do
        {
            if (contadorIndex == index)
            {
                encontrado = aux;
                aux=First;
            }else{
                contadorIndex++;
                aux = aux->Siguiente;
            }
        } while (aux != First);
        
        
    }    
    return encontrado;
}
//Metodo publico para buscar un nodo SEGUN: el primero siempre
Nodo<T>* GetFirst(){
    return First;
}
//Metodo publico para buscar un nodo SEGUN: el ultimo siempre
Nodo<T>* GetLast(){
    return Last;
}
/***********************************************************************
    METODOS PARA LA COMPROBACION DE DATOS Y DESPLIEGE EN PANTALLA
***********************************************************************/
//Verifica que la lista este vacia
bool Verificar(){
    if (First == NULL)
    {
        return false;
    }else{
        return true;
    }
    
}
//Verifica el tamanio de la lista
int Size(){
    Nodo<T>* aux = First;
    int contador=0;
    do
    {
        contador++;
        aux=aux->Siguiente;
    } while (aux != First);
    
    return contador;    
}
//Imprime todo el contenido de la lista
void Imprimir(){
    Nodo<T>* aux= First;
    do
    {
        cout<<aux->dato<<'\n';
        aux=aux->Siguiente;
    } while (aux != First);
    
}
};
