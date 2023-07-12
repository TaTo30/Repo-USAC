const express = require('express');
const oracle = require('oracledb');

var router = express.Router();

let conexion = {
    user: 'TEST',
    password: '1234',
    connectString: '172.17.0.2/ORCL18',
}

router.get('/get_publicaciones', (req, res) => {
    sql = `SELECT A.*, B.NOMBRE, C.ID, C.NOMBRE, C.APELLIDO, C.EMAIL, C.NAC, C.PAIS, C.FOTO, C.CREDITOS 
    FROM PUBLICACION A
    INNER JOIN CATEGORIAS B ON A.id_categoria = B.id 
    INNER JOIN CLIENTE C ON A.vendedor = C.id`;
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