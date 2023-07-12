const socketClass = require('../Recursos/Socket');

var registrosMongo = 0; 
var registrosRedis = 0;

exports.verificacion = () => {
    socket = socketClass.getSocket();    

    socket.on('connection', (socket) => {    
        socket.on('verificar', msg => {
            registrosMongo++
            socket.emit('verificado', verificarDBs(msg))
        })
    })
}

function verificarDBs(player){    
    return {
        reporte1Columnas: {mongo: ["Registros", "Jugadores", "Jugados"], redis: ["Registros", "Jugadores", "Jugados"]},
        reporte2Columnas: {mongo: ["ID", "Juego", "Ganador", "Jugadores"], redis: ["ID", "Juego", "Ganador", "Jugadores"]},
        reporte3Columnas: {mongo: ["Jugador", "Juegos Ganados"], redis: ["Jugador", "Juegos Ganados"]},
        reporte4Columnas: {mongo: ["Jugador", "Juegos Ganados"], redis: ["Jugador", "Juegos Ganados"]},
        reporte5Columnas: {mongo: ["Fecha", "Transaccion"], redis: ["Fecha", "Transaccion"]},
        reporte6Labels: {mongo: [], redis: []},
        reporte7Labels: {mongo: ["Kafka", "Pub/Sub", "RabbitMQ"], redis: ["Kafka", "Pub/Sub", "RabbitMQ"]},

        reporte1Data: {mongo: [["R", "M"]], redis: [["R", "M"]]},
        reporte2Data: {mongo: [["R", "M"]], redis: [["R", "M"]]},
        reporte3Data: {mongo: [["R", "M"]], redis: [["R", "M"]]},
        reporte4Data: {mongo: [["R", "M"]], redis: [["R", "M"]]},
        reporte5Data: {mongo: [["R", "M"]], redis: [["R", "M"]]},
        reporte6Data: {mongo: [registrosMongo++, 1], redis: [1, 1]},
        reporte7Data: {mongo: [1, 1], redis: [1, 1]}
    }  
}