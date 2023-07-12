
var fs = require('fs');
var parser = require('./Jison/java');
var Errores = require('./Analizador/Errores');
var Tokens = require('./Analizador/Tokens');
const express = require('express');
const body_parser = require('body-parser');
const cors = require('cors');
var app = express();

app.use(body_parser.json());
app.use(cors());

app.post('/javascript',function (req, res) {
    /*=========LIMPIAR BUFFERES=========*/
    Errores.Limpiar();
    Tokens.Limpiar();
    parser.Clear();
    /*=========TRADUCIR CODIGO==========*/
    var traduccion;
    var texto = req.body.Texto;
    try {
        traduccion = parser.parse(texto);
    } catch (error) {
        traduccion = parser.Save();
        //console.log(traduccion);
    }
    var astdot = parser.Dot();
    //console.log("Desde el retorno: ", astdot);
    var tokens = Tokens.ObtenerTokens;
    var errores = Errores.ObtenerErrores;
    /*======CREAR VALOR DE RETRONO======*/
    res.send({
        "Errores": errores,
        "Tokens": tokens,
        "Traduccion": traduccion,
        "Dot": astdot
    });
    
})


app.listen('3000', function () {
    console.log(`Escuchando en el puerto 3000`);
});
