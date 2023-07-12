use E2;

CREATE TABLE temp_uno(
invento varchar(100),
inventor varchar(100),
profesinal_asignado varchar(100),
profesinal_es_jefe_de_area varchar(100),
fecha_contrato varchar(100),
salario varchar(100),
comision varchar(100),
area_inv_profesional varchar(100),
ranking varchar(100),
anio_el_invento varchar(100),
pais_del_invento varchar(100),
pais_del_inventor varchar(100),
region_pais varchar(100),
capital varchar(100),
poblacion varchar(100),
area_en_km2 varchar(100),
frontera_con varchar(100),
norte varchar(100),
sur varchar(100),
este varchar(100),
oeste varchar(100)
);

create table temp_dos(
encuesta varchar(255),
pregunta varchar(255),
respuesta_posible varchar(255),
respuesta_correcta varchar(255),
pais varchar(255),
respuesta_pais varchar(255)
);

create table temp_tres(
region varchar(100),
region_padre varchar(100)
);

select * from temp_uno;
select * from temp_dos;
select * from temp_tres;

/*=======================
CARGA A LA TABLA regiones
=======================*/
INSERT INTO regiones (nombre) SELECT DISTINCT region
FROM temp_tres A;
SELECT * FROM regiones;	
update regiones set region_padre = 1 where id_region between 2 and 5;
update regiones set region_padre = 6 where id_region between 7 and 9;
update regiones set region_padre = 10 where id_region between 11 and 12;
update regiones set region_padre = 13 where id_region between 14 and 17;
update regiones set region_padre = 18 where id_region between 19 and 21;

/*===================
CARGA A LA TABLE pais
===================*/
SELECT DISTINCT A.pais_del_inventor, B.id_region, capital, poblacion, area_en_km2
FROM temp_uno A
INNER JOIN regiones B ON A.region_pais = B.nombre;

INSERT INTO paises (nombre, region, capital, poblacion, area) 
SELECT DISTINCT A.pais_del_inventor, B.id_region, capital, poblacion, area_en_km2
FROM temp_uno A
INNER JOIN regiones B ON A.region_pais = B.nombre;

SELECT * FROM paises;

/*========================
CARGA DE LA TABLA Frontera
========================*/
INSERT INTO paises (nombre, region, capital, poblacion, area) values ('Belgica', 6, 'unknow',0,0);
INSERT INTO fronteras (anfrition, adyacente, norte, sur, este, oeste)
SELECT DISTINCT B.id_pais, C.id_pais, norte, sur, este, oeste
FROM temp_uno A
INNER JOIN paises B ON A.pais_del_inventor = B.nombre
INNER JOIN paises C ON A.frontera_con = C.nombre;
DELETE FROM fronteras where anfrition > 0;
SELECT * FROM fronteras;

/*=============================
CARGA DE LA TABLA Profesionales
=============================*/
INSERT INTO profesionales (nombre, salario, comision)
SELECT DISTINCT profesinal_asignado, salario, comision
FROM temp_uno A limit 12;
SELECT * FROM profesionales;

/*====================
CARGA DE LA TABLA area
====================*/
INSERT INTO areas(nombre, ranking, jefe)
SELECT A.nombre, A.ranking, B.id 
FROM
(
SELECT DISTINCT area_inv_profesional as nombre, ranking
FROM temp_uno A WHERE area_inv_profesional != ''
) A
LEFT JOIN 
(
SELECT DISTINCT C.id as id, profesinal_asignado, profesinal_es_jefe_de_area
FROM temp_uno B
INNER JOIN profesionales C ON B.profesinal_asignado = C.nombre
WHERE profesinal_es_jefe_de_area != ''
) B ON A.nombre = B.profesinal_es_jefe_de_area;
SELECT * FROM areas;


/*=============
CARGA pais_area
=============*/
INSERT INTO pais_area(area, pais)
SELECT distinct B.id, C.id_pais
FROM temp_uno A
INNER JOIN areas B ON A.area_inv_profesional = B.nombre
INNER JOIN paises C ON A.pais_del_invento = C.nombre;
SELECT * FROM pais_area;


/*====================
CARGA profesional_area
====================*/
INSERT INTO profesional_area(profesional, area)
SELECT DISTINCT C.id, B.id
FROM temp_uno A
INNER JOIN areas B ON A.area_inv_profesional = B.nombre
INNER JOIN profesionales C ON A.profesinal_asignado = C.nombre;
SELECT * FROM profesional_area;


/*===========
CARGA Patente
============*/
INSERT INTO patentes(nombre, inventor, anio, pais)
SELECT DISTINCT invento, inventor, anio_el_invento, B.id_pais
FROM temp_uno A
INNER JOIN paises B ON A.pais_del_invento = B.nombre;
SELECT * FROM patentes;

/*====================
CARGA detalle patentes
====================*/
INSERT INTO detalle_patente (patente, profesional, fecha_contratacion)
SELECT DISTINCT B.id, C.id, fecha_contrato
FROM temp_uno A
INNER JOIN patentes B ON A.invento = B.nombre
INNER JOIN profesionales C ON A.profesinal_asignado = C.nombre;
SELECT * FROM detalle_patente;


/*===============
CARGA de encuesta
===============*/
INSERT INTO encuestas (nombre)
SELECT DISTINCT encuesta 
FROM temp_dos;
SELECT * FROM encuestas;


/*=================
CARGA de respuestas
=================*/
INSERT INTO respuestas (respuesta)
SELECT DISTINCT respuesta_posible
FROM temp_dos;
SELECT * FROM respuestas;


/*================
CARGA DE preguntas
================*/
INSERT INTO preguntas (pregunta, encuesta, respuesta_correcta)
SELECT DISTINCT A.pregunta, C.id, B.id
FROM temp_dos A
INNER JOIN respuestas B ON A.respuesta_correcta = B.respuesta
INNER JOIN encuestas C ON A.encuesta = C.nombre;
SELECT * FROM preguntas;


/*========================
CARGA DE detalle preguntas
========================*/
INSERT INTO detalle_pregunta (pregunta, respuesta)
SELECT distinct B.id, C.id
FROM temp_dos A
INNER JOIN preguntas B ON A.pregunta = B.pregunta
INNER JOIN respuestas C ON A.respuesta_posible = C.respuesta;


/*=================
CARGA DE resultados
=================*/
INSERT INTO resultados (pais, pregunta, respuesta)
SELECT DISTINCT E.id_pais, B.id, C.id
FROM temp_dos A
INNER JOIN preguntas B ON A.pregunta = B.pregunta
INNER JOIN respuestas C ON C.respuesta LIKE concat(A.respuesta_pais,'%')
INNER JOIN detalle_pregunta D ON D.pregunta = B.id and D.respuesta = C.id
INNER JOIN paises E ON E.nombre = A.pais;
SELECT * FROM resultados;

