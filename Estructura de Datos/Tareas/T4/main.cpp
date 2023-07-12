#include "ListaDoble.cpp"
#include "Pila.cpp"
#include "ListaCircularSimple.cpp"
#include <stdio.h>

using namespace std;

int main()
{
    ListaDoble* lista = new ListaDoble();
    lista->InsertarLast('a');
    lista->InsertarLast('b');
    lista->InsertarFirst('e');
    lista->InsertarFirst('f');
    lista->InsertarAfterAt(lista->Buscar('a'),'h');
    lista->InsertarBeforeAt(lista->Buscar('c'), '8');
    lista->InsertarBeforeAt(lista->Buscar(1),'t');
    lista->InsertarAfterAt(lista->Buscar(2), 'p');
    cout<<lista->Buscar("besar")->dato<<endl;
    cout<<lista->Size()<<endl;
    lista->Eliminar(lista->ObtenerFirst());
    lista->Eliminar(lista->ObtenerLast());
    lista->Eliminar(lista->Buscar('a'));
    lista->Imprimir();

    /*Pila* pila = new Pila();
    pila->Push('a');
    pila->Push('b');
    pila->Push('c');
    pila->Push('d');
    pila->Push('e');
    cout<<pila->Pop();
    cout<<pila->Pop();
    ListaCircularSimple* lista = new ListaCircularSimple();
    lista->Insertar('a');
    lista->Insertar('b');
    lista->Insertar('c');*/
    

    
}
