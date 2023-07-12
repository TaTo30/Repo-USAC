/*=======CONSULTA 1========*/
select proveedor, telefono, noOrden, cantidad*precio as 'Precio pagado' from compra inner join producto on compra.producto = producto.nombre inner join proveedor on proveedor.nombre = compra.proveedor order by cantidad*precio desc, proveedor limit 1;
/*=======CONSULTA 2========*/
select noCliente, cliente, sum(cantidad) as 'Productos comprados' from venta inner join cliente on venta.cliente = cliente.nombre group by cliente order by sum(cantidad) desc limit 1;
/*=======CONSULTA 3========*/
(select proveedor, direccion, region, ciudad, codigoPostal, count(proveedor) as 'Pedidos hechos' from compra inner join proveedor on compra.proveedor = proveedor.nombre group by proveedor order by count(proveedor) desc limit 1) union (select proveedor, direccion, region, ciudad, codigoPostal, count(proveedor) as 'Pedidos hechos' from compra inner join proveedor on compra.proveedor = proveedor.nombre group by proveedor order by count(proveedor) asc limit 1);
/*=======CONSULTA 4========*/
select noCliente, cliente, count(cantidad) as 'Ordenes hechas', sum(cantidad) as 'Articulos adquiridos' from venta inner join producto on venta.producto = producto.nombre inner join cliente on venta.cliente = cliente.nombre where producto.categoria = 'Cheese' group by cliente order by sum(cantidad) desc limit 5;
/*=======CONSULTA 5========*/
(select fecha, cliente, sum(cantidad*precio) as 'Total gastado' from venta inner join cliente on venta.cliente = cliente.nombre inner join producto on venta.producto = producto.nombre group by cliente order by sum(cantidad*precio) desc limit 3) union (select fecha, cliente, sum(cantidad*precio) as 'Total gastado' from venta inner join cliente on venta.cliente = cliente.nombre inner join producto on venta.producto = producto.nombre group by cliente order by sum(cantidad*precio) asc limit 3);
/*=======CONSULTA 6========*/
(select categoria, sum(cantidad) as 'Articulos vendidos', sum(cantidad*precio) as 'Monto recaudado' from venta inner join producto on venta.producto = producto.nombre group by categoria order by sum(cantidad) desc, sum(cantidad*precio) desc limit 1) union (select categoria, sum(cantidad) as 'Articulos vendidos', sum(cantidad*precio) as 'Monto recaudado' from venta inner join producto on venta.producto = producto.nombre group by categoria order by sum(cantidad) asc, sum(cantidad*precio) asc limit 1);
/*=======CONSULTA 7========*/
select proveedor, sum(cantidad*precio) as 'Montor recaudado' from compra inner join producto on compra.producto = producto.nombre where categoria = 'Fresh vegetables'  group by proveedor order by sum(cantidad*precio) desc limit 5;
/*=======CONSULTA 8========*/
(select cliente, direccion, region, ciudad, codigoPostal, sum(cantidad*precio) as 'Monto gastado' from venta inner join cliente on venta.cliente = cliente.nombre inner join producto on venta.producto = producto.nombre group by cliente order by sum(cantidad*precio) desc limit 3) union (select cliente, direccion, region, ciudad, codigoPostal, sum(cantidad*precio) as 'Monto gastado' from venta inner join cliente on venta.cliente = cliente.nombre inner join producto on venta.producto = producto.nombre group by cliente order by sum(cantidad*precio) asc limit 3);
/*=======CONSULTA 9========*/
select proveedor, telefono, noOrden, producto, cantidad, cantidad*precio as 'Total recaudado'from compra inner join producto on compra.producto = producto.nombre inner join proveedor on proveedor.nombre = compra.proveedor order by cantidad asc, cantidad*precio asc limit 1;
/*=======CONSULTA 10=======*/
select cliente, sum(cantidad) as 'Articulos adquiridos' from venta inner join producto on venta.producto = producto.nombre where producto.categoria = 'Seafood' group by cliente order by sum(cantidad) desc limit 10;
