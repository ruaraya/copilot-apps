const db = require('../../db/db');

module.exports = async (req, res) => {
  const userId = req.user.userId;
  const { title, content } = req.body;
  const result = await db.query(
    'INSERT INTO prompts (user_id, title, content) VALUES ($1, $2, $3) RETURNING *',
    [userId, title, content]
  );
  res.status(201).json(result.rows[0]);
};
