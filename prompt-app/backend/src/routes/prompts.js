const express = require('express');
const router = express.Router();
const getPrompts = require('../controllers/prompts/getPrompts');
const createPrompt = require('../controllers/prompts/createPrompt');
const updatePrompt = require('../controllers/prompts/updatePrompt');
const deletePrompt = require('../controllers/prompts/deletePrompt');

// Get all prompts for user, with optional search
router.get('/', getPrompts);

// Create prompt
router.post('/', createPrompt);

// Update prompt
router.put('/:id', updatePrompt);

// Delete prompt
router.delete('/:id', deletePrompt);

module.exports = router;