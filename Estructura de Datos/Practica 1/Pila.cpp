#include "Pila.h"

Pila::Pila(){
    Header=NULL;
}

void Pila::Push(string B, string R){
    NodoPila* aux = new NodoPila();
    aux->buscar=B;
    aux->reemplazar=R;
    if (Header == NULL)
    {
        //PILA VACIA
        Header = aux;
    }else{
        //PILA CON DATOS
        aux->Siguiente = Header;
        Header = aux;
    }   
}

NodoPila* Pila::Pop(){
    NodoPila* c = Header;
    Header = Header->Siguiente;
    return c;
}

bool Pila::Vacio(){
    if (Header == NULL)
    {
        return true;
    }else{
        return false;
    }    
}

void Pila::Vaciar(){
    Header = NULL;
}