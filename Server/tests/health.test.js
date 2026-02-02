const request = require('supertest');
const app = require('../src/server');

describe('GET /health', () => {
    it('should return 200 OK and status ok', async () => {
        const res = await request(app).get('/health');
        expect(res.statusCode).toEqual(200);
        expect(res.body).toHaveProperty('status', 'ok');
    });
});
