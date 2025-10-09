const createPrompt = require('../../../src/controllers/prompts/createPrompt');
const db = require('../../../src/db/db');

jest.mock('../../../src/db/db');

describe('createPrompt controller', () => {
  let req, res;

  beforeEach(() => {
    req = {
      user: { userId: 1 },
      body: { title: 'Test Title', content: 'Test Content' }
    };
    res = {
      status: jest.fn().mockReturnThis(),
      json: jest.fn()
    };
    db.query.mockClear();
  });

  it('should insert a prompt and return it with status 201', async () => {
    const fakePrompt = {
      id: 1,
      user_id: 1,
      title: 'Test Title',
      content: 'Test Content'
    };
    db.query.mockResolvedValue({ rows: [fakePrompt] });

    await createPrompt(req, res);

    expect(db.query).toHaveBeenCalledWith(
      'INSERT INTO prompts (user_id, title, content) VALUES ($1, $2, $3) RETURNING *',
      [1, 'Test Title', 'Test Content']
    );
    expect(res.status).toHaveBeenCalledWith(201);
    expect(res.json).toHaveBeenCalledWith(fakePrompt);
  });

  it('should throw if db.query fails', async () => {
    db.query.mockRejectedValue(new Error('DB error'));

    await expect(createPrompt(req, res)).rejects.toThrow('DB error');
  });
});