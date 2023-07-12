const express = require('express');
const oracle = require('oracledb');

var router = express.Router();

let conexion = {
    user: 'TEST',
    password: '1234',
    connectString: '172.17.0.2/ORCL18',
}

router.post('/set_denuncia', (req, res) => {
    sql = `INSERT INTO DENUNCIA (publicacion, contenido) 
        VALUES (${req.body.publicacion}, '${req.body.contenido}')`;
    oracle.getConnection(conexion, (err, con) => {
        if (err) {
            console.error(err);
        } else {
            con.execute(sql, [], {autoCommit: true}, (err, result) => {
                if (err) {
                    console.error(err);
                    res.send({'Resultado':0});
                } else {
                    res.send({'Resultado':1});
                }
                con.release(err => {
                    if (err) {
                        console.error(err);
                    }
                })
            })
        }
    })
})

exports.router = router;