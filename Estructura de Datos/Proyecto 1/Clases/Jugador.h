#include <stdio.h>
#include <string>
#include "../Estructuras/OrderList.h"
#include "../Estructuras/LinkedListP.h"
#include "Ficha.h"

using namespace std;

class Jugador
{
private:
    string Nombre;
    OrderList<int>* Puntajes = new OrderList<int>();
    LinkedListP<Ficha>* Fichas = new LinkedListP<Ficha>();
public:
    
    Jugador(string Nombre)
    {
        this->Nombre = Nombre;
    }

    LinkedListP<Ficha>* GetFichasPlayer(){
        return Fichas;
    }

    OrderList<int>* GetPuntajesPlayer(){
        return Puntajes;
    }

    string GetNombre(){
        return Nombre;
    }
};

