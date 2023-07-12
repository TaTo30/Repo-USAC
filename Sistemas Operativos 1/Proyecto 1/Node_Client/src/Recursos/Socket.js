let socket = null;

exports.setSocket = (newSocket) => {
    socket = newSocket;
}

exports.getSocket = ()=>{
    return socket;
}