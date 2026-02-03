const db = require('../db');

exports.getGlobalLeaderboard = async (req, res) => {
    try {
        const query = `
            SELECT r.score, u.username, r.created_at
            FROM runs r
            JOIN users u ON r.user_id = u.id
            ORDER BY r.score DESC
            LIMIT 50
        `;
        const { rows } = await db.query(query);
        res.json(rows);
    } catch (err) {
        console.error(err);
        res.status(500).json({ error: 'Internal server error' });
    }
};
