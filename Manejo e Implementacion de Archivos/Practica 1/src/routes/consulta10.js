const { Router } = require("express");
var mysql = require("mysql");
const router = Router(); 

const conexion = mysql.createConnection({
    host: "localhost",
    user: "root",
    password: "31370599",
    database: "Tests"
});


router.get("/consulta10",function (req, res) {
    var query = "select cliente, sum(cantidad) as 'Articulos adquiridos' from venta inner join producto on venta.producto = producto.nombre where producto.categoria = 'Seafood' group by cliente order by sum(cantidad) desc limit 10";
    conexion.query(query, function (err, result) {
       if (err) {
           console.error(err)
       }else{
            res.send(result)
       }
    });
});

module.exports = router