const express = require("express");
const cors = require("cors");
const morgan = require("morgan");
const helmet = require("helmet");
const http = require("http")
const {Server} = require("socket.io")


// Config
require("dotenv").config();
const PORT = process.env.PORT;

// Database
require("./database/mongo");

// Routes
const authenticationRoutes = require("./routes/authentication.route");
const userRoutes = require("./routes/user.route");
const friendRoutes = require("./routes/friend.route");
const publicationRoutes = require("./routes/publication.route");

// Express App
const app = express();

// Sockets
const server = http.createServer(app)

const io = new Server(server, {
    cors: {
        origin: '*',
        methods: '*',
        allowedHeaders: '*'
    }
})

io.on('connection', (socket) => {
    console.log('Un usuario se a conectado');
    socket.on('disconnect', () => {
        console.log('Un usuario se desconecto');
    })
    socket.on('chat', (data) => {
        console.log(data);
        io.emit('chat', data)
    })
})

/**
 * MIDDLEWARES
 */
app.disable('etag')
app.use(cors({ origin: true, optionsSuccessStatus: 200 }));
app.use(helmet());
app.use(morgan("dev"));
app.use(express.json({ extended: true, limit: "100mb" }));
app.use(express.urlencoded({ extended: true }));

/**
 * ROUTES
 */
app.get("/", (req, res) => res.json({ message: "Backend funcionando" }));
app.use("/api/authentication", authenticationRoutes);
app.use("/api/user", userRoutes);
app.use("/api/friend", friendRoutes);
app.use("/api/publication", publicationRoutes);

//settings
server.listen(PORT)
//app.listen(PORT);
console.log("Server on port", PORT);
