const { Router } = require("express");
const {
	signup,
	edit,
	user
} = require("../controllers/user.controller");

const router = Router();

//router.route("/:id").get(getReport);

//router.route("/").get(getReports);
router.route("/:id").get(user);
router.route("/").put(signup);
router.route("/").post(edit);


module.exports = router;
