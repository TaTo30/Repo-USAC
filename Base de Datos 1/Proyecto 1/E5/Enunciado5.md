# Computadoras INC.
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

## **Bodega**
Dado lo importante que es mantener cuatificaca los productos que se mueven, se tiene una entidad que registra y controla todos estos productos.

* **Atributos**

| Nombre    | Tipo                    | Descripcion                                                     |
| --------- | ----------------------- | --------------------------------------------------------------- |
| No_Bodega | ***PK, NN*** - SERIAL   | Identificador Unico usado por el DBMS para control e indexacion |
| Direccion | ***NN*** - VARCHAR(125) | Direccion donde se ubica la bodega                              |

* **Relaciones**

| Nombre         | Tipo                   | Descripcion                                                                                                                          |
| -------------- | ---------------------- | ------------------------------------------------------------------------------------------------------------------------------------ |
| ID_Encargado   | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Bodega** con la entidad **Empleado**, que indica que empleado esta encargado de la bodega |
| ID_Tipo_Bodega | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Bodega** con la entidad **Tipo Bodega**, que indica que tipo de bodega se esta definiendo |


---

## **Tipo Bodega**
Entidad auxiliar con la principial funcion de definir los tipos de bodegas de las que se puede tener disposicion.

* **Atributos**

| Nombre         | Tipo                   | Descripcion                                                     |
| -------------- | ---------------------- | --------------------------------------------------------------- |
| ID_Tipo_Bodega | ***PK, NN*** - SERIAL  | Identificador Unico usado por el DBMS para control e indexacion |
| Nombre         | ***NN*** - VARCHAR(25) | Nombre del tipo de bodga                                        |


---

## **Detalle Bodega**
Entidad auxiliar con la principal funcion de definir el contenido de cada una de las bodegas.

* **Atributos**

| Nombre    | Tipo                              | Descripcion                                                                                                                                                 |
| --------- | --------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------- |
| No_Bodega | ***PK, FK, NN*** - INTEGER        | **Llave Foranea**, que indexa la entidad **Detalle Bodega** con la entidad **Bodega**, que indica que bodga es la que se esta detallando                    |
| No_Unidad | ***PK, FK, NN*** - INTEGER        | **Llave Foranea**, que indexa la entidad **Detalle Bodga** con la entidad **Producto, Parte, Soporte**, que indica que cosa se esta almacenando en la bodga |
| Cantidad  | ***NN, (Cantidad > 0)*** SMALLINT | Cantidad de unidades disponibles en la bodega                                                                                                               |


---

## **Parte**
Muchos de los productos se constituyen de partes para su funcionamiento, esta entidad es para el registro de partes para la fabricacion de esos dispositivos electronicos.

* **Atributos**

| Nombre   | Tipo                   | Descripcion                                                     |
| -------- | ---------------------- | --------------------------------------------------------------- |
| ID_Parte | ***PK, NN*** - SERIAL  | Identificador Unico usado por el DBMS para control e indexacion |
| Codigo   | ***UQ, NN*** - INTEGER | Codigo unico para identifica una parte o un grupo de partes     |
| Nombre   | ***NN*** - VARCHAR(35) | Nombre nominal del componente                                   |
| Color    | VARCHAR(15)            | Color predominante del componente                               |
| Precio   | ***NN*** - MONEY       | Precio de venta                                                 |

* **Relaciones**

| Nombre        | Tipo                   | Descripcion                                                                                                                       |
| ------------- | ---------------------- | --------------------------------------------------------------------------------------------------------------------------------- |
| ID_Proveedor  | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Parte** con la entidad **Proveedor**, que indica que proveedor destribuye la parte     |
| ID_Tipo_Parte | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Parte** con la entidad **Tipo Parte**, que indica que tipo de parte se esta definiendo |


---

## **Tipo Parte**
Entidad auxiliar con la principial funcion de definir los tipos de partes de las que se puede tener disposicion.

* **Atributos**

| Nombre        | Tipo                   | Descripcion                                                     |
| ------------- | ---------------------- | --------------------------------------------------------------- |
| ID_Tipo_Parte | ***PK, NN*** - SERIAL  | Identificador Unico usado por el DBMS para control e indexacion |
| Nombre        | ***NN*** - VARCHAR(25) | Nombre del tipo de parte que se consideran en el ensamblaje     |


