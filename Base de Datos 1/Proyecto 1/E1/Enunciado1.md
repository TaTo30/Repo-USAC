# Aseguradora Vida Segura

![Document Version](https://img.shields.io/badge/Version-2.0-green.svg)
![SQL Syntax](https://img.shields.io/badge/Type-PostgreSQL-blue.svg)



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

## **Area**



Dado que una la empresa tiene diferentes areas de trabajo es de suma importancia tener un registrado y controlado cada area para una mayor eficiencia administrativa y operativa.

* **Atributos**:

| Nombre      | Tipo                   | Descripcion                                                                       |
| ----------- | ---------------------- | --------------------------------------------------------------------------------- |
| ID_Area     | ***PK, NN*** - SERIAL  | Identificador Unico del area usado por DBMS internamente para indexacion.         |
| Nombre      | ***NN*** - VARCHAR(25) | Especifica el nombre del area de trabajo de la empresa, ej. "Area de Informatica" |
| Descripcion | VARCHAR(255)           | Breve descripcion de las principales funciones del area de trabajo                |

---

## **Departamento**
Un area de trabajo de una empresa puede tener departamentos de trabajo mas especializados en tareas concretas muchas veces mejorando el nivel organizativo de la empresa.

* **Atributos**

| Nombre          | Tipo                   | Descripcion                                                                         |
| --------------- | ---------------------- | ----------------------------------------------------------------------------------- |
| ID_Departamento | ***PK, NN*** - SERIAL  | Identificador Unico del departamento usado por el DBMS para indexacion              |
| Nombre          | ***NN*** - VARCHAR(25) | Especifica el nombre del departamento de trabajo del area, ej "Departamento de Red" |
| Descripcion     | VARCHAR(255)           | Breve descripcion de las principales funciones del departamento de trabajo          |

* **Relaciones**

| Nombre  | Tipo                   | Descripcion                                                                                                                                  |
| ------- | ---------------------- | -------------------------------------------------------------------------------------------------------------------------------------------- |
| ID_AREA | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Departamento** con la entidad padre **Area**, para conocer a que area pertenece cada departamento |

---

## **Funcion** 
Cada departamento tiene muchas funciones y tareas que cumplir, naturalmente es de interes administrativo llevar el control de esas funciones considerando que estas funciones solo pueden formar parte de un departamento.

* **Atributos**

| Nombre      | Tipo                   | Descripcion                                                                |
| ----------- | ---------------------- | -------------------------------------------------------------------------- |
| ID_Funcion  | ***PK, NN*** - SERIAL  | Identificador unico de entidad usado por el DBMS para control e indexacion |
| Nombre      | ***NN*** - VARCHAR(25) | Especifica el nombre de la funcion que se debe realiza                     |
| Descripcion | VARCHAR(255)           | Breve descripcion de que hace la funcion, sus objetivos, etc.              |

* **Relaciones**

| Nombre          | Tipo                   | Descripcion                                                                                                                                   |
| --------------- | ---------------------- | --------------------------------------------------------------------------------------------------------------------------------------------- |
| ID_Departamento | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Funcion** con la entidad **Departamento**, y especifica a que departamento pertenece cada funcion. |

---

## **Empleado**
Dada la maxima importancia que supone a una empresa tener el control administrativo de sus empleados, se lleva registro de todos los empleados que actualmente estan trabajando en la empresa con la informacion necesaria para contactarlos en caso de emergencia.

* **Atributos**

| Nombre           | Tipo                            | Descripcion                                                                                                       |
| ---------------- | ------------------------------- | ----------------------------------------------------------------------------------------------------------------- |
| ID_Empleado      | ***PK, NN*** - SERIAL           | Identificador unico usado por el DBMS para control e indexacion                                                   |
| Nombre           | ***NN*** - VARCHAR(25)          | Nombre del empleado                                                                                               |
| Apellido         | ***NN*** - VARCHAR(35)          | Apellido del empleado                                                                                             |
| DPI              | ***NN*** - BIGINT               | Numero de DPI del empleado                                                                                        |
| Fecha_Nacimiento | ***NN*** - DATE                 | Fecha de nacimiento del empleado                                                                                  |
| Fecha_Inicio     | ***NN*** - DATE                 | Fecha en que el empleado fue contratado a labores de la empresa                                                   |
| Edad             | ***NN*** - SMALLINT             | Edad actual del empleado, este atributo debe actualizarse cada cumpleaños del empleado con el uso de programacion |
| Telefono         | ***NN*** - INTEGER              | Numero telefonico del empleado para contacto                                                                      |
| Direccion        | ***NN*** - VARCHAR(75)          | Direccion de residencia del empleado.                                                                             |
| Salario          | ***NN, (Salario > 0)*** - MONEY | Salario final que el empleado cobra por su mano de obra                                                           |

* **Relaciones**

| Nombre          | Tipo                   | Descripcion                                                                                                                                                  |
| --------------- | ---------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| ID_Funcion      | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Empleado** con la entidad **Funcion**, con el fin de especificar a que funcion fue asignado el empleado           |
| ID_Departamento | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Empleado** con la entidad **Departamento**, con el fin de especificar a que departamento fue asignado el empleado |

---

## **Seguro**
Como empresa de ssguros que es la Aseguradora Vida Segura, es de maxima importancia tener bien registrado y controlado los seguros que se ofrecen al publico.

* **Atributos**

| Nombre      | Tipo                   | Descripcion                                                        |
| ----------- | ---------------------- | ------------------------------------------------------------------ |
| ID_Seguro   | ***PK, NN*** - SERIAL  | Identificador Unico usado por el DBMS para el control e indexacion |
| Nombre      | ***NN*** - VARCHAR(25) | Especifica el nombre del producto                                  |
| Descripcion | VARCHAR(255)           | Resumen o descripcion breve de lo que ofrece el seguro             |

---

## **Requisitos Seguro**
Cada seguro tiene sus limitaciones sociales, economicos, etc. Asi que la aseguradora debe tener un registro de todos los requerimientos necesarios para cada seguro que ofrece.

* **Atributos**

| Nombre       | Tipo                    | Descripcion                                                        |
| ------------ | ----------------------- | ------------------------------------------------------------------ |
| ID_Requisito | ***PK, NN*** - SERIAL   | Identificador Unico usado por el DBMS para el control e indexacion |
| Descripcion  | ***NN*** - VARCHAR(255) | Se especifica exactamente el requerimiento del seguro.             |

* **Relaciones**
  
| Nombre    | Tipo                   | Descripcion                                                                                                                                  |
| --------- | ---------------------- | -------------------------------------------------------------------------------------------------------------------------------------------- |
| ID_Seguro | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Requisito Seguro** con la entidad **Seguro**, que especifica para que seguro es cada restriccion. |

---

## **Llamada**
Para llevar control estadistico es necesario que la administracion de la empresa registre los datos necesarios de las llamas que hacen los clientes hacia la empresa de seguros.

* **Atributos**

| Nombre     | Tipo                   | Descripcion                                                     |
| ---------- | ---------------------- | --------------------------------------------------------------- |
| ID_Llamada | ***PK, NN*** - SERIAL  | Identificador Unico usado por el DBMS para control e indexacion |
| Cliente    | ***NN*** - VARCHAR(50) | Nombre del cliente que esta interesado en adquirir un seguro    |
| Telefono   | ***NN*** - INTEGER     | Numero telefonico del cliente interesado en adquirir un seguro  |
| Fecha      | DATE                   | Fecha que se realizo la llamada                                 |
| Hora       | TIME                   | Hora en la que empezo la llamada                                |
| Duracion   | INTEGER                | La duracion que tuvo la llamada entre el cliente y el operador  |

* **Relaciones**

| Nombre      | Tipo                   | Descripcion                                                                                                                                      |
| ----------- | ---------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------ |
| ID_Empleado | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Llamada** con la entidad **Empleado**, y que especifica que empleado atendio al cliente en la llamada |

---

## **Detalle Llamada**
Entidad auxiliar utilizada para detallar los seguros que el cliente se muestra interesado en adquirir

* **Relaciones**

| Nombre     | Tipo                       | Descripcion                                                                                                                                                |
| ---------- | -------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ID_Llamada | ***FK, PK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Detalle Llamada** con la entidad **Llamada**, y que especifica que llamada se va a detallar.                    |
| ID_Seguro  | ***FK, PK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Detalle Llamada** con la entidad **Seguro**, y que especifica que seguro es el que el cliente quiere contratar. |

---


## **Tipo de Pago**
Dado que en el paso de los tiempos surgen nuevas forma de pagos, es necesario llevar registro de ello, por ejemplo: "Tarjeta de Credito", "Efectivo", "Pago contra entrega", "bitcoin", etc.

* **Atributos**

| Nombre       | Tipo                   | Descripcion                                                      |
| ------------ | ---------------------- | ---------------------------------------------------------------- |
| ID_Tipo_Pago | ***PK, NN*** - SERIAL  | Identificador Unico que usa el DBMS para el control e indexacion |
| Nombre       | ***NN*** - VARCHAR(25) | Nombre del tipo de pago, ej. "Tarjeta de Credito"                |

---

## **Cliente**
Como empresa siempre se esta interesado en tener clientes habituales y dar incetivos de compra, para ello es necesario tener un registro de todos los clientes que han adquirido algun tipo de servicio.

* **Atributos**

| Nombre           | Tipo                            | Descripcion                                                        |
| ---------------- | ------------------------------- | ------------------------------------------------------------------ |
| ID_Cliente       | ***PK, NN*** - SERIAL           | Identificador Unico usado por el DBMS para el control e indexacion |
| CUI              | ***NN*** - BIGINT               | Numero de CUI o DPI del cliente que ha adquirido un servicio       |
| Nombre           | ***NN*** - VARCHAR(25)          | Nombre del cliente                                                 |
| Apellido         | ***NN*** - VARCHAR(35)          | Apellido del cliente                                               |
| Fecha_Nacimiento | ***NN*** - DATE                 | Fecha de nacimiento del cliente                                    |
| Telefono         | ***NN*** - INTEGER              | Numero telefonico del cliente                                      |
| Direccion        | ***NN*** - VARCHAR(75)          | Direccion de residencia del cliente                                |
| Edad             | ***NN, (Edad > 0)*** - SMALLINT | Edad actual de cliente                                             |
| Correo           | ***NN*** - VARCHAR(25)          | Correo de contacto del cliente                                     |

---

## **Poliza**
Es altamente necesario llevar control extremo de las polizas vigentes, hay que considerar que un cliente solo puede tener 5 polizas y en el caso de que el cliente quiera adquirir otra se debe validar que no se pueda registrar otra a su nombre, esta validacion debe implementarse con programacion de parte del frontend o bien usando trigger o procedimientos almacenados.

* **Atributos**

| Nombre        | Tipo                          | Descripcion                                                        |
| ------------- | ----------------------------- | ------------------------------------------------------------------ |
| ID_Poliza     | ***PK, NN*** - SERIAL         | Identificador Unico usado por el DBMS para el control e indexacion |
| Codigo        | ***NN*** - INTEGER            | Codigo unico que identifica una poliza, es distinta al ID_Poliza   |
| Fecha_Inicial | ***NN*** - DATE               | Fecha en que entra en vigencia la poliza                           |
| Fecha_Final   | ***NN*** - DATE               | Fecha en que finaliza la vigencia de la poliza                     |
| Monto         | ***NN, (Monto > 0)*** - MONEY | El costo mensual de la poliza, que el cliente deber cancelar       |

* **Relaciones**

| Nombre      | Tipo                   | Descripcion                                                                                                                               |
| ----------- | ---------------------- | ----------------------------------------------------------------------------------------------------------------------------------------- |
| ID_Cliente  | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Relaciones** con la entidad **Cliente**, y que especifica a que cliente pertenece una poliza   |
| ID_Empleado | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Relaciones** con la entidad **Empleado**, y que especifica que empleado logro vender la poliza |

---

## **Pago**
La aseguradora debe asegurarse de que los clientes vayan cancelando la poliza para poder ofrecer el servicio, para ello es necesario ir registrando el historial de pagos.

* **Atributos**

| Nombre     | Tipo                           | Descripcion                                                                                                                                                                     |
| ---------- | ------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ID_Pago    | ***PK, NN*** - SERIAL          | Identificador Unico usado por el DBMS para el control e indexacion                                                                                                              |
| Tarifa     | ***NN, (Tarifa > 0)*** - MONEY | Monto de tarifa mensual que el cliente debe pagar por el seguro adquirido                                                                                                       |
| Mora       | ***NN, (Mora > 0)*** - MONEY   | Monto de la cantidad de mora que el cliente debe pagar por pasarse de la fecha limite de pago, este cantidad debe calcularse usando programacion y con el ultimo pago historico |
| Monto      | ***NN, (Monto > 0)*** - MONEY  | Monto total que el cliente debe pagar                                                                                                                                           |
| Fecha_Pago | ***NN*** - DATE                | Fecha que se realizo el pago                                                                                                                                                    |

* **Relaciones**

| Nombre    | Tipo                   | Descripcion                                                                                                         |
| --------- | ---------------------- | ------------------------------------------------------------------------------------------------------------------- |
| ID_Poliza | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Pago** con la entidad **Poliza**, y que especifica que poliza fue pagada |
| ID_Tipo_Pago | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Pago** con la entidad **Tipo Pago**, y que especifica como fue el tipo de pago empleado |
