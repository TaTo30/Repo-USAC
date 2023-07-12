# VueloQuetzal
![Document Version](https://img.shields.io/badge/Version-2.0-green.svg)
![SQL Syntax](https://img.shields.io/badge/Syntax-PostgreSQL-blue.svg)



## Entidades

---

## Nomenclatura de Restricciones

>   ***NN***: NOT NULL
> 
>   ***PK***: PRIMARY KEY
> 
>   ***FK***: FOREIGN KEY
>  
>   ***UQ***: UNIQUE KEY
> 
>   ***(\*)***: CHECK KEY

---

## **Aviones**
Para que VueloQuetzal pueda llevar un correcto control de sus prestaciones es necesario de primero tener un excelente control y registro acerca de los aviones que se disponen y todas sus caracteristicas.

* **Atributos**

| Nombre                | Tipo                    | Descripcion                                                                                                                                                                                   |
| --------------------- | ----------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| NO_Vuelo              | ***PK, NN*** - SERIAL   | Identificador Unico usado por el DBMS para control e indexacion                                                                                                                               |
| Modelo                | VARCHAR(10)             | Modelo del avion                                                                                                                                                                              |
| Matricula             | VARCHAR(15)             | Matricula que lo identifica en el exterior                                                                                                                                                    |
| No_Primera_Clase      | SMALLINT                | La cantidad de asientos de primera clase que el avion dispone                                                                                                                                 |
| No_Clase_Economica    | SMALLINT                | La cantidad de asientos de clase ejecutiva que el avion dispone                                                                                                                               |
| No_Clase_Ejecutiva    | SMALLINT                | La cantidad de asientos de clase economica que el avion dispone                                                                                                                               |
| Ultimo_Mantenimiento  | DATE                    | Fecha de la ultima vez que se le realizo mantenimiento al avion, esta debe ser obtenida mediante programacion de los datos en la entidad **Mantenimiento**                                    |
| Proximo_Mantenimiento | DATE                    | Fecha del proximo mantenimiento del avion, esta debe ser calculada usando los datos de la entidad **Mantenimiento** y el tiempo estandar de mantenimiento segun las politicas de VueloQuetzal |
| Max_Combustible       | ***NN*** - DECIMAL(8,2) | La cantidad maxima de combustible (Galones) que puede ser cargado al avion                                                                                                                    |
| Max_Distancia         | ***NN*** - DECIMAL(8,2) | La cantidad maxima de Kilometros que puede recorrer un avion                                                                                                                                  |
| Max_Altura            | ***NN*** - DECIMAL(8,2) | La cantidad maxima de Kilometros al que puede elevarse un avion                                                                                                                               |

* **Relaciones**

| Nombre       | Tipo                   | Descripcion                                                                                                                                     |
| ------------ | ---------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------- |
| Ultimo_Vuelo | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Avion** con la entidad **Vuelo**, que especifica que vuelo fue el ultimo vuelo que realizo el avion. |

---

## **Mantenimiento**
Dada la alta importancia que supone mantener los aviones en buen estado, se deber llevar un registro y control de todas las veces que se hace mantenimiento a los aviones

* **Atributos**

| Nombre           | Tipo                  | Descripcion                                                                                                                              |
| ---------------- | --------------------- | ---------------------------------------------------------------------------------------------------------------------------------------- |
| ID_Mantenimiento | ***PK, NN*** - SERIAL | Identificador Unico usado por el DBMS para control e indexacion                                                                          |
| Fecha_Finalizada | ***NN*** - DATE       | Fecha en que el mantenimiento fue terminado                                                                                              |
| Tipo?            | ***NN*** - BOOLEAN    | Atributo que define que tipo de mantenimiento se realizo, `True` = *'Mantenimiento Periodico'* : `False` = *'Mantenimiento Excepcional'* |
| Falla?           | ***NN*** - BOOLEAN    | Atributo que define si hubo una falla mecanica o mantenimiento basico `True` = *'Hay falla'* : `False` = *'No hay Falla'*                |
| Observaciones    | VARCHAR(255)          | Breve resumen del mantenimiento y algunas observaciones a considerar del estado del avion                                                |

* **Relaciones**

| Nombre   | Tipo                   | Descripcion                                                                                                                                |
| -------- | ---------------------- | ------------------------------------------------------------------------------------------------------------------------------------------ |
| NO_Vuelo | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Mantenimiento** con la entidad **Avion**, que indica a que avion se le realizo el mantenimiento |

---

## **Empleado**
Para que la empresa pueda sobresalir en el mercado y ofrecer un excelente servicion, es necesario que esta tenga empleados competentes, por ello es necesario el control y registro de los empleados que actualmente operan dentro de VueloQuetzal.

| Nombre             | Tipo                  | Descripcion                                                     |
| ------------------ | --------------------- | --------------------------------------------------------------- |
| ID_Empleado        | ***PK, NN*** - SERIAL | Identificador Unico usado por el DBMS para control e indexacion |
| Nombre             | VARCHAR(25)           | Nombre del empleado                                             |
| Apellido           | VARCHAR(35)           | Apellido del empleado                                           |
| Edad               | SMALLINT              | Edad actual del empleado                                        |
| CUI                | BIGINT                | Correlativo unico de identificacion del empleado                |
| Direccion          | VARCHAR(125)          | Direccion donde reside el empleado                              |
| Fecha_Nacimiento   | DATE                  | Fecha de nacimiento del empleado                                |
| Correo             | VARCHAR(125)          | Correo electronico del empleado                                 |
| Telefono           | INTEGER               | Numero telefonico del empleado                                  |
| Idiomas            | VARCHAR(255)          | Los idiomas que puede hablar el empleado                        |
| Fecha_Contratacion | DATE                  | Fecha de cuando el empleado fue contratado                      |
| Hora_Inicio        | TIME                  | Horario de inicio de labores para el empleado                   |
| Hora_Final         | TIME                  | Horario de finalizacion de labores para el empleado             |

| Nombre     | Tipo                   | Descripcion |
| ---------- | ---------------------- | ----------- |
| ID_Puesto  | ***FK, NN*** - INTEGER |             | Ultimo_Vuelo | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Empleado** con la entidad **Puesto de Trabajo**, y que indica en que puesto de trabajo fue asignado el empleado     |
| ID_Jornada | ***FK, NN*** - INTEGER |             | Ultimo_Vuelo | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Empleado** con la entidad **Jornada Laboral**, y que indica la jornada de trabajo que tiene que cumplir el empleado |

---


## **Puesto de Trabajo**
Dada la necesidad y demanda de trabajo, es deseoso tener registrado que puestos de trabajo puede ofrecer la empresa de cara a nuevos empleados.

* **Atributos**

| Nombre      | Tipo                   | Descripcion                                                     |
| ----------- | ---------------------- | --------------------------------------------------------------- |
| ID_Puesto   | ***PK, NN*** - SERIAL  | Identificador Unico usado por el DBMS para control e indexacion |
| Nombre      | ***NN*** - VARCHAR(50) | Nombre del puesto de trabajo                                    |
| Descripcion | VARCHAR(255)           | Resume a grandes rasgos las funciones del puesto de trabajo     |
|Salario|***NN*** - MONEY | El salario base que el empleado recibira por emplear en este puesto

---

## **Jornada Laboral**
Dado lo flexible que pueden ser los horarios de los trabajadores en el mercado de aviacion, se deben registrar las jornadas laborales a las que se pueden ofrecer puestos.

* **Atributos**

| Nombre     | Tipo                   | Descripcion                                                     |
| ---------- | ---------------------- | --------------------------------------------------------------- |
| ID_Jornada | ***PK, NN*** - SERIAL  | Identificador Unico usado por el DBMS para control e indexacion |
| Nombre     | ***NN*** - VARCHAR(50) | Nombre de la jornada laboral                                    |

---

## **Retiros, Despidos, Jubilaciones, etc**
Define la estructura de datos para el registro de empleados que por decision administrativa o propia del empleado, se haya que tenido que retirar de la empresa.

* **Atributos**

| Nombre    | Tipo                  | Descripcion                                                                          |
| --------- | --------------------- | ------------------------------------------------------------------------------------ |
| ID_Retiro | ***PK, NN*** - SERIAL | Identificador Unico usado por DBMS para control e indexacion                         |
| Fecha     | ***NN*** - DATE       | Fecha en que el empleado fue retirado de la plantilla                                |
| Motivo    | VARCHAR(255)          | Espacio para explicar el motivo por el cual el empleado fue retirado de la plantilla |

* **Relaciones**

| Nombre      | Tipo                   | Descripcion                                                                                                                         |
| ----------- | ---------------------- | ----------------------------------------------------------------------------------------------------------------------------------- |
| ID_Empleado | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Retiro** con la entidad **Empleado**, que indica que empleado fue revocado de la empresa |

---

## **Asistencia**
Para llevar control de empresarial, es sumamente necesario llevar control de asistencia de los empleados

* **Atributo**

| Nombre        | Tipo                  | Descripcion                                                     |
| ------------- | --------------------- | --------------------------------------------------------------- |
| ID_Asistencia | ***UQ, NN*** - SERIAL | Identificador Unico usado por el DBMS para control e indexacion |
| Fecha         | ***NN*** - DATE       | Fecha en que un empleado registra una asistencia                |
| Entrada       | ***NN*** - TIME       | Hora en el que empleado registro sus asistencia                 |

* **Relaciones**

| Nombre      | Tipo                   | Descripcion                                                                                                                          |
| ----------- | ---------------------- | ------------------------------------------------------------------------------------------------------------------------------------ |
| ID_Empleado | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Asistencia** con la entidad **Empleado**, que indica que empleado registro su asistencia. |

---

## **Inasistencia**
Si un empleado que por cualesquiera motivo no asiste a laborar, se debe dejar registrado toda la informacion del porque.

* **Atributos**

| Nombre          | Tipo                  |
| --------------- | --------------------- |
| ID_Inasistencia | ***UQ, NN*** - SERIAL | Identificador Unico usado por el DBMS para control e indexacion |
| Fecha           | ***NN*** - DATE       | Fecha en que un empleado registra una asistencia                |
| Motivo          | VARCHAR(255)          | Resumen del porque el empleado no asistio en la fecha indicada  |

* **Relaciones**
  
| Nombre      | Tipo                   | Descripcion                                                                                                                            |
| ----------- | ---------------------- | -------------------------------------------------------------------------------------------------------------------------------------- |
| ID_Empleado | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Inasistencia** con la entidad **Empleado**, que indica que empleado registro su asistencia. |

---

## **Cliente**
Define la estructura de datos para el registro de clientes que hayan usado por lo menos una vez el servicio de la empresa.

* **Atributo**

| Nombre           | Tipo                   | Descripcion                                                  |
| ---------------- | ---------------------- | ------------------------------------------------------------ |
| ID_Cliente       | ***PK, NN*** - SERIAL  | Identificador Unico usado por DBMS para control e indexacion |
| Nombre           | ***NN*** - VARCHAR(25) | Nombre del cliente                                           |
| Apellido         | ***NN*** - VARCHAR(35) | Apellido del cliente                                         |
| Edad             | ***NN*** - SMALLINT    | Edad actual del cliente                                      |
| CUI              | ***NN*** - BIGINT      | Correlativo unico de identificacion del cliente              |
| Fecha_Nacimiento | DATE                   | Fecha de nacimiento del cliente                              |
| Correo           | VARCHAR(25)            | Correo electronico del cliente                               |
| Telefono         | INTEGER                | Numero telefonico del cliente                                |
| Direccion        | VARCHAR(125)           | Direccion de la residencia del cliente                       |
| Codigo_Postal    | INTEGER                | codigo postal del cliente                                    |
| Pasaporte        | ***NN*** - BIGINT      | Numero de pasaporte del cliente                              |

---

## **Lugar**
Como empresa de vuelo que presta servicios en distintas ciudades del mundo, es de suma importancia tener el registro y control de todas aquellas ciudades a la que VueloQuetzal puede operar.

* **Atributos**

| Nombre   | Tipo                  | Descripcions                                                    |
| -------- | --------------------- | --------------------------------------------------------------- |
| ID_Lugar | ***PK, NN*** - SERIAL | Identificador Unico usado por el DBMS para control e indexacion |
| Pais     | ***NN*** VARCHAR(50)  | Pais donde se ubica la ciudad                                   |
| Ciudad   | ***NN*** VARCHAR(50)  | El nombre de la ciudad donde opera la agencia                   |

---

## **Formulario**
Para que la agencia pueda prestar servicio es necesario un formulario que un cliente debe llenar para la contratacion del servicio de vuelos.

* **Atributos**

| Nombre        | Tipo                  | Descripcion                                                     |
| ------------- | --------------------- | --------------------------------------------------------------- |
| ID_Formulario | ***PK, NN*** - SERIAL | Identificador unico usado por el DBMS para control e indexacion |
| Monto         | ***NN*** - MONEY      | Precio del boleto que el cliente debe pagar                     |
| Fecha_Salida  | DATE                  | Fecha en que el avion sale hacia el destino                     |
| Fecha_Retorno | DATE                  | Fecha en que el avion regresa desde el destino                  |

* **Relaciones**

| Nombre     | Tipo                   | Descripcion                                                                                                                             |
| ---------- | ---------------------- | --------------------------------------------------------------------------------------------------------------------------------------- |
| ID_Cliente | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Relaciones** con la entidad **Cliente**, que indica que cliente ha llenado el formulario     |
| ID_Origen  | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Relaciones** con la entidad **Lugar**, que indica desde que ciudad parte el vuelo            |
| ID_Destino | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Relaciones** con la entidad **Lugar**, que indica hacia que ciudad llega el vuelo            |
| ID_Clase   | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Relaciones** con la entidad **Clase de Asiento**, que indica que asiento solicito el cliente |


### **Clases de Asiento**
Se deben poder registrar todo tipo de asientos que puede ofrecer un avion.

* **Atributos**

| Nombre   | Tipo                   | Descripcion                                                        |
| -------- | ---------------------- | ------------------------------------------------------------------ |
| ID_Clase | ***PK, NN*** - SERIAL  | Identificador Unico usado por el DBMS para el control e indexacion |
| Nombre   | ***NN*** - VARCHAR(25) | Nombre del tipo de clase de asiento                                |

---

## **Boleto**
La agencia de vuelo necesita registar todos los boletos que han sido vendidos a los clientes.

* **Atributos**

| Nombre              | Tipo                   |Descripcion
| ------------------- | ---------------------- |-
| NO_Boleto           | ***PK, NN*** - SERIAL  |Identificador Unico usado por el DBMS para el control e indexacion
| Peso_Equipaje       | INTEGER                |Peso el kilogramos del equipaje que cargara el cliente en el avion
| Restricciones       | VARCHAR(255)           |Algunas restricciones impuestas al cliente
| Fecha_Restricciones | DATE                   |Ultima fecha que tiene el cliente para atender sus restricciones
| Directo?            | ***NN*** - BOOLEAN     |Atributo que define si el vuelo es directo o a escala,  `True` = *'Directo'* : `False` = *'Escala'*

* **Relaciones**

|Nombre |Tipo|Descripcion
|-|-|-
| ID_Cliente          | ***FK, NN*** - INTEGER |**Llave Foranea**, que indexa la entidad **Boleto** con la entidad **Cliente**, y que indica a que cliente fue vendido el boleto
| NO_Vuelo            | ***FK, NN*** - INTEGER |**Llave Foranea**, que indexa la entidad **Boleto** con la entidad **Avion**, y que indica que avion realizara el vuelo

---

## **Detalle Boleto Ciudades**
Entidad auxiliar de la entidad boleto, define los datos de todas las paradas correspondientes a un vuelo, en el caso de ser vuelo en escala se podran almacenar mas ciudades aparte de las ciuades que corresponden al vuelo directo (Ciudad de Salida y Ciudad de Aterrizaje).

* **Relaciones**

| Nombre    | Tipo                   |Descripcion
| --------- | ---------------------- |-
| NO_Boleto | ***FK, PK*** - INTEGER |**Llave Foranea**, que indexa la entidad **Detalle** con la entidad **Boleto**, y que indica el boleto a detallar
| ID_Lugar  | ***FK, PK*** - INTEGER |**Llave Foranea**, que indexa la entidad **Detalle** con la entidad **Lugar**, y que indica el lugar a detallar

---

## **Pago**
Estructura que define los datos que se registraran, en el momento de que un cliente page su boleto de avion.

* **Atributos**

| Nombre     | Tipo                   |Descripcion
| ---------- | ---------------------- |-
| NO_Pago    | ***PK, NN*** - SERIAL  |Identificador Unico usado por el DBMS para control e indexacion
| No_Tarjeta | ***NN*** - INTEGER     |Tarjeta con el que cliente realizo el pago de algun boleto
| Credito?   | ***NN*** - BOOLEAN     |Atributo que define si la tarjeta con la que se pago es de credito o de debito, `True` = *'Credito'* : `False` = *'Debito'* 
| Monto      | ***NN*** - MONEY       |Monto de dinero que fue padado

* **Relaciones**

|Nombre|Tipo|Descripcion
|-|-|-
| NO_Boleto  | ***FK, NN*** - INTEGER |**Llave Foranea**, que indexa la entidad **Pago** con la entidad **Boleto**, y que indica que boleto fue pagado

---

## **Vuelo**
Entidad que define los datos a registrar para los vuelos que se hayan realizado o se vayan a realizar en proximos momentos.

* **Atributos**

| Nombre        | Tipo                   |Descripcion
| ------------- | ---------------------- |-
| ID_Vuelo      | ***PK, NN*** - SERIAL  |Identificador Unico usado por el DBMS para control e indexacion
| Cnt_Pasajeros | ***NN*** - SMALLINT    |Establece cuantos pasajeros hay programados para abordar el vuelo
| Despegue      | ***NN*** - TIMESTAMP   |Fecha y hora en que el avion despega hacia el destino
| Aterrizaje    | ***NN*** - TIMESTAMP   |Fecha y hora en que el avion aterriza en el destino

* **Relaciones**

|Nombre|Tipo|Descripcion
|-|-|-|
| NO_Vuelo      | ***FK, NN*** - INTEGER |**Llave Foranea**, que indexa la entidad **Vuelo** con la entidad **Avion**, que indica que avion realizara el vuelo
| Destino       | ***FK, NN*** - INTEGER |**Llave Foranea**, que indexa la entidad **Vuelo** con la entidad **Lugar**, que indica por que ciudades paso el vuelo, si fuera a escala

---

## **Detalle_Tripulacion**
Entidad auxiliar que detalla los tripulantes encargados de llevar el vuelo, como piloto, copiloto, cocineros, azafatas, etc.

* **Relaciones**

| Nombre      | Tipo                   |Descripcion
| ----------- | ---------------------- |-
| ID_Vuelo    | ***FK, NN*** - INTEGER |**Llave Foranea**, que indexa la entidad **Detalle** con la entidad **Vuelo**, que indica por el vuelo a detallar
| ID_Empleado | ***FK, NN*** - INTEGER |**Llave Foranea**, que indexa la entidad **Detalle** con la entidad **Empleado**, que indica los empleados que atienden el vuelo

