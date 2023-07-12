const express = require('express');
const oracle = require('oracledb');

var router = express.Router();

let conexion = {
    user: 'TEST',
    password: '1234',
    connectString: '172.17.0.2/ORCL18',
}

router.post('/get_reaccion', (req, res) =>{
    sql = `SELECT * FROM REACCION WHERE 
    CLIENTE_ID = ${req.body.cliente} AND PUBLICACION_ID = ${req.body.publicacion}`;
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