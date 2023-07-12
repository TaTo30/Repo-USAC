#include "Persona.h"

Persona::Persona(string nombre_) {
	nombre = nombre_;
	monedas_posesion = 0;
}

string Persona::ObtenerNombre() {
	return nombre;
}

int Persona::ObtenerMonedas() {
	return monedas_posesion;
}

void Persona::AnadirMonedas(int cantidad) {
	monedas_posesion += cantidad;
}

void Persona::ExtraerMonedas(int cantidad) {
	monedas_posesion -= cantidad;
}