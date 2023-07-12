#include "NodoCircular.h"
#include <iostream>
#include <stdio.h>


class ListaCircularSimple
{
private:
    NodoCircular* First;
    NodoCircular* Last;
public:
    void Insertar(string c);
    bool Vacio();
    NodoCircular* ObtenerFirst();
    NodoCircular* ObtenerLast();
    int Size();
    ListaCircularSimple();
};
