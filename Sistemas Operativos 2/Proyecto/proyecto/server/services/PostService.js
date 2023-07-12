const postModel = require('../models/post');
const userModel = require('../models/user');

module.exports.createPost = async (userId, post) => {
    return postModel.create(post)
    .then(data => {        
        return userModel.findByIdAndUpdate(userId, {
            $push: {posts: data._id}
        }, {
            new: true,
            useFindAndModify: false
        });
    });
};