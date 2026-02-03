const express = require('express');
const router = express.Router();
const crewController = require('../controllers/crewController');

router.post('/', crewController.createCrew);
router.post('/:crewId/join', crewController.joinCrew);
router.get('/:id', crewController.getCrew);

module.exports = router;
