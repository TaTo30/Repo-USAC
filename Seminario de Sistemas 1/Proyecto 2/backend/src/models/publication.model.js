const { Schema, model } = require("mongoose");
const moment = require("moment");
moment.locale("es");

const PublicationSchema = new Schema(
	{
		owner: String,
		text: String,
		image: String
	},
	{
        timestamps: true,
		versionKey: false,
	}
);

PublicationSchema.method("toJSON", function () {
	const { _id, updatedAt, ...object } = this.toObject();
	object.id = _id;
	//object.createdAt = moment(object.createdAt).format("LLL");
	return object;
});

module.exports = model("Publication", PublicationSchema, "publications");
