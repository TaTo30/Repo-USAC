CREATE TABLE victima(
    ID_Victima SERIAL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(70) NOT NULL,
    Edad SMALLINT,
    Fecha_Nacimiento DATE,
    Residencia VARCHAR(125),
    Telefono INTEGER
);


CREATE TABLE denuncia(
    ID_Denuncia SERIAL PRIMARY KEY,
    ID_Denunciante INTEGER REFERENCES victima(ID_Victima),
    Fecha_Denuncia DATE NOT NULL,
    Fecha_Desaparacion DATE NOT NULL,
    ID_Desaparecido INTEGER REFERENCES desaparecido(ID_Desaparecido)
);


CREATE TABLE desaparecido(
    ID_Desaparecido SERIAL PRIMARY KEY,
    Nombres VARCHAR(50) NOT NULL,
    Apellidos VARCHAR(70) NOT NULL,
    Profesion VARCHAR(75),
    Edad SMALLINT,
    Altura SMALLINT,
    Color_Piel VARCHAR(25),
    Color_Cabello VARCHAR(25),
    Etnia VARCHAR(25)
);


CREATE TABLE hallazgo(
    ID_Hallazgo SERIAL PRIMARY KEY,
    Ubicacion VARCHAR(125) NOT NULL,
    Edad SMALLINT,
    Altura SMALLINT,
    Descripcion VARCHAR(255)
);


CREATE TABLE ADN(
    ID_ADN SERIAL PRIMARY KEY,
    Persona INTEGER REFERENCES hallazgo(ID_Hallazgo),
    Persona_1 INTEGER REFERENCES victima(ID_Victima)
);


