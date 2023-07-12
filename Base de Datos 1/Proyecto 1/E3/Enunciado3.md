# Programa Nacional de Resarcimiento (PNR)
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

## **Victimas**
Para llevar el correcto control de las victimas supone una mejor organizacion para ello se describe la siguiente entidad que considera datos personales de la victima como nombre y apellidos, edad, residencia, telefono entre otros.

* **Atributos**

| Nombre           | Tipo                   | Descripcion                                                     |
| ---------------- | ---------------------- | --------------------------------------------------------------- |
| ID_Victima       | ***PK, NN*** - SERIAL  | Identificador Unico usado por el DBMS para control e indexacion |
| Nombre           | ***NN*** - VARCHAR(50) | Nombre de la victima                                            |
| Apellidos        | ***NN*** - VARCHAR(70) | Apellidos de la victima                                         |
| Edad             | SMALLINT               | Supuesta edad de la victima                                     |
| Fecha_Nacimiento | DATE                   | Supuesta fecha de nacimiento de la victima                      |
| Residencia       | VARCHAR(125)           | Residencia actual de la victima                                 |
| Telefono         | INTEGER                | Numero de telefono de la victima                                |

---

## **Denuncia**
Estructura de datos correspondiente a una carta de denucia que alguna victima del conflicto armado interno realice en animo de encontrar algun familiar o amigo desaparecido durante dicho conflicto.

* **Atributos**

| Nombre             | Tipo                  | Descripcion                                                         |
| ------------------ | --------------------- | ------------------------------------------------------------------- |
| ID_Denuncia        | ***PK, NN*** - SERIAL | Identificador Unico usado por el DBMS para control e indexacion     |
| Fecha_Denuncia     | ***NN*** - DATE       | Fecha en la que la denuncia fue impuesta                            |
| Fecha_Desaparicion | ***NN*** - DATE       | Fecha que se especula que la persona fue secuestrada o desaparecida |

* **Relaciones**

| Nombre          | Tipo                   | Descripcion                                                                                                                                                        |
| --------------- | ---------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| ID_Denunciante  | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Denuncia** con la entidad **Victima**, y que indica que victima de la guerra realizo la denuncia                        |
| ID_Desaparecido | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Denuncia** con la entidad **Desaparecido**, y que indica la informacion del desaparecido que se describe en la denuncia |

---

## **Desaparecido**
Definicion de datos para el registro de personas actualmente desaparecidas con datos otorgados a traves de las denuncias de algunas victimas.

* **Atributos**

| Nombre          | Tipo                   | Descripcion                                                     |
| --------------- | ---------------------- | --------------------------------------------------------------- |
| ID_Desaparecido | ***PK, NN*** - SERIAL  | Identificador Unico usado por el DBMS para control e indexacion |
| Nombres         | ***NN*** - VARCHAR(50) | Supuesto nombre del desaparecido                                |
| Apellidos       | ***NN*** - VARCHAR(70) | Supuestos apellidos del desaparecido                            |
| Profesion       | VARCHAR(75)            | Supuesta profesion del desaparecido                             |
| Edad            | SMALLINT               | Presunta edad del desaparecido                                  |
| Altura          | SMALLINT               | Presunta altura del desaparecido                                |
| Color_Piel      | VARCHAR(25)            | Presunto color de piel del desaparecido                         |
| Color_Cabello   | VARCHAR(25)            | Presunto color de cabello del desaparecido                      |
| Etnia           | VARCHAR(25)            | Presunta etnia del desaparecido                                 |

---

## **Hallazgo**
Definicion de datos para el registro de cuerpos hallados sin vida, con datos segun datos forenses.

* **Atributos**

| Nombre      | Tipo                    | Descripcion                                                                                      |
| ----------- | ----------------------- | ------------------------------------------------------------------------------------------------ |
| ID_Hallazgo | ***PK, NN*** - SERIAL   | Identificador Unico usado por el DBMS para control e indexacion                                  |
| Ubicacion   | ***NN*** - VARCHAR(125) | Ubicacion donde fue encontrado el cuerpo                                                         |
| Edad        | SMALLINT                | Edad calculada del cuerpo hallado                                                                |
| Altura      | SMALLINT                | Altura estimada del cuerpo hallado                                                               |
| Descripcion | VARCHAR(255)            | Descripcion del estado del cuerpo cuando este fue encontrado, como vestimentas, accesorios, etc. |

---

## **ADN**
Estructura de datos utilizada para almacenar informacion de ADN de cualquier persona.

* **Atributos**

| Nombre  | Tipo                       | Descripcion                                                              |
| ------- | -------------------------- | ------------------------------------------------------------------------ |
| ID_ADN  | ***PK, NN*** - SERIAL      | Identificador Unico usado por el DBMS para control e indexacion          |
| Cadena  | ***NN*** - VARCHAR(255)      | Informacion del ADN del cuerpo en cuestion                               |
| Persona | ***PK, FK, NN*** - INTEGER | Id, de quien pertenece el ADN, ya sea de una victima o un cuerpo hallado |

