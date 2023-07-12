const express = require('express');
const oracle = require('oracledb');

var router = express.Router();

let conexion = {
    user: 'TEST',
    password: '1234',
    connectString: '172.17.0.2/ORCL18',
}

router.get('/productos_vendidos', (req, res) =>{
    sql = `select COUNT(P.id) Ventas, P.id, P.NOMBRE_PRODUCTO 
    from VENTA V
    INNER JOIN PUBLICACION P ON V.PUBLICACION = P.ID
    GROUP BY P.id, P.NOMBRE_PRODUCTO
    ORDER BY COUNT(P.id) DESC`;
    oracle.getConnection(conexion, (err, cn) =>{
        if (err) {
            console.log(`Ha ocurrido un error en la conexion a la base de datos ${err.message}`);
        } else {
            cn.execute(sql, (err, result) =>{
                if (err) {
                    console.log(`Ha ocurrido un error en el query ${err.message}`);
                } else {
                    res.send(result.rows);
                }
                cn.release( err =>{
                    if (err) {
                        console.log(`Ha ocurrido un error en el cierre de conexion ${err.message}`);
                    }
                })
            })
        }
    })
});

exports.router = router;