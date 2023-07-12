const express = require('express');
var app = express();
const body_parser = require('body-parser');
const cors = require('cors');
var http = require('http').createServer(app);
var io = require('socket.io')(http);


//====================ATRIBUTOS DE CONEXION======================//

app.use(body_parser.json());
app.use(cors());


//================IMPORTACION DE RUTAS ENDPOINT==================//
app.use(require('./src/usuarios/registro').router);
app.use(require('./src/usuarios/login').router);
app.use(require('./src/usuarios/update_cliente').router);
app.use(require('./src/usuarios/get_cliente').router);
app.use(require('./src/categorias/get_categorias').router);
app.use(require('./src/categorias/set_categoria').router);
app.use(require('./src/publicacion/set_publicacion').router);
app.use(require('./src/publicacion/get_publicaciones').router);
app.use(require('./src/publicacion/get_publicaciones_filter').router);
app.use(require('./src/publicacion/get_publicacion').router);
app.use(require('./src/carrito/set_carrito').router);
app.use(require('./src/carrito/get_carrito').router)
app.use(require('./src/carrito/vaciar_carrito').router);
app.use(require('./src/carrito/transferir_creditos').router);
app.use(require('./src/comentarios/set_comentario').router);
app.use(require('./src/comentarios/get_comentario').router);
app.use(require('./src/comentarios/set_denuncia').router);
app.use(require('./src/reaccion/set_reaccion').router);
app.use(require('./src/reaccion/get_reaccion').router);
app.use(require('./src/reportes/productos_vendidos').router);
app.use(require('./src/reportes/productos_dislikes').router);
app.use(require('./src/reportes/productos_likes').router);
app.use(require('./src/reportes/comentarios_clientes').router);
app.use(require('./src/reportes/ventas_clientes').router)
app.use(require('./src/reportes/creditos_clientes').router);
app.use(require('./src/reportes/creditos_paises').router);



app.listen(3000, function () {
    console.log("Escuchando en el puerto 3000");
});



