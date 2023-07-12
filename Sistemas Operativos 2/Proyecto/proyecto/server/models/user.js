var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var userSchema = Schema({
    name: {
        type: String,
        required: [true, "El campo nombre es requerido"]
    },
    username: {
        type: String,
        unique: true,
        required: [true, 'EL campo username es requerido']
    },
    password: {
        type: String,
        required: [true, "Le contraseña es obligatoria"]
    },
    photo: {
        type: String
    },
    posts: [
        {
            type: mongoose.Schema.Types.ObjectId,
            ref: "Post"
        }
    ],
    requests: [
        {
            type: mongoose.Schema.Types.ObjectId,
            ref: "Request"
        }
    ],
    friends:[
        {
            type: mongoose.Schema.Types.ObjectId,
            ref: "User"
        }
    ]
});

module.exports = mongoose.model('User', userSchema);