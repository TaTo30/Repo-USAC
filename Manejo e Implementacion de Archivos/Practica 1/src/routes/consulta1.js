const { Router } = require("express");
var mysql = require("mysql");
const router = Router(); 

const conexion = mysql.createConnection({
    host: "localhost",
    user: "root",
    password: "31370599",
    database: "Tests"
});


router.get("/consulta1",function (req, res) {
    var query = "select proveedor, telefono, noOrden, cantidad*precio as 'Precio pagado' from compra inner join producto on compra.producto = producto.nombre inner join proveedor on proveedor.nombre = compra.proveedor order by cantidad*precio desc, proveedor limit 1";
    conexion.query(query, function (err, result) {
       if (err) {
           console.error(err)
       }else{
            res.send(result)
       }
    });
});

module.exports = router