create table jugadores
(
    id               int auto_increment
        primary key,
    nombre           varchar(150)  null,
    fecha_nacimiento date          null,
    lugar_nacimiento varchar(150)  null,
    posicion         varchar(150)  null,
    numeros_camiseta varchar(150)  null,
    altura           decimal(5, 2) null,
    sitio_web        varchar(250)  null,
    apodo            varchar(150)  null,
    nacionalidad     varchar(150)  null
);

create table pais
(
    id     int auto_increment
        primary key,
    nombre varchar(150) null
);

create table mundial
(
    id             int auto_increment
        primary key,
    id_organizador int           null,
    selecciones    int           null,
    partidos       int           null,
    goles          int           null,
    promedio_gol   decimal(5, 2) null,
    anio           int           null,
    constraint mundial_ibfk_1
        foreign key (id_organizador) references pais (id)
);

create table etapa
(
    id         int auto_increment
        primary key,
    etapa      varchar(150) null,
    id_mundial int          null,
    constraint etapa_ibfk_1
        foreign key (id_mundial) references mundial (id)
);

create table detalle_etapa
(
    id_etapa int null,
    id_pais  int null,
    constraint detalle_etapa_ibfk_1
        foreign key (id_etapa) references etapa (id),
    constraint detalle_etapa_ibfk_2
        foreign key (id_pais) references pais (id)
);

create index id_etapa
    on detalle_etapa (id_etapa);

create index id_pais
    on detalle_etapa (id_pais);

create index id_mundial
    on etapa (id_mundial);

create table goleadores
(
    id           int auto_increment
        primary key,
    id_mundial   int           null,
    id_jugador   int           null,
    goles        int           null,
    promedio_gol decimal(5, 2) null,
    partidos     int           null,
    constraint goleadores_ibfk_1
        foreign key (id_mundial) references mundial (id),
    constraint goleadores_ibfk_2
        foreign key (id_jugador) references jugadores (id)
);

create index id_jugador
    on goleadores (id_jugador);

create index id_mundial
    on goleadores (id_mundial);

create index id_organizador
    on mundial (id_organizador);

create table premios
(
    id         int auto_increment
        primary key,
    premio     varchar(150) null,
    categoria  varchar(150) null,
    id_jugador int          null,
    id_mundial int          null,
    id_pais    int          null,
    constraint premios_ibfk_1
        foreign key (id_pais) references pais (id),
    constraint premios_ibfk_2
        foreign key (id_jugador) references jugadores (id),
    constraint premios_ibfk_3
        foreign key (id_mundial) references mundial (id)
);

create index id_jugador
    on premios (id_jugador);

create index id_mundial
    on premios (id_mundial);

create index id_pais
    on premios (id_pais);

create table seleccion
(
    id         int auto_increment
        primary key,
    id_pais    int null,
    id_mundial int null,
    constraint seleccion_ibfk_1
        foreign key (id_pais) references pais (id),
    constraint seleccion_ibfk_3
        foreign key (id_mundial) references mundial (id)
);

create table partidos
(
    id          int auto_increment
        primary key,
    seleccion_a int  null,
    seleccion_b int  null,
    goles_a     int  null,
    goles_b     int  null,
    fecha       date null,
    id_etapa    int  null,
    constraint partidos_ibfk_1
        foreign key (seleccion_a) references seleccion (id),
    constraint partidos_ibfk_2
        foreign key (seleccion_b) references seleccion (id),
    constraint partidos_ibfk_3
        foreign key (id_etapa) references etapa (id)
);

create index id_etapa
    on partidos (id_etapa);

create index seleccion_a
    on partidos (seleccion_a);

create index seleccion_b
    on partidos (seleccion_b);

create table posiciones
(
    id           int auto_increment
        primary key,
    posicion     varchar(100) null,
    id_seleccion int          null,
    id_mundial   int          null,
    constraint posiciones_ibfk_1
        foreign key (id_seleccion) references seleccion (id),
    constraint posiciones_ibfk_2
        foreign key (id_mundial) references mundial (id)
);

create index id_mundial
    on posiciones (id_mundial);

create index id_seleccion
    on posiciones (id_seleccion);

create index id_mundial
    on seleccion (id_mundial);

create index id_pais
    on seleccion (id_pais);

create table seleccion_jugador
(
    id_seleccion int null,
    id_jugador   int null,
    constraint seleccion_jugador_ibfk_1
        foreign key (id_seleccion) references seleccion (id),
    constraint seleccion_jugador_ibfk_2
        foreign key (id_jugador) references jugadores (id)
);

create index id_jugador
    on seleccion_jugador (id_jugador);

create index id_seleccion
    on seleccion_jugador (id_seleccion);

create
    definer = root@`%` procedure InfoJugadores(IN jugador_filtro varchar(150), IN mundial_id int, IN info int)
