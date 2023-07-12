#include <iostream>
#include <string.h>

using namespace std;

class NodoCircular
{
public:
    NodoCircular();
    NodoCircular* Siguiente;
    string dato;

};

NodoCircular::NodoCircular()
{
    this->dato = "";
    this->Siguiente = NULL;
}
