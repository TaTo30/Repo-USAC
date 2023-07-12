use Tests;

/*=====================CARGA DE DATOS.SQL=====================*/
LOAD DATA LOCAL INFILE '/home/aldoh/Desarrollo/Manejo e Implementacion de Archivos/DATA.csv'
INTO TABLE temporal
fields terminated by ';'
lines terminated by '\n'
ignore 1 rows;
select * from temporal;
truncate table temporal;

insert into compania (nombre, contacto, correo, telefono) 
select nombre_compania, contacto_compania, correo_compania, telefono_compania from temporal group by nombre_compania order by nombre_compania;
select * from compania;
truncate table compania;

insert into cliente (nombre, correo, telefono, fecha, direccion, ciudad, codigoPostal, region)
select nombre, correo, telefono, fecha_registro, direccion, ciudad, codigo_postal, region from temporal where tipo = 'C' group by nombre order by nombre;
select * from cliente;
truncate table cliente;

insert into proveedor (nombre, correo, telefono, fecha, direccion, ciudad, codigoPostal, region)
select nombre, correo, telefono, fecha_registro, direccion, ciudad, codigo_postal, region from temporal where tipo = 'P' group by nombre order by nombre;
select * from proveedor;
truncate table proveedor;

insert into compra (compania, proveedor, producto, cantidad)
select nombre_compania, nombre, producto, cantidad from temporal where tipo = 'P';
select * from compra;
truncate table compra;

insert into venta (compania, cliente, producto, cantidad)
select nombre_compania, nombre, producto, cantidad from temporal where tipo = 'C';
select * from venta;
truncate table venta;

insert into producto (nombre, categoria, precio)
select producto, categoria_producto, precio_unitario from temporal group by producto;
select * from producto;
truncate table producto;