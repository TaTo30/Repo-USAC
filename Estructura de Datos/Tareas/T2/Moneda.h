#pragma once
#include<iostream>
#include<vector>
#include<string>
#include"Persona.h"

using namespace std;

class Moneda
{
private:
	int cantidad_monedas_reserva;
	std::string simbolo_moneda;
	std::string nombre_moneda;

public:
	Moneda();
	Moneda(string nombre, string simbolo, int cantidad);
	void Transferir(Persona A, int cantidad);
	void TransferirFrom(Persona From, Persona To, int cantidad);
	void SolicitarEstadoCuenta(Persona A);
	void ReservaMonedas();

};

