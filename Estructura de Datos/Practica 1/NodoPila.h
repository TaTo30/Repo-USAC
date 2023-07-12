
#include <stdio.h>
#include <string.h>
#include <iostream>

using namespace std;
class NodoPila
{
public:
    string buscar;
    string reemplazar;
    NodoPila* Siguiente;
    NodoPila();
};

NodoPila::NodoPila()
{
   // this->dato = NULL;
    this->Siguiente = NULL;
    this->buscar = "";
    this->reemplazar = "";
}
