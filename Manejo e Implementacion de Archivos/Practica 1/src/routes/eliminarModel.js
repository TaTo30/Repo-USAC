const { Router, query } = require("express");
const router = Router();
var mysql = require("mysql");


const conexion = mysql.createConnection({
    host: "localhost",
    password: "31370599",
    database: "Tests",
    user: "root"
});

router.get("/eliminarModelo",function (req, res) {
   var query0 = "truncate table compania";
   var query1 = "truncate table cliente";
   var query2 = "truncate table proveedor";
   var query3 = "truncate table compra";
   var query4 = "truncate table venta"; 
   var query5 = "truncate table producto"; 
   conexion.query(query0);
   conexion.query(query1);
   conexion.query(query2);
   conexion.query(query3);
   conexion.query(query4);
   conexion.query(query5);
   res.send("Se ha eliminado el modelo correctamiente");
});

module.exports = router;