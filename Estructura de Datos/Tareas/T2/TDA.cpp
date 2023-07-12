
#include <iostream>
#include <stdio.h>
#include <stdlib.h>
#include "Persona.h"
#include "Moneda.h"

int main(void)
{
	//PSD::
	/*
		FELIZ DIA DEL INGENIERO!!!
	*/
	Moneda coin("Tu Gfa Coin", "TGC", 1000);
	Persona A("Goku SSJ fase 3");
	Persona B("Ozuna bb, como estas??");
	Persona C("sale en vacas gg");
	coin.Transferir(A, 30);
	coin.Transferir(B, 10000);
	coin.SolicitarEstadoCuenta(A);
	coin.TransferirFrom(B, A, 100);

	cin.ignore();
	cin.get();
}

