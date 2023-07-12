const { Router } = require("express");
var mysql = require("mysql");
const router = Router(); 

const conexion = mysql.createConnection({
    host: "localhost",
    user: "root",
    password: "31370599",
    database: "Tests"
});


router.get("/consulta7",function (req, res) {
    var query = "select proveedor, sum(cantidad*precio) as 'Montor recaudado' from compra inner join producto on compra.producto = producto.nombre where categoria = 'Fresh vegetables'  group by proveedor order by sum(cantidad*precio) desc limit 5";
    conexion.query(query, function (err, result) {
       if (err) {
           console.error(err)
       }else{
            res.send(result)
       }
    });
});

module.exports = router