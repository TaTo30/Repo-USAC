const express = require('express');
const oracle = require('oracledb');

var router = express.Router();

let conexion = {
    user: 'TEST',
    password: '1234',
    connectString: '172.17.0.2/ORCL18',
}

router.get('/paises_creditos', (req, res) =>{
    sql = `SELECT A.*, B.PUBLICACIONES FROM
    (
    SELECT C.PAIS, SUM(C.CREDITOS) Creditos, COUNT(C.ID) Vendedores FROM CLIENTE C
    GROUP BY C.PAIS
    ) A
    INNER JOIN 
    (
    SELECT DISTINCT C.PAIS, COUNT(P.ID) Publicaciones FROM CLIENTE C
    LEFT JOIN PUBLICACION P ON P.VENDEDOR = C.ID
    GROUP BY C.PAIS
    ) B
    ON A.PAIS = B.PAIS
    ORDER BY A.PAIS`;
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