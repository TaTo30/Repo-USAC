import mysql.connector
import json
import locale
import time

locale.setlocale(locale.LC_TIME, 'en_US.UTF-8')

mydb = mysql.connector.connect(
    host="localhost",
    port=10300,
    user="root",
    password="m}m0HX7M",
    database="BD2P1"
)


def LeerArchivo(path):
    with open(path, 'rb') as file:
        return json.loads(file.read())
    
def InsertarBD(query, vals):
    print(query)
    print(vals)
    mycursor = mydb.cursor()
    mycursor.executemany(query, vals)
    mydb.commit()
    print(f'{query}: {mycursor.rowcount} filas insertadas')
    
def SelectDB(query):
    mycursor = mydb.cursor()
    mycursor.execute(query)
    return mycursor.fetchall()

## CARGAMOS LOS PAISES
def CargarPaises():
    datos = LeerArchivo('paises.json')
    query_values = list(map(lambda pais: (pais['nombre'], ), datos))
    InsertarBD("INSERT INTO pais (nombre) VALUES (%s)", query_values)
    
## CARGAMOS LOS MUNDIALES
def CargarMundiales():
    mundiales = LeerArchivo('mundiales.json')
    query_values = []
    for mundial in mundiales:
        if '/' in mundial['organizador']:
            mundial['organizador'] = mundial['organizador'].split('/')[1].strip()
        dato = SelectDB(f"SELECT id FROM pais WHERE nombre = '{mundial['organizador']}'")
        if len(dato) > 0:
            query_values.append((dato[0][0], mundial['selecciones'], mundial['partidos'], mundial['goles'], mundial['promedio_gol'], mundial['año']))
    InsertarBD("INSERT INTO mundial (id_organizador, selecciones, partidos, goles, promedio_gol, anio) VALUES (%s, %s, %s, %s, %s, %s)", query_values)
        
## CARGAMOS LOS JUGADORES
def CargarJugadores():
    locale.setlocale(locale.LC_TIME, 'es_ES.UTF-8')

    jugadores = LeerArchivo('jugadores.json')
    query_values = []
    for jugador in jugadores:
        try:
            fecha_nac = time.strftime("%Y-%m-%d", time.strptime(jugador['Fecha de Nacimiento'], "%d de %B de %Y")) 
            query_values.append((
                jugador['Nombre completo'], 
                fecha_nac, 
                jugador['Lugar de nacimiento'], 
                jugador['Posición'], 
                str(jugador['Números de camiseta']),
                jugador['Altura'] or None,
                jugador['Twitter'],
                jugador['Apodo'],
                jugador['Nacionalidad']
            ))
        except:
            continue
    InsertarBD(
        "INSERT INTO jugadores (nombre, fecha_nacimiento, lugar_nacimiento, posicion, numeros_camiseta, altura, sitio_web, apodo, nacionalidad) VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s)",
        query_values
    )

## CARGAMOS SELECCIONES
def CargarSelecciones():
    selecciones = LeerArchivo('selecciones.json')
    paises_selecciones = []
    for seleccion in selecciones:
        if (seleccion['pais'], seleccion['mundial']) not in paises_selecciones:
            paises_selecciones.append((seleccion['pais'], seleccion['mundial']))
    query_values = []
    for pais_seleccion in paises_selecciones:
        try:
            id_pais = SelectDB(f"select id from pais where nombre = '{pais_seleccion[0]}'")
            mundial_id = SelectDB(f"select id from mundial where anio = {pais_seleccion[1]}")
            query_values.append((id_pais[0][0], mundial_id[0][0]))
        except:
            continue
    InsertarBD("INSERT INTO seleccion (id_pais, id_mundial) VALUES (%s, %s)", query_values)

## CARGAMOS DETALLE SELECCIONES
def CargarDetalleSelecciones():
    seleccionados = LeerArchivo('selecciones.json')
    query_values = []
    for seleccionado in seleccionados:
        try:
            id_seleccion = SelectDB(f"SELECT id FROM seleccion where id_mundial = (SELECT id from mundial where anio = {seleccionado['mundial']}) and id_pais = (SELECT id from pais where nombre = '{seleccionado['pais']}')")
            id_jugador = SelectDB(f"SELECT id from jugadores where nombre = '{seleccionado['jugador']}'")
            query_values.append((id_seleccion[0][0], id_jugador[0][0]))
        except:
            continue
    InsertarBD("INSERT INTO seleccion_jugador (id_seleccion, id_jugador) VALUES (%s, %s)", query_values)

## CARGAMOS POSICIONES
def CargarPosiciones():
    posiciones = LeerArchivo('posiciones.json')
    query_values = []
    for posicion in posiciones:
        try:
            id_seleccion = SelectDB(f"SELECT id FROM seleccion where id_mundial = (SELECT id from mundial where anio = {posicion['mundial']}) and id_pais = (SELECT id from pais where nombre = '{posicion['seleccion']}')")
            mundial_id = SelectDB(f"select id from mundial where anio = {posicion['mundial']}")
            query_values.append((posicion['posicion'], id_seleccion[0][0], mundial_id[0][0]))
        except:
            continue
    InsertarBD(
        "INSERT INTO posiciones (posicion, id_seleccion, id_mundial) VALUES (%s, %s, %s)",
        query_values
    )

