const db = require('../../db/db');

module.exports = async (req, res) => {
  const userId = req.user.userId;
  const search = req.query.q;
  let result;
  if (search) {
    result = await db.query(
      'SELECT * FROM prompts WHERE user_id = $1 AND (title ILIKE $2 OR content ILIKE $2) ORDER BY created_at DESC',
      [userId, `%${search}%`]
    );
  } else {
    result = await db.query(
      'SELECT * FROM prompts WHERE user_id = $1 ORDER BY created_at DESC',
      [userId]
    );
  }
  res.json(result.rows);
};
