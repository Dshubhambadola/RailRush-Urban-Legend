const db = require('../db');

exports.submitRun = async (req, res) => {
    const { userId, distance, score, coins } = req.body;

    if (!userId || distance == null || score == null) {
        return res.status(400).json({ error: 'Missing run data' });
    }

    const client = await db.pool.connect();
    try {
        await client.query('BEGIN');

        // Record Run
        const runRes = await client.query(
            'INSERT INTO runs (user_id, distance_meters, score, coins_collected) VALUES ($1, $2, $3, $4) RETURNING id',
            [userId, distance, score, coins || 0]
        );

        // Update Player Stats (Cumulative)
        if (coins > 0) {
            await client.query(
                'UPDATE player_profiles SET coins = coins + $1, xp = xp + $2 WHERE user_id = $3',
                [coins, Math.floor(score / 10), userId] // Simple XP formula
            );
        }

        await client.query('COMMIT');

        res.status(201).json({
            message: 'Run recorded',
            runId: runRes.rows[0].id
        });

    } catch (err) {
        await client.query('ROLLBACK');
        console.error(err);
        res.status(500).json({ error: 'Internal server error' });
    } finally {
        client.release();
    }
};

exports.getRuns = async (req, res) => {
    const { userId } = req.params;

    if (!userId) {
        return res.status(400).json({ error: 'Missing User ID' });
    }

    try {
        const result = await db.query(
            'SELECT * FROM runs WHERE user_id = $1 ORDER BY started_at DESC LIMIT 50',
            [userId]
        );
        res.json(result.rows);
    } catch (err) {
        console.error(err);
        res.status(500).json({ error: 'Internal server error' });
    }
};
