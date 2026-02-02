const request = require('supertest');
const app = require('../src/server');
const db = require('../src/db');

jest.mock('../src/db', () => {
    const mPool = {
        connect: jest.fn(),
        query: jest.fn(),
    };
    return { pool: mPool, query: mPool.query };
});

describe('Runs API', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it('POST /api/runs should record run and update stats', async () => {
        const client = {
            query: jest.fn(),
            release: jest.fn(),
        };
        db.pool.connect.mockResolvedValue(client);

        // Mock transaction steps
        client.query.mockResolvedValueOnce({}); // BEGIN
        client.query.mockResolvedValueOnce({ rows: [{ id: 'run-1' }] }); // INSERT
        client.query.mockResolvedValueOnce({}); // UPDATE
        client.query.mockResolvedValueOnce({}); // COMMIT

        const res = await request(app)
            .post('/api/runs')
            .send({
                userId: 'user-123',
                distance: 1000,
                score: 5000,
                coins: 100
            });

        expect(res.statusCode).toEqual(201);
        expect(res.body).toHaveProperty('runId', 'run-1');
        expect(client.query).toHaveBeenCalledTimes(4); // BEGIN, Insert Run, Update Profile, COMMIT
    });

    it('GET /api/runs/:userId should return history', async () => {
        const mockRuns = [{ id: 'run-1', score: 5000 }];
        db.pool.query.mockResolvedValueOnce({ rows: mockRuns });

        const res = await request(app).get('/api/runs/user-123');

        expect(res.statusCode).toEqual(200);
        expect(res.body).toEqual(mockRuns);
    });
});
