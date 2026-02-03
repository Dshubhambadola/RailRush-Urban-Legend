const db = require('../db');

exports.verifyReceipt = async (req, res) => {
    const { receipt, productId, userId } = req.body;

    // In production, you would validate 'receipt' with Apple/Google servers.
    // Here, we mock it.

    console.log(`Verifying receipt for user ${userId}, product ${productId}`);

    try {
        if (!userId) throw new Error('User ID required');

        // Grant Request Item
        // Logic depends on what the product is.
        // If it's a "Coin Pack", add coins.
        // If it's "Remove Ads", add to inventory.

        if (productId === 'coins_1000') {
            await db.query('UPDATE player_profiles SET coins = coins + 1000 WHERE user_id = $1', [userId]);
        } else if (productId === 'no_ads') {
            await db.query(
                'INSERT INTO inventory (user_id, item_id, item_type) VALUES ($1, $2, $3) ON CONFLICT DO NOTHING',
                [userId, 'no_ads', 'feature']
            );
        }

        res.json({ success: true, message: 'Purchase verified' });
    } catch (err) {
        console.error(err);
        res.status(500).json({ error: 'Verification failed' });
    }
};
