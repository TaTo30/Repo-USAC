import io from 'socket.io-client'

let socket = io("//localhost:4000", { transports : ['websocket']})

export default socket;