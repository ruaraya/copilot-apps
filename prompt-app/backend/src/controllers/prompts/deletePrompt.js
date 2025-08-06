const db = require('../../db/db');

module.exports = async (req, res) => {
  const userId = req.user.userId;
  const { id } = req.params;
  const result = await db.query(
    'DELETE FROM prompts WHERE id = $1 AND user_id = $2 RETURNING *',
    [id, userId]
  );
  if (result.rows.length === 0) return res.status(404).json({ error: 'Prompt not found' });
  res.json({ message: 'Prompt deleted' });
};
