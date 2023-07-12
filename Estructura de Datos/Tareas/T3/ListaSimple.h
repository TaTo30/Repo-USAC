//#include "Nodo.h"


using namespace std;


class ListaSimple
{   
    class Nodo
    {
    public:
        string nombreCompleto;
        int carnet;
        Nodo* siguiente;
    };
private:
    Nodo* first;
public:
    void Insertar(string nombre, int id);
    bool EliminarPre(Nodo* n);
    bool EliminarPost(Nodo* n);
    bool EliminarSolo(Nodo* n);
    Nodo* Buscar(int index);
    bool Verificar();
    void Imprimir();
};
