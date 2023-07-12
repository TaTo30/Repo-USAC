
const BadRequest = (message) => {
    return {
        statusCode: 400,
        message: message
    }
}

const Ok = (data) => {
    return {
        statusCode: 200,
        data: data
    }
}

exports.BadRequest = BadRequest
exports.Ok = Ok