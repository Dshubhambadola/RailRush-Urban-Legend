const db = require('../db');
const { v4: uuidv4 } = require('uuid');

// Mock hashing for prototype - REPLACE with bcrypt/argon2 in production
const hashPassword = (password) => `hashed_${password}`;

exports.register = async (req, res) => {
    const { username, email, password } = req.body;

    if (!username || !email || !password) {
        return res.status(400).json({ error: 'Missing required fields' });
    }

    const client = await db.pool.connect();
    try {
        await client.query('BEGIN');

        // Check if user exists
        const userCheck = await client.query('SELECT id FROM users WHERE email = $1 OR username = $2', [email, username]);
        if (userCheck.rows.length > 0) {
            await client.query('ROLLBACK');
            return res.status(409).json({ error: 'User already exists' });
        }

        // Create User
        const passwordHash = hashPassword(password);
        const userRes = await client.query(
            'INSERT INTO users (username, email, password_hash) VALUES ($1, $2, $3) RETURNING id',
            [username, email, passwordHash]
        );
        const userId = userRes.rows[0].id;

        // Create initial Profile
        await client.query(
            'INSERT INTO player_profiles (user_id) VALUES ($1)',
            [userId]
        );

        // Grant default character (Dash)
        await client.query(
            'INSERT INTO inventory (user_id, item_id, item_type) VALUES ($1, $2, $3)',
            [userId, 'char_dash', 'character']
        );

        await client.query('COMMIT');

        res.status(201).json({
            message: 'User registered successfully',
            userId: userId
        });

    } catch (err) {
        await client.query('ROLLBACK');
        console.error(err);
        res.status(500).json({ error: 'Internal server error' });
    } finally {
        client.release();
    }
};

exports.login = async (req, res) => {
    const { email, password } = req.body;

    if (!email || !password) {
        return res.status(400).json({ error: 'Missing credentials' });
    }

    try {
        const result = await db.query('SELECT id, password_hash, username FROM users WHERE email = $1', [email]);

        if (result.rows.length === 0) {
            return res.status(401).json({ error: 'Invalid credentials' });
        }

        const user = result.rows[0];

        // Mock password check
        if (user.password_hash !== hashPassword(password)) {
            return res.status(401).json({ error: 'Invalid credentials' });
        }

        // Update last login
        await db.query('UPDATE users SET last_login = NOW() WHERE id = $1', [user.id]);

        // Return basic session info (In prod, use JWT)
        res.json({
            message: 'Login successful',
            user: {
                id: user.id,
                username: user.username
            }
        });

    } catch (err) {
        console.error(err);
        res.status(500).json({ error: 'Internal server error' });
    }
};