---

## **Parte Parte**
Entidad auxiliar que define la composicion de cada una de las partes.

* **Relaciones**

| Nombre               | Tipo                   | Descripcion                                                                                                                         |
| -------------------- | ---------------------- | ----------------------------------------------------------------------------------------------------------------------------------- |
| ID_Parte (Compuesto) | ***PK, FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Parte** con la entidad **Parte**, que indica que parte sera detallada su composicion     |
| ID_Parte (Unidad)    | ***PK, FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Parte** con la entidad **Parte**, que indica la parte que forma parte de una parte padre |


---

## **Etapa**
Definicion de datos para el registro de etapas de fabricacion para los productos que se producen en la empresa.

* **Atributos**

| Nombre   | Tipo                   | Descripcion                                                     |
| -------- | ---------------------- | --------------------------------------------------------------- |
| ID_Etapa | ***PK, NN*** - SERIAL  | Identificador Unico usado por el DBMS para control e indexacion |
| Nombre   | ***NN*** - VARCHAR(25) | Nombre de la etapa de desarrollo                                |
|Costo|***NN*** - MONEY |Establece el coste que supone producir una unidad en la etapa

---

## **Producto**
Definicion de datos para el registro de productos que han terminado por todas las etapas de produccion.

* **Atributos**

| Nombre      | Tipo                   | Descripcion                                                     |
| ----------- | ---------------------- | --------------------------------------------------------------- |
| ID_Producto | ***PK, NN*** - SERIAL  | Identificador Unico usado por el DBMS para control e indexacion |
| Codigo      | ***UQ, NN*** - INTEGER | Codigo unico que identifica el producto, publicamente           |
| Nombre      | ***NN*** - VARCHAR(25) | Nombre que se le dara al producto finalizado                    |
| Marca       | ***NN*** - VARCHAR(15) | Marca a la que pertenece el producto finalizado                 |
| Fecha       | ***NN*** - DATE        | Fecha que termino de fabricar el producto                       |
| Hora        | ***NN*** - TIME        | Hora que se termino de fabricar el producto                     |


* **Relaciones**

| Nombre      | Tipo                   | Descripcion                                                                                                                                    |
| ----------- | ---------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------- |
| ID_Puesto   | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Producto** con la entidad **Puesto**, que indica en que puesto de trabajo fue terminado el producto |
| ID_Empleado | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Producto** con la entidad **Empleado**, que indica que empleado termino el producto                 |
| ID_Diseño | ***FK, NN*** - INTEGER |**Llave Foranea**, que indexa la entidad **Producto** con la entidad **Producto Plano**, que indica el plano de produccion del producto  

---

## **Producto Plano**
Definicion de datos para el registro de productos previo a ser fabricados y que actua como cabecera para la lista detallada de partes que componen el producto.

* **Atributos**

| Nombre    | Tipo                  | Descripcion                                                     |
| --------- | --------------------- | --------------------------------------------------------------- |
| ID_Diseño | ***PK, NN*** - SERIAL | Identificador Unico usado por el DBMS para control e indexacion |
| Codigo    | ***UQ, NN*** INTEGER  | Codigo unico que identifica al diseño del producto              |


---

## **Detalle Plano Producto**
Entidad auxiliar que define la lista detallada de partes para la fabricacion de un producto antes de ser detallado.

* **Atributos**

| Nombre    | Tipo                   |Descripcion
| --------- | ---------------------- |-
| ID_Diseño | ***FK, NN*** - INTEGER |**Llave Foranea**, que indexa la entidad **Detalle Plano Producto** con la entidad **Plano Producto**, que indica que diseño sera detallado
| ID_Parte  | ***FK, NN*** - INTEGER |**Llave Foranea**, que indexa la entidad **Detalle Plano Producto** con la entidad **Parte**, que indica de que partes se compondra el nuevo producto


---

## **Producto Desarrollo**
Definicion de datos para registrar y controlar los productos que estan siendo actualmente fabricados.

* **Atributos**

