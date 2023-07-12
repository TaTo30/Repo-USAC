CREATE DATABASE E2;
USE E2;

/*=====================================
CREACION DE LA TABLA REGIONES RECURSIVA
=====================================*/
CREATE TABLE regiones(
id_region int primary key auto_increment,
nombre varchar(100) not null,
region_padre int
);
ALTER TABLE regiones auto_increment = 10;
ALTER TABLE regiones
ADD CONSTRAINT fk_region_padre
FOREIGN KEY (region_padre) REFERENCES regiones(id_region);

/*=========================
CREACION DE LA TABLA PAISES
===========================*/
CREATE TABLE paises(
id_pais int primary key auto_increment,
region int,
nombre varchar(75) default 'unknow',
capital varchar(75) default 'unknow',
poblacion int default 0,
area int default 0
);
ALTER TABLE paises auto_increment = 100;
ALTER TABLE paises 
ADD CONSTRAINT fk_pais_region
FOREIGN KEY (region) REFERENCES regiones(id_region);

/*===========================
CREACION DE LA TABLA Frontera
===========================*/
CREATE TABLE fronteras(
anfrition int,
adyacente int,
cardinalidad char,
primary key (anfrition, adyacente, cardinalidad)
);
ALTER TABLE fronteras
ADD CONSTRAINT fk_frontera_anfrition
FOREIGN KEY (anfrition) REFERENCES paises(id_pais);
ALTER TABLE fronteras
ADD CONSTRAINT fk_frontera_adyacente
FOREIGN KEY (adyacente) REFERENCES paises(id_pais);
ALTER TABLE fronteras rename column cardinalidad to norte;
ALTER TABLE fronteras add column oeste char;
ALTER TABLE fronteras add column sur char;
ALTER TABLE fronteras add column este char;
ALTER TABLE fronteras drop primary key;
alter table fronteras add primary key (anfrition, adyacente, norte, oeste, sur, este);
alter table fronteras drop foreign key fk_frontera_adyacente;
alter table fronteras drop foreign key fk_frontera_anfrition;


/*================================
CREACION DE LA TABLA Profesionales
================================*/
CREATE TABLE profesionales(
id int primary key auto_increment,
pais int,
salario numeric(8,2),
comision varchar(15)
);
ALTER TABLE profesionales auto_increment = 1000;
ALTER TABLE profesionales
ADD CONSTRAINT fk_profesionales_paises
FOREIGN KEY (pais) REFERENCES paises(id_pais);
ALTER TABLE profesionales drop foreign key fk_profesionales_paises;
ALTER TABLE profesionales drop column pais;
ALTER TABLE profesionales add column nombre varchar(100);
ALTER TABLE profesionales modify column salario int;

/*=======================
CREACION DE LA TABLA Area
=======================*/
CREATE TABLE areas(
id int primary key auto_increment,
nombre varchar(100) default 'unknow',
ranking int default 0,
jefe int
);
ALTER TABLE areas auto_increment = 100;
ALTER TABLE areas
ADD CONSTRAINT fk_area_profesional
foreign key (jefe) REFERENCES profesionales(id);


/*============================
CREACION DEL DETALLE pais_area
============================*/
CREATE TABLE pais_area(
pais int,
area int,
primary key (pais, area)
);
ALTER TABLE pais_area ADD CONSTRAINT fk_det_pais
FOREIGN KEY (pais) REFERENCES paises(id_pais);
ALTER TABLE pais_area ADD CONSTRAINT fk_det_area
FOREIGN KEY (area) REFERENCES areas(id);


/*==================================
CREACION DE DETALLE AREA_PROFESIONAL
==================================*/
CREATE TABLE profesional_area(
profesional int,
area int,
primary key (profesional, area)
);
ALTER TABLE profesional_area ADD CONSTRAINT fk_deta_profesional
FOREIGN KEY (profesional) REFERENCES profesionales(id);
ALTER TABLE profesional_area ADD CONSTRAINT fk_deta_area
FOREIGN KEY (area) REFERENCES areas(id);

/*==========================
CREACION DE LA TABLA patente
==========================*/
CREATE TABLE patentes(
id int primary key auto_increment,
nombre varchar(100) default 'unknow',
inventor varchar(100) default 'unknow',
anio int,
pais int
);
ALTER TABLE patentes ADD CONSTRAINT fk_pat_pais
FOREIGN KEY (pais) REFERENCES paises(id_pais);

/*==========================
CREACION DEL Detalle Patente
==========================*/
CREATE TABLE detalle_patente(
patente int,
profesional int,
fecha_contratacion date,
primary key (patente, profesional)
);
ALTER TABLE detalle_patente ADD CONSTRAINT fk_dp
FOREIGN KEY (patente) REFERENCES patentes(id);
ALTER TABLE detalle_patente ADD CONSTRAINT fk_dpr
FOREIGN KEY (profesional) REFERENCES profesionales(id);

/*===========================
CREACION DE LA TABLA Encuesta
===========================*/
CREATE TABLE encuestas(
id int primary key auto_increment,
nombre varchar(255) not null
);
ALTER TABLE encuestas auto_increment = 1000;

/*=============================
CREACION DE LA TABLA respuestas
=============================*/
CREATE TABLE respuestas(
id int primary key auto_increment,
respuesta varchar(255) not null
);
ALTER TABLE respuestas auto_increment = 1000;

/*===========================
CREACION DE LA TABLA Pregunta
===========================*/
CREATE TABLE preguntas(
id int primary key auto_increment,
pregunta varchar(255) not null,
encuesta int, 
respuesta_correcta int
);
ALTER TABLE preguntas auto_increment = 1000;
ALTER TABLE preguntas ADD CONSTRAINT fk_preguntas_encuesta
FOREIGN KEY (encuesta) REFERENCES encuestas(id);
ALTER TABLE preguntas ADD CONSTRAINT fk_preguntas_respuesta
FOREIGN KEY (respuesta_correcta) REFERENCES respuestas(id);


/*===========================
CREACION DEL Detalle Pregunta
===========================*/
CREATE TABLE detalle_pregunta(
pregunta int not null,
respuesta int not null,
primary key (pregunta, respuesta)
);
ALTER TABLE detalle_pregunta ADD CONSTRAINT fkdpp
foreign key (pregunta) references preguntas(id);
ALTER TABLE detalle_pregunta ADD CONSTRAINT fkdpr
foreign key (respuesta) references respuestas(id);


/*=============================
CREACION DE LA TABLA Resultados
=============================*/
CREATE TABLE resultados(
pais int,
pregunta int,
respuesta int,
primary key (pais, pregunta, respuesta)
);
ALTER TABLE resultados ADD CONSTRAINT fk_res_pais
FOREIGN KEY (pais) REFERENCES paises(id_pais);
ALTER TABLE resultados ADD CONSTRAINT fk_res_pregunta
FOREIGN KEY (pregunta) REFERENCES preguntas(id);
ALTER TABLE resultados ADD CONSTRAINT fk_res_respuesta
FOREIGN KEY (respuesta) REFERENCES respuestas(id);









