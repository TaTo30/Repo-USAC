const { Router } = require("express");
const {
	friends,
	friend,
	accept
} = require("../controllers/friend.controller");

const router = Router();

//router.route("/:id").get(getReport);

//router.route("/").get(getReports);
router.route("/:nickname").get(friends);
router.route("/").put(friend);
router.route("/").post(accept);

module.exports = router;
