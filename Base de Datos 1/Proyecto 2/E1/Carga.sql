CREATE TABLE temporal (
nombre_compania varchar(250),
contacto_compania varchar(250),
correo_compania varchar(250),
telefono_compania varchar(250),
tipo varchar(10),
nombre varchar(250),
correo varchar(250),
telefono varchar(250),
fecha_registro varchar(250),
direccion varchar(250),
ciudad varchar(250),
codigo_postal varchar(250),
region varchar(250),
producto varchar(250),
categoria_producto varchar(250),
cantidad varchar(250),
precio_unitario varchar(250)
);

LOAD DATA INFILE '/home/aldoh/Descargas/Enunciado1.csv'
INTO TABLE temporal
FIELDS terminated by ';'
LINES TERMINATED BY '\n'
IGNORE 1 ROWS;

/*=============================
INSERSION A LA TABLA categorias
=============================*/
SELECT DISTINCT categoria_producto FROM temporal;
INSERT INTO categorias (nombre) (SELECT DISTINCT categoria_producto FROM temporal);
SELECT * FROM categorias;

/*============================
INSERSION A LA TABLA productos
============================*/
SELECT DISTINCT producto, precio_unitario, B.no_Categoria
FROM temporal A
INNER JOIN categorias B ON A.categoria_producto = B.nombre;
INSERT INTO producto (nombre, precio, no_categoria) (
SELECT DISTINCT producto, precio_unitario, B.no_Categoria
FROM temporal A
INNER JOIN categorias B ON A.categoria_producto = B.nombre
);
SELECT * FROM producto;

/*============================
INSERSION A LA TABLA companias
============================*/

SELECT DISTINCT 
nombre_compania, 
contacto_compania, 
correo_compania, 
telefono_compania
FROM temporal;
INSERT INTO companias (nombre, contacto, correo, telefono) 
(
SELECT DISTINCT 
nombre_compania, 
contacto_compania, 
correo_compania, 
telefono_compania
FROM temporal
);
SELECT * FROM companias;

/*===========================
INSERSION A LA TABLA regiones
===========================*/
SELECT DISTINCT region FROM temporal;
INSERT INTO regiones (nombre) 
(
SELECT DISTINCT region FROM temporal
);
SELECT * FROM regiones;


/*===========================
INSERSION A LA TABLA ciudades
===========================*/
SELECT DISTINCT A.ciudad, B.no_Region
FROM temporal A
INNER JOIN regiones B ON A.region = B.nombre;

INSERT INTO ciudades (nombre, no_region) (
SELECT DISTINCT A.ciudad, B.no_Region
FROM temporal A
INNER JOIN regiones B ON A.region = B.nombre
);
SELECT * FROM ciudades;

/*=============================
INSERSION DE LA TABLA direccion
=============================*/
SELECT DISTINCT A.direccion, A.codigo_postal, B.no_Ciudad
FROM temporal A
INNER JOIN ciudades B on A.ciudad = B.nombre;

INSERT INTO direccion (direccion, codigo_postal, no_ciudad) 
(
SELECT DISTINCT A.direccion, A.codigo_postal, B.no_Ciudad
FROM temporal A
INNER JOIN ciudades B on A.ciudad = B.nombre
);
SELECT * FROM direccion;

/*============================
INSERSION A LA TABLA entidades
============================*/
SELECT DISTINCT A.tipo, A.nombre, A.correo, A.telefono, 
str_to_date(A.fecha_registro, '%d/%m/%Y'), B.no_Direccion
FROM temporal A
INNER JOIN direccion B on A.direccion = B.direccion;

INSERT INTO entidades (tipo, nombre, correo, telefono, fecha_registro, no_direccion) 
(
SELECT DISTINCT A.tipo, A.nombre, A.correo, A.telefono, str_to_date(A.fecha_registro, '%d/%m/%Y'), B.no_Direccion
FROM temporal A
INNER JOIN direccion B on A.direccion = B.direccion
);

SELECT * FROM entidades;

/*==============================
INSERSION A LA TABLA transaccion
==============================*/

INSERT INTO transaccion (tipo, cantidad, no_producto, no_entidad, no_compania)
(
SELECT DISTINCT
CASE
	WHEN A.tipo = 'C' THEN 'V'
    WHEN A.tipo = 'P' THEN 'C'
END as 'tipo',
A.cantidad, B.no_Producto, C.no_Entidad, D.no_Compania
FROM temporal A
INNER JOIN producto B ON A.producto = B.nombre
INNER JOIN entidades C ON C.nombre = A.nombre
INNER JOIN companias D ON A.nombre_compania = D.nombre
);


SELECT 'V', A.cantidad, B.no_Producto, C.no_Entidad, D.no_Compania
FROM temporal A
INNER JOIN producto B ON A.producto = B.nombre
INNER JOIN entidades C ON C.nombre = A.nombre
INNER JOIN companias D ON A.nombre_compania = D.nombre
WHERE A.tipo = 'C';

SELECT count(*) from temporal where tipo = 'C';

CREATE TABLE temporal2(
tipo char,
cantidad int,
producto int,
entidad int,
compania varchar(100)
);

INSERT INTO temporal1 (tipo, cantidad, producto, entidad, compania)
SELECT DISTINCT
CASE
	WHEN A.tipo = 'C' THEN 'V'
    WHEN A.tipo = 'P' THEN 'C'
END as 'tipo',
A.cantidad, B.no_Producto, A.nombre, A.nombre_compania
FROM temporal A
INNER JOIN producto B ON A.producto = B.nombre;

INSERT INTO temporal2 (tipo, cantidad, producto, entidad, compania)
SELECT A.tipo, A.cantidad, A.producto, B.no_Entidad, A.compania 
FROM temporal1 A
INNER JOIN entidades B ON A.entidad = B.nombre;

INSERT INTO transaccion (tipo, cantidad, no_producto, no_entidad, no_compania)
SELECT A.tipo, A.cantidad, A.producto, A.entidad, B.no_Compania
FROM temporal2 A
INNER JOIN companias B ON A.compania = B.nombre;

SELECT * FROM transaccion;

DROP table temporal1, temporal2;