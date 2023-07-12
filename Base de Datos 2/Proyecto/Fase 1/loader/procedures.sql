DROP PROCEDURE InfoMundial;
CREATE PROCEDURE InfoMundial (anio_filtro int, info int)
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
        select e.etapa, m.anio as mundial, p.nombre as local, goles_a, goles_b, pb.nombre as visitante, fecha from partidos
        inner join seleccion s on partidos.seleccion_a = s.id
        inner join seleccion sb on partidos.seleccion_b = sb.id
        inner join pais p on s.id_pais = p.id
        inner join pais pb on sb.id_pais = pb.id
        inner join etapa e on partidos.id_etapa = e.id
        inner join mundial m on e.id_mundial = m.id
        where anio = anio_filtro;
    END IF;
end;

DROP procedure InfoPais;
CREATE PROCEDURE InfoPais (pais_filtro varchar(150), mundial_id int, info int)
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
            select e.etapa, m.anio as mundial, p.nombre as local, goles_a, goles_b, pb.nombre as visitante, fecha from partidos
            inner join seleccion s on partidos.seleccion_a = s.id
            inner join seleccion sb on partidos.seleccion_b = sb.id
            inner join pais p on s.id_pais = p.id
            inner join pais pb on sb.id_pais = pb.id
            inner join etapa e on partidos.id_etapa = e.id
            inner join mundial m on e.id_mundial = m.id
            where anio = mundial_id and (p.nombre = pais_filtro or pb.nombre = pais_filtro);
        ELSEIF info = 4 THEN
            select m.anio, p.nombre as pais, j.nombre as jugador, j.posicion, j.numeros_camiseta as dorsal from seleccion_jugador
            inner join seleccion s on seleccion_jugador.id_seleccion = s.id
            inner join pais p on s.id_pais = p.id
            inner join mundial m on s.id_mundial = m.id
            inner join jugadores j on seleccion_jugador.id_jugador = j.id
            where m.anio = mundial_id and p.nombre = pais_filtro;
        end if;
    end if;
end;


DROP procedure  InfoJugadores;
CREATE PROCEDURE InfoJugadores (jugador_filtro varchar(150), mundial_id int, info int)
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
        ELSEIF info = 2 THEN
            select distinct j.nombre, goleadores.goles, m.anio from goleadores
            inner join jugadores j on goleadores.id_jugador = j.id
            inner join mundial m on goleadores.id_mundial = m.id
            where j.nombre = jugador_filtro and anio = mundial_id;
            end if;
    end if;
end;



select m.anio, j.nombre, g.goles from goleadores g
inner join mundial m on g.id_mundial = m.id
inner join jugadores j on g.id_jugador = j.id
where m.anio = 2010;

/*
1: INFO BASICA DEL MUNDIAL
2: TABLA DE POSICIONES
3: GRUPOS
4: PREMIOS
5: PARTIDOS DISPUTADOS
*/

CALL InfoMundial(1952, 3);


/*
0:
    1: VECES QUE EL PAIS FUE SEDE
    2: VECES QUE PARTICIPO EN UN MUNDIAL
    3: PREMIOS GANADOS
Mundial:
    1: INFORMACION DEL GRUPO
    2: POSICION EN EL CAMPEONATO
    3: PARTIDOS JUGADOS EN EL MUNDIAL
    4: PLANTEL DE LA SELECCION
*/
CALL InfoPais('Brasil', 0, 2);

/*
0:
    1: INFO DEL JUGADOR
    2: AÑOS QUE PARTICIPO
    3: PREMIOS GANADOS
Mundial:
    1: PARTIDOS DONDE PARTICIPO
    2: CANTIDAD DE GOLES QUE MARCO
*/
CALL InfoJugadores('Ronaldo', 0, 2);



SELECT TABLE_NAME as nombre,(DATA_FREE/(DATA_LENGTH+INDEX_LENGTH))*100 as fragmentation,TABLE_COLLATION
FROM INFORMATION_SCHEMA.TABLES
where ENGINE='InnoDB' and TABLE_SCHEMA = 'BD2P1';



select * from jugadores