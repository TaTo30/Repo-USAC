/*==============LIBRERIAS==============*/
const express = require("express");
const morgan = require("morgan")
const app  = express();


/*==============ATRIBUTOS==============*/
app.use(morgan('dev'));
app.use(express.urlencoded({extended: false}))
app.use(express.json());


/*================RUTAS================*/
app.use(require('./routes/eliminarTemporal'));
app.use(require('./routes/cargarTemporal'));
app.use(require('./routes/cargarModelo'));
app.use(require('./routes/eliminarModel'));
app.use(require('./routes/consulta1'));
app.use(require('./routes/consulta2'));
app.use(require('./routes/consulta3'));
app.use(require('./routes/consulta4'));
app.use(require('./routes/consulta5'));
app.use(require('./routes/consulta6'));
app.use(require('./routes/consulta7'));
app.use(require('./routes/consulta8'));
app.use(require('./routes/consulta9'));
app.use(require('./routes/consulta10'));


/*=============CAPTURADOR==============*/
app.listen(3000, () => {
    console.log(`Servidor iniciacion ${3000}`);
});