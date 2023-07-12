#include <stdio.h>
#include <iostream>

using namespace std;

template <typename T> class Queue
{
    template <typename T> struct Nodo
    {
        T* dato;
        Nodo* siguiente;
    };

private:
    Nodo<T>* First;
    Nodo<T>* Last;
public:

//Constructor
Queue(){
    First = NULL;
    Last = NULL;
}

//Encolar un dato a la lista enlazanda
void Enqueue(T *dato)
{
    Nodo<T>* aux = new Nodo<T>();
    aux->dato = dato;
    if (First == NULL)
    {
        //LISTA VACIA INSERTAR DATOS
        First = aux;
        Last = aux;
    }else{
        //ENCOLAR DATO SIEMPRE APUNTANTOD A FIRST
        aux->siguiente = First;
        First = aux;
    }    
}

//Desencolar un dato a la lista enlazada
Nodo<T>* Dequeue(){
    Nodo<T>* aux = First;
    Nodo<T>* toReturn = NULL;
    if (First == Last)
    {
        toReturn = First;
        First = NULL;
        Last = NULL;
    }
    else
    {
        while (aux != NULL)
        {
            if (aux->siguiente == Last)
            {
                toReturn = Last;
                Last = aux;
                Last->siguiente = NULL;
                aux = NULL;
            }
            else
            {
                aux = aux->siguiente;
            }        
        }
    }    
    return toReturn;
}

//Metodos Propiedades de la Cola
bool Contain(){
    bool toReturn = true;
    if (First ==NULL)
    {
        toReturn = false;
    }
    return toReturn;
}

int Size(){
    Nodo<T>* aux = First;
    int contador=0;
    while (aux != NULL)
    {
        contador++;
        aux=aux->siguiente;
    }
    return contador;
}

Queue<T>* Clonar(){
    Queue<T>* colaClonada = new Queue<T>();
    T* Aux;
    for (int i = 0; i < this->Size(); i++)
    {
        Aux = this->Dequeue()->dato;
        colaClonada->Enqueue(Aux);
        this->Enqueue(Aux);
    }   
    return colaClonada;
}

};