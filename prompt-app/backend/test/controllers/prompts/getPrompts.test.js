const getPrompt = require('../../../src/controllers/prompts/getPrompts');
const db = require('../../../src/db/db');

jest.mock('../../../src/db/db');

describe('getPrompts controller', () => {
  let req, res;

  beforeEach(() => {
    req = {
      user: { userId: 1 },
      query: { q: 'search term' }
    };
    res = {
      status: jest.fn().mockReturnThis(),
      json: jest.fn()
    };
    db.query.mockClear();
  });

  it('should get a prompt and return it with status 200', async () => {
    const fakePrompt = {
      id: 1,
      user_id: 1,
      title: 'Title',
      content: 'Content'
    };
    db.query.mockResolvedValue({ rows: [fakePrompt] });

    await getPrompt(req, res);

    /*
    expect(db.query).toHaveBeenCalledWith(
      'SELECT * FROM prompts WHERE user_id = $1 ORDER BY created_at DESC',
      [1]
    );
    */
    expect(res.status).toHaveBeenCalledWith(200);
    expect(res.json).toHaveBeenCalledWith(fakePrompt);
  });

  it('should throw if db.query fails', async () => {
    db.query.mockRejectedValue(new Error('DB error'));

    await expect(getPrompt(req, res)).rejects.toThrow('DB error');
  });
});