const parser = require('./Analizador/python');
const DOT = require('./Analizador/dot');
const express = require('express');
const body_parser = require('body-parser');
const cors = require('cors');
const Errores = require('./Analizador/Errores');
const Tokens = require('./Analizador/Tokens');


var app = express();

app.use(body_parser.json());
app.use(cors());

app.post('/python', function (req, res) {
    /*==========LIMPIAR PARAMETROS===========*/
    Errores.Limpiar();
    Tokens.Limpiar();
    DOT.Clear();
    parser.Clear();
    /*==========TRADUCIR EL CODIGO===========*/
    var texto = req.body.Texto;
    let traduccion = parser.parser(texto);
    /*========CREAR VALOR DE RETORNO=========*/    
    res.send({
        "Errores": Errores.ObtenerErrores(),
        "Tokens": Tokens.ObtenerTokens(),
        "Traduccion": traduccion,
        "Dot": DOT.GetDot()
    });
    
})


app.listen('3030', function () {
    console.log(`Escuchando en el puerto 3030`);
});
