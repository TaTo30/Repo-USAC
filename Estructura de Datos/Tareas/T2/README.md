# TDA MONEDA
##### Moneda
constructor de la moneda, de instanciarce sin valores se agregara sus atributos 'predeterminados', de lo contrario, se instanciara con los valores que el usuario decida (nombre, simbolo, cantidad de monedas en reserva)

>Moneda()
>Moneda(**string** nombre, **string** simbolo, **int** cantidad )


##### Transferir
Transfiere a una persona una cantidad de monedas si las hay disponibles en la reserva, caso contrario no se transferirá las monedas

>Transferir(**Persona** A, **int** cantidad)

##### TransferirFrom
Transfiere monedas de una persona a otra sin tocar la reserva de monedas 'disponibles', si la persona al que se extrae las monedas no tiene suficientes la transferencia se detiene

>TransferirFrom(**Persona** From, **Persona** To, **int** cantidad)

##### Estado de Cuenta
Devuelve cual es la cantidad de monedas de una persona cualquiera.

>SolicitarEstadoCuenta(**Persona** A)

##### Reserva de Monedas
Devuelve cual es la cantidad de monedas disponibles para realizar una transferencia

>ReservaMonedas()