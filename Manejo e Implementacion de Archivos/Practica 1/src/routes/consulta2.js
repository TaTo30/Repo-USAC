const { Router } = require("express");
var mysql = require("mysql");
const router = Router(); 

const conexion = mysql.createConnection({
    host: "localhost",
    user: "root",
    password: "31370599",
    database: "Tests"
});


router.get("/consulta2",function (req, res) {
    var query = "select noCliente, cliente, sum(cantidad) as 'Productos comprados' from venta inner join cliente on venta.cliente = cliente.nombre group by cliente order by sum(cantidad) desc limit 1";
    conexion.query(query, function (err, result) {
       if (err) {
           console.error(err)
       }else{
            res.send(result)
       }
    });
});

module.exports = router