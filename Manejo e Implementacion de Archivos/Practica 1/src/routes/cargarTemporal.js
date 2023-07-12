const { Router, query } = require("express");
const router = Router();
var mysql = require("mysql");


const conexion = mysql.createConnection({
    host: "localhost",
    password: "31370599",
    database: "Tests",
    user: "root"
});

router.get("/cargarTemporal", function (req, res) {
    var query = 
    "LOAD DATA LOCAL INFILE '/home/aldoh/Desarrollo/Manejo e Implementacion de Archivos/DATA.csv' "+
    "INTO TABLE temporal "+
    "fields terminated by ';' "+
    "lines terminated by '\n' "+
    "ignore 1 rows";
    conexion.query(query, function (err, result) {
        if (err) {
            res.send(`Ha ocurrido un error! ${err}`);
        } else {
            res.send("Se han cargado los datos a la tabla!");
        }
    });
});

module.exports = router