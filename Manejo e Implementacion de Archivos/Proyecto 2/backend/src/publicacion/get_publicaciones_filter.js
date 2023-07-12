const express = require('express');
const oracle = require('oracledb');

var router = express.Router();

let conexion = {
    user: 'TEST',
    password: '1234',
    connectString: '172.17.0.2/ORCL18',
}

router.post('/get_publicaciones_filter', (req, res) => {
    sql = `select A.*, B.NOMBRE, C.ID, C.NOMBRE, C.APELLIDO, C.EMAIL, C.NAC, C.PAIS, C.FOTO, C.CREDITOS  
    FROM PUBLICACION A
    INNER JOIN CATEGORIAS B ON A.id_categoria = B.id 
    INNER JOIN CLIENTE C ON A.vendedor = C.id
    WHERE B.NOMBRE LIKE '%${req.body.categoria}%' AND TAGS LIKE '%${req.body.busqueda}%' 
    ORDER BY PRECIO_PRODUCTO ${req.body.orden}`;
    oracle.getConnection(conexion, (err, con) => {
        if (err) {
            console.error(err);
        } else {
            con.execute(sql, [], {autoCommit: true}, (err, result) => {
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