#include "ListaSimple.h"


void::ListaSimple::Insertar(string nombre, int id){
    Nodo* aux = new Nodo();
    aux->nombreCompleto = nombre;
    aux->carnet = id;
    aux->siguiente = first;
    first = aux;
    cout<<"Se ha agregado a la lista a "<<aux->nombreCompleto<<" con carnet: "<<aux->carnet<<endl;
}

bool::ListaSimple::EliminarPre(Nodo* n){
    if (n == NULL)
    {
        cout<<"Error amigo el nodo que intentas eliminar no existe en la pila"<<endl;
        return false;
    }else{
        first = n->siguiente;
        cout<<"Se han borrado los nodos previos a: "<<n->carnet<<endl;
        return true;
    }    
}

bool::ListaSimple::EliminarPost(Nodo* n){
    if (n == NULL)
    {
        cout<<"Error amigo el nodo que intentas eliminar no existe en la pila"<<endl;
        return false;
    }else{
        n->siguiente = NULL;
        cout<<"Se han borrado los nodos posteriores a: "<<n->carnet<<endl;
        return true;
    }    
}

bool::ListaSimple::EliminarSolo(Nodo* n){
    if (n == NULL)
    {
        cout<<"Error amigo el nodo que intentas eliminar no existe en la pila"<<endl;
        return false;
    }
    else
    {
        Nodo* aux = first;
        while (aux!=NULL)
        {
            if (aux->siguiente == n)
            {
                aux->siguiente = n->siguiente;
                aux=NULL;
            }
            else
            {
                aux = aux->siguiente;
            }            
        }
        cout<<"Se han borrado el nodo: "<<n->carnet<<endl;
        return true;        
    }    
}



ListaSimple::Nodo*::ListaSimple::Buscar(int carnet){
    Nodo* aux;
    aux = first;
    bool encontrado = false;
    while (!encontrado)
    {
        if(aux->carnet == carnet){
            encontrado = true;
        }else{
            aux = aux->siguiente;
        }
    }
    if(!encontrado){
        return NULL;
        cout<<"Nodo no encontrado"<<endl;
    }else{
        cout<<"Nodo encontrado"<<endl;
        return aux;
    }    
}

bool::ListaSimple::Verificar(){
    if (first == NULL)
    {
        return true;
    }else{
        return false;
    }    
}

void::ListaSimple::Imprimir(){
    Nodo* aux;
    aux = first;
    while (aux != NULL)
    {
        cout<<"Nombre: "<<aux->nombreCompleto<<" Carnet: "<<aux->carnet<<endl;
        aux = aux->siguiente;
    }    
}