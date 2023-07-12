const { Schema, model } = require("mongoose");
const moment = require("moment");
moment.locale("es");

const UserSchema = new Schema(
	{
		name: String,
		nickname: String,
		image: String,
		password:String,
		bot: Number,
		email: String,
	},
	{
        timestamps: true,
		versionKey: false,
	}
);

UserSchema.method("toJSON", function () {
	const { _id, updatedAt, ...object } = this.toObject();
	object.id = _id;
	//object.createdAt = moment(object.createdAt).format("LLL");
	return object;
});

module.exports = model("User", UserSchema, "users");
