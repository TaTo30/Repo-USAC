CREATE TABLE vehiculo(
    ID_Vehiculo SERIAL PRIMARY KEY,
    Placa VARCHAR(25) NOT NULL,
    Color VARCHAR(25),
    Marca VARCHAR(25),
    Modelo VARCHAR(25),
    Kilometraje INTEGER NOT NULL,
    Anio INTEGER,
    Transmision VARCHAR(15) NOT NULL,
    No_Puertas SMALLINT NOT NULL,
    Condicion VARCHAR(255),
    ID_Proveedor INTEGER REFERENCES proveedor(ID_Proveedor)
);


CREATE TABLE inventario(
    ID_Inventario SERIAL PRIMARY KEY,
    ID_Vehiculo INTEGER REFERENCES vehiculo(ID_Vehiculo),
    ID_Sucursal INTEGER REFERENCES sucursal(ID_Sucursal),
    Precio MONEY NOT NULL,
    Vendido BOOLEAN NOT NULL
);


CREATE TABLE compra(
    ID_Compra SERIAL PRIMARY KEY,
    ID_Vehiculo INTEGER REFERENCES vehiculo(ID_Vehiculo),
    ID_Sucursal INTEGER REFERENCES sucursal(ID_Sucursal),
    Monto MONEY NOT NULL,
    Fecha DATE,
    Observaciones VARCHAR(255)
);


CREATE TABLE venta(
    ID_Venta SERIAL PRIMARY KEY,
    ID_Cliente INTEGER REFERENCES cliente(ID_Cliente),
    ID_Sucursal INTEGER REFERENCES sucursal(ID_Sucursal),
    ID_Empleado INTEGER REFERENCES empleado(ID_Empleado),
    Monto MONEY NOT NULL,
    Fecha DATE NOT NULL,
    Hora TIME NOT NULL,
    Banco VARCHAR(25),
    Tarjeta BIGINT, 
    Observaciones VARCHAR(255)
);


CREATE TABLE cliente(
    ID_Cliente SERIAL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(75) NOT NULL,
    Direccion VARCHAR(125) NOT NULL,
    Telefono_Celular INTEGER NOT NULL,
    Telefono_Casa INTEGER NOT NULL,
    DPI BIGINT NOT NULL,
    NIT BIGINT NOT NULL
);


CREATE TABLE proveedor(
    ID_Proveedor SERIAL PRIMARY KEY,
    Telefono INTEGER NOT NULL,
    Nombre VARCHAR(25) NOT NULL,
    Direccion VARCHAR(125) NOT NULL,
    Correo VARCHAR(125) NOT NULL,
    Empresa VARCHAR(25) NOT NULL
);


CREATE TABLE empleado(
    ID_Empleado SERIAL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(75) NOT NULL,
    Telefono INTEGER NOT NULL,
    DPI BIGINT NOT NULL,
    NIT BIGINT NOT NULL,
    Sueldo MONEY NOT NULL
);


CREATE TABLE turnos(
    ID_Turno SERIAL PRIMARY KEY,
    ID_Empleado INTEGER REFERENCES empleado(ID_Empleado),
    ID_Sucursal INTEGER REFERENCES sucursal(ID_Sucursal),
    FECHA DATE NOT NULL
);


CREATE TABLE sucursal(
    ID_Sucursal SERIAL PRIMARY KEY,
    ID_Municipio INTEGER REFERENCES municipio(ID_Municipio),
    Nombre VARCHAR(75) NOT NULL,
    Web VARCHAR(75) NOT NULL,
    Direccion VARCHAR(125) NOT NULL,
    Telefono INTEGER NOT NULL
);


CREATE TABLE municipio(
    ID_Municipio SERIAL PRIMARY KEY,
    ID_Departamento INTEGER REFERENCES departamento(ID_Departamento),
    Nombre VARCHAR(75) NOT NULL
);


CREATE TABLE departamento(
    ID_Departamento SERIAL PRIMARY KEY,
    Nombre VARCHAR(75) NOT NULL
);


