const { Pool } = require('pg');

// Database connection configuration
// Uses environment variables for security
const pool = new Pool({
    user: process.env.DB_USER || 'postgres',
    host: process.env.DB_HOST || 'localhost',
    database: process.env.DB_NAME || 'railrush',
    password: process.env.DB_PASSWORD || 'password',
    port: process.env.DB_PORT ? parseInt(process.env.DB_PORT) : 5432,
});

module.exports = {
    query: (text, params) => pool.query(text, params),
    pool,
};
