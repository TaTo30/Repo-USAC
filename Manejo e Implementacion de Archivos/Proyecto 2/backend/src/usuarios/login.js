const express = require('express');
const md5 = require('md5');
const oracle = require('oracledb');

var router = express.Router();

let conexion = {
    user: 'TEST',
    password: '1234',
    connectString: '172.17.0.2/ORCL18',
}

router.post("/login", (req, res) => {
    let correo = req.body.correo;
    let pass = md5(req.body.pass);
   // let sql = `select case when (email = '${correo}' and pass = '${pass}') 
   //         then 1 else 0 end as resultado from CLIENTE order by resultado desc FETCH FIRST 1 ROWS ONLY`;
    let sql =`select * from CLIENTE WHERE email='${correo}' and pass = '${pass}'` 
    oracle.getConnection(conexion, (err, connection) =>{
        if (err) {
            console.log(`Ha ocurrido un error en la conexion a la base de datos ${err.message}`);
        } else {
            connection.execute(sql, [], {autoCommit: true}, (err, result) => {
                if (err) {
                    console.log(`Ha ocurrido un error en el query ${err.message}`);
                    res.send({"resultado": 0})
                } else {
                    res.send(result.rows);
                }
                connection.release((err)=>{
                    if (err) {
                        console.log(`Ha ocurrido un error cerrando la conexion ${err.message}`);
                    }
                });
            });
        }
    });

});

exports.router = router;