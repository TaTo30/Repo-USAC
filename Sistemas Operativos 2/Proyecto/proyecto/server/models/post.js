var mongoose = require("mongoose");
var Schema = mongoose.Schema;

var postSchema = Schema({    
    date: {
        type: Date
    },
    text: {
        type: String                
    },
    image: {
        type: String
    }
});

module.exports = mongoose.model('Post', postSchema);