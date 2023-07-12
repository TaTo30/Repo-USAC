USE BD2P1;

CREATE TABLE pais(
    id int auto_increment,
    nombre varchar(150),
    primary key (id)
);

CREATE TABLE mundial(
    id int auto_increment,
    id_organizador int,
    selecciones int,
    partidos int,
    goles int,
    promedio_gol decimal(5,2),
    primary key (id),
    foreign key (id_organizador) references pais(id)
);
ALTER TABLE mundial ADD COLUMN anio int;

CREATE TABLE jugadores(
    id int auto_increment,
    nombre varchar(150),
    fecha_nacimiento date,
    lugar_nacimiento varchar(150),
    posicion varchar(150),
    numeros_camiseta varchar(150),
    altura decimal(5,2),
    sitio_web varchar(250),
    primary key (id)
);
ALTER TABLE  jugadores ADD COLUMN apodo varchar(150);
ALTER TABLE jugadores ADD COLUMN nacionalidad varchar(150);

CREATE TABLE seleccion(
    id int,
    id_pais int,
    id_mundial int,
    primary key (id),
    foreign key (id_pais) references pais(id),
    foreign key (id_mundial) references mundial(id)
);

CREATE TABLE seleccion_jugador(
    id_seleccion int,
    id_jugador int,
    foreign key (id_seleccion) references seleccion(id),
    foreign key (id_jugador) references jugadores(id)
);

CREATE TABLE etapa(
    id int auto_increment,
    etapa varchar(150),
    id_mundial int,
    primary key (id),
    foreign key (id_mundial) references mundial(id)
);

CREATE TABLE detalle_etapa(
    id_etapa int,
    id_pais int,
    foreign key (id_etapa) references etapa(id),
    foreign key (id_pais) references pais(id)
);

drop table partidos;
CREATE TABLE partidos(
    id int auto_increment,
    seleccion_a int,
    seleccion_b int,
    goles_a int,
    goles_b int,
    fecha date,
    id_etapa int,
    primary key (id),
    foreign key (seleccion_a) references seleccion(id),
    foreign key (seleccion_b) references seleccion(id),
    foreign key (id_etapa) references etapa(id)
);

drop table goles;
CREATE TABLE goles(
    id int auto_increment,
    id_seleccion int,
    id_jugador int,
    id_partido int,
    minuto varchar(50),
    penal boolean,
    primary key (id),
    foreign key (id_seleccion) references seleccion(id),
    foreign key (id_jugador) references jugadores(id),
    foreign key (id_partido) references partidos(id)
);

CREATE TABLE posiciones(
    id int auto_increment,
    posicion varchar(100),
    id_seleccion int,
    id_mundial int,
    primary key (id),
    foreign key (id_seleccion) references seleccion(id),
    foreign key (id_mundial) references mundial(id)
);

CREATE TABLE goleadores(
    id int auto_increment,
    id_mundial int,
    id_jugador int,
    goles int,
    promedio_gol decimal(5,2),
    partidos int,
    primary key (id),
    foreign key (id_mundial) references mundial(id),
    foreign key (id_jugador) references jugadores(id)
);

drop table premios;
CREATE TABLE premios(
    id int auto_increment,
    premio varchar(150),
    categoria varchar(150),
    id_jugador int,
    id_mundial int,
    id_pais int,
    primary key (id),
    foreign key (id_pais) references pais(id),
    foreign key (id_jugador) references jugadores(id),
    foreign key (id_mundial) references mundial(id)
);



select * from pais;
select * from mundial;
select * from jugadores;
select * from seleccion;
select * from seleccion_jugador;
select * from posiciones;
select * from goleadores;
select * from etapa;
select * from detalle_etapa;
select * from premios;

select etapa.id from etapa
inner join mundial m on etapa.id_mundial = m.id
where m.anio = 2010 and etapa like '%Grupo H%';

SELECT id FROM seleccion
where id_mundial = (SELECT id from mundial where anio = 1930)
and id_pais = (SELECT id from pais where nombre = 'Brasil');

SELECT id from jugadores where  nombre like '%Velloso%';


select cast(posicion as decimal), p.nombre, m.anio from posiciones
inner join seleccion s on posiciones.id_seleccion = s.id
inner join pais p on s.id_pais = p.id
inner join mundial m on posiciones.id_mundial = m.id
where anio = 2010 order by 1;

select * from partidos;
