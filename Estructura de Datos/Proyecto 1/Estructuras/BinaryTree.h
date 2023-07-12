#include<stdio.h>
#include<iostream>

using namespace std;

template <typename T> struct BinaryTree
{
    template <typename T> struct Nodo
    {
        T* Dato;
        int Key;
        Nodo* HijoDerecho;
        Nodo* HijoIzquierdo;
    };
private:
    Nodo<T>* Raiz;
    int contadorNodos = 0;
    
Nodo<T>* AddRecursive(Nodo<T>* raiz, T* dato, int key)
{
    if (raiz == NULL)
    {
        //SI LA RAIZ (O CUALQUIER NODO RAIZ) ESTA VACIA AÑADIR EL NODO
        Nodo<T>* aux = new Nodo<T>();
        aux->Dato = dato;
        aux->Key = key;
        raiz = aux;
    }
    else if (key < raiz->Key)
    {
        //SI LA LLAVE ES MAS PEQUEÑA INSERTAR EN LOS SUBARBOLES IZQUIERDO
        raiz->HijoIzquierdo = AddRecursive(raiz->HijoIzquierdo,dato,key);
    }
    else if (key > raiz->Key)
    {
        //SI LA LLAVE ES MAS PEQUEÑA INSERTAR EN LOS SUBARBOLES DERECHOS
        raiz->HijoDerecho = AddRecursive(raiz->HijoDerecho,dato,key);
    }
    else
    {
        cout<<"Hay un hijo duplicado \n";
    }
    return raiz;
}
Nodo<T>* RemoveRecursive(Nodo<T>* raiz, int key)
{
    if (raiz == NULL)
    {
        //SI LA RAIZ ES NULA DEVOLVEMOS EL DATO
        return raiz;
    }
    else if (key < raiz->Key)
    {
        //SI LA LLAVE ES MENOR A LA LLAVE DEL NODO RAIZ, SEGUIMOS BUSCANDO POR SUBARBOL IZQUIERDO
        raiz->HijoIzquierdo = RemoveRecursive(raiz->HijoIzquierdo,key);
    }
    else if (key > raiz->Key)
    {
        //SI LA LLAVE ES MENOR A LA LLAVE DEL NODO RAIZ, SEGUIMOS BUSCANDO POR SUBARBOL IZQUIERDO
        raiz->HijoDerecho = RemoveRecursive(raiz->HijoDerecho,key);
    }else{
        //SI SON IGUALES ELIMINAMOS EL NODO
        //SI EL NODO TIENE 0 O UN HIJO
        if (raiz->HijoIzquierdo == NULL)
        {
            return raiz->HijoDerecho;
        }else if (raiz->HijoDerecho == NULL)
        {
            return raiz->HijoIzquierdo;
        }else{
            //SI TIENE 2 HIJOS
            raiz->Key = minValue(raiz->HijoDerecho);
            raiz->Dato = MinValueData(raiz->HijoDerecho);
            raiz->HijoDerecho = RemoveRecursive(raiz->HijoDerecho, raiz->Key);
        }
    }
    return raiz;
}
bool PreOrdenFindRecursive(Nodo<T>* raiz, int key)
{
    if (raiz == NULL)
    {
        //SI LLEGA A NULO, ENTONCES EL DATO NO EXISTE
        return false;
    }
    else
    {
        if (raiz->Key == key)
        {
            //SI EL NODO ES IGUAL A LO QUE SE BUSCA
            return true;
        }
        else if (raiz->Key > key)
        {
            //SI ES MAYOR AL DATO EVALUADO, BUSCAR EL SUB ARBOL DERECHO
            return PreOrdenFindRecursive(raiz->HijoDerecho,key);
        }
        else if (raiz->Key < key)
        {
            //SI ES MAYOR AL DATO EVALUADO, BUSCAR EL SUB ARBOL IZQUIERDO
            return PreOrdenFindRecursive(raiz->HijoIzquierdo,key);
        }
        else
        {
            return false;
        }
    }
}
Nodo<T>* NODELEFTR(Nodo<T>* raiz, int key){
    cout<<"#"<<key;
    if (raiz == NULL)
    {
        //SI LLEGA A NULO, ENTONCES EL DATO NO EXISTE
        return NULL;
    }
    else
    {
        if (raiz->Key == key)
        {
            cout<<"Para "<<key<<"Se retorna";            
            //SI EL NODO ES IGUAL A LO QUE SE BUSCA
            return raiz->HijoIzquierdo;
        }
        else if (raiz->Key > key)
        {
            //SI ES MAYOR AL DATO EVALUADO, BUSCAR EL SUB ARBOL DERECHO
            return NODELEFTR(raiz->HijoDerecho,key);
        }
        else if (raiz->Key < key)
        {
            //SI ES MAYOR AL DATO EVALUADO, BUSCAR EL SUB ARBOL IZQUIERDO
            return NODELEFTR(raiz->HijoIzquierdo,key);
        }
        else
        {
            return NULL;
        }
    }
}

Nodo<T>* NODERIGHTR(Nodo<T>* raiz, int key){
    cout<<"#"<<key;
    if (raiz == NULL)
    {
        //SI LLEGA A NULO, ENTONCES EL DATO NO EXISTE
        return NULL;
    }
    else
    {
        if (raiz->Key == key)
        {
            cout<<"Para "<<key<<"Se retorna";            
            //SI EL NODO ES IGUAL A LO QUE SE BUSCA
            return raiz->HijoDerecho;
        }
        else if (raiz->Key > key)
        {
            //SI ES MAYOR AL DATO EVALUADO, BUSCAR EL SUB ARBOL DERECHO
            return NODERIGHTR(raiz->HijoDerecho,key);
        }
        else if (raiz->Key < key)
        {
            //SI ES MAYOR AL DATO EVALUADO, BUSCAR EL SUB ARBOL IZQUIERDO
            return NODERIGHTR(raiz->HijoIzquierdo,key);
        }
        else
        {
            return NULL;
        }
    }
}
int minValue(Nodo<T>* raiz){
    int min = raiz->Key;
    while (raiz->HijoIzquierdo != NULL)
    {
        min = raiz->HijoIzquierdo->Key;
        raiz = raiz->HijoIzquierdo;
    }
    return min;    
}
T MinValueData(Nodo<T>* raiz){
    T dato = raiz->Dato;
    while (raiz->HijoIzquierdo != NULL)
    {
        dato = raiz->HijoIzquierdo->Dato;
        raiz = raiz->HijoIzquierdo;
    }
    return dato;
}
public:
//CONSTRUCTOR
BinaryTree(){
    Raiz=NULL;
}

//METODO INSERTAR DATOS
void Add(T* dato, int key){
    Raiz = AddRecursive(Raiz,dato,key);
    contadorNodos++;
}

void Remove(int key){
    Raiz = RemoveRecursive(Raiz,key);
    contadorNodos--;
}

bool Find(int key){
    return PreOrdenFindRecursive(Raiz, key);
}

Nodo<T>* NODELEFT(Nodo<T>* Raiz){
    return Raiz->HijoIzquierdo;
}

Nodo<T>* NODERIGHT(Nodo<T>* Raiz){
    return Raiz->HijoDerecho;
}

Nodo<T>* GetRoot(){
    return Raiz;
}

int Size(){
    return contadorNodos;
}

};