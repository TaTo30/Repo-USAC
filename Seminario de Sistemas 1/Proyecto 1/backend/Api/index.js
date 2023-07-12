const express = require('express')
const cors = require('cors')


const app = express()

app.use(cors())
app.use(express.json({limit: '50mb'}))


app.use(require('./routes/usuario'))
app.use(require('./routes/archivos'))
app.use(require('./routes/amigos'))
app.use(require('./routes/autentica'))

app.listen(5000, () => {
    console.log(`Servicio REST publicado en el puerto ${5000}`);
})