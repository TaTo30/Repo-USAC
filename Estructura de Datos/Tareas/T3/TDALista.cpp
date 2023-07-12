#include <iostream>
#include <string>
#include "ListaSimple.cpp"

using namespace std;

int main()
{
    cout<<"Bienvenido"<<endl;
    ListaSimple* listado = new ListaSimple();
    listado->Insertar("Persona 1",201800585);
    listado->Insertar("Persona 2",201800586);
    listado->Insertar("Persona 3",201800587);
    listado->Insertar("Persona 4",201800588);
    listado->Insertar("Persona 5",201800589);
    listado->Insertar("Persona 6",201800590);
    listado->Insertar("Persona 7",201800591);
    listado->Insertar("Persona 8",201800592);
    listado->Insertar("Persona 9",201800593);
    listado->Insertar("Persona 10",201800594);
    listado->Imprimir();
    listado->EliminarPre(listado->Buscar(201800593));
    listado->Insertar("Persona 11", 201800595);
    listado->EliminarPost(listado->Buscar(201800587));
    listado->Imprimir();
    listado->EliminarSolo(listado->Buscar(201800588));
    listado->Imprimir();
    cin.ignore();
    cin.get();

}