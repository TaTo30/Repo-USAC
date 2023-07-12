const { Router, query } = require("express");
const router = Router();
var mysql = require("mysql");


const conexion = mysql.createConnection({
    host: "localhost",
    password: "31370599",
    database: "Tests",
    user: "root"
});

router.get("/eliminarTemporal", function (req, res) {
    var query = "TRUNCATE TABLE temporal";
    conexion.query(query, function (err, result) {
        if (err) {
            res.send(`Ha ocurrido un error! ${err}`);
        } else {
            res.send("Tabla Temporal eliminada");
        }
    });
});

module.exports = router