| Nombre                | Tipo            | Descripcion                                                                                                  |
| --------------------- | --------------- | ------------------------------------------------------------------------------------------------------------ |
| Fecha_Etapa_Terminada | DATE | Fecha de finalizacion de la etapa que se encontraba desarrollando                                            |
| Hora_Etapa_Terminada  | DATE | Hora de finalizacion de la etapa que se encontraba desarrollando                                             |
| Estado                | **NN** - VARCHAR(25)     | Estado actual de producto en la linea de ensamblaje, que puede ser: 'Iniciando', 'Intermedio', 'Finalizando' |

* **Relaciones**

| Nombre        | Tipo                   | Descripcion                                                                                                                                                              |
| ------------- | ---------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| ID_Diseño     | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Producto Desarrollo** con la entidad **Diseño**, que indica que plano de producto se va fabricar                              |
| ID_Etapa      | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Producto Desarrollo** con la entidad **Etapa**, que indica en que etapa de desarrollo se encuentra el producto                |
| ID_Ensamblaje | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Producto Desarrollo** con la entidad **Linea de ensamblaje**, que indica en que linea de ensamblaje se desarrolla el producto |

---

## **Linea de ensamblaje**
Definicion de datos para registrar y controlar las lineas de ensamblaje disponibles en el centro tecnico.

* **Atributos**

| Nombre        | Tipo                   | Descripcion                                                                                                   |
| ------------- | ---------------------- | ------------------------------------------------------------------------------------------------------------- |
| ID_Ensamblaje | ***PK, NN*** - SERIAL  | Identificador Unico usado por el DBMS para control e indexacion                                               |
| Codigo        | ***UQ, NN*** - INTEGER | Codigo unico relacionado con la linea de ensamblaje                                                           |
| Tipo          | ***NN*** - VARCHAR(25) | Tipo de prodcuto que se esta produciendo en la linea de ensamblaje, esta puede ser 'Telefono' o 'Computadora' |

* **Relaciones**

| Nombre             | Tipo                   | Descripcion                                                                                                                                                |
| ------------------ | ---------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ID_Empleado (Jefe) | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Linea de ensamblaje** con la entidad **Empleado**, que indica que empleado es el jefe de la linea de ensamblaje |


---

## **Puesto de Trabajo**
Definicion de datos para registrar y controlar los puestos de trabajo disponibles para cada linea de ensamblaje.

* **Atributos**

| Nombre    | Tipo                  | Descripcion                                                     |
| --------- | --------------------- | --------------------------------------------------------------- |
| ID_Puesto | ***PK, NN*** - SERIAL | Identificador Unico usado por el DBMS para control e indexacion |
| Codigo    | ***UQ, NN*** INTEGER  | Codigo unico identificador del puesto de trabajo                |
|Salario|***NN*** - MONEY | El salario base que el empleado recibira por emplear en este puesto


* **Relaciones**

| Nombre               | Tipo                   | Descripcion                                                                                                                                          |
| -------------------- | ---------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------- |
| ID_Empleado (Obrero) | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Puesto de Trabajo** con la entidad **Empleado**, que indica que operador esta a cargo del puesto          |
| ID_Ensamblaje        | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Puesto de Trabajo** con la entidad **Empleado**, que indica a que linea de ensamblaje pertenece el puesto |


---

## **Jornada Laboral**
Dado lo flexible que pueden ser los horarios de los trabajadores en la empresa, se debe registrar la fecha y horas pertinentes para calcular el tiempo que trabajo el empleado, para en base a eso calcular su paga.

* **Atributos**

| Nombre     | Tipo                   | Descripcion                                                     |
| ---------- | ---------------------- | --------------------------------------------------------------- |
| Hora_Inicial     | ***NN*** - TIME| Hora en que el empleado registro el inicio de su jornada de trabajo
|Hora_Finalizacion|***NN*** - TIME | Hora en que el empleado registro la finalizacion de su jornada   
|Fecha|***NN*** - DATE | Fecha en que el empleado registro que llego a trabajar


| Nombre     | Tipo                   | Descripcion                                                     |
| ---------- | ---------------------- | --------------------------------------------------------------- |
| ID_Empleado | ***FK, NN*** - INTEGER  | **Llave Foranea**, que indexa la entidad **Jornada Laboral** con la entidad **Empleado**, que indica que empleado registra su jornada |


---

## **Empleado**
Defincion de datos para el registro y control de los empleados contratados que actualmente operan en el centro tecnico.

* **Atributos**

| Nombre      | Tipo                    | Descripcion                                                     |
| ----------- | ----------------------- | --------------------------------------------------------------- |
| ID_Empleado | ***PK, NN*** - SERIAL   | Identificador Unico usado por el DBMS para control e indexacion |
| Codigo      | ***UQ, NN*** - INTEGER  | Codigo unico correlacionado con el empleado                     |
| Nombre      | ***NN*** - VARCHAR(50)  | Nombre del empleado contratado                                  |
| Apellidos   | ***NN*** - VARCHAR(75)  | Apellidos del empleado                                          |
| Direccion   | ***NN*** - VARCHAR(125) | Direccion de residencia del empleado                            |
| Correo      | ***NN*** - VARCHAR(125) | Correo electronico del empleado                                 |
| Telefono    | ***NN*** - INTEGER      | Numero de contacto del empleado                                 |


---

## **Jefe Trabajador**
Entidad auxiliar que funciona para controlar y coordenar todos los jefes y los empleados que tiene a su disposicion.

* **Relaciones**

| Nombre                   | Tipo                   | Descripcion                                                                                                                                     |
| ------------------------ | ---------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------- |
| ID_Empleado (Jefe)       | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Jefe Trabajador** con la entidad **Empleado**, que indica que empleado cumple con el rol de jefe     |
| ID_Empleado (Trabajador) | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Jefe Trabajador** con la entidad **Empleado**, que indica que empleado cumple con el rol de operario |


---

## **Proveedor**
Definicion de datos para el registro y control de proveedores que ofrecen partes al centro tecnico.

* **Atributos**

| Nombre       | Tipo                   | Descripcion                                                     |
| ------------ | ---------------------- | --------------------------------------------------------------- |
| ID_Proveedor | ***PK, NN*** - SERIAL  | Identificador Unico usado por el DBMS para control e indexacion |
| Nombre       | ***NN*** - VARCHAR(75) | Nombre del proveedor                                            |
| Empresa      | ***NN*** - VARCHAR(75) | Empresa a la que pertenece la proveedora                        |
| Pais         | ***NN*** - VARCHAR(25) | Pais donde pertenece la proveedora                              |
| Telefono     | ***NN*** - INTEGER     | Numero de contacto de la proveedora                             |


---

## **Cliente**
Definicion de datos para el registro y control de clientes que han comprado al menos una vez alguno de los productos producidos por el centro de tecnico.

* **Atributos**

| Nombre     | Tipo                    | Descripcion                                                     |
| ---------- | ----------------------- | --------------------------------------------------------------- |
| ID_Cliente | ***PK, NN*** - SERIAL   | Identificador Unico usado por el DBMS para control e indexacion |
| Nombre     | ***NN*** - VARCHAR(50)  | Nombre del cliente                                              |
| Apellidos  | ***NN*** - VARCHAR(75)  | Apellidos del cliente                                           |
| Direccion  | ***NN*** - VARCHAR(125) | Direccion de residencia del cliente                             |
| Pais       | ***NN*** - VARCHAR(25)  | Pais donde reside el cliente                                    |


---

## **Soporte (Reclamos)**
Estructura de datos que controla y registra todos los casos de devoluciones que un cliente haga debido a alguna falla en la operacion del dispositivo. 

* **Atributos**

| Nombre     | Tipo                  | Descripcion                                                     |
| ---------- | --------------------- | --------------------------------------------------------------- |
| ID_Soporte | ***PK, NN*** - SERIAL | Identificador Unico usado por el DBMS para control e indexacion |
| Fecha      | ***NN*** - DATE       | Fecha del dia que el cliente realizo el reclamo                 |
| Hora       | ***NN*** - TIME       | Hora del dia en que el cliente realizo el reclamo               |
| Monto      | ***NN*** - MONEY      | Monto que debe pagar la empresa por la devolucion del producto  |

* **Relaciones**

| Nombre      | Tipo               | Descripcion                                                                                                             |
| ----------- | ------------------ | ----------------------------------------------------------------------------------------------------------------------- |
| ID_Producto | ***FK*** - INTEGER | **Llave Foranea**, que indexa la entidad **Soporte** con la entidad **Producto**, que indica que producto fue reclamado |
| ID_Cliente  | ***FK*** - INTEGER | **Llave Foranea**, que indexa la entidad **Soporte** con la entidad **Cliente**, que indica que cliente hizo el reclamo |





