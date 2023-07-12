create database seminariodos201800585columnar;

use seminariodos201800585columnar;

create nonclustered columnstore index cliente_index on cliente (idCliente);
create nonclustered columnstore index producto_index on producto (idProducto);
create nonclustered columnstore index proveedor_index on proveedor (idProveedor);
create nonclustered columnstore index sucursal_index on sucursal (idSucursal);
create nonclustered columnstore index tiempo_index on tiempo (idTiempo);
create nonclustered columnstore index vendedor_index on vendedor (idVendedor);

create nonclustered columnstore index fact_compras_index on fact_compras (id, idSucursal, idProducto, idProveedor, idTiempo, unidades, costo);
create nonclustered columnstore index fact_ventas_index on fact_ventas (id, idSucursal, idProducto, idCliente, idVendedor, idTiempo, unidades, costo);


select (select count(*) from cliente) as clientes,
       (select count(*) from vendedor) as vendedores,
       (select count(*) from proveedor) as proveedores,
       (select count(*) from producto) as productos,
       (select count(*) from sucursal) as sucursales,
       (select count(*) from tiempo) as tiempos,
       (select count(*) from fact_compras) as fact_compras,
       (select count(*) from fact_ventas) as fact_ventas,
       '201800585' as carnet;
