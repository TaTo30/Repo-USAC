#include <iostream>
#include <stdio.h>
#include "Nodo.h"

using namespace std;

class ListaDoble
{   
private:   
    Nodo* First;
    Nodo* Last;
    void Insertar(Nodo* n, char dato, bool pos);
    

public:   
    void InsertarFirst(char dato);
    void InsertarLast(char dato);
    void InsertarBeforeAt(Nodo* n, char dato);
    void InsertarAfterAt(Nodo* n, char dato);    
    Nodo* Buscar(char c);
    Nodo* Buscar(int index);
    Nodo* Buscar(string s);
    Nodo* ObtenerFirst();
    Nodo* ObtenerLast();
    int ObtenerIndex();
    void Eliminar(Nodo* n);
    void Imprimir();
    void Enlistar();
    bool Verificar();
    int Size();
    void Vaciar();
    ListaDoble();
};
