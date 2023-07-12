const { Router } = require("express");
var mysql = require("mysql");
const router = Router(); 

const conexion = mysql.createConnection({
    host: "localhost",
    user: "root",
    password: "31370599",
    database: "Tests"
});


router.get("/consulta3",function (req, res) {
    var query = "(select proveedor, direccion, region, ciudad, codigoPostal, count(proveedor) as 'Pedidos hechos' from compra inner join proveedor on compra.proveedor = proveedor.nombre group by proveedor order by count(proveedor) desc limit 1) union (select proveedor, direccion, region, ciudad, codigoPostal, count(proveedor) as 'Pedidos hechos' from compra inner join proveedor on compra.proveedor = proveedor.nombre group by proveedor order by count(proveedor) asc limit 1)";
    conexion.query(query, function (err, result) {
       if (err) {
           console.error(err)
       }else{
            res.send(result)
       }
    });
});

module.exports = router