
class Nodo
{
    public:
        char dato;
        Nodo* Anterior;
        Nodo* Siguiente;
        Nodo();
};

Nodo::Nodo(){
    this->Anterior = NULL;
    this->Siguiente = NULL;
    this->dato = NULL;
}

