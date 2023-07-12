#include <stdio.h>
#include <iostream>

using namespace std;

template <typename T> class OrderListP
{
    template <typename T> struct Nodo
    {
        T* dato;
        int orderIndex;
        Nodo* siguiente;        
    };

private:
    Nodo<T>* First;
    Nodo<T>* Last;

public:
OrderListP(){
    First = NULL;
    Last = NULL;
}

void Add(T* dato_, int OrderIndex){
    Nodo<T>* aux = new Nodo<T>();
    aux->dato = dato_;
    aux->orderIndex = OrderIndex;
    if (First == NULL)
    {
        //SI LA LISTA ESTA VACIA: SE AÑADE E INICA LA LISTA
        First = aux;
        Last = aux;
    }
    else if (OrderIndex < First->orderIndex)
    {
        //SI EL DATO ES MENOR QUE EL PRIMERO SE INSERTA DE PRIMERO
        aux->siguiente = First;
        First = aux;
    }else if (OrderIndex > Last->orderIndex)
    {
        //SI EL DATO ES MAYOR QUE EL ULTIMO SE INSERTA DE ULTIMO
        Last->siguiente = aux;
        Last = aux;
    }else{
        //SE BUSCA INSERTAR DONDE SEA MAYOR QUE UNO Y MENOR O IGUAL AL QUE SIGUE
        Nodo<T>* busqueda = First;
        while (busqueda != NULL)
        {
            if ((OrderIndex > busqueda->orderIndex) && (OrderIndex <= busqueda->siguiente->orderIndex) )
            {
                aux->siguiente = busqueda->siguiente;
                busqueda->siguiente = aux;
                busqueda = NULL;
                
            }else{
                busqueda = busqueda->siguiente;
            }            
        }        
    }    
}


/***************************************************************************
    METODOS PARA LA ELIMINACION DE DATOS EN LA LISTA SIMPLE ORDENADA
***************************************************************************/
void Remove(Nodo<T>* n){
    if (n !=NULL && Contain())
    {
        if (n == First)
        {
            //SI SE ELIMINA EL PRIMER NODO
            First = First->siguiente;
        }
        else if (n == Last)
        {
            //SI SE ELIMINA EL ULTIMO NODO
            Nodo<T>* busqueda = First;
            while (busqueda != NULL)
            {
                if (busqueda->siguiente == Last)
                {
                    busqueda->siguiente = NULL;
                    Last = busqueda;
                }else{
                    busqueda = busqueda->siguiente;
                }                
            }
        }
        else
        {
            //SI SE ELIMINA CUALQUIER NODO
            Nodo<T>* busqueda = First;
            while (busqueda != NULL)
            {
                if (busqueda->siguiente == n)
                {
                    busqueda->siguiente = n->siguiente;                    
                }else{
                    busqueda = busqueda->siguiente;
                }                
            }
        }        
    }    
}
//Metodo para eliminar un nodo segun su posicion
void RemoveAt(int index){
    Remove(Index(index));
}

/***********************************************************************
    METODOS PARA LA COMPROBACION DE DATOS Y DESPLIEGE EN PANTALLA
***********************************************************************/
bool Contain(){
    bool toReturn = true;
    if (First == NULL)
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
/***********************************************************************
    METODOS PARA LA BUSCAR DE DATOS EN LA LISTA SIMPLE ORDENADA
            TODOS LOS METODOS DE BUSCAR DEVUELVEN UN NODO
***********************************************************************/
//Metodo publico para buscar un nodo SEGUN: Su contenido
Nodo<T>* Find(T* dato){
    Nodo<T>* aux = First;
    Nodo<T>* encontrado=NULL;
    while (aux != NULL)
    {
        if (aux->dato == dato)
        {
            encontrado = aux;
            aux=NULL;
        }else{
            aux=aux->siguiente;
        }        
    }
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
        while (aux != NULL)
        {
            if (contadorIndex == index)
            {
                encontrado = aux;
                aux=NULL;
            }else{
                contadorIndex++;
                aux = aux->siguiente;
            }        
        }
    }    
    return encontrado;
}
//Devuelve el valor en lista mas PEQUEÑO
Nodo<T>* Min(){
    return First;
}
//Devuelve el valor en lista mas GRANDE
Nodo<T>* Max(){
    return Last;
}  

};