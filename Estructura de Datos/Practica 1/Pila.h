#include "NodoPila.h"


using namespace std;

class Pila
{
private:
    NodoPila* Header;
 
public:
    void Push(string B, string R);
    NodoPila* Pop();
    bool Vacio();
    void Vaciar();
    Pila();
};