BEGIN
    IF mundial_id = 0 THEN
        IF info = 1 THEN
            select * from jugadores where nombre = jugador_filtro;
        ELSEIF info = 2 THEN
            select m.anio as año, j.nombre from seleccion_jugador
            inner join seleccion s on seleccion_jugador.id_seleccion = s.id
            inner join mundial m on s.id_mundial = m.id
            inner join jugadores j on seleccion_jugador.id_jugador = j.id
            where j.nombre = jugador_filtro order by 1 desc;
        ELSEIF info = 3 THEN
            select m.anio, premio, j.nombre from premios
            inner join jugadores j on premios.id_jugador = j.id
            inner join mundial m on premios.id_mundial = m.id
            where j.nombre = jugador_filtro;
        end if;
    ELSE
        IF info = 1 THEN
            select j.nombre, e.etapa, fecha from partidos
            inner join etapa e on partidos.id_etapa = e.id
            inner join seleccion s on partidos.seleccion_a = s.id
            inner join seleccion_jugador sj on s.id = sj.id_seleccion
            inner join jugadores j on sj.id_jugador = j.id
            where j.nombre = jugador_filtro and year(fecha) = mundial_id;
            end if;
    end if;
end;

create
    definer = root@`%` procedure InfoMundial(IN anio_filtro int, IN info int)
BEGIN
    IF info = 1 THEN
        select p.nombre as 'organizador', selecciones, partidos, goles, promedio_gol, anio as 'año' from mundial
        inner join pais p on mundial.id_organizador = p.id
        where anio = anio_filtro;
    ELSEIF info = 2 THEN
        select cast(posicion as decimal) as 'posicion', p.nombre from posiciones
        inner join seleccion s on posiciones.id_seleccion = s.id
        inner join pais p on s.id_pais = p.id
        inner join mundial m on posiciones.id_mundial = m.id
        where anio = anio_filtro order by 1;
    ELSEIF info = 3 THEN
        SELECT distinct substr(e.etapa from 11) as 'grupo', p.nombre as 'pais' from detalle_etapa
        inner join etapa e on detalle_etapa.id_etapa = e.id
        inner join pais p on detalle_etapa.id_pais = p.id
        inner join mundial m on e.id_mundial = m.id
        where m.anio = anio_filtro and e.etapa like '%Grupo%';
    ELSEIF info = 4 THEN
        select * from (
        select premio, p.nombre as ganador, m.anio as 'año' from premios
        inner join pais p on premios.id_pais = p.id
        inner join mundial m on premios.id_mundial = m.id
        union
        select premio, j.nombre as ganador, m.anio as 'año' from premios
        inner join jugadores j on premios.id_jugador = j.id
        inner join mundial m on premios.id_mundial = m.id) as A
        where año = anio_filtro;
    ELSEIF info = 5 THEN
        select e.etapa, m.anio as mundial, p.nombre as local, goles_a, goles_b, pb.nombre as visitante from partidos
        inner join seleccion s on partidos.seleccion_a = s.id
        inner join seleccion sb on partidos.seleccion_b = sb.id
        inner join pais p on s.id_pais = p.id
        inner join pais pb on sb.id_pais = pb.id
        inner join etapa e on partidos.id_etapa = e.id
        inner join mundial m on e.id_mundial = m.id
        where anio = anio_filtro;
    END IF;
end;

create
    definer = root@`%` procedure InfoPais(IN pais_filtro varchar(150), IN mundial_id int, IN info int)
BEGIN
    IF mundial_id = 0 THEN
        IF info = 1 THEN
            Select p.nombre as organizador, anio as mundial from mundial
            inner join pais p on mundial.id_organizador = p.id
            where p.nombre = pais_filtro order by 2 desc;
        ELSEIF info = 2 THEN
            select p.nombre as participante, m.anio as mundial from seleccion
            inner join mundial m on seleccion.id_mundial = m.id
            inner join pais p on seleccion.id_pais = p.id
            where p.nombre = pais_filtro order by 2 desc;
        ELSEIF info = 3 THEN
            select premio, p.nombre as pais, m.anio as 'año' from premios
            inner join pais p on premios.id_pais = p.id
            inner join mundial m on premios.id_mundial = m.id
            where p.nombre = pais_filtro order by 3;
        end if;
    ELSE
        IF info = 1 THEN
            Select distinct m.anio, p.nombre, substr(e.etapa from 11) as grupo from detalle_etapa
            inner join pais p on detalle_etapa.id_pais = p.id
            inner join etapa e on detalle_etapa.id_etapa = e.id
            inner join mundial m on e.id_mundial = m.id
            where anio = mundial_id and e.etapa like '%Grupo%' and p.nombre = pais_filtro;
        ELSEIF info = 2 THEN
            select posicion from posiciones
            inner join mundial m on posiciones.id_mundial = m.id
            inner join seleccion s on posiciones.id_seleccion = s.id
            inner join pais p on s.id_pais = p.id
            where anio = mundial_id and p.nombre = pais_filtro;
        ELSEIF info = 3 THEN
            select e.etapa, m.anio as mundial, p.nombre as local, goles_a, goles_b, pb.nombre as visitante from partidos
            inner join seleccion s on partidos.seleccion_a = s.id
            inner join seleccion sb on partidos.seleccion_b = sb.id
            inner join pais p on s.id_pais = p.id
            inner join pais pb on sb.id_pais = pb.id
            inner join etapa e on partidos.id_etapa = e.id
            inner join mundial m on e.id_mundial = m.id
            where anio = mundial_id and (p.nombre = pais_filtro or pb.nombre = pais_filtro);
        end if;
    end if;
end;

