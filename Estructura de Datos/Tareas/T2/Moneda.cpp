#include "Moneda.h"


Moneda::Moneda() {
	cantidad_monedas_reserva = 10000;
	nombre_moneda = "TatoCoin";
	simbolo_moneda = "$T.";
	cout << "La moneda " << nombre_moneda << " (" << simbolo_moneda << ") ha entrado en circulacion con: " << simbolo_moneda << cantidad_monedas_reserva << " " << nombre_moneda << "s"<<endl;
}

Moneda::Moneda(string nombre, string simbolo, int cantidad) {
	cantidad_monedas_reserva = cantidad;
	nombre_moneda = nombre;
	simbolo_moneda = simbolo;
	cout << "La moneda " << nombre_moneda << " (" << simbolo_moneda << ") ha entrado en circulacion con: " << simbolo_moneda << cantidad_monedas_reserva << " " << nombre_moneda << "s"<<endl;
}

void Moneda::Transferir(Persona A, int cantidad) {
	if (cantidad <= cantidad_monedas_reserva)
	{
		A.AnadirMonedas(cantidad);
		cantidad_monedas_reserva -= cantidad;
		cout << "A " << A.ObtenerNombre() << " se le han transferido una cantidad de: " << simbolo_moneda << cantidad << " ahora tiene " << simbolo_moneda << A.ObtenerMonedas()<<endl;
	}
	else {
		cout << "No hay suficientes monedas en circulacion para transferirle a " << A.ObtenerNombre()<<endl;
	}
}

void Moneda::TransferirFrom(Persona From, Persona To, int cantidad) {
	if (cantidad <= cantidad_monedas_reserva)
	{
		if (cantidad <= From.ObtenerMonedas())
		{
			From.ExtraerMonedas(cantidad);
			To.AnadirMonedas(cantidad);
			cout << From.ObtenerNombre() << " le ha transferido a " << To.ObtenerNombre() << " la generosa cantidad de " << simbolo_moneda << cantidad << endl;
			cout << From.ObtenerNombre() << " ahora tiene " << simbolo_moneda<<From.ObtenerMonedas() << endl;
			cout << To.ObtenerNombre() << " ahora tiene " <<simbolo_moneda<< To.ObtenerMonedas() << endl;
		}
		else {
			cout << From.ObtenerNombre() << " no tiene suficientes monedas para transferirle a " << To.ObtenerNombre()<<endl;
		}
	}
	else {
		cout << "La cantidad que se quiere transferir excede la cantidad actual de monedas en circulacion";
	}
}

void Moneda::SolicitarEstadoCuenta(Persona A) {
	cout << A.ObtenerNombre() << " tiene actualmente " << simbolo_moneda << A.ObtenerMonedas()<<endl;
}

void Moneda::ReservaMonedas() {
	cout << "Hay en reserva " << simbolo_moneda << cantidad_monedas_reserva;
}

