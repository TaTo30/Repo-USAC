const { Router, query } = require("express");
const router = Router();
var mysql = require("mysql");


const conexion = mysql.createConnection({
    host: "localhost",
    password: "31370599",
    database: "Tests",
    user: "root"
});

router.get("/cargarModelo",function (req, res) {
    //=======CARGAR DATOS COMPANIA========//
    var query = "insert into compania (nombre, contacto, correo, telefono) "+
    "select nombre_compania, contacto_compania, correo_compania, telefono_compania from temporal group by nombre_compania order by nombre_compania";
    conexion.query(query, function (err, result) {
        if (err) {
            res.send(`Ha ocurrido un error cargando companias! ${err}`);
        } else {
            //=========CARGAR DATOS CLIENTES==========//
            var query1 = "insert into cliente (nombre, correo, telefono, fecha, direccion, ciudad, codigoPostal, region) "+
            "select nombre, correo, telefono, fecha_registro, direccion, ciudad, codigo_postal, region from temporal where tipo = 'C' group by nombre order by nombre";
            conexion.query(query1, function (err, result) {
                if (err) {
                    res.send(`Ha ocurrido un error cargando clientes! ${err}`);
                } else {
                    //=========CARGAR DATOS PROVEEDORES==========//
                    var query2 = "insert into proveedor (nombre, correo, telefono, fecha, direccion, ciudad, codigoPostal, region) "+
                    "select nombre, correo, telefono, fecha_registro, direccion, ciudad, codigo_postal, region from temporal where tipo = 'P' group by nombre order by nombre";
                    conexion.query(query2, function (err, result) {
                        if (err) {
                            res.send(`Ha ocurrido un error cargando proveedores! ${err}`);
                        } else {
                            //============CARGAR DATOS COMPRA=============//
                            var query3 = "insert into compra (compania, proveedor, producto, cantidad) "+
                            "select nombre_compania, nombre, producto, cantidad from temporal where tipo = 'P'";
                            conexion.query(query3, function (err, result) {
                                if (err) {
                                    res.send(`Ha ocurrido un error cargando compras! ${err}`);
                                } else {
                                    //============CARGAR DATOS VENTA=============//
                                    var query4 = "insert into venta (compania, cliente, producto, cantidad) "+
                                    "select nombre_compania, nombre, producto, cantidad from temporal where tipo = 'C'";
                                    conexion.query(query4, function (err, result) {
                                        if (err) {
                                            res.send(`Ha ocurrido un error cargando ventas! ${err}`);
                                        } else {
                                            //============CARGAR DATOS PRODUCTOS==============//
                                            var query5 = "insert into producto (nombre, categoria, precio) "+
                                            "select producto, categoria_producto, precio_unitario from temporal group by producto";
                                            conexion.query(query5, function(err, result){
                                                if (err) {
                                                    res.send(`Ha ocurrido un error cargando productos! ${err}`);
                                                } else {
                                                    res.send("Todos los datos se han cargado correctamente");
                                                }
                                            });
                                        }
                                    });
                                }
                            });
                        }
                    });                 
                }
            });
        }
    });
});

module.exports = router