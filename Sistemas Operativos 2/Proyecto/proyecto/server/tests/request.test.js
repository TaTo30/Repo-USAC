const app = require('../app')
const request = require("supertest");
const db = require('./mongoConnectTest');

beforeAll(async () => await db.connect());
afterEach(async () => await db.clear());
afterAll(async () => await db.close());

describe('Test of Requests', () => {
    it('Create request', async () => {
        const myUser = {
            name: 'TEST',
            username: 'TEST',
            password: '1234'
        }

        const friendUser = {
            name: 'TEST FRIEND',
            username: 'FRIEND',
            password: '1234'
        }

        await request(app)
            .post('/user/register').send(myUser);

        await request(app)
            .post('/user/register').send(friendUser);

        const response = await request(app)
            .post(`/requests/send`).send({
                from: myUser.username,
                to: friendUser.username
            });

        expect(response.statusCode).toBe(200);
    });
    it('Reject request', async () => {
        const myUser = {
            name: 'TEST',
            username: 'TEST',
            password: '1234'
        }

        const friendUser = {
            name: 'TEST FRIEND',
            username: 'FRIEND',
            password: '1234'
        }

        await request(app)
            .post('/user/register').send(myUser);

        await request(app)
            .post('/user/register').send(friendUser);

        await request(app)
            .post(`/requests/send`).send({
                from: myUser.username,
                to: friendUser.username
            });

        const getRequest = await request(app)
            .get(`/requests/${friendUser.username}`);

        const response = await request(app)
            .post(`/requests/${getRequest.body.requests[0]._id}/reject`).send({});

        expect(response.statusCode).toBe(200);
    });
    it('Acept request', async () => {
        const myUser = {
            name: 'TEST',
            username: 'TEST',
            password: '1234'
        }

        const friendUser = {
            name: 'TEST FRIEND',
            username: 'FRIEND',
            password: '1234'
        }

        await request(app)
            .post('/user/register').send(myUser);

        await request(app)
            .post('/user/register').send(friendUser);

        const userFriend = await request(app)
            .post('/user/login').send(friendUser);

        await request(app)
            .post(`/requests/send`).send({
                from: myUser.username,
                to: friendUser.username
            });

        const getRequest = await request(app)
            .get(`/requests/${friendUser.username}`);

        const response = await request(app)
            .post(`/requests/${userFriend._body.data._id}/accept/${getRequest.body.requests[0]._id}`).send({});

        expect(response.statusCode).toBe(200);
    });
});