## CARGAMOS GOLEADORES
def CargarGoleadores():
    goleadores = LeerArchivo('goleadores.json')
    query_values = []
    for goleador in goleadores:
        try:
            mundial_id = SelectDB(f"select id from mundial where anio = {goleador['Mundial']}")
            id_jugador = SelectDB(f"SELECT id from jugadores where nombre = '{goleador['Jugador']}'")
            query_values.append((mundial_id[0][0], id_jugador[0][0], goleador['Goles'], goleador['Promedio'], goleador['Partidos']))
        except:
            continue
    InsertarBD("INSERT INTO goleadores (id_mundial, id_jugador, goles, promedio_gol, partidos) VALUES (%s, %s, %s, %s, %s)", query_values)

## CARGAMOS ETAPAS
def CargarEtapas():
    etapas = LeerArchivo('grupo.json')
    query_values = []
    filtro_etapa = []
    for etapa in etapas:
        if (etapa['grupo/etapa'], etapa['mundial']) not in filtro_etapa:
            filtro_etapa.append((etapa['grupo/etapa'], etapa['mundial']))

    etapas = filtro_etapa
    for etapa in etapas:
        try:
            mundial_id = SelectDB(f"select id from mundial where anio = {etapa[1]}")
            query_values.append((etapa[0], mundial_id[0][0]))
        except:
            continue
    InsertarBD("INSERT INTO etapa (etapa, id_mundial) VALUES (%s, %s)", query_values)

## CARGAMOS DETALLES ETAPAS
def CargarDetalleEtapas():
    detalles = LeerArchivo('detalle.json')
    query_values = []
    for detalle in detalles:
        try:
            id_pais = SelectDB(f"select id from pais where nombre = '{detalle['pais']}'")
            id_etapa = SelectDB(f"select * from etapa where id_mundial = (select id from mundial where anio = {detalle['mundial']}) and etapa = '{detalle['grupo/etapa']}'")
            query_values.append((id_etapa[0][0], id_pais[0][0]))
        except:
            continue
    InsertarBD("INSERT INTO detalle_etapa (id_etapa, id_pais) VALUES (%s, %s)", query_values)
    
## CARGAMOS PREMIOS
def CargarPremios():
    premios = LeerArchivo('premios.json')
    query_values = []
    for premio in premios:
        try:
            id_jugador = None
            id_pais = None
            mundial_id = SelectDB(f"select id from mundial where anio = {premio['mundial']}")[0][0]
            if premio['jugador']:
                id_jugador = SelectDB(f"SELECT id from jugadores where  nombre = '{premio['jugador']}'")[0][0]
            elif premio['pais']:
                id_pais = SelectDB(f"select id from pais where nombre = '{premio['pais']}'")[0][0]
        
            query_values.append((premio['premio'], '', id_jugador, mundial_id, id_pais))
        except:
            continue
    InsertarBD("INSERT INTO premios (premio, categoria, id_jugador, id_mundial, id_pais) VALUES (%s, %s, %s, %s, %s)", query_values)

def CargarPartidos():
    locale.setlocale(locale.LC_TIME, 'en_US.UTF-8')
    
    partidos = LeerArchivo('partidos.json')
    query_values = []
    for partido in partidos:
        try:
            fecha = time.strftime("%Y-%m-%d", time.strptime(partido['fecha'], "%d-%b-%Y")) 
            seleccion_a = SelectDB(f"SELECT id FROM seleccion where id_mundial = (SELECT id from mundial where anio = {partido['mundial']}) and id_pais = (SELECT id from pais where nombre = '{partido['sleccion_a']}')")[0][0]
            seleccion_b = SelectDB(f"SELECT id FROM seleccion where id_mundial = (SELECT id from mundial where anio = {partido['mundial']}) and id_pais = (SELECT id from pais where nombre = '{partido['seleccion_b']}')")[0][0]
            etapa_str = partido['grupo']
            if 'Grupo' in etapa_str:
                etapa_str = etapa_str[0:7]
            etapa_id = SelectDB(f"select etapa.id from etapa inner join mundial m on etapa.id_mundial = m.id where m.anio = {partido['mundial']} and etapa like '%{etapa_str}%'")[0][0]           
            print(etapa_str)
            print(etapa_id)
            query_values.append((seleccion_a, seleccion_b, partido['goles_a'], partido['goles_b'], fecha, etapa_id))
        except:
            continue
    InsertarBD("INSERT INTO partidos (seleccion_a, seleccion_b, goles_a, goles_b, fecha, id_etapa) VALUES (%s, %s, %s, %s, %s, %s)", query_values)

#CargarPaises()
#CargarMundiales()
#CargarJugadores()
#CargarSelecciones()
#CargarDetalleSelecciones()
#CargarPosiciones()
#CargarGoleadores()
#CargarEtapas()
#CargarDetalleEtapas()
#CargarPremios()
#CargarPartidos()













    