const express = require('express');
const oracle = require('oracledb');

var router = express.Router();

let conexion = {
    user: 'TEST',
    password: '1234',
    connectString: '172.17.0.2/ORCL18',
}

router.get('/get_comentario', (req, res) => {
    sql = `SELECT * FROM COMENTARIO`;
    oracle.getConnection(conexion, (err, con) => {
        if (err) {
            console.error(err);
        } else {
            con.execute(sql, (err, result) => {
                if (err) {
                    console.error(err);
                } else {
                    res.send(result.rows);
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