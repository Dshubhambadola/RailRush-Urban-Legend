const express = require('express');
const router = express.Router();
const iapController = require('../controllers/iapController');

router.post('/verify', iapController.verifyReceipt);

module.exports = router;
