import mysql.connector
from pandas import period_range
import pymongo

def SelectDB(query):
    print(query)
    mydb = mysql.connector.connect(
        host="localhost",
        port=10300,
        user="root",
        password="m}m0HX7M",
        database="BD2P1"
    )
    mycursor = mydb.cursor()
    mycursor.execute(query)
    return mycursor.fetchall()

def InsertMongo(col, values):
    myclient = pymongo.MongoClient("mongodb+srv://p1fase2:p1fase2@tato30.88p2b.mongodb.net/BD2Fase2?retryWrites=true&w=majority")
    mydb = myclient['BD2Fase2']
    mycol = mydb[col]
    mycol.insert_many(values)

def DocumentoMundial(year):
    # Recuperamos Info basica del mundial
    info_basica_query = SelectDB(f'CALL InfoMundial({year}, 1)')
    mundial = {
        'organizador': info_basica_query[0][0],
        'promedio_gol': float(info_basica_query[0][4]),
        'goles': info_basica_query[0][3],
        'año': info_basica_query[0][5]
    }
    
    # Recuperamos Info de las selecciones de ese mundial
    selecciones = []
    info_seleccion_grupo = SelectDB(f'CALL InfoMundial({year}, 3)')
    for sle_grp in info_seleccion_grupo:
        posicion = SelectDB(f"CALL InfoPais('{sle_grp[1]}', {year}, 2)")[0][0]
        jugadores = list(map(lambda jug: {
                'nombre': jug[2],
                'posicion': jug[3],
                'dorsal': jug[4]
            }, SelectDB(f"CALL InfoPais('{sle_grp[1]}', {year}, 4)")))
        selecciones.append({
            'pais': sle_grp[1],
            'grupo': sle_grp[0],
            'posicion': int(posicion),
            'jugadores': jugadores
        })
    mundial.update({
        'selecciones': selecciones
    })
    
    # Recuperamos Info de los partidos
    partidos = list(map(lambda partido: {
            "etapa": partido[0],
            "local": partido[2],
            "visitante": partido[5],
            "goles_local": partido[3],
            "goles_visitante": partido[4],
            "fecha": str(partido[6])
        }, SelectDB(f"CALL InfoMundial({year}, 5)")))
    mundial.update({
        'partidos': partidos
    })
    
    # Recuperamos Info de los premios
    premios = list(map(lambda premio: {
        'nombre': premio[0],
        'ganador': premio[1]
    }, SelectDB(f"CALL InfoMundial({year}, 4)")))
    mundial.update({
        'premios': premios
    })
    
    # Recuperamos Info de los goleadores
    goleadores = list(map(lambda goleador: {
        'jugador': goleador[1],
        'goles': goleador[2]
    }, SelectDB(f"select m.anio, j.nombre, g.goles from goleadores g inner join mundial m on g.id_mundial = m.id inner join jugadores j on g.id_jugador = j.id where m.anio = {year}")))
    mundial.update({
        'goleadores': goleadores
    })
    
    return mundial

def ColeccionMundial():
    mundiales = []
    mundiales_anios = SelectDB("select anio from mundial")
    for anio in mundiales_anios:
        mundial = DocumentoMundial(anio[0])
        mundiales.append(mundial)
    InsertMongo('mundiales2', mundiales)
            
def DocumentoJugador(player):
    # Recuperamos Info basica del jugador
    info_jugador = SelectDB(f"CALL InfoJugadores(\"{player}\", 0, 1)")[0]
    jugador = {
        "nombre": info_jugador[1],
        "fecha_nacimiento": str(info_jugador[2]),
        "lugar_nacimiento": info_jugador[3],
        "posicion": info_jugador[4],
        "dorsales": info_jugador[5],
        "altura": float(info_jugador[6] or 0),
        "sitio_web": info_jugador[7],
        "apodo": info_jugador[8],
        "nacionalidad": info_jugador[9],
    }
    
    #Recuperamos info de los premios
    premios = list(map(lambda premio: {
        "mundial": premio[0],
        "nombre": premio[1]
    }, SelectDB(f"CALL InfoJugadores(\"{player}\", 0, 3)")))
    jugador.update({
        'premios': premios
    })
    
    # Recuperamos sus participaciones
    participaciones = []
    mundiales_participados = SelectDB(f"CALL InfoJugadores(\"{player}\", 0, 2)")
    for mundial_participado in mundiales_participados:
        mundial = mundial_participado[0]
        goles = SelectDB(f"CALL InfoJugadores(\"{player}\", {mundial}, 2)")
        if len(goles) > 0:
            goles = goles[0][1]
        else:
            goles = 0
        partidos = list(map(lambda partido: {
            "etapa": partido[1],
            "fecha": str(partido[2])
        }, SelectDB(f"CALL InfoJugadores(\"{player}\", {mundial}, 1)")))
        participaciones.append({
            "mundial": mundial,
            "goles": goles,
            "partidos": partidos
        })
    jugador.update({
        'participaciones': participaciones
    })
    
    return jugador

def ColeccionJugadores():
    jugadores = []
    jugadores_nombres = SelectDB("select nombre from jugadores")
    for nombre in jugadores_nombres:
        jugador = DocumentoJugador(nombre[0])
        jugadores.append(jugador)
    InsertMongo('jugadores', jugadores)
    
    
def DocumentoPaises(country):
    # Recuperamos info de las veces que ha sido sede
    veces_sede = list(map(lambda pais: pais[1], SelectDB(f"CALL InfoPais('{country}', 0, 1 )")))
    pais = {
        "nombre": country,
        "sede": veces_sede
    }
    
    # Recuperamos las participaciones
    participaciones = []
    mundiales_participados = SelectDB(f"CALL InfoPais('{country}', 0, 2 )")
    for mundial_participado in mundiales_participados:
        try:
            mundial = mundial_participado[1]
            grupo = SelectDB(f"CALL InfoPais('{country}', {mundial}, 1 )")[0][2]
            posicion = SelectDB(f"CALL InfoPais('{country}', {mundial}, 2)")[0][0]

            partidos = list(map(lambda partido: {
                "etapa": partido[0],
                "local": partido[2],
                "visitante": partido[5],
                "local_goles": partido[3],
                "visitante_goles": partido[4],
                "fecha": str(partido[6])
            }, SelectDB(f"CALL InfoPais('{country}', {mundial}, 3)")))
            
            jugadores = list(map(lambda jugador: {
                'nombre': jugador[2],
                'posicion': jugador[3],
                'dorsal': jugador[4]
            }, SelectDB(f"CALL InfoPais('{country}', {mundial}, 4)")))
            
            participaciones.append({
                "mundial": mundial,
                "grupo": grupo,
                "posicion": posicion,
                "partidos": partidos,
                "jugadores": jugadores
            })
        except:
            continue
    pais.update({
        'participaciones': participaciones
    })
    
    # Recuperamos los premios ganados
    premios = list(map(lambda premio: {
        "mundial": premio[2],
        "nombre": premio[0]
    }, SelectDB(f"CALL InfoPais('{country}', 0, 3)")))
    pais.update({
        'premios': premios
    })
    
    return pais
    
    
def ColeccionPais():
    paises = []
    paises_nombres = SelectDB("select nombre from pais")
    for nombre in paises_nombres:
        pais = DocumentoPaises(nombre[0])
        paises.append(pais)
    InsertMongo('paises', paises)


ColeccionMundial()
#ColeccionJugadores()
#ColeccionPais()


    