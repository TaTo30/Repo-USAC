const express = require('express');
const md5 = require('md5');
const oracle = require('oracledb');

var router = express.Router();

let conexion = {
    user: 'TEST',
    password: '1234',
    connectString: '172.17.0.2/ORCL18',
}

router.post('/registrar', (req, res) => {
    let nombre = req.body.nombre;
    let apellido = req.body.apellido;
    let mail = req.body.mail;
    let pass = md5(req.body.pass);
    let cpass = md5(req.body.cpass);
    let nac = req.body.nac;
    let pais = req.body.pais;
    let foto = req.body.foto;
    let sql = `INSERT INTO CLIENTE (nombre, apellido, email, pass, cpass, nac, pais, foto, creditos) 
            VALUES ('${nombre}', '${apellido}', '${mail}', '${pass}', '${cpass}', '${nac}', '${pais}', '${foto}', 10000)`
    oracle.getConnection(conexion, (err, cn) => {
        if (err) {
            console.log(`Ha ocurrido un error en la conexion a la base de datos ${err.message}`);
        } else {
            cn.execute(sql, [], {autoCommit: true}, (err, result) =>{
                if (err) {
                    console.log(`Ha ocurrido un error en el query ${err.message}`);
                    res.send({"resultado": 0})
                } else {
                    res.send({"resultado": 1});
                }
                cn.release((err) => {
                    if (err) {
                        console.log(`Ha ocurrido un error cerrando la conexion ${err.message}`);
                    }
                })
            });
        }
    });
});

exports.router = router;
