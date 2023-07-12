#include "ListaCircularSimple.h"

void ListaCircularSimple::Insertar(string c){
    NodoCircular* aux = new NodoCircular();
    aux->dato = c;
    if (First == NULL)
    {
        //LISTA VACIA
        First = aux;
        Last = aux;
        First->Siguiente = aux;
        Last->Siguiente = aux;
    }else{
        // 0 - 1 - 2
        aux->Siguiente = First;
        Last->Siguiente = aux;
        First = aux;
    }    
}

bool ListaCircularSimple::Vacio(){
    if (First == NULL)
    {
        return true;
    }else{
        return false;
    }
}

ListaCircularSimple::ListaCircularSimple(){
    First = NULL;
    Last= NULL;
}

NodoCircular* ListaCircularSimple::ObtenerFirst(){
    return First;
}

NodoCircular* ListaCircularSimple::ObtenerLast(){
    return Last;
}

int ListaCircularSimple::Size(){
    int contadorindex = 0;
    if (First == NULL)
    {
        return 0;
    }else{
        contadorindex++;
        NodoCircular* aux = First;
        while(aux->Siguiente != First){
            contadorindex++;
            aux=aux->Siguiente;
        }
        return contadorindex;
    }
    
}