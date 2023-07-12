var uuid = require('uuid');
var express = require('express');
var userService = require('./services/UserService');
var postService = require('./services/PostService');
var requestService = require('./services/RequestService');
var bodyParser = require('body-parser');
var cors = require('cors')
const user = require('./models/user');
var loginService = require('./services/Login')



var app = express();
app.use(cors())


app.use(bodyParser.json({ limit: '5mb' }));
app.use(bodyParser.urlencoded({
    extended: true
}));

app.use(express.json());
app.use(express.urlencoded({ extended: false }));

app.get('/users', (req, res) => {
    userService.getAllUsers().then(data => res.status(200).send(data)).catch(err => res.status(500).send(err));
})

app.post('/createUser', (req, res) => {
    userService.createUser(req.body).then(data => res.status(200).send(data)).catch(err => res.status(500).send(err));
})

app.get('/profile/:id', (req, res) => {
    userService.findUserById(req.params.id)
        .then(data => {
            if (!data) {
                return res.status(404).send({ message: "El usuario no existe" });
            } else {
                return res.status(200).send(data);
            }
        })
        .catch(err => {
            return res.status(500).send({ message: "Ocurrió un error al procesar la petición" });
        });
});

app.get('/notFriends/:id', (req, res) => {
    userService.getNotFriends(req.params.id)
        .then(data => {
            if (!data) {
                return res.status(404).send({ message: "El usuario no existe" });
            } else {
                return res.status(200).send(data);
            }
        })
        .catch(err => {
            return res.status(500).send({ message: "Ocurrió un error al procesar la petición" });
        });
});


//EDITAR INFORMACION DE USUARIO 
app.put('/user/update/:id', (req, res) => {
    userService.patchUserById(req,res)
});

//EL ENDPOINT PARA PROBAR EL LOGIN ES /USER/LOGIN 
//EL ENDPOINT PARA PROBAR EL REGISTRO ES /USER/REGISTER
//Si intentan entrar con el usuario enrique y juan no los dejara ya que en la db la password no se registro encriptada
app.use('/user', loginService)

// Configuración de S3

const AWS = require('aws-sdk');
const { func } = require('@hapi/joi');

AWS.config.update({
    region: 'us-east-1',
    accessKeyId: 'AKIAW2CKO7KN3FWYRIGW',
    secretAccessKey: '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'
});

// Crear publicación

app.post('/posts/:username/create', (req, res) => {
    userService.findUserByUsername({ username: req.params.username })
        .then(data => {
            let fileName;
            const userId = data._id;
            if (req.body.image && req.body.image !== '') {
                // Crear buffer de la imagen enviada en base64
                const buffer = new Buffer.from(req.body.image, 'base64');
                // Generar el nombre del archivo
                fileName = new Date().toISOString() + "-" + uuid.v4() + '.jpg';
                // Crear el objeto de S3
                const s3 = new AWS.S3();
                // Parámetros a enviar
                const params = {
                    Bucket: 'ayd1-practica2',
                    Key: fileName,
                    Body: buffer,
                    ContentType: "image"
                };
                // Subir la foto a S3
                s3.putObject(params).promise()
                    .then(data => {
                        // Obtener la ruta de la imagen
                        req.body.image = fileName;
                        // Crear publicación
                        postService.createPost(userId, {
                            date: Date.now(),
                            text: req.body.text,
                            image: 'https://ayd1-practica2.s3.amazonaws.com/' + fileName
                        })
                            .then(data => {
                                res.status(200).json({ message: "Publicación creada satisfactoriamente" });
                            })
                            .catch(err => {
                                res.status(500).json({ message: "Se produjo un error al crear la publicación" })
                            });
                    })
                    .catch(err => {
                        res.status(500).json({ "message": "Se produjo un error al cargar la foto" });
                    });
            }
        })
        .catch(err => {
            res.status(500).json({ message: "Se produjo un error al procesar la petición" });
        });
});

// ENVIAR SOLICITUDES DE AMISTAD

app.post('/requests/send', (req, res) => {
    userService.findUserByUsername({ username: req.body.to })
        .then(data => {
            requestService.sendRequest(req.body.from, {
                from: req.body.from,
                to: req.body.to,
                status: 'pending'
            })
                .then(data => {
                    res.status(200).json({ message: "Solicitud de amistad enviada con éxito!" });
                })
        })
        .catch(err => {
            res.status(500).json({ message: "Se produjo un error al procesar la petición" });
        });
});

// OBTENER SOLICITUDES DEL USUARIO

app.get('/requests/:username', (req, res) => {
    userService.findUserByUsername({username: req.params.username})
    .then(data => {
        requestService.getUserRequests(data._id)
        .then(data => {              
            res.status(200).json({requests: data.requests})
        })
    })
    .catch(err => {
        res.status(500).json({message: "Se produjo un error al obtener las solicitudes"});
    });
});

// RECHAZAR SOLICITUD DE AMISTAD

app.post('/requests/:id/reject', (req, res) => {
    requestService.findRequestById({_id: req.params.id})
    .then(data => {
        requestService.rejectRequest(data._id)
        .then(data => {
            res.status(200).json({message: "Solicitud de amistad rechazada"});
        })
    })
    .catch(err => {
        res.status(500).json({message: "Se produjo un error al procesar la petición"});
    });
});

// ACEPTAR SOLICITUD DE AMISTAD

app.post('/requests/:idMyUser/accept/:idRequest', (req, res) => {
    requestService.findRequestById({_id: req.params.idRequest})
    .then(data => {
        requestService.acceptRequest(data._id)
        .then(data => {
            //ACTUALIZAR LISTA DE AMIGOS
            userService.addFriends(req.params.idMyUser, data.from);

            res.status(200).json({message: "Solicitud de amistad aceptada"});
        })
    })
    .catch(err => {
        res.status(500).json({message: "Se produjo un error al procesar la petición"});
    });
});

// Obtener publicaciones del usuario (propias)

app.get('/posts/:username', (req, res) => {
    userService.findUserByUsername({ username: req.params.username })
        .then(data => {
            userService.getUserPosts(data._id)
                .then(data => {
                    if (data) {
                        // Obtener los posts
                        const posts = data.posts;
                        // Ordenar los posts                
                        const orderedPosts = posts.sort((a, b) => b.date - a.date);
                        // Agregar la ruta del bucket a las imagenes
                       
                        // Devolver el array ordenado
                        res.status(200).json({ posts: orderedPosts });
                    } else {
                        res.status(200).json({ posts: data.posts });
                    }
                })
                .catch(err => {
                    res.status(500).json({ message: "Se produjo un error al obtener las publicaciones" });
                });
        })
        .catch(err => {
            res.status(500).json({ error: "Se produjo un error al procesar la petición" });
        });
});

module.exports = app;