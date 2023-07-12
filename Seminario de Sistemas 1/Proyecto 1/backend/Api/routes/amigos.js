const Router = require('express')
const { cn } = require('../config/connection')
const { BadRequest, Ok } = require('../config/responses')

const router = Router()

// POST: /usuario/::id1/amigo/::id2
// Agrega una amistad entre id1 y id2
router.post('/usuario/:uid1/amigo/:uid2', (request, response) => {
    console.log(`POST: /usuario/${request.params.uid1}/amigo/${request.params.uid2}`);
    
    const verifyQuery = `select * from amigos where usuario1 in (${request.params.uid1}, ${request.params.uid2}) and usuario2 in (${request.params.uid1}, ${request.params.uid2})`
    cn.query(verifyQuery, (err, res) => {
        if (err) {
            console.error(`Error: verificando amigos: ${err.message}`);
            response.status(400).send(BadRequest('No se pudo completar la operacion'))
        }else{
            if (res.length !== 0) {
                response.status(400).send(BadRequest('Los usuarios ya son amigos'))
            }else{
                const query = `insert into amigos (usuario1, usuario2) values (${request.params.uid1}, ${request.params.uid2})`
                cn.query(query, (err, res) => {
                    if (err) {
                        console.error(`Error: creacion de amigos: ${err.message}`);
                        response.status(400).send(BadRequest('No se pudo completar la operacion'))
                    } else {
                        response.send(Ok([request.params.uid1, request.params.uid2]))
                    }
                })
            }
        }
    })
})

// DELETE: /usuario/::id1/amigo/::id2
// Elimina la amistad entre id2 y id2
router.delete('/usuario/:uid1/amigo/:uid2', (request, response) => {
    console.log(`DELETE: /usuario/${request.params.uid1}/amigo/${request.params.uid2}`);

    const query = `delete from amigos where usuario1 in (${request.params.uid1}, ${request.params.uid2}) and usuario2 in (${request.params.uid1}, ${request.params.uid2})`
    cn.query(query, (err, res) => {
        if (err) {
            console.error(`Error: eliminacion de amigos: ${err.message}`);
            response.status(400).send(BadRequest('No se pudo completar la operacion'))
        } else {
            response.send(Ok([request.params.uid1, request.params.uid2]))
        }
    })
})

// GET: /usuario/::id/amigo
// Obtiene los amigos del usuario id
router.get('/usuario/:uid1/amigo/', (request, response) => {
    console.log(`GET: /usuario/${request.params.uid1}/amigo/`);

    const queryUser1 = `
    select B.id, B.nombre, B.email, B.foto, sum(case when C.usuario is null then 0 else 1 end) as archivos from amigos A 
    inner join usuario B on A.usuario2 = B.id 
    left join (select A.* from carpeta A inner join archivo B on A.archivo = B.id where public = 1 ) C on C.usuario = A.usuario2
    where A.usuario1 = ${request.params.uid1}
    group by B.id
    `
    cn.query(queryUser1, (err, res) => {
        if (err) {
            console.error(`Error: recuperacion de amigos (1): ${err.message}`);
            response.status(400).send(BadRequest('No se pudo completar la operacion'))
        } else {
            const queryUser2 = `
            select B.id, B.nombre, B.email, B.foto, sum(case when C.usuario is null then 0 else 1 end) as archivos from amigos A 
            inner join usuario B on A.usuario1 = B.id 
            left join (select A.* from carpeta A inner join archivo B on A.archivo = B.id where public = 1 ) C on C.usuario = A.usuario1
            where A.usuario2 = ${request.params.uid1}
            group by B.id
            `
            cn.query(queryUser2, (err, _res) => {
                if (err) {
                    console.error(`Error: recuperacion de amigos (2): ${err.message}`);
                    response.status(400).send(BadRequest('No se pudo completar la operacion'))
                } else {
                    let usuario2 = res
                    let usuario1 = _res
                    //response.send(Ok(usuario2.concat(usuario1)))
                    const queryAllUser = `
                        select A.id, A.nombre, A.email, A.foto, sum(case when C.usuario is null then 0 else 1 end) as archivos from usuario A 
                        left join (select A.* from carpeta A inner join archivo B on A.archivo = B.id where public = 1 ) C on C.usuario = A.id
                        where A.id not in (select (case when usuario1 = ${request.params.uid1} then usuario2 else usuario1 end) from amigos where usuario1 = ${request.params.uid1} or usuario2 = ${request.params.uid1})
                        and A.id != ${request.params.uid1}
                        group by A.id
                    `
                    cn.query(queryAllUser, (err, __res) => {
                        if (err) {
                            console.error(`Error: recuperacion de amigos (3): ${err.message}`);
                            response.send(Ok(usuario2.concat(usuario1)))
                            //response.status(400).send(BadRequest('No se pudo completar la operacion'))
                        } else {
                            response.send(Ok({amigos: usuario2.concat(usuario1), otros: __res}))
                        }
                    })
                    
                }
            }) 
        }
    })
})


module.exports = router