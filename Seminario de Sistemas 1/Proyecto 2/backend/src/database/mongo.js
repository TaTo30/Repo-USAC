const mongoose = require("mongoose");

const MONGO_URI = process.env.MONGO_URI;
const MONGO_CONFIG = { useNewUrlParser: true, useUnifiedTopology: true };

mongoose
	.connect(MONGO_URI, MONGO_CONFIG)
	.then((db) => {
		console.log("DB is connected to:", db.connection.name);
	})
	.catch((error) => {
		console.log("error:", error);
		process.exit(1);
	});
