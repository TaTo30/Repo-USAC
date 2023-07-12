const express = require("express");
const router = express.Router();
const controlador = require("../Controller/user.controller")

router.get("/", controlador.index);

module.exports = router