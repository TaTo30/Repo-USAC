#pragma once
#include<iostream>
 
using namespace std;

class Persona
{
private:
	string nombre;
	int monedas_posesion;

public:
	Persona(string nombre_);
	string ObtenerNombre();
	int ObtenerMonedas();
	void AnadirMonedas(int cantidad);
	void ExtraerMonedas(int cantidad);
};

