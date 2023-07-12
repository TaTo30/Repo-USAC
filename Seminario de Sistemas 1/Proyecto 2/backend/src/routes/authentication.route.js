const { Router } = require("express");
const {
	login,
	recognition
} = require("../controllers/authentication.controller");

const router = Router();

//router.route("/:id").get(getReport);

//router.route("/").get(getReports);

router.route("/").post(login);
router.route("/recognition").post(recognition);

module.exports = router;
