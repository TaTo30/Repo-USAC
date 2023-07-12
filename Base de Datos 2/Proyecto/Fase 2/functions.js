/*
1: INFO BASICA DEL MUNDIAL
2: TABLA DE POSICIONES
3: GRUPOS
4: PREMIOS
5: PARTIDOS DISPUTADOS
*/
var f1 = function (year, spec) {
  if (year != 0) {
    if (spec == 1) {
      return db.mundiales.aggregate([
        { $match: { año: year } },
        {
          $project: {
            _id: 0,
            organizador: 1,
            año: 1,
            goles: 1,
            promedio_gol: 1,
            selecciones: { $size: "$selecciones" },
            partidos: { $size: "$partidos" },
          },
        },
      ]);
    } else if (spec == 2) {
      return db.mundiales.aggregate([
        { $match: { año: year } },
        { $unwind: "$selecciones" },
        {
          $project: {
            _id: 0,
            _pais: "$selecciones.pais",
            _posicion: "$selecciones.posicion",
          },
        },
        { $sort: { _posicion: 1 } },
      ]);
    } else if (spec == 3) {
      return db.mundiales.aggregate([
        { $match: { año: year } },
        { $unwind: "$selecciones" },
        {
          $project: {
            _id: 0,
            _pais: "$selecciones.pais",
            _grupo: "$selecciones.grupo",
          },
        },
        { $sort: { _grupo: 1 } },
      ]);
    } else if (spec == 4) {
      return db.mundiales.aggregate([
        { $match: { año: year } },
        { $unwind: "$premios" },
        {
          $project: {
            _id: 0,
            _año: "$año",
            _nombre: "$premios.nombre",
            _ganador: "$premios.ganador",
          },
        },
      ]);
    } else {
      return db.mundiales.aggregate([
        { $match: { año: year } },
        { $unwind: "$partidos" },
        {
          $project: {
            _id: 0,
            _año: "$año",
            _etapa: "$partidos.etapa",
            _local: "$partidos.local",
            _visitante: "$partidos.visitante",
            _goles_local: "$partidos.goles_local",
            _goles_visitante: "$partidos.goles_visitante",
            _fecha: "$partidos.fecha",
          },
        },
      ]);
    }
  } else {
    return db.mundiales.aggregate([
      {
        $project: {
          _id: 0,
          organizador: 1,
          año: 1,
          goles: 1,
          promedio_gol: 1,
          selecciones: { $size: "$selecciones" },
          partidos: { $size: "$partidos" },
        },
      },
      { $sort: { año: -1 } },
    ]);
  }
};

/*0:  sin especificar mundial
    1: VECES QUE EL PAIS FUE SEDE
    2: VECES QUE PARTICIPO EN UN MUNDIAL
    3: PREMIOS GANADOS
    4: VECES QUE FUE CAMPEON
Mundial:
    1: INFORMACION DEL GRUPO
    2: POSICION EN EL CAMPEONATO
    3: PARTIDOS JUGADOS EN EL MUNDIAL
*/
var f2 = function (year, contry, spec) {
  if (year == 0) {
    if (spec == 1) {
      return db.paises.aggregate([
        { $match: { nombre: contry } },
        { $unwind: "$sede" },
        {
          $project: {
            _id: 0,
            _pais: "$nombre",
            _año: "$sede",
          },
        },
      ]);
    } else if (spec == 2) {
      return db.paises.aggregate([
        { $match: { nombre: contry } },
        { $unwind: "$participaciones" },
        {
          $project: {
            _id: 0,
            _pais: "$nombre",
            _año: "$participaciones.mundial",
          },
        },
      ]);
    } else if (spec == 3) {
      return db.paises.aggregate([
        { $match: { nombre: contry } },
        { $unwind: "$premios" },
        {
          $project: {
            _id: 0,
            _pais: "$nombre",
            _año: "$premios.mundial",
            _premio: "$premios.nombre",
          },
        },
      ]);
    } else {
      return db.mundiales.aggregate([
        { $unwind: "$selecciones" },
        {
          $project: {
            _id: 0,
            _pais: "$selecciones.pais",
            _posicion: "$selecciones.posicion",
            _año: "$año",
          },
        },
        { $sort: { _posicion: 1 } },
        { $limit: 22 },
        { $match: { _pais: contry } },
      ]);
    }
  } else {
    if (spec == 1) {
      return db.paises.aggregate([
        { $unwind: "$participaciones" },
        {
          $project: {
            _id: 0,
            _pais: "$nombre",
            _año: "$participaciones.mundial",
            _grupo: "$participaciones.grupo",
          },
        },
        { $match: { _año: year, _pais: contry } },
      ]);
    } else if (spec == 2) {
      return db.mundiales.aggregate([
        { $unwind: "$selecciones" },
        {
          $project: {
            _id: 0,
            _pais: "$selecciones.pais",
            _posicion: "$selecciones.posicion",
            _año: "$año",
          },
        },
        { $match: { _pais: contry, _año: year } },
      ]);
    } else {
      return db.mundiales.aggregate([
        { $unwind: "$partidos" },
        {
          $project: {
            _id: 0,
            _año: "$año",
            _etapa: "$partidos.etapa",
            _local: "$partidos.local",
            _visitante: "$partidos.visitante",
            _goles_local: "$partidos.goles_local",
            _goles_visitante: "$partidos.goles_visitante",
            _fecha: "$partidos.fecha",
          },
        },
        {
          $match: {
            _año: year,
            $or: [{ _local: contry }, { _visitante: contry }],
          },
        },
      ]);
    }
  }
};

/*
0:  sin especificar mundial
    1: INFO DEL JUGADOR
    2: AÑOS QUE PARTICIPO
    3: PREMIOS GANADOS
Mundial:
    1: PARTIDOS DONDE PARTICIPO
*/
var f3 = function (player, year, spec) {
  if (year == 0) {
    if (spec == 1) {
      return db.jugadores.aggregate([
        { $match: { nombre: player } },
        {
          $project: {
            _id: 0,
            _nombre: "$nombre",
            _fecha_nacimiento: "$fecha_nacimiento",
            _lugar_nacimiento: "$lugar_nacimiento",
            _posicion: "posicion",
            _dorsales: "$dorsales",
            _altura: "$altura",
            _sitio_web: "$sitio_web",
            _apodo: "$apodo",
            _nacionalidad: "$nacionalidad",
          },
        },
      ]);
    } else if (spec == 2) {
      return db.jugadores.aggregate([
        { $match: { nombre: player } },
        { $unwind: "$participaciones" },
        {
          $project: {
            _id: 0,
            _nombre: "$nombre",
            _mundial: "$participaciones.mundial",
            _goles: "$participaciones.goles",
          },
        },
      ]);
    } else {
      return db.jugadores.aggregate([
        { $match: { nombre: player } },
        { $unwind: "$premios" },
        {
          $project: {
            _id: 0,
            _nombre: "$nombre",
            _mundial: "$premios.mundial",
            _premio: "$premios.nombre",
          },
        },
      ]);
    }
  } else {
    return db.jugadores.aggregate([
      { $unwind: "$participaciones" },
      { $unwind: "$participaciones.partidos" },
      {
        $project: {
          _id: 0,
          _nombre: "$nombre",
          _mundial: "$participaciones.mundial",
          _etapa: "$participaciones.partidos.etapa",
          _fecha: "$participaciones.partidos.fecha",
        },
      },
      { $match: { _nombre: player, _mundial: year } },
    ]);
  }
};
