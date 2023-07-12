# Modelo Entidad-Relacion Propuesto


## **Encuesta**
Entidad creada para la definicion de encuestas.

* Atributos

| Nombre                 | Tipo         | Observaciones        |
| ---------------------- | ------------ | -------------------- |
| ***PK*** - ID_Encuesta | INTEGER      | Llave Primaria       |
| Nombre                 | VARCHAR(255) | Atributo obligatorio |

---

## **Pregunta**
Entidad para la definicion de preguntas que seran asociadas a una encuesta.

* Atributos

| Nombre                 | Tipo         | Observaciones        |
| ---------------------- | ------------ | -------------------- |
| ***PK*** - ID_Pregunta | INTEGER      | Llave Primaria       |
| Pregunta               | VARCHAR(255) | Atributo obligatorio |

* Relaciones

| Nombre                        | Tipo    | Referencia A                 |
| ----------------------------- | ------- | ---------------------------- |
| ***FK*** - Encuesta           | INTEGER | **Encuestas (ID_Encuesta)**  |
| ***FK*** - Respuesta_Correcta | INTEGER | **Respuesta (ID_Respuesta)** |

---

## **Respuesta**
Entidad creada para definicion de respuestas preguntas, estan son independientes de la pregunta, y una respuesta puede estar asociada a muchas preguntas.

* Atributos

| Nombre                  | Tipo         | Observaciones        |
| ----------------------- | ------------ | -------------------- |
| ***PK*** - ID_Respuesta | INTEGER      | Llave Primaria       |
| Respuesta               | VARCHAR(255) | Atributo obligatorio |

---

## **Detalle Pregunta-Respuesta**
Entidad auxiliar que establece una relacion muchos a muchos entre la entidad ***Pregunta*** y la entidad ***Respuesta***. 

* Relaciones

| Nombre                     | Tipo    | Referencia A                 |
| -------------------------- | ------- | ---------------------------- |
| ***FK,PK*** - ID_Pregunta  | INTEGER | **Pregunta (ID_Pregunta)**   |
| ***FK,PK*** - ID_Respuesta | INTEGER | **Respuesta (ID_Respuesta)** |

---

## **Resultados**
Entidad creada para el almacenamiento de respuestas a preguntas que se le realizan a diferentes paises a modo de encuestas, esta entidad actua como detalle de las entidades ***Pregunta, Respuesta, Pais***.

* Relaciones

| Nombre                  | Tipo    | Referencia A                 |
| ----------------------- | ------- | ---------------------------- |
| ***FK,PK*** - ID_Pais   | INTEGER | **Pais (ID_Pais)**           |
| ***FK,PK*** - Pregunta  | INTEGER | **Pregunta (ID_Pregunta)**   |
| ***FK,PK*** - Respuesta | INTEGER | **Respuesta (ID_Respuesta)** |

---

## **Patente**
Entidad creada para la definicion y almacenamiento de patentes de inventos que han sido creados a lo largo del tiempo.

* Atributos

| Nombre                | Tipo         | Observaciones        |
| --------------------- | ------------ | -------------------- |
| ***PK*** - ID_Patente | INTEGER      | Llave Primaria       |
| Nombre                | VARCHAR(100) | Atributo obligatorio |
| Inventor              | VARCHAR(100) | Atributo obligatorio |
| Año                   | INTEGER      | Atributo obligatorio |

* Relaciones

| Nombre          | Tipo    | Referencia A       |
| --------------- | ------- | ------------------ |
| ***FK*** - Pais | INTEGER | **Pais (ID_Pais)** |

---

## **Detalle Patente-Profesional**
Entidad Auxiliar que actua como detalle entre la entidad ***Patente*** y la entidad ***Profesional***.

* Atributos

| Nombre             | Tipo | Observaciones        |
| ------------------ | ---- | -------------------- |
| Fecha_contratacion | DATE | Atributo obligatorio |

* Relaciones

| Nombre                       | Tipo    | Referencia A                     |
| ---------------------------- | ------- | -------------------------------- |
| ***FK,PK*** - ID_Patente     | INTEGER | **Patente (ID_Patente)**         |
| ***FK,PK*** - ID_Profesional | INTEGER | **Profesional (ID_Profesional)** |

---

## **Profesional**
Entidad creada para la definicion y almacenamiento de cientificos profesionales, que son asignados a trabajar en muchas areas de trabajo en distintos paises.

* Atributos

| Nombre                    | Tipo    | Observaciones        |
| ------------------------- | ------- | -------------------- |
| ***PK*** - ID_Profesional | INTEGER | Llave Primaria       |
| Salario                   | INTEGER | Atributo obligatorio |
| Comision                  | INTEGER | Atributo obligatorio |

* Relaciones

| Nombre          | Tipo    | Referencia A       |
| --------------- | ------- | ------------------ |
| ***FK*** - Pais | INTEGER | **Pais (ID_Pais)** |

---

## **Detalle Area-Profesional**
Entidad Auxiliar que actua como detalle entre la entidad ***Area*** y la entidad ***Profesional***.

* Relaciones

| Nombre                       | Tipo    | Referencia A                     |
| ---------------------------- | ------- | -------------------------------- |
| ***FK,PK*** - ID_AREA        | INTEGER | **Area (ID_Area)**               |
| ***FK,PK*** - ID_Profesional | INTEGER | **Profesional (ID_Profesional)** |

---

## **Area**
Entidad creada para la definicion y almacenamiento de las distintas areas de investigacion que pueden estar asociadas en muchos paises.

* Atributos

| Nombre             | Tipo         | Observaciones        |
| ------------------ | ------------ | -------------------- |
| ***PK*** - ID_Area | INTEGER      | Llave Primaria       |
| Nombre             | VARCHAR(100) | Atributo Obligatorio |
| Ranking            | INTEGER      | Atributo Obligatorio |

* Relaciones

| Nombre          | Tipo    | Referencia A                     |
| --------------- | ------- | -------------------------------- |
| ***FK*** - Jefe | INTEGER | **Profesional (ID_Profesional)** |

---

## **Regiones**
Entidad creada para la definicion y almacenamiento de regiones territoriales que pueden abarcar una cantidad inmensa de paises.

* Atributos

| Nombre                 | Tipo         | Observaciones        |
| ---------------------- | ------------ | -------------------- |
| ***PK*** - ID_Region   | INTEGER      | Llave Primaria       |
| Nombre (NOMBRE_REGION) | VARCHAR(100) | Atributo obligatorio |

* Relaciones

| Nombre           | Tipo    | Referencia A             |
| ---------------- | ------- | ------------------------ |
| ***FK*** - Padre | INTEGER | **Regiones (ID_Region)** |

---

## **Pais**
Entidad creada para la definicion y almacenamiento de paises que participan el programa internacional de patentes, tambien estan incluidos los paises de donde provienen muchos profesionales.

* Atributos

| Nombre             | Tipo        | Observaciones        |
| ------------------ | ----------- | -------------------- |
| ***PK*** - ID_Pais | INTEGER     | Llave Primaria       |
| Nombre             | VARCHAR(75) | Atributo obligatorio |
| Capital            | VARCHAR(75) | Atributo obligatorio |
| Poblacion          | INTEGER     | Atributo obligatorio |
| AreaKM2            | INTEGER     | Atributo obligatorio |

* Relaciones

| Nombre               | Tipo    | Referencia A             |
| -------------------- | ------- | ------------------------ |
| ***FK*** - ID_Region | INTEGER | **Regiones (ID_Region)** |

---

## **Detalle Pais-Area**
Entidad Auxiliar que actua como detalle entre la entidad ***Pais*** y la entidad ***Area***.

* Relaciones

| Nombre                | Tipo    | Referencia A       |
| --------------------- | ------- | ------------------ |
| ***FK,PK*** - ID_Pais | INTEGER | **Pais (ID_Pais)** |
| ***FK,PK*** - ID_Area | INTEGER | **Area (ID_Area)** |

---

## **Frontera**
Entidad creada para la definicion de fronteras, actua como detalle entre paises

* Atributos

| Nombre                  | Tipo | Observaciones                                                                             |
| ----------------------- | ---- | ----------------------------------------------------------------------------------------- |
| ***PK*** - Cardinalidad | CHAR | Llave Primaria, este atributo se desglosa en 4 atributos que son: Norte, Sur, Esta, Oeste |

* Relaciones

| Nombre                       | Tipo    | Referencia A       |
| ---------------------------- | ------- | ------------------ |
| ***FK,PK*** - Pais_Central   | INTEGER | **Pais (ID_Pais)** |
| ***FK,PK*** - Pais_Adyacente | INTEGER | **Pais (ID_Pais)** |
