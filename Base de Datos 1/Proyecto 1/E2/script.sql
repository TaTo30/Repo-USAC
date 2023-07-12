CREATE TABLE avion(
    NO_Vuelo SERIAL PRIMARY KEY,
    Modelo VARCHAR(10),
    Matricula VARCHAR(15),
    No_Primer_Clase SMALLINT,
    No_Clase_Economica SMALLINT,
    No_Clase_Ejecutiva SMALLINT,
    Ultimo_Mantenimiento DATE,
    Proximo_Mantenimiento DATE,
    Ultimo_Vuelo INTEGER REFERENCES vuelo(ID_Vuelo),
    MAX_Combustible INTEGER NOT NULL,
    MAX_Distancia INTEGER NOT NULL,
    MAX_Altura INTEGER NOT NULL
);

CREATE TABLE mantenimiento(
    ID_Mantenimiento SERIAL PRIMARY KEY,
    Fecha_Finalizada DATE NOT NULL,
    Tipo BOOLEAN NOT NULL,
    Falla BOOLEAN NOT NULL,
    Observaciones VARCHAR(255)
    NO_Vuelo INTEGER REFERENCES avion(NO_Vuelo)
);


CREATE TABLE empleado(
    ID_Empleado SERIAL PRIMARY KEY,
    Nombre VARCHAR(25),
    Apellido VARCHAR(35),
    Edad SMALLINT,
    CUI BIGINT,
    Direccion VARCHAR(125),
    Fecha_Nacimiento DATE,
    Correo VARCHAR(125),
    Telefono INTEGER,
    Idiomas VARCHAR(255),
    Fecha_Contratacion DATE,
    ID_Puesto INTEGER REFERENCES puesto(ID_Puesto),
    ID_Jornada INTEGER REFERENCES jornada(ID_Jornada),
    Hora_Inicio TIME,
    Hora_Final TIME
);

CREATE TABLE puesto(
    ID_Puesto SERIAL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(255)        
);

CREATE TABLE jornada(
    ID_Jornada SERIAL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL  
);

CREATE TABLE retiro(
    ID_Retiro SERIAL PRIMARY KEY,
    ID_Empleado INTEGER REFERENCES empleado(ID_Empleado),
    Fecha DATE NOT NULL,
    Motivo VARCHAR(255)
);


CREATE TABLE asistencia(
    ID_Asistencia SERIAL PRIMARY KEY,
    ID_Empleado INTEGER REFERENCES empleado(ID_Empleado),
    Fecha DATE NOT NULL,
    Entrada TIME NOT NULL
);


CREATE TABLE inasistencia(
    ID_Inasistencia SERIAL PRIMARY KEY,
    ID_Empleado INTEGER REFERENCES empleado(ID_Empleado),
    Fecha DATE NOT NULL,
    Motivo VARCHAR(255)
);


CREATE TABLE cliente( 
    ID_Cliente SERIAL PRIMARY KEY,
    Nombre VARCHAR(25) NOT NULL,
    Apellido VARCHAR(35) NOT NULL,
    Edad SMALLINT NOT NULL,
    CUI BIGINT NOT NULL,
    Fecha_Nacimiento DATE,
    Correo VARCHAR(25),
    Telefono INTEGER,
    Direccion VARCHAR(125),
    Codigo_Postal INTEGER,
    Pasaporte BIGINT NOT NULL
);


CREATE TABLE lugar(
    ID_Lugar SERIAL PRIMARY KEY,
    Pais VARCHAR(50) NOT NULL,
    Ciudad VARCHAR(50) NOT NULL
);


CREATE TABLE formulario(
    ID_Formulario SERIAL PRIMARY KEY,
    ID_Cliente INTEGER REFERENCES cliente(ID_Cliente),
    ID_Origen INTEGER REFERENCES lugar(ID_Lugar),
    ID_Destino INTEGER REFERENCES lugar(ID_Lugar),
    ID_Clase INTEGER REFERENCES clase(ID_Clase),
    Monto MONEY NOT NULL,
    Fecha_Salida DATE,
    Fecha_Retorno DATE
);


CREATE TABLE clase(
    ID_Clase SERIAL PRIMARY KEY,
    Nombre VARCHAR(25) NOT NULL
);


CREATE TABLE boleto(
    NO_Boleto SERIAL PRIMARY KEY,
    ID_Cliente INTEGER REFERENCES cliente(ID_Cliente),
    Peso_Equipaje INTEGER,
    Restricciones VARCHAR(255),
    Fecha_Restricciones DATE,
    NO_Vuelo INTEGER REFERENCES avion(NO_Vuelo),
    Directo BOOLEAN NOT NULL
);


CREATE TABLE detalle_boleto_ciudades(
    NO_Boleto INTEGER REFERENCES boleto(NO_Boleto),
    ID_Lugar INTEGER REFERENCES lugar(ID_Lugar)
);


CREATE TABLE pago(
    NO_Pago SERIAL PRIMARY KEY,
    NO_Boleto INTEGER REFERENCES boleta(NO_Boleto),
    Tarjeta INTEGER NOT NULL,
    Credito BOOLEAN NOT NULL,
    Monto MONEY NOT NULL
);


CREATE TABLE vuelo(
    ID_Vuelo SERIAL PRIMARY KEY,
    NO_Vuelo INTEGER REFERENCES avion(NO_Vuelo),
    Cantidad_Pasajeros SMALLINT NOT NULL,
    Destino INTEGER REFERENCES lugar(ID_Lugar),
    Despegue TIMESTAMP NOT NULL,
    Aterrizaje TIMESTAMP NOT NULL
);


CREATE TABLE tripulacion(
    ID_Vuelo INTEGER REFERENCES vuelo(ID_Vuelo),
    ID_Empleado INTEGER REFERENCES empleado(ID_Empleado)
);


