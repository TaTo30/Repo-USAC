const express = require('express')
const Server = require('socket.io')
const WebSocketServer = require('socket.io')
const http = require('http')
const clientRouter = require('./Routes/client.router')
const socketClass = require('./Controller/socket.controller');

require('./Mongo/database')

const morgan = require('morgan');
const cors = require('cors')
const urls = require('url')

const app = express()

app.use(express.json({limit: '50mb'}));
app.use(express.urlencoded({limit: '50mb'}));
app.use(cors());
app.use(morgan('dev'));
app.set('port', process.env.PORT || 3001);
app.use(express.json());
app.use(express.urlencoded({extended: false}));

app.use('/', clientRouter)

const httpServer = http.createServer(app);

const io = WebSocketServer(httpServer)

io.on('connection', (socket) => {
    console.log("conectado: "+socket.id)
    socketClass.sockets(socket)    
})

httpServer.listen(app.get('port'), ()=>{
    console.log('server chambeando', app.get('port'));    
});