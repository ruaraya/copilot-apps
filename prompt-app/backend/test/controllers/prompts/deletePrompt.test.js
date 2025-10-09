const deletePrompts = require('../../../src/controllers/prompts/deletePrompts');
const db = require('../../../src/db/db');

jest.mock('../../../src/db/db');

describe('deletePrompts controller', () => {
  let req, res;

  beforeEach(() => {
    req = {
      params: { id: 1 },
      user: { userId: 1 }
    };
    res = {
      sendStatus: jest.fn(),
    };
    db.query.mockClear();
  });

  it('should delete a prompt and return status 204', async () => {
    db.query.mockResolvedValue({ rowCount: 1 });

    await deletePrompts(req, res);

    expect(db.query).toHaveBeenCalledWith(
      'DELETE FROM prompts WHERE id = $1 AND user_id = $2',
      [1, 1]
    );
    expect(res.sendStatus).toHaveBeenCalledWith(204);
  });

  it('should throw if db.query fails', async () => {
    db.query.mockRejectedValue(new Error('DB error'));

    await expect(deletePrompts(req, res)).rejects.toThrow('DB error');
  });
});