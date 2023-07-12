const app = require('../app')
const request = require("supertest");
const db = require('./mongoConnectTest');


beforeAll(async () => await db.connect());
afterEach(async () => await db.clear());
afterAll(async () => await db.close());


describe('Test of Posts', () => {
    it('Register Post', async () => {

        const req = {
            name: 'TEST',
            username: 'TEST',
            password: '1234'
        }

        //CREAR USUARIO
        await request(app)
            .post('/user/register').send(req);


        const response = await request(app)
            .post(`/posts/${req.username}/create`)
            .send({
                text: 'Imagen de Prueba',
                image: "data:image/png;base64,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"
            });

        expect(response.statusCode).toBe(200);
    });

    it('Obtener Post por usuario', async () => {

        const req = {
            name: 'TEST',
            username: 'TEST',
            password: '1234'
        }

        //CREAR USUARIO
        await request(app)
            .post('/user/register').send(req);


        await request(app)
            .post(`/posts/${req.username}/create`)
            .send({
                text: 'Imagen de Prueba',
                image: "data:image/png;base64,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"
            });



        const response = await request(app)
            .get(`/posts/${req.username}`);;

        expect(response.statusCode).toBe(200);
    });

});