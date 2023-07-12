const { Schema, model } = require("mongoose");
const moment = require("moment");
moment.locale("es");

const ConversationSchema  = new Schema(
	{
		members: Array
	},
	{
        timestamps: true,
		versionKey: false,
	}
);

ConversationSchema .method("toJSON", function () {
	const { _id, updatedAt, ...object } = this.toObject();
	object.id = _id;
	object.createdAt = moment(object.createdAt).format("LLL");
	return object;
});

module.exports = model("Conversation", ConversationSchema , "conversations");
