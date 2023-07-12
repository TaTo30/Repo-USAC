const express = require('express');
const md5 = require('md5');
const oracle = require('oracledb');

var router = express.Router();

let conexion = {
    user: 'TEST',
    password: '1234',
    connectString: '172.17.0.2/ORCL18',
}

router.post('/update_cliente', (req, res) =>{
    let sql = "";
    let pass = req.body.pass;
    let id = req.body.id;
    if (pass != "") {
        //existe un cambio de contraseña
        pass = md5(pass);
        let cpass = md5(req.body.pass);
        let nac = req.body.nac;
        let pais = req.body.nac;
        let nombre = req.body.nombre;
        let apellido = req.body.apellido;
        sql = `UPDATE CLIENTE SET nombre='${nombre}', apellido='${apellido}', pass = '${pass}', cpass = '${cpass}', nac='${nac}', pais='${pais}' WHERE id = ${id}`;
    }else{
        //no hay un cambio de contraseña
        let nac = req.body.nac;
        let pais = req.body.pais;
        let nombre = req.body.nombre;
        let apellido = req.body.apellido;
        sql = `UPDATE CLIENTE SET nombre='${nombre}', apellido='${apellido}', nac='${nac}', pais='${pais}' WHERE id = ${id}`;
    }
    console.log(sql);
    oracle.getConnection(conexion, (err, connection) => {
        if (err) {
            console.log(`Ha ocurrido un error en la conexion a la base de datos ${err.message}`);
        } else {
            connection.execute(sql, [], {autoCommit: true}, (err, result) =>{
                if (err) {
                    console.log(`Ha ocurrido un error en el query ${err.message}`);
                    res.send({"resultado": 0})
                } else {
                    res.send(result);
                }
                connection.release(err => {
                    if (err) {
                        console.log(`Ha ocurrido un error cerrando la conexion ${err.message}`);
                    }
                })
            });
        }
    });
});

exports.router = router;