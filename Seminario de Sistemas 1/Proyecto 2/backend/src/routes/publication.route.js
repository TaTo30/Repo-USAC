const { Router } = require("express");
const {
	publications,
	create,
	translate
} = require("../controllers/publication.controller");

const router = Router();

router.route("/:nickname").get(publications);
router.route("/").put(create);
router.route("/").post(translate);

module.exports = router;
