/*========
CONSULTA 1
========*/
SELECT A.no_entidad as 'no. Cliente', B.nombre, sum(A.cantidad) as 'Productos Adquiridos'
FROM transaccion A
INNER JOIN entidades B on A.no_entidad = B.no_Entidad
WHERE A.tipo = 'V'
GROUP BY A.no_entidad
ORDER BY sum(A.cantidad) DESC LIMIT 1;

/*========
CONSULTA 2
========*/
(
SELECT month(B.fecha_registro) as 'Mes registrado', B.nombre, sum(A.cantidad*C.precio) as 'Dinero Gastado'
FROM transaccion A
INNER JOIN entidades B on A.no_entidad = B.no_Entidad
INNER JOIN producto C on A.no_producto = C.no_Producto
WHERE A.tipo = 'V'
GROUP BY A.no_entidad
ORDER BY sum(A.cantidad*C.precio) DESC LIMIT 3
)
UNION
(
SELECT month(B.fecha_registro) as 'Mes registrado', B.nombre, sum(A.cantidad*C.precio) as 'Dinero Gastado'
FROM transaccion A
INNER JOIN entidades B on A.no_entidad = B.no_Entidad
INNER JOIN producto C on A.no_producto = C.no_Producto
WHERE A.tipo = 'V'
GROUP BY A.no_entidad
ORDER BY sum(A.cantidad*C.precio) ASC LIMIT 3
);


/*========
CONSULTA 3
========*/
SELECT D.nombre, sum(A.cantidad * B.precio) as 'Dinero Recaudado'
FROM transaccion A
INNER JOIN producto B ON A.no_producto = B.no_Producto
INNER JOIN categorias C ON B.no_categoria = C.no_Categoria
INNER JOIN entidades D ON A.no_entidad = D.no_Entidad
WHERE A.tipo = 'C' and C.nombre = 'Fresh Vegetables'
GROUP BY A.no_entidad
ORDER BY 2 DESC LIMIT 5;

/*========
CONSULTA 4
========*/
SELECT B.nombre, B.telefono, A.no_Transaccion as 'No Orden', A.cantidad,  C.precio as 'Total' FROM transaccion A
INNER JOIN entidades B ON A.no_entidad = B.no_Entidad
INNER JOIN producto C ON A.no_producto = C.no_Producto
WHERE A.cantidad = (SELECT min(cantidad) FROM transaccion) AND A.tipo = 'C';

/*========
CONSULTA 5
========*/
SELECT B.nombre, sum(A.cantidad) as 'Productos Adquiridos' 
FROM transaccion A
INNER JOIN entidades B ON A.no_entidad = B.no_Entidad
INNER JOIN producto C ON A.no_producto = C.no_Producto
INNER JOIN categorias D ON C.no_categoria = D.no_Categoria
WHERE D.nombre = 'Seafood' AND A.tipo = 'V'
GROUP BY A.no_entidad
ORDER BY sum(A.cantidad) DESC LIMIT 10;

/*========
CONSULTA 6
========*/

SELECT D.nombre, 
100*(count(D.no_Region)/(SELECT count(*) FROM entidades WHERE tipo = 'C')) as 'Porcentaje de Clientes'
FROM entidades A
INNER JOIN direccion B ON A.no_direccion = B.no_Direccion
INNER JOIN ciudades C ON B.no_ciudad = C.no_Ciudad
INNER JOIN regiones D ON C.no_region = D.no_Region
WHERE A.tipo = 'C'
GROUP BY D.no_Region;

/*========
CONSULTA 7
========*/
SELECT E.nombre, sum(A.cantidad) as 'Veces Consumida'
FROM transaccion A
INNER JOIN producto B ON A.no_producto = B.no_Producto
INNER JOIN entidades C ON A.no_entidad = C.no_Entidad
INNER JOIN direccion D ON C.no_direccion = D.no_Direccion
INNER JOIN ciudades E ON D.no_ciudad = E.no_Ciudad
WHERE B.nombre = 'Tortillas' AND A.tipo = 'V'
GROUP BY E.no_Ciudad
ORDER BY 2 DESC;

/*========
CONSULTA 8
========*/
SELECT substring(C.nombre, 1, 1) as 'Letra Inicial', count(*) as 'No Clientes'
FROM entidades A
INNER JOIN direccion B ON A.no_direccion = B.no_Direccion
INNER JOIN ciudades C ON B.no_ciudad = C.no_Ciudad
WHERE A.tipo = 'C'
GROUP BY 1
ORDER BY 1;

/*========
CONSULTA 9
========*/
SELECT D.nombre, F.nombre, 100*(count(*)/(SELECT count(*) FROM transaccion WHERE A.tipo = 'V')) as 'Porcentaje del Mercado'
FROM transaccion A
INNER JOIN entidades B ON A.no_entidad = B.no_Entidad
INNER JOIN direccion C ON B.no_direccion = C.no_Direccion
INNER JOIN ciudades D ON C.no_ciudad = D.no_Ciudad
INNER JOIN producto E ON A.no_producto = E.no_Producto
INNER JOIN categorias F ON E.no_categoria = F.no_Categoria
WHERE A.tipo = 'V'
GROUP BY 1,2;

/*=========
CONSULTA 10
=========*/
SELECT B.nombre, sum(cantidad) 'Consumido'
FROM transaccion A
INNER JOIN entidades B ON A.no_entidad = B.no_Entidad
INNER JOIN direccion C ON B.no_direccion = C.no_Direccion
INNER JOIN ciudades D ON C.no_ciudad = D.no_Ciudad
WHERE A.tipo = 'V' AND D.nombre = 'Frankfort'
GROUP BY B.no_Entidad
HAVING sum(cantidad) > 
(
SELECT avg(A.cantidad) from 
(
SELECT sum(cantidad) as 'cantidad'
FROM transaccion A
INNER JOIN entidades B ON A.no_entidad = B.no_Entidad
INNER JOIN direccion C ON B.no_direccion = C.no_Direccion
INNER JOIN ciudades D ON C.no_ciudad = D.no_Ciudad
WHERE A.tipo = 'V' AND D.nombre = 'Frankfort'
GROUP BY B.no_Entidad
) A
);

