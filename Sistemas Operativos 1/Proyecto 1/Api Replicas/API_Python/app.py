from os import error
import MySQLdb
from flask import Flask, jsonify, request
from pymongo import MongoClient
from flask_mysqldb import MySQL
from datetime import datetime
import json
import requests

#Cosmos DB
cosmos_user_name = "cosmos-calificacion"
cosmos_password = "3IlHj1WMnBFTvlc0MeAG9rWLMwNu14bkevGnwIS4VcxEmKHA3o3czPMhcXbnXGCOYyNLAyjVyGczZ1esgc0Vmg=="
cosmos_url = "cosmos-calificacion.mongo.cosmos.azure.com"
cosmos_database_name = "sopes1"
cosmos_collection_name = "proyecto1"
cosmos_port = 10255
cosmos_args = "ssl=true&retrywrites=false&ssl_cert_reqs=CERT_NONE"

uri = f'mongodb://{cosmos_user_name}:{cosmos_password}@{cosmos_url}:{cosmos_port}/{cosmos_database_name}?{cosmos_args}'
mongo_client = MongoClient(uri)
my_db = mongo_client[cosmos_database_name]

#
app = Flask(__name__)
app.config['MYSQL_HOST'] = '35.184.7.29'
app.config['MYSQL_USER'] = 'root'
app.config['MYSQL_PASSWORD'] = '1234'
app.config['MYSQL_DB'] = 'Proyecto1'
mysql = MySQL(app)

@app.route('/iniciarCarga', methods=['GET'])
def iniciarCarga(): 
    try:
        resp = requests.get('http://35.184.136.235:4444/iniciarCarga')
        return resp.text
    except ConnectionError :
        print('Error De Conexion')
        return jsonify({"Error": "Error"})        

@app.route('/finalizarCarga', methods=['GET'])
def finalizarCarga(): 
    try:
        resp = requests.get('http://35.184.136.235:4444/finalizarCarga')
        return resp.text
    except Exception as e :
        print(e)
        return jsonify({"Error": "Error"})  

#@app.route('/addM', methods=['POST'])
#def addCosmos():
#    try:
#        my_col = my_db[cosmos_collection_name]
#        insert = my_col.insert_one(request.json)
#        #my_docs = my_col.find({})
#        #for doc in my_docs:
#        #    print(doc)
#        return jsonify({"mensaje": "Ingresado"})
#    except Exception as e :
#        print(e)
#        return jsonify({"Error": "Error"}) 
    

@app.route('/getCosmos', methods=['GET'])
def getCosmos():
    try:
        my_col = my_db[cosmos_collection_name]
        my_docs = my_col.find({})
        for doc in my_docs:
            print(doc)
        return jsonify({"mensaje": "recibido"})
    except Exception as e :
        print(e)
        return jsonify({"Error": "Error"})

@app.route('/publicar', methods=['POST'])
def publicar(): 

    fecha = datetime.strptime(request.json['fecha'], '%d/%m/%Y')
    newDate = datetime.strftime(fecha, '%Y-%m-%d')

    cursor = mysql.connection.cursor()

    try:
        cursor.execute('INSERT INTO twit (nombre, comentario, fecha, upvotes, downvotes, api) VALUES (%s, %s, %s, %s, %s, \'python\')',
            (request.json['nombre'], request.json['comentario'], newDate, int(request.json['upvotes']), int(request.json['downvotes'])))
        mysql.connection.commit()
    except MySQLdb.Error as e:
        print(e)
    try:
        cursor.execute('SELECT MAX(id) FROM twit')
        idTwit = cursor.fetchall()
    except MySQLdb.Error as e:
        print(e)

    for i in range(0, len(request.json['hashtags'])):
        try:
            hashtag = request.json['hashtags'][i]
            cursor.execute('INSERT INTO hashtags (hashtag) VALUES (%s)',
                (hashtag,))
            mysql.connection.commit()

            try:
                cursor.execute('SELECT MAX(id) FROM hashtags')
                idHash = cursor.fetchall()
            except MySQLdb.Error as e:
                print(e)

            try:
                cursor.execute('INSERT INTO hash_twit (idTwit, idHash) VALUES (%s, %s)',
                    (idTwit[0][0], idHash[0][0]))
                mysql.connection.commit()
            except MySQLdb.Error as e:
                print(e)

        except MySQLdb.Error as e:
            try:
                cursor.execute('SELECT id FROM hashtags WHERE hashtag = %s', (hashtag,))
                idHash = cursor.fetchall()

                try:
                    cursor.execute('INSERT INTO hash_twit (idTwit, idHash) VALUES (%s, %s)',
                        (idTwit[0][0], idHash[0][0]))
                    mysql.connection.commit()
                except MySQLdb.Error as e:
                    print(e)

            except MySQLdb.Error as e:
                print(e)

    try:
        my_col = my_db[cosmos_collection_name]
        insert = my_col.insert_one(request.json)
        #my_docs = my_col.find({})
        #for doc in my_docs:
        #    print(doc)
    except Exception as e :
        print(e)

    
    try:
        p = {"Api": "python"}
        requests.post('http://35.184.136.235:4444/publicar', data=json.dumps(p))
    except Exception as e:
        print(e) 

    return jsonify({"message": "ingresado"})

if __name__ == '__main__':
    app.run(debug=True, port=4500,host="0.0.0.0")
