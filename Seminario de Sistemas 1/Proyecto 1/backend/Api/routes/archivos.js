const Router = require('express')
const { v4: uuidv4 } = require('uuid')
const aws = require('aws-sdk')

const { cn } = require('../config/connection')
const { BadRequest, Ok } = require('../config/responses')
const aws_keys = require('../config/aws')

const router = Router()
const s3 = new aws.S3(aws_keys.s3)

// POST: /usuario/::id/archivo/
// Sube un archivo
router.post('/usuario/:id/archivo/', (request, response) => {
    console.log(`POST: /usuario/${request.params.id}/archivo/`);

    if (!request.body.nombre) response.status(400).send(BadRequest("Campo 'nombre' es obligatorio"))
    if (!request.body.datos) response.status(400).send(BadRequest("Campo 'datos' es obligatorio"))
    if (request.body.public === undefined) request.body.public = true;

    const nombre = `archivos/${request.body.nombre}${uuidv4().replace('-', '')}.${request.body.extension}`
    const bytes = new Buffer.from(request.body.datos, 'base64')

    s3.upload({
        Bucket: 'semi1archivos',
        Key: nombre,
        Body: bytes,
        ACL: 'public-read'
    }, (err, data) => {
        if (err) {
            console.error(`Error: subida de archivos: ${err.message}`);
            response.status(400).send(BadRequest('No se pudo completar la operacion'))
        } else {
            const uploadDate = Date.now()
            const fileQuery = `insert into archivo (nombre, extension, public, datos, fecha) values 
            ('${request.body.nombre}', '${request.body.extension}', ${request.body.public}, '${data.Location}', ${uploadDate})`
            cn.query(fileQuery, (err, res) => {
                if (err) {
                    console.error(`Error: creacion de archivos: ${err.message}`);
                    response.status(400).send(BadRequest('No se pudo completar la operacion'))
                } else {
                    const fileGetQuery = `select id from archivo where fecha = ${uploadDate} and nombre = '${request.body.nombre}'`
                    cn.query(fileGetQuery, (err, res) => {
                        if (err) {
                            console.error(`Error: recuperacion de archivo creado: ${err.message}`);
                            response.status(400).send(BadRequest('No se pudo completar la operacion'))
                        } else {
                            const fileId = res[0].id
                            const carpetQuery = `insert into carpeta (usuario, archivo) values 
                            (${request.params.id}, ${fileId})`
                            cn.query(carpetQuery, (err, res) => {
                                if (err) {
                                    console.error(`Error: vinculo de archivo: ${err.message}`);
                                    response.status(400).send(BadRequest('No se pudo completar la operacion'))
                                } else {
                                    response.send(Ok({id: fileId}))
                                }
                            })
                        }
                    })
                }
            })            
        }
    })    
})

// GET: /usuario/::id/archivo/
// Obtiene todos los archivos del usuario
router.get('/usuario/:id/archivo', (request, response) => {
    console.log(`GET: /usuario/${request.params.id}/archivo/`);

    const query = `select B.* from carpeta A inner join archivo B on A.archivo = B.id where A.usuario = ${request.params.id}`
    cn.query(query, (err, res) => {
        if (err) {
            console.error(`Error: recuperacion de archivos: ${err.message}`);
            response.status(400).send(BadRequest('No se pudo completar la operacion'))
        } else {
            let result = res.map(value => {
                var {fecha, ...x} = value
                var formatDate = new Date(fecha/1)
                return {
                    ...x,
                    fecha: formatDate.toLocaleString()
                }
            }) 
            response.send(Ok(result))
        }
    })
})

// PUT: /usuario/::id1/archivo/::id2
// Modifica los datos del archivo id2 del usuario id1
router.put('/usuario/:uid/archivo/:fid', (request, response) => {
    console.log(`PUT: /usuario/${request.params.uid}/archivo/${request.params.fid}`);
    
    const query = `update archivo set public = ${request.body.public} where id = ${request.params.fid}`
    cn.query(query, (err, res) => {
        if (err) {
            console.error(`Error: recuperacion de archivos: ${err.message}`);
            response.status(400).send(BadRequest('No se pudo completar la operacion'))
        } else {
            response.send(Ok({id: request.params.fid}))
        }
    })
})

// DELETE: /usuario/::id/archivo/::id2
// Elimina el archivo id2 del usuario id1
router.delete('/usuario/:uid/archivo/:fid', (request, response) => {
    console.log(`DELETE: /usuario/${request.params.uid}/archivo/${request.params.fid}`);

    const query = `delete from carpeta where usuario = ${request.params.uid} and archivo = ${request.params.fid}`
    cn.query(query, (err, res) => {
        if (err) {
            console.error(`Error: recuperacion de archivos: ${err.message}`);
            response.status(400).send(BadRequest('No se pudo completar la operacion'))
        } else {
            const getFileData = `select datos from archivo where id = ${request.params.fid}`
            cn.query(getFileData, (err, res) => {
                if (!err) {
                    const key = res[0].datos.split('/')
                    const deleteResult = s3.deleteObject({
                        Bucket: 'semi1archivos',
                        Key:`archivos/${key[key.length - 1]}` 
                    }).promise()
                    deleteResult
                        .then(console.info('Archivo eliminado en la nube'))
                        .catch(err => console.warn(`No se pudo eliminar el archivo en la nube: ${err}`))
                }
                const queryFile = `delete from archivo where id = ${request.params.fid}`
                cn.query(queryFile, (err, res) => {
                    if (err) {
                        console.error(`Error: recuperacion de archivos: ${err.message}`);
                        response.status(400).send(BadRequest('No se pudo completar la operacion'))
                    } else {
                        response.send(Ok({id: request.params.fid}))
                    }
                })
            })
        }
    })
})

module.exports = router