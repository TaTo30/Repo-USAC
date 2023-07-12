#include <stdio.h>

class Ficha
{
private:
    char Letra;
    int Puntaje;
public:
    Ficha(char Letra, int Puntaje);
    char GetLetra();
    int GetPuntaje();
};

Ficha::Ficha(char Letra, int Puntaje)
{
    this->Letra = Letra;
    this->Puntaje = Puntaje;
}

char Ficha::GetLetra(){
    return Letra;
}

int Ficha::GetPuntaje(){
    return Puntaje;
}