/*========
CONSULTA 1
========*/

SELECT C.nombre 'Jefe de Area', D.nombre 'Subalterno', A.nombre Area
FROM areas A 
INNER JOIN profesional_area B ON A.id = B.area
INNER JOIN profesionales C ON A.jefe = C.id
INNER JOIN profesionales D ON B.profesional = D.id
ORDER BY 1;

/*========
CONSULTA 2*
========*/
SELECT nombre, salario 
FROM profesionales WHERE salario > 
(
SELECT sum(salario)/count(salario)
FROM profesionales
)
ORDER BY 2 DESC;

/*========
CONSULTA 3
========*/
SELECT substring(nombre, 1, 1) Inicial, sum(area) Area
FROM paises
GROUP BY 1;

/*========
CONSULTA 4
========*/
SELECT inventor, anio
FROM patentes
WHERE (inventor LIKE 'B%r' OR inventor LIKE 'B%n')
AND (anio BETWEEN 1801 AND 1900);	

/*========
CONSULTA 5
========*/
SELECT B.nombre Pais, B.area Area, count(A.anfrition) Fronteras
FROM fronteras A
INNER JOIN paises B ON A.anfrition = B.id_pais
GROUP BY A.anfrition
HAVING count(A.anfrition) > 7
ORDER BY B.area DESC;


/*========
CONSULTA 6
========*/
SELECT nombre, salario, comision, salario+comision sueldo
FROM profesionales
WHERE comision > salario/4;


/*========
CONSULTA 7
========*/
SELECT nombre, poblacion
FROM paises
WHERE poblacion > 
(
SELECT SUM(poblacion)
FROM paises A
INNER JOIN regiones B ON A.region = B.id_region
WHERE B.nombre = 'Centro America'
);


/*========
CONSULTA 8
========*/
SELECT nombre Invento, anio Año 
FROM patentes
WHERE anio in
(
SELECT anio FROM patentes
WHERE inventor like 'Benz'
);

/*========
CONSULTA 9
========*/

SELECT nombre, poblacion
FROM paises
WHERE id_pais NOT IN
(
SELECT DISTINCT anfrition FROM fronteras
)
AND
area >= 
(
SELECT area FROM paises
WHERE nombre = 'Japon'
);

/*=========
CONSULTA 10
=========*/
SELECT nombre, salario, comision
FROM profesionales
WHERE salario > comision*2;






