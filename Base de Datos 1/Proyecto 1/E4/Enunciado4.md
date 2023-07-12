# Compra y Venta de Vehiculos
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
>   ***(\*)***: CHECK OR DEFAULT KEY

---

## **Vehiculo**
Definicion de los datos necesarios para el registro de los vehiculos comprados y el proveedor al cual fue comprado.

* **Atributos**

| Nombre      | Tipo                     | Descripcion                                                                        |
| ----------- | ------------------------ | ---------------------------------------------------------------------------------- |
| ID_Vehiculo | ***PK, NN*** SERIAL      | Identificador Unico usado por el DBMS para control e indexacion                    |
| Placa       | ***UQ, NN*** VARCHAR(25) | Placa del vehiculo                                                                 |
| Color       | VARCHAR(25)              | Color predominante del vehiculo                                                    |
| Marca       | VARCHAR(25)              | Marca del vehiculo                                                                 |
| Modelo      | VARCHAR(25)              | Modelo del vehiculo                                                                |
| Kilometraje | ***NN*** - DECIMAL(2,8)  | Kilometraje reciente del vehiculo                                                  |
| Año         | INTEGER                  | Año de fabricacion del vehiculo                                                    |
| Transmision | ***NN*** VARCHAR(15)     | Tipo de transmision, si es 'Automatico', 'Mecanico', 'Triptronic', etc             |
| No_Puertas  | ***NN*** SMALLINT        | Cantidad de puertas funcionales que posee el vehiculo                              |
| Condicion   | VARCHAR(255)             | Algunas observaciones y consideraciones a tomar en cuenta, del estado del vehiculo |

* **Relaciones**

| Nombre       | Tipo                   | Descripcion                                                                                                                            |
| ------------ | ---------------------- | -------------------------------------------------------------------------------------------------------------------------------------- |
| ID_Proveedor | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Vehiculo** con la entidad **Proveedor**, y que indica que proveedor proporciono el vehiculo |

---

## **Inventario (Vehiculos en Venta y Vendidos)**
Estructura de datos que define los datos de inventario de vehiculos que historicamente han estado en alguna sucursal de la empresa.

* **Atributos**

| Nombre        | Tipo                  | Descripcion                                                     |
| ------------- | --------------------- | --------------------------------------------------------------- |
| ID_Inventario | ***PK, NN*** - SERIAL | Identificador Unico usado por el DBMS para control e indexacion |
| Precio        | ***NN*** MONEY        | Precio inicial que la empresa pretende vender el vehiculo       |
| Vendido?      | ***NN*** - BOOLEAN    | Determina si el vehiculo ya fue vendido o no                    |

* **Relaciones**

| Nombre      | Tipo                   | Descripcion                                                                                                                                         |
| ----------- | ---------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------- |
| ID_Sucursal | ***FK, NN***- INTEGER  | **Llave Foranea**, que indexa la entidad **Inventario** con la entidad **Sucursal**, y que indica de que sucursal pertenece el inventario a evaluar |
| ID_Vehiculo | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Inventario** con la entidad **Vehiculo**, y que indica de que vehiculo se detalla en el inventario       |

---

## **Compra**
Definicion de datos que registra la compra de un nuevo vehiculo por parte de la empresa.

* **Atributos**

| Nombre        | Tipo                        | Descripcion                                                                        |
| ------------- | --------------------------- | ---------------------------------------------------------------------------------- |
| ID_Compra     | ***PK, NN*** - SERIAL       | Identificador Unico usado por el DBMS para control e indexacion                    |
| Monto         | ***NN, (Monto > 0)*** MONEY | Precio total que pago la empresa para la adquisicion del vehiculo                  |
| Fecha         | DATE                        | Fecha de la compra                                                                 |
| Observaciones | VARCHAR(255)                | Algunas observaciones o consideraciones a tomar en cuenta de la compra de vehiculo |

* **Relaciones**

| Nombre      | Tipo                   | Descripcion                                                                                                                        |
| ----------- | ---------------------- | ---------------------------------------------------------------------------------------------------------------------------------- |
| ID_Sucursal | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Compra** con la entidad **Sucursal**, y que indica que sucursal hizo efectiva la compra |
| ID_Vehiculo | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Compra** con la entidad **Vehiculo**, y a detalle el vehiculo que fue comprado          |

---

## **Venta**
Definicion de datos que registra la venta de un vehiculo.

* **Atributos**

| Nombre        | Tipo                        | Descripcion                                                                                 |
| ------------- | --------------------------- | ------------------------------------------------------------------------------------------- |
| ID_Venta      | ***PK, NN*** - SERIAL       | Identificador Unico usado por el DBMS para control e indexacion                             |
| Monto         | ***NN, (Monto > 0)*** MONEY | Precio al que el vehiculo fue vendido                                                       |
| Fecha         | ***NN*** - DATE             | Fecha del dia que el vehiculo se vendio                                                     |
| Hora          | ***NN*** - TIME             | Hora del dia que el vehiculo se vendio                                                      |
| Banco         | VARCHAR(25)                 | En caso de pago con tarjeta, se especifica la agencia bancaria que emitio dicha tarjeta     |
| No_Tarjeta    | BIGINT                      | Numero de la tarjeta                                                                        |
| Observaciones | VARCHAR(255)                | Observaciones o consideraciones que deban ser descritas en el proceso de venta del vehiculo |

* **Relaciones**

| Nombre      | Tipo                   | Descripcion                                                                                                                         |
| ----------- | ---------------------- | ----------------------------------------------------------------------------------------------------------------------------------- |
| ID_Cliente  | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Venta** con la entidad **Cliente**, y que indica a que cliente fue vendido el vehiculo   |
| ID_Sucursal | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Venta** con la entidad **Sucursal**, y que indica en que sucursal fue efectiva la compra |
| ID_Empleado | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Venta** con la entidad **Empleado**, y que indica que empleado hizo efectiva la compra   |

---

## **Cliente**
Definicion de datos para el registro de clientes nuevos.

* **Atributos**

| Nombre           | Tipo                    | Descripcion                                                       |
| ---------------- | ----------------------- | ----------------------------------------------------------------- |
|                  |                         | Identificador Unico usado por el DBMS para control e indexacion   |
| Nombre           | ***NN*** - VARCHAR(50)  | Nombre del cliente                                                |
| Apellidos        | ***NN*** - VARCHAR(75)  | Apellidos del cliente                                             |
| Direccion        | ***NN*** - VARCHAR(125) | Direccion de residencia del cliente                               |
| Telefono_Celular | ***NN*** - INTEGER      | Telefono primario o de celular del cliente                        |
| Telefono_Casa    | ***NN*** - INTEGER      | Telefono secundario o de casa del cliente                         |
| DPI              | ***NN, UQ*** - BIGINT   | Numero del documento personal de identificacion -DPI, del cliente |
| NIT              | ***NN, UQ*** - BIGINT   | Numero de identificacion Tributario -NIT, del cliente             |

---

## **Proveedor**
Definicion de datos para el registro de proveedores nuevos.

* **Atributos**

| Nombre       | Tipo                    | Descripcion                                                     |
| ------------ | ----------------------- | --------------------------------------------------------------- |
| ID_Proveedor | ***PK, NN*** - SERIAL   | Identificador Unico usado por el DBMS para control e indexacion |
| Telefono     | ***NN*** - INTEGER      | Numero telefonico del proveedor                                 |
| Nombre       | ***NN*** - VARCHAR(25)  | Nombre del proveedor                                            |
| Direccion    | ***NN*** - VARCHAR(125) | Ubicacion fisica de la empresa donde opera el proveedor         |
| Correo       | ***NN*** - VARCHAR(125) | Correo electronico del proveedor                                |
| Empresa      | ***NN*** - VARCHAR(25)  | Nombre de la empresa                                            |

---

## **Empleado**
Definicion de datos para el registro de nuevos empleados contratados.

* **Atributos**

| Nombre      | Tipo                               | Descripcion                                                                   |
| ----------- | ---------------------------------- | ----------------------------------------------------------------------------- |
| ID_Empleado | ***PK, NN*** - SERIAL              | Identificador Unico usado por el DBMS para control e indexacion               |
| Nombres     | ***NN*** - VARCHAR(50)             | Nombre del empleado contratado                                                |
| Apellidos   | ***NN*** - VARCHAR(75)             | Apellidos del empleado contratado                                             |
| Telefono    | ***NN*** - INTEGER                 | Numero de telefono del empleado contratado                                    |
| DPI         | ***NN, UQ*** - BIGINT              | Numero del documento personal de identificacion -DPI, del empleado contratado |
| NIT         | ***NN, UQ*** - BITINT              | Numero de identificacion Tributario -NIT, del empleado                        |
| Sueldo      | ***NN, (Default: $2300)*** - MONEY | Sueldo que que el empleado cobrara por prestacion de mano de obra             |

---

## **Turno**
Definicion de datos para el control de empleados y turnos en distintas sucursales de la empresa.

* **Atributos**

| Nombre   | Tipo                  | Descripcion                                                      |
| -------- | --------------------- | ---------------------------------------------------------------- |
| ID_Turno | ***PK, NN*** - SERIAL | Identificador Unico usado por el DBMS para control e indexacion  |
| Fecha    | ***NN*** - DATE       | Fecha de cuando el empleado debe cumplir con su turno de trabajo |

* **Relaciones**

| Nombre      | Tipo                   | Descripcion                                                                                                                                             |
| ----------- | ---------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ID_Sucursal | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Turno** con la entidad **Sucursal**, y que indica en que sucursal el empleado tiene que cumplir con su turno |
| ID_Empleado | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Turno** con la entidad **Empleado**, y que indica que empleado va cumplir con el turno                       |

---

## **Sucursal**
Definicion de datos para el control de sucursales

* **Atributos**

| Nombre      | Tipo                    | Descripcion                                                     |
| ----------- | ----------------------- | --------------------------------------------------------------- |
| ID_Sucursal | ***PK, NN*** - SERIAL   | Identificador Unico usado por el DBMS para control e indexacion |
| Nombre      | ***NN*** - VARCHAR(75)  | Nombre de la sucursal de venta                                  |
| Web         | ***NN*** - VARCHAR(75)  | Direccion de pagina web de la sucursal                          |
| Direccion   | ***NN*** - VARCHAR(125) | Ubicacion fisica de la sucursal                                 |
| Telefono    | ***NN*** - INTEGER      | Numero telefonico de contacto                                   |

* **Relaciones**

| Nombre       | Tipo                   | Descripcion                                                                                                                                                 |
| ------------ | ---------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ID_Municipio | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Sucursal** con la entidad **Municipio**, y que indica en que municipio del pais se encuentra ubicada la sucursal |

---

## **Municipio** 
Definicion de datos para el control y registro de municipios.

* **Atributos**

| Nombre       | Tipo                   | Descripcion                                                     |
| ------------ | ---------------------- | --------------------------------------------------------------- |
| ID_Municipio | ***PK, NN*** - SERIAL  | Identificador Unico usado por el DBMS para control e indexacion |
| Nombre       | ***NN*** - VARCHAR(75) | Nombre de algun municipio de algun departamento de Guatemala    |

* **Relaciones**

| Nombre          | Tipo                   | Descripcion                                                                                                                                             |
| --------------- | ---------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ID_Departamento | ***FK, NN*** - INTEGER | **Llave Foranea**, que indexa la entidad **Municipio** con la entidad **Departamento**, y que indica a que departamento del pais pertenece el municipio |

---

## **Departamento**
Definicion de datos para el control y registro de departamentos de Guatemala.

| Nombre          | Tipo                   | Descripcion                                                     |
| --------------- | ---------------------- | --------------------------------------------------------------- |
| ID_Departamento | ***PK, NN*** - SERIAL  | Identificador Unico usado por el DBMS para control e indexacion |
| Nombre          | ***NN*** - VARCHAR(75) | Nombre de algun departamento de Guatemala                       |