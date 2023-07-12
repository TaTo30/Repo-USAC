#include "ListaDoble.h"

ListaDoble::ListaDoble(){
    First = NULL;
    Last = NULL;
}

/*************************************************************************
    METODOS PARA LA INSERSION DE DATOS EN LA LISTA DOBLEMENTE ENLAZADA
*************************************************************************/
//Metodo privado general para insertar datos
void ListaDoble::Insertar(Nodo* n, char dato_, bool pos){
    Nodo* aux = new Nodo();
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
        }else if(n == First && pos){
            //INSERTAR INICIO DE LA LISTA
            aux->Siguiente = First;
            First->Anterior = aux;
            First = aux;
        }else if(n == Last && !pos){
            //INSERTAR FINAL DE LA LISTA
            aux->Anterior = Last;
            Last->Siguiente = aux;
            Last = aux;
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
//Metodo publico para insertar al inicio de la lista
void ListaDoble::InsertarFirst(char dato){
    Insertar(First, dato, true);
}
//Metodo publico para insertar al final de la lista
void ListaDoble::InsertarLast(char dato){
    Insertar(Last, dato, false);
}
//Metodo publico para insertar antes de un nodo dado (el nodo se obtiene con los metodos de buscar)
void ListaDoble::InsertarBeforeAt(Nodo* n, char dato){
    Insertar(n,dato, true);
}
//Metodo publico para insertar despues de un nodo dado (el nodo se obtiene con los metodos de buscar)
void ListaDoble::InsertarAfterAt(Nodo* n, char dato){
    Insertar(n, dato, false);
}


/***************************************************************************
    METODOS PARA LA ELIMINACION DE DATOS EN LA LISTA DOBLEMENTE ENLAZADA
***************************************************************************/
void ListaDoble::Eliminar(Nodo* n){
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
            First->Anterior = NULL;
        }
        else if (n == Last)
        {
            //ELIMINAR EL SEGUNDO NODO
            Last = Last->Anterior;
            Last->Siguiente = NULL;
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

/***********************************************************************
    METODOS PARA LA BUSCAR DE DATOS EN LA LISTA DOBLEMENTE ENLAZADA
            TODOS LOS METODOS DE BUSCAR DEVUELVEN UN NODO
***********************************************************************/
//Metodo publico para buscar un nodo SEGUN: su contenido
Nodo* ListaDoble::Buscar(char dato){
    Nodo* aux = First;
    Nodo* encontrado=NULL;
    while (aux != NULL)
    {
        if (aux->dato == dato)
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
Nodo* ListaDoble::Buscar(int index){
    Nodo* aux = First;
    Nodo* encontrado = NULL;
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
//Metodo publico para buscar un nodo SEGUN: el primer caracter de una cadena
Nodo* ListaDoble::Buscar(string s){
    Nodo* aux = First;
    Nodo* encontrado = NULL;
    char a = s[0];
    while (aux !=NULL)
    {
        if (aux->dato == a)
        {
            encontrado = aux;
            aux=NULL;
        }else{
            aux = aux->Siguiente;
        }        
    }
    return encontrado;
}
//Metodo publico para buscar un nodo SEGUN: el primero siempre
Nodo* ListaDoble::ObtenerFirst(){
    return First;
}
//Metodo publico para buscar un nodo SEGUN: el ultimo siempre
Nodo* ListaDoble::ObtenerLast(){
    return Last;
}
//Metodo publico para obtener index de un nodo buscado por contenido
int ListaDoble::ObtenerIndex(){
    return 0;
}
/***********************************************************************
    METODOS PARA LA COMPROBACION DE DATOS Y DESPLIEGE EN PANTALLA
***********************************************************************/
//Verifica que la lista este vacia
bool ListaDoble::Verificar(){
    if (First == NULL)
    {
        return false;
    }else{
        return true;
    }
    
}
//Verifica el tamanio de la lista
int ListaDoble::Size(){
    Nodo* aux = First;
    int contador=0;
    while (aux != NULL)
    {
        contador++;
        aux=aux->Siguiente;
    }
    return contador;    
}
//Vacia la lista para volverse a iniciar
void ListaDoble::Vaciar(){
    First= NULL;
    Last = NULL;
}
/*Imprime todo el contenido de la lista
void ListaDoble::Imprimir(){
    Nodo* aux= First;
    while (aux != NULL)
    {
        cout<<aux->dato;
        aux=aux->Siguiente;
    }    
}
//Enlista los caracteres de la lista
void ListaDoble::Enlistar(){
    Nodo* aux= First;
    cout<<endl<<endl<<"Listado:"<<endl;
    while (aux != NULL)
    {       
        cout<<aux->dato;
        aux=aux->Siguiente;        
    }
}*/