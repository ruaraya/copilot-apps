const db = require('../../db/db');

module.exports = async (req, res) => {
  const userId = req.user.userId;
  const { title, content } = req.body;
  const { id } = req.params;
  const result = await db.query(
    'UPDATE prompts SET title = $1, content = $2 WHERE id = $3 AND user_id = $4 RETURNING *',
    [title, content, id, userId]
  );
  if (result.rows.length === 0) return res.status(404).json({ error: 'Prompt not found' });
  res.json(result.rows[0]);
};
