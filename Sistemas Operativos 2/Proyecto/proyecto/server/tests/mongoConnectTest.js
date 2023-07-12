const mongoose = require("mongoose");
const { MongoMemoryServer } = require("mongodb-memory-server");

let mongod;

const connect = async () => {
    mongod =  await MongoMemoryServer.create();
    await mongoose.connect(mongod.getUri(), {
        useNewUrlParser: true,
        useUnifiedTopology: true
    });
};

const close = async () => {
    await mongoose.connection.dropDatabase();
    await mongoose.connection.close();
    await mongod.stop();
};

const clear = async () => {
    const collections = mongoose.connection.collections;
    for (const key in collections) {
        await collections[key].deleteMany();
    }
};
module.exports = { connect, close, clear };