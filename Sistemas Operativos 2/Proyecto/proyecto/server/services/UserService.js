const { func } = require('@hapi/joi');
var userModel = require('../models/user');
const bcrypt = require('bcrypt')
const requestModel = require('../models/request');
const { request } = require('../app');
const AWS = require('aws-sdk');
const uuid = require('uuid');

AWS.config.update({
    region: 'us-east-1',
    accessKeyId: 'AKIAW2CKO7KN3FWYRIGW',
    secretAccessKey: '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'
});

module.exports.createUser = async (user) => {
    return userModel.create(user);
}

module.exports.getAllUsers = async (user) => {
    return userModel.find({});
}

module.exports.findUserById = async (id) => {
    return userModel.findById(id)
        .populate('requests')
        .populate('posts')
        .populate({ 
            path: 'friends',
            populate: {
                path: 'posts',
                model: 'Post',
                
            } 
        })
        .exec();
}

module.exports.findUserByUsername = async (condition) => {
    return userModel.findOne(condition);
}

module.exports.getUserPosts = async (userId) => {
    return userModel.findById(userId, { posts: 1 }).populate("posts").sort();
};

module.exports.patchUserById = async (req, res) => {
    var id = req.params.id
    userModel.findOne({ _id: id }, async (err, user) => {
        if (err) {
            return res.status(500).json({
                message: 'Se ha producido al actualizar el usuario',
                error: err
            })
        }
        if (!user) {
            return res.status(404).json({
                message: 'No hemos encontrado el usuario'
            })
        }


        const salt = await bcrypt.genSalt(10)
        const password = await bcrypt.hash(req.body.password, salt)

        user.name = req.body.name;
        user.username = req.body.username;
        user.password = password;
        let fileName;

        if (req.body.photo && req.body.photo !== "") {
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
                return res.status(500).json({ "message": "Se produjo un error al cargar la foto" });
            });

            user.photo = 'https://ayd1-practica2.s3.amazonaws.com/' + fileName;
        }




        user.save(function (err, user) {
            if (err) {
                return res.status(500).json({
                    message: 'Error al actualizar el usuario'
                })
            }
            if (!user) {
                return res.status(404).json({
                    message: 'No se ha encontrado el usuario'
                })
            }
            return res.status(200).json({
                status: 200,
                message: 'El usuario ' + req.body.name + ' ha sido modificado'
            })
        })

    })
}


module.exports.addFriends = async (idMyUser, friend) => {

    let idFriend = await userModel.findOne({ username: friend });

    userModel.updateOne({ _id: idMyUser }, {
        $push: { friends: idFriend }
    }, {
        new: true,
        useFindAndModify: false
    }).exec();

    userModel.updateOne({ _id: idFriend }, {
        $push: { friends: idMyUser }
    }, {
        new: true,
        useFindAndModify: false
    }).exec();
}

module.exports.getNotFriends = async (id) => {
    notFriends = []
    await userModel.find().exec().then(async (allUsers) => {
        await userModel.findById(id).exec().then(async (myUser) => {

            for (let user of allUsers) {
                if (user.id != id) {
                    if (!myUser.friends.some(x => x._id.toString() == user.id)) {
                        notFriends.push(user)
                    }
                }

            }
        })
    })

    return notFriends;

}
