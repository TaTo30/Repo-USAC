const { Router } = require("express");
var mysql = require("mysql");
const router = Router(); 

const conexion = mysql.createConnection({
    host: "localhost",
    user: "root",
    password: "31370599",
    database: "Tests"
});


router.get("/consulta5",function (req, res) {
    var query = "(select fecha, cliente, sum(cantidad*precio) as 'Total gastado' from venta inner join cliente on venta.cliente = cliente.nombre inner join producto on venta.producto = producto.nombre group by cliente order by sum(cantidad*precio) desc limit 3) "+
    "union "+
    "(select fecha, cliente, sum(cantidad*precio) as 'Total gastado' from venta inner join cliente on venta.cliente = cliente.nombre inner join producto on venta.producto = producto.nombre group by cliente order by sum(cantidad*precio) asc limit 3)";
    conexion.query(query, function (err, result) {
       if (err) {
           console.error(err)
       }else{
            res.send(result)
       }
    });
});

module.exports = router