#include <stdio.h>

using namespace std;

template <typename T> class Matrix
{
    template <typename T> struct Nodo
    {
        T* Dato;
        int X, Y;
        Nodo* Siguiente;
    };
private:
    Nodo<T>* First;
    Nodo<T>* Last;
public:
Matrix(){
    First = NULL;
    Last = NULL;
}

void Add(T* Value, int X, int Y){
    Nodo<T>* aux = new Nodo<T>();
    aux->Dato = Value;
    aux->X = X;
    aux->Y = Y;
    if (First == NULL)
    {
       //LISTA VACIA
       First = aux;
       Last = aux;
    }
    else
    {
        //INSERTAR SIEMPRE AL FINAL
       Last->Siguiente = aux;
       Last = aux;
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
            First = First->Siguiente;
        }
        else if (n == Last)
        {
            //SI SE ELIMINA EL ULTIMO NODO
            Nodo<T>* busqueda = First;
            while (busqueda != NULL)
            {
                if (busqueda->Siguiente == Last)
                {
                    busqueda->Siguiente = NULL;
                    Last = busqueda;
                }else{
                    busqueda = busqueda->Siguiente;
                }                
            }
        }
        else
        {
            //SI SE ELIMINA CUALQUIER NODO
            Nodo<T>* busqueda = First;
            while (busqueda != NULL)
            {
                if (busqueda->Siguiente == n)
                {
                    busqueda->Siguiente = n->Siguiente;                    
                }else{
                    busqueda = busqueda->Siguiente;
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
        aux=aux->Siguiente;
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
        if (aux->Dato == dato)
        {
            encontrado = aux;
            aux=NULL;
        }else{
            aux=aux->Siguiente;
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
                aux = aux->Siguiente;
            }        
        }
    }    
    return encontrado;
}

//METODO QUE DEVUELVE UN NODO SEGUN: SU POSICION EN EL ESPACIO
Nodo<T>* GetPosition(int x, int y){
    Nodo<T>* aux = First;
    Nodo<T>* encontrado = NULL;
    while (aux != NULL)
    {
        if (aux->X == x && aux->Y == y)
        {
            encontrado = aux;
            aux=NULL;
        }else{
            aux=aux->Siguiente;
        }        
    }
    return encontrado;
}

//METODO QUE DEVUELVE SI UNA POSICION ESTA OCUPADA
bool Position(int x, int y){
    Nodo<T>* aux = First;
    bool encontrado = false;
    while (aux != NULL)
    {
        if (aux->X == x && aux->Y == y)
        {
            encontrado = true;
            aux=NULL;
        }else{
            aux=aux->Siguiente;
        }        
    }
    return encontrado;
}

void Clear(){
    First = NULL;
    Last = NULL;
}
};