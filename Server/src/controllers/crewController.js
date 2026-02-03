const db = require('../db');

exports.createCrew = async (req, res) => {
    const { name, tag, userId } = req.body;
    const client = await db.pool.connect();

    try {
        await client.query('BEGIN');

        // Check if user is already in a crew (optional rule, enforcing for simplicity)
        const userCheck = await client.query('SELECT * FROM crew_members WHERE user_id = $1', [userId]);
        if (userCheck.rows.length > 0) {
            throw new Error('User is already in a crew');
        }

        // Create Crew
        const crewRes = await client.query(
            'INSERT INTO crews (name, tag, created_by) VALUES ($1, $2, $3) RETURNING id',
            [name, tag, userId]
        );
        const crewId = crewRes.rows[0].id;

        // Add User as Leader
        await client.query(
            'INSERT INTO crew_members (crew_id, user_id, role) VALUES ($1, $2, $3)',
            [crewId, userId, 'leader']
        );

        await client.query('COMMIT');
        res.status(201).json({ message: 'Crew created', crewId });
    } catch (err) {
        await client.query('ROLLBACK');
        console.error(err);
        res.status(400).json({ error: err.message });
    } finally {
        client.release();
    }
};

exports.joinCrew = async (req, res) => {
    const { crewId } = req.params;
    const { userId } = req.body;

    try {
        await db.query(
            'INSERT INTO crew_members (crew_id, user_id, role) VALUES ($1, $2, $3)',
            [crewId, userId, 'member']
        );
        res.json({ message: 'Joined crew successfully' });
    } catch (err) {
        console.error(err);
        res.status(400).json({ error: 'Could not join crew (already member or invalid crew)' });
    }
};

exports.getCrew = async (req, res) => {
    const { id } = req.params;

    try {
        const crewRes = await db.query('SELECT * FROM crews WHERE id = $1', [id]);
        if (crewRes.rows.length === 0) {
            return res.status(404).json({ error: 'Crew not found' });
        }

        const membersRes = await db.query(
            'SELECT u.username, cm.role FROM crew_members cm JOIN users u ON cm.user_id = u.id WHERE cm.crew_id = $1',
            [id]
        );

        res.json({
            crew: crewRes.rows[0],
            members: membersRes.rows
        });
    } catch (err) {
        console.error(err);
        res.status(500).json({ error: 'Internal server error' });
    }
};
