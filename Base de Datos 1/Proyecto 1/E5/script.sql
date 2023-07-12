CREATE TABLE bodega(
    No_Bodega SERIAL PRIMARY KEY,
    ID_Encargado INTEGER REFERENCES empleado(ID_Empleado),
    ID_Tipo_Bodega INTEGER tipo_bodega(ID_Tipo_Bodega),
    Direccion VARCHAR(125) NOT NULL
);


CREATE TABLE tipo_bodega(
    ID_Tipo_Bodega SERIAL PRIMARY KEY,
    Nombre VARCHAR(25) NOT NULL
);


CREATE TABLE tipo_parte(
    ID_Tipo_Parte SERIAL PRIMARY KEY,
    Nombre VARCHAR(25) NOT NULL
);


CREATE TABLE detalle_bodega(
    No_Bodega INTEGER REFERENCES bodega(No_Bodega),
    ID_Parte INTEGER REFERENCES parte(ID_Parte),
    ID_Producto INTEGER REFERENCES producto(ID_Producto),
    Cantidad SMALLINT NOT NULL
);


CREATE TABLE parte(
    ID_Parte SERIAL PRIMARY KEY,
    ID_Proveedor INTEGER REFERENCES proveedor(ID_Proveedor),
    ID_Tipo_Parte INTEGER REFERENCES tipo_parte(ID_Tipo_Parte),
    Codigo INTEGER NOT NULL,
    Nombre VARCHAR(25) NOT NULL,
    Color VARCHAR(15) NOT NULL,
    Precio MONEY NOT NULL
);


CREATE TABLE parte_parte(
    Padre INTEGER REFERENCES parte(ID_Parte),
    Hijo INTEGER REFERENCES parte(ID_Parte)
);


CREATE TABLE etapa(
    ID_etapa SERIAL PRIMARY KEY,
    Nombre VARCHAR(25) NOT NULL
);


CREATE TABLE producto(
    ID_Producto SERIAL PRIMARY KEY,
    ID_Puesto INTEGER REFERENCES puesto(ID_Puesto),
    ID_Empleado INTEGER REFERENCES empleado(ID_Empleado),
    Codigo INTEGER NOT NULL,
    Nombre VARCHAR(25) NOT NULL,
    Marca VARCHAR(15) NOT NULL,
    Fecha DATE NOT NULL,
    Hora TIME NOT NULL
);


CREATE TABLE producto_disenio(
    ID_Disenio SERIAL PRIMARY KEY,
    Nombre VARCHAR(25) NOT NULL
);


CREATE TABLE detalle_producto_disenio(
    ID_Disenio INTEGER REFERENCES producto_disenio(ID_Disenio),
    ID_Parte INTEGER REFERENCES parte(ID_Parte)
);


CREATE TABLE producto_desarrollo(
    ID_Disenio INTEGER REFERENCES producto_disenio(ID_Disenio),
    ID_etapa INTEGER REFERENCES etapa(ID_etapa),
    ID_Ensamblaje INTEGER REFERENCES ensamblaje(ID_Ensamblaje),
    Fecha DATE NOT NULL,
    Hora TIME NOT NULL,
    Estado VARCHAR(25)
);


CREATE TABLE ensamblaje(
    ID_Ensamblaje SERIAL PRIMARY KEY,
    Jefe INTEGER REFERENCES empleado(ID_Empleado),
    Codigo INTEGER NOT NULL,
    Tipo VARCHAR(25) NOT NULL
);


CREATE TABLE puesto(
    ID_Puesto SERIAL PRIMARY KEY,
    Jefe INTEGER REFERENCES empleado(ID_Empleado),
    Operador INTEGER REFERENCES empleado(ID_Empleado),
    Codigo INTEGER NOT NULL
);


CREATE TABLE empleado(
    ID_Empleado SERIAL PRIMARY KEY, 
    Codigo INTEGER NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(75) NOT NULL,
    Direccion VARCHAR(125) NOT NULL,
    Correo VARCHAR(125) NOT NULL,
    Telefono INTEGER NOT NULL
);


CREATE TABLE jefe_trabajador(
    Jefe INTEGER REFERENCES empleado(ID_Empleado),
    Operador INTEGER REFERENCES empleado(ID_Empleado)
);


CREATE TABLE proveedor(
    ID_Proveedor SERIAL PRIMARY KEY, 
    Nombre VARCHAR(75) NOT NULL,
    Empresa VARCHAR(75) NOT NULL,
    Pais VARCHAR(25) NOT NULL,
    Telefono INTEGER NOT NULL
);


CREATE TABLE cliente(
    ID_Cliente SERIAL PRIMARY KEY, 
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(75) NOT NULL,
    Direccion VARCHAR(125) NOT NULL,
    pais VARCHAR(125) NOT NULL,
);


CREATE TABLE Soporte(
    ID_Soporte SERIAL PRIMARY KEY,
    Fecha DATE NOT NULL,
    Hora TIME NOT NULL,
    ID_Cliente INTEGER REFERENCES cliente(ID_Cliente),
    ID_Producto INTEGER REFERENCES producto(ID_Producto),
    Monto MONEY NOT NULL
);