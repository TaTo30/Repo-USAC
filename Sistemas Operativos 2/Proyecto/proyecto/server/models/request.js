var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var requestSchema = Schema({
    from: {
        type: String
    },
    to: {
        type: String
    },
    status: {
        type: String
    }
});

module.exports = mongoose.model('Request', requestSchema);