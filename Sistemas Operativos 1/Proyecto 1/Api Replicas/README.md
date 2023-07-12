# Api Replicas Settings

## Endpoints

```null
POST: /<LANG>/add_tweet

GET: /<LANG>/echo
```

## Base de datos Cosmos DB

```json
{
    "connection": " mongodb://sopes1-g24-2021:kxeCcSywgmVVNUgN2vuDMPKwULZ01ZryPyJQm3R8SjfJeG2WB3pBd7BmwI8pA3nnd28No0gJIUOBLnK5JoNWdw==@sopes1-g24-2021.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@sopes1-g24-2021@",
    "database": "sopes1",
    "collection": "proyecto1"
}
```

## Base de datos Cloud SQL

```json
{
    "connection": "mysql://root:1234@35.184.7.29/Proyecto1",
    "database": "Proyecto1"
}
```

...
