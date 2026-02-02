const request = require('supertest');
const app = require('../src/server');
const db = require('../src/db');

// Mock db.pool
jest.mock('../src/db', () => {
    const mPool = {
        connect: jest.fn(),
        query: jest.fn(),
    };
    return { pool: mPool, query: mPool.query };
});

describe('Auth API', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    it('POST /api/auth/register should create user', async () => {
        const client = {
            query: jest.fn(),
            release: jest.fn(),
        };
        db.pool.connect.mockResolvedValue(client);

        // Mock transaction steps
        client.query.mockResolvedValueOnce({}) // BEGIN
            .mockResolvedValueOnce({ rows: [] }) // Check user exists (empty)
            .mockResolvedValueOnce({ rows: [{ id: 'user-123' }] }) // Insert user
            .mockResolvedValueOnce({}) // Insert profile
            .mockResolvedValueOnce({}) // Insert inventory
            .mockResolvedValueOnce({}); // COMMIT

        const res = await request(app)
            .post('/api/auth/register')
            .send({
                username: 'testu',
                email: 'test@example.com',
                password: 'pass'
            });

        expect(res.statusCode).toEqual(201);
        expect(res.body).toHaveProperty('userId', 'user-123');
        expect(client.query).toHaveBeenCalledTimes(6); // BEGIN, Check, Insert User, Insert Profile, Insert Inventory, COMMIT 
    });

    it('POST /api/auth/login should return success on valid creds', async () => {
        db.pool.query.mockResolvedValueOnce({
            rows: [{ id: 'user-123', username: 'testu', password_hash: 'hashed_pass' }]
        });

        // Mock update last login
        db.pool.query.mockResolvedValueOnce({});

        const res = await request(app)
            .post('/api/auth/login')
            .send({ email: 'test@example.com', password: 'pass' });

        expect(res.statusCode).toEqual(200);
        expect(res.body.message).toBe('Login successful');
    });
});
