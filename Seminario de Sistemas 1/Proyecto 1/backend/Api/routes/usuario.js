const Router = require('express')
const md5 = require('md5')
const { v4: uuidv4 } = require('uuid')
const aws = require('aws-sdk')

const { cn } = require('../config/connection')
const { BadRequest, Ok } = require('../config/responses')
const aws_keys = require('../config/aws')

const router = Router()
const s3 = new aws.S3(aws_keys.s3)

// POST: /usuario
// Crear un nuevo usuario
router.post('/usuario', async (request, response) => {
    console.log('POST: /usuario');
    
    if (!request.body.nombre) response.status(400).send(BadRequest("Campo 'nombre' es obligatorio"))
    if (!request.body.email) response.status(400).send(BadRequest("Campo 'email' es obligatorio"))
    if (!request.body.password) response.status(400).send(BadRequest("Campo 'password' es obligatorio"))
    let fotoData = ''
    if (request.body.foto) {
        const nombre = `fotos/${uuidv4().replace('-', '')}.${request.body.foto.extension}`
        const bytes = new Buffer.from(request.body.foto.datos, 'base64')
        const uploadResult = await s3.upload({
            Bucket: 'semi1archivos',
            Key: nombre,
            Body: bytes,
            ACL: 'public-read'
        }).promise()
        fotoData = uploadResult.Location
    }
    //Verificamos el Email no esta en uso
    const verifyQuery = `select id from usuario where email = '${request.body.email}'`

    cn.query(verifyQuery, (err, res) => {
        if (err) {
            console.error(`Error: verificacion de usuario: ${err.message}`);
            response.status(400).send(BadRequest('No se pudo completar la operacion'))
        }else{
            console.log(res);
            if (res.length !== 0) {
                response.status(400).send(BadRequest(`El email ${request.body.email} ya esta en uso`))
            }else{
                const query = `Insert into usuario (nombre, email, password, foto) values 
                ('${request.body.nombre}', '${request.body.email}', '${md5(request.body.password)}', '${fotoData}')`
                cn.query(query, (err, res)=>{
                    if (err) {
                        console.error(`Error: creacion de usuario: ${err.message}`);
                        response.status(400).send(BadRequest('No se pudo completar la operacion'))
                    } else {
                        const userQuery = `select id from usuario where email = '${request.body.email}' and password = '${md5(request.body.password)}'`
                        cn.query(userQuery, (err, res) => {
                            if (err) {
                                console.error(`Error: recuperacion de id usuario: ${err.message}`);
                                response.status(400).send(BadRequest('No se pudo completar la operacion'))
                            } else {
                                response.send( Ok(res[0]))
                            }
                        })
                    }
                })
            }
        }
    })   
})

// GET: /usuario
// obtiene todos los usuarios
router.get('/usuario', (request, response) => {
    console.log('GET: /usuario');

    const query = `
    select A.id, A.nombre, A.email, A.foto, sum(case when C.usuario is null then 0 else 1 end) as archivos from usuario A 
    left join (select A.* from carpeta A inner join archivo B on A.archivo = B.id where public = 1 ) C on C.usuario = A.id
    group by A.id`
    cn.query(query, (err, res)=>{
        if (err) {
            console.error(`Error: recuperacion de usuarios: ${err.message}`);
            response.status(400).send(BadRequest('No se pudo completar la operacion'))
        } else {
            response.send(Ok(res)) 
        }
    })
})

module.exports = router