const { Schema, model } = require("mongoose");
const moment = require("moment");
moment.locale("es");

const FriendSchema = new Schema(
	{
		send: String,
		reciver: String,
		status: Number
	},
	{
        timestamps: true,
		versionKey: false,
	}
);

FriendSchema.method("toJSON", function () {
	const { _id, updatedAt, ...object } = this.toObject();
	object.id = _id;
	object.createdAt = moment(object.createdAt).format("LLL");
	return object;
});

module.exports = model("Friend", FriendSchema, "friends");
