create database seminariodos201800585;

use seminariodos201800585;

set ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON


create table temporalCompras(
    fecha varchar(150),
    codProveedor varchar(150),
    nombreProveedor varchar(150),
    direccionProveedor varchar(150),
    numeroProveedor varchar(150),
    webProveedor varchar(150),
    codProducto varchar(150),
    nombreProducto varchar(150),
    marcaProducto varchar(150),
    categoria varchar(150),
    codSucursal varchar(150),
    nombreSucursal varchar(150),
    direccionSucursal varchar(150),
    region varchar(150),
    departamento varchar(150),
    unidades varchar(150),
    costoU varchar(150),
);

create table temporalVentas(
    fecha varchar(150),
    codigoCliente varchar(150),
    nombreCliente varchar(150),
    tipoCliente varchar(150),
    direccionCliente varchar(150),
    numeroCliente varchar(150),
    codVendedor varchar(150),
    nombreVendedor varchar(150),
    vacacionista varchar(150),
    codProducto varchar(150),
    nombreProducto varchar(150),
    marcaProducto varchar(150),
    categoria varchar(150),
    codSucursal varchar(150),
    nombreSucursal varchar(150),
    direccionSucursal varchar(150),
    region varchar(150),
    departamento varchar(150),
    unidades varchar(150),
    precioU varchar(150),
);


/*COMPRAS Y VENTAS*/
create table producto(
    idProducto int identity primary key,
    codigo varchar(150),
    nombre varchar(150),
    marca varchar(150),
    categoria varchar(150),
);

create table sucursal(
    idSucursal int identity primary key,
    codigo varchar(150),
    nombre varchar(150),
    direccion varchar(150),
    region varchar(150),
    departamento varchar(150)
);


create table tiempo(
    idTiempo int identity primary key,
    day int,
    month int,
    year int
);


/*COMPRAS*/
create table proveedor(
    idProveedor int identity primary key,
    codigo varchar(150),
    nombre varchar(150),
    direccion varchar(150),
    numero bigint,
    web bit,
);

/*VENTAS*/
create table cliente(
    idCliente int identity primary key,
    codigo varchar(150),
    nombre varchar(150),
    tipo varchar(150),
    direccion varchar(150),
    numero bigint
);

create table vendedor(
    idVendedor int identity primary key,
    codigo varchar(150),
    nombre varchar(150),
    vacacionista bit
);

/*TABLAS DE HECHOS*/
create table fact_compras(
    id int identity primary key,
    idSucursal int,
    idProducto int,
    idProveedor int,
    idTiempo int,
    unidades int,
    costo decimal(18,2)
);

create table fact_ventas(
    id int identity primary key,
    idSucursal int,
    idProducto int,
    idCliente int,
    idVendedor int,
    idTiempo int,
    unidades int,
    costo decimal(18,2)
);


select * from temporalCompras;
select * from temporalVentas;

select * from producto;
select * from sucursal;
select * from tiempo;

select * from proveedor;

select * from cliente;
select * from vendedor;

select * from fact_compras;
select * from fact_ventas;

select (select count(*) from cliente) as clientes,
       (select count(*) from vendedor) as vendedores,
       (select count(*) from proveedor) as proveedores,
       (select count(*) from producto) as productos,
       (select count(*) from sucursal) as sucursales,
       (select count(*) from tiempo) as tiempos,
       (select count(*) from fact_compras) as fact_compras,
       (select count(*) from fact_ventas) as fact_ventas,
       '201800585' as carnet;
