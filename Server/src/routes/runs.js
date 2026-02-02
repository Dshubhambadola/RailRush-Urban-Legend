const express = require('express');
const router = express.Router();
const runController = require('../controllers/runController');

router.post('/', runController.submitRun);
router.get('/:userId', runController.getRuns);

module.exports = router;
