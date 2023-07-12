CREATE DATABASE E1;
USE E1;

/*==============================
CREACION DE LA TABLA 'Companias'
==============================*/
CREATE TABLE companias(
no_Compania int primary key auto_increment,
nombre varchar(100) default 'unknow',
contacto varchar(100) default 'unknow',
correo varchar(100) default 'correo@correo',
telefono varchar(50) default '00'
);
ALTER TABLE companias auto_increment = 1000;

/*==============================
CREACION DE LA TABLA 'Entidades'
==============================*/
CREATE TABLE entidades(
no_Entidad int primary key auto_increment,
tipo char default 'C',
nombre varchar(100) default 'unknow',
correo varchar(100) default 'correo@correo',
telefono varchar(50) default '00',
fecha_registro date default null,
no_direccion int not null
);

ALTER TABLE entidades 
auto_increment = 1000;
ALTER TABLE entidades
ADD CONSTRAINT fk_ent_direccion
FOREIGN KEY (no_direccion)
REFERENCES direccion(no_Direccion);


/*=============================
CREACION DE LA TABLA 'Ciudades'
=============================*/
CREATE TABLE ciudades(
no_Ciudad int primary key auto_increment,
nombre varchar(50) not null,
no_region int not null
);
ALTER TABLE ciudades auto_increment = 100;
ALTER TABLE ciudades
ADD CONSTRAINT fk_ciudad_region
FOREIGN KEY (no_region)
REFERENCES regiones(no_Region);


/*==============================
CREACION DE LA TABLA 'Regiones
==============================*/
CREATE TABLE regiones(
no_Region int primary key auto_increment,
nombre varchar(50) not null
);
ALTER TABLE regiones
AUTO_INCREMENT = 100;
DROP TABLE regiones;


/*================================
CREACION DE LA TABLA 'Direcciones'
================================*/
CREATE TABLE direccion(
no_Direccion int primary key auto_increment,
direccion varchar(100),
codigo_postal int,
no_ciudad int not null
);

ALTER TABLE direccion
AUTO_INCREMENT = 1000;
ALTER TABLE direccion
ADD CONSTRAINT fk_direccion_ciudad
FOREIGN KEY (no_ciudad)
REFERENCES ciudades(no_Ciudad);



/*==============================
CREACION DE LA TABLA 'Categorias
==============================*/
CREATE TABLE categorias(
no_Categoria int primary key auto_increment,
nombre varchar(50)
);
ALTER TABLE categorias
AUTO_INCREMENT = 10;


/*=============================
CREACION DE LA TABLA 'Producto'
=============================*/
CREATE TABLE producto(
no_Producto int primary key auto_increment,
nombre varchar(100),
precio int default 0,
no_categoria int not null
);
ALTER TABLE producto
AUTO_INCREMENT = 1000;
ALTER TABLE producto
ADD CONSTRAINT fk_producto_categoria
FOREIGN KEY (no_categoria)
REFERENCES categorias(no_Categoria);

/*================================
CREACION DE LA TABLA 'Transaccion'
================================*/
CREATE TABLE transaccion(
no_Transaccion int primary key auto_increment,
tipo char default 'C',
cantidad int default 0,
no_producto int not null,
no_entidad int not null,
no_compania int not null
);
ALTER TABLE transaccion
AUTO_INCREMENT = 10000;
ALTER TABLE transaccion
ADD CONSTRAINT fk_transaccion_producto
FOREIGN KEY (no_producto)
REFERENCES producto(no_Producto);
ALTER TABLE transaccion
ADD CONSTRAINT fk_transaccion_entidad
FOREIGN KEY (no_entidad)
REFERENCES entidades(no_Entidad);
ALTER TABLE transaccion
ADD CONSTRAINT fk_transaccion_compania
FOREIGN KEY (no_compania)
REFERENCES companias(no_Compania);




