const { Router } = require("express");
var mysql = require("mysql");
const router = Router(); 

const conexion = mysql.createConnection({
    host: "localhost",
    user: "root",
    password: "31370599",
    database: "Tests"
});


router.get("/consulta9",function (req, res) {
    var query = "select proveedor, telefono, noOrden, producto, cantidad, cantidad*precio as 'Total recaudado'from compra inner join producto on compra.producto = producto.nombre inner join proveedor on proveedor.nombre = compra.proveedor order by cantidad asc, cantidad*precio asc limit 1";
    conexion.query(query, function (err, result) {
       if (err) {
           console.error(err)
       }else{
            res.send(result)
       }
    });
});

module.exports = router