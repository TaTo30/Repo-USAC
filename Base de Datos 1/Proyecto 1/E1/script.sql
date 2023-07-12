

CREATE TABLE area(
    ID_Area SERIAL PRIMARY KEY,
    Nombre VARCHAR(25) NOT NULL,
    Descripcion VARCHAR(255)
);

CREATE TABLE departamento(
    ID_Departamento SERIAL PRIMARY KEY,
    ID_Area INTEGER REFERENCES area(ID_Area) NOT NULL,
    Nombre VARCHAR(25) NOT NULL,
    Descripcion VARCHAR(255)
);


CREATE TABLE funcion(
    ID_Funcion SERIAL PRIMARY KEY, 
    ID_Departamento INTEGER REFERENCES departamento(ID_Departamento) NOT NULL,
    Nombre VARCHAR(25) NOT NULL,
    Descripcion VARCHAR(255)
);


CREATE TABLE empleado(
    ID_Empleado SERIAL PRIMARY KEY,
    Nombre VARCHAR(25) NOT NULL,
    Apellido VARCHAR(35) NOT NULL,
    DPI BIGINT NOT NULL,
    Fecha_Nacimiento DATE,
    Fecha_Inicio DATE,
    Edad SMALLINT NOT NULL,
    Telefono INTEGER NOT NULL,
    Direccion VARCHAR(75) NOT NULL,
    Salario MONEY NOT NULL,
    ID_Funcion INTEGER REFERENCES funcion(ID_Funcion),
    ID_Departamento INTEGER REFERENCES departamento(ID_Departamento)
);


CREATE TABLE seguro(
    ID_Seguro SERIAL PRIMARY KEY,
    Nombre VARCHAR(25) NOT NULL,
    Descripcion VARCHAR(255)
);


CREATE TABLE requisito_Seguro(
    ID_Requisito SERIAL PRIMARY KEY,
    ID_Seguro INTEGER REFERENCES seguro(ID_Seguro),
    Descripcion VARCHAR(255) NOT NULL
);


CREATE TABLE llamada(
    ID_Llamada SERIAL PRIMARY KEY,
    ID_Empleado INTEGER REFERENCES empleado(ID_Empleado),
    Cliente VARCHAR(50) NOT NULL,
    Telefono INTEGER NOT NULL,
    Fecha DATE,
    Hora TIME,
    Duracion INTEGER
);


CREATE TABLE detalle_llamada(
    ID_Llamada INTEGER REFERENCES llamada(ID_Llamada),
    ID_Seguro INTEGER REFERENCES seguro(ID_Seguro)
);


CREATE TABLE tipo_Pago(
    ID_Tipo_Pago SERIAL PRIMARY KEY,
    Nombre VARCHAR(25) NOT NULL
);


CREATE TABLE cliente(
    ID_Cliente SERIAL PRIMARY KEY,
    CUI BIGINT NOT NULL,
    Nombre VARCHAR(25) NOT NULL,
    Apellido VARCHAR(35) NOT NULL,
    Fecha_Nacimiento DATE NOT NULL,
    Telefono INTEGER NOT NULL,
    Direccion VARCHAR(75) NOT NULL,
    Edad SMALLINT NOT NULL CHECK(Edad > 0),
    Correo VARCHAR(25) NOT NULL
);


CREATE TABLE poliza(
    ID_Poliza SERIAL PRIMARY KEY,
    Codigo INTEGER NOT NULL,
    ID_Empleado INTEGER REFERENCES empleado(ID_Empleado),
    ID_Cliente INTEGER REFERENCES cliente(ID_Cliente),
    Fecha_Inicio DATE NOT NULL,
    Fecha_Finalizacion DATE NOT NULL,
    Monto MONEY NOT NULL CHECK(Monto > 0),
    ID_Empleado INTEGER REFERENCES Empleado(ID_Empleado),
    ID_Cliente INTEGER REFERENCES Cliente(ID_Cliente)
);


CREATE TABLE Pago(
    ID_Pago SERIAL PRIMARY KEY,
    Tarifa MONEY NOT NULL CHECK(Tarifa > 0),
    Mora MONEY NOT NULL CHECK(Mora > 0),
    Monto MONEY NOT NULL CHECK(Monto > 0),
    Fecha_Pago DATE NOT NULL,
    ID_Tipo_Pago INTEGER REFERENCES tipo_pago(ID_Tipo_Pago),
    ID_Poliza INTEGER REFERENCES poliza(ID_Poliza)
);

