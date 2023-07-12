
const app = require('../app')
const request = require("supertest");
const db = require('./mongoConnectTest');

beforeAll(async () => await db.connect());
afterEach(async () => await db.clear());
afterAll(async () => await db.close());

describe('Test of User', () => {
    it('Register User', async () => {
        const req = {
            name: 'TEST',
            username: 'TEST',
            password: '1234'
        }

        const response = await request(app)
            .post('/user/register').send(req);

        expect(response.statusCode).toBe(200);
    });

    it('Login', async () => {
        const req = {
            name: 'TEST',
            username: 'TEST',
            password: '1234'
        }

        //CREAR USUARIO PRIMERO
        await request(app)
            .post('/user/register').send(req);

        //LOGIN
        const response = await request(app)
            .post('/user/login').send(req);

        expect(response.statusCode).toBe(200);
    });

    it('Get all users', async () => {

        const response = await request(app)
            .get('/users');

        expect(response.statusCode).toBe(200);
    });

    it('Get profile', async () => {

        const req = {
            name: 'TEST',
            username: 'TEST',
            password: '1234'
        }

        //CREAR USUARIO
        await request(app)
            .post('/user/register').send(req);

        //LOGIN
        const user = await request(app)
            .post('/user/login').send(req);

        const response = await request(app)
            .get('/profile/' + user._body.data._id);

        expect(response.statusCode).toBe(200);
    });


});





