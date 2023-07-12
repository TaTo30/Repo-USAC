const Router = require('express')
const md5 = require('md5')

const { cn } = require('../config/connection')
const { BadRequest, Ok } = require('../config/responses')

const router = Router()


// POST: /login
// Logea un usuario nuevo

router.post('/login', (request, response) => {
    console.log('POST: /login');

    const query = `select * from usuario where email = '${request.body.email}'`
    cn.query(query, (err, res)=>{
        if (err) {
            console.error(`Error: recuperacion de usuarios: ${err.message}`);
            response.status(400).send(BadRequest('No se pudo completar la operacion'))
        } else {
            if (res.length !== 0) {
                const {password, ...x} = res[0]
                if (password === md5(request.body.password)) {
                    response.send(Ok(x))
                } else {
                    response.status(400).send(BadRequest('Contraseña Incorrecta'))
                }
            }else{
                response.status(400).send(BadRequest('Email Incorrecto'))
            }
        }
    })
})

// POST: /verify
// Verifica la contraseña del usuario

router.post('/verify', (request, response) => {
    console.log('POST: /verify');

    const query = `select * from usuario where id = ${request.body.id}`
    cn.query(query, (err, res)=>{
        if (err) {
            console.error(`Error: recuperacion de usuarios: ${err.message}`);
            response.status(400).send(BadRequest('No se pudo completar la operacion'))
        } else {
            if (res.length !== 0) {
                const {password, ...x} = res[0]
                if (password === md5(request.body.password)) {
                    response.send(Ok(x))
                } else {
                    response.status(400).send(BadRequest(false))
                }
            }else{
                response.status(400).send(BadRequest(false))
            }
        }
    })
})

module.exports = router