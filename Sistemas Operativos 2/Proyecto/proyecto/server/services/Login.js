const router = require('express').Router()
const User = require('../models/user');
const Joi = require('@hapi/joi');
const jwt = require('jsonwebtoken')
const bcrypt = require('bcrypt')
const uuid = require('uuid');

const AWS = require('aws-sdk');

AWS.config.update({
    region: 'us-east-1',
    accessKeyId: 'AKIAW2CKO7KN3FWYRIGW',
    secretAccessKey: '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'
});

const schemaUser = Joi.object({
    username: Joi.string().required(),
    password: Joi.string().required(),
    name: Joi.string().required(),
    photo: Joi.string().optional()
})

router.post('/login', async (req, res) => {

    //const { error } = schemaUser.validate(req.body)
    //if (error) {
    //    return res.status(400).json({ error: 'Ups, algo salio mal' })
    //}

    const user = await User.findOne({ username: req.body.username })
    if (!user) {
        return res.status(400).json({ error: 'El usuario no existe ' })
    }

    const validatePassword = await bcrypt.compare(req.body.password, user.password)
    if (!validatePassword) {
        return res.status(400).json({ error: ' Contraseña invalida ' })
    }

    res.json({
        status: 200,
        data: user
    })

})

router.post('/register', async (req, res) => {
    const { error } = schemaUser.validate(req.body)
    if (error) {
        return res.status(400).json({ error: 'Ups, algo salio mal' })
    }

    const userExist = await User.findOne({ username: req.body.username })
    if (userExist) {
        return res.status(400).json({ error: 'El usuario ya existe ' })
    }

    const salt = await bcrypt.genSalt(10)
    const password = await bcrypt.hash(req.body.password, salt)

    const user = await new User({
        username: req.body.username,
        password: password,
        name: req.body.name
    });

    let fileName;

    if(req.body.photo && req.body.photo !== "") {
        // Crear buffer de la imagen enviada en base64
        const buffer = new Buffer.from(req.body.photo, 'base64');
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
        s3.putObject(params).promise().then(data => {
            // Nada
        }).catch(err => {
            return res.status(500).json({"message": "Se produjo un error al cargar la foto"});
        });

    }

    user.photo = 'https://ayd1-practica2.s3.amazonaws.com/' + fileName;

    try {
        const savedUser = await user.save()
        res.send({
            status: 200,
            data: savedUser.username + " fue registrado"
        })
    } catch (error) {
        res.status(400).send({ error })
    }

});


module.exports = router
