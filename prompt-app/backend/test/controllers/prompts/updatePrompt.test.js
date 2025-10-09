const updatePrompt = require('../../../src/controllers/prompts/updatePrompt');
const db = require('../../../src/db/db');

jest.mock('../../../src/db/db');

describe('updatePrompt controller', () => {
  let req, res;

  beforeEach(() => {
    req = {
      params: { id: 1 },
      user: { userId: 1 },
      body: { title: 'Updated Title', content: 'Updated Content' }
    };
    res = {
      status: jest.fn().mockReturnThis(),
      json: jest.fn()
    };
    db.query.mockClear();
  });

  it('should update a prompt and return it with status 200', async () => {
    const fakePrompt = {
      id: 1,
      user_id: 1,
      title: 'Updated Title',
      content: 'Updated Content'
    };
    db.query.mockResolvedValue({ rows: [fakePrompt] });

    await updatePrompt(req, res);

    expect(db.query).toHaveBeenCalledWith(
      'UPDATE prompts SET title = $1, content = $2 WHERE id = $3 AND user_id = $4 RETURNING *',
      ['Updated Title', 'Updated Content', 1, 1]
    );
    expect(res.status).toHaveBeenCalledWith(200);
    expect(res.json).toHaveBeenCalledWith(fakePrompt);
  });

  it('should throw if db.query fails', async () => {
    db.query.mockRejectedValue(new Error('DB error'));

    await expect(updatePrompt(req, res)).rejects.toThrow('DB error');
  });
});