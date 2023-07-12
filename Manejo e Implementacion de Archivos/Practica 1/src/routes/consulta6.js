const { Router } = require("express");
var mysql = require("mysql");
const router = Router(); 

const conexion = mysql.createConnection({
    host: "localhost",
    user: "root",
    password: "31370599",
    database: "Tests"
});


router.get("/consulta6",function (req, res) {
    var query = "(select categoria, sum(cantidad) as 'Articulos vendidos', sum(cantidad*precio) as 'Monto recaudado' from venta inner join producto on venta.producto = producto.nombre group by categoria order by sum(cantidad) desc, sum(cantidad*precio) desc limit 1) union (select categoria, sum(cantidad) as 'Articulos vendidos', sum(cantidad*precio) as 'Monto recaudado' from venta inner join producto on venta.producto = producto.nombre group by categoria order by sum(cantidad) asc, sum(cantidad*precio) asc limit 1)";
    conexion.query(query, function (err, result) {
       if (err) {
           console.error(err)
       }else{
            res.send(result)
       }
    });
});

module.exports = router