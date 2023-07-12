const mysql = require('mysql')

const pool = mysql.createConnection({
    host: 'localhost',
    user: 'root',
    password: '31370599',
    database: 'semi1'
})

pool.connect(err => {
    if (err) {
        console.log('No se puede establecer conexion con la base de datos');
        return
    }
    console.log('Conexion establecida');
    
})

exports.cn = pool