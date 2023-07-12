const express = require('express');
const oracle = require('oracledb');

var router = express.Router();

let conexion = {
    user: 'TEST',
    password: '1234',
    connectString: '172.17.0.2/ORCL18',
}

router.get('/comentarios_clientes', (req, res) =>{
    sql = `SELECT B.NOMBRE, B.APELLIDO, B.NAC, COUNT(B.ID) Comentarios FROM COMENTARIO A
    INNER JOIN CLIENTE B ON a.remitente = B.ID
    GROUP BY B.ID, B.NOMBRE, B.APELLIDO, B.NAC`;
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