const requestModel = require('../models/request');
const userModel = require('../models/user');

module.exports.sendRequest = async (username, request) => {

    return requestModel.create(request)
        .then(async (data) => {
            await userModel.updateOne({username: username}, {
                $push: { requests: data._id }
            }, {
                new: true,
                useFindAndModify: false
            });

            return userModel.updateOne({username: request.to}, {
                $push: { requests: data._id }
            }, {
                new: true,
                useFindAndModify: false
            });
        });
};

module.exports.getUserRequests = async (userId) => {

    return userModel.findById(userId, { requests: 1 }).populate("requests").sort();
};

module.exports.findRequestById = async (condition) => {
    return requestModel.findOne(condition);
}

module.exports.rejectRequest = async (requestId) => {
    return requestModel.findByIdAndUpdate(requestId, {
        $set: { status: 'rejected' }
    }, {
        new: true,
        useFindAndModify: false
    });
};

module.exports.acceptRequest = async (requestId) => {
    return requestModel.findByIdAndUpdate(requestId, {
        $set: { status: 'accepted' }
    }, {
        new: true,
        useFindAndModify: false
    });
